IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[CD].[GetItemsCompareCars]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [CD].[GetItemsCompareCars]
GO

	-- =============================================
-- Author:		Supriya 
-- Create date: 9/7/2014
-- Description:	Get items data for compare cars
-- Approved by Manish 11-07-2014 06:20 pm
-- =============================================
CREATE PROCEDURE [CD].[GetItemsCompareCars]
	-- Add the parameters for the stored procedure here
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	SELECT IM.ItemMasterId
		,IM.Name,IM.SortOrder
		,IM.OverviewSortOrder
		,CIM.NodeCode
		,IM.IsOverviewable
		,UT.Name UnitType
	FROM CD.ItemMaster IM WITH (NOLOCK)
	INNER JOIN CD.CategoryItemMapping CIM  WITH (NOLOCK) ON IM.ItemMasterId = CIM.ItemMasterId
	LEFT JOIN CD.UnitTypes UT WITH (NOLOCK) ON IM.UnitTypeId = UT.UnitTypeId
	WHERE IM.IsPublished = 1 AND IM.ItemTypeId IN (1,3) AND IM.IsActive = 1

END

