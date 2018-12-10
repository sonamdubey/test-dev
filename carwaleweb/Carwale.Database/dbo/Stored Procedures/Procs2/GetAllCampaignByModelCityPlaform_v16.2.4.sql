IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[GetAllCampaignByModelCityPlaform_v16]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[GetAllCampaignByModelCityPlaform_v16]
GO

	--=====================================================================
--Author:Rakesh Yadav on 02 Dec 2015
--get only campaing data, this will also include Pan India campaign
-- Modified: Vicky Lund, 05/04/2016, Used applicationId column of MM_SellerMobileMasking
--=====================================================================
CREATE PROCEDURE [dbo].[GetAllCampaignByModelCityPlaform_v16.2.4] @ModelId INT
	,@ZoneId INT
	,@CityId INT
	,@PlatformId TINYINT
AS
BEGIN
	WITH dealerCampaigns_cte
	AS (
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
			,SDT.Template
			,SDT.TemplateName
		FROM PQ_DealerSponsored DS WITH (NOLOCK)
		JOIN Dealers D WITH (NOLOCK) ON D.ID = DS.DealerId
		JOIN PQ_DealerCitiesModels DCM WITH (NOLOCK) ON DS.Id = DCM.CampaignId
		JOIN vwRunningCampaigns vGAC WITH (NOLOCK) ON vGAC.CampaignId = DS.Id --changed the view name by Vinayak on 11/02/16 
		JOIN pq_dealerad_template_platform_maping TPM WITH (NOLOCK) ON TPM.CampaignId = DS.Id
		JOIN pq_sponsoreddealead_templates SDT WITH (NOLOCK) ON SDT.TemplateId = TPM.AssignedTemplateId
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
			AND (
				MM.MaskingNumber IS NOT NULL
				OR DS.IsDefaultNumber != 0
				)
		)
	SELECT *
	FROM dealerCampaigns_cte cc WITH (NOLOCK)
	WHERE [Priority] = (
			SELECT Min([Priority])
			FROM dealerCampaigns_cte WITH (NOLOCK)
			)
END
