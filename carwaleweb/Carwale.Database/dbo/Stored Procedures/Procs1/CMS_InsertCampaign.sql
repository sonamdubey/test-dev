IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[CMS_InsertCampaign]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[CMS_InsertCampaign]
GO

	
CREATE    PROCEDURE [dbo].[CMS_InsertCampaign]

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
@Comments      		 VARCHAR(2000),

@Id			BIGINT OUTPUT,
@IsActive		BIT

AS	
BEGIN
	SET @Id = -1

	INSERT INTO CMS_Campaigns (AgencyId,CampaignType, CampaignCategory, CampaignName,StartDate,EndDate,BookedQuantity,
			Rate,BookedAmount,RONumber,RODate,ROFilePath,Comments,IsActive)
	VALUES 			(@AgencyId,@CampaignType, @CampaignCategory, @CampaignName,@StartDate,@EndDate,@BookedQty,
			@Rate,@BookedAmt,@RONumber,@RODate,@ROFilePath,@Comments,@IsActive)

	SET @Id = SCOPE_IDENTITY()
END
