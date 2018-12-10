IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[CV_VerifyMobile_14]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[CV_VerifyMobile_14]
GO

	--Modified by Aditi Dhaybar 0n 13/10/14. 
CREATE PROCEDURE [dbo].[CV_VerifyMobile_14.10.1]
	@EmailId 		AS VARCHAR(100), 
	@MobileNo 		AS VARCHAR(50), 
	@CVID			AS VARCHAR(100), 
	@CWICode 		AS VARCHAR(50), 
	@CUICode 		AS VARCHAR(50), 
	@EntryDateTime	AS DATETIME,
	@SourceId		AS TINYINT,			--Modified by Aditi Dhaybar 0n 13/10/14.
	@IsMobileVer	AS BIT OUTPUT,
	@NewCVID		AS NUMERIC OUTPUT
	
AS

BEGIN

	SELECT * FROM CV_MobileEmailPair WITH (NOLOCK)         --Modified by Aditi Dhaybar 0n 13/10/14. Added WITH (NOLOCK) 
	WHERE EmailId = @EmailId AND MobileNo = @MobileNo

	IF @@ROWCOUNT <> 0
		BEGIN
			SET @IsMobileVer = 1
		END
	ELSE
		BEGIN
			SET @IsMobileVer = 0
		END

	IF @IsMobileVer = 0
		BEGIN
			IF @CVID = -1
				BEGIN
					EXEC [dbo].[CV_InsertPendingList_14.10.1] @CWICode, @CUICode, @EmailId, @MobileNo, @EntryDateTime, @SourceId, @NewCVID OUTPUT		--Modified by Aditi Dhaybar 0n 13/10/14. Added SourceId
				END
		END
		
END




