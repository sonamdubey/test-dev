IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_RetrivePwd_SP]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_RetrivePwd_SP]
GO

	--Modifed By: Tejashree Patil on 25 Jun 2014, Fetched mobile number for password recovery.
-- =============================================
CREATE PROCEDURE [dbo].[TC_RetrivePwd_SP]
	-- Add the parameters for the stored procedure here
	@Email VARCHAR(100)
AS
BEGIN
	DECLARE @IsSpecial BIT = 0

	SELECT @IsSpecial = ISUserSpecial
	FROM TC_vwAllUsers WITH(NOLOCK)
	WHERE Email = @Email
		AND IsActive = 1

	IF (@IsSpecial = 0)
	BEGIN
		SELECT 
		--Password
			--,
			UserName
			,PwdRecoveryEmail
			,Mobile
		FROM TC_Users WITH(NOLOCK)
		WHERE Email = @Email
			AND IsActive = 1
	END
	ELSE
	BEGIN
		SELECT 
		--Password,
			UserName
			,PwdRecoveryEmail
			,Mobile
		FROM TC_SpecialUsers WITH(NOLOCK)
		WHERE Email = @Email
			AND IsActive = 1
	END
END

