IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[InsertUsedCarPurchaseInquiry_12032012]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[InsertUsedCarPurchaseInquiry_12032012]
GO

	--THIS PROCEDURE IS FOR INSERTING AND UPDATING RECORDS FOR SellInquiries TABLE

CREATE PROCEDURE [dbo].[InsertUsedCarPurchaseInquiry_12032012]
	@SellInquiryId		NUMERIC,	-- Sell Inquiry Id
	@CarModelIds		VARCHAR(100) = Null, -- Model IDs
	@CarModelNames	VARCHAR(500), -- Model Names
	@CustomerId		NUMERIC,	-- Customer Id
	@YearFrom		NUMERIC = NULL,	-- Car Year From
	@YearTo		NUMERIC = NULL,	-- Car Year To
	@BudgetFrom		NUMERIC = NULL,	-- Budget From
	@BudgetTo		NUMERIC = NULL,	-- Budget To
	@MileageFrom		NUMERIC = NULL,	-- Mileage From
	@MileageTo		NUMERIC = NULL,	-- Mileage To
	@NoOfCars		INT = 1,		-- How Many cars customer intend to buy
	@BuyTime		VARCHAR(20) = NULL,	-- When customer intend to buy? i time-frame
	@RequestDateTime	DATETIME,	-- Entry Date
	@Comments 		VARCHAR(2000) = NULL,
	@IsApproved		BIT = 1,		--- Whether Inquiry is verified or not. If admin submits the query this value should be 1, 0 otherwise.	
	@InquiryId		NUMERIC OUTPUT	--id of the inquiry just submitted
 AS
	BEGIN
		-- Check if user is already shown interest
		SET @InquiryId = (SELECT TOP 1 ID FROM UsedCarPurchaseInquiries WHERE CustomerID = @CustomerId AND SellInquiryId = @SellInquiryId)
		
		-- If this customer showing inquiry in this car first time.
		If @InquiryId IS NULL
		BEGIN
			INSERT INTO UsedCarPurchaseInquiries(CustomerId, SellInquiryId, 
				YearFrom, YearTo, KmFrom, KmTo, PriceFrom, PriceTo,
				NoOfCars, BuyTime, CarModelIds, CarModelNames, Comments, RequestDateTime, IsApproved)
			VALUES( @CustomerId, @SellInquiryId, 
				@YearFrom, @YearTo, @MileageFrom, @MileageTo, @BudgetFrom, @BudgetTo,
				@NoOfCars, @BuyTime, @CarModelIds, @CarModelNames,  @Comments,  @RequestDateTime,@IsApproved)

			SET @InquiryId =  SCOPE_IDENTITY()
			
			-- Check if this car uploaded through Trading Cars application
			declare @StockId numeric, @BranchId int, @dealerid numeric
			
			select @StockId = si.TC_StockId, @BranchId = st.BranchId, @dealerid = si.DealerId 
			from SellInquiries si, TC_Stock st where si.TC_StockId = st.Id and si.ID = @SellInquiryId
			
			if @StockId is not null
			BEGIN
				declare @customer varchar(50), @email varchar(100), @mobile varchar(12), @current_time datetime = getdate(), @id numeric
				
				-- If yes, send this inquiry back to thet dealer
				select @customer=cs.Name, @mobile=cs.Mobile, @email=cs.email from Customers cs where Id = @CustomerId
				
				-- execute SP to submit purchase inquiry back to trading car dealer
				exec TC_SaveInquiryDetails_SP @dealerid, @customer, @mobile, @email,''/*comments*/,@CarModelNames,
				1/*status*/,1/*inquiry source(carwale)*/,@current_time/*followup time*/,@StockId,-1/*inquiryid*/,@current_time/*request time*/,@id output
			END
		END			
	END
