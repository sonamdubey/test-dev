IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_Insurance_SaveInquiry_V2]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_Insurance_SaveInquiry_V2]
GO

	-- =============================================
-- Author		:	Tejashree Patil
-- Date			:	25 Aug 2016
-- Modified By	:	Save Insurance inquiry
-- Modified By  :   Khushaboo Patil on 30th Aug 2016 added parameter @PaymentCompletedDate
-- Modified By	:	Ashwini Dhamankar on Sep 9,2016 (saved and updated IDV,NCB,Discount,Premuim and CoverNote )
-- Modified By  :   Khushaboo Patil on 12th Sept 2016 added parameter @ChequeNumber,@PaymentConfirmationId,@PaymentMode,@RenewalConfirmedDate
-- Modified By  :   Nilima More On 13th sept 2016,added iszerodep,PaymentFailedDate parameter to save.
-- Modified By  :   Khushaboo Patil on 14th Sept 2016 added parameter @ChequePickUpAddress,@CollectionDateTime,@PaymentMethod and removed ChequePickUpDate,PayAtShowroomDate updatation
-- =============================================
CREATE PROCEDURE [dbo].[TC_Insurance_SaveInquiry_V2.0]
	 @BranchId INT
	,@INQLeadId INT = NULL
	,@TC_LeadDispositionId INT = NULL
	,@RegistrationNumber VARCHAR(20) = NULL
	,@VersionId	INT
	,@InsuranceProvider VARCHAR(50)= NULL
	,@PolicyNumber VARCHAR(25) = NULL
	,@ExpiryDate DATETIME = NULL
	,@IsClaimsExist BIT = 0
	,@ModifiedBy INT = NULL
	,@PaymentCompletedDate DATETIME= NULL
	,@ChequePickupDate DATETIME = NULL
	,@PayAtShowroomDate DATETIME = NULL
	,@IDV INT = NULL
	,@NCB FLOAT = NULL
	,@Discount FLOAT = NULL
	,@Premium INT = NULL
	,@CoverNote VARCHAR(25) = NULL
	,@PaymentMode TINYINT = NULL
	,@PaymentConfirmationId VARCHAR(100) = NULL
	,@ChequeNumber VARCHAR(20) = NULL
	,@HypothecationId TINYINT = NULL
	,@IsZeroDep BIT = 0
	,@PaymentMethod TINYINT = NULL
	,@ChequePickUpAddress VARCHAR(200) = NULL
	,@CollectionDateTime DATETIME = NULL
	,@PaymentFailedDate DATETIME = NULL
	,@InsuranceInquiryId INT = NULL OUTPUT
AS
BEGIN
	IF @InsuranceInquiryId > 0
		BEGIN
			UPDATE	TC_Insurance_Inquiries 
			SET		TC_InquiriesLeadId		= ISNULL(@INQLeadId,TC_InquiriesLeadId)
					,TC_LeadDispositionId	= ISNULL(@TC_LeadDispositionId,TC_LeadDispositionId)
					,RegistrationNumber		= ISNULL(@RegistrationNumber,RegistrationNumber)
					,VersionId				= ISNULL(@VersionId,VersionId)
					,ModifiedDate			= GETDATE()
					,ModifiedBy				= ISNULL(@ModifiedBy,ModifiedBy)
					,InsuranceProvider		= ISNULL(@InsuranceProvider,InsuranceProvider)
					,PolicyNumber			= ISNULL(@PolicyNumber,PolicyNumber)
					,ExpiryDate				= ISNULL(@ExpiryDate,ExpiryDate)
					,IsClaimsExist			= ISNULL(@IsClaimsExist,IsClaimsExist)
					,PaymentCompletedDate	= ISNULL(@PaymentCompletedDate,PaymentCompletedDate)
					,IDV					= ISNULL(@IDV,IDV)
					,NCB					= ISNULL(@NCB,NCB)
					,Discount				= ISNULL(@Discount,Discount)
					,Premium				= ISNULL(@Premium,Premium)
					,CoverNote				= ISNULL(@CoverNote,CoverNote)
					,PaymentMode			= ISNULL(@PaymentMode,PaymentMode)
					,PaymentConfirmationId	= ISNULL(@PaymentConfirmationId,PaymentConfirmationId)
					,ChequeNumber			= ISNULL(@ChequeNumber,ChequeNumber)
					,HypothecationId		= ISNULL(@HypothecationId,HypothecationId)
					,IsZeroDep				= ISNULL(@IsZeroDep,IsZeroDep)						-----------added by Nilima More On 13th sept 2016.
					,PaymentFailedDate		= ISNULL(@PaymentFailedDate,PaymentFailedDate)
					,PaymentMethod			= ISNULL(@PaymentMethod,PaymentMethod)
					,ChequePickUpAddress	= ISNULL(@ChequePickUpAddress,ChequePickUpAddress)
					,CollectionDateTime		= ISNULL(@CollectionDateTime,CollectionDateTime)
			WHERE	TC_Insurance_InquiriesId = @InsuranceInquiryId 
		
		END
		ELSE
		BEGIN
			-- SAVE IN TC_Insurance_Inquiries
			INSERT INTO TC_Insurance_Inquiries(	TC_InquiriesLeadId,TC_LeadDispositionId,RegistrationNumber,VersionId, EntryDate,
												InsuranceProvider,PolicyNumber,ExpiryDate,IsClaimsExist,BranchId,ModifiedBy,ModifiedDate,IDV,NCB,Discount,Premium,HypothecationId)
			VALUES(	@INQLeadId, @TC_LeadDispositionId, @RegistrationNumber, @VersionId, GETDATE(), 
					@InsuranceProvider, @PolicyNumber, @ExpiryDate, @IsClaimsExist, @BranchId,@ModifiedBy,GETDATE(),@IDV,@NCB,@Discount,@Premium,@HypothecationId)
			
			SET @InsuranceInquiryId = SCOPE_IDENTITY();
		END
END


