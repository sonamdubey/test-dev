IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[ClientVerifiedResponse_ZipDial]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[ClientVerifiedResponse_ZipDial]
GO

	
-- =============================================
-- Author:		Akansha
-- Create date: 19.06.2013
-- Description:	Update TokenResponseTime For particular client id Exec [ClientVerifiedResponse_ZipDial] 'a'
-- Modified By: Akansha - Added condition to check if mobile and email are not null
-- Modified By: Manish on 18-06-2014 for checking the mobile and email pair already exists in CV_MobileEmailPair table
-- =============================================
CREATE PROCEDURE [dbo].[ClientVerifiedResponse_ZipDial] @TransactionToken VARCHAR(50)
	,@Email VARCHAR(50) = NULL
	,@Mobile VARCHAR(15) = NULL
AS
BEGIN
	UPDATE ZipDial_Transactions
	SET IsVerified = 1
		,VerificationResponseTime = GETDATE()
	WHERE TransactionToken = @TransactionToken

	SELECT @Email = email
		,@Mobile = mobileNumber
	FROM ZipDial_Transactions WITH (NOLOCK)
	WHERE TransactionToken = @TransactionToken

	IF EXISTS (
			SELECT ID
			FROM ZipDial_Transactions WITH (NOLOCK)
			WHERE TransactionToken = @TransactionToken
			)    AND NOT EXISTS (SELECT 1 FROM CV_MobileEmailPair WITH (NOLOCK) WHERE EmailId = @Email AND MobileNo = @Mobile)
	BEGIN
		INSERT INTO CV_MobileEmailPair (
			EmailId
			,MobileNo
			)
		VALUES (
			@Email
			,@Mobile
			)
	END
END
 