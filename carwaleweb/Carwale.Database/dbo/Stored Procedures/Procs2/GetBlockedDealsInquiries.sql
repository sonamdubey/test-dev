IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[GetBlockedDealsInquiries]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[GetBlockedDealsInquiries]
GO

	
-- =============================================
-- Author:		Shalini Nair
-- Create date: 06/01/2015
-- Description:	To get the deals inquiries which are in 
--				blocked stages(Blocked(online),Blocked(confirmed),Blocked(rejected),Blocked(cancelled)
--				,Booking cancelled ,Unavailable)
-- Modified By: Shalini Nair on 19/01/2015 checking calltype 3 and 4 instead of 1 and 2
--Modified By: Anchal gupta on 16/02/2016 checking calltype 1 and 2 instead of 3 and 4
--Modified By : Ashwini Dhamankar on June 24,2016 (added constraint of TC_InquirySourceId 147 and 148)
-- =============================================
CREATE PROCEDURE [dbo].[GetBlockedDealsInquiries]
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	-- Insert statements for procedure here
	SELECT CUST.CustomerName AS UserName
		,CUST.Mobile AS UserMobile
		,CUST.email AS UserEmail
		,CT.NAME AS UserCity
		,STKP.DiscountedPrice
		,STKP.ActualOnroadPrice
		,D.Organization AS DealerName
		,CMA.NAME AS MakeName
		,CM.NAME AS ModelName
		,CV.NAME AS VersionName
		,STK.InteriorColor
		,COL.Color
		,STK.Id AS StockId
		,nci.TC_DealsStockVINId AS VINNo
		,AC.LastCallComment AS Comment
		,Status = CASE 
			WHEN STKS.Id = 4 -- Blocked(online)
				THEN 'Request Pending'
			WHEN STKS.ID = 5 -- Blocked(confirmed)
				THEN 'Confirmed'
			WHEN STKS.Id in (10,12) -- Blocked(rejected) or Unavailable
				THEN 'Rejected'
			WHEN STKS.Id in(8,9) -- Blocked(cancelled) or Booking cancelled
				THEN 'Cancelled'
			END
		,AC.LastCallDate AS TimeOfLastAction
		,stk.Offers AS AssociatedOffer
		,stk.MakeYear AS ManufacturingMonthYear
		,Action = CASE WHEN AC.NextCallTo = 1 
					THEN 'Call Dealer'
				   WHEN AC.NextCallTo = 2
					THEN 'Call User'
				  END
		,D.ID AS DealerId
		,D.IsDealerActive 
		,0   IsDealerDeleted
		,D.ApplicationId
		,CT.ID AS CityId
		,TCL.TC_InquiriesLeadId AS [LeadId]
		,TCL.TC_CustomerId AS [CustomerId]
		,TCL.TC_UserId AS [UserId]

	FROM TC_NewCarInquiries NCI WITH (NOLOCK)
	JOIN TC_Deals_Stock STK WITH (NOLOCK) ON NCI.TC_Deals_StockId = STK.Id
	JOIN TC_Deals_StockPrices STKP WITH (NOLOCK) ON STKP.TC_Deals_StockId = STK.Id
		AND NCI.CityId = STKP.CityId
	JOIN TC_InquiriesLead TCL WITH (NOLOCK) ON TCL.TC_InquiriesLeadId = NCI.TC_InquiriesLeadId
	JOIN Dealers D WITH (NOLOCK) ON D.ID = TCL.BranchId
	JOIN TC_ActiveCalls AC WITH (NOLOCK) ON AC.TC_LeadId = TCL.TC_LeadId
	JOIN TC_CustomerDetails CUST WITH (NOLOCK) ON CUST.Id = TCL.TC_CustomerId
	JOIN TC_Deals_StockVIN VIN WITH (NOLOCK) ON NCI.TC_DealsStockVINId = VIN.TC_DealsStockVINId
	JOIN TC_Deals_StockStatus STKS WITH (NOLOCK) ON VIN.STATUS = STKS.Id
	JOIN CarVersions CV WITH (NOLOCK) ON CV.ID = STK.CarVersionId
	JOIN CarModels CM WITH (NOLOCK) ON CM.ID = CV.CarModelId
	JOIN CarMakes CMA WITH (NOLOCK) ON CMA.ID = CM.CarMakeId
	JOIN VersionColors COL WITH (NOLOCK) ON COL.ID = STK.VersionColorId
	JOIN Cities CT WITH (NOLOCK) ON CT.ID = NCI.CityId
	
	WHERE (STKS.Id IN (4,5,8,9,10,12)) -- Blocked(online),Blocked(confirmed),Blocked(rejected),Blocked(cancelled),Booking cancelled,Unavailable
		AND ((NCI.TC_InquirySourceId = 134) OR (NCI.TC_InquirySourceId IN(147,148) AND NCI.IsPaymentSuccess=1)) -- For Deals  -- Modified By : Ashwini Dhamankar
		AND D.TC_DealerTypeId in (2,3)  
		AND D.IsDealerActive = 1 
		AND AC.CallType IN (1,2)
	ORDER BY AC.LastCallDate DESC
END
