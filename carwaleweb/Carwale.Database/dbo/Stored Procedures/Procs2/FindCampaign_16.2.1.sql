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
-- PENDING: Check for Dealer -> IsPremium
-- Modeified By Chetan on <27/11/2015>- Changing return text of column 'type' - 'Lead Wise' to 'Lead Based' and 'No Limit' to 'Date Based',
-- EXEC FindCampaign 7101,-1,'',-1,false,'EndDate','DESC','7107,3797'
-- Modified By : Shalini Nair on 24/12/2015 to retrieve Masking number from MM_SellerMobileMasking
-- Modified By: Shalini Nair on 13/01/2016 to retrieve TotalCount and TotalGoal from TC_ContractCampaignMapping
-- Modified By: Shalini Nair on 14/01/2016 to retrieve startdate and enddate from TC_ContractCampaignMapping
-- Modified By: Shalini Nair on 18/01/2016 to retrieve type of contract based on ContractBehaviour column in TC_ContractCampaignMapping
-- Modified By: Shalini Nair on 21/01/2016 to retrieve records based on @DealerIds(comma separated) value
-- Modified By: Shalini Nair on 28/01/2016 to retrieve CarwaleTollFree number based on IsDefaultnumber column
-- =============================================
CREATE PROCEDURE [dbo].[FindCampaign_16.2.1] @DealerId INT
	,@StateId INT
	,@CityIds VARCHAR(MAX)
	,@MakeId INT
	,@IsCarwaleCampaign BIT
	,@SortCriteria VARCHAR(100)
	,@SortDirection VARCHAR(100)
	,@DealerIds VARCHAR(100) = NULL
AS
BEGIN
	--DECLARE @TollFreeNumber VARCHAR(15)

	--SELECT TOP 1 @TollFreeNumber = TollFreeNumber
	--FROM CarwaleTollFreeNumber WITH (NOLOCK)

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
		,Phone = CASE 
			WHEN PDS.IsDefaultNumber !=0 
				THEN CTOLL.TollFreeNumber
			ELSE MM.MaskingNumber
			END
		,REPLACE(PDS.DealerEmailId, ',', ', ') DealerEmailId
		,CONVERT(VARCHAR(10), CCM.StartDate, 103) StartDate
		,CASE 
			WHEN CCM.EndDate IS NULL
				THEN ''
			ELSE CONVERT(VARCHAR(10), CCM.EndDate, 103)
			END EndDate
		,CASE 
			WHEN CCM.ContractBehaviour = 1
				THEN 'Lead Based'
			WHEN CCM.ContractBehaviour = 2
				THEN 'Date Based'
			END AS [Type]
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
			(@DealerIds IS NULL OR PDS.DealerId IN (SELECT ListMember
				FROM fnSplitCSVValuesWithIdentity(@DealerIds)))
			OR
			(
				@IsCarwaleCampaign = 1
				AND PDS.DealerId = 9350
				)
			OR (
				@IsCarwaleCampaign = 0
				AND ((PDS.DealerId = @DealerId 
						and @DealerId != - 1)
				or (@DealerIds = '' AND @DealerId = - 1))
					))
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
	LEFT JOIN MM_SellerMobileMasking MM WITH (NOLOCK) ON MM.LeadCampaignId = PDS.Id
		AND PDS.DealerId = MM.ConsumerId
	LEFT JOIN CarwaleTollFreeNumber CTOLL WITH(NOLOCK) ON CTOLL.Id = PDS.IsDefaultNumber

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


/****** Object:  StoredProcedure [dbo].[InsertorUpdatePQCampaign_v16.1.1]    Script Date: 28/01/2016 17:23:34 ******/
SET ANSI_NULLS ON
