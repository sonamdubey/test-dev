IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_Deals_StockBulkOperation]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_Deals_StockBulkOperation]
GO

	-- =============================================
-- Author:		Anchal gupta
-- Create date: 18/10/2016
-- Description:	Performing operation on all stocks 
--             1) Operations == 1 => set Ispriceupdated flag true for all stocks
--             2) Operations == 2 => set ispriceupdated flag false for all stocks
-- ModifiedBy : Saket on 19th Oct 2016 added Update for Livedeals Table
-- =============================================
CREATE PROCEDURE [dbo].[TC_Deals_StockBulkOperation]
	-- Add the parameters for the stored procedure here
	@dealerId int, 
	@operations int,
	@value int,
	@model int

AS
BEGIN
 Declare @IsUpdated int
 Update TDS SET TDS.PriceUpdated = @value FROM TC_Deals_Stock AS TDS WITH(NOLOCK) Inner JOIN CarVersions AS CV WITH(NOLOCK) ON TDS.CarVersionId = CV.Id
 where TDS.BranchId = @dealerId and (@model = 0 OR CV.CarModelId = @model)
 set @IsUpdated = @@ROWCOUNT

 UPDATE LiveDeals SET PriceUpdated = @value where DealerId = @dealerId AND (@model = 0 OR ModelId = @model) --Update the live deals Table

 IF   @IsUpdated > 0
   Select CONVERT(BIT, 1)	
 ELSE
   Select CONVERT(BIT, 0)

 
END
