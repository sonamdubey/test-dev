IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[ClientMissedCallVerification]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[ClientMissedCallVerification]
GO

	
-- =============================================
-- Author:		Shikhar Maheshwari
-- Create date: 11.03.2015
-- Description:	Update TransactionToken For particular client id Exec [ClientVerifiedResponse_ZipDial] 'a'
-- =============================================
CREATE PROCEDURE [dbo].[ClientMissedCallVerification] 
	@TransactionToken VARCHAR(50)
	,@ToCall VARCHAR(50) = NULL
	,@Email VARCHAR(50) = NULL
	,@Mobile VARCHAR(15) = NULL
AS 
BEGIN
	SELECT TOP 1
		@Email = email
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
		)
		VALUES 
		(
			@Email
			,@Mobile
		)
	END
END
 



/****** Object:  StoredProcedure [CD].[GetPQFeaturedVersionIDByVersionID_API]    Script Date: 3/16/2015 4:30:27 PM ******/
SET ANSI_NULLS ON
