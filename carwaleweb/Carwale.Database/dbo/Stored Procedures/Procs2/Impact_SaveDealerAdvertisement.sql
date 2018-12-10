IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[Impact_SaveDealerAdvertisement]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[Impact_SaveDealerAdvertisement]
GO

	

-- =============================================
-- Author      : Vinay Kumar Prajapati
-- Create date : 19th Sept 2014
-- Description : Save Dealer Advertisement Details
--             : PackageTypeId =64 For Banner Ad And 65 For Multiple dealer
-- =============================================

CREATE PROCEDURE [dbo].[Impact_SaveDealerAdvertisement]
    (
	@MakeId              INT,
	@CityId              INT,
	@DealerId            INT,
	@PackageTypeId       INT,
	@StartDate           DATETIME,
	@EndDate             DATETIME,
	@CreatedBy		     NUMERIC,
	@CreatedOn	         DATETIME  ,
	@Status			     BIT OUTPUT,
	@CampaignId			 INT OUTPUT,
	@StatusMsg			 VARCHAR(250) OUTPUT
	)
 AS

	DECLARE @ID INT

	BEGIN
		-- PackageTypeId =64 For Banner Ad And 65 For MultiText
		
		-- For Banner Ad
		IF @PackageTypeId = 64  
			BEGIN 
				-- Check whether there is any running/paused with greater end date, campaing for this package type in the selected city and make
				SELECT IC.DealerId FROM Impact_Campaign AS IC WITH(NOLOCK)
				WHERE IC.MakeId=@MakeId AND IC.CityId = @CityId AND  IC.PackageTypeId=64 
					AND (IsActive = 1 OR (IsActive = 0 AND EndDate >= GETDATE()))
			
				IF @@ROWCOUNT <> 0
					BEGIN		
						--If yes return without any entry						
						SET @Status=0
						SET @StatusMsg = 'There is already a campign in running/Paused Status for selected make-city.'
						SET @CampaignId=-1
					END	
				ELSE
					BEGIN
						--If not 
						SET @StatusMsg = ''
						
						--Check if there is any 65 package type for same city and make
						SELECT PS.Impact_CampaignId FROM Impact_Slot AS PS WITH(NOLOCK)
						WHERE PS.MakeId=@MakeId AND PS.CityId = @CityId AND PS.PackageTypeId=65 AND IsActive = 1
									
						IF @@ROWCOUNT <> 0  --slot exists
							BEGIN
								
								--Log This Data
								INSERT INTO Impact_CampaignLog 
								SELECT Impact_CampaignId, MakeId,CityId,DealerId,PackageTypeId,StartDate,GETDATE(),0,GETDATE(),@CreatedBy 
								FROM Impact_Campaign AS PS WITH(NOLOCK) WHERE PS.MakeId=@MakeId AND PS.CityId = @CityId AND PS.PackageTypeId=65 AND IsActive = 1
								
								-- Deactivate Active Campaign and log it
								UPDATE Impact_Campaign SET IsActive = 0, EndDate = GETDATE(), CreatedBy = @CreatedBy, CreatedDate = GETDATE()
								WHERE MakeId=@MakeId AND CityId = @CityId AND PackageTypeId=65 AND IsActive = 1
							
								--Empty The Slot
								DELETE FROM  Impact_Slot WHERE  MakeId=@MakeId AND  Cityid=@CityId  AND PackageTypeId=65
							END
							
							--Create Campaign
							INSERT INTO Impact_Campaign(MakeId,CityId,DealerId,PackageTypeId,StartDate,EndDate,IsActive,CreatedDate,CreatedBy)
							VALUES(@MakeId,@CityId,@DealerId,@PackageTypeId,@StartDate,@EndDate,1,@CreatedOn,@CreatedBy)

							SET @ID= SCOPE_IDENTITY()
							--Log Campaign
							INSERT INTO Impact_CampaignLog( Impact_CampaignId,MakeId,CityId,DealerId,PackageTypeId,StartDate,EndDate,IsActive,LogDate,LoggedBy)
							VALUES(@ID,@MakeId,@CityId,@DealerId,@PackageTypeId,@StartDate,@EndDate,1,GETDATE(),@CreatedBy)		
							
							--Create Slot 							
							INSERT INTO Impact_Slot(MakeId,CityId,DealerId,PackageTypeId,Impact_CampaignId,IsActive)
							VALUES(@MakeId,@CityId,@DealerId,@PackageTypeId,@ID,1)
							
							SET  @Status=1	
						    SET @CampaignId = @ID   --  this id is use to save image.
					END
			END
		ELSE   -- For MultiText Dealer 
			BEGIN 
			    --set campaign for return   
				SET @CampaignId=-1

				-- Check whether there is any running/paused with greater end date, campaing for this package type in the selected city and make
				SELECT IC.DealerId FROM Impact_Campaign AS IC WITH(NOLOCK)
				WHERE IC.MakeId=@MakeId AND IC.CityId = @CityId AND  IC.PackageTypeId=65 AND IC.DealerId=@DealerId
				AND (IsActive = 1 OR (IsActive = 0 AND EndDate >= GETDATE()))

				IF @@ROWCOUNT <> 0
					BEGIN				
						--If yes return without any entry				
						SET @Status=0
						SET @StatusMsg = 'There is already a campign in running/Paused Status for selected make-city-dealer.'
					END	
				ELSE
					BEGIN
						SET @StatusMsg = ''
						
						 -- Check to Avoid more than 3 slot for multiple
						 SELECT IPC.Impact_CampaignId FROM Impact_Campaign AS IPC WITH(NOLOCK) 
						 WHERE IPC.MakeId=@MakeId AND IPC.CityId = @CityId  AND IPC.PackageTypeId=65 AND IPC.IsActive = 1
						 IF @@ROWCOUNT >= 3 
							 BEGIN
								--If yes return without any entry no slot is avilable
								SET  @Status=0
								SET @StatusMsg = 'There is already three dealer campigns in running/Paused Status for selected make-city.'
							 END
						 ELSE
							BEGIN
								--If yes
								----Check if there is any 64 package type for same city and make
								SELECT PS.Impact_SlotId FROM Impact_Slot AS PS WITH(NOLOCK)
								WHERE PS.MakeId=@MakeId AND PS.CityId = @CityId  AND PS.PackageTypeId=64 AND IsActive = 1

								IF @@ROWCOUNT <> 0
								   BEGIN	
										--slot exists
										--Log This Data
										INSERT INTO Impact_CampaignLog 
										SELECT Impact_CampaignId, MakeId,CityId,DealerId,PackageTypeId,StartDate,GETDATE(),0,GETDATE(),@CreatedBy 
										FROM Impact_Campaign AS PS WITH(NOLOCK) WHERE PS.MakeId=@MakeId AND PS.CityId = @CityId AND PS.PackageTypeId=64 AND IsActive = 1
										
										-- Deactivate Active Campaign and log it
										UPDATE Impact_Campaign SET IsActive = 0, EndDate = GETDATE(), CreatedBy = @CreatedBy, CreatedDate = GETDATE()
										WHERE MakeId=@MakeId AND CityId = @CityId AND PackageTypeId=64 AND IsActive = 1
																		
										--Empty Slot			       
										DELETE FROM  Impact_Slot WHERE  MakeId=@MakeId AND  Cityid=@CityId AND PackageTypeId=64	
								   END

									-- Row Insert
									INSERT INTO Impact_Campaign(MakeId,CityId,DealerId,PackageTypeId,StartDate,EndDate,IsActive,CreatedDate,CreatedBy)
									VALUES(@MakeId,@CityId,@DealerId,@PackageTypeId,@StartDate,@EndDate,1,@CreatedOn,@CreatedBy)
									SET @ID= SCOPE_IDENTITY()
									-- Make Current  Running Slot (Create Slot )
									INSERT INTO Impact_Slot(MakeId,CityId,DealerId,PackageTypeId,Impact_CampaignId,IsActive)
									VALUES(@MakeId,@CityId,@DealerId,@PackageTypeId,@ID,1)
									-- Log the Data 
									INSERT INTO Impact_CampaignLog( Impact_CampaignId,MakeId,CityId,DealerId,PackageTypeId,StartDate,EndDate,IsActive,LogDate,LoggedBy)
									VALUES(@ID,@MakeId,@CityId,@DealerId,@PackageTypeId,@StartDate,@EndDate,1,GETDATE(),@CreatedBy)			
								     
									SET  @Status=1	
									
							END
					END				
			END
 END
