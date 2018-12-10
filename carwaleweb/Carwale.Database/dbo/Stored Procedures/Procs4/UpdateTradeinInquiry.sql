IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[UpdateTradeinInquiry]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[UpdateTradeinInquiry]
GO

	
--THIS PROCEDURE IS FOR UPDATING RECORDS FOR customertradeininquiries

CREATE PROCEDURE [dbo].[UpdateTradeinInquiry]
	@Id			NUMERIC,	-- Inquiry ID
	@CarVersionId		NUMERIC,	-- Car Version Id
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
	@ListInClassifieds	BIT

 AS
	DECLARE 
		@SellId		NUMERIC,
		@PurchaseId	NUMERIC		
	
	BEGIN
		BEGIN TRANSACTION TransTradeIn
			
			--get the sellid and the purchaseid
			SELECT @SellId = SellId, @PurchaseId = PurchaseId FROM CustomerTradeinInquiries with (NOLOCK) WHERE ID = @ID

			--update entries for the sell inquiries
			UPDATE CustomerSellInquiries SET
				CarVersionId	= @CarVersionId, 
				CarRegNo	= @RegNo, 
				Price		= @Price,
				MakeYear	= @MakeYear, 
				Kilometers	= @Kms,
				Color		= @Color, 
				Comments	= @Comments,
				ForwardDealers	= @SendDealers, 
				ListInClassifieds	= @ListInClassifieds
			WHERE
				ID = @SellId						

			--update entries for the purchase inquiries	
			UPDATE NewCarPurchaseInquiries SET
				CarVersionId	= @PurchaseCarVersionId, 
				Color		= @PurchaseColor, 
				Comments	= @PurchaseComments
			WHERE
				ID = @PurchaseId	
			
	
		COMMIT TRANSACTION TransTradeIn
	END
