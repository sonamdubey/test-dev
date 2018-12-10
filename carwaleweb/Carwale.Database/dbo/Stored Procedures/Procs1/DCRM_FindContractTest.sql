IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[DCRM_FindContractTest]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[DCRM_FindContractTest]
GO
	-- =============================================
-- Author:		Vinay Kumar
-- Create date: 24th Nov 2015	
-- Description: Get Contract details
-- Updated By : Vinay Kumar  show extra details
-- Updated By : Vinay Kumar Prajapati 14th Desc 2015 Fix Bug (same contract showing multiple time due to associate Executive )
-- EXEC DCRM_FindContract 5,1,'',-1,0,'EndDate','DESC',null,null,3,6,null,null
-- Modified By : Sunil Yadav On 17th Dec 2015
-- Description : Executive filter modified(Changed to executiveId instead of executive name) and Expiry filter added
-- Modified by : Ajay Singh on 29 jan 2016
-- Description : Added two More Parameter TranscationId and BillingType
-- Modified by : Amit Yadav 29 Jan 2016
-- Purpose : To get more fields(state,zone,city,l2,l3,BO,ApprovedLeads,PitchQty,DeliveryLeads,FinalAmout,CostPerLeads)and also get the totals leads or duration(days) in ApprovedLeads
-- EXEC DCRM_FindContractTest -1,-1,' ',-1,0,'DealerName','ASC',null,null,null,5
-- Modified By :  Vinay Kumar Prajapati 2nd Feb 2015  
-- Purpose  : to get expired and renewed contract
-- =============================================
CREATE PROCEDURE [dbo].[DCRM_FindContractTest] 
     @DealerId INT
	,@StateId INT
	,@CityIds VARCHAR(MAX)
	,@MakeId INT
	,@IsCarwaleCampaign BIT
	,@SortCriteria VARCHAR(100)
	,@SortDirection VARCHAR(100)
	,@StartDate DATETIME = NULL
	,@EndDate   DATETIME = NULL
	,@ExecutiveId INT=NULL
	,@ContractStatus INT  = NULL
	,@ContractEndDateFrom DATETIME = NULL
	,@ContractEndDateTo DATETIME = NULL
	,@TranscationId INT = NULL
	,@BillingType  INT = NULL
	
