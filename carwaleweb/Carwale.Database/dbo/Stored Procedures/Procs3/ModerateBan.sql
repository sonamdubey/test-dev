IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[ModerateBan]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[ModerateBan]
GO

	-- =============================================
-- Author:		<Ravi Koshal>
-- Create date: <6/20/2013>
-- Description:	<Accepts the list of post(s) to be banned and forumthreads table is updated accordingly>
-- =============================================
CREATE PROCEDURE [dbo].[ModerateBan]
	@banList [dbo].[ModerateList] READONLY
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
	DECLARE @RowsPresent NUMERIC
	INSERT INTO @TempList (ThreadID , CustomerID , ForumID) SELECT ThreadID , CustomerID , ForumID FROM @banList
	
	SELECT @RowCount = (SELECT COUNT(ID) FROM @TempList)
	
	WHILE(@Counter <= @RowCount)
	BEGIN
		SELECT @ThreadID = ThreadID , @CustomerID=CustomerID, @ForumID=ForumID FROM @TempList WHERE ID = @Counter
		SELECT @RowsPresent = COUNT(CustomerId) FROM Forum_BannedList WHERE CustomerId = @CustomerID
		IF @RowsPresent = 0
	BEGIN
		INSERT INTO Forum_BannedList
		(CustomerId, BannedBy, BannedDate)
		VALUES
		(@CustomerId, 1864, GETDATE())
		INSERT INTO ModActionsLog (CustomerId,ThreadId,ForumId,ActionType,ActionDate) VALUES (@modId,@ThreadID,@ForumID,'Moderate-Ban',CURRENT_TIMESTAMP) 
		Update Customers SET IsFake = 1 where Id = @CustomerID
		 UPDATE ForumThreads SET IsModerated = 0 WHERE ID = @ThreadID 
		 UPDATE Forums SET IsModerated = 0 WHERE ID = @ForumID AND CustomerId = @CustomerID
		
	END
		SET  @Counter = @Counter + 1
	END
END

