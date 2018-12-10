IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[ModdeactivatePost]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[ModdeactivatePost]
GO

	-- =============================================
-- Author:		<Ravi Koshal>
-- Create date: <7/03/2013>
-- Description:	<Accepts the selected post(s) to be deactivated and forumthreads table is updated accordingly>
-- =============================================
CREATE PROCEDURE [dbo].[ModdeactivatePost]
	@deactivateList [dbo].[ModerateList] READONLY
	,@modId nvarchar(max)
AS
BEGIN
	
	SET NOCOUNT ON;
	DECLARE @TempList TABLE
	(
	 ID INT IDENTITY,
	 ThreadID numeric (18,0),
	 CustomerID numeric (18,0),
	 ForumID numeric (18,0)
	)
	DECLARE @ThreadID numeric (18,0)
	DECLARE @CustomerID numeric (18,0)
	DECLARE @ForumID numeric (18,0)
	DECLARE @Counter INT = 1
	DECLARE @RowCount INT
	INSERT INTO @TempList (ThreadID , CustomerID , ForumID) SELECT ThreadID , CustomerID , ForumID FROM @deactivateList
	
	SELECT @RowCount = (SELECT COUNT(ID) FROM @TempList)
	
	WHILE(@Counter <= @RowCount)
	BEGIN
		SELECT @ThreadID = ThreadID , @CustomerID=CustomerID, @ForumID=ForumID FROM @TempList WHERE ID = @Counter
		INSERT INTO ModActionsLog (CustomerId,ThreadId,ForumId,ActionType,ActionDate) VALUES (@modId,@ThreadID,@ForumID,'Moderate-Deactivate',CURRENT_TIMESTAMP)-- action type added by Ravi Koshal 
		 UPDATE ForumThreads SET IsActive = 0  WHERE ID = @ThreadID 
		 UPDATE Forums SET IsActive = 1 WHERE ID = @ForumID 
		SET  @Counter = @Counter + 1
	END
END

