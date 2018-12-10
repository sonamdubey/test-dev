IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[InsertUsedCarPurchaseInquiryNew]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[InsertUsedCarPurchaseInquiryNew]
GO

	
--THIS PROCEDURE IS FOR INSERTING AND UPDATING RECORDS FOR SellInquiries TABLE

CREATE PROCEDURE [dbo].[InsertUsedCarPurchaseInquiryNew]
	@SellInquiryId		NUMERIC,	-- Sell Inquiry Id
	@CarModelIds		VARCHAR(100), -- Model IDs
	@CarModelNames	VARCHAR(500), -- Model Names
	@CustomerId		NUMERIC,	-- Customer Id
	@YearFrom		NUMERIC,	-- Car Year From
	@YearTo		NUMERIC,	-- Car Year To
	@BudgetFrom		NUMERIC,	-- Budget From
	@BudgetTo		NUMERIC,	-- Budget To
	@MileageFrom		NUMERIC,	-- Mileage From
	@MileageTo		NUMERIC,	-- Mileage To
	@NoOfCars		INT,		-- How Many cars customer intend to buy
	@BuyTime		VARCHAR(20),	-- When customer intend to buy? i time-frame
	@RequestDateTime	DATETIME,	-- Entry Date
	@Comments 		VARCHAR(2000),
	@IsApproved		BIT,		--- Whether Inquiry is verified or not. If admin submits the query this value should be 1, 0 otherwise.	
	@InquiryId		NUMERIC OUTPUT	--id of the inquiry just submitted
 AS
	BEGIN
		INSERT INTO UsedCarPurchaseInquiries(CustomerId, SellInquiryId, 
			YearFrom, YearTo, KmFrom, KmTo, PriceFrom, PriceTo,
			NoOfCars, BuyTime, CarModelIds, CarModelNames, Comments, RequestDateTime, IsApproved)
		VALUES( @CustomerId, @SellInquiryId, 
			@YearFrom, @YearTo, @MileageFrom, @MileageTo, @BudgetFrom, @BudgetTo,
			@NoOfCars, @BuyTime, @CarModelIds, @CarModelNames,  @Comments,  @RequestDateTime,@IsApproved)

		SET @InquiryId =  SCOPE_IDENTITY()  
	END
