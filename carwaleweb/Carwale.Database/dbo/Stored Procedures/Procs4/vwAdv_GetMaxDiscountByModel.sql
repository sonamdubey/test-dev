IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[vwAdv_GetMaxDiscountByModel]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[vwAdv_GetMaxDiscountByModel]
GO

	-- =============================================
-- Author:		<Sourav>
-- Create date: <03/03/2016>
-- Description:	Get MAxDiscount And SKUCount For given city and modelid for deals
-- Description : To Get model data based on RootModelId
-- exec [dbo].[vwAdv_GetMaxDiscountByModel] 552,1
-- Modified by Purohith Guguloth on 20th july, 2016
--		Added a column in the select clause called Offers 
-- =============================================
CREATE PROCEDURE [dbo].[vwAdv_GetMaxDiscountByModel] @ModelId INT
	,@CityId INT
AS
BEGIN

DECLARE @RootModelId INT

	SELECT @RootModelId = RootId
	FROM CarModels WITH (NOLOCK)
	WHERE ID = @ModelId 

	SELECT Top 1 VWLD.CityId
	    ,VWLD.StockCount as CarCount
		,VWLD.ModelId
		,VWLD.VersionId
		,VWLD.Savings AS SavingPrice
		,VWLD.MaskingName
		,VWLD.Model AS ModelName
		,VWLD.Offers,					-- Modified by Purohith Guguloth on 20th july, 2016
		CASE WHEN @ModelId = VWLD.ModelId THEN 1 ELSE 0 END AS ModelOrder
	FROM vwLiveDeals VWLD WITH (NOLOCK)
	WHERE VWLD.RootId = @RootModelId and (
			@CityId IS NULL
			OR CityId = @CityId
			)	
	ORDER BY ModelOrder DESC, SavingPrice DESC

END
