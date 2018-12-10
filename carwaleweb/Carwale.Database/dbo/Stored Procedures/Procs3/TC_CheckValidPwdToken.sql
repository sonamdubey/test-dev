IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_CheckValidPwdToken]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_CheckValidPwdToken]
GO

	-- ======================================================
-- Author		: Suresh Prajapati
-- Created On	: 08th Mar, 2016
-- Description	: This procedure checks for active token
-- ======================================================
CREATE PROCEDURE [dbo].[TC_CheckValidPwdToken]
	-- Add the parameters for the stored procedure here
	@TC_UserId INT
	,@Token VARCHAR(200)
	,@IsValidToken BIT OUTPUT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	SET @IsValidToken = 0

	IF EXISTS (
			SELECT PRT.IsActiveToken
			FROM TC_UserPasswordRecoveryTokens AS PRT WITH (NOLOCK)
			WHERE PRT.IsActiveToken = 1
				AND DATEDIFF(HH, GETDATE(), PRT.ExpiryDate) > 0
				AND PRT.TC_UserId = @TC_UserId
				AND Token = @Token
			)
		SET @IsValidToken = 1

	SELECT @IsValidTOken
END


