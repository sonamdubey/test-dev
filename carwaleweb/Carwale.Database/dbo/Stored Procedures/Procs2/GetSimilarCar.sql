IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[GetSimilarCar]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[GetSimilarCar]
GO

	
-- =============================================
-- Author:		Kumar Vikram
-- Create date: 03.03.2014
-- Description:	for Suggesting Similar Cars
-- exec [GetSimilarCar] 254
-- =============================================
CREATE PROCEDURE [dbo].[GetSimilarCar] @VersionId VARCHAR(100)
AS
BEGIN
	SET NOCOUNT ON;

	DECLARE @carVersionId VARCHAR(100)

	SELECT TOP 1 @carVersionId = SimilarVersions
	FROM SimilarCars WITH (NOLOCK)
	WHERE VersionId = @VersionId
		AND IsActive = 1
	ORDER BY UpdatedOn DESC

	SELECT TOP 1 Mk.NAME + ' ' + Mo.NAME + ' ' + Vs.NAME CarName,Mk.ID AS MakeId,Mo.ID AS ModelId,Vs.ID AS VersionId
	FROM CarVersions Vs WITH (NOLOCK)
	JOIN CarModels Mo WITH (NOLOCK) ON Vs.CarModelId = Mo.ID
	JOIN CarMakes Mk WITH (NOLOCK) ON Mo.CarMakeId = Mk.ID
	WHERE Vs.CarModelId = Mo.Id
		AND Mo.CarMakeId = Mk.Id
		AND Vs.ID IN (
			SELECT *
			FROM SplitTextRS(@carVersionId, ',')
			)
		AND Vs.New = 1
		AND Vs.IsDeleted = 0
		AND Vs.Futuristic = 0 -- Added by Raghu
	GROUP BY Mk.NAME + ' ' + Mo.NAME + ' ' + Vs.NAME,Mk.ID,Mo.ID,Vs.ID
END


