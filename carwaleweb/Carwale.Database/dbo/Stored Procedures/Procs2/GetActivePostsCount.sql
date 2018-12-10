IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[GetActivePostsCount]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[GetActivePostsCount]
GO

	-- =============================================
-- Author:		<Ravi Koshal>
-- Create date: <6/18/2013>
-- Description:	<Gets the number of active posts for a user>
-- =============================================
CREATE PROCEDURE [dbo].[GetActivePostsCount]
	-- Add the parameters for the stored procedure here
	@CustomerId int,
	@ActivePostsCount int OUTPUT
AS
BEGIN
	SET NOCOUNT ON;
		SELECT @ActivePostsCount = Count(CustomerId)
			FROM ForumThreads WITH (NOLOCK) 
			WHERE IsModerated = 1 AND CustomerId = @CustomerId	
			
    
	
END

