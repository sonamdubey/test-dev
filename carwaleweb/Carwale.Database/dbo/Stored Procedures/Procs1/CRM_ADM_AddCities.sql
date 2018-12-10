IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[CRM_ADM_AddCities]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[CRM_ADM_AddCities]
GO

	
--THIS PROCEDURE INSERTS THE VALUES FOR THE Cities

CREATE PROCEDURE [dbo].[CRM_ADM_AddCities]
	@CityId			NUMERIC,
	@Status         BIT OUTPUT
 AS
	
BEGIN
	SELECT CityId FROM CRM_ADM_OperationalCities WITH (NOLOCK) WHERE CityId = @CityId

	IF @@ROWCOUNT = 0
		BEGIN
			INSERT INTO CRM_ADM_OperationalCities(CityId)			
			Values(@CityId)	

			SET @Status = 1
		END
	ELSE
		
		SET @Status = 0
END
