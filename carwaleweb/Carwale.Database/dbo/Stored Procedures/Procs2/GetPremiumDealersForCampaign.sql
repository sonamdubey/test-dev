IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[GetPremiumDealersForCampaign]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[GetPremiumDealersForCampaign]
GO

	
-- =============================================
-- Author:		Anchal gupta
-- Create date: 11-12-2015
-- Description: Getting list of premium dealers for a particular campaign
-- =============================================
CREATE PROCEDURE [dbo].[GetPremiumDealersForCampaign]
	-- Add the parameters for the stored procedure here
	@CampaignId int
AS
BEGIN
	SELECT DLC.DealerLocatorConfigurationId AS Id
		,DLC.DealerID AS DealerId  -- Added by Manish on 23-10-2015
		,D.Organization AS DealerName
		,D.Address1  AS Address
		,D.EmailId 
		,'+91' + D.MobileNo As PhoneNo
		, ROW_NUMBER() OVER(ORDER BY NEWID()) RowOrder
		, 1 as IsPremium
	FROM DealerLocatorConfiguration AS DLC WITH (NOLOCK) 
	JOIN Dealers as D WITH(NOLOCK) ON D.ID=DLC.DealerId
	JOIN TC_DealerMakes as TDM WITH(NOLOCK) ON TDM.DealerId=D.ID
	WHERE 
		DLC.PQ_DealerSponsoredId = @CampaignId
		and DLC.IsDealerLocatorPremium = 1
		and DLC.IsLocatorActive =1
		and D.IsDealerActive= 1
END

