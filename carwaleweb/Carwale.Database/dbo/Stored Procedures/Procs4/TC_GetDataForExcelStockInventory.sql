IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_GetDataForExcelStockInventory]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_GetDataForExcelStockInventory]
GO

	CREATE PROCEDURE [dbo].[TC_GetDataForExcelStockInventory]
@BranchId BIGINT,
@MakeId INT = NULL
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	
	--Get DealerCode
	EXEC TC_CheckDealerBelongtoMake @BranchId,@MakeId
			
	--Get Version / model code and Get Color code
	EXEC TC_GetCarVersionColorCode NULL,NULL
END
