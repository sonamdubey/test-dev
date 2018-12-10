IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[Con_GetValuations]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[Con_GetValuations]
GO

	
-- =============================================
-- Author:		<Khushaboo Patil>
-- Create date: <15 jan 2015>
-- Description:	<Get valuations>
-- =============================================
CREATE PROCEDURE [dbo].[Con_GetValuations]
	@GuideId	INT,
	@CityId		INT,
	@MakeId		INT,
	@RootId		INT,
	@ModelId	INT
AS
BEGIN
	SELECT MMV.VersionId,MMV.Make,MMV.Model, MMV.Version,NCSP.Price,NCSP.RTO,CV.CarYear,CV.CarValue,
    CASE CVER.CarFuelType WHEN 1 THEN 'Petrol' WHEN 2 THEN 'Diesel' WHEN 3 THEN 'CNG' WHEN 4 THEN 'LPG' WHEN 5 THEN 'Electric' ELSE '' END AS FuelType,
    CASE CVER.CarTransmission WHEN 1 THEN 'AT' WHEN 2 THEN 'MT' ELSE '' END AS Transmission,
    CASE CVER.Imported WHEN 1 THEN 'YES' ELSE 'NO' END AS IsImported
    FROM vwMMV MMV
	INNER JOIN CarModels CM WITH(NOLOCK) ON CM.ID = MMV.ModelId
    LEFT JOIN CarValues CV ON CV.CarVersionId=MMV.VersionId AND CV.GuideId= @GuideId
    LEFT JOIN NewCarShowroomPrices NCSP ON MMV.VersionId=NCSP.CarVersionId AND NCSP.CityId = @CityId
    LEFT JOIN CarVersions CVER ON MMV.VersionId = CVER.Id
    WHERE MMV.MakeId = @MakeId
	AND CM.RootId =  CASE WHEN @RootId IS NULL THEN CM.RootId ELSE @RootId END 
	AND MMV.ModelId= CASE WHEN @ModelId IS NULL THEN MMV.ModelId ELSE @ModelId END
    ORDER BY MMV.Make,MMV.Model,MMV.Version
END

