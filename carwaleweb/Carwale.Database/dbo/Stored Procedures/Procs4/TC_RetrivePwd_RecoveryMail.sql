IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_RetrivePwd_RecoveryMail]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_RetrivePwd_RecoveryMail]
GO

	-- =============================================  
--Author:   Vivek Singh on 18-02-2014 
--Description: Return Password Recovery Email of the User
--Modified By: Tejashree Patil on 25 Jun 2014, Fetched mobile numebr for password recovery mail.
-- Modified By : Suresh Prajapati on 11th Mar, 2016
-- Description : Removed Password Check
-- =============================================
CREATE PROCEDURE [dbo].[TC_RetrivePwd_RecoveryMail]
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
		SELECT Email
			--,Password
			,UserName
			,ISNULL(PwdRecoveryEmail, Email) AS PwdEmail
			,Mobile
		FROM TC_Users WITH (NOLOCK)
		WHERE Email = @Email
			AND IsActive = 1
	END
	ELSE
	BEGIN
		SELECT Email
			----,Password
			,UserName
			,ISNULL(PwdRecoveryEmail, Email) AS PwdEmail
			,Mobile
		FROM TC_SpecialUsers WITH (NOLOCK)
		WHERE Email = @Email
			AND IsActive = 1
	END
END


