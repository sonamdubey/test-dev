IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_GetContractStatus]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_GetContractStatus]
GO

	
------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------

-- =============================================
-- Author:		Ruchira Patil
-- Create date: 25th Jan 2016
-- Description:	To get active contract details of a dealer
-- Modified By : Nilima More On 27 Jan 2016(Added GoalPercentage)
-- EXEC TC_GetContractStatus 5
-- Modified By : Nilima More(Instead of IsActiveContract addded Logic for Contract status and End date > current date).
-- Modified By : Nilima More (Fetch L2 and L3 for requesting Renew request,If Contract is expired Show latest Contract)
-- Modified By : Ashwini Dhamankar on March 3,2016 (Fetched LPA,ContractStaus,)
-- Modified By : Khushaboo Patil on 2nd june 2016 fetched ExpectedClosureDate.
-- Modified By : Nilima More On 9th June 2016,Added condition of isActiveContract.
-- [TC_GetContractStatus] 10819
-- =============================================
CREATE PROCEDURE [dbo].[TC_GetContractStatus] 
	@ContractId INT 
AS
BEGIN
		SELECT DISTINCT Cs.Status As ContractStatus,P.Name AS PackageName,ISNULL(CM.TotalGoal,0)TotalGoal,   --total goal will be lead signed for lead based
		ISNULL(CM.TotalDelivered,0)TotalDelivered,   --leadsdelivered 
		DATEDIFF(dd,CM.StartDate,GETDATE()) ActiveSince,   -- Duration expired for durationbased
		DATEDIFF(dd,CM.StartDate,EndDate) TotalDays,   --totaldays will be duration signed for duration based contract
		CM.StartDate,CM.EndDate,CM.IsActiveContract
		,CM.ContractBehaviour -- 1 is lead based and 2 is date based
		,CASE ISNULL(CM.ContractBehaviour,0) WHEN  1 THEN  CAST(CAST(CM.TotalGoal AS float) * (0.85) AS NUMERIC(15,0))  ELSE  0 END leadMargin
		,DATEDIFF(dd,CM.StartDate,EndDate-7) DateMargin
		,D.EmailId Email
		,CM.Id TC_ContractCampaignMappingId
		,D.MobileNo Mobile
		,d.ID DealerId
		,D.FirstName DealerName
		,(SELECT TOP 1 OU1.UserName FROM OprUsers AS OU1(NOLOCK) WHERE OU1.Id=DAUD.UserId AND DAUD.RoleId=3) L3
		,(SELECT TOP 1 OU1.LoginId FROM OprUsers AS OU1(NOLOCK) WHERE OU1.Id=DAUD.UserId AND DAUD.RoleId=3) + '@carwale.com' as l3Mail
		,(SELECT OU.UserName FROM DCRM_ADM_MappedUsers AS DAM1(NOLOCK) LEFT JOIN OprUsers OU(NOLOCK) ON OU.ID= DAM1.OprUserId WHERE DAM1.NodeRec=DAMU.NodeRec.GetAncestor(1) ) L2
		,(SELECT OU.LoginId FROM DCRM_ADM_MappedUsers AS DAM1(NOLOCK) LEFT JOIN OprUsers OU(NOLOCK) ON OU.ID= DAM1.OprUserId WHERE DAM1.NodeRec=DAMU.NodeRec.GetAncestor(1) ) + '@carwale.com'  as l2Mail
		,AD.HostURL+'0X0'+AD.OriginalImgPath AS LPA
		,D.Organization AS Organization
		,SD.ClosingDate AS ExpectedClosureDate
		,
		CASE ContractBehaviour
		WHEN 1
			THEN 'StartDate-'+ CONVERT(VARCHAR, CM.StartDate, 106) +'-'+ CONVERT(VARCHAR, TotalGoal) + '-Leads' 
		ELSE 'StartDate-'+ CONVERT(VARCHAR, CM.StartDate, 106) +'-'+ CONVERT(VARCHAR, DATEDIFF(dd, CM.StartDate, EndDate)) + '-Days' 
		END  ContractId

		INTO #Temp
		FROM TC_ContractCampaignMapping CM WITH (NOLOCK)
		INNER JOIN Dealers D WITH (NOLOCK) ON D.ID = CM.DealerId
		INNER JOIN ContractStatus CS WITH(NOLOCK) ON CS.Id = CM.ContractStatus 
		INNER JOIN RVN_DealerPackageFeatures RDP WITH(NOLOCK) ON RDP.DealerPackageFeatureID = CM.ContractId
		INNER JOIN Packages P WITH(NOLOCK) ON P.Id = RDP.PackageId
		INNER JOIN DCRM_SalesDealer SD WITH(NOLOCK) ON SD.Id = RDP.ProductSalesDealerId
		LEFT JOIN DCRM_ADM_UserDealers DAUD WITH (NOLOCK) ON DAUD.DealerId = D.ID AND DAUD.RoleId = 3  --sales field (L3)
		LEFT JOIN DCRM_ADM_MappedUsers AS DAMU WITH(NOLOCK) ON DAMU.OprUserId=DAUD.UserId	
		LEFT JOIN M_AttachedLpaDetails AD WITH(NOLOCK) ON AD.SalesDealerId = SD.Id AND AD.HostURL IS NOT NULL
		WHERE 
		CM.ContractId = @ContractId AND
		CM.ContractStatus IN (1) AND 
		SD.PitchingProduct IN (59,70) AND --new car lead - 70 and PQ sponsor - 59
		CONVERT(DATE,CM.StartDate) <= CONVERT(DATE,GETDATE()) AND
		CONVERT(DATE,ISNULL(CM.EndDate,GETDATE())) >= CONVERT(DATE,GETDATE()) 
	

		SELECT TOP 1
		    *,
		    STUFF
		    (
		        (
		            SELECT ',' + LPA
		            FROM #Temp M
		            FOR XML PATH('')
		        ), 1, 1, ''
		    ) AS LPAs
		FROM
		#Temp
	
	DROP TABLE #Temp
END
-------------------------------------------------------------------------------------------

