IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[ModerateApprove]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[ModerateApprove]
GO

	-- =============================================
-- Author:		<Ravi Koshal>
-- Create date: <6/27/2013>
-- Description:	<Accepts the selected post(s) to be approved and forumthreads table is updated accordingly>
-- =============================================
CREATE PROCEDURE [dbo].[ModerateApprove]
	@approveList [dbo].[ModerateList] READONLY
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
	DECLARE @URL VARCHAR(400)
	DECLARE @Title VARCHAR(400)
	DECLARE @Message VARCHAR(MAX)
	DECLARE @Replies numeric(18,0)
	DECLARE @Reads numeric(18,0)


	INSERT INTO @TempList (ThreadID , CustomerID , ForumID) SELECT ThreadID , CustomerID , ForumID FROM @approveList
	
	SELECT @RowCount = (SELECT COUNT(ID) FROM @TempList)
	
	WHILE(@Counter <= @RowCount)
	BEGIN
		SELECT @ThreadID = ThreadID , @CustomerID=CustomerID, @ForumID=ForumID FROM @TempList WHERE ID = @Counter
		INSERT INTO ModActionsLog (CustomerId,ThreadId,ForumId,ActionType,ActionDate) VALUES (@modId,@ThreadID,@ForumID,'Moderate-Approve',CURRENT_TIMESTAMP) -- Action Type Added by Ravi Koshal
		 UPDATE ForumThreads SET IsModerated = 1 WHERE ID = @ThreadID 
		 UPDATE Forums SET IsModerated = 1 WHERE ID = @ForumID 
		SET  @Counter = @Counter + 1
	END
END

