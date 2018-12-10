IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[GetDealerRulesOnCampaign]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[GetDealerRulesOnCampaign]
GO

	
-- =============================================
-- Author:		Vinayak
-- Create date: <25/07/2016>
-- Description:	Get Dealer Rules on model version page
-- exec [dbo].[GetDealerRulesOnCampaign] 4661
-- DealerDetails_9671_camp4661_city1_make1 //hare krsihna //m site mmv
-- =============================================
CREATE PROCEDURE [dbo].[GetDealerRulesOnCampaign] @CampaignId INT
	
AS
BEGIN
	SET NOCOUNT ON;

	SELECT PDS.DealerId as DealerId,PDCM.CityId as CityId ,PDCM.MakeId as MakeId,PDS.Id as CampaignId 
	FROM PQ_DealerSponsored PDS WITH (NOLOCK) 
	INNER JOIN PQ_DealerCitiesModels PDCM WITH(NOLOCK) ON PDCM.CampaignId=PDS.Id
	INNER JOIN Dealers D WITH (NOLOCK) ON D.ID = PDS.DealerId
	WHERE PDS.Id = @CampaignId
END

