IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[GetMultiplePriceQuote]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[GetMultiplePriceQuote]
GO

	-- =============================================
-- Author:		Raghupathy
-- Create date: 3/06/2014
-- Description:	<Gets Details of SimilarVersions> 
-- =============================================
--EXEC [GetMultiplePriceQuote] '2186,1294',1

CREATE PROCEDURE [dbo].[GetMultiplePriceQuote] 
	-- Add the parameters for the stored procedure here
		@carVersionId VARCHAR(100),
		@CityId INT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	
	IF (@carVersionId IS NOT NULL)
	BEGIN
	SELECT 
		Mk.Name AS MakeName,Mo.Name AS ModelName,Vs.Name AS VersionName,Mo.MaskingName,
		Mk.Name +  ' ' + Mo.Name +' '+ Vs.Name CarName, Vs.ID VersionId,Mo.Id ModelId, 'http://'+ Vs.HostUrl +'/cars/'+ Vs.smallPic AS SmallPic
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
		 Mk.Name ,Mo.Name,Vs.Name, Mo.MaskingName,
		 Mk.Name +  ' ' + Mo.Name +' '+ Vs.Name, Vs.ID,Mo.Id ,'http://'+ Vs.HostUrl +'/cars/'+ Vs.smallPic
	 END
END



