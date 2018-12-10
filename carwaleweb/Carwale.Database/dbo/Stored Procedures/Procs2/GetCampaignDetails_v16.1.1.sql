IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[GetCampaignDetails_v16]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[GetCampaignDetails_v16]
GO

	
-- =============================================
-- Author:		Anchal Gupta
-- Create date: 30/09/2015
-- Description: 
-- Modified by: Shalini Nair on 30/09/2015
-- Modified by: Vicky Lund on 07/10/2015
-- Modified by : Shalini Nair on 03/11/2015
-- Modified by : Vicky Lund on 24/12/2015
-- Modified by : Shalini Nair on 31/12/2015 to retrieve masking number from MM_SellerMobileMasking based on IsDefaultNumber flag
-- exec [dbo].[GetCampaignDetails_v16.1.1] 0,4410
-- Modified By: Shalini Nair on 28/01/2016 to retrieve CarwaleTollFree Number based on IsDefaultNumber column
-- Modified By: Shalini Nair on 18/02/2016 to retrieve campaignbehaviour
-- Modified: Vicky Lund, 05/04/2016, Used applicationId column of MM_SellerMobileMasking
-- =============================================
CREATE PROCEDURE [dbo].[GetCampaignDetails_v16.1.1]
	-- Add the parameters for the stored procedure here
	@ContractId INT
	,@CampaignId INT
AS
BEGIN
	DECLARE @TollFreeNumber VARCHAR(15)

	--SELECT TOP 1 @TollFreeNumber = TollFreeNumber
	--FROM CarwaleTollFreeNumber WITH (NOLOCK)
	IF (@CampaignId > 0)
	BEGIN
		SELECT TOP 1 PDS.DealerId AS DealerId
			,D.Organization AS DealerName
			,NULL TotalGoal
			,NULL TotalDelivered
			,PDS.TotalGoal AS CampaignTotalGoal
			,PDS.TotalCount AS CampaignTotalDelivered
			,PDS.DailyCount
			,PDS.DailyGoal
			,PDS.DealerEmailId AS DealerEmailId
			,PDS.EnableDealerEmail
			,PDS.EnableDealerSMS
			,PDS.EnableUserEmail
			,PDS.EnableUserSMS
			,PDS.CampaignPriority
			,PDS.LeadPanel
			,@CampaignId AS CampaignId
			,PDS.IsActive
			,MaskingNumber = CASE 
				WHEN PDS.IsDefaultNumber != 0
					THEN TOLL.TollFreeNumber
				ELSE MM.MaskingNumber
				END
			,PDS.IsDefaultNumber
			,MM.Mobile
			,MM.ConsumerType
			,MM.NCDBrandId
			,NULL StartDate
			,NULL EndDate
			,PDS.CampaignBehaviour AS ContractBehaviour
			,PDS.LeadPanel
		FROM PQ_DealerSponsored PDS WITH (NOLOCK)
		INNER JOIN Dealers D WITH (NOLOCK) ON PDS.DealerId = D.ID
			AND PDS.Id = @CampaignId
		LEFT OUTER JOIN MM_SellerMobileMasking MM WITH (NOLOCK) ON PDS.Id = MM.LeadCampaignId
			AND MM.ConsumerId = PDS.DealerId
			AND MM.ApplicationId = 1
		--LEFT OUTER JOIN TC_ContractCampaignMapping TCCM WITH (NOLOCK) ON PDS.Id = TCCM.CampaignId
		--	AND TCCM.IsActiveContract = 1
		LEFT JOIN CarwaleTollFreeNumber TOLL WITH (NOLOCK) ON TOLL.Id = PDS.IsDefaultNumber
			--ORDER BY TCCM.StartDate
	END
	ELSE
	BEGIN
		SELECT ccm.DealerId AS DealerId
			,D.Organization AS DealerName
			,ccm.TotalGoal
			,ccm.TotalDelivered
			,NULL AS CampaignTotalGoal
			,NULL AS CampaignTotalDelivered
			,NULL AS DailyCount
			,NULL AS DailyGoal
			,NULL AS DealerEmailId
			,NULL AS EnableDealerEmail
			,NULL AS EnableDealerSMS
			,NULL AS EnableUserEmail
			,NULL AS EnableUserSMS
			,NULL AS CampaignPriority
			,NULL AS LeadPanel
			,0 CampaignId
			,NULL AS IsActive
			,NULL AS MaskingNumber
			,NULL AS Mobile
			,NULL AS ConsumerType
			,NULL AS NCDBrandId
			,ccm.StartDate
			,ccm.EndDate
			,ccm.ContractBehaviour
			,NULL AS LeadPanel
		FROM TC_ContractCampaignMapping ccm WITH (NOLOCK)
		LEFT JOIN Dealers D WITH (NOLOCK) ON CCM.DealerId = D.ID
		--LEFT JOIN PQ_DealerSponsored ds WITH (NOLOCK) ON ccm.CampaignId = ds.Id
		--LEFT JOIN MM_SellerMobileMasking MM WITH (NOLOCK) ON ccm.DealerId = MM.ConsumerId
		--	AND MM.LeadCampaignId = ccm.CampaignId
		WHERE ccm.ContractId = @ContractId;
	END
END
