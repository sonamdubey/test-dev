IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[DCRM_SaveRegion]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[DCRM_SaveRegion]
GO

	
CREATE PROCEDURE [dbo].[DCRM_SaveRegion]
	@Id				NUMERIC,
	@RegionName		VARCHAR(30),
	@IsActive       BIT,    
	@UpdatedOn		DATETIME,
	@UpdatedBy		NUMERIC,
	@Status			INT OUTPUT 
AS

BEGIN
 IF @ID = -1 
	IF EXISTS(SELECT ID from DCRM_ADM_Regions      
	WHERE Name = @RegionName)      
		BEGIN      
		 SET @Status = 0      
		END   
	ELSE
		BEGIN
			INSERT INTO DCRM_ADM_Regions
			(
				Name, IsActive,	UpdatedOn, UpdatedBy
			) 
			VALUES
			( 
				@RegionName, @IsActive, @UpdatedOn, @UpdatedBy
			)
		
			SET @Status = 1 
		END	
	
 ELSE 
	BEGIN
		IF EXISTS (SELECT ID from DCRM_ADM_Regions      
		WHERE Name = @RegionName AND ID <> @Id)      
			BEGIN      
			 SET @Status = 0      
			END      
		ELSE 
			BEGIN
				UPDATE DCRM_ADM_Regions 
				SET UpdatedOn = @UpdatedOn, UpdatedBy = @UpdatedBy, 
					Name = @RegionName, IsActive = @IsActive
				WHERE Id  = @Id 
				
				SET @Status = 1 
			END
	END	
	
END