IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[CD].[GetItemsFromCategory]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [CD].[GetItemsFromCategory]
GO

	---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
/*
	THIS STORED PROCEDURE RETURN CATEGORIES OF ITEMS
	IF YOU PASS CategoryMasterId = 0, IT WILL FETCH ALL THE ACTIVE CATEGORIES

	WRITTEN BY : SHIKHAR MAHESHWARI ON 3 APR 2012

	Changes History:
       
       Edited By               		EditedON               		Description
       Shikhar Maheshwari			April 12, 2012              Added an additional parameter
       --------                	-----------       	            of Car Version Id
       Amit Verma					March 29, 2013				Added logic to return only ItemTypes 1 and 2
       
       [CD].[GetItemsFromCategory] 6,0
*/

CREATE PROCEDURE  [CD].[GetItemsFromCategory]
	@categoryMasterId INT = 0,
	@carVersionId INT = 0
	
AS

BEGIN
SELECT
	IM.ItemMasterId, 
	IM.Name,
	IM.[Description],  
	LTRIM(RTRIM(IM.Abbreviation)) Abbreviation,
	IM.DatatypeId,
	IM.UnitTypeId,
	CD.GetValuesforItem(IM.ItemMasterId) AS DataValues,
	IM.MaxVal, 
	IM.MinVal,
	CD.GetItemValue(IM.ItemMasterId, @carVersionId) AS Value
FROM
	CD.ItemMaster IM WITH(NOLOCK)
	INNER JOIN CD.CategoryItemMapping CIM WITH(NOLOCK)
		ON IM.ItemMasterId = CIM.ItemMasterId
	INNER JOIN CD.CategoryMaster CM WITH(NOLOCK)
		ON CIM.NodeCode = CM.NodeCode
WHERE
	CM.CategoryMasterId = @categoryMasterId
AND
	IM.IsActive = 1
AND 
	(IM.ItemTypeId = 1 OR ItemTypeId = 2)
	
--SELECT
--	IM.ItemMasterId, 
--	IM.Name,
--	IM.[Description],  
--	LTRIM(RTRIM(IM.Abbreviation)) Abbreviation,
--	IM.DatatypeId,
--	IM.UnitTypeId,
--	CD.GetValuesforItem(IM.ItemMasterId) AS DataValues,
--	IM.MaxVal, 
--	IM.MinVal,
--	CD.GetItemValue(IM.ItemMasterId, @carVersionId) AS Value
--FROM
--	CD.ItemMaster IM WITH(NOLOCK)
--	INNER JOIN CD.CategoryItemMapping CIM WITH(NOLOCK)
--		ON IM.ItemMasterId = CIM.ItemMasterId
--	INNER JOIN CD.CategoryMaster CM WITH(NOLOCK)
--		ON CIM.CategoryMasterId = CM.CategoryMasterId
--WHERE
--	CM.CategoryMasterId = @categoryMasterId
--AND
--	IM.IsActive = 1
END
