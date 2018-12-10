IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[CV_CheckVerification]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[CV_CheckVerification]
GO

	
-- Modified By: Akansha 21.04.2014
-- Added SourceId Parameter
CREATE PROCEDURE [dbo].[CV_CheckVerification] @MobileNo AS VARCHAR(50)
	,@CWICode AS VARCHAR(50)
	,@CUICode AS VARCHAR(50)
	,@IsVerified AS BIT OUTPUT
	,@SourceId AS TINYINT = 0
AS
DECLARE @Email AS VARCHAR(200)

BEGIN
	SET @IsVerified = 0
	SET @Email = ''

	IF @CWICode <> ''
	BEGIN
		SELECT @Email = Email
		FROM CV_PendingList
		WHERE Mobile = @MobileNo
			AND CWICode = @CWICode
	END
	ELSE
	BEGIN
		SELECT @Email = Email
		FROM CV_PendingList
		WHERE Mobile = @MobileNo
			AND CUICode = @CUICode
	END

	IF @Email <> ''
	BEGIN
		--the lead is verified 
		--Check whether this entry is already in the CV_MobileEmailPair. If not then insert the data
		SELECT EmailId
			,MobileNo
		FROM CV_MobileEmailPair
		WHERE EmailId = @Email
			AND MobileNo = @MobileNo

		IF @@ROWCOUNT = 0
		BEGIN
			INSERT INTO CV_MobileEmailPair (
				EmailId
				,MobileNo
				,SourceId
				)
			VALUES (
				@Email
				,@MobileNo
				,@SourceId
				)
		END

		--also make this customer as verified
		UPDATE Customers
		SET IsVerified = 1
			,IsFake = 0
		WHERE Email = @Email

		SET @IsVerified = 1
	END
	ELSE
	BEGIN
		--the code provided is wrong
		SET @IsVerified = 0
	END
END


