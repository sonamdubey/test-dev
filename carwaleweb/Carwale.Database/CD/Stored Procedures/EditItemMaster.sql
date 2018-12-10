IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[CD].[EditItemMaster]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [CD].[EditItemMaster]
GO

	---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
/*
	THIS STORED PROCEDURE EDITS THE DATA VALUES IN ITEM MASTER TABLE
	AS WELL AS THE CATEGORY-ITEM MAPPING TABLE

	WRITTEN BY : SHIKHAR MAHESHWARI ON 28 JUN 2012

	Changes History:
       
       Edited By               		EditedON               		Description
       amit-------------			----------------------			---------------------
       --------                	-----------       				--------------------
*/

CREATE PROC [CD].[EditItemMaster]
	@itemMasterId INT,
	@categoryMasterId INT = NULL,
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
	@isLive BIT
AS

BEGIN
BEGIN TRY	
	BEGIN TRAN
	UPDATE [CD].ItemMaster
	SET
		Name = @itemName,
		--amit
		ItemImportance = @itemImportance,
		--amit
		DatatypeId = @dataTypeId,
		UnitTypeId = @unitTypeId,
		[Description] = @description,
		UpdatedBy = @userName,
		UpdatedOn = GETDATE(),
		Abbreviation = @abbreviation,
		MinVal = @minVal,
		MaxVal = @maxVal,
		IsPublished = @isLive
	WHERE
		ItemMasterId = @itemMasterId
		
	IF @categoryMasterId IS NOT NULL
	BEGIN
		IF NOT EXISTS(SELECT * FROM CD.CategoryItemMapping 
			WHERE NodeCode = (select NodeCode from CD.CategoryMaster where CategoryMasterId = @categoryMasterId) AND ItemMasterId = @itemMasterId)
		BEGIN
			UPDATE CD.CategoryItemMapping
			SET NodeCode = (select NodeCode from CD.CategoryMaster where CategoryMasterId = @categoryMasterId)WHERE ItemMasterId = @itemMasterId		
		END
	END
	
	IF (@dataTypeId=4)
	BEGIN -- Insert / Update the values in UserDefinedMaster Table
	DECLARE @flag CHAR(5), @userDefinedId INT, @UDFitem VARCHAR(50), @sortOrder FLOAT, @maxUDIindex INT, @comboString VARCHAR(150)
	
		CREATE TABLE #tempt
		(
			Id INT IDENTITY(1,1),
			Value VARCHAR(200)	
		)
		
		INSERT INTO #tempt(Value)
		SELECT items FROM dbo.SplitText(@userDefinedValues, '|')
		
		SELECT @maxUDIindex = MAX(Id) FROM #tempt
		WHILE(@maxUDIindex > 0)
		BEGIN
			SELECT @comboString = Value FROM #tempt WHERE Id = @maxUDIindex
			
			SELECT @flag = s FROM dbo.Splitf(',',@comboString) WHERE pn = 1
			SELECT @userDefinedId = s FROM dbo.Splitf(',',@comboString) WHERE pn = 2
			SELECT @UDFitem = s FROM dbo.Splitf(',',@comboString) WHERE pn = 3
			SELECT @sortOrder = s FROM dbo.Splitf(',',@comboString) WHERE pn = 4
			
			IF @flag = 'add'
			BEGIN
				EXEC [CD].[EditUDM] 'add', 0, @itemMasterId, @UDFitem, '', @sortOrder, @userName
			END 
			ELSE IF @flag = 'edit'
			BEGIN
				EXEC [CD].[EditUDM] 'edit', @userDefinedId, @itemMasterId, @UDFitem, '', @sortOrder, @userName
			END 
			ELSE IF @flag = 'del'
			BEGIN
				EXEC [CD].[EditUDM] 'del', @userDefinedId
			END
		
			SET @maxUDIindex = @maxUDIindex - 1
		END
		
		DROP TABLE #tempt
	END
	COMMIT TRAN
END TRY
BEGIN CATCH
	ROLLBACK TRAN
END CATCH
END

