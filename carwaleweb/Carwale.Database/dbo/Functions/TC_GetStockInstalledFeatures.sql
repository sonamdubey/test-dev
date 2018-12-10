IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_GetStockInstalledFeatures]') 
    AND xtype IN (N'FN', N'IF', N'TF')
)
    DROP FUNCTION [dbo].[TC_GetStockInstalledFeatures]
GO

	
------------------------------------------------------------------------------------------------------
-- Author : Kritika Choudhary on 9th March 2016, get installed features for a particular versionId
------------------------------------------------------------------------------------------------------
CREATE FUNCTION [dbo].[TC_GetStockInstalledFeatures]
	(@VersionId INT,
	@StockId INT)
RETURNS VARCHAR(MAX)
AS
BEGIN
	DECLARE @InstalledFeatures VARCHAR(MAX)

	SELECT @InstalledFeatures = COALESCE(@InstalledFeatures + ', ', '') + ItemName 
	FROM (
		SELECT DISTINCT IM.NAME AS ItemName
		,IM.ItemMasterId AS ItemMasterId
			,CASE IV.DatatypeId
				WHEN 2
					THEN CASE IV.ItemValue
							WHEN 1
								THEN 'Yes'
							WHEN 0
								THEN 'No'
							ELSE ''
							END
				ELSE ISNULL(CustomText, '') + ISNULL(CAST(ItemValue AS VARCHAR(20)), '') + ISNULL(UDM.NAME, '')
				END ItemValue
			,CategoryName
			,CIM.NodeCode
			--,UT.NAME AS UnitType
			,ItemImportance
			,CM.SortOrder AS CategorySort
			,IM.SortOrder AS ItemSort
		FROM CD.ItemValues IV WITH (NOLOCK)
		INNER JOIN CD.ItemMaster IM  WITH (NOLOCK) ON IV.ItemMasterId = IM.ItemMasterId
		LEFT JOIN CD.UserDefinedMaster UDM WITH (NOLOCK) ON IV.UserDefinedId = UDM.UserDefinedId
		LEFT JOIN CD.CategoryItemMapping CIM WITH (NOLOCK) ON IM.ItemMasterId = CIM.ItemMasterId
		LEFT JOIN CD.CategoryMaster CM WITH (NOLOCK) ON CIM.NodeCode = CM.NodeCode
		JOIN TC_CarCondition CC ON cc.StockId=@StockId
		WHERE CarVersionId = @VersionID
			AND IM.IsPublished = 1
			AND IM.IsActive = 1
			AND (
				IM.ItemTypeId = 1
				OR IM.ItemTypeId = 3
				) --added by amit v
			AND CM.NodeCode LIKE '/2/%'
			--AND IV.ItemValue=1
			--AND IV.DatatypeId=2
			AND IM.ItemMasterId NOT IN (SELECT ListMember FROM fnSplitCSV( CC.MissingInstalledFeatures))
		) AS Tab
		WHERE ItemValue<>'No'
	ORDER BY CategorySort
		,ItemSort
		
RETURN @InstalledFeatures
END


