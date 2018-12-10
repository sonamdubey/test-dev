IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_Deals_FetchDealsStockData]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_Deals_FetchDealsStockData]
GO

	-- =============================================
-- Author:		<Khushaboo Patil>
-- Create date: <6 th Jan 15>
-- Description:	<Get Deals Stock basic Data>
-- Modified By : Khushaboo Patil on 13/04/2016 fetch cityName
-- =============================================
CREATE PROCEDURE [dbo].[TC_Deals_FetchDealsStockData]
	@Deals_StockId	INT
AS
BEGIN
	SELECT DS.CarVersionId,DS.MakeYear,DS.VersionColorId,DS.InteriorColor,DS.Offers,DS.TermsConditions,DSP.CityId, CT.Name+'-' +ST.Name CityName,DSP.DiscountedPrice,DSP.ActualOnroadPrice,
	DSV.TC_DealsStockVINId,DSV.VINNo,ISNULL(DSV.Status,'')Status,isApproved
	FROM TC_Deals_Stock DS WITH(NOLOCK)
	LEFT JOIN TC_Deals_StockPrices DSP WITH(NOLOCK) ON DS.Id = DSP.TC_Deals_StockId
	LEFT JOIN  TC_Deals_StockVIN DSV WITH(NOLOCK) ON DS.Id = DSV.TC_Deals_StockId
	INNER JOIN Cities CT WITH(NOLOCK) ON CT.ID = DSP.CityId
	INNER JOIN States ST WITH(NOLOCK) ON ST.ID = CT.StateId
	WHERE DS.Id = @Deals_StockId
END
