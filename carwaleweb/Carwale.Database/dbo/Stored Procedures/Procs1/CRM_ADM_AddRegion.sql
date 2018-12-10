IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[CRM_ADM_AddRegion]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[CRM_ADM_AddRegion]
GO

	
--THIS PROCEDURE INSERTS THE VALUES FOR THE Cities

CREATE PROCEDURE [dbo].[CRM_ADM_AddRegion]
	@MakeId			NUMERIC,
	@Region			VARCHAR(50),
	@IsActive       BIT,
	@Status         BIT OUTPUT
 AS
	
BEGIN
	SELECT MakeId FROM OLM_Regions WITH (NOLOCK) WHERE MakeId = @MakeId AND Region=@Region

	IF @@ROWCOUNT = 0
		BEGIN
			INSERT INTO OLM_Regions(MakeId, Region, IsActive)			
			Values(@MakeId, @Region, @IsActive)	

			SET @Status = 1
		END
	ELSE
		
		SET @Status = 0
END
