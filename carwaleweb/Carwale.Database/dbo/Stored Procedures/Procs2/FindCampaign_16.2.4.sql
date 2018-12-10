IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[FindCampaign_16]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[FindCampaign_16]
GO

	-- =============================================
-- Author:		Vicky Lund
-- Create date: 08/10/2015
-- Description: 
-- Modified By Chetan on <27/11/2015>- Changing return text of column 'type' - 'Lead Wise' to 'Lead Based' and 'No Limit' to 'Date Based',
-- Modified By : Shalini Nair on 24/12/2015 to retrieve Masking number from MM_SellerMobileMasking
-- Modified By: Shalini Nair on 13/01/2016 to retrieve TotalCount and TotalGoal from TC_ContractCampaignMapping
-- Modified By: Shalini Nair on 14/01/2016 to retrieve startdate and enddate from TC_ContractCampaignMapping
-- Modified By: Shalini Nair on 18/01/2016 to retrieve type of contract based on ContractBehaviour column in TC_ContractCampaignMapping
-- Modified By: Shalini Nair on 21/01/2016 to retrieve records based on @DealerIds(comma separated) value
-- Modified By: Shalini Nair on 28/01/2016 to retrieve CarwaleTollFree number based on IsDefaultnumber column
-- Modified By: Vicky Lund on 12/02/2016 Using GetRelevantContract function to get most relevant contract for a campaign 
--										 and Used vwActiveCampaigns for getting campaign is running or not
-- Modified By: Chetan Thambad on 02/03/2016 to retrieve ContractType, DealerActualNumber, DisplayName ,isDealerActive, IsDealerLocatorRuleExist, IsCrossSellRuleExist, IsPqRuleExist
-- Modified By: Shalini Nair on 08/03/2016 to retrieve DealerStatus,ContractStatus and enddate as '' for leadbased contract
-- Modified By: Vicky Lund on 15/03/2016 to use contract StartDate and EndDate for sorting instead of Campaign StartDate and EndDate
-- Modified: Vicky Lund, 05/04/2016, Used applicationId column of TC_ContractCampaignMapping, MM_SellerMobileMasking
-- EXEC [FindCampaign_16.2.4] -1,-1,'',-1,false,'EndDate','DESC',''
-- =============================================
CREATE PROCEDURE [dbo].[FindCampaign_16.2.4] @DealerId INT
	,@StateId INT
	,@CityIds VARCHAR(MAX)
	,@MakeId INT
	,@IsCarwaleCampaign BIT
	,@SortCriteria VARCHAR(100)
	,@SortDirection VARCHAR(100)
	,@DealerIds VARCHAR(100) = NULL
