IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[CV_VerifyMobile_v16_10_3]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[CV_VerifyMobile_v16_10_3]
GO

	CREATE PROCEDURE [dbo].[CV_VerifyMobile_v16_10_3]
	@EmailId 		AS VARCHAR(100), 
	@MobileNo 		AS VARCHAR(50), 
	@CVID			AS VARCHAR(100), 
	@CWICode 		AS VARCHAR(50), 
	@CUICode 		AS VARCHAR(50), 
	@EntryDateTime	AS DATETIME,
	@IsMobileVer	AS BIT OUTPUT,
	@SourceId		AS TINYINT,
	@PlatformId		AS INT, --added by navead to pass inquiry id
	@NewCVID		AS NUMERIC OUTPUT 
AS
--Create By: Rakesh Yadav on 26 Aug 2016
--Check if mobile number is verified, if not save verification code and return rowId
BEGIN
	SELECT * FROM CV_MobileEmailPair WITH (NOLOCK)   
	WHERE EmailId = @EmailId AND MobileNo = @MobileNo
	IF @@ROWCOUNT <> 0
		BEGIN
			SET @IsMobileVer = 1
		END
	ELSE
		BEGIN
			SET @IsMobileVer = 0
		END
	SET @NewCVID = -1
	
	IF @IsMobileVer = 0 and @CVID = -1
		BEGIN
			EXEC [dbo].[CV_InsertPendingList_16.10.3] @CWICode, @CUICode,@EmailId, @MobileNo, @EntryDateTime, @SourceId, @PlatformId, @NewCVID OUTPUT	
		END
END

