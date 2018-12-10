IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[GetCampaignDetails_v16_8_7]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[GetCampaignDetails_v16_8_7]
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
-- Modified BY : Sanjay Soni on 31/03/2016 to fetch notification mobile
-- Modified: Vicky Lund, 05/04/2016, Used applicationId column of MM_SellerMobileMasking
-- Modified By: Shalini Nair on 11/04/2016 to pass DealerId 
-- Modified: Vinayak, 03/05/2016, Added into select "IsThirdPartyCampaign"
-- Modified By : Sanjay Soni 25/07/2016 Fetch Consumer Type from dealer table
-- Modified : Vicky Lund, 26/08/2016, Fetch State Id
-- =============================================
create PROCEDURE [dbo].[GetCampaignDetails_v16_8_7]
	@ContractId INT
	,@CampaignId INT
	,@DealerId INT
AS
BEGIN
	DECLARE @TollFreeNumber VARCHAR(15)

	IF (@CampaignId > 0)
	BEGIN
		SELECT TOP 1 PDS.DealerId AS DealerId
			,D.Organization AS DealerName
			,D.CityId
			,D.StateId
			,DealerMobileNumber = CASE 
				WHEN MM.LeadCampaignId = @CampaignId
					AND MM.ConsumerId = D.ID
					THEN MM.Mobile
				ELSE D.MobileNo
				END
			,NULL TotalGoal
			,NULL TotalDelivered
			,PDS.TotalGoal AS CampaignTotalGoal
			,PDS.TotalCount AS CampaignTotalDelivered
			,PDS.DailyCount
			,PDS.DailyGoal
			,PDS.DealerEmailId AS DealerNotificationEmailId
			,PDS.NotificationMobile AS DealerNotificationMobile
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
			,D.TC_DealerTypeId AS ConsumerType
			,MM.NCDBrandId
			,NULL StartDate
			,NULL EndDate
			,PDS.CampaignBehaviour AS ContractBehaviour
			,PDS.LeadPanel
			,PDS.IsThirdPartyCampaign
			,PDS.isFeaturedEnabled AS IsFeaturedEnabled
		FROM PQ_DealerSponsored PDS WITH (NOLOCK)
		INNER JOIN Dealers D WITH (NOLOCK) ON PDS.DealerId = D.ID
			AND PDS.Id = @CampaignId
		LEFT OUTER JOIN MM_SellerMobileMasking MM WITH (NOLOCK) ON PDS.Id = MM.LeadCampaignId
			AND MM.ConsumerId = PDS.DealerId
			AND MM.ApplicationId = 1
		LEFT JOIN CarwaleTollFreeNumber TOLL WITH (NOLOCK) ON TOLL.Id = PDS.IsDefaultNumber
	END
	ELSE
	BEGIN
		SELECT ccm.DealerId AS DealerId
			,D.Organization AS DealerName
			,D.CityId
			,D.StateId
			,D.MobileNo AS DealerMobileNumber
			,ccm.TotalGoal
			,ccm.TotalDelivered
			,NULL AS CampaignTotalGoal
			,NULL AS CampaignTotalDelivered
			,NULL AS DailyCount
			,NULL AS DailyGoal
			,NULL AS DealerEmailId
			,NULL AS DealerNotificationMobile
			,NULL AS DealerNotificationEmailId
			,NULL AS EnableDealerEmail
			,NULL AS EnableDealerSMS
			,NULL AS EnableUserEmail
			,NULL AS EnableUserSMS
			,NULL AS CampaignPriority
			,NULL AS LeadPanel
			,0 CampaignId
			,NULL AS IsActive
			,NULL AS MaskingNumber
			,D.TC_DealerTypeId AS ConsumerType
			,NULL AS NCDBrandId
			,ccm.StartDate
			,ccm.EndDate
			,ccm.ContractBehaviour
			,NULL AS LeadPanel
			,1 AS IsThirdPartyCampaign
			,1 AS isFeaturedEnabled
		FROM TC_ContractCampaignMapping ccm WITH (NOLOCK)
		LEFT JOIN Dealers D WITH (NOLOCK) ON CCM.DealerId = D.ID
		WHERE ccm.ContractId = @ContractId
			AND ccm.DealerId = @DealerId;
	END
END
