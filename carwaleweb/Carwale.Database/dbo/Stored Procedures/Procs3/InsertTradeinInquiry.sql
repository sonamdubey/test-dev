IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[InsertTradeinInquiry]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[InsertTradeinInquiry]
GO

	
--THIS PROCEDURE IS FOR INSERTING AND UPDATING RECORDS FOR SellInquiries TABLE

CREATE PROCEDURE [dbo].[InsertTradeinInquiry]
	@CustomerId		NUMERIC,	-- Dealer ID
	@CarVersionId		NUMERIC,	-- Car Version Id
	@RequestDateTime	DATETIME,	-- Entry Date
	@MakeYear		DATETIME,
	@RegNo		VARCHAR(50),
	@Kms			NUMERIC,
	@Price			NUMERIC,
	@Color			VARCHAR(50),	
	@Comments		VARCHAR(500),
	@PurchaseCarVersionId	NUMERIC,
	@PurchaseColor	VARCHAR(50),
	@PurchaseComments	VARCHAR(2000),
	@SendDealers		BIT,
	@ListInClassifieds	BIT,
	@IsApproved		BIT

 AS
	DECLARE 
		@SellId		NUMERIC,
		@PurchaseId	NUMERIC		
	
	BEGIN
		INSERT INTO CustomerSellInquiries( CustomerId, CarVersionId, CarRegNo, EntryDate, Price,
			MakeYear, Kilometers,Color, Comments,ForwardDealers, ListInClassifieds,IsApproved) 
			VALUES(@CustomerId, @CarVersionId, @RegNo, @RequestDateTime, @Price,
			@MakeYear, @Kms, @Color, @Comments, @SendDealers, @ListInClassifieds,@IsApproved)

		SET @SellId = SCOPE_IDENTITY()  

		INSERT INTO NewCarPurchaseInquiries(CustomerId, CarVersionId, Color, Comments, RequestDateTime,IsApproved )
		VALUES(@CustomerId, @PurchaseCarVersionId, @PurchaseColor, @PurchaseComments, @RequestDateTime,@IsApproved )

		SET @PurchaseId = SCOPE_IDENTITY()  			

		INSERT INTO CustomerTradeinInquiries( CustomerId, SellId, PurchaseId, RequestDateTime,IsApproved )
		VALUES( @CustomerID, @SellId, @PurchaseId, @RequestDateTime,@IsApproved )
	END
