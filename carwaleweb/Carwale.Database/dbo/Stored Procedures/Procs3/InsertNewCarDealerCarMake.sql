IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[InsertNewCarDealerCarMake]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[InsertNewCarDealerCarMake]
GO

	
--THIS PROCEDURE IS FOR INSERTING AND UPDATING RECORDS FOR SellInquiries TABLE

CREATE PROCEDURE [dbo].[InsertNewCarDealerCarMake]
	@DealerId		NUMERIC,	-- Dealer ID
	@CarMakeId		NUMERIC,	-- Car Make Id
	@IsAuthorised		BIT		
 AS
	
BEGIN
	
	INSERT INTO NewCarDealerCarMakes 
		(DealerId, CarMakeId, IsAuthorised, Status)
	VALUES
		(@DealerId, @CarMakeId, @IsAuthorised, 0 )
	
	
END
