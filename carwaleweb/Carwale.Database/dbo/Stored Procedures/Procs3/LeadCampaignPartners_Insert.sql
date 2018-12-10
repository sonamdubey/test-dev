IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[LeadCampaignPartners_Insert]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[LeadCampaignPartners_Insert]
GO

	--THIS PROCEDURE IS FOR INSERTING AND UPDATING RECORDS FOR AdCampaignPartners TABLE

CREATE PROCEDURE [dbo].[LeadCampaignPartners_Insert]

	@Id			NUMERIC,	
	@CampaignName	VARCHAR(200),	
	@StartingFrom		DATETIME,
	@EndingTo		DATETIME,
	@CampaignType	SMALLINT,
	@Status		INTEGER OUTPUT

 AS
	BEGIN
		SELECT CampaignName FROM LeadCampaignPartners WHERE  CampaignName = @CampaignName AND IsActive = 1
		
		IF @@RowCount = 0
			BEGIN
			
				INSERT INTO LeadCampaignPartners( CampaignName, StartingFrom, EndingTo, CampaignType) 
				VALUES(@CampaignName, @StartingFrom, @EndingTo, @CampaignType)

				SET @Status = 1
			END
		ELSE
			SET @Status = 0
	END