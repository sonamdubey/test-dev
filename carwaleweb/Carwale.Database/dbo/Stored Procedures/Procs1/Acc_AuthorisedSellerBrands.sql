IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[Acc_AuthorisedSellerBrands]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[Acc_AuthorisedSellerBrands]
GO

	
-- PROCEDUTRE TO INSERT/UPDATE ACCESSORIES SELLER DETAILS

CREATE PROCEDURE [dbo].[Acc_AuthorisedSellerBrands]
	@SellerId	NUMERIC,
	@BrandId	NUMERIC
 AS
	
BEGIN
	INSERT INTO Acc_AuthorisedSeller(SellerId, BrandId, IsAuthorised, IsActive) 
	VALUES(@SellerId, @BrandId, 1,1)
END
