IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[GetCustomerIdByAccessToken]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[GetCustomerIdByAccessToken]
GO

	
-- =============================================
-- Author:		amit vema
-- Create date: 12 june 2014
-- Description:	get customer id by access token
-- =============================================
CREATE PROCEDURE [dbo].[GetCustomerIdByAccessToken] 
	-- Add the parameters for the stored procedure here
	@AccessToken VARCHAR(MAX),
	@CustomerId NUMERIC(18,0) OUTPUT,
	@MinutesDiff VARCHAR(MAX) OUTPUT
AS
BEGIN
	SET NOCOUNT ON
	SET @CustomerId = -1
	SELECT @CustomerId = CustomerId, @MinutesDiff = DATEDIFF(MINUTE,StartDateTime,GETDATE()) FROM PwdResetAT WITH(NOLOCK)
	WHERE AccessToken = @AccessToken
	IF(@MinutesDiff <= 1440) SET @MinutesDiff = 1
END

