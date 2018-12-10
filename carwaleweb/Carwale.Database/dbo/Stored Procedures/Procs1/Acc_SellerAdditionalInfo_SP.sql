IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[Acc_SellerAdditionalInfo_SP]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[Acc_SellerAdditionalInfo_SP]
GO

	
-- PROCEDUTRE TO INSERT/UPDATE ACCESSORIES SELLER DETAILS

CREATE PROCEDURE [dbo].[Acc_SellerAdditionalInfo_SP]
	@SellerId		NUMERIC,
	@SellerAdditionalsId	NUMERIC
 AS
	
BEGIN
	INSERT INTO Acc_SellerAdditionalInfo(SellerId, SellerAdditionalsId,IsActive) 
	VALUES(@SellerId, @SellerAdditionalsId, 1)
END
