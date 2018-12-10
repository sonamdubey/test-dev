IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_Deals_GetInquiryDeatails]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_Deals_GetInquiryDeatails]
GO

	
-- =============================================
-- Author		: Nilima More
-- Created Date : 22nd Jan 2016.
-- Description  : To get Inquiry Details For source 134 and 140.
-- EXEC [TC_Deals_GetInquiryDeatails_V] 18100
-- Modified By : Nilima More On 1st March 2016(Added Condition For cityId,prices will be shown on the basis of city.)
-- Modified By : Nilima More On 14th April 2016,To fetch Source for inquiry.
-- Modified by : Nilima More On 13th May 2016,to fetch IsPaymentSuccess value(paid/unpaid).
-- =============================================
CREATE PROCEDURE [dbo].[TC_Deals_GetInquiryDeatails] 
@InqId INT = NULL
AS
BEGIN
  
  SELECT DISTINCT NCB.TC_Deals_StockId TC_Deals_StockId  ,NCB.TC_DealsStockVINId TC_Deals_StockVINId ,VWC.Color VersionColor,DSP.ActualOnroadPrice ActualOnroadPrice,DSP.DiscountedPrice DiscountedPrice,
  TIS.Source,
  CASE WHEN ISNULL(NCB.IsPaymentSuccess,0) = 0 THEN 'Unpaid'ELSE 'Paid' END PaymentType,IsPaymentSuccess -- Modified by : Nilima More On 13th May 2016,to fetch IsPaymentSuccess value(paid/unpaid).
  FROM TC_NewCarInquiries NCB WITH(NOLOCK) 
  INNER JOIN TC_Deals_StockPrices DSP WITH(NOLOCK) ON  DSP.TC_Deals_StockId = NCB.TC_Deals_StockId
  INNER JOIN TC_Deals_Stock DS  WITH(NOLOCK) ON DS.ID = NCB.TC_Deals_StockId
  INNER JOIN VersionColors VWC WITH(NOLOCK) ON VWC.ID = DS.VersionColorId
  INNER JOIN TC_InquirySource TIS WITH(NOLOCK) ON TIS.Id = NCB.TC_InquirySourceId
  WHERE NCB.TC_NewCarinquiriesid = @inqId  AND  DSP.CityId = NCB.CityId
 
END
---------------------------------------------------------------------------------------------------
