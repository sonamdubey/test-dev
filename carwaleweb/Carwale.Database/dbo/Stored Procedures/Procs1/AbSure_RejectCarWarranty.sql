IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[AbSure_RejectCarWarranty]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[AbSure_RejectCarWarranty]
GO

	CREATE PROCEDURE [dbo].[AbSure_RejectCarWarranty] --610640,5
	@StockId INT = NULL,
	@BranchId BIGINT
AS
BEGIN
	SET NOCOUNT ON;
	UPDATE	TC_Stock
	SET		IsWarrantyRequested=0
	WHERE	Id=@StockId

	UPDATE  AbSure_CarDetails
	SET		IsRejected=1,
			RejectedDateTime = GETDATE()
	WHERE	StockId = @StockId
END