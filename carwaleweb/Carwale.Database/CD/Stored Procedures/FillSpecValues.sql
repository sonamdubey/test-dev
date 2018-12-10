IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[CD].[FillSpecValues]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [CD].[FillSpecValues]
GO

	-- =============================================
-- Author:		Amit Verma
-- Create date: 17 March 2013
-- Description:	Saves or deletes ItemValue for group itoems
-- [CD].[FillSpecValues] 2689,30
-- =============================================
/*
	Changes History:
       
       Edited By               		EditedON               		Description
       ----------------       -----------------              	-----------------------
       -- Amit Verma 14-08-2013 Added where CarVersionId = @VersionID
       
*/
CREATE PROCEDURE [CD].[FillSpecValues] 
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
           TblID int,
           NCS_Col_Name varchar(50)
        )
	INSERT into @IMIdTable (TblID,NCS_Col_Name)
	SELECT ID,NCS_Col_Name from dbo.New_Old_Specs_mapping WITH(NOLOCK)
	WHERE ItemMasterID = @ItemMasterID
	DECLARE @IMIdCount INT = (SELECT COUNT(ID) FROM @IMIdTable)
	
	WHILE (@IMIdCount != 0)
	
	BEGIN
		DECLARE @TblID int = (SELECT TOP 1 TblID FROM @IMIdTable)
		DECLARE @ItemIds VARCHAR(50) = (SELECT TOP 1 ItemIDs FROM dbo.New_Old_Specs_mapping WITH(NOLOCK) WHERE ID = @TblID)
		DECLARE @IValueCount INT = (SELECT COUNT(ItemMasterId) FROM CD.ItemValues WITH(NOLOCK) WHERE CarVersionId = @VersionID AND ItemMasterId IN (SELECT items FROM SplitText(@ItemIds,',')))
		DECLARE @SQL NVARCHAR(MAX) = ''
		DECLARE @SingleQuote VARCHAR(2) = '''';
		DECLARE @cmd VARCHAR(50) = 'No operation performed'
		IF @IValueCount != 0
			BEGIN
				DECLARE @Val NVARCHAR(MAX) = (SELECT CD.ComputeSpecValue(@VersionID,@TblID))
				IF(@Val IS NULL)
				BEGIN
					SET @SQL = 'UPDATE NewCarSpecifications SET '
							   + (SELECT TOP 1 NCS_Col_Name FROM @IMIdTable)
							   + ' = '
							   + 'NULL'
							   + ' WHERE CarVersionId = ' + CAST(@VersionID AS VARCHAR(10))
					SET @cmd = 'SET TO NULL'
					EXEC Sp_executesql @SQL
				END
				-- Amit Verma 14-08-2013 Added where CarVersionId = @VersionID
				IF EXISTS(SELECT CarVersionId FROM NewCarSpecifications where CarVersionId = @VersionID)
				BEGIN
					SET @SQL = 'UPDATE NewCarSpecifications SET '
							   + (SELECT TOP 1 NCS_Col_Name FROM @IMIdTable)
							   + ' = '
							   + @SingleQuote
							   + @Val
							   + @SingleQuote
							   + ' WHERE CarVersionId = ' + CAST(@VersionID AS VARCHAR(10))
					SET @cmd = 'UPDATED'
				END
				ELSE
				BEGIN
					SET @SQL = 'INSERT INTO NewCarSpecifications (CarVersionId,'
							   + (SELECT TOP 1 NCS_Col_Name FROM @IMIdTable)
							   + ') VALUES('
							   + CAST(@VersionID AS VARCHAR(10))
							   + ','
							   + @SingleQuote
							   + @Val
							   + @SingleQuote
							   + ')'
					SET @cmd = 'INSERTED'
				END
				EXEC Sp_executesql @SQL
			END
		ELSE
		BEGIN
			SET @SQL = 'UPDATE NewCarSpecifications SET '
					   + (SELECT TOP 1 NCS_Col_Name FROM @IMIdTable)
					   + ' = '
					   + 'NULL'
					   + ' WHERE CarVersionId = ' + CAST(@VersionID AS VARCHAR(10))
			SET @cmd = 'SET TO NULL'
			EXEC Sp_executesql @SQL
		END
		PRINT @cmd
		DELETE TOP (1) FROM @IMIdTable
		SET @IMIdCount = (SELECT COUNT(ID) FROM @IMIdTable)
		
	END
END
