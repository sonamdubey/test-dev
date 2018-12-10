IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[CD].[EditUDM]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [CD].[EditUDM]
GO

	---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
/*
	THIS STORED PROCEDURE EDITS THE USER DEFINED DATA VALUES IN User Defined Master TABLE

	WRITTEN BY : SHIKHAR MAHESHWARI ON 26 JUN 2012

	Changes History:
       
       Edited By               		EditedON               		Description
       amit----------------       -----------------              	-----------------------
       --------                	-----------       	             ----------------------
*/

CREATE PROC [CD].[EditUDM]
	@flag CHAR(4),
	@userDefinedId INT = NULL,
	@itemMasterId INT = NULL,
	@userDefinedName VARCHAR(100) = NULL,
	@description VARCHAR(150) = NULL,
	@weightage VARCHAR(50) = NULL,
	@userName VARCHAR(50) = NULL
AS

BEGIN
IF @flag = 'add'
	BEGIN
	INSERT INTO CD.UserDefinedMaster
	(
		ItemMasterId,
		Name,
		[Description],
		ValueImportance,
		UpdatedBy
	)
	VALUES
	(
		@itemMasterId,
		@userDefinedName,
		@description,
		@weightage,
		@userName
	)
	END
ELSE IF @flag = 'edit'
	BEGIN
	UPDATE CD.UserDefinedMaster
	SET 
		Name = @userDefinedName,
		[Description] = @description,
		ValueImportance = @weightage,
		UpdatedOn = GETDATE(),
		UpdatedBy = @userName
	WHERE
		UserDefinedId = @userDefinedId	
	END
ELSE IF @flag = 'del'
	BEGIN
	UPDATE CD.UserDefinedMaster
	SET
		IsActive = 0
	WHERE
		UserDefinedId = @userDefinedId
	END
END
