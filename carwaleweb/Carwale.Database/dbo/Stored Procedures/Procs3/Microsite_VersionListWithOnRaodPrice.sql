IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[Microsite_VersionListWithOnRaodPrice]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[Microsite_VersionListWithOnRaodPrice]
GO

	
CREATE PROCEDURE [dbo].[Microsite_VersionListWithOnRaodPrice]
@CarModelId NUMERIC(18,0),
@CityId NUMERIC(18,0)
AS 
--Author:Rakesh Yadav
--Date Created: 06 April 2015
--Desc: Get version list with there on road prices for perticular city

BEGIN
	SELECT DISTINCT CV.Model AS ModelName,CV.ModelId,CV.Version AS VersionName,CV.VersionId ,dbo.Microsite_Fun_GetOnRoadPrice(CV.VersionId,@CityId) 
	As OnRoadPrice,CF.Descr AS CarFuelType,CF.CarFuelTypeId, CT.Descr AS CarTransmission,CT.Id AS CarTransmissionTypeId
	FROM vwMMV AS CV INNER JOIN CarFuelTypes CF WITH(NOLOCK) ON CV.CarFuelType=CF.CarFuelTypeId
	INNER JOIN CarTransmission CT WITH(NOLOCK) ON CV.CarTransmission=CT.Id
	INNER JOIN CW_NewCarShowroomPrices NCS WITH(NOLOCK) ON NCS.CarVersionId=CV.VersionId
	WHERE CV.ModelId=@CarModelId AND NCS.CityId=@CityId

	SELECT Name AS CityName FROM Cities where ID=@CityId
END
