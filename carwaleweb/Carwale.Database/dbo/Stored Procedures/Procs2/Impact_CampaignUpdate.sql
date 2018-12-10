IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[Impact_CampaignUpdate]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[Impact_CampaignUpdate]
GO

	
-- =============================================
-- Author      : Vinay Kumar Prajapati
-- Create date : 19th Sept 2014
-- Description : Update Impact_Slot And Impact_Campaign
-- =============================================

CREATE PROCEDURE [dbo].[Impact_CampaignUpdate]
    (
	@ImpactCampaignId    INT,
	@MakeId              INT,
    @CityId              INT,
    @PackageId           INT ,
    @DealerID            INT,
	@IsActive            BIT,
	@Action              VARCHAR(50),
	@StartDate           DateTime,
	@EndDate             DateTime,
	@UpdatedBy           INT,
	@ReturnStatus              BIT OUTPUT
	)
 AS
   DECLARE @StatusValue BIT

    BEGIN 
	    IF @Action ='Activate'
			BEGIN
				EXEC  Impact_CampaignActivate @ImpactCampaignId, @MakeId,@CityId,@DealerId,@PackageId,@IsActive,@StartDate,@EndDate,@UpdatedBy
				SET @ReturnStatus=1
			END			  
		ELSE IF @Action ='Deactivate'
			BEGIN
			    BEGIN 
				   --Delete From Impact_Slot
					    DELETE FROM Impact_Slot  WHERE Impact_CampaignId=@ImpactCampaignId
						
					-- Update Impact_Campaign
						 UPDATE Impact_Campaign SET IsActive=@IsActive ,CreatedBy=@UpdatedBy ,StartDate=@StartDate,EndDate= @EndDate ,CreatedDate=GETDATE()
						 WHERE Impact_CampaignId=@ImpactCampaignId	 
				   --Log the Data 
						INSERT INTO Impact_CampaignLog( Impact_CampaignId,MakeId,CityId,DealerId,PackageTypeId,StartDate,EndDate,IsActive,LogDate,LoggedBy)
						VALUES(@ImpactCampaignId,@MakeId,@CityId,@DealerId,@PackageId,@StartDate,@EndDate,0,GETDATE(),@UpdatedBy)	

						SET @ReturnStatus=1
				   END
			END

		ELSE IF @Action ='NoAction'
		    BEGIN
			---user want to change only start date and end date
				UPDATE Impact_Campaign SET StartDate=@StartDate, EndDate=@EndDate WHERE Impact_CampaignId=@ImpactCampaignId
		    --Log the Data 
				INSERT INTO Impact_CampaignLog( Impact_CampaignId,MakeId,CityId,DealerId,PackageTypeId,StartDate,EndDate,IsActive,LogDate,LoggedBy)
				VALUES(@ImpactCampaignId,@MakeId,@CityId,@DealerId,@PackageId,@StartDate,@EndDate,@IsActive,GETDATE(),@UpdatedBy)	
				SET  @ReturnStatus=1
			END

       ELSE  -- having any problem to update       
			SET  @ReturnStatus=0
   END

	     