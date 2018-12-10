IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[ClientMissedCallVerification_15]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[ClientMissedCallVerification_15]
GO

	

-- =============================================
-- Author:		Shikhar Maheshwari
-- Create date: 11.03.2015
-- Description:	Update TransactionToken For particular client id Exec [ClientVerifiedResponse_ZipDial] 'a'
--Modified by Aditi Dhaybar on 19-05-2015 to Insert SourceId into CV_MobileEmailPair.
-- =============================================
CREATE PROCEDURE [dbo].[ClientMissedCallVerification_15.5.2] 
	@TransactionToken VARCHAR(50)
	,@ToCall VARCHAR(50) = NULL
	,@Email VARCHAR(50) = NULL
	,@Mobile VARCHAR(15) = NULL
	,@Source SMALLINT = NULL    
AS 
BEGIN
	SELECT TOP 1
		@Email = email
	   ,@Source = Source
	FROM ZipDial_Transactions WITH (NOLOCK)
	WHERE MobileNumber = @Mobile
	ORDER BY CreatedTime DESC

	UPDATE ZipDial_Transactions
	SET IsVerified = 1
		,TransactionToken = @TransactionToken
		,ZipDialNumber = @ToCall
		,VerificationResponseTime = GETDATE()
	WHERE MobileNumber = @Mobile

	IF EXISTS (
			SELECT ID
			FROM ZipDial_Transactions WITH (NOLOCK)
			WHERE MobileNumber = @Mobile
			)    AND NOT EXISTS (SELECT 1 FROM CV_MobileEmailPair WITH (NOLOCK) WHERE EmailId = @Email AND MobileNo = @Mobile)
	BEGIN
		INSERT INTO CV_MobileEmailPair 
		(
			 EmailId
			,MobileNo
			,SourceId
		)
		VALUES 
		(
			@Email
		   ,@Mobile
		   ,@Source
		)
	END
END
 
