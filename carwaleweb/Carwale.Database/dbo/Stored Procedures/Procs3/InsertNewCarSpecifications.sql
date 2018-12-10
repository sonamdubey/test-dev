IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[InsertNewCarSpecifications]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[InsertNewCarSpecifications]
GO

	--THIS PROCEDURE IS FOR INSERTING AND UPDATING RECORDS FOR NewCarSpecifications TABLE

CREATE PROCEDURE [dbo].[InsertNewCarSpecifications]
	@CarVersionId		NUMERIC,	-- CarVersionId. Will be -1 if Its Insertion
	@Length		NUMERIC,
	@Width			NUMERIC,
	@Height		NUMERIC,
	@WheelBase		NUMERIC,
	@GroundClearance	NUMERIC,
	@FrontTrack		NUMERIC,
	@RearTrack		NUMERIC,
	@FrontHeadRoom	NUMERIC,
	@FrontLegRoom	NUMERIC,
	@RearLegRoom	NUMERIC,
	@Bootspace		NUMERIC,
	@GrossWeight		NUMERIC,
	@KerbWeight		NUMERIC,
	@SeatCapacity		NUMERIC,
	@FuelTankCapacity	NUMERIC,
	@Doors			NUMERIC,
	@MileageHighway	FLOAT,
	@MileageCity		FLOAT,
	@MileageOverall	FLOAT,
	@EngineType		VARCHAR(200),
	@Displacement		NUMERIC,
	@Power		VARCHAR(50),
	@Torque		VARCHAR(50),
	@ValueMechanism	VARCHAR(200),
	@Bore			FLOAT,
	@Stroke		FLOAT,
	@CompressionRatio	FLOAT,
	@NoOfCylinders		NUMERIC,
	@CylinderConfiguration	VARCHAR(200),
	@ValvesPerCylinder	NUMERIC,
	@IgnitionType		VARCHAR(200),
	@BlockMaterial		VARCHAR(50),
	@HeadMaterial		VARCHAR(50),
	@FuelType		VARCHAR(50),
	@FuelSystem		VARCHAR(200),
	@TransmissionType	VARCHAR(50),
	@Drive			VARCHAR(50),
	@Speeds		NUMERIC,
	@MaxSpeed		NUMERIC,
	@ClutchType		VARCHAR(50),
	@GearReductionRatio	FLOAT,
	@FrontSuspension	VARCHAR(200),
	@RearSuspension	VARCHAR(200),
	@SteeringType		VARCHAR(200),
	@PowerAssisted	VARCHAR(50),
	@MinTurningRadius	FLOAT,
	@BrakeType		VARCHAR(200),
	@FrontBrakes		VARCHAR(200),
	@RearBrakes		VARCHAR(200),
	@WheelType		VARCHAR(200),
	@WheelSize		VARCHAR(50),
	@Tyres			VARCHAR(200),
	@RearShoulder		NUMERIC, 		-- Added 17th Feb
	@ZeroToHundred	FLOAT, 		-- Added 17th Feb
	@QuarterMile		FLOAT,			-- Added 17th Feb
	@BrakingHundredToZero	FLOAT, 	-- Added 17th Feb
	@BrakingEightyToZero		FLOAT,	 	-- Added 17th Feb
	@FrontWheelDiameter		NUMERIC, 		-- Added 17th Nov
	@FrontTyreProfile		NUMERIC, 		-- Added 17th Nov
	@FrontWheelSectionWidth	NUMERIC, 		-- Added 17th Nov
	@RearWheelDiameter		NUMERIC, 		-- Added 17th Nov
	@RearTyreProfile		NUMERIC, 		-- Added 17th Nov
	@RearWheelSectionWidth	NUMERIC 		-- Added 17th Nov
 AS

BEGIN
	DECLARE @Power_BHP AS Decimal(18,2)
	SET @Power_BHP = 0
	
	IF @Power <> '@'
	BEGIN
		SET @Power_BHP = Convert(decimal(18,2), SUBSTRING(@Power, 0, CHARINDEX('@',@Power)))
	END
	
	DELETE FROM NewCarSpecifications WHERE CarVersionId=@CarVersionId
	
	BEGIN
		INSERT INTO NewCarSpecifications
		(CarVersionId, Length, Width, Height, Wheelbase, GroundClearance, FrontTrack,
		 RearTrack, FrontHeadRoom, FrontLegRoom, RearLegRoom, Bootspace, GrossWeight, KerbWeight,
		 SeatingCapacity, FuelTankCapacity, Doors, 
		 MileageHighway, MileageCity, MileageOverall,
		 EngineType, Displacement, Power, Torque,
		 ValueMechanism, Bore, Stroke, CompressionRatio, NoOfCylinders, CylinderConfiguration,
		 ValvesPerCylinder, IgnitionType, EngineBlockMaterial, BlockHeadMaterial, FuelType, FuelSystem,
		 TransmissionType,Drive, Speeds, MaxSpeed, ClutchType,FinalGearReductionRatio,
		 SuspensionFront, SuspensionRear, SteeringType, PowerAssisted, MinimumTurningRadius,
		 BrakesType, BrakesFront, BrakesRear, WheelType, WheelSize, Tyres,
		 RearShoulder,ZeroToHundred,QuarterMile,BrakingHundredToZero,BrakingEightyToZero,
		 FrontWheelDiameter,FrontTyreProfile,FrontWheelSectionWidth,RearWheelDiameter,
		 RearTyreProfile,RearWheelSectionWidth, Power_BHP)
		VALUES
		(@CarVersionId, @Length, @Width, @Height, @Wheelbase, @GroundClearance, @FrontTrack,
		 @RearTrack, @FrontHeadRoom, @FrontLegRoom, @RearLegRoom, @Bootspace, @GrossWeight, @KerbWeight,
		 @SeatCapacity, @FuelTankCapacity, @Doors, 
		 @MileageHighway, @MileageCity, @MileageOverall,	
		 @EngineType, @Displacement, @Power, @Torque,
		 @ValueMechanism, @Bore, @Stroke, @CompressionRatio, @NoOfCylinders, @CylinderConfiguration,
		 @ValvesPerCylinder, @IgnitionType, @BlockMaterial, @HeadMaterial, @FuelType, @FuelSystem,
		 @TransmissionType, @Drive,@Speeds, @MaxSpeed, @ClutchType, @GearReductionRatio,
		 @FrontSuspension, @RearSuspension, @SteeringType, @PowerAssisted, @MinTurningRadius,
		 @BrakeType, @FrontBrakes, @RearBrakes, @WheelType, @WheelSize, @Tyres,
		 @RearShoulder,@ZeroToHundred,@QuarterMile,@BrakingHundredToZero,@BrakingEightyToZero,
		 @FrontWheelDiameter,@FrontTyreProfile,@FrontWheelSectionWidth,@RearWheelDiameter,
		 @RearTyreProfile,@RearWheelSectionWidth, @Power_BHP)

	END
END
