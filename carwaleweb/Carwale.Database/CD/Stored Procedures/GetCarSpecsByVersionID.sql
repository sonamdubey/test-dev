IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[CD].[GetCarSpecsByVersionID]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [CD].[GetCarSpecsByVersionID]
GO

	-- =============================================
-- Author:		Amit Verma
-- Create date: <Create Date,,>
-- Description:	this stored procedure collects specifications of a version
-- [CD].[GetCarSpecsByVersionID] 2282
-- =============================================
/*
	Changes History:
       
       Edited By               		EditedON               		Description
       ----------------       -----------------              	-----------------------
       Amit Verma                 28/3/2013                     added logic to consider group items      
*/
-- Modified by Manish on 14-09-2015 added with (nolock) in all the tables.
CREATE  PROCEDURE   [CD].[GetCarSpecsByVersionID]
@VersionID INT
AS
BEGIN
	SELECT ItemName
		,ItemValue
		,CategoryName
		,NodeCode
		,UnitType
	FROM(
	SELECT DISTINCT IM.NAME AS ItemName
		,CASE IV.DatatypeId
			WHEN 2 THEN CASE IV.ItemValue WHEN 1 THEN 'Yes' WHEN 0 THEN 'No' ELSE '' END
			ELSE ISNULL(CustomText, '') + ISNULL(CAST(ItemValue AS VARCHAR(20)), '') + ISNULL(UDM.NAME, '') END ItemValue
		,CategoryName
		,CIM.NodeCode
		,UT.NAME AS UnitType
		,CM.SortOrder AS CategorySort
		,IM.SortOrder AS ItemSort
	FROM CD.ItemValues IV WITH (NOLOCK)
	INNER JOIN CD.ItemMaster IM  WITH (NOLOCK) ON IV.ItemMasterId = IM.ItemMasterId
	LEFT JOIN CD.UserDefinedMaster UDM WITH (NOLOCK) ON IV.UserDefinedId = UDM.UserDefinedId
	LEFT JOIN CD.CategoryItemMapping CIM WITH (NOLOCK) ON IM.ItemMasterId = CIM.ItemMasterId
	LEFT JOIN CD.CategoryMaster CM WITH (NOLOCK) ON CIM.NodeCode = CM.NodeCode
	LEFT JOIN CD.UnitTypes UT WITH (NOLOCK) ON IM.UnitTypeId = UT.UnitTypeId AND UT.UnitTypeId NOT IN(1,2)
	WHERE CarVersionId = @VersionID AND IM.IsPublished =1
	AND IM.IsActive = 1 AND (IM.ItemTypeId = 1 OR IM.ItemTypeId = 3) --added by amit v
	AND CM.NodeCode like '/1/%') AS Tab
	ORDER BY CategorySort,ItemSort

END