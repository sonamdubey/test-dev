IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[UpdateUsedCarPurchaseInquiry]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[UpdateUsedCarPurchaseInquiry]
GO

	
--THIS PROCEDURE IS FOR UPDATING RECORDS FOR used car purchase inquiries request

CREATE PROCEDURE [dbo].[UpdateUsedCarPurchaseInquiry]
	@Id			NUMERIC,	-- 
	@YearFrom		NUMERIC,	-- Car Year From
	@YearTo		NUMERIC,	-- Car Year To
	@BudgetFrom		NUMERIC,	-- Budget From
	@BudgetTo		NUMERIC,	-- Budget To
	@MileageFrom		NUMERIC,	-- Mileage From
	@MileageTo		NUMERIC,	-- Mileage To
	@NoOfCars		INT,		-- How Many cars customer intend to buy
	@BuyTime		VARCHAR(20),	-- When customer intend to buy? i time-frame
	@Comments 		VARCHAR(2000)
 AS
	BEGIN
		UPDATE UsedCarPurchaseInquiries SET
			YearFrom	= @YearFrom, 
			YearTo		= @YearTo, 
			KmFrom		= @MileageFrom, 
			KmTo		= @MileageTo, 
			PriceFrom	= @BudgetFrom, 
			PriceTo		= @BudgetTo,
			NoOfCars	= @NoOfCars, 
			BuyTime	= @BuyTime, 
			Comments	= @Comments
		WHERE
			ID = @Id		
	END
