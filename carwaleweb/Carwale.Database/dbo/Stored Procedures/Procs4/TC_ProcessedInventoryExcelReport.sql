IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_ProcessedInventoryExcelReport]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_ProcessedInventoryExcelReport]
GO

	-- Created By:	Tejashree Patil
-- Create date: 17 Sept 2013
-- Description:	Get report from excel sheet from log table after proccessing done.
-- =============================================
CREATE  PROCEDURE       [dbo].[TC_ProcessedInventoryExcelReport]
( 
@BranchId BIGINT,
@UserId BIGINT,
@ExcelSheetId BIGINT
)		
AS 	
BEGIN

	SET NOCOUNT ON;
	
	--inserting record with inactive status,later once image will save in appropriate folder need to activate
	SELECT	TotalRecords, ValidRecords, InValidRecords,RejectedRecords,IsProperExcel,Status, DirPath+FileName AS 'FilePath'
	FROM	TC_ExcelSheetStockInventory 
	WHERE	(@BranchId IS NULL OR BranchId=@BranchId) 
			AND CreatedBy=@UserId --IsDeleted=1 AND StatusId=1 AND IsProperExcel=1
			AND TC_ExcelSheetStockInventoryId=@ExcelSheetId
		
END
