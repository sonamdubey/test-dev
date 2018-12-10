IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[CV_VerifyMobile]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[CV_VerifyMobile]
GO

	CREATE PROCEDURE [dbo].[CV_VerifyMobile]
	@EmailId 		AS VARCHAR(100), 
	@MobileNo 		AS VARCHAR(50), 
	@CVID			AS VARCHAR(100), 
	@CWICode 		AS VARCHAR(50), 
	@CUICode 		AS VARCHAR(50), 
	@EntryDateTime	AS DATETIME,
	@IsMobileVer	AS BIT OUTPUT,
	@NewCVID		AS NUMERIC OUTPUT
	
AS

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

	IF @IsMobileVer = 0
		BEGIN
			IF @CVID = -1
				BEGIN
					EXEC CV_InsertPendingList @CWICode, @CUICode, @EmailId, @MobileNo, @EntryDateTime, @NewCVID OUTPUT
				END
		END
		
END

