IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_Deals_GetCustomerDetails]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_Deals_GetCustomerDetails]
GO

	-- =============================================
-- Author:		Ruchira Patil
-- Create date: 18th feb 2016
-- Description:	To get the customer details of dropped off leads for a particular dealstock
-- EXEC TC_Deals_GetCustomerDetails 101
-- Modified By : Ruchira Patil on 14 April 2016 (to get the customer details of CW advantage Masking Number Inquiry)
-- Modified By : Ashwini Dhamankar on May 12,2016 (added 147 and 148 deals source condition)
-- Modified By : Ruchira Patil on 1st June,2016 (fetch customer mobile number when lead is pushed through opr(customer name is not available))
-- =============================================
CREATE PROCEDURE [dbo].[TC_Deals_GetCustomerDetails]
@TC_Deals_StockId INT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	SELECT DISTINCT C.Id CustomerId,
	                CASE WHEN C.CustomerName = '' THEN C.Mobile ELSE C.CustomerName END  CustomerName,-- Modified By : Ruchira Patil on 1st June,2016 (fetch customer mobile number when lead is pushed through opr(customer name is not available))
					IL.TC_LeadId AS LeadId,
					IL.TC_UserId LeadOwnerId,
					NCI.TC_NewCarInquiriesId InquiryId
					,NCI.TC_InquirySourceId
	FROM TC_NewCarInquiries NCI WITH (NOLOCK)
	INNER JOIN	TC_InquiriesLead IL WITH(NOLOCK) ON IL.TC_InquiriesLeadId = NCI.TC_InquiriesLeadId
	INNER JOIN	TC_CustomerDetails C WITH(NOLOCK) ON C.Id = IL.TC_CustomerId
	INNER JOIN	TC_Deals_Stock DS WITH(NOLOCK) ON DS.Id = NCI.TC_Deals_StockId
	WHERE (NCI.TC_InquirySourceId = 140 OR (NCI.TC_InquirySourceId = 146 AND NCI.TC_Deals_StockId = @TC_Deals_StockId) OR (NCI.TC_InquirySourceId IN (147,148) AND NCI.IsPaymentSuccess = 0))-- CW advantage Masking Number Inquiry
			AND NCI.TC_DealsStockVINId IS NULL--dropped off leads
			AND DS.Id = @TC_Deals_StockId
END
