IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[ModActionLog]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[ModActionLog]
GO

	-- =============================================
-- Author:		<Ravi Koshal>
-- Create date: <8/19/2013>
-- Description:	<Logs moderator action(s) details in the database>
-- =============================================
CREATE PROCEDURE [dbo].[ModActionLog]
	@customerid int,
	@threadid nvarchar(max),
	@forumid nvarchar(max), 
	@actiontype nvarchar(50)
	AS
BEGIN
	
	SET NOCOUNT ON;
	INSERT INTO ModActionsLog (CustomerId,ThreadId,ForumId,ActionType,ActionDate) VALUES (@customerid,@threadid,@forumid,@actiontype,CURRENT_TIMESTAMP)
	
END

