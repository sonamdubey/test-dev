IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[InsertUsedCarWishInquiry]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[InsertUsedCarWishInquiry]
GO

	
--THIS PROCEDURE IS FOR INSERTING AND UPDATING RECORDS FOR SellInquiries TABLE

CREATE PROCEDURE [dbo].[InsertUsedCarWishInquiry]
	
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
	@StateId		NUMERIC,	--state id
	@CityId			NUMERIC,	--City
	@WithinDist		NUMERIC,	--within distance from city
	@ExpiryDate		DATETIME ,	--expiry Date
	@LastReminded		DATETIME ,
	@Comments 		VARCHAR(2000),
	@IsApproved		BIT,		--- Whether Inquiry is verified or not. If admin submits the query this value should be 1, 0 otherwise.	
	@InquiryId		NUMERIC OUTPUT	--id of the inquiry just submitted
	
 AS
	BEGIN
		INSERT INTO UsedCarWishInquiries(CustomerId, 
			YearFrom, YearTo, KmFrom, KmTo, PriceFrom, PriceTo,
			NoOfCars, BuyTime, CarModelIds, CarModelNames, Comments, RequestDateTime,
			StateId,CityId	, WithinDistance , ExpiryDate,	 IsApproved , statusId , LastReminded)
		VALUES( @CustomerId,  @YearFrom, @YearTo, @MileageFrom, @MileageTo, @BudgetFrom, @BudgetTo,
			@NoOfCars, @BuyTime, @CarModelIds, @CarModelNames,  @Comments,  @RequestDateTime,
			@StateId,@CityId	, @WithinDist	,@ExpiryDate,	@IsApproved ,1,@LastReminded	)

		SET @InquiryId =  SCOPE_IDENTITY()  
	END
