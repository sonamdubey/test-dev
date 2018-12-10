IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_MMDeleteCustomerDetails]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_MMDeleteCustomerDetails]
GO

	-- =============================================
-- Author:		Ranjeet Kumar
-- Create date: 12-Dec-2013
-- Description:	Delete Customer for Mix N Match if Dealer has been bought this customer
-- Table: TC_MMCustomerDetails
-- =============================================
CREATE PROCEDURE [dbo].[TC_MMDeleteCustomerDetails] 
@CustomerId BIGINT,
@DealerId	INT,
@StockId	BIGINT
AS
BEGIN
	SET NOCOUNT ON;
	DELETE FROM TC_MMCustomerDetails  WHERE CWCustomersId =  @CustomerId AND DealerId = @DealerId; 
	UPDATE TC_MMDealersMatchCount SET MatchViewCount = MatchViewCount - 1 WHERE StockId = @StockId AND DealerId = @DealerId
END
