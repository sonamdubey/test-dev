IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[cw].[ForumsChangeSubscriptionType]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [cw].[ForumsChangeSubscriptionType]
GO

	-- =============================================
-- Author:		<Reshma Shetty>
-- Create date: <01/08/2012>
-- Description:	<Returns the number of discussions,contributors and posts>
-- =============================================
CREATE procedure [cw].[ForumsChangeSubscriptionType] 
	-- Add the parameters for the stored procedure here
	@EmailSubscriptionId INT ,
	@CustomerId INT ,
	@ForumThreadId INT 
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
    -- Insert statements for procedure here
UPDATE ForumSubscriptions SET EmailSubscriptionId = @EmailSubscriptionId
WHERE CustomerId = @CustomerId AND ForumThreadId = @ForumThreadId
END

