IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[InsertClientCampaigns]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[InsertClientCampaigns]
GO
	
CREATE PROCEDURE [dbo].[InsertClientCampaigns]
	@Id			BIGINT,
	@ClientId		BIGINT, 
	@CampaignName	VARCHAR(200),
	@CampaignId		BIGINT,
	@StartDate		DATETIME,
	@EndDate		DATETIME,
	@CampaignType	BIGINT,
	@BookedValue		BIGINT,
	@Status		INT OUTPUT		
 AS
	
BEGIN
	SET @Status = 0

	IF @Id = -1
		BEGIN

			
			INSERT INTO AW_ClientCampaigns(ClientId, CampaignName, AWCampaignId, StartDate, EndDate, IsActive, CampaignType, BookedValue ) 
			VALUES(@ClientId, @CampaignName, @CampaignId, @StartDate, @EndDate, 1, @CampaignType, @BookedValue)
		
			SET @Status = 1 
				
		END
	ELSE
		BEGIN
			
			UPDATE  AW_ClientCampaigns SET ClientId = @ClientId, CampaignName = @CampaignName, AWCampaignId = @CampaignId, 
				StartDate = @StartDate, EndDate = @EndDate, CampaignType = @CampaignType, BookedValue = @BookedValue
			WHERE ID = @Id

			SET @Status = 1 
					
		END
END