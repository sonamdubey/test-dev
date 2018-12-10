IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[ESM_SaveCampaign]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[ESM_SaveCampaign]
GO

	
CREATE PROCEDURE [dbo].[ESM_SaveCampaign]
(
	@CampaignName AS VARCHAR(50),
	@IsActive AS BIT,
	@UpdatedOn AS DATETIME,
	@ID AS NUMERIC(18,0),
	@UpdatedBy AS NUMERIC(18,0)
)
AS
BEGIN

	IF(@ID = -1)  
		BEGIN  
		   INSERT INTO ESM_CampaignTypes (Name, UpdatedOn, UpdatedBy,IsActive )  
		   VALUES( @CampaignName, @UpdatedOn, @UpdatedBy,@IsActive)  
		END  
	ELSE  
		BEGIN  
			UPDATE ESM_CampaignTypes SET   
			Name = @CampaignName, IsActive = @IsActive, UpdatedOn = @UpdatedOn,UpdatedBy = @UpdatedBy  
			WHERE Id = @ID  
		END   
END
