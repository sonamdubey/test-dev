IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[PQ_SaveFeaturedCampaign]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[PQ_SaveFeaturedCampaign]
GO

	
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[PQ_SaveFeaturedCampaign] 
	@Id				INT,
	@CampaignName	VARCHAR(100),
	@StartDate		VARCHAR(50),
	@EndDate		VARCHAR(50),
	@CampaignType	INT,
	@DealerId		INT,
	@UpdatedBy		INT
AS
BEGIN
	IF @Id = -1
	BEGIN
		INSERT INTO PQ_FeaturedCampaign(CampaignName,StartDate,EndDate,CampaignType,DealerId)
		VALUES(@CampaignName,@StartDate,@EndDate,@CampaignType,@DealerId)
	END
	ELSE
	BEGIN
		UPDATE PQ_FeaturedCampaign
		SET CampaignName = @CampaignName,
		StartDate = @StartDate,
		EndDate = @EndDate,
		CampaignType = @CampaignType,
		DealerId = @DealerId,
		UpdatedBy = @UpdatedBy,
		UpdatedOn = GETDATE()
		WHERE Id = @Id
	END
END

