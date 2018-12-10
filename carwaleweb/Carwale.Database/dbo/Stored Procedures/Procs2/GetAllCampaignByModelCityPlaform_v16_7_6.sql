IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[GetAllCampaignByModelCityPlaform_v16_7_6]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[GetAllCampaignByModelCityPlaform_v16_7_6]
GO

	
--=====================================================================================
-- Modified: Vicky Lund, 05/04/2016, Used applicationId column of MM_SellerMobileMasking
-- Modified: Vinayak, 06/04/2016, Removed fetching ad templates html  
-- Modified: Vinayak, 03/05/2016, Added into select "IsThirdPartyCampaign"
-- Modified: Shalini Nair,20/07/2016, Fetching "ShowInRecommendation" column
--=====================================================================================
CREATE PROCEDURE [dbo].[GetAllCampaignByModelCityPlaform_v16_7_6] @ModelId INT
	,@ZoneId INT
	,@CityId INT
	,@PlatformId TINYINT
AS
--Author:Rakesh Yadav on 02 Dec 2015
--get only campaing data, this will also include Pan India campaign
BEGIN
		SELECT DISTINCT DS.Id
			,D.Id AS DealerId
			,DealerName AS ContactName
			,ContactNumber = CASE 
				WHEN DS.IsDefaultNumber != 0
					THEN CTOLL.TollFreeNumber
				ELSE MM.MaskingNumber
				END
			,D.MobileNo AS ActualMobile
			,DealerEmailId AS ContactEmail
			,vGAC.StartDate
			,vGAC.EndDate
			,D.DealerLeadBusinessType AS Type
			,IsDesktop AS ShowOnDesktop
			,IsMobile AS ShowOnMobile
			,IsAndroid AS ShowOnAndroid
			,IsIPhone AS ShowOniOS
			,vGAC.TotalGoal AS LeadTarget
			,vGAC.TotalCount AS LeadTargetAchieved
			,vGAC.DailyGoal AS DailyLeadTarget
			,vGAC.DailyCount AS DailyLeadTargetAchieved
			,EnableUserEmail AS NotifyUserByEmail
			,EnableUserSMS AS NotifyUserBySMS
			,CampaignPriority AS [Priority]
			,LinkText AS ActionText
			,EnableDealerEmail AS NotifyDealerByEmail
			,EnableDealerSMS AS NotifyDealerBySMS
			,ShowEmail AS IsEmailRequired
			,LeadPanel AS LeadPanel
			,DS.IsThirdPartyCampaign AS IsThirdPartyCampaign--Added into select "IsThirdPartyCampaign"
			,DS.isFeaturedEnabled AS IsFeaturedEnabled
			,DS.ShowInRecommendation AS ShowInRecommendation
		FROM PQ_DealerSponsored DS WITH (NOLOCK)
		JOIN Dealers D WITH (NOLOCK) ON D.ID = DS.DealerId
		JOIN PQ_DealerCitiesModels DCM WITH (NOLOCK) ON DS.Id = DCM.CampaignId
		JOIN vwRunningCampaigns vGAC WITH (NOLOCK) ON vGAC.CampaignId = DS.Id --changed the view name by Vinayak on 11/02/16 
		JOIN pq_dealerad_template_platform_maping TPM WITH (NOLOCK) ON TPM.CampaignId = DS.Id
		LEFT JOIN MM_SellerMobileMasking MM WITH (NOLOCK) ON MM.LeadCampaignId = DS.Id
			AND MM.ApplicationId = 1
		LEFT JOIN CarwaleTollFreeNumber CTOLL WITH (NOLOCK) ON DS.IsDefaultNumber = CTOLL.Id
		WHERE DCM.MakeId = (
				SELECT CarMakeId
				FROM CarModels WITH (NOLOCK)
				WHERE Id = @ModelId
				)
			AND (
				DCM.ModelId = @ModelId
				OR DCM.ModelId = - 1
				)
			AND (
				(DCM.StateId = - 1) --Pan India
				OR (
					DCM.StateId = (
						SELECT StateId
						FROM Cities WITH (NOLOCK)
						WHERE Id = @CityId
						)
					AND (
						(
							DCM.CityId = @CityId
							AND (
								ISNULL(DCM.ZoneId, 0) = ISNULL(@ZoneId, 0)
								OR @CityId NOT IN (1, 10)
								) -- zone check only for mumbai and newDelhi
							)
						OR DCM.CityId = - 1
						)
					) --City wise or Pan State
				)
			AND TPM.PlatformId = @PlatformId
			AND D.STATUS = 0
END

