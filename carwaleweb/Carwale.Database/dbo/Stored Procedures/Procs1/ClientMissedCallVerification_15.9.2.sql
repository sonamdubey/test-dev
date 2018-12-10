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
--modified by prachi phalak on 09/09/2015.
--modified by Purohith Guguloth on 14th sep, 2015 added condition of mobile in updation of ClassifiedLeads table
---- Modified by Prachi Phalak on 21/09/2015.added check of sellerType = '1' 
-- =============================================
CREATE PROCEDURE [dbo].[ClientMissedCallVerification_15.9.2] 
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
	WHERE 
		MobileNumber = @Mobile
	AND
		Email = @Email

	IF EXISTS (
			SELECT ID
			FROM ZipDial_Transactions WITH (NOLOCK)
			WHERE MobileNumber = @Mobile AND Email = @Email
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

		--modified by prachi phalak on 09/09/2015,
		--make entry of verified customers in ClassifiedLeads table 
		UPDATE ClassifiedLeads
		SET IsVerified = 1
		WHERE CustMobile = @Mobile AND CustEmail = @Email
		
		UPDATE ClassifiedLeads
		SET IsSentToAutoBiz = 1
		WHERE Id IN
		( SELECT TOP 1 Id
			FROM ClassifiedLeads WITH (NOLOCK) WHERE CustMobile = @Mobile AND CustEmail = @Email AND sellerType = '1'
			ORDER BY EntryDateTime DESC )
		
	END
END
