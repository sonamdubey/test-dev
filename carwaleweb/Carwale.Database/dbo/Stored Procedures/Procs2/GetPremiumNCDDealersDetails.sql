IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[GetPremiumNCDDealersDetails]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[GetPremiumNCDDealersDetails]
GO

	-- =============================================
-- Author:		Anchal gupta
-- Create date: 16-12-2015
-- Description:	Get the dealer details of premium dealers
-- EXEC [GetPremiumNCDDealersDetails] 10,8
-- =============================================
CREATE PROCEDURE [dbo].[GetPremiumNCDDealersDetails]
	-- Add the parameters for the stored procedure here
	@CityId SMALLINT
	,@MakeId SMALLINT
	,@CampaignId Int
AS
BEGIN

	SELECT DLC.DealerLocatorConfigurationId AS Id
		,DLC.DealerID AS DealerId
		,D.Organization AS DealerName
		,D.Address1 AS Address
		,D.EmailId
		,'+91' + D.MobileNo AS PhoneNo
		,ROW_NUMBER() OVER (
			ORDER BY NEWID()
			) RowOrder
		,1 AS IsPremium
	FROM DealerLocatorConfiguration AS DLC WITH (NOLOCK)
	JOIN Dealers AS D WITH (NOLOCK) ON D.ID = DLC.DealerId
	JOIN TC_DealerMakes AS TDM WITH (NOLOCK) ON TDM.DealerId = D.ID
	WHERE D.CityId = @CityId
		AND TDM.MakeId = @MakeId
		AND DLC.IsDealerLocatorPremium = 1
		AND DLC.IsLocatorActive = 1
		AND D.IsDealerActive = 1
		AND DLC.PQ_DealerSponsoredId = @CampaignId
END
