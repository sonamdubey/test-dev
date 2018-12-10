IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[CD].[GetItemDetails]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [CD].[GetItemDetails]
GO

	---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
/*
	THIS STORED PROCEDURE RETURN Details OF ITEM whose Item Master Id you have passed

	WRITTEN BY : SHIKHAR MAHESHWARI ON 28 APR 2012

	Changes History:
       
       Edited By               		EditedON               		Description
       Shikhar Maheshwari         May 28, 2012               	Added the Edit Item details 
       amit--------                	-----------       	             of Custom Type
*/

CREATE PROCEDURE [CD].[GetItemDetails]
	@itemMasterId NUMERIC = 0
	
AS

BEGIN
SELECT
	IM.ItemMasterId, 
	IM.Name,
	--amit
	IM.ItemImportance,
	--amit
	IM.[Description],  
	LTRIM(RTRIM(IM.Abbreviation)) Abbreviation,
	IM.DatatypeId,
	IM.UnitTypeId,
	CD.GetValuesforItem(@itemMasterId) AS DataValues,
	IM.MaxVal, 
	IM.MinVal,
	IM.IsPublished
FROM
	CD.ItemMaster IM WITH(NOLOCK)
WHERE
	IM.ItemMasterId = @itemMasterId
END

