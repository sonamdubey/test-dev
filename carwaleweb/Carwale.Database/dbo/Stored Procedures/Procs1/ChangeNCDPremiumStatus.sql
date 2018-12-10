IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[ChangeNCDPremiumStatus]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[ChangeNCDPremiumStatus]
GO

	-- =============================================
-- Author:		Anchal Gupta
-- Create date: 28th oct 2015
-- Description:	Changing non premium dealers to premium dealers and vice versa
-- =============================================
CREATE PROCEDURE [dbo].[ChangeNCDPremiumStatus] 
     @Id INT
	,@IsPremium bit
	,@CampaignId Int
	,@UpdatedBy Int
	
AS
BEGIN
	SET NOCOUNT ON;

    Update DealerLocatorConfiguration
	Set IsDealerLocatorPremium = @IsPremium,
	    PQ_DealerSponsoredId = @CampaignId,
	    LastUpdatedBy = @UpdatedBy
	From DealerLocatorConfiguration  WITH (NOLOCK)
	Where DealerId = @Id
	    AND IsLocatorActive = 1

	INSERT INTO DealerLocatorConfigurationActionLogs (
	     DealerLocatorConfigurationId
		,DealerId
		,PQ_DealerSponsoredId
		,IsDealerLocatorPremium
		,IsLocatorActive
		,CreatedOn
		,CreatedBy
		,LastUpdatedOn
		,LastUpdatedBy
		,ActionTaken
		,ActionTakenOn
		)
	Select
	     DealerLocatorConfigurationId
		,DealerId
		,PQ_DealerSponsoredId
		,IsDealerLocatorPremium
		,IsLocatorActive
		,CreatedOn
		,CreatedBy
		,LastUpdatedOn
		,LastUpdatedBy 
		,CASE 
			WHEN (@IsPremium = 1) THEN 
			'Record is made Premium'
			ELSE 
			'Record is made Non Premium'
		END
		,GetDate()
	From DealerLocatorConfiguration WITH (NOLOCK)
	Where DealerId = @Id
	
	
END

