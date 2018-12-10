IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[GetDealerLocatorRules]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[GetDealerLocatorRules]
GO

	
-- =============================================
-- Author:		Vinayak
-- Create date: <25/07/2016>
-- Description:	Get Dealer Locator Rules
-- exec [dbo].[GetDealerLocatorRules] 4649
-- =============================================
CREATE PROCEDURE [dbo].[GetDealerLocatorRules] @CampaignId INT
	
AS
BEGIN
	SET NOCOUNT ON;

	SELECT D.StateId,D.CityId,TDM.MakeId,DNC.DealerId as DealerId
	FROM DealerLocatorConfiguration DNC WITH (NOLOCK) 
	--INNER JOIN PQ_DealerSponsored PDS WITH (NOLOCK) ON PDS.Id=DNC.PQ_DealerSponsoredId
	INNER JOIN Dealers D WITH (NOLOCK) ON D.ID = DNC.DealerId
	INNER JOIN TC_DealerMakes TDM WITH (NOLOCK) ON TDM.DealerId = D.ID
	WHERE DNC.PQ_DealerSponsoredId = @CampaignId
END
