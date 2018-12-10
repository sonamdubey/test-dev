IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_GetPastContracts]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_GetPastContracts]
GO
	

-- =============================================
-- Author:		Ashwini Dhamankar 
-- Create date: Feb 4,2016
-- Description:	To get contract history of dealer
-- Modified by : Kritika Choudhary on 4th March 2016, added StartDate,EndDate,DeliveryType,Quantity,ClosingAmount,ContractBehaviour and join with RVN_DealerPackageFeatures and DCRM_SalesDealer 
-- EXEC TC_GetPastContracts 995
-- Modified by : Kritika Choudhary on 2nd June 2016, added LPAImageUrl
-- Modified by : Kritika Choudhary on 22nd June 2016, modified Quantity
-- Modifeied by : Kritika CHoudhary on 23rd June 2016, added isnull condition for lpa isactive
-- =============================================
CREATE PROCEDURE [dbo].[TC_GetPastContracts] 
	@BranchId BIGINT
AS
BEGIN
	SET NOCOUNT ON;
	
	SELECT DISTINCT TOP 5 CC.Id, CC.ContractId,CC.CampaignId,CC.StartDate,CC.EndDate,--CC.TotalGoal,CC.TotalDelivered,
	CASE WHEN CC.ContractBehaviour=1 THEN 'Leads' ELSE 'Branding' END AS DeliveryType,
	CASE WHEN CC.ContractBehaviour=1 THEN CC.TotalDelivered ELSE datediff(day,CC.StartDate,CC.EndDate) + 1 END AS Quantity
	,CC.ContractBehaviour-- DP.ClosingAmount 
	, STUFF (( SELECT ','+ HostUrl + '0x0' + OriginalImgPath 
                    FROM M_AttachedLpaDetails WITH(NOLOCK)
                    WHERE IsNull(IsActive,1)=1 and SalesDealerId =( -- Modifeied by : Kritika CHoudhary on 23rd June 2016, added isnull condition
	SELECT TOP 1 RDPF.ProductSalesDealerId
	FROM RVN_DealerPackageFeatures RDPF WITH(NOLOCK)
	JOIN TC_ContractCampaignMapping TCC WITH(NOLOCK) ON TCC.ContractId = RDPF.DealerPackageFeatureID
	WHERE TCC.DealerId = @BranchId AND TCC.ContractStatus=3 and TCC.ContractId = CC.ContractId
	)FOR XML PATH('') 
	),1,1,'' )AS  LPAImageUrl
   -- INTO #TempTable
    FROM TC_ContractCampaignMapping CC WITH(NOLOCK)
	JOIN RVN_DealerPackageFeatures DP WITH(NOLOCK) ON DP.DealerPackageFeatureID = CC.ContractId
	JOIN DCRM_SalesDealer SD WITH(NOLOCK) ON SD.Id = DP.ProductSalesDealerId
    WHERE CC.DealerId = @BranchId AND CC.ContractStatus = 3
    --ORDER BY Id DESC
	ORDER BY ContractId DESC    
  
 --   ;WITH CTE AS
	--(
	--	SELECT  T.ContractId
	--	       -- ,COUNT(DISTINCT TCIL.TC_LeadId) TotalLeads
	--	      --  ,COUNT(DISTINCT (CASE WHEN NCI.BookingStatus=32 THEN NCI.TC_NewCarInquiriesId END)) BookedLeads
	--			,T.StartDate
	--			,T.EndDate
	--			,T.DeliveryType  
	--			,T.Quantity
	--			-- ,T.ClosingAmount
	--	FROM #TempTable  AS T
	--	LEFT JOIN TC_NewCarInquiries AS NCI WITH(NOLOCK) ON T.ContractId=NCI.ContractId
	--	LEFT JOIN TC_InquiriesLead AS TCIL WITH(NOLOCK)  ON  TCIL.TC_InquiriesLeadId=NCI.TC_InquiriesLeadId AND TCIL.TC_LeadInquiryTypeId = 3
	--	GROUP BY T.ContractId,T.StartDate,T.EndDate
	--			,T.DeliveryType  
	--			,T.Quantity
	--			-- ,T.ClosingAmount
	--) 
		   
	--SELECT ContractId,--,TotalLeads,BookedLeads, CASE WHEN TotalLeads = 0 THEN 0 ELSE ((BookedLeads*100)/TotalLeads) END AS Conversion
	--StartDate, EndDate, DeliveryType, Quantity,
 --   STUFF (( SELECT ','+ HostUrl + '0x0' + OriginalImgPath 
 --                   FROM M_AttachedLpaDetails WITH(NOLOCK)
 --                   WHERE IsActive=1 and SalesDealerId =(
	--SELECT RDPF.ProductSalesDealerId
	--FROM RVN_DealerPackageFeatures RDPF 
	--JOIN TC_ContractCampaignMapping TCC ON TCC.ContractId = RDPF.DealerPackageFeatureID
	--WHERE TCC.DealerId = @BranchId AND TCC.ContractStatus=3 and TCC.ContractId = T.ContractId
	--)FOR XML PATH('') 
	--),1,1,'' )AS  LPAImageUrl-- , ClosingAmount
	--FROM #TempTable T
	--ORDER BY ContractId DESC

   -- DROP TABLE #TempTable

END


