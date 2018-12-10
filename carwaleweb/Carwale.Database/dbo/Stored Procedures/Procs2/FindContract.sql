IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[FindContract]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[FindContract]
GO

	
-- =============================================
-- Author:		Vinay Kumar
-- Create date: 24th Nov 2015	
-- Description: Get Contract details
-- Updated By : Vinay Kumar  show extra details
-- Updated By : Vinay Kumar Prajapati 14th Desc 2015 Fix Bug (same contract showing multiple time due to associate Executive )
-- EXEC FindContract 5,1,'',-1,0,'EndDate','DESC',null,null,null,3
-- =============================================
CREATE PROCEDURE [dbo].[FindContract] 
     @DealerId INT
	,@StateId INT
	,@CityIds VARCHAR(MAX)
	,@MakeId INT
	,@IsCarwaleCampaign BIT
	,@SortCriteria VARCHAR(100)
	,@SortDirection VARCHAR(100)
	,@StartDate DATETIME = NULL
	,@EndDate   DATETIME = NULL
	,@ExecutiveName VARCHAR(100)=NULL
	,@ContractStatus INT  = NULL
AS
BEGIN

    --  Avoid Extra Message 
	 SET NOCOUNT ON 

	SELECT  CCM.ContractId,PDS.Id AS CampaignId,PDS.DealerId,D.Organization DealerName,D.IsDealerDeleted,D.TC_DealerTypeId,

	  OU.UserName
		,CASE 
			WHEN (
					(
						(
							(
								PDS.EndDate IS NOT NULL
								AND CONVERT(DATE, GETDATE()) BETWEEN CONVERT(DATE, PDS.StartDate)
									AND CONVERT(DATE, PDS.EndDate)
								--AND (ISNULL(PDS.DailyCount, 0) < ISNULL(PDS.DailyGoal, 999999999))
								AND @IsCarwaleCampaign = 0
								)
							OR @IsCarwaleCampaign = 1
							)
						OR (
							PDS.EndDate IS NULL
							AND PDS.StartDate <= CONVERT(DATE, GETDATE())
							AND ISNULL(PDS.TotalCount, 0) < PDS.TotalGoal
							--AND ISNULL(PDS.DailyCount, 0) < ISNULL(PDS.DailyGoal, PDS.TotalGoal)
							)
						)
					AND (
						(
							EXISTS (
								SELECT  DCM.Id
								FROM PQ_DealerCitiesModels DCM WITH (NOLOCK)
								WHERE DCM.CampaignId = PDS.Id
								)
							AND EXISTS (
								SELECT DATPM.ID
								FROM PQ_DealerAd_Template_Platform_Maping DATPM WITH (NOLOCK)
								WHERE DATPM.CampaignId = PDS.Id
								)
							)
						OR (
							EXISTS (
								SELECT PCSCR.Id
								FROM PQ_CrossSellCampaignRules PCSCR WITH (NOLOCK)
								INNER JOIN PQ_CrossSellCampaign PCSC WITH (NOLOCK) ON PCSCR.CrossSellCampaignId = PCSC.Id
									AND PCSC.CampaignId = PDS.Id
								)
							)
						)
					)
				THEN CONVERT(BIT, 1)
			ELSE CONVERT(BIT, 0)
			END IsCampaignRunning
		,PDS.Phone
		,REPLACE(PDS.DealerEmailId, ',', ', ') DealerEmailId

		--,CONVERT(VARCHAR(10), CCM.StartDate, 103) StartDate
		,CASE 
			WHEN CCM.StartDate IS NULL OR CCM.StartDate ='1900-01-01 00:00:00.000'
				THEN ''
			ELSE CONVERT(VARCHAR(10), CCM.StartDate, 103)
			END StartDate		
		,CASE 
			WHEN CCM.EndDate IS NULL OR  CCM.EndDate ='1900-01-01 00:00:00.000'
				THEN ''
			ELSE CONVERT(VARCHAR(10), CCM.EndDate, 103)
			END EndDate
		,CASE 
			WHEN PDS.EndDate IS NULL
				THEN 'Lead Based'
			ELSE 'Date Based'
			END AS [Type]
		,PDS.TotalCount
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
		,CASE WHEN CCM.ContractStatus = 1 THEN 'Active' WHEN CCM.ContractStatus = 2 THEN 'Paused'
			 WHEN CCM.ContractStatus = 3 THEN 'Closed' WHEN CCM.ContractStatus = 4 THEN 'Aborted'
			END [Status]
	    ,CASE CCM.ContractBehaviour WHEN 1 THEN 'Lead Based' WHEN 2 THEN 'Duration Based' ELSE '--' END  AS ContractBehaviour 
		,CASE CCM.ContractType WHEN 1 THEN 'Normal Lead' WHEN 2 THEN 'Replacement' ELSE '--' END  AS ContractType 
		,CASE CCM.ContractType WHEN 2 THEN CONVERT(VARCHAR,CCM.ReplacementContractId) ELSE ''   END  AS ContractIdReplace
		,CASE CCM.ContractStatus WHEN 1 THEN 'Active' WHEN 2 THEN 'Paused' WHEN 3 THEN 'Closed' WHEN 4 THEN 'Aborted'  END  AS ContractStatus --Remove This Line  After Live VKP 
		,CCM.TotalGoal
		,CCM.TotalDelivered,
		CASE CCM.ContractBehaviour  --- Get percentage lead based ->1 and duration based ->2 
			  WHEN 1 THEN
			   ( CASE ISNULL(CCM.TotalGoal,0) WHEN 0 THEN 0.00 ELSE ROUND(((ISNULL(CCM.TotalDelivered,0)*100.0)/ CCM.TotalGoal),2) END )
			  WHEN 2 THEN	
			  (CASE WHEN ISNULL(DATEDIFF(DAY, CCM.StartDate, CCM.EndDate),0) <=  0 THEN 0.00 
			   WHEN  ISNULL(DATEDIFF(DAY, CCM.StartDate, CCM.EndDate),0)<=ISNULL(DATEDIFF(DAY, CCM.StartDate, GETDATE()),0)  THEN 100.00 ---when (Duration based) last day is over now 
			     ELSE ROUND(((ISNULL(DATEDIFF(DAY, CCM.StartDate, GETDATE()),0)*100.0)/ ISNULL(DATEDIFF(DAY, CCM.StartDate, CCM.EndDate),0)),2) END )
			  ELSE 0 END AS Completion

		--,CASE ISNULL(CCM.TotalGoal,0) WHEN 0 THEN 0.00 ELSE ROUND(((ISNULL(CCM.TotalDelivered,0)*100.0)/ CCM.TotalGoal),2) END AS Completion
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
			AND CCM.ContractStatus = @ContractStatus
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
   AND ((( CCM.StartDate BETWEEN @StartDate AND @EndDate ) ) OR @StartDate IS NULL OR @EndDate IS NULL)		
   LEFT JOIN DCRM_ADM_UserDealers AS DUD WITH(NOLOCK) ON DUD.DealerId= D.ID AND DUD.RoleId=3
   LEFT JOIN OprUsers AS OU WITH(NOLOCK) ON OU.Id=DUD.UserId 
   WHERE ( (@ExecutiveName IS NULL) OR (OU.UserName  LIKE '%'+ @ExecutiveName+ '%'))
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



