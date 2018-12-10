IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[DailyAlertforNewModelWithAvlVersions]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[DailyAlertforNewModelWithAvlVersions]
GO

	
-- =============================================
-- Author:		Manish
-- Create date: 14-09-2015
-- Description:	Daily alert if new model launch with versions.
-- =============================================
CREATE PROCEDURE [dbo].[DailyAlertforNewModelWithAvlVersions]	
AS
	BEGIN
		

	SELECT  M.id                      AS CarMake
	       ,M.Name                    AS Make
		   ,CM.id                     AS ModelId
		   ,CM.Name                   AS ModelName
		   ,CONVERT(DATE,L.CreatedOn) AS ModelLaunchedOn
		   ,V.ID                      AS CarVersionId
		   ,V.Name                    AS VersionName
		   ,(SELECT TOP 1 Price FROM NewCarShowroomPrices AS NCSP WITH (NOLOCK) WHERE  NCSP.CarVersionId=V.Id AND CityId IN (10) ORDER BY CityId) AS [Ex-showroom Price]
		   ,(SELECT  TOP 1   IV.ItemValue FROM CD.ItemValues  AS IV WITH (NOLOCK) WHERE IV.CarVersionId=V.Id AND   IV.ItemMasterId=14) AS [Cubic Capacity (CC)]
		   ,(SELECT  TOP 1   UDF.NAME FROM CD.ItemValues  AS IV WITH (NOLOCK), CD.UserDefinedMaster UDF WITH (NOLOCK) WHERE  UDF.UserDefinedId = IV.UserDefinedId AND IV.CarVersionId=V.Id AND   IV.ItemMasterId=9) AS [Seating Capacity]
		   ,CFT.FuelType AS FuelType
		FROM CarWaleMasterDataLogs AS L WITH (NOLOCK)
		JOIN carModels as CM  WITH (NOLOCK) on CM.ID=L.AffectedId
		JOIN CarMakes as M  WITH (NOLOCK) on M.Id=CM.Carmakeid
		JOIN CarVersions AS V WITH (NOLOCK) ON V.CarModelId=CM.ID
		JOIN CarFuelType AS CFT WITH (NOLOCK) ON CFT.FuelTypeId=V.CarFuelType
		WHERE L.Tablename='CarModels' 
		AND   L.Remarks='Record Inserted'
		AND   CM.New=1
		AND   V.IsDeleted=0
		AND   V.New=1
 		AND   L.CreatedOn>=(GETDATE()-1)     --CONVERT(DATE,GETDATE()-1)
  UNION 
	SELECT  M.id                       as CarMake
	       ,M.Name                     as Make
		   ,CM.id                      as ModelId
		   ,CM.Name                    as ModelName
		   ,CONVERT(DATE,L.CreatedOn)  as ModelLaunchedOn
		   ,V.ID                      AS CarVersionId
		   ,V.Name                    AS VersionName
		   ,(SELECT  TOP 1 Price FROM NewCarShowroomPrices AS NCSP WITH (NOLOCK) WHERE  NCSP.CarVersionId=V.Id AND CityId IN (10) ORDER BY CityId) AS [Ex-showroom Price]
		   ,(SELECT  TOP 1    IV.ItemValue FROM CD.ItemValues  AS IV WITH (NOLOCK) WHERE IV.CarVersionId=V.Id AND   IV.ItemMasterId=14) AS [Cubic Capacity (CC)]
		   ,(SELECT  TOP 1   UDF.NAME FROM CD.ItemValues  AS IV WITH (NOLOCK), CD.UserDefinedMaster UDF WITH (NOLOCK) WHERE  UDF.UserDefinedId = IV.UserDefinedId AND IV.CarVersionId=V.Id AND   IV.ItemMasterId=9) AS [Seating Capacity]
		   ,CFT.FuelType AS FuelType
		FROM CarWaleMasterDataLogs AS L WITH (NOLOCK)
		JOIN carModels as CM  WITH (NOLOCK) on CM.ID=L.AffectedId
		JOIN CarMakes as M WITH (NOLOCK)  on M.Id=CM.Carmakeid
		JOIN CarVersions AS V WITH (NOLOCK) ON V.CarModelId=CM.ID
		JOIN CarFuelType AS CFT WITH (NOLOCK) ON CFT.FuelTypeId=V.CarFuelType
		WHERE L.Tablename='CarModels' 
		AND   L.Remarks='Record Updated'
		AND   L.ColumnName='NEW'
		AND   L.OldValue=0
		AND   L.NewValue=1
		AND   V.IsDeleted=0
		AND   V.New=1
		AND   L.CreatedOn>=(GETDATE()-1) --CONVERT(DATE,GETDATE()-1)
		ORDER BY ModelLaunchedOn DESC
	END
