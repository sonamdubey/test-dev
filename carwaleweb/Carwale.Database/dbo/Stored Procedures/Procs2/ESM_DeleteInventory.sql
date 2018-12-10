IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[ESM_DeleteInventory]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[ESM_DeleteInventory]
GO

	-- =============================================
-- Author	:	Ajay Singh(29th Oct 2015)
-- Description	:	To Delete  inventory
-- =============================================
CREATE PROCEDURE [dbo].[ESM_DeleteInventory]
@InventoryId INT 
AS
BEGIN
	UPDATE ESM_InventoryBooking 
	SET isActive = 0
    WHERE InventoryId=@InventoryId 
END