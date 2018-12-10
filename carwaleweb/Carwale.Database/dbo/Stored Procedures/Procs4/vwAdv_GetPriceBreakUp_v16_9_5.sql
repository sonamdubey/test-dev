IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[vwAdv_GetPriceBreakUp_v16_9_5]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[vwAdv_GetPriceBreakUp_v16_9_5]
GO

	

-- =============================================
-- Author:		<Harshil Mehta>
-- Create date: <7th March, 2016>
-- Description:	<stored Procedure to get all details for a stock id>
-- Modifed : Harshil on 15 Sept. , 2016
-- =============================================
CREATE PROCEDURE [dbo].[vwAdv_GetPriceBreakUp_v16_9_5] 
	@StockId INT,
	@CityId INT
AS
BEGIN
 Declare @priceBreakUpId int;
 select @priceBreakUpId = PriceBreakupId from TC_Deals_StockPrices  WITH(NOLOCK) where TC_Deals_StockId =@stockId and CityId  = @cityId; -- modified to get breakupId

	SELECT ExShowroom
	  ,RTO
	  ,Insurance
	  ,Accesories
	  ,Customer_Care as CustomerCare
	  ,Incidental
	  ,Handling_Logistics as HandlingLogistics
	  ,TCS
	  ,LBT
	  ,Depot
	  ,Other
	  ,Additional_Comments as AdditionalComments
	  ,Facilitation
	  ,Delivery
	  ,Service
	  ,Registration
	  FROM TC_Deals_StockPriceBreakup  WITH(NOLOCK) where TC_Deals_PriceBreakupId = @priceBreakUpId
END


