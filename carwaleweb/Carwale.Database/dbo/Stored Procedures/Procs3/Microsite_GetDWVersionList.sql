IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[Microsite_GetDWVersionList]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[Microsite_GetDWVersionList]
GO

	-- =============================================  
--- Auhor: Rakesh Yadav on 08 Jun 2015
-- Desc: Fetch all version for dealer. This procedure will give list of all versions from TC_DealerVersions table along with 
-- OnRoad and ex-showroom price,car fueltype and transmission type
-- Modified By : sunil M. Yadav ON 27th Oct 2016, Get DM.CWModelId.
-- =============================================     

CREATE PROCEDURE [dbo].[Microsite_GetDWVersionList]
@ModelId NUMERIC(18,0),--ModelId is Id of TC_DealerModels table and not of CarModels table
@DealerId NUMERIC(18,0),
@CityId NUMERIC(18,0)
AS 
BEGIN
	SELECT DV.DWModelId,DV.ID ,VMM.Make AS MakeName,DM.DWModelName AS ModelName,DV.DWVersionName AS VersionName
	,CF.FuelType,CT.Descr AS CarTransmission,dbo.Microsite_Fun_GetOnRoadPrice(VMM.VersionId,@CityId) As OnRoadPrice
	,VMM.VersionId,NCS.PQ_CategoryItemValue AS ExShowroomPrice,DV.ID AS DWVersionId,C.ID AS CityId,C.Name AS CityName
	,DM.CWModelId
	FROM 
	TC_DealerVersions DV WITH(NOLOCK)
	JOIN TC_DealerModels DM WITH(NOLOCK) ON DV.DWModelId=DM.ID
	JOIN vwMicrositeMMV VMM WITH(NOLOCK) ON DV.CWVersionId=VMM.VersionId
	JOIN CarFuelType CF WITH(NOLOCK) ON VMM.CarFuelType=CF.FuelTypeId
	JOIN CarTransmission CT WITH(NOLOCK) ON VMM.CarTransmission=CT.Id
	JOIN CW_NewCarShowroomPrices NCS WITH(NOLOCK) ON NCS.CarVersionId=VMM.VersionId
	JOIN PQ_CategoryItems CI WITH (NOLOCK) ON NCS.PQ_CategoryItem=CI.Id AND CI.CategoryId=3	-- CI.CategoryId=3 for Ex-showroom price of that city
	JOIN Cities C WITH(NOLOCK) ON NCS.CityId=C.ID AND C.ID=@CityId
	WHERE DV.DWModelId=@ModelId AND DM.IsDeleted=0 AND DV.IsDeleted=0 and DV.DealerId=@DealerId
END



