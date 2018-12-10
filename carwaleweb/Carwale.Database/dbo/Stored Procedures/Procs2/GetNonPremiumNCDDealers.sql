IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[GetNonPremiumNCDDealers]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[GetNonPremiumNCDDealers]
GO

	-- =============================================
-- Author:		Anchal gupta
-- Create date: 16-12-2015
-- Description:	Get the dealer details of non premium dealers
-- EXEC [GetNonPremiumNCDDealers] 1,8, 4401
-- REMOVED JOIN WITH pq_dEALERSPONSORED TABLE BY ANCHAL GUPTA ON 30-12-2015
-- Updated: Vicky Lund, 02/11/2016, Renamed view
-- =============================================
CREATE PROCEDURE [dbo].[GetNonPremiumNCDDealers]
	-- Add the parameters for the stored procedure here
	@CityId SMALLINT
	,@MakeId SMALLINT
	,@CampaignId INT
AS
BEGIN
	WITH cte
	AS (
		SELECT DLC.DealerLocatorConfigurationId AS Id
			,DLC.DealerID AS DealerId -- Added by Manish on 23-10-2015
			,D.Organization AS DealerName
			,D.Address1 AS Address
			,D.EmailId
			,'+91' + D.MobileNo AS PhoneNo
			,ROW_NUMBER() OVER (
				ORDER BY NEWID()
				) RowOrder
			,0 AS IsPremium
			,1 AS Available
			,2 AS isActive
			,0 AS CampaignId
			,NULL AS MappedDealerName
			,0 AS MappedDealerId
		FROM DealerLocatorConfiguration AS DLC WITH (NOLOCK)
		JOIN Dealers AS D WITH (NOLOCK) ON D.ID = DLC.DealerId
		JOIN TC_DealerMakes AS TDM WITH (NOLOCK) ON TDM.DealerId = D.ID
		WHERE D.CityId = @CityId
			AND TDM.MakeId = @MakeId
			AND DLC.IsDealerLocatorPremium = 0
			AND DLC.IsLocatorActive = 1
			AND D.IsDealerActive = 1
		)
	SELECT Id
		,DealerId
		,DealerName
		,Address
		,EmailId
		,PhoneNo
		,RowOrder
		,IsPremium
		,Available
		,isActive
		,CampaignId
		,MappedDealerName
		,MappedDealerId
	FROM cte WITH (NOLOCK)
	
	UNION ALL
	
	SELECT DLC.DealerLocatorConfigurationId AS Id
		,DLC.DealerID AS DealerId
		,D.Organization AS DealerName
		,D.Address1 AS Address
		,D.EmailId
		,'+91' + D.MobileNo AS PhoneNo
		,ROW_NUMBER() OVER (
			ORDER BY NEWID()
			) RowOrder
		,0 AS IsPremium
		,0 AS Available
		,CASE 
			WHEN AC.campaignId IS NULL
				THEN 0
			ELSE 1
			END AS isActive
		,DLC.PQ_DealerSponsoredId AS CampaignId
		,Ds.DealerName AS MappedDealerName
		,DS.DealerId AS MappedDealerI
	FROM DealerLocatorConfiguration AS DLC WITH (NOLOCK)
	JOIN Dealers AS D WITH (NOLOCK) ON D.ID = DLC.DealerId
	JOIN TC_DealerMakes AS TDM WITH (NOLOCK) ON TDM.DealerId = D.ID
	JOIN PQ_DealerSponsored AS DS WITH (NOLOCK) ON DLC.PQ_DealerSponsoredId = DS.Id
	LEFT OUTER JOIN vwActiveCampaigns AS AC WITH (NOLOCK) ON DS.ID = AC.CampaignId
	WHERE D.CityId = @CityId
		AND TDM.MakeId = @MakeId
		AND DLC.IsDealerLocatorPremium = 1
		AND DLC.IsLocatorActive = 1
		AND D.IsDealerActive = 1
		AND DLC.PQ_DealerSponsoredId <> @CampaignId
END
