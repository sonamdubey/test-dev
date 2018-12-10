IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[CMS_UpdateCampaign]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[CMS_UpdateCampaign]
GO

	

CREATE   PROCEDURE [dbo].[CMS_UpdateCampaign]

@Id			BIGINT,
@AgencyId		NUMERIC,
@CampaignType	SMALLINT,
@CampaignCategory INT,
@CampaignName	VARCHAR(150),
@StartDate		DATETIME,
@EndDate		DATETIME,
@BookedQty		NUMERIC,
@Rate			DECIMAL(9,2),
@BookedAmt		DECIMAL(9,2),
@RONumber		VARCHAR(50),
@RODate		DateTime,
@ROFilePath		VARCHAR(100),
@IsActive 		BIT,
@Status		INT OUTPUT

AS	
BEGIN
	SET @Status= -1
	
	IF @ROFilePath <> ' '

		BEGIN

			UPDATE CMS_Campaigns SET AgencyId=@AgencyId, CampaignType=@CampaignType, 
				CampaignName=@CampaignName,StartDate=@StartDate,EndDate=@EndDate,
				BookedQuantity=@BookedQty,Rate=@Rate,BookedAmount=@BookedAmt,RONumber=@RONumber,
				RODate=@RODate,ROFilePath=@ROFilePath,IsActive=@IsActive,
				CampaignCategory = @CampaignCategory
			WHERE Id = @Id
		END
	ELSE
		
		BEGIN
		
			UPDATE CMS_Campaigns SET AgencyId=@AgencyId, CampaignType=@CampaignType, 
				CampaignName=@CampaignName,StartDate=@StartDate,EndDate=@EndDate,
				BookedQuantity=@BookedQty,Rate=@Rate,BookedAmount=@BookedAmt,RONumber=@RONumber,
				RODate=@RODate,IsActive=@IsActive,
				CampaignCategory = @CampaignCategory
			WHERE Id = @Id
		END

	SET @Status = 1
END
