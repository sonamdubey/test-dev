IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[NCS_AddCities]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[NCS_AddCities]
GO

	
--THIS PROCEDURE INSERTS THE VALUES FOR THE Cities

CREATE PROCEDURE [dbo].[NCS_AddCities]
	@CityId			NUMERIC,
	@Status         BIT OUTPUT
 AS
	
BEGIN
	SELECT CityId FROM NCS_Cities WHERE CityId = @CityId

	IF @@ROWCOUNT = 0
		BEGIN
			INSERT INTO NCS_Cities( CityId, IsActive )			
			Values( @CityId, 1)	

			SET @Status = 1
		END
	ELSE
		
		SET @Status = 0
END
