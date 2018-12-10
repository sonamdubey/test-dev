IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[Classified_GetCarStdFeatures]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[Classified_GetCarStdFeatures]
GO

	  
CREATE PROCEDURE [dbo].[Classified_GetCarStdFeatures]    
 @VersionId AS NUMERIC(18,0),  
 @Safety AS VARCHAR(500) OUT,  
 @Comfort AS VARCHAR(500) OUT,  
 @Others AS VARCHAR(500) OUT  
AS  
BEGIN  
  
DECLARE @PowerWindows AS VARCHAR(1),  
  @PowerDoorLocks AS VARCHAR(1),  
  @PowerSteering AS VARCHAR(1),  
  @AirConditioner AS VARCHAR(1),  
  @ABS AS VARCHAR(1),  
  @SteeringAdjustment AS VARCHAR(1),  
  @Tachometer AS VARCHAR(1),  
  @ChildSafetyLocks AS VARCHAR(1),  
  @FrontFogLights AS VARCHAR(1),  
  @Defogger AS VARCHAR(1),  
  @LeatherSeats AS VARCHAR(1),  
  @PowerSeats AS VARCHAR(1),  
  @Radio AS VARCHAR(1),  
  @CDPlayer AS VARCHAR(1),  
  @SunRoof AS VARCHAR(1),  
  @TractionControl AS VARCHAR(1),  
  @Immobilizer AS VARCHAR(1),  
  @DriverAirBags AS VARCHAR(1),  
  @PassengerAirBags AS VARCHAR(1),  
  @RemoteBootFuelLid AS VARCHAR(1),  
  @CupHolder AS VARCHAR(1),  
  @SplitFoldingRearSeats AS VARCHAR(1),  
  @RearWashWiper AS VARCHAR(1),  
  @CentralLocking AS VARCHAR(1),  
  @AlloyWheels AS VARCHAR(1),  
  @TubelessTyres AS VARCHAR(1)   
  
SELECT  @PowerWindows   = PowerWindows,  
  @PowerDoorLocks   = PowerDoorLocks,  
  @PowerSteering   = PowerSteering,  
  @AirConditioner   = AirConditioner,  
  @ABS     = [ABS],  
  @SteeringAdjustment  = SteeringAdjustment,  
  @Tachometer    = Tachometer,  
  @ChildSafetyLocks  = ChildSafetyLocks,  
  @FrontFogLights   = FrontFogLights,  
  @Defogger    = Defogger,  
  @LeatherSeats   = LeatherSeats,  
  @PowerSeats    = PowerSeats,   
  @Radio     = Radio,   
  @CDPlayer    = CDPlayer,  
  @SunRoof    = SunRoof,  
  @TractionControl  = TractionControl,   
  @Immobilizer   = Immobilizer,  
  @DriverAirBags   = DriverAirBags,   
  @PassengerAirBags  = PassengerAirBags,  
  @RemoteBootFuelLid  = RemoteBootFuelLid,  
  @CupHolder    = CupHolder,  
  @SplitFoldingRearSeats = SplitFoldingRearSeats,  
  @RearWashWiper   = RearWashWiper,  
  @CentralLocking   = CentralLocking,  
  @AlloyWheels   = AlloyWheels,  
  @TubelessTyres   = TubelessTyres  
    
FROM NewCarStandardFeatures WHERE CarVersionId = @VersionId  
  
 SET @Comfort = ''  
 SET @Safety = ''  
 SET @Others = ''  
    
 IF (@AirConditioner = 'A')   SET @Comfort += 'Air Conditioning|'     
 IF (@PowerWindows = 'A')   SET @Comfort += 'Power Windows|'     
 IF (@PowerDoorLocks = 'A')   SET @Comfort += 'Power Door Locks|'     
 IF (@PowerSteering = 'A')   SET @Comfort += 'Power Steering|'    
 IF (@PowerSeats = 'A')    SET @Comfort += 'Power Seats|'     
 IF (@SteeringAdjustment = 'A')   SET @Comfort += 'Steering Adjustment|'   
 IF (@CentralLocking = 'A')   SET @Comfort += 'Central Locking|'   
 IF (@Defogger = 'A')    SET @Comfort += 'Defogger|'   
 IF (@RemoteBootFuelLid = 'A')  SET @Comfort += 'Remote Boot/Fuel-Lid|'   
   
 IF (@DriverAirBags = 'A')   SET @Safety += 'Driver Air Bag|'    
 IF (@PassengerAirBags = 'A')  SET @Safety += 'Passenger Air Bags|'   
 IF (@Immobilizer = 'A')    SET @Safety += 'Immobilizer|'    
 IF (@TractionControl = 'A')   SET @Safety += 'Traction Control|'   
 IF (@ChildSafetyLocks = 'A')  SET @Safety += 'Child Safety Locks|'    
 IF (@ABS = 'A')      SET @Safety += 'Anti-Lock Brakes|'   
   
 IF (@SplitFoldingRearSeats = 'A') SET @Others += 'Folding Rear Seats|'    
 IF (@CupHolder = 'A')    SET @Others += 'Cup Holder|'    
 IF (@LeatherSeats = 'A')   SET @Others += 'Leather Seats|'    
 IF (@Radio = 'A')     SET @Others += 'Radio|'    
 IF (@CDPlayer = 'A')    SET @Others += 'CD Player|'    
 IF (@Tachometer = 'A')    SET @Others += 'Tachometer|'    
   
 IF (@AlloyWheels = 'A')    SET @Others += 'Alloy Wheels|'    
 IF (@TubelessTyres = 'A')   SET @Others += 'Tubeless Tyres|'    
 IF (@SunRoof = 'A')     SET @Others += 'Sun Roof|'    
 IF (@FrontFogLights = 'A')   SET @Others += 'Fog Lights|'    
 IF (@RearWashWiper = 'A')   SET @Others += 'Rear Wash Wiper|'    
   
 SET @Comfort = SUBSTRING(@Comfort,0,Len(@Comfort))  
 SET @Safety = SUBSTRING(@Safety,0,Len(@Safety))  
 SET @Others = SUBSTRING(@Others,0,Len(@Others))  
   
END  
