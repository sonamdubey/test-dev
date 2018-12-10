IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_Insurance_FetchInquiryDetails]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_Insurance_FetchInquiryDetails]
GO

	--===============================================
-- Author		: Tejashree Patil
-- Date			: 26 August 2016
-- Discription	: To fetch insurance inquiry details.
-- Modified By  : Nilima More On 31st Aug 2016,fetch CustomerMobile.
-- Modified By  : Khushaboo Patil On 9 Sept 2016,fetch Premium.
-- Modified By  : Ashwini Dhamankar On 13 Sept 2016,fetch IDV,NCB and Discount and CustomerId.
--===============================================
CREATE PROCEDURE [dbo].[TC_Insurance_FetchInquiryDetails] 
(
@InqLeadId INT
--,@InsuranceInquiryId INT
)
AS
BEGIN

	SELECT  TIS.TC_Insurance_InquiriesId InsuranceInquiryId,TIS.TC_LeadDispositionId,TIS.RegistrationNumber,
			TIS.VersionId,InsuranceProvider,PolicyNumber, ExpiryDate,IsClaimsExist,CD.Mobile AS CustomerMobile,
			TIS.Premium,TIS.IDV,TIS.NCB,TIS.Discount,CD.Id AS CustomerId,TIS.PaymentMode,TIS.CoverNote,TIS.PaymentMethod,
			TIS.CollectionDateTime,TIS.ChequePickUpAddress
	FROM	TC_Insurance_Inquiries  TIS WITH(NOLOCK)
			--INNER JOIN TC_TaskLists TSL  WITH(NOLOCK) ON TSL.TC_InquiriesLeadId= TIS.TC_InquiriesLeadId
			INNER JOIN TC_InquiriesLead TIL WITH(NOLOCK) ON TIS.TC_InquiriesLeadId = TIL.TC_InquiriesLeadId
			INNER JOIN TC_CustomerDetails CD  WITH(NOLOCK) ON CD.Id = TIL.TC_CustomerId
 	WHERE	TIS.TC_InquiriesLeadId = @InqLeadId

END

