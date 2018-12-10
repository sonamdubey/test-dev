IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[CD].[InactiveItemMaster]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [CD].[InactiveItemMaster]
GO

	---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
/*
	THIS STORED PROCEDURE CHANGES THE ACTIVE STATE OF THE ITEM MASTER TO INACTIVE RATHER THAN DELETING IT
	FROM THE ITEM MASTER

	WRITTEN BY : SHIKHAR MAHESHWARI ON 25 JUN 2012

	Changes History:
       
       Edited By               		EditedON               		Description
       -----------------          ---------------               	---------------------
       --------                	-----------       	             ----------
*/

CREATE PROC [CD].[InactiveItemMaster]
	@itemMasterId INT = NULL
AS
BEGIN
	UPDATE [CD].[ItemMaster] 
		SET IsActive = 0
	WHERE 
		ItemMasterId = @itemMasterId
END
