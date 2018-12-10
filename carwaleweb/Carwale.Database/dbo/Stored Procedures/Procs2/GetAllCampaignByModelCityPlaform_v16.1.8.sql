IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[GetAllCampaignByModelCityPlaform_v16]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[GetAllCampaignByModelCityPlaform_v16]
GO

	 
CREATE PROCEDURE [dbo].[GetAllCampaignByModelCityPlaform_v16.1.8]
@ModelId INT 
,@ZoneId INT 
,@CityId INT 
,@PlatformId TINYINT
AS
--Author:Rakesh Yadav on 02 Dec 2015
--get only campaing data, this will also include Pan India campaign
BEGIN
	
	SELECT DISTINCT DS.Id
	,DCM.DealerId
	,DealerName AS ContactName
	,ContactNumber = CASE 
		WHEN DS.IsDefaultNumber != 0 THEN 
			CTOLL.TollFreeNumber
		ELSE 
			MM.MaskingNumber
		END
	,DealerEmailId AS ContactEmail
	,DS.StartDate
	,DS.EndDate
	,D.DealerLeadBusinessType AS Type 
	,IsDesktop AS ShowOnDesktop
	,IsMobile AS ShowOnMobile
	,IsAndroid AS ShowOnAndroid
	,IsIPhone AS ShowOniOS
	,DS.TotalGoal AS LeadTarget
	,DS.TotalCount AS LeadTargetAchieved
	,DailyGoal AS DailyLeadTarget
	,DailyCount AS DailyLeadTargetAchieved
	,EnableUserEmail AS NotifyUserByEmail
	,EnableUserSMS AS NotifyUserBySMS
	,CampaignPriority AS [Priority]
	,LinkText AS ActionText
	,EnableDealerEmail AS NotifyDealerByEmail
	,EnableDealerSMS AS NotifyDealerBySMS
	,ShowEmail AS IsEmailRequired
	,LeadPanel AS LeadPanel
 FROM PQ_DealerSponsored DS WITH(NOLOCK)
JOIN Dealers D WITH(NOLOCK) ON D.ID=DS.DealerId
JOIN PQ_DealerCitiesModels DCM WITH(NOLOCK) ON DS.Id = DCM.CampaignId
JOIN pq_dealerad_template_platform_maping TPM WITH (NOLOCK) ON TPM.CampaignId = DS.Id
JOIN TC_ContractCampaignMapping CCM WITH (NOLOCK) ON CCM.CampaignId=DS.Id
LEFT JOIN MM_SellerMobileMasking MM WITH(NOLOCK) ON MM.LeadCampaignId = DS.Id
LEFT JOIN CarwaleTollFreeNumber CTOLL WITH(NOLOCK) ON DS.IsDefaultNumber = CTOLL.Id
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
			AND ds.IsActive = 1
			AND TPM.PlatformId = @PlatformId
			AND D.Status = 0 
			AND (MM.MaskingNumber IS NOT NULL OR DS.IsDefaultNumber != 0 )
END




