IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_ExcelStockInventoryOperationLog]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_ExcelStockInventoryOperationLog]
GO
	-- =============================================
-- Author:		Tejashree Patil
-- Create date: 12 Sept,2013
-- Description:	Insert log for stock inventory excel operation
-- Modified By:Tejashree Patil on 30 Sept 2013, Added IsSpecialUser parameter. 
-- =============================================
CREATE PROCEDURE [dbo].[TC_ExcelStockInventoryOperationLog]
@BranchId BIGINT,
@UserId BIGINT,
@IsExcelProcessed BIT = 0,
@IsProperExcelFormat BIT = 1,
@ExcelSheetName VARCHAR(50), 
@FileName VARCHAR(100), 
@DirPath VARCHAR(255),
@HostUrl VARCHAR(100),
@TotalRecords INT,
@ValidRecords INT,
@InValidRecords INT,
@RejectedRecords INT,
@IsSpecialUser BIT,
@ExcelSheetId BIGINT OUTPUT,
@Location VARCHAR(255) OUTPUT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	SET @Location = 'http://'+@HostUrl+@DirPath+@FileName
	
	
	IF(@ExcelSheetId IS NULL)
	BEGIN
		INSERT INTO TC_ExcelSheetStockInventory(CreatedBy,BranchId,EntryDate,Status,IsDelated, DirPath, FileName, HostUrl, Location, ExcelSheetName, IsSpecialUser)
		VALUES		(@UserId,@BranchId,GETDATE(), @IsExcelProcessed, 0, @DirPath, @FileName, @HostUrl, @Location, @ExcelSheetName, @IsSpecialUser)	
		
		SET @ExcelSheetId = SCOPE_IDENTITY();
	END
	ELSE
	BEGIN
		UPDATE	TC_ExcelSheetStockInventory
		SET		Status=@IsExcelProcessed, IsProperExcel=@IsProperExcelFormat, TotalRecords=@TotalRecords, ValidRecords=@ValidRecords, 
				InValidRecords=@InValidRecords, RejectedRecords=@RejectedRecords, IsDelated=1,LastUpdatedDate=GETDATE()
		WHERE	TC_ExcelSheetStockInventoryId=@ExcelSheetId
	END
	

END
