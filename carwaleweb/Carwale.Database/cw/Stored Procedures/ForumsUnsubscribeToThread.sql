IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[cw].[ForumsUnsubscribeToThread]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [cw].[ForumsUnsubscribeToThread]
GO

	-- =============================================
-- Author:		<Reshma Shetty>
-- Create date: <01/08/2012>
-- Description:	<Returns the number of discussions,contributors and posts>
-- =============================================
CREATE procedure [cw].[ForumsUnsubscribeToThread] 
	-- Add the parameters for the stored procedure here
	@CustomerId INT ,
	@ForumThreadId INT 
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
    -- Insert statements for procedure here
DELETE FROM ForumSubscriptions
WHERE CustomerId = @CustomerId AND ForumThreadId = @ForumThreadId
END

