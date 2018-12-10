IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[GetNCDDealersForStatusChange]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[GetNCDDealersForStatusChange]
GO

	
-- Author:		Anchal gupta
-- Create date: 18-11-2015
-- Description:	Get the dealer details to show them as premium and non premium listing
-- =============================================
CREATE PROCEDURE [dbo].[GetNCDDealersForStatusChange] (
	@CityId SMALLINT
	,@MakeId SMALLINT
	,@CampaignId Int
	)
AS
BEGIN
	SET NOCOUNT ON;

	with cte as 
(	SELECT DLC.DealerLocatorConfigurationId AS Id
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
	    D.CityId = @CityId 
		and TDM.MakeId = @MakeId
		and DLC.PQ_DealerSponsoredId = @CampaignId
		and DLC.IsDealerLocatorPremium = 1
		and DLC.IsLocatorActive =1
		and D.IsDealerActive= 1
		--and D.IsDealerDeleted = 0
		) select Id, DealerId, DealerName, Address, EmailId, PhoneNo, RowOrder, IsPremium from cte  
	UNION ALL

	SELECT DLC.DealerLocatorConfigurationId AS Id
		,DLC.DealerID AS DealerId  -- Added by Manish on 23-10-2015
		,D.Organization AS DealerName
		,D.Address1 AS Address
		,D.EmailId 
		,'+91' + D.MobileNo As PhoneNo
		, ROW_NUMBER() OVER(ORDER BY NEWID()) RowOrder
		, 0 as IsPremium
	FROM DealerLocatorConfiguration AS DLC WITH (NOLOCK) 
	JOIN Dealers as D WITH(NOLOCK) ON D.ID=DLC.DealerId
	JOIN TC_DealerMakes as TDM WITH(NOLOCK) ON TDM.DealerId=D.ID
	WHERE 
	    D.CityId = @CityId 
		and TDM.MakeId = @MakeId
		and DLC.IsDealerLocatorPremium = 0
		and DLC.IsLocatorActive =1
		and D.IsDealerActive= 1
	--	and D.IsDealerDeleted = 0
			ORDER BY IsPremium DESC ,RowOrder
END
