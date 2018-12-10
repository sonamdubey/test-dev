IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[InsertValuationRequest]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[InsertValuationRequest]
GO

	--THIS PROCEDURE IS FOR INSERTING AND UPDATING RECORDS FOR SellInquiries TABLE

CREATE PROCEDURE [dbo].[InsertValuationRequest]
	@CarVersionId		NUMERIC,	-- Car Version Id
	@CarYear		DATETIME,	-- Car Year
	@CarKms		NUMERIC,	-- Car Mileage
	@CustomerId		NUMERIC,	-- CustomerId
	@City			VARCHAR(100),	-- City
	@CityId			NUMERIC,	-- CityId
	@ActualCityId		NUMERIC,	-- ActualCityId
	@RequestDateTime	DATETIME,	-- Entry Date
	@RemoteHost		VARCHAR(100),
	@RequestSource	INT,		-- 1 or 2
	@ValuationId		NUMERIC OUTPUT -- Valuation Id
	
 AS

BEGIN
	INSERT INTO CarValuations(CarVersionId, CarYear, CustomerId, City, RequestDateTime, RemoteHost, RequestSource, Kms, CityId, ActualCityId )
	VALUES (@CarVersionId, @CarYear, @CustomerId, @City, @RequestDateTime, @RemoteHost, @RequestSource, @CarKms, @CityId, @ActualCityId)

	SET @ValuationId = SCOPE_IDENTITY()  	
		
END