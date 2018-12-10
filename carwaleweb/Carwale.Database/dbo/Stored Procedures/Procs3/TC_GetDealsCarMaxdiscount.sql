IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_GetDealsCarMaxdiscount]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_GetDealsCarMaxdiscount]
GO
	-- =============================================

-- Author:		<Upendra Kumar>

-- Create date: <05/01/2016>

-- Description:	Get MAxDiscount And SKUCount For given city and modelid for deals

-- EXEC TC_GetDealsCarMaxdiscount 2,100002

-- =============================================

CREATE PROCEDURE [dbo].[TC_GetDealsCarMaxdiscount]

@ModelId	INT,

@CityId 	INT



AS

BEGIN

 -- DECLARE @CityIds VARCHAR(100)

 --  if(@CityId = 3000)

 --    BEGIN

	--  SET @CityIds = '1,40,13'		-- group of cities thane ,navi mumbai and mumbai

 --   END

 -- ELSE

 --  BEGIN

 --    SET @CityIds = @CityId

 --   END





 -- SELECT MAX(ActualOnroadPrice - DiscountedPrice) AS MaxDiscount ,COUNT(SKURowId) AS SKUCount

 -- FROM ( SELECT DISTINCT 1 SKURowId, TCDS.BranchId,TCDS.CarVersionId,TCDS.MakeYear,TCDS.VersionColorId,TCDSP.DiscountedPrice,TCDSP.ActualOnroadPrice 

	--	  FROM vwAllMMV MMV WITH(NOLOCK) 

	--	  INNER JOIN TC_Deals_Stock TCDS WITH(NOLOCK) ON MMV.VersionId = TCDS.CarVersionId 

	--	  INNER JOIN TC_Deals_StockPrices TCDSP WITH(NOLOCK) ON TCDS.Id = TCDSP.TC_Deals_StockId 

	--	  WHERE  TCDSP.CityId IN  (SELECT ListMember FROM fnSplitCSV(@CityIds))

	--			 AND MMV.ModelId = @ModelId	  

	--) AS tempTable



	

	SELECT 

		TDSP.CityId		

		,V.ModelId		

		,(TDSP.ActualOnroadPrice - TDSP.DiscountedPrice) AS SavingPrice	

	INTO #TempDealMaxDiscount

	FROM TC_Deals_Stock TDS WITH (NOLOCK)

	JOIN TC_Deals_StockPrices TDSP WITH (NOLOCK) ON TDS.Id = TDSP.TC_Deals_StockId

	JOIN TC_Deals_StockVIN TDSV WITH (NOLOCK) ON TDS.Id = TDSV.TC_Deals_StockId

		AND TDSV.[Status] = 2

	JOIN vwAllMMV V WITH (NOLOCK) ON TDS.CarVersionId = V.VersionId

		AND V.ApplicationId = 1

	JOIN vwAllVersionColors VC WITH (NOLOCK) ON TDS.VersionColorId = VC.VersionColorsId

		AND VC.ApplicationId = 1

	JOIN TC_Deals_Dealers DD WITH (NOLOCK) ON DD.DealerId = TDS.BranchId

		AND DD.IsDealerDealActive = 1

	JOIN Dealers D WITH (NOLOCK) ON D.Id = TDS.BranchId

	WHERE V.ModelId = @ModelId

		



	SELECT TOP 1 SavingPrice, CityId , ModelId

	FROM #TempDealMaxDiscount 

	WHERE

	(@CityId IS NULL OR CityId = @CityId)

	ORDER BY SavingPrice DESC 







END


