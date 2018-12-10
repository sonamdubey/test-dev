IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[GetTransactionDetails]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[GetTransactionDetails]
GO

	
-- =============================================
-- Author:		Mukul Bansal
-- Create date: 03-05-2016
-- Description:	Get the transaction Details from the transaction Id for App
-- Exec [GetTransactionDetails] 90
-- =============================================
CREATE PROCEDURE [dbo].[GetTransactionDetails]
	-- Add the parameters for the stored procedure here
	@transId BIGINT
AS
BEGIN
	select DI.CustomerName as Name, DI.CustomerEmail as Email,
	 DI.CustomerMobile as Mobile, DI.StockId as DealsStockId, DI.CityId, 
	 TDS.CarVersionId as VersionId, TDS.BranchId as DealerId, CV.CarModelId as ModelId, 
	 PGT.CarId as PQId from PGTransactions PGT  With(NoLock)
	inner join DealInquiries DI With(NoLock) ON PGT.ConsumerId = DI.ID
	INNER JOIN TC_Deals_Stock TDS With(NoLock) On DI.StockId = TDS.Id
	Inner Join CarVersions CV With(NoLock) on TDS.CarVersionId = CV.ID
    WHERE PGT.ID = @transId 
END

