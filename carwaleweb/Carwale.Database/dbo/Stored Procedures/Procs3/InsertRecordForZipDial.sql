IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[InsertRecordForZipDial]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[InsertRecordForZipDial]
GO

	
-- =============================================
-- Author:		Akansha
-- Create date: 19.06.2013
-- Description:	Record For ZipDial Service Exec InsertRecordForZipDial '9967821412', 'DC-D4554-','D45554',2
-- Modified By: Akansha increased the varchar size of clientId and ReturnedClientId
-- Modified By: Amit, added source column 10/31/2013
-- =============================================
CREATE PROCEDURE [dbo].[InsertRecordForZipDial] @MobileNumber VARCHAR(15)

	,@Email VARCHAR(50)
	,@ClientId VARCHAR(50)
	,@InquiryId VARCHAR(10)
	,@SellerType TINYINT
	,@Source SMALLINT = NULL
	,@ReturnedClientId VARCHAR(50) = NULL OUTPUT
AS
BEGIN
	INSERT INTO ZipDial_Transactions (
		ClientTransactionId
		,MobileNumber
		,InquiryId
		,SellerType
		,CreatedTime
		,Email
		,Source -- Modified By: Amit, added source column 10/31/2013
		)
	VALUES (
		@ClientId
		,@MobileNumber
		,@InquiryId
		,@SellerType
		,GetDate()
		,@Email
		,@Source  --Modified By: Amit, added source column 10/31/2013
		)

	DECLARE @ID VARCHAR(10)

	SET @ID = SCOPE_IDENTITY()

	UPDATE ZipDial_Transactions
	SET ClientTransactionId = @ClientId + @ID
	WHERE ID = @ID

	SELECT @ReturnedClientId = ClientTransactionId
	FROM ZipDial_Transactions
	WHERE id = @ID

	SELECT @ReturnedClientId
END


