IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[CD].[DeleteItemMaster]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [CD].[DeleteItemMaster]
GO

	---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
/*
	THIS STORED PROCEDURE DELETE THE DATA VALUES OF ITEMS FROM ItemValues, UserDefinedMaster, CategoryItemMapping 
	AND ItemMaster TABLE 

	WRITTEN BY : SHIKHAR MAHESHWARI ON 25 JUN 2012

	Changes History:
       
       Edited By               		EditedON               		Description
       -----------------          ---------------               	---------------------
       --------                	-----------       	             ----------
*/

CREATE PROC [CD].[DeleteItemMaster]
	@itemMasterId INT = NULL
AS
BEGIN
	DELETE FROM [CD].[ItemValues] WHERE ItemMasterId = @itemMasterId
	DELETE FROM [CD].[UserDefinedMaster] WHERE	ItemMasterId = @itemMasterId
	DELETE FROM [CD].[CategoryItemMapping] WHERE ItemMasterId = @itemMasterId
	DELETE FROM [CD].[ItemMaster] WHERE ItemMasterId = @itemMasterId
END
