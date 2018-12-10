IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[CD].[InsertItemMaster]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [CD].[InsertItemMaster]
GO

	---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
/*
	THIS STORED PROCEDURE INSERTS THE DATA VALUES OF IN ITEM MASTER TABLE
	AS WELL AS THE CATEGORY-ITEM MAPPING TABLE

	WRITTEN BY : SHIKHAR MAHESHWARI ON 19 APR 2012

	Changes History:
       
       Edited By               		EditedON               		Description
       Shikhar Maheshwari        Jun 22, 2012               	Added the part of SP for INSERTING CUSTOM
       amit--------                	-----------       	             Values
       amit verma				 March 19, 2013					Added itemType cloumn check
	   khushaboo Patil			 Feb 25,2015					added 
*/

CREATE PROC [CD].[InsertItemMaster]
	@categoryMasterId INT,
	@itemName VARCHAR(100) = NULL,
	--amit
	@itemImportance INT = NULL,
	--amit
	@dataTypeId SMALLINT = NULL,
	@unitTypeId SMALLINT = NULL,
	@description VARCHAR(150) = NULL,
	@abbreviation CHAR(5) = NULL,
	@minVal SMALLINT = NULL,
	@maxVal INT = NULL,
	@userDefinedValues VARCHAR(500) = NULL,
	@userName VARCHAR(50) = NULL,
	@isLive	BIT
AS

BEGIN
DECLARE @itemMasterId INT
DECLARE @NodeCode VARCHAR(50)
BEGIN TRY	
	BEGIN TRAN
	INSERT INTO [CD].ItemMaster
	(
		Name,
		ItemImportance,
		DatatypeId,
		UnitTypeId,
		[Description],
		UpdatedBy,
		Abbreviation,
		MinVal,
		MaxVal,
		ItemTypeId,
		IsPublished
	)
	VALUES
	(
		@itemName,
		--amit
		@itemImportance,
		--amit
		@dataTypeId,
		@unitTypeId,
		@description,
		@userName,
		@abbreviation,
		@minVal,
		@maxVal,
		1,
		@isLive
	)
	
	SET @itemMasterId = SCOPE_IDENTITY()
	
	
	SET @nodecode = (select NodeCode from CD.CategoryMaster where CategoryMasterId = @categoryMasterId)
	
		INSERT INTO CD.CategoryItemMapping
	(
		ItemMasterId,
		NodeCode
	)
	VALUES
	(
		@itemMasterId,
		@NodeCode
	)
	--INSERT INTO CD.CategoryItemMapping
	--(
	--	CategoryMasterId,
	--	ItemMasterId,
	--	UpdatedBy
	--)
	--VALUES
	--(
	--	@categoryMasterId,
	--	@itemMasterId,
	--	@userName
	--)
	
	IF (@dataTypeId=4)
	BEGIN -- Insert the values in UserDefinedMaster Table
		EXEC [CD].[InsertUDM] @itemMasterId, @userDefinedValues, @userName
	END
	COMMIT TRAN
END TRY
BEGIN CATCH
	ROLLBACK TRAN
END CATCH
END

