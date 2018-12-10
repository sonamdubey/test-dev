IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_Deals_GetDealsDetails]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_Deals_GetDealsDetails]
GO

	
-- =============================================
-- Author		: Nilima More
-- Created Date : 9th Jan 2016
-- Description  : To get Deals Details.5 == Blocked Online(Confirmed)
-- EXEC TC_Deals_GetDealsDetails 14337,1
-- =============================================
CREATE PROCEDURE [dbo].[TC_Deals_GetDealsDetails] 
--@VinId INT,
@Inqid INT, 
@dealsFlag INT 
AS
BEGIN
	
	DECLARE @TC_Deals_StockId INT= NULL,@inqLeadId INT;

	SELECT @inqLeadId = NCI.TC_InquiriesLeadId
	FROM TC_NewCarInquiries NCI  WITH(NOLOCK)
	WHERE NCI.TC_NewCarInquiriesId = @Inqid

	--SELECT @TC_Deals_StockId '@TC_Deals_StockId'
	--SELECT @inqLeadId '@inqLeadId'
	--SELECT @dealsFlag '@dealsFlag'


	IF (@dealsFlag = 3) --Approve Cancellation
	BEGIN

		SELECT DISTINCT CD.CustomerName CustomerName ,CD.Email Email ,cd.Mobile AS CustNumber,VW.Car Car
		FROM TC_NewCarInquiries NCI WITH(NOLOCK)
			INNER JOIN TC_InquiriesLead IL  WITH(NOLOCK) ON IL.TC_InquiriesLeadId = NCI.TC_InquiriesLeadId
			INNER JOIN TC_CustomerDetails CD  WITH(NOLOCK) ON CD.Id = IL.TC_CustomerId
			INNER JOIN vwAllMMV VW WITH(NOLOCK) ON  VW.VersionId = NCI.VersionId  
		WHERE NCI.TC_InquiriesLeadId =  @inqLeadId

	END
	ELSE 
	BEGIN
	SELECT DISTINCT VW.Make  Make,VW.Model Model,VW.Version Version,D.FirstName FirstName,D.LastName LastName,D.MobileNo MobileNo,D.Address1 Address,
	VW.Car Car,DS.MakeYear MakeYear,
	DSP.ActualOnroadPrice ActualOnroadPrice,DSP.DiscountedPrice DiscountedPrice,
	DS.Offers Offers ,CD.CustomerName CustomerName,cd.Email Email,cd.Mobile AS CustNumber,vwc.VersionColor Color, C.Name City, *--,DI.ID TransactionReferenceId, *
	FROM TC_NewCarInquiries NCI WITH(NOLOCK)
		INNER JOIN TC_Deals_StockVIN SV WITH(NOLOCK) ON NCI.TC_DealsStockVINId = SV.TC_DealsStockVINId
		INNER JOIN TC_Deals_Stock DS WITH(NOLOCK) ON DS.Id = SV.TC_Deals_StockId
		INNER JOIN vwAllMMV VW WITH(NOLOCK) ON  VW.VersionId = NCI.VersionId AND VW.ApplicationId = 1
		INNER JOIN Dealers D WITH(NOLOCK) ON D.ID = DS.BranchId
		INNER JOIN TC_InquiriesLead IL  WITH(NOLOCK) ON IL.TC_InquiriesLeadId = NCI.TC_InquiriesLeadId
		INNER JOIN TC_CustomerDetails CD  WITH(NOLOCK) ON CD.Id = IL.TC_CustomerId 
		INNER JOIN vwAllVersionColors VWC  WITH(NOLOCK) ON VWC.VersionId = DS.CarVersionId AND VWC.ApplicationId = 1
		INNER JOIN TC_Deals_StockPrices DSP WITH(NOLOCK) ON DSP.TC_Deals_StockId = SV.TC_Deals_StockId 
		INNER JOIN Cities C WITH(NOLOCK) ON C.ID = DSP.CityId
		--INNER JOIN DealInquiries DI WITH(NOLOCK) ON DI.PushStatus= NCI.TC_NewCarInquiriesId
	--INNER JOIN PGTransactions PT WITH(NOLOCK) ON PT.CarId = DI.ID
	WHERE NCI.TC_NewCarInquiriesId = @Inqid	--OR SV.TC_Deals_StockId = @TC_Deals_StockId
	ORDER BY SV.EnteredOn DESC
	END
	 
END