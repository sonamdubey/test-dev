IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_Insurance_SavePaymentDetails]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_Insurance_SavePaymentDetails]
GO

	
-- =============================================
-- Author:		<Khushaboo Patil>
-- Create date: <12th Sept 2016>
-- Description:	<log payment details for insurance>
-- =============================================
CREATE PROCEDURE [dbo].[TC_Insurance_SavePaymentDetails] 
	@ChequeNumber VARCHAR(20) =  null,
	@PaymentConfirmationId VARCHAR(100) = null,
	@PaymentMode TINYINT,
	@InsuranceInquiryId INT,
	@CoverNoteNo VARCHAR(100) = null,
	@CollectionDateTime DATETIME = NULL,
	@ChequePickUpAddress VARCHAR(200) = NULL,
	@PaymentMethod TINYINT = NULL,
	@PaymentDate DATETIME = NULL,
	@UserId	INT = NULL,
	@PaymentLogId INT = null OUTPUT
AS
BEGIN
	INSERT INTO 
	TC_Insurance_PaymentLog (ChequeNumber,PaymentConfirmationId,PaymentDate,PaymentMode,TC_Insurance_InquiryId,CoverNoteNo,CollectionDateTime,ChequePickUpAddress,PaymentMethod,ModifiedBy,ModifiedOn)
	VALUES (@ChequeNumber,@PaymentConfirmationId,@PaymentDate,@PaymentMode,@InsuranceInquiryId,@CoverNoteNo,@CollectionDateTime,@ChequePickUpAddress,@PaymentMethod,@UserId,GETDATE())

	SET @PaymentLogId = SCOPE_IDENTITY()
END
 ------------------------------------------------------------------------------------------------------------------------------------------------------------------------

 
