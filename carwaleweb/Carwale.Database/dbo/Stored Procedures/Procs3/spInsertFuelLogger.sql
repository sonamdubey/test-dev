IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[spInsertFuelLogger]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[spInsertFuelLogger]
GO

	

CREATE PROCEDURE [dbo].[spInsertFuelLogger]    
@MyGarageId	NUMERIC = 0,
@OdometerReading numeric,    
@FilledOn datetime,    
@Quantity decimal(5, 2),    
@TotalCost decimal(9, 2),    
@FuelPumpName varchar(100),    
@FullTank bit,
@Action varchar(100),
@Id numeric = 0    
AS    
BEGIN    
 
 IF @Action = 'Insert' 
 BEGIN	   
	 INSERT INTO MyGarageFuelLogger    
	 (MyGarageId,OdometerReading,FilledOn,Quantity,TotalCost,FuelPumpName,FullTank,CreatedOn)    
	 VALUES    
	 (@MyGarageId,@OdometerReading,@FilledOn,@Quantity,@TotalCost,@FuelPumpName,@FullTank,getdate())    
 END	

 IF @Action = 'Edit' 
 BEGIN	
	UPDATE MyGarageFuelLogger
	SET		
		OdometerReading = @OdometerReading,
		FilledOn = @FilledOn,
		Quantity = @Quantity,
		TotalCost = @TotalCost,
		FuelPumpName = @FuelPumpName,
		FullTank = @FullTank
	WHERE
		Id = @Id
 END    
END    

