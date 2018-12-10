IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[vwAdv_GetPriceBreakUp]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[vwAdv_GetPriceBreakUp]
GO

	-- =============================================
-- Author:		<Harshil Mehta>
-- Create date: <2th Sept., 2016>
-- Description:	<stored Procedure to get all details for a stock id>
-- =============================================
create PROCEDURE [dbo].[vwAdv_GetPriceBreakUp] 
	@priceBreakUpId INT
AS
BEGIN
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