AS
BEGIN
	SELECT PDS.Id AS CampaignId
		,CCM.ContractId
		,PDS.DealerId
		,D.Organization DealerName
		,D.IsDealerDeleted
		,D.TC_DealerTypeId
		,PDS.IsActive
		,CASE 
			WHEN vwAC.CampaignId IS NOT NULL
				THEN CONVERT(BIT, 1)
			ELSE CONVERT(BIT, 0)
			END IsCampaignRunning
		,Phone = CASE 
			WHEN PDS.IsDefaultNumber != 0
				THEN CTOLL.TollFreeNumber
			ELSE MM.MaskingNumber
			END
		,REPLACE(PDS.DealerEmailId, ',', ', ') DealerEmailId
		,CONVERT(VARCHAR(10), CCM.StartDate, 101) StartDate
		,CASE 
			WHEN CCM.EndDate IS NULL
				OR CCM.ContractBehaviour = 1 --LEAD BASED
				THEN ''
			ELSE CONVERT(VARCHAR(10), CCM.EndDate, 101)
			END EndDate
		,CASE 
			WHEN CCM.ContractBehaviour = 1
				THEN 'Lead Based'
			WHEN CCM.ContractBehaviour = 2
				THEN 'Date Based'
			END AS [Type]
		,CCM.TotalGoal
		,CCM.TotalDelivered AS TotalCount
		,PDS.DailyGoal
		,PDS.DailyCount
		,CASE PDS.LeadPanel
			WHEN 1
				THEN 'Normal CRM'
			WHEN 2
				THEN 'Autobiz'
			WHEN 3
				THEN 'CRM Autoassigned'
			END AS LeadPanel
		,CASE 
			WHEN PDS.IsActive = 1
				THEN 'Active'
			ELSE 'Paused'
			END [Status]
		,CASE 
			WHEN (
					EXISTS (
						SELECT DCM.Id
						FROM PQ_DealerCitiesModels DCM WITH (NOLOCK)
						WHERE DCM.CampaignId = PDS.Id
						)
					AND EXISTS (
						SELECT DATPM.ID
						FROM PQ_DealerAd_Template_Platform_Maping DATPM WITH (NOLOCK)
						WHERE DATPM.CampaignId = PDS.Id
						)
					)
				THEN 'True'
			ELSE 'False'
			END AS IsPqRuleExist
		,CASE 
			WHEN (
					EXISTS (
						SELECT PCSCR.Id
						FROM PQ_CrossSellCampaignRules PCSCR WITH (NOLOCK)
						INNER JOIN PQ_CrossSellCampaign PCSC WITH (NOLOCK) ON PCSCR.CrossSellCampaignId = PCSC.Id
							AND PCSC.CampaignId = PDS.Id
						)
					)
				THEN 'True'
			ELSE 'False'
			END AS IsCrossSellRuleExist
		,CASE 
			WHEN (
					EXISTS (
						SELECT DLC.DealerLocatorConfigurationId
						FROM DealerLocatorConfiguration DLC WITH (NOLOCK)
						WHERE DLC.IsLocatorActive = 1
							AND DLC.IsDealerLocatorPremium = 1
							AND DLC.PQ_DealerSponsoredId = PDS.Id
						)
					)
				THEN 'True'
			ELSE 'False'
			END AS IsDealerLocatorRuleExist
		,CASE CCM.ContractType
			WHEN 1
				THEN 'N'
			WHEN 2
				THEN 'R'
			END AS ContractType
		,MM.Mobile
		,PDS.DealerName AS DisplayName
		,D.STATUS AS DealerStatus
		,CCM.ContractStatus AS ContractStatus
	FROM PQ_DealerSponsored PDS WITH (NOLOCK)
	INNER JOIN TC_ContractCampaignMapping CCM WITH (NOLOCK) ON PDS.Id = CCM.CampaignId
		AND CCM.ApplicationID = 1
		AND CCM.ContractId = dbo.GetRelevantContract(PDS.Id)
		AND (
			@MakeId = - 1
			OR @IsCarwaleCampaign = 1
			OR (
				EXISTS (
					SELECT DCM.Id
					FROM PQ_DealerCitiesModels DCM WITH (NOLOCK)
					WHERE DCM.CampaignId = PDS.Id
						AND DCM.MakeId = @MakeId
					)
				)
			)
	INNER JOIN Dealers D WITH (NOLOCK) ON D.ID = PDS.DealerId
		AND Organization IS NOT NULL
		AND Organization <> ''
		AND D.IsDealerDeleted = 0
		AND D.IsTCDealer = 1
		AND (
			(
				@DealerIds IS NULL
				OR PDS.DealerId IN (
					SELECT ListMember
					FROM fnSplitCSVValuesWithIdentity(@DealerIds)
					)
				)
			OR (
				@IsCarwaleCampaign = 1
				AND PDS.DealerId = 9350
				)
			OR (
				@IsCarwaleCampaign = 0
				AND (
					(
						PDS.DealerId = @DealerId
						AND @DealerId != - 1
						)
					OR (
						@DealerIds = ''
						AND @DealerId = - 1
						)
					)
				)
			)
		AND (
			@IsCarwaleCampaign = 1
			OR @CityIds = ''
			OR (
				D.CityId IN (
					SELECT items
					FROM [SplitText](@CityIds, ',')
					)
				)
			)
		AND (
			@IsCarwaleCampaign = 1
			OR @StateId = - 1
			OR D.StateId = @StateId
			)
	LEFT OUTER JOIN vwActiveCampaigns vwAC WITH (NOLOCK) ON PDS.Id = vwAC.CampaignId
	LEFT OUTER JOIN MM_SellerMobileMasking MM WITH (NOLOCK) ON MM.LeadCampaignId = PDS.Id
		AND PDS.DealerId = MM.ConsumerId
		AND MM.ApplicationId = 1
	LEFT OUTER JOIN CarwaleTollFreeNumber CTOLL WITH (NOLOCK) ON CTOLL.Id = PDS.IsDefaultNumber
	ORDER BY CASE 
			WHEN @SortDirection = 'ASC'
				AND @SortCriteria = 'DealerName'
				THEN PDS.DealerName
			END ASC
		,CASE 
			WHEN @SortDirection = 'ASC'
				AND @SortCriteria = 'StartDate'
				THEN CCM.StartDate
			END ASC
		,CASE 
			WHEN @SortDirection = 'ASC'
				AND @SortCriteria = 'EndDate'
				THEN CASE 
						WHEN CCM.ContractBehaviour = 1 --LEAD BASED
							THEN ''
						ELSE CONVERT(VARCHAR(10), CCM.EndDate, 101)
						END
			END ASC
		,CASE 
			WHEN @SortDirection = 'DESC'
				AND @SortCriteria = 'DealerName'
				THEN PDS.DealerName
			END DESC
		,CASE 
			WHEN @SortDirection = 'DESC'
				AND @SortCriteria = 'StartDate'
				THEN CCM.StartDate
			END DESC
		,CASE 
			WHEN @SortDirection = 'DESC'
				AND @SortCriteria = 'EndDate'
				THEN CASE 
						WHEN CCM.ContractBehaviour = 1 --LEAD BASED
							THEN ''
						ELSE CONVERT(VARCHAR(10), CCM.EndDate, 101)
						END
			END DESC
END
