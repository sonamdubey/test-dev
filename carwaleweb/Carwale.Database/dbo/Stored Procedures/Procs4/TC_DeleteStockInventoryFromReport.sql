IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_DeleteStockInventoryFromReport]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_DeleteStockInventoryFromReport]
GO

	
-- =============================================
-- Author:		VISHAL SRIVASTAVA
-- Create date: 23 JANUARY 2014 1145 HRS IST
-- Description:	Delete stock from TC_StockInventory Table parmanently
-- =============================================
CREATE PROCEDURE [dbo].[TC_DeleteStockInventoryFromReport]
	-- Add the parameters for the stored procedure here
	@RecordsId VARCHAR(1000),
	@UserId INT = NULL,
	@BranchId INT = NULL
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Update statements for TC_ExcelStockInventory
	UPDATE	TC_ExcelStockInventory 
	SET		IsDeleted = 1 , ModifiedBy = @UserId, ModifiedDate=GETDATE()
	FROM    TC_ExcelStockInventory AS E 
			INNER JOIN TC_StockInventory AS S
			ON E.ChassisNumber=S.ChassisNumber--E.TC_ExcelStockInventoryId=S.TC_ExcelStockInventoryId
	WHERE	S.TC_StockInventoryId IN (SELECT ListMember AS UserId FROM fnSplitCSV(@RecordsId))
	
	-- Insert statement for TC_StockInventoryDeletedLog from TC_StockInventory
	INSERT INTO [dbo].[TC_StockInventoryDeletedLog] 
											(TC_StockInventoryId,
											 TC_ExcelStockInventoryId, 
											 ModelCode, 
											 ColourCode,
											 PrCodes,
											 Region, 
											 ChassisNumber, 
											 DealerCompanyName, 
											 SellingDealerCode, 
											 DealerLocation, 
											 PaymentDealerInvoiceDate, 
											 ModelYear, 
											 CheckpointDate, 
											 WholesaleDate, 
											 EntryDate, 
											 TC_UserId, 
											 BranchId, 
											 ModifiedBy, 
											 ModifiedDate, 
											 TC_ExcelSheetStockInventoryId, 
											 IsSpecialUser, 
											 DeletedDate, 
											 DeletedBy)
														SELECT	T.TC_StockInventoryId, 
																E.TC_ExcelStockInventoryId, 
																T.ModelCode,
																T.ColourCode,
																T.PrCodes,
																T.Region, 
																T.ChassisNumber,
																T.DealerCompanyName,
																T.SellingDealerCode,
																T.DealerLocation,
																T.PaymentDealerInvoiceDate,
																T.ModelYear,
																T.CheckpointDate,
																T.WholesaleDate,
																T.EntryDate,
																T.TC_UserId,
																T.BranchId,
																T.ModifiedBy,
																T.ModifiedDate,
																T.TC_ExcelSheetStockInventoryId,
																T.IsSpecialUser,
																GETDATE(),
																@UserId
																FROM	TC_StockInventory AS T WITH(NOLOCK) 
																inner join TC_ExcelStockInventory as E with(nolock)
																on T.ChassisNumber=E.ChassisNumber
																WHERE	T.TC_StockInventoryId IN (SELECT ListMember AS UserId FROM fnSplitCSV(@RecordsId)) 

	-- Delete statement for TC_StockInventory
	DELETE 
	FROM	TC_StockInventory
	WHERE	TC_StockInventoryId IN (SELECT ListMember AS UserId FROM fnSplitCSV(@RecordsId))
	
END

