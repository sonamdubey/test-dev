IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[GetDealerSummary]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[GetDealerSummary]
GO

	
-- =============================================
-- Author:		<Author,,Name>
-- Create date: 26/07/2016
-- Description:	Get dealersummary, basic info
-- =============================================
CREATE PROCEDURE [dbo].[GetDealerSummary] @DealerId INT,
@CampaignId INT
	
AS
BEGIN
	SET NOCOUNT ON;

    SELECT
		D.CityId 
		,D.StateId
		,TDM.MakeId
	FROM DealerLocatorConfiguration AS DLC WITH (NOLOCK)
	JOIN Dealers AS D WITH (NOLOCK) ON D.ID = DLC.DealerId
	JOIN TC_DealerMakes AS TDM WITH (NOLOCK) ON TDM.DealerId = D.ID
	JOIN PQ_DealerSponsored AS DS WITH (NOLOCK) ON DLC.PQ_DealerSponsoredId = DS.Id
	WHERE DLC.PQ_DealerSponsoredId = @CampaignId AND D.ID = @DealerId

END

