IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[AE_SaveCarCondition]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[AE_SaveCarCondition]
GO

	

--CREATED ON 05 Nov 2009 BY SENTIL          
--PROCEDURE FOR Auction Engine Car Condition INSERT and Update

CREATE PROCEDURE [dbo].[AE_SaveCarCondition]
(
	@CarId AS NUMERIC =0,
	@Interior AS SMALLINT =0,
	@Exterior AS SMALLINT =0,
	@Engine AS SMALLINT =0,
	@Tyre AS SMALLINT =0,
	@Transmission AS SMALLINT =0,
	@Electrical AS SMALLINT =0,
	@Seat AS SMALLINT =0,
	@Brakes AS SMALLINT =0,
	@Suspension AS SMALLINT =0,
	@Steering AS SMALLINT =0,
	@UpdatedOn AS DATETIME =NULL,
	@UpdatedBy AS NUMERIC =0
)	
AS
BEGIN

	UPDATE AE_CarCondition 
	SET Interior = @Interior, Exterior = @Exterior, Engine = @Engine, Tyre = @Tyre,
		Transmission = @Transmission, Electrical = @Electrical, Seat = @Seat, Brakes = @Brakes, 
		Suspension = @Suspension, Steering = @Steering, UpdatedOn = @UpdatedOn, UpdatedBy = @UpdatedBy 
	WHERE carId = @CarId
				
	 IF @@ROWCOUNT = 0
		BEGIN

			INSERT INTO AE_CarCondition
			(
				CarId, Interior, Exterior, Engine, Tyre, Transmission, Electrical, Seat, 
				Brakes, Suspension, Steering, UpdatedOn, UpdatedBy
			)
			VALUES
			( 
				@CarId, @Interior, @Exterior, @Engine, @Tyre, @Transmission, @Electrical, @Seat, 
				@Brakes, @Suspension, @Steering, @UpdatedOn, @UpdatedBy
			)
		END
	  	
END

