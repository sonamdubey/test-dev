IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[UpdateCustomerSellInquiry]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[UpdateCustomerSellInquiry]
GO

	
--THIS PROCEDURE IS FOR UPDATING RECORDS FOR SellInquiries TABLE

CREATE PROCEDURE [dbo].[UpdateCustomerSellInquiry]
	@ID			NUMERIC,
	@CarVersionId		NUMERIC,	-- Car Version Id
	@MakeYear		DATETIME,
	@RegNo		VARCHAR(50),
	@Kms			NUMERIC,
	@Price			NUMERIC,
	@Color			VARCHAR(50),	
	@Comments		VARCHAR(500),
	@SendDealers		BIT,
	@ListInClassifieds	BIT
	
 AS
	BEGIN

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
			ID = @ID
	END
