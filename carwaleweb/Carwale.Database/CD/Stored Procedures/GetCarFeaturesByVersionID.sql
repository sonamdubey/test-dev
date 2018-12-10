IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[CD].[GetCarFeaturesByVersionID]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [CD].[GetCarFeaturesByVersionID]
GO

	
-- =============================================
-- Author:		Amit Verma
-- Create date: <Create Date,,>
-- Description:	this stored procedure collects features of a version
-- [CD].[GetCarFeaturesByVersionID] 2340
-- Modified By : Suresh Prajapati on 27th July, 2015
-- Description : Added ItemMasterId in select clause
-- =============================================
/*
	Changes History:
       Edited By               		EditedON               			Description
       ----------------       -----------------              	-----------------------
       Amit Verma                 28/3/2013                     added logic to consider group items      
	   Suresh Prajapati			27th July, 2015					Added ItemMasterId in select clause
*/
-- Modified by Manish Chourasiya on 26-08-2015 added WITH (NOLOCK).
CREATE PROCEDURE [CD].[GetCarFeaturesByVersionID] @VersionID INT --   
AS
BEGIN
	SELECT ItemName
		,ItemMasterId
		,ItemValue
		,CategoryName
		,NodeCode
	--,UnitType
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
		--LEFT JOIN CD.UnitTypes UT ON IM.UnitTypeId = UT.UnitTypeId
		WHERE CarVersionId = @VersionID
			AND IM.IsPublished = 1
			AND IM.IsActive = 1
			AND (
				IM.ItemTypeId = 1
				OR IM.ItemTypeId = 3
				) --added by amit v
			AND CM.NodeCode LIKE '/2/%'
		) AS Tab
	ORDER BY CategorySort
		,ItemSort
END