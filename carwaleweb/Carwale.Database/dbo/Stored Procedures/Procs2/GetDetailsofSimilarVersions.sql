IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[GetDetailsofSimilarVersions]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[GetDetailsofSimilarVersions]
GO

	-- =============================================
-- Author:		Raghupathy
-- Create date: 3/12/2013
-- Description:	<Gets Details of SimilarVersions> 
-- Modified By : Raghu on <27/12/2013> Added Discontinued and future cars condition
-- Modified By : Raghu on <02/01/2014> Optimized the Query by using Joins between Carmakes and models and version tables
-- Modified By : Raghu on <17/02/2014> getting top 4 results and getting makename, modelname
-- Modified By : Akansha on 06.03.2014 Added Masking Name Column
-- =============================================
--EXEC GetDetailsofSimilarVersions 2281,10

CREATE PROCEDURE [dbo].[GetDetailsofSimilarVersions] 
	-- Add the parameters for the stored procedure here
		@VersionId INT,
		@CityId INT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	DECLARE @carVersionId varchar(100)
	SELECT TOP 1 @carVersionId =  SimilarVersions
	FROM SimilarCars WHERE VersionId = @VersionId AND IsActive = 1
	ORDER BY UpdatedOn desc

	--SELECT TOP 3 Mk.Name +  ' ' + Mo.Name +' '+ Vs.Name CarName, Vs.ID VersionId,Mo.Id ModelId, 'http://'+ Vs.HostUrl +'/cars/'+ Vs.LargePic AS LargePic,'http://'+ Vs.HostUrl +'/cars/'+ Vs.smallPic AS SmallPic
	--, SUM (CASE WHEN PQC.CategoryId = 1  OR (PQC.CategoryId = 2 AND IsNULL(PQLT.IsTaxOnTax,0) = 1) THEN  0 ELSE PQ_CategoryItemValue END)  OnRoadPrice
	-- FROM CarVersions Vs WITH(NOLOCK)
	-- LEFT JOIN CarModels Mo WITH(NOLOCK) ON Vs.CarModelId = Mo.ID
	-- LEFT JOIN CarMakes Mk WITH(NOLOCK) ON Mo.CarMakeId = Mk.ID
	-- INNER JOIN CW_NewCarShowroomPrices NCS WITH(NOLOCK) ON NCS.CarVersionId = Vs.ID AND NCS.CityId =@CityId
	-- INNER JOIN PQ_CategoryItems CI WITH(NOLOCK) ON CI.Id = NCS.PQ_CategoryItem
	-- INNER JOIN PQ_Category PQC WITH(NOLOCK) ON PQC.CategoryId = CI.CategoryId
	-- LEFT JOIN PriceQuote_LocalTax PQLT WITH(NOLOCK) ON CI.Id = PQLT.CategoryItemid AND PQLT.CityId = @CityId
	-- WHERE Vs.CarModelId = Mo.Id AND Mo.CarMakeId = Mk.Id AND Vs.ID in (select * from SplitTextRS(@carVersionId,',')) 
	-- AND Vs.New =1 AND Vs.IsDeleted = 0 AND Vs.Futuristic = 0 -- Added by Raghu
	-- GROUP BY Mk.Name +  ' ' + Mo.Name +' '+ Vs.Name, Vs.ID,Mo.Id ,'http://'+ Vs.HostUrl +'/cars/'+ Vs.LargePic,'http://'+ Vs.HostUrl +'/cars/'+ Vs.smallPic\


	SELECT TOP 4 Mk.Name AS MakeName,Mo.Name AS ModelName,Mo.MaskingName, Vs.Name AS VersionName, Mk.Name +  ' ' + Mo.Name +' '+ Vs.Name CarName, Vs.ID VersionId,Mo.Id ModelId, 'http://'+ Vs.HostUrl +'/cars/'+ Vs.LargePic AS LargePic,'http://'+ Vs.HostUrl +'/cars/'+ Vs.smallPic AS SmallPic
	, SUM (CASE WHEN PQC.CategoryId = 1  OR (PQC.CategoryId = 2 AND IsNULL(PQLT.IsTaxOnTax,0) = 1) THEN  0 ELSE PQ_CategoryItemValue END)  OnRoadPrice
	 FROM CarVersions Vs WITH(NOLOCK)
	 JOIN CarModels Mo WITH(NOLOCK) ON Vs.CarModelId = Mo.ID
	 JOIN CarMakes Mk WITH(NOLOCK) ON Mo.CarMakeId = Mk.ID
	 INNER JOIN CW_NewCarShowroomPrices NCS WITH(NOLOCK) ON NCS.CarVersionId = Vs.ID AND NCS.CityId =@CityId
	 INNER JOIN PQ_CategoryItems CI WITH(NOLOCK) ON CI.Id = NCS.PQ_CategoryItem
	 INNER JOIN PQ_Category PQC WITH(NOLOCK) ON PQC.CategoryId = CI.CategoryId
	 LEFT JOIN PriceQuote_LocalTax PQLT WITH(NOLOCK) ON CI.Id = PQLT.CategoryItemid AND PQLT.CityId = @CityId
	 WHERE Vs.CarModelId = Mo.Id AND Mo.CarMakeId = Mk.Id AND Vs.ID in (select * from SplitTextRS(@carVersionId,',')) 
	 AND Vs.New =1 AND Vs.IsDeleted = 0 AND Vs.Futuristic = 0 -- Added by Raghu
	 GROUP BY
	 Mk.Name ,Mo.Name,Mo.MaskingName,Vs.Name, Mk.Name +  ' ' + Mo.Name +' '+ Vs.Name, Vs.ID,Mo.Id ,'http://'+ Vs.HostUrl +'/cars/'+ Vs.LargePic,'http://'+ Vs.HostUrl +'/cars/'+ Vs.smallPic

END
