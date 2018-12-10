IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[CD].[FillGroupItemValues]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [CD].[FillGroupItemValues]
GO

	-- =============================================
-- Author:		Amit Verma
-- Create date: 17 March 2013
-- Description:	Saves or deletes ItemValue for group items
-- [CD].[FillGroupItemValues] 2200,16
-- =============================================
/*
	Changes History:
       
       Edited By               		EditedON               		Description
       ----------------       -----------------              	-----------------------
       
*/
CREATE PROCEDURE [CD].[FillGroupItemValues] 
	@VersionID INT,
	@ItemMasterID INT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	DECLARE @IMIdTable table
	    (
           id TINYINT IDENTITY,
           ItemMasterId int
        )
	INSERT into @IMIdTable (ItemMasterID)
	SELECT ItemMasterID from CD.GroupItemConfig WITH(NOLOCK) WHERE ItemIDs LIKE '%,'+ CONVERT(VARCHAR(50), @ItemMasterID)+',%'
			OR ItemIDs LIKE CONVERT(VARCHAR(50), @ItemMasterID)+',%'
			OR ItemIDs LIKE '%,'+ CONVERT(VARCHAR(50), @ItemMasterID)
			OR ItemIDs LIKE CONVERT(VARCHAR(50), @ItemMasterID)
	DECLARE @IMIdCount INT = (SELECT COUNT(ID) FROM @IMIdTable)
	
	WHILE (@IMIdCount != 0)
	
	BEGIN
		DECLARE @GPItemMasterdID int = (SELECT TOP 1 ItemMasterId FROM @IMIdTable)
		DECLARE @ItemIds VARCHAR(50) = (SELECT TOP 1 ItemIDs FROM CD.GroupItemConfig WITH(NOLOCK) WHERE ItemMasterId = @GPItemMasterdID)
		DECLARE @ItemIdsCount INT = (SELECT COUNT(items) FROM SplitText(@ItemIds,','))
		DECLARE @IValueCount INT = (SELECT COUNT(ItemMasterId) FROM CD.ItemValues WITH(NOLOCK) WHERE CarVersionId = @VersionID AND ItemMasterId IN (SELECT items FROM SplitText(@ItemIds,',')))
		--SELECT @ItemIdsCount 'ItemIdsCount',@IValueCount 'IValueCount'
		IF @IValueCount != 0
			BEGIN
				IF ((SELECT COUNT(ItemMasterId) FROM CD.ItemValues WITH(NOLOCK) WHERE CarVersionId = @VersionID AND ItemMasterId = @GPItemMasterdID) = 0)
					BEGIN
						INSERT INTO CD.ItemValues (CarVersionId,ItemMasterId,DataTypeId,CustomText,UpdatedBy)
						VALUES (@VersionID,@GPItemMasterdID,5,(SELECT CD.ComputeGroupItemValue(@VersionID,@GPItemMasterdID)),'System')
						print 'INSERTED'
					END
				ELSE
					BEGIN
						UPDATE CD.ItemValues
						SET CustomText = (SELECT CD.ComputeGroupItemValue(@VersionID,@GPItemMasterdID)),UpdatedOn = GETDATE()
						WHERE CarVersionId = @VersionID AND ItemMasterId = @GPItemMasterdID
						print 'UPDATED'
					END				
			END
		ELSE
			BEGIN
				DELETE FROM CD.ItemValues WHERE CarVersionId = @VersionID AND ItemMasterId IN (SELECT TOP 1 ItemMasterId FROM @IMIdTable)
				print 'DELETED'
			END
		DELETE TOP (1) FROM @IMIdTable
		SET @IMIdCount = (SELECT COUNT(ID) FROM @IMIdTable)
		
	END
END
