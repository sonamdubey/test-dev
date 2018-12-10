IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[vwAdv_GetOfferList]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[vwAdv_GetOfferList]
GO
-- =============================================
-- Author:		Saket Thapliyal
-- Create date: 16-11-2016
-- Description:	Fetching OfferList
-- EXEC vwAdv_GetOfferList 566, 1 
-- =============================================
CREATE PROCEDURE [dbo].[vwAdv_GetOfferList]
@StockId INT,
@CityId INT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	SELECT DO.OfferWorth,DO.AdditionalComments,DOC.Description
	FROM TC_Deals_Offers DO WITH (NOLOCK)
	INNER Join TC_Deals_OfferCategories DOC WITH (NOLOCK) ON DO.CategoryId = DOC.CategoryId AND DO.StockId = @StockId
	UNION  
	SELECT DSP.Insurance, null, 'Free Insurance' 
	FROM TC_Deals_StockPrices DSP  WITH (NOLOCK)
	WHERE DSP.TC_Deals_StockId = @StockId AND DSP.CityId = @CityId
END