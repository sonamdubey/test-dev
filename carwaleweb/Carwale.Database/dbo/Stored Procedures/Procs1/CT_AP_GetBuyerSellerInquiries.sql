IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[CT_AP_GetBuyerSellerInquiries]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[CT_AP_GetBuyerSellerInquiries]
GO

	-- =============================================
-- Author:		Vaibhav K
-- Create date: 25 Apr 2016
-- Description:	To get Car Trade Buyer & Seller inquiries to be pushed to CarTrade exchange system
-- Modifier: Vaibhav K 5 May 2016 changed the where condition to check if cteinquiryid is null
-- Modifier: Vaibhav K 19 May 2016 also fetched dealerId in sell car inquiries queries with alias as 'cw_dealer_id'
-- =============================================
CREATE PROCEDURE [dbo].[CT_AP_GetBuyerSellerInquiries]
	-- Add the parameters for the stored procedure here
	
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT TOP 10
	B.StockId	stockId, cd.CustomerName custName, cd.Mobile custMobile, cd.Email custEmail, 
	B.Comments custComments,B.Buytime buyingTime, B.TC_BuyerInquiriesId cwBuyInquiryId	
	FROM TC_CustomerDetails CD WITH (NOLOCK)
	INNER JOIN TC_Lead L WITH (NOLOCK) ON L.TC_CustomerId = CD.Id
	INNER JOIN TC_InquiriesLead IL WITH (NOLOCK) ON IL.TC_LeadId = L.TC_LeadId
	INNER JOIN TC_BuyerInquiries B WITH (NOLOCK) ON B.TC_InquiriesLeadId = Il.TC_InquiriesLeadId
	JOIN Dealers D WITH (NOLOCK) ON CD.BranchId = D.ID AND D.IsCarTrade = 1
	WHERE B.cteinquiryid is null -- isnull(B.cteinquiryid,0) = 0 -- Vaibhav K 5 May 2016
	ORDER BY B.TC_BuyerInquiriesId

	SELECT TOP 10
	cd.CustomerName customerName,cd.Mobile customerMobile,cd.Email customerEmail,cd.Location customerLocation,s.CarVersionId versionId,
	s.MakeYear makeYear,s.RegNo registrationNo,s.Kms kilometers,s.Price askingPrice,s.Colour color,s.Owners owners,s.RegistrationPlace registrationPlace,
	s.Insurance insurance,CAST(ISNULL(s.InsuranceExpiry,GETDATE()) AS DATE) insuranceExpiry,s.CityMileage carMileage,s.InteriorColor interiorColor,s.AdditionalFuel additionalFuel,s.CarDriven carDrivenIn,
	s.Accidental accidental,s.FloodAffected floodAffected,s.Features_SafetySecurity featuresSafetySecurity,s.Features_Comfort featuresComfort,s.Features_Others featuresOthers,
	s.Warranties warranties,s.Tax tax,s.Modifications modifications,s.Comments comments,s.ACCondition airConditioning,s.BrakesCondition brakesCondition,
	s.ElectricalsCondition carElectricals,s.EngineCondition carEngine,s.ExteriorCondition exteriorCondition,s.SeatsCondition seatCondition,
	s.SuspensionsCondition suspensions,s.TyresCondition tyresCondition,s.InteriorCondition interiorCondition,s.OverallCondition overAllCondition,
	'eagerness' eagerness,s.TC_InquirySourceId inquirySourceId,s.TC_SellerInquiriesId cwSellInquiryId,cd.Id cwCustomerId,
	D.ID cw_dealer_id
	FROM TC_CustomerDetails CD WITH (NOLOCK)
	INNER JOIN TC_Lead L WITH (NOLOCK) ON L.TC_CustomerId = CD.Id
	INNER JOIN TC_InquiriesLead IL WITH (NOLOCK) ON IL.TC_LeadId = L.TC_LeadId
	INNER JOIN TC_SellerInquiries S WITH (NOLOCK) ON S.TC_InquiriesLeadId = Il.TC_InquiriesLeadId
	JOIN Dealers D WITH (NOLOCK) ON CD.BranchId = D.ID AND D.IsCarTrade = 1
	WHERE S.cteinquiryid is null -- isnull(S.cteinquiryid,0) = 0 -- Vaibhav K 5 May 2016
	ORDER BY S.TC_SellerInquiriesId

END
