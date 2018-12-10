IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[UpdateStock]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[UpdateStock]
GO

	
--THIS PROCEDURE IS FOR INSERTING AND UPDATING RECORDS FOR SellInquiries TABLE

CREATE PROCEDURE [dbo].[UpdateStock]
	@DealerId		NUMERIC,	-- Dealer ID
	@CarVersionId		NUMERIC,	-- Car Version Id
	@CarRegNo		VARCHAR(15),	-- Car RegistrationNo 
	@StatusId		NUMERIC,	-- Car Status Id
	@EntryDate 		DATETIME,	-- Entry Date
	@Price			DECIMAL(9),	-- Car Price
	@MakeYear		DATETIME,	-- Car Make-Year
	@Kilometers		NUMERIC,	-- Mileage, Kilometers Done
	@Color			VARCHAR(30),	-- Car Color  
	@Comments		VARCHAR(500),	-- Dealer Comments
	@ImportChecksum	NUMERIC,	-- THE CHECKSUM, DEFAULT IS SET TO -1. TO BE USED WHILE IMPORTING DATA
	@Accessories		VARCHAR(2000),
	@StockCar		TINYINT,	-- Weather accessories are of Stocked Car or customer Sell Inquiry? 0 no 1 yes.

	-- sell inquiry details
	@Owners		VARCHAR(50),	-- No Of Owners
	@RegPlace		VARCHAR(50), 	-- Registration Place Id
	@OneTimeTax  		VARCHAR(50), 	-- OneTimeTax
	@Insurance		VARCHAR(50),	-- Insurance
	@InsExpiry		DateTime	-- Insurance Expiry
 AS
	DECLARE @StockId NUMERIC
	
		BEGIN
			UPDATE SellInquiries SET CarVersionId=@CarVersionId, 
			CarRegNo=@CarRegNo, LastUpdated=@EntryDate, StatusId=@StatusId,
			Price=@Price, MakeYear=@MakeYear, Kilometers=@Kilometers,Color=@Color, Comments=@Comments 
			WHERE ImportChecksum=@ImportChecksum AND DealerId=@DealerId

			SELECT @StockId=Id FROM SellInquiries WHERE ImportChecksum=@ImportChecksum AND DealerId=@DealerId

			DELETE FROM SellInquiriesDetails WHERE SellInquiryId=@StockId
				
			INSERT INTO SellInquiriesDetails( SellInquiryId, Owners, RegistrationPlace, OneTimeTax, Insurance, InsuranceExpiry) 
			VALUES(@StockId, @Owners, @RegPlace, @OneTimeTax, @Insurance, @InsExpiry)
			
			DELETE FROM SellInquiryAccessories WHERE CarId=@StockId AND StockCar=@StockCar			

			INSERT INTO SellInquiryAccessories(CarId,Accessories,StockCar)
			VALUES( @StockId, @Accessories, @StockCar )

		END
