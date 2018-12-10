IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[CD].[GetOverviewByVersionID]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [CD].[GetOverviewByVersionID]
GO

	-- =============================================
-- Author:		Amit Verma
-- Create date: <Create Date,,>
-- Description:	this stored procedure collects overview data of a version
-- [CD].[GetOverviewByVersionID] 2340
-- =============================================
/*
	Changes History:
       
       Edited By               		EditedON               		Description
       ----------------       -----------------              	-----------------------
       Amit Verma                 28/3/2013                     added logic to consider group items      
*/
CREATE  PROCEDURE   [CD].[GetOverviewByVersionID]
	@VersionID int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	SELECT ItemName
		,UnitType
		,ItemValue
	FROM(
	SELECT DISTINCT IM.NAME AS ItemName
		,ISNULL(CustomText, '') + ISNULL(CAST(ItemValue AS VARCHAR(20)), '') + ISNULL(UDM.NAME, '') ItemValue
		,UT.NAME AS UnitType
		,IM.OverviewSortOrder AS CategoryOrder
	FROM CD.ItemValues IV WITH(NOLOCK)
	INNER JOIN CD.ItemMaster IM WITH(NOLOCK) ON IV.ItemMasterId = IM.ItemMasterId
	LEFT JOIN CD.UserDefinedMaster UDM WITH(NOLOCK) ON IV.UserDefinedId = UDM.UserDefinedId
	LEFT JOIN CD.CategoryItemMapping CIM WITH(NOLOCK) ON IM.ItemMasterId = CIM.ItemMasterId
	LEFT JOIN CD.CategoryMaster CM WITH(NOLOCK) ON CIM.NodeCode = CM.NodeCode
	LEFT JOIN CD.UnitTypes UT WITH(NOLOCK) ON IM.UnitTypeId = UT.UnitTypeId
	WHERE CarVersionId = @VersionID and IM.IsPublished = 1 and IM.IsOverviewable = 1
	AND IM.IsActive = 1 AND (IM.ItemTypeId = 1 OR IM.ItemTypeId = 3)) --added by amit v
	AS Tab
	ORDER BY CategoryOrder
	
END
