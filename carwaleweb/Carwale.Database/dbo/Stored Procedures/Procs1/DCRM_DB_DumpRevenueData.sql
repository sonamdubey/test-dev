IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[DCRM_DB_DumpRevenueData]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[DCRM_DB_DumpRevenueData]
GO

	 
-- =============================================
-- Author:		Sunil M. Yadav
-- Create date: 01 March 2016
-- Description:	Dump revenue data in DCRM_DB_Revenue
-- =============================================
CREATE PROCEDURE [dbo].[DCRM_DB_DumpRevenueData]
	
AS
BEGIN
	
	DECLARE @CurrentDate DATE =   GETDATE() 
	DECLARE @NcdPackageIds VARCHAR(50) = '70,59';
	DECLARE @UcdPackageIds VARCHAR(50) = '34,30,31,32,33,81,47,77'
 
  	-- declare TempTable to store ucd and ncd revenue data
	DECLARE @TempRevenue TABLE (DealerId INT , ContractId INT ,UserId INT, BusinessUnitId INT , StartDate DATETIME , EndDate DATETIME ,Totaldelivered INT, CostPerUnit FLOAT , ContractBehaviour INT)

	--NCD revenue calcution and insert into temp table 
	-- dump lead base contract data 

	INSERT INTO @TempRevenue(DealerId, ContractId,UserId, BusinessUnitId, StartDate, EndDate,Totaldelivered, CostPerUnit,ContractBehaviour)
	SELECT  TCC.DealerId,TCC.ContractId,
	UD.UserId UserId,
	DAMU.BusinessUnitId,TCC.StartDate,TCC.EndDate,
	CASE WHEN Count(TC_InquiriesLeadId) > 0 THEN Count(TC_InquiriesLeadId)
		 ELSE 0
	END AS TotalDelivered,
	CASE WHEN DSD.NoOfLeads > 0 
		 THEN ( CAST (RDPF.ClosingAmount AS FLOAT ) /  CAST (DSD.NoOfLeads AS FLOAT)) 
		 ELSE 0 
	END	AS CostPerUnit ,

	TCC.ContractBehaviour
	FROM TC_ContractCampaignMapping TCC WITH(NOLOCK)
	JOIN RVN_DealerPackageFeatures RDPF WITH(NOLOCK) ON RDPF.DealerPackageFeatureID = TCC.ContractId
	JOIN DCRM_SalesDealer DSD WITH(NOLOCK) ON DSD.Id = RDPF.ProductSalesDealerId -- JOIN DCRM_SalesDealer For DSD.NoOfLeads
	JOIN TC_InquiriesLead TIL WITH(NOLOCK) ON TIL.ContractId = TCC.ContractId
	LEFT JOIN DCRM_ADM_UserDealers UD WITH(NOLOCK) ON UD.DealerId = TCC.DealerId AND UD.RoleId = 3 
	LEFT JOIN DCRM_ADM_MappedUsers DAMU WITH(NOLOCK) ON DAMU.OprUserId = UD.UserId
	WHERE CONVERT(DATE, TIL.CreatedDate) = @CurrentDate AND RDPF.PackageId IN (SELECT ListMember FROM fnSplitCSV(@NcdPackageIds))  -- RDPF.PackageId = 59 --> new car leads , 70 --> PQ sponsered
		  AND TCC.ContractBehaviour = 1 AND DSD.CampaignType <>1 AND TCC.ContractType <> 2  AND TCC.ContractStatus NOT IN (2,4) -- for lead based contract --> to count no. of leads delivered on that day.
	GROUP BY TCC.ContractId,TCC.DealerId,TCC.StartDate,TCC.EndDate,TCC.TotalGoal,TCC.TotalDelivered,RDPF.ClosingAmount,
	TCC.ContractBehaviour,TCC.ContractStatus,DAMU.BusinessUnitId,DSD.NoOfLeads,UD.UserId
	ORDER BY ContractId DESC

	-- dump duration based contract
	INSERT INTO		@TempRevenue(DealerId, ContractId,UserId, BusinessUnitId, StartDate, EndDate,Totaldelivered, CostPerUnit,ContractBehaviour)
	SELECT  TCC.DealerId,TCC.ContractId, 
			UD.UserId UserId
			,DAMU.BusinessUnitId,TCC.StartDate,TCC.EndDate,
			1 TotalDelivered,

			CASE WHEN DSD.PitchDuration >0 THEN  (CAST (RDPF.ClosingAmount AS FLOAT) / CAST(DSD.PitchDuration AS FLOAT))
				 ELSE 0
			END AS CostPerUnit,

			TCC.ContractBehaviour
	FROM	TC_ContractCampaignMapping TCC WITH(NOLOCK)
	JOIN	RVN_DealerPackageFeatures RDPF WITH(NOLOCK) ON RDPF.DealerPackageFeatureID = TCC.ContractId 
	JOIN DCRM_SalesDealer DSD WITH(NOLOCK) ON DSD.Id = RDPF.ProductSalesDealerId -- JOIN DCRM_SalesDealer For DSD.PitchDuration
	LEFT JOIN DCRM_ADM_UserDealers UD WITH(NOLOCK) ON UD.DealerId = TCC.DealerId AND UD.RoleId = 3 
	LEFT JOIN DCRM_ADM_MappedUsers DAMU WITH(NOLOCK) ON DAMU.OprUserId = UD.UserId
	WHERE  RDPF.PackageId IN (SELECT ListMember FROM fnSplitCSV(@NcdPackageIds)) AND TCC.ContractBehaviour = 2 AND CONVERT(DATE,TCC.EndDate) >= @CurrentDate -- RDPF.PackageId = 59 --> new car leads , 70 --> PQ sponsered
		   AND CONVERT(DATE,TCC.StartDate) <= @CurrentDate AND TCC.ContractStatus NOT IN (2,4) AND DSD.CampaignType <>1 AND TCC.ContractType <> 2 -- for lead based contract --> to count no. of leads delivered on that day.
	GROUP BY TCC.ContractId,TCC.DealerId,TCC.StartDate,TCC.EndDate,TCC.TotalGoal,TCC.TotalDelivered,RDPF.ClosingAmount,
	TCC.ContractBehaviour,TCC.ContractStatus,DAMU.BusinessUnitId,DSD.PitchDuration,UD.UserId
	ORDER BY ContractId DESC


	-- UCD  revenue calcution and insert into temp table 
   INSERT INTO @TempRevenue(DealerId, ContractId,UserId, BusinessUnitId, StartDate, EndDate,Totaldelivered, CostPerUnit,ContractBehaviour)
   SELECT DealerId,ContractId,UserId,BusinessUnitId,StartDate,EndDate,TotalDeliverd,CostPerUnit,ContractBehaviour
   FROM
		(
			SELECT  CPR.ConsumerId AS DealerId,CPR.ContractId,
					UD.UserId UserId
					,DAMU.BusinessUnitId,CPR.ApprovalDate AS StartDate,CCP.ExpiryDate AS EndDate,
					CASE 
						WHEN CONVERT(DATE,CPR.ApprovalDate) <= @CurrentDate AND CONVERT(DATE,CCP.ExpiryDate) >= @CurrentDate
						THEN  1
					 END AS TotalDeliverd,
					 CASE 
						WHEN CPR.ActualValidity > 0 
						THEN( CAST (CPR.ActualAmount AS FLOAT ) /  CAST (CPR.ActualValidity AS FLOAT)) 
						ELSE 0
					 END AS CostPerUnit,
					2 ContractBehaviour, -- all ucd packages are duration based i.e. ContractBehaviour = 2
					(ROW_NUMBER()OVER (PARTITION BY CPR.ConsumerId ORDER BY CPR.Id DESC)) AS ROWNUM 
			FROM ConsumerPackageRequests CPR WITH(NOLOCK)
			JOIN ConsumerCreditPoints CCP WITH(NOLOCK) ON CCP.ConsumerId = CPR.ConsumerId  
			LEFT JOIN DCRM_ADM_UserDealers UD WITH(NOLOCK) ON UD.DealerId = CPR.ConsumerId AND UD.RoleId = 3 
			LEFT JOIN DCRM_ADM_MappedUsers DAMU WITH(NOLOCK) ON DAMU.OprUserId = UD.UserId
			WHERE  CPR.ConsumerType = 1 AND CCP.ConsumerType = 1 AND CPR.ApprovalDate IS NOT NULL AND CCP.CustomerPackageId IN (SELECT ListMember FROM fnSplitCSV(@UcdPackageIds)) -- ConsumerType = 1 --> dealer , 2 --> individual
		) T WHERE ROWNUM = 1 AND TotalDeliverd IS NOT NULL

		-- dump data for seller leads package i.e. packageId = 39 , Ucd package 
	INSERT  INTO @TempRevenue(DealerId, ContractId,UserId, BusinessUnitId, StartDate, EndDate,Totaldelivered, CostPerUnit,ContractBehaviour)
	SELECT  DealerId,ContractId,UserId,BusinessUnitId,StartDate,EndDate,TotalDeliverd,CostPerUnit,ContractBehaviour
    FROM (
	SELECT	RDPF.DealerId,RDPF.DealerPackageFeatureID ContractId,UD.UserId UserId,DAMU.BusinessUnitId,RDPF.PackageStartDate StartDate,RDPF.PackageEndDate EndDate,
			1 TotalDeliverd ,( CAST (RDPF.ClosingAmount AS FLOAT ) /  CAST (DATEDIFF(DAY,RDPF.PackageStartDate,RDPF.PackageEndDate) AS FLOAT)) AS CostPerUnit ,
			2 ContractBehaviour,
			(ROW_NUMBER()OVER (PARTITION BY RDPF.DealerId ORDER BY RDPF.PackageEndDate DESC)) AS ROWNUM 

	FROM	RVN_DealerPackageFeatures RDPF WITH(NOLOCK)
			LEFT JOIN DCRM_ADM_UserDealers UD WITH(NOLOCK) ON UD.DealerId = RDPF.DealerId 
			LEFT JOIN DCRM_ADM_MappedUsers DAMU WITH(NOLOCK) ON DAMU.OprUserId = UD.UserId
	WHERE	RDPF.PackageId = 39 AND RDPF.PackageEndDate IS NOT NULL AND RDPF.PackageStartDate IS NOT NULL  -- RDPF.PackageId = 39 --> seller leads
			AND RDPF.IsActive = 1 AND CONVERT(DATE,RDPF.PackageStartDate) <= @CurrentDate AND CONVERT(DATE,RDPF.PackageEndDate) >= @CurrentDate AND UD.RoleId = 3 
			AND (RDPF.CampaignType IS NULL OR RDPF.CampaignType = 3) -- CampaignType = 3 --> paid
	) T 
	WHERE   ROWNUM = 1 AND TotalDeliverd IS NOT NULL


	-- Insert data into DCRM_DB_Revenue from @TempRevenue table
	INSERT INTO DCRM_DB_Revenue (DealerId,ContractId,UserId,BusinessUnitId,StartDate,EndDate,TotalDelivered,CostPerUnit,ContractBehaviour)
	SELECT DealerId,ContractId,UserId,BusinessUnitId,StartDate,EndDate,Totaldelivered,CostPerUnit,ContractBehaviour 
	FROM @TempRevenue
	WHERE UserId IS NOT NULL
	--SELECT * FROM @TempRevenue

END
