IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[CD].[GetCarDataByVersionID]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [CD].[GetCarDataByVersionID]
GO

	-- =============================================
-- Author:		Amit Verma
-- Create date: <Create Date,,>
-- Description:	this stored procedure collects specification and features of all the versions for a model
-- [CD].[GetCarDataByVersionID] 1537
-- =============================================
/*
	Changes History:
       
       Edited By               		EditedON               		Description
       ----------------       -----------------              	-----------------------
       Amit Verma                 28/3/2013                     added logic to consider group items      
*/
CREATE  PROCEDURE   [CD].[GetCarDataByVersionID]
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
		,ISNULL(CustomText, '') + ISNULL(CAST(ItemValue AS VARCHAR(20)), '') + ISNULL(UDM.NAME, '') ItemValue
		,CategoryName
		,CIM.NodeCode
		,UT.NAME AS UnitType
		,CM.SortOrder AS CategoryOrder
		,IM.SortOrder AS ItemOrder
	FROM CD.ItemValues IV WITH(NOLOCK)
	INNER JOIN CD.ItemMaster IM WITH(NOLOCK) ON IV.ItemMasterId = IM.ItemMasterId
	LEFT JOIN CD.UserDefinedMaster UDM WITH(NOLOCK) ON IV.UserDefinedId = UDM.UserDefinedId
	LEFT JOIN CD.CategoryItemMapping CIM WITH(NOLOCK) ON IM.ItemMasterId = CIM.ItemMasterId
	LEFT JOIN CD.CategoryMaster CM WITH(NOLOCK) ON CIM.NodeCode = CM.NodeCode
	LEFT JOIN CD.UnitTypes UT WITH(NOLOCK) ON IM.UnitTypeId = UT.UnitTypeId
	WHERE CarVersionId = @VersionID and IM.IsPublished = 1
	AND IM.IsActive = 1 AND (IM.ItemTypeId = 1 OR IM.ItemTypeId = 3)) --added by amit v
	AS Tab
	ORDER BY CategoryOrder,ItemOrder


END
