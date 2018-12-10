IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[FindCampaign_15]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[FindCampaign_15]
GO

	-- =============================================
-- Author:		Vicky Lund
-- Create date: 08/10/2015
-- Description: 
-- PENDING: Check for Dealer -> IsPremium
-- Modeified By Chetan on <27/11/2015>- Changing return text of column 'type' - 'Lead Wise' to 'Lead Based' and 'No Limit' to 'Date Based',
-- EXEC FindCampaign 4691,-1,'',-1,false,'EndDate','DESC'
-- Modified By: Shalini Nair on 13/01/2015 to retrieve TotalCount and TotalGoal from TC_ContractCampaignMapping
-- Modified By: Shalini Nair on 14/01/2015 to retrieve startdate and enddate from TC_ContractCampaignMapping
-- Modified By: Shalini Nair on 18/01/2015 to retrieve type of contract based on ContractBehaviour column in TC_ContractCampaignMapping
-- =============================================
CREATE PROCEDURE [dbo].[FindCampaign_15.11.7] @DealerId INT
	,@StateId INT
	,@CityIds VARCHAR(MAX)
	,@MakeId INT
	,@IsCarwaleCampaign BIT
	,@SortCriteria VARCHAR(100)
	,@SortDirection VARCHAR(100)
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
			WHEN (
					(
						CONVERT(DATE, GETDATE()) BETWEEN CONVERT(DATE, PDS.StartDate)
							AND (
									CASE 
										WHEN PDS.EndDate IS NULL
											THEN CONVERT(DATE, '2099-01-01')
										ELSE CONVERT(DATE, PDS.EndDate)
										END
									)
						)
					AND (
						(
							(
								(
									isnull(PDS.TotalCount, 0) < isnull(PDS.TotalGoal, 0)
									AND isnull(PDS.TotalGoal, 0) != 0
									)
								OR isnull(PDS.TotalGoal, 0) = 0
								)
							AND @IsCarwaleCampaign = 0
							)
						OR @IsCarwaleCampaign = 1
						)
					AND EXISTS (
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
				THEN CONVERT(BIT, 1)
			ELSE CONVERT(BIT, 0)
			END IsCampaignRunning
		,PDS.Phone
		,REPLACE(PDS.DealerEmailId, ',', ', ') DealerEmailId
		--,CONVERT(VARCHAR(10), PDS.StartDate, 103) StartDate
		,CONVERT(VARCHAR(10), CCM.StartDate, 103) StartDate
		,CASE 
			WHEN CCM.EndDate IS NULL
				THEN ''
			ELSE CONVERT(VARCHAR(10), CCM.EndDate, 103)
			END EndDate
		--,CASE 
		--	WHEN CCM.EndDate IS NULL
		--		THEN 'Lead Based'
		--	ELSE 'Date Based'
		--	END AS [Type]
		,CASE 
			WHEN CCM.ContractBehaviour = 1
				THEN 'Lead Based'
			WHEN CCM.ContractBehaviour = 2
				THEN 'Date Based'
			END AS [Type]
		--,PDS.TotalGoal
		--,PDS.TotalCount
		,CCM.TotalGoal
		,CCM.TotalDelivered as TotalCount
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
	FROM PQ_DealerSponsored PDS WITH (NOLOCK)
	INNER JOIN TC_ContractCampaignMapping CCM WITH (NOLOCK) ON PDS.Id = CCM.CampaignId
		AND (
			@MakeId = - 1
			OR @IsCarwaleCampaign = 1
			OR (
				@MakeId != - 1
				AND @IsCarwaleCampaign = 0
				AND EXISTS (
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
					OR (@DealerId = - 1)
					)
				)
			)
		AND (
			(
				D.CityId IN (
					SELECT items
					FROM [SplitText](@CityIds, ',')
					)
				AND @IsCarwaleCampaign = 0
				AND @CityIds != ''
				)
			OR @IsCarwaleCampaign = 1
			OR @CityIds = ''
			)
		AND (
			(
				D.StateId = @StateId
				AND @IsCarwaleCampaign = 0
				AND @StateId != - 1
				)
			OR @IsCarwaleCampaign = 1
			OR @StateId = - 1
			)
	ORDER BY CASE 
			WHEN @SortDirection = 'ASC'
				AND @SortCriteria = 'DealerName'
				THEN PDS.DealerName
			END ASC
		,CASE 
			WHEN @SortDirection = 'ASC'
				AND @SortCriteria = 'StartDate'
				THEN PDS.StartDate
			END ASC
		,CASE 
			WHEN @SortDirection = 'ASC'
				AND @SortCriteria = 'EndDate'
				THEN PDS.EndDate
			END ASC
		,CASE 
			WHEN @SortDirection = 'DESC'
				AND @SortCriteria = 'DealerName'
				THEN PDS.DealerName
			END DESC
		,CASE 
			WHEN @SortDirection = 'DESC'
				AND @SortCriteria = 'StartDate'
				THEN PDS.StartDate
			END DESC
		,CASE 
			WHEN @SortDirection = 'DESC'
				AND @SortCriteria = 'EndDate'
				THEN PDS.EndDate
			END DESC
END