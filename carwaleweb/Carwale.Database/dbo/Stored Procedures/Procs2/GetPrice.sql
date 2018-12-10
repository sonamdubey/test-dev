IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[GetPrice]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[GetPrice]
GO
-- =============================================
-- Author:		<Anuj Dhar>
-- Create date: <03/08/2016>
-- Description:	<Description: Fetches pricequote based on modelid, cityid>
-- [GetPriceQuote] 552, 333
-- =============================================
CREATE PROCEDURE [dbo].[GetPrice]
	@ModelId INT
	,@CityId INT
AS
BEGIN
	CREATE TABLE #TempVersions (Id INT, Name VARCHAR(50), IsMetallic BIT, IsNew BIT)

	INSERT INTO #TempVersions (Id, Name, IsMetallic, IsNew)
	SELECT ID, Name, IsMetallic = 0, New FROM CarVersions WITH (NOLOCK) WHERE CarModelId = @ModelId AND IsDeleted = 0

	INSERT INTO #TempVersions (Id, Name, IsMetallic, IsNew)
	SELECT Id, Name, IsMetallic = 1, IsNew from #TempVersions
	
	SELECT DISTINCT TV.Id AS VersionId, TV.Name AS VersionName, TV.IsMetallic, TV.IsNew, DATEDIFF(dd, VPL.LastUpdated, GETDATE()) AS LastUpdated,
					NP.PQ_CategoryItem AS PQItemId, CI.CategoryName AS PQItemName, NP.PQ_CategoryItemValue AS PQItemValue
	FROM #TempVersions TV
		LEFT JOIN CW_NewCarShowroomPrices NP WITH (NOLOCK) on TV.Id = NP.CarVersionId AND NP.isMetallic = TV.IsMetallic AND CityId = @CityId
		LEFT JOIN PQ_CategoryItems CI WITH (NOLOCK) ON CI.Id = NP.PQ_CategoryItem
		LEFT JOIN VersionPricesUpdationLog VPL WITH (NOLOCK) ON VPL.VersionId = TV.Id AND VPL.CityId = @CityId AND VPL.IsMetallic = TV.IsMetallic
	ORDER BY VersionId, IsMetallic -- Do not change the order

	DROP TABLE #TempVersions
END
