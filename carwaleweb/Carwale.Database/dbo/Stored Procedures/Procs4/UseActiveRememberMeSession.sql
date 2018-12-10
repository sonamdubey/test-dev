IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[UseActiveRememberMeSession]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[UseActiveRememberMeSession]
GO

	
-- =============================================
-- Author:		amit vema
-- Create date: 12 june 2014
-- Description:	update active remember me details based on the inputs and return a status output
-- =============================================
CREATE PROCEDURE [dbo].[UseActiveRememberMeSession]
	-- Add the parameters for the stored procedure here
	@CustomerId [numeric](18, 0),
	@Identifier [varchar](30),
	@AccessToken [varchar](30),
	@NewAccessToken [varchar](30),
	@IPAddress [varchar](40),
	@UserAgent [varchar](max),
	@ReturnVal VARCHAR(2) OUTPUT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	SET @ReturnVal = 'N' --NO
	IF EXISTS (SELECT CustomerId FROM RememberMeSessions WITH(NOLOCK) WHERE CustomerId = @CustomerId AND Identifier = @Identifier AND AccessToken = @AccessToken AND IsActive = 1)
	BEGIN
		UPDATE RememberMeSessions
		SET AccessToken = @NewAccessToken,DateUpdated = GETDATE(),SessionCount = SessionCount + 1,UserAgent = @UserAgent,IPAddress = @IPAddress
		WHERE CustomerId = @CustomerId AND Identifier = @Identifier AND AccessToken = @AccessToken AND IsActive = 1
		IF (@@ROWCOUNT = 1)
			SET @ReturnVal = 'Y' --YES
	END
	ELSE IF EXISTS (SELECT CustomerId FROM RememberMeSessions WITH(NOLOCK) WHERE CustomerId = @CustomerId AND Identifier = @Identifier AND AccessToken != @AccessToken AND IsActive = 1)
	BEGIN
		UPDATE RememberMeSessions
		SET IsActive = 0, IsHacked = 1, DateUpdated = GETDATE(), IPAddress = @IPAddress, UserAgent = @UserAgent
		WHERE CustomerId = @CustomerId AND Identifier = @Identifier AND IsActive = 1
		IF (@@ROWCOUNT = 1)
			SET @ReturnVal = 'H' --HACKED
	END	
END

