IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[CD].[InsertUDM]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [CD].[InsertUDM]
GO

	---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
/*
	THIS STORED PROCEDURE INSERTS THE USER DEFINED DATA VALUES IN User Defined Master TABLE
	AS WELL

	WRITTEN BY : SHIKHAR MAHESHWARI ON 25 JUN 2012

	Changes History:
       
       Edited By               		EditedON               		Description
       amit----------------       -----------------              	-----------------------
       --------                	-----------       	             ----------------------
*/

CREATE PROC [CD].[InsertUDM]
	@itemMasterId INT,
	@userDefinedValues VARCHAR(500) = NULL,
	@userName VARCHAR(50) = NULL
AS

BEGIN
	DECLARE @UDFitem VARCHAR(50), @custDescription VARCHAR(50), @sortOrder FLOAT, @maxUDIindex INT, @sortComboString VARCHAR(150)
	SET @UDFitem = ''
	SET @custDescription = ''
	SET @sortOrder = 0
	SET @maxUDIindex = 0
	
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
		SELECT @sortComboString = Value FROM #tempt WHERE Id = @maxUDIindex
		SELECT @UDFitem = s FROM dbo.Splitf(',',@sortComboString) WHERE pn = 1
		SELECT @sortOrder = CAST(s AS decimal) FROM dbo.Splitf(',',@sortComboString) WHERE pn = 2
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
			@UDFitem,
			@custDescription,
			@sortOrder,
			@userName
		)
		
		SET @maxUDIindex = @maxUDIindex - 1
	END
	
	DROP TABLE #tempt
END