AS
BEGIN

    --  Avoid Extra Message 
	 SET NOCOUNT ON 

	SELECT CCM.DealerId,DPT.TransactionId, CCM.ContractId,CCM.CampaignId AS CampaignId,D.Organization DealerName,D.IsDealerDeleted,D.TC_DealerTypeId,

	  OU.UserName
		,CASE 
			WHEN (
					(
						(
							(
								CCM.EndDate IS NOT NULL
								AND CONVERT(DATE, GETDATE()) BETWEEN CONVERT(DATE, CCM.StartDate)
									AND CONVERT(DATE, CCM.EndDate)
								--AND (ISNULL(PDS.DailyCount, 0) < ISNULL(PDS.DailyGoal, 999999999))
								AND @IsCarwaleCampaign = 0
								)
							OR @IsCarwaleCampaign = 1
							)
						OR (
							CCM.EndDate IS NULL
							AND CCM.StartDate <= CONVERT(DATE, GETDATE())
							AND ISNULL(CCM.TotalDelivered, 0) < CCM.TotalGoal
							--AND ISNULL(PDS.DailyCount, 0) < ISNULL(PDS.DailyGoal, PDS.TotalGoal)
							)
						)
					AND (
						(
							EXISTS (
								SELECT  DCM.Id
								FROM PQ_DealerCitiesModels DCM WITH (NOLOCK)
								WHERE DCM.CampaignId = CCM.CampaignId
								)
							AND EXISTS (
								SELECT DATPM.ID
								FROM PQ_DealerAd_Template_Platform_Maping DATPM WITH (NOLOCK)
								WHERE DATPM.CampaignId = CCM.CampaignId
								)
							)
						OR (
							EXISTS (
								SELECT PCSCR.Id
								FROM PQ_CrossSellCampaignRules PCSCR WITH (NOLOCK)
								INNER JOIN PQ_CrossSellCampaign PCSC WITH (NOLOCK) ON PCSCR.CrossSellCampaignId = PCSC.Id
									AND PCSC.CampaignId = CCM.CampaignId
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
			WHEN CCM.EndDate IS NULL
				THEN 'Lead Based'
			ELSE 'Date Based'
			END AS [Type]
		,CCM.TotalGoal AS TotalCount
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
		--Below feilds added by Amit Yadav 29 Jan 2016
		,CASE CCM.ContractBehaviour 
			WHEN 1 THEN 
			( CAST (CASE ISNULL(CCM.TotalGoal,0) WHEN 0 THEN 0 ELSE CCM.TotalGoal END AS VARCHAR(20))) +CAST(' Leads ' AS VARCHAR(10))
			WHEN 2 THEN
			(CAST (CASE WHEN ISNULL(DATEDIFF(DAY, CCM.StartDate, CCM.EndDate),0) <=  0 THEN 0
				 ELSE  DATEDIFF(DAY, CCM.StartDate, CCM.EndDate) END AS VARCHAR(20)))+CAST(' Days ' AS VARCHAR(10)) 
			END AS ApprovedQty-- Leads or Days which are approved
		,CASE CCM.ContractBehaviour 
			WHEN 1 THEN 
			( CAST (CASE ISNULL(CCM.TotalDelivered,0) WHEN 0 THEN 0 ELSE CCM.TotalDelivered END AS VARCHAR(20))) +CAST(' Leads ' AS VARCHAR(10))
			WHEN 2 THEN
			(CAST (CASE WHEN ISNULL(DATEDIFF(DAY, CCM.StartDate, GETDATE()),0) <=  0 THEN 0 
				   WHEN  ISNULL(DATEDIFF(DAY, CCM.StartDate, CCM.EndDate),0) <= ISNULL(DATEDIFF(DAY, CCM.StartDate, GETDATE()),0)  THEN DATEDIFF(DAY, CCM.StartDate, CCM.EndDate)  
				   ELSE  DATEDIFF(DAY, CCM.StartDate, CCM.EndDate) END AS VARCHAR(20))) + CAST(' Days ' AS VARCHAR(10))
			END AS DeliveredQty --Leads or Days which are already delivered
		,CASE CCM.ContractBehaviour 
			WHEN 1 THEN 
			( CAST (CASE ISNULL(DSD.NoOfLeads,0) WHEN 0 THEN 0 ELSE DSD.NoOfLeads END AS VARCHAR(20))) +CAST(' Leads ' AS VARCHAR(10))
			WHEN 2 THEN
			(CAST (CASE ISNULL(DSD.PitchDuration,0) WHEN 0 THEN 0 ELSE DSD.PitchDuration END AS VARCHAR(20))) +CAST(' Days ' AS VARCHAR(10)) 
			END AS PitchedQty--Leads or Days which were entered at the time of pitching
		,S.Name AS State,BZ.ZoneName AS Zone,C.Name AS City--To get the State,Zone,City
		,(SELECT TOP 1 OU1.UserName FROM OprUsers AS OU1(NOLOCK) WHERE OU1.Id=DUD.UserId AND DUD.RoleId=3 ) AS L3Name --L3 associated to the dealer
		,(SELECT TOP 1 OU2.UserName FROM OprUsers AS OU2(NOLOCK) WHERE OU2.Id=DUD.UserId AND DUD.RoleId=4 ) AS BOName --Back Office executive associated to the dealer
		,(SELECT OU.UserName FROM DCRM_ADM_MappedUsers AS DAM1(NOLOCK) LEFT JOIN OprUsers OU(NOLOCK) ON OU.ID= DAM1.OprUserId WHERE DAM1.NodeRec=DAMU.NodeRec.GetAncestor(1)) AS L2Name--L2 associated to that L3
		,DPT.FinalAmount --Amount at Transaction level
		,CCM.CostPerLead --Cost Per Leads entered at time of delivery,
		  -- @ContractStatus: 5 - Expired not renewed Above 6 month,
		  -- @ContractStatus: 7 - Expired not renewed within 6 month,
		  -- @ContractStatus: 8 - Renewed 
		,CASE @ContractStatus
			 WHEN 5 THEN (SELECT TOP 1  (CASE WHEN ISNULL(DATEDIFF(DAY, GETDATE(), STCM.EndDate),0) <=  180 THEN 1 ELSE 0 END) FROM TC_ContractCampaignMapping AS STCM WITH(NOLOCK)  WHERE STCM.StartDate >=CCM.StartDate AND STCM.DealerId= CCM.DealerId  ORDER BY STCM.StartDate DESC)   
			 WHEN 7 THEN (SELECT TOP 1  (CASE WHEN ISNULL(DATEDIFF(DAY, GETDATE(), STCM.EndDate),0) <=  180 THEN 1 ELSE 0 END) FROM TC_ContractCampaignMapping AS STCM WITH(NOLOCK)  WHERE STCM.StartDate >=CCM.StartDate AND STCM.DealerId= CCM.DealerId  ORDER BY STCM.StartDate DESC)
			 WHEN 8 THEN (SELECT TOP 1  (CASE WHEN (ISNULL(DATEDIFF(DAY, STCM.EndDate, CCM.StartDate),0) <=  180 AND ISNULL(DATEDIFF(DAY, STCM.EndDate, CCM.StartDate),0) >= 0) THEN 1 ELSE 0 END) FROM TC_ContractCampaignMapping AS STCM WITH(NOLOCK)  WHERE STCM.ContractStatus=3 AND STCM.DealerId= CCM.DealerId  ORDER BY STCM.EndDate DESC) 
	    END AS DefinitionStatus  INTO #TempTable
	FROM 
	
	TC_ContractCampaignMapping CCM WITH (NOLOCK)
	INNER JOIN PQ_DealerSponsored PDS WITH (NOLOCK) ON PDS.Id = CCM.CampaignId
		--AND (
		--	@MakeId = - 1
		--	OR @IsCarwaleCampaign = 1
		--	OR (
		--		@MakeId != - 1
		--		AND @IsCarwaleCampaign = 0
		--		AND EXISTS (
		--			SELECT DCM.Id
		--			FROM PQ_DealerCitiesModels DCM WITH (NOLOCK)
		--			WHERE DCM.CampaignId = PDS.Id
		--				AND DCM.MakeId = @MakeId
		--			)
		--		)
		--	)
		
	INNER JOIN Dealers D WITH (NOLOCK) ON D.ID = CCM.DealerId
		AND Organization IS NOT NULL
		AND Organization <> ''
		AND D.IsDealerDeleted = 0
		AND D.IsTCDealer = 1
		AND (
			(
				@IsCarwaleCampaign = 1
				AND ccm.DealerId = 9350
				)
			OR (
				@IsCarwaleCampaign = 0
				AND (
					(
						CCM.DealerId = @DealerId
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
   AND ((( CCM.EndDate BETWEEN @ContractEndDateFrom AND @ContractEndDateTo ) ) OR @ContractEndDateFrom IS NULL OR @ContractEndDateTo IS NULL)
   LEFT JOIN DCRM_ADM_UserDealers AS DUD WITH(NOLOCK) ON DUD.DealerId= D.ID AND DUD.RoleId IN (3,4)
   LEFT JOIN OprUsers AS OU WITH(NOLOCK) ON OU.Id=DUD.UserId 
   LEFT JOIN RVN_DealerPackageFeatures DFT WITH(NOLOCK) ON DFT.DealerPackageFeatureID = CCM.ContractId
   LEFT JOIN DCRM_PaymentTransaction DPT WITH(NOLOCK) ON DPT.TransactionId = DFT.TransactionId
   LEFT JOIN DCRM_ADM_MappedUsers AS DAMU WITH(NOLOCK) ON DAMU.OprUserId=DUD.UserId
   LEFT JOIN TC_BrandZone AS BZ WITH(NOLOCK) ON BZ.TC_BrandZoneId=D.TC_BrandZoneId
   LEFT JOIN Cities AS C(NOLOCK) ON C.ID = D.CityId
   LEFT JOIN States S(NOLOCK) ON S.ID = C.StateId  
   --LEFT JOIN DCRM_SalesDealer DSD WITH(NOLOCK) ON DSD.DealerId = CCM.DealerId
   LEFT JOIN DCRM_SalesDealer DSD WITH(NOLOCK) ON DSD.Id = DFT.ProductSalesDealerId
   WHERE
   	 (( CCM.ContractStatus = @ContractStatus OR @ContractStatus = 0)
				 OR (	
					(@ContractStatus = 5 OR @ContractStatus = 7 AND CCM.ContractStatus = 3 ) OR
						((@ContractStatus = 6 AND CCM.ContractStatus = 1) 
						AND (
								((CCM.ContractBehaviour = 1) AND (ISNULL(CCM.TotalGoal,0)<> 0 ) AND ((CCM.TotalDelivered*100/ CCM.TotalGoal ) BETWEEN  85 AND 99 ) )
								OR (CCM.ContractBehaviour = 2 AND (DATEDIFF(day,CONVERT(DATE ,GETDATE()),ISNULL(CCM.EndDate,CONVERT(DATE, '2099-01-01'))) BETWEEN  0 AND 7))
							)
						) 
					)
					OR 
					  (@ContractStatus = 8 AND CCM.ContractStatus = 1) -- Added By vinay kumar prajapati 
				)
   AND ( (@ExecutiveId IS NULL) OR (OU.Id=@ExecutiveId))
   AND ( (@TranscationId IS NULL) OR (DPT.TransactionId = @TranscationId))
   AND ( (@BillingType IS NULL ) OR (DFT.CampaignType = @BillingType))
	ORDER BY CASE 
			WHEN @SortDirection = 'ASC'


				AND @SortCriteria = 'DealerName'
				THEN D.Organization
			END ASC
		,CASE 
			WHEN @SortDirection = 'ASC'
				AND @SortCriteria = 'StartDate'
				THEN CCM.StartDate
			END ASC
		,CASE 
			WHEN @SortDirection = 'ASC'
				AND @SortCriteria = 'EndDate'
				THEN CCM.EndDate
			END ASC
		,CASE 
			WHEN @SortDirection = 'DESC'
				AND @SortCriteria = 'DealerName'
				THEN D.Organization
			END DESC
		,CASE 
			WHEN @SortDirection = 'DESC'
				AND @SortCriteria = 'StartDate'
				THEN CCM.StartDate
			END DESC
		,CASE 
			WHEN @SortDirection = 'DESC'
				AND @SortCriteria = 'EndDate'
				THEN CCM.EndDate
			END DESC


	IF @ContractStatus IN(5,7,8)
		BEGIN
			SELECT * FROM #TempTable AS TT WHERE  TT.DefinitionStatus = 1
		END
	ELSE
		BEGIN
		  SELECT * FROM #TempTable AS TT
		END 


      DROP TABLE #TempTable
END



