IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[Impact_CampaignActivate]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[Impact_CampaignActivate]
GO

	

-- =============================================
-- Author      : Vinay Kumar Prajapati
-- Create date : 19th Sept 2014
-- Description : Activate  Campaign which is Inactive state
--             : PackageTypeId =64 For Banner Ad And 65 For Multiple dealer
--Modified By  : Vinay Kumar 25th sept 2014 rectify logic
-- =============================================

CREATE PROCEDURE [dbo].[Impact_CampaignActivate]
    (
	@ImpactCampaignId    INT,
	@MakeId              INT,
	@CityId              INT,
	@DealerId            INT,
	@PackageId           INT,
	@IsActive            BIT,
	@StartDate           DATETIME,
	@EndDate             DATETIME,
	@UpdatedBy		     NUMERIC
	)
 AS

	BEGIN			
		-- For Banner Ad
		IF @PackageId = 64   
			BEGIN
			    SELECT IPS.DealerId FROM Impact_Slot AS IPS WITH(NOLOCK)
				WHERE IPS.MakeId=@MakeId AND IPS.CityId = @CityId AND  IPS.PackageTypeId=64 AND IPS.IsActive = 1 
			
				IF @@ROWCOUNT = 0
					BEGIN
						--Check if there is any 65 package type for same city and make
						SELECT PS.Impact_CampaignId FROM Impact_Slot AS PS WITH(NOLOCK)
						WHERE PS.MakeId=@MakeId AND PS.CityId = @CityId AND PS.PackageTypeId=65 AND IsActive = 1
									
						IF @@ROWCOUNT <> 0  --slot exists
							BEGIN
								-- Deactivate Active Campaign and log it
								UPDATE Impact_Campaign SET IsActive = 0, EndDate = GETDATE(), CreatedBy = @UpdatedBy, CreatedDate = GETDATE()
								WHERE MakeId=@MakeId AND CityId = @CityId AND PackageTypeId=65 AND IsActive = 1
								
								--Log This Data
								INSERT INTO Impact_CampaignLog 
								SELECT Impact_CampaignId, MakeId,CityId,DealerId,PackageTypeId,StartDate,EndDate,IsActive,GETDATE(),@UpdatedBy 
								FROM Impact_Campaign AS PS WITH(NOLOCK) WHERE PS.MakeId=@MakeId AND PS.CityId = @CityId AND PS.PackageTypeId=65 AND IsActive = 1
							
								--Empty The Slot
								DELETE FROM  Impact_Slot WHERE  MakeId=@MakeId AND  Cityid=@CityId  AND PackageTypeId=65
							END


						UPDATE Impact_Campaign SET StartDate=@StartDate,EndDate=@EndDate,IsActive=@IsActive				
						WHERE Impact_CampaignId=@ImpactCampaignId
										
						--Log the Data 
						INSERT INTO Impact_CampaignLog( Impact_CampaignId,MakeId,CityId,DealerId,PackageTypeId,StartDate,EndDate,IsActive,LogDate,LoggedBy)
						VALUES(@ImpactCampaignId,@MakeId,@CityId,@DealerId,@PackageId,@StartDate,@EndDate,1,GETDATE(),@UpdatedBy)		
						
						--Create Slot 
						INSERT INTO Impact_Slot(MakeId,CityId,DealerId,PackageTypeId,Impact_CampaignId,IsActive)
					    VALUES(@MakeId,@CityId,@DealerId,@PackageId,@ImpactCampaignId,1)			
                END

			END	
		ELSE   -- For MultiText Dealer 
			BEGIN 
			     --Check if there is any 64 package type for same city and make
				SELECT PS.Impact_CampaignId FROM Impact_Slot AS PS WITH(NOLOCK)
				WHERE PS.MakeId=@MakeId AND PS.CityId = @CityId AND PS.PackageTypeId=64 AND IsActive = 1
									
				IF @@ROWCOUNT <> 0  --slot exists
					BEGIN
						-- Deactivate Active Campaign and log it
						UPDATE Impact_Campaign SET IsActive = 0, EndDate = GETDATE(), CreatedBy = @UpdatedBy, CreatedDate = GETDATE()
						WHERE MakeId=@MakeId AND CityId = @CityId AND PackageTypeId=64 AND IsActive = 1
								
						--Log This Data
						INSERT INTO Impact_CampaignLog 
						SELECT Impact_CampaignId, MakeId,CityId,DealerId,PackageTypeId,StartDate,EndDate,IsActive,GETDATE(),@UpdatedBy 
						FROM Impact_Campaign AS PS WITH(NOLOCK) WHERE PS.MakeId=@MakeId AND PS.CityId = @CityId AND PS.PackageTypeId=64 AND IsActive = 1
							
						--Empty The Slot
						DELETE FROM  Impact_Slot WHERE  MakeId=@MakeId AND  Cityid=@CityId  AND PackageTypeId=64
					END
      
				-- Avoid Duplicate entry
				SELECT IPS.Impact_SlotId FROM Impact_Slot AS IPS WITH(NOLOCK) 
				WHERE IPS.MakeId=@MakeId AND IPS.CityId = @CityId  AND IPS.PackageTypeId=65 AND IPS.IsActive=1
				IF @@ROWCOUNT < 3 					
					BEGIN
					    -- Avoid duplicate entry.
						SELECT PS.Impact_SlotId FROM Impact_Slot AS PS WITH(NOLOCK)
						WHERE PS.MakeId=@MakeId AND PS.CityId = @CityId  AND PS.DealerId=@DealerId
										
						IF @@ROWCOUNT = 0 -- If Not available
							BEGIN										   
								-- Row Update
								UPDATE Impact_Campaign SET StartDate=@StartDate,EndDate=@EndDate,IsActive=@IsActive
								WHERE Impact_CampaignId=@ImpactCampaignId

								-- Make Current  Running Slot (Create Slot )
								INSERT INTO Impact_Slot(MakeId,CityId,DealerId,PackageTypeId,Impact_CampaignId,IsActive)
								VALUES(@MakeId,@CityId,@DealerId,@PackageId,@ImpactCampaignId,1)
								-- Log the Data 
								INSERT INTO Impact_CampaignLog( Impact_CampaignId,MakeId,CityId,DealerId,PackageTypeId,StartDate,EndDate,IsActive,LogDate,LoggedBy)
								VALUES(@ImpactCampaignId,@MakeId,@CityId,@DealerId,@PackageId,@StartDate,@EndDate,1,GETDATE(),@UpdatedBy)
							END												     
				END
		END				
 END
