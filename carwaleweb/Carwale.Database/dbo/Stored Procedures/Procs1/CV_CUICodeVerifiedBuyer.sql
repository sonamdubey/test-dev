IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[CV_CUICodeVerifiedBuyer]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[CV_CUICodeVerifiedBuyer]
GO

	-- =============================================
-- Author:		Ashish G. Kamble
-- Create date: 27/2/2012 12.15 am.
-- Description:	Procedure will verify buyer with mobile number and cuiCode entered. return true if buyer is
--				verified or return false. 
--				Get email from CV_PendingList on the basis of mobile number and cuiCode and 
--				check from CV_MobileEmailPair with email and mobile number buyer entered.
-- Parameters : Takes mobile number of buyer and cuiCode shown to buyer as input parameters. Return IsVerified as true or false
-- =============================================
CREATE PROCEDURE [dbo].[CV_CUICodeVerifiedBuyer]
	-- Add the parameters for the stored procedure here
	@MobileNo 		AS VARCHAR(50), 
	@CUICode 		AS VARCHAR(50), 
	@IsVerified		AS BIT OUTPUT	
AS
	
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	
	-- initiallize email and isVerified variables	
	SET @IsVerified = 0
	
	-- check for cuiCode and mobile number
	-- ISNULL will check whether CUICode and MobileNo,Email is null or not. If null will convert it into ''
	IF ISNULL(@CUICode,'') <> '' AND ISNULL(@MobileNo,'') <> ''
		BEGIN
			DECLARE @Email AS VARCHAR(200)	
			
			-- select email id for the buyers mobile number and cuiCode
			SELECT @Email = Email FROM CV_PendingList WHERE Mobile = @MobileNo AND CUICode = @CUICode
			
			IF @Email IS NOT NULL
				BEGIN
					-- if email id exists for the selected email id and mobile number the buyer is verified return 1 else not verified return 0
					IF EXISTS (SELECT EmailId FROM CV_MobileEmailPair WHERE EmailId = @Email AND MobileNo = @MobileNo )
						SET @IsVerified = 1
				END
		END
END
