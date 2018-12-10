IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[EntryRecommendCarSearchData]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[EntryRecommendCarSearchData]
GO

	
--THIS PROCEDURE IS FOR INSERTING recommended car preferences
--CustomerId, StartPrice, EndPrice, CityKm, HighwayKm, FuelEconomy, PreferredBrands, AC, PowerSteering, PowerWindows, CentralLocking, 
--AutomaticTransmission, ABS, SeatingCapacity, Petrol, Diesel, LPG, PreferredSegments, PreferredBodyStyles, Tall, RearSeat, WomenDriven, 
--HighSpeed, EntryDateTime
CREATE PROCEDURE [dbo].[EntryRecommendCarSearchData]
	@CustomerId			AS NUMERIC, 
	@StartPrice 			AS NUMERIC,
	@EndPrice 			AS NUMERIC,
	@CityKm 			AS NUMERIC,
	@HighwayKm 			AS NUMERIC,
	@FuelEconomy 			AS SMALLINT,			
	@PreferredBrands 		AS VARCHAR(500),
	@AC 				AS BIT,
	@PowerSteering 		AS BIT,
	@PowerWindows 		AS BIT,
	@CentralLocking		AS BIT,
	@AutomaticTransmission		AS BIT,
	@ABS 				AS BIT,
	@SeatingCapacity		AS SMALLINT,
	@Petrol 			AS BIT,
	@Diesel 			AS BIT,
	@LPG 				AS BIT,
	@MaintenanceCost		AS SMALLINT,			
	@PreferredSegments		AS VARCHAR(200),
	@PreferredBodyStyles		AS VARCHAR(200),
	@Tall 				AS BIT,
	@RearSeat 			AS BIT,
	@WomenDriven			AS BIT,
	@HighSpeed			AS BIT,
	@EntryDateTime		AS DATETIME,
	@IPAdress			AS VARCHAR(200),
	@NewCar			AS BIT,
	@Safety			AS SMALLINT,
	@Comfort			AS SMALLINT,
	@Performance			AS SMALLINT,
	@Resale			AS SMALLINT,
	@VFM				AS SMALLINT,
	@ID				AS NUMERIC OUTPUT
 AS
	
BEGIN
	
	
		INSERT INTO RecommendCarSearchData
			(
				CustomerId, 		StartPrice, 		EndPrice, 		CityKm, 			HighwayKm, 	
				FuelEconomy, 		PreferredBrands, 	AC, 			PowerSteering, 		PowerWindows, 	
				CentralLocking, 		AutomaticTransmission, 	ABS, 			SeatingCapacity, 	Petrol, 	
				Diesel, 			LPG, 			MaintenanceCost, 	PreferredSegments, 	PreferredBodyStyles, 
				Tall, 			RearSeat, 		WomenDriven, 		HighSpeed, 		IPAdress, 
				EntryDateTime,		NewCar,		Safety,			Comfort,			Performance,
				Resale, 			VFM		
			) 
		VALUES
			(
				@CustomerId, 		@StartPrice, 		@EndPrice, 		@CityKm, 		@HighwayKm, 	
				@FuelEconomy, 	@PreferredBrands, 	@AC, 			@PowerSteering, 	@PowerWindows, 	
				@CentralLocking, 	@AutomaticTransmission, @ABS, 		@SeatingCapacity, 	@Petrol, 	
				@Diesel, 		@LPG, 			@MaintenanceCost, 	@PreferredSegments, 	@PreferredBodyStyles, 
				@Tall, 			@RearSeat, 		@WomenDriven, 	@HighSpeed, 		@IPAdress, 
				@EntryDateTime,	@NewCar,		@Safety,		@Comfort,		@Performance,
				@Resale, 		@VFM	
			)

		SET @ID = SCOPE_IDENTITY()  		
		
END