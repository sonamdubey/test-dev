IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[DCRM].[AssignRegionOnCity]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [DCRM].[AssignRegionOnCity]
GO

	




-- Description	:	Assign Region on City
-- Author		:	Dilip V. 23-Jul-2012
-- Modifier		:	
CREATE PROCEDURE [DCRM].[AssignRegionOnCity]
	@RegionId	BIGINT,
	@CityId		BIGINT
AS
BEGIN
	SET NOCOUNT ON	
	
	IF NOT EXISTS (SELECT RegionId FROM DCRM_ADM_RegionCities WHERE RegionId = @RegionId AND CityId = @CityId )
		BEGIN
			INSERT INTO DCRM_ADM_RegionCities (RegionId,CityId) VALUES (@RegionId,@CityId)
		END
END






