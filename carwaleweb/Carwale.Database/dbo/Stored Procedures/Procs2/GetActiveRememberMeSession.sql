IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[GetActiveRememberMeSession]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[GetActiveRememberMeSession]
GO

	
-- =============================================
-- Author:		amit vema
-- Create date: 12 june 2014
-- Description:	get active remember me session details based on the inputs
-- =============================================
CREATE PROCEDURE [dbo].[GetActiveRememberMeSession] 
	-- Add the parameters for the stored procedure here
	@CustomerId [numeric](18, 0),
	@Identifier [varchar](24),
	@AccessToken [varchar](24),
	@IPAddress [varchar](20),
	@UserAgent [varchar](max)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT * FROM RememberMeSessions WITH(NOLOCK)
	WHERE CustomerId = @CustomerId
	AND Identifier = @Identifier
	AND AccessToken = @AccessToken
END

