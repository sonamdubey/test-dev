IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[InsertAwardVoting]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[InsertAwardVoting]
GO

	CREATE PROCEDURE [dbo].[InsertAwardVoting]
	@Id					NUMERIC,	-- Id. Will be -1 if Its Insertion
	@CustomerId			NUMERIC,
	@ModelId			NUMERIC,	
	@CategoryId			NUMERIC,
	@VotingDate			DATETIME,
	@FuelEconomy		SMALLINT,
	@Maintenance		SMALLINT,
	@Style				SMALLINT,
	@Performance		SMALLINT,
	@CarSpace			SMALLINT,
	@Comfort			SMALLINT,
	@Safety				SMALLINT,
	@Features			SMALLINT,
	@ASServices			SMALLINT,
	@ORCapabilities		SMALLINT,
	@Maneuverability	SMALLINT,
	@HWDriving			SMALLINT,
	@BrandValue			SMALLINT,
	@Description		VARCHAR(500),
	@CustomerName		VARCHAR(200),
	@CityId				NUMERIC,
	@MobileNo			VARCHAR(50),
	@LandLineNo			VARCHAR(50),
	@CustomerEmail		VARCHAR(100),
	@IPAddress			VARCHAR(50),
	@RecordID			NUMERIC OUTPUT	--return value, -1 for unsuccessfull attempt, and 0 for success
	
 AS
	
BEGIN
	IF @Id = -1 -- Insertion

		BEGIN
			IF @CustomerId = -1
				BEGIN
					INSERT INTO AW_Votes 
						( CustomerId, ModelId, CategoryId, VotingDate,
						  FuelEconomy, Maintenance, Style, Performance,
						  CarSpace, Comfort, Safety, Features,
						  ASServices, ORCapabilities, Maneuverability, BrandValue, Description,
						  CustomerName, CityId, MobileNo, LandLineNo, IPAddress, HWDriving
						)
					 VALUES
						( @CustomerId, @ModelId, @CategoryId, @VotingDate,
						  @FuelEconomy, @Maintenance, @Style, @Performance,
						  @CarSpace, @Comfort, @Safety, @Features,
						  @ASServices, @ORCapabilities, @Maneuverability, @BrandValue, @Description,
						  @CustomerName, @CityId, @MobileNo, @LandLineNo, @IPAddress, @HWDriving
						)
					SET @RecordID = SCOPE_IDENTITY() 
				END
			ELSE
				BEGIN
	                 INSERT INTO AW_Votes 
						( CustomerId, ModelId, CategoryId, VotingDate,
						  FuelEconomy, Maintenance, Style, Performance,
						  CarSpace, Comfort, Safety, Features,
						  ASServices, ORCapabilities, Maneuverability, BrandValue, Description, IPAddress, HWDriving
						)
					 VALUES
						( @CustomerId, @ModelId, @CategoryId, @VotingDate,
						  @FuelEconomy, @Maintenance, @Style, @Performance,
						  @CarSpace, @Comfort, @Safety, @Features,
						  @ASServices, @ORCapabilities, @Maneuverability, @BrandValue, @Description, @IPAddress, @HWDriving
						)
					SET @RecordID = SCOPE_IDENTITY() 
				END
		END
	ELSE
		BEGIN

			IF @CustomerId = -1
				BEGIN
					UPDATE AW_Votes SET ModelId = @ModelId, FuelEconomy = @FuelEconomy, Maintenance = @Maintenance, 
							Style = @Style, Performance = @Performance,
					  		CarSpace = @CarSpace, Comfort = @Comfort,
							Safety = @Safety, Features = @Features,
					 		ASServices = @ASServices, ORCapabilities = @ORCapabilities, 
							Maneuverability = @Maneuverability, BrandValue = @BrandValue, 
							Description = @Description, CustomerName = @CustomerName, 
							CityId = @CityId, MobileNo = @MobileNo, 
							LandLineNo = @LandLineNo, HWDriving = @HWDriving
					WHERE ID = @Id
						
					SET @RecordID = @Id
				END
			ELSE
				BEGIN
               		UPDATE AW_Votes SET CustomerId = @CustomerId, ModelId = @ModelId,
			  			FuelEconomy = @FuelEconomy, Maintenance = @Maintenance, 
						Style = @Style, Performance = @Performance,
			  			CarSpace = @CarSpace, Comfort = @Comfort, 
						Safety = @Safety, Features = @Features,
			  			ASServices = @ASServices, ORCapabilities = @ORCapabilities, 
						Maneuverability = @Maneuverability, BrandValue = @BrandValue, 
						Description = @Description, HWDriving = @HWDriving
					WHERE ID = @Id
						
					SET @RecordID = @Id
				END
		END
END


