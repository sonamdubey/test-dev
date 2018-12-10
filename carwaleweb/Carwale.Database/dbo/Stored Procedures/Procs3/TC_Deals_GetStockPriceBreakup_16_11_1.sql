IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_Deals_GetStockPriceBreakup_16_11_1]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_Deals_GetStockPriceBreakup_16_11_1]
GO

	
-- =============================================
-- Author:		Anchal Gupta
-- Create date: 22-08-2016
-- Description:	Get Price Breakup
-- Modified by : Anchal on 7/11/2016 Change parameter stockid type from int to varchar
-- Exec [TC_Deals_GetStockPriceBreakup_16_11_1] '', 1
-- =============================================
CREATE PROCEDURE [dbo].[TC_Deals_GetStockPriceBreakup_16_11_1] 
	-- Add the parameters for the stored procedure here
	 @StockId varchar(100)
	,@CityId int
AS
BEGIN
        select SPB.TC_Deals_PriceBreakupId,SPB.ExShowroom,SPB.RTO, SPB.Insurance, SPB.Accesories, SPB.Customer_Care, SPB.Incidental, SPB.Handling_Logistics, SPB.TCS, SPB.LBT, SPB.Depot, SPB.Other, SPB.Additional_Comments, SPB.Facilitation, SPB.Delivery, SPB.Service, SPB.Registration,
	        SP.ActualOnRoadPrice, SP.DiscountedPrice, SP.PriceBreakupId
	    from TC_Deals_StockPriceBreakup SPB  WITH(NOLOCK) right join TC_Deals_StockPrices SP WITH(NOLOCK) on SPB.TC_Deals_PriceBreakupId = SP.PriceBreakUpId 
	    where (SP.TC_Deals_StockId in (SELECT LISTMEMBER FROM fnSplitCSV(@StockId))) and SP.CityId = @CityId 
END

