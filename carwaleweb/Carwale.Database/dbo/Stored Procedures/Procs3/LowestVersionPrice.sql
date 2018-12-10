IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[LowestVersionPrice]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[LowestVersionPrice]
GO

	
-- =============================================
-- Author:		Avishkar
-- Create date: 12-03-2015
-- Description:	Alert if  version price updated which is lowest for the model
-- =============================================
CREATE PROCEDURE [dbo].[LowestVersionPrice]	
AS
	BEGIN
		


			SELECT V.VersionId AS VersionId,
		        V.Make AS CarMake,
				V.Model AS CarModel,
				V.Version AS CarVersion,
				(SELECT TOP 1 Price FROM NewCarShowroomPrices AS NCSP WITH (NOLOCK) WHERE  NCSP.CarVersionId=V.VersionId AND CityId IN (10,2) ORDER BY CityId) AS [Ex-showroom Price],
				(SELECT     IV.ItemValue FROM CD.ItemValues  AS IV WITH (NOLOCK) WHERE IV.CarVersionId=V.VersionId AND   IV.ItemMasterId=14) AS [Cubic Capacity (CC)],
				(SELECT     UDF.NAME FROM CD.ItemValues  AS IV WITH (NOLOCK), CD.UserDefinedMaster UDF WITH (NOLOCK) WHERE  UDF.UserDefinedId = IV.UserDefinedId AND IV.CarVersionId=V.VersionId AND   IV.ItemMasterId=9) AS [Seating Capacity],
			    CFT.FuelType AS FuelType,
				CONVERT(VARCHAR,L.CreatedOn) CreatedOn
		INTO #TempVersion
		FROM CarWaleMasterDataLogs AS L WITH (NOLOCK)
		JOIN vwMMV AS V ON L.AffectedId=V.VersionId
		JOIN CarFuelType AS CFT WITH (NOLOCK) ON CFT.FuelTypeId=V.CarFuelType 
		WHERE Tablename='CARVERSIONS' 
		AND Remarks='Record Inserted'
		AND CONVERT(DATE,L.CreatedOn)>=CONVERT(DATE,GETDATE()-2)
		ORDER BY L.CreatedOn DESC

		select T.*
		from #TempVersion as T
		join CarVersions as CV on CV.ID=T.VersionId
		join CarModels as CM on CM.ID=CV.CarModelId and T.[Ex-showroom Price]<=CM.MinPrice

		
	END



	
