IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_GetAllOffersOfStock]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_GetAllOffersOfStock]
GO

	
-- =============================================
-- Author:		Vivek Gupta
-- Create date: 24-01-2014
-- Description:	Get All Active offers of a stock
-- =============================================
CREATE PROCEDURE [dbo].[TC_GetAllOffersOfStock]
@BranchId INT,
@StockId INT
AS
BEGIN

    SELECT UCO.OfferName FROM TC_UsedCarOffers UCO WITH(NOLOCK)
	INNER JOIN TC_MappingOfferWithStock MOS WITH(NOLOCK)
	ON UCO.TC_UsedCarOfferId = MOS.TC_UsedCarOfferId
	WHERE MOS.IsActive = 1
	AND MOS.StockId = @StockId
	AND UCO.BranchId = @BranchId

END

