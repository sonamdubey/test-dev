IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[DailyAlertforNewVersion]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[DailyAlertforNewVersion]
GO

	
-- =============================================
-- Author:		Manish
-- Create date: 08-11-2013
-- Description:	Daily alert if new version created during the day.
-- Modified by: Manish on 25-03-2014 for New version addition alert as per Bharati Axa requirement.
-- =============================================
CREATE PROCEDURE [dbo].[DailyAlertforNewVersion]	
AS
	BEGIN
		/*SELECT V.MakeId,V.Make,V.ModelId,V.Model,V.VersionId,V.Version,CONVERT(VARCHAR,L.CreatedOn) CreatedOn
		FROM CarWaleMasterDataLogs AS L WITH (NOLOCK)
		JOIN vwMMV AS V ON L.AffectedId=V.VersionId
		WHERE Tablename='CARVERSIONS' 
		AND Remarks='Record Inserted'
		AND CONVERT(DATE,CreatedOn)>=CONVERT(DATE,GETDATE()-3)
		ORDER BY L.CreatedOn DESC*/


			SELECT V.VersionId AS VersionId,
		        V.Make AS CarMake,
				V.Model AS CarModel,
				V.Version AS CarVersion,
				Remarks,
				(SELECT TOP 1 Price FROM NewCarShowroomPrices AS NCSP WITH (NOLOCK) WHERE  NCSP.CarVersionId=V.VersionId AND CityId IN (10,2) ORDER BY CityId) AS [Ex-showroom Price],
				(SELECT     IV.ItemValue FROM CD.ItemValues  AS IV WITH (NOLOCK) WHERE IV.CarVersionId=V.VersionId AND   IV.ItemMasterId=14) AS [Cubic Capacity (CC)],
				(SELECT     UDF.NAME FROM CD.ItemValues  AS IV WITH (NOLOCK), CD.UserDefinedMaster UDF WITH (NOLOCK) WHERE  UDF.UserDefinedId = IV.UserDefinedId AND IV.CarVersionId=V.VersionId AND   IV.ItemMasterId=9) AS [Seating Capacity],
			    CFT.FuelType AS FuelType,
				CONVERT(VARCHAR,L.CreatedOn) CreatedOn
		FROM CarWaleMasterDataLogs AS L WITH (NOLOCK)
		JOIN vwMMV AS V ON L.AffectedId=V.VersionId
		JOIN CarFuelType AS CFT WITH (NOLOCK) ON CFT.FuelTypeId=V.CarFuelType 
		WHERE Tablename='CARVERSIONS' 
		--AND Remarks='Record Inserted'
		AND CONVERT(DATE,L.CreatedOn)>=CONVERT(DATE,GETDATE()-2)
		ORDER BY L.CreatedOn DESC

	END



	
