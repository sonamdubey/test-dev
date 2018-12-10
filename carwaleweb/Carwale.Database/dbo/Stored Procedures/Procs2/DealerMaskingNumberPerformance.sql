IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[DealerMaskingNumberPerformance]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[DealerMaskingNumberPerformance]
GO

	-- =============================================
-- Author:		Avishkar
-- Create date: 05-02-2016
-- Description:	To report dealers with a lesser percentage of masking number leads
-- =============================================
CREATE PROCEDURE [dbo].[DealerMaskingNumberPerformance]	
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

		DECLARE @previousDay as date = CAST (getdate()-1 as date)

		SELECT DISTINCT d.id DealerId,
		                d.organization DealerName		
					   ,COUNT(DISTINCT tc.Mobile) as 'Total_Leads' 
					   ,0 AS  'Masking_Leads'
		INTO #temp
		FROM dealers AS D WITH(NOLOCK) 
		LEFT JOIN TC_CustomerDetails tc  WITH(NOLOCK)  ON D.ID = TC.BranchId		
		LEFT JOIN TC_Lead AS F   WITH(NOLOCK)  ON TC.Id = F.TC_CustomerId		
		LEFT JOIN TC_InquiriesLead c   WITH(NOLOCK)  ON C.TC_LeadId = f.TC_LeadId		
		LEFT JOIN TC_NewCarInquiries a    WITH(NOLOCK)  ON a.TC_InquiriesLeadId = c.TC_InquiriesLeadId		
		LEFT JOIN TC_InquirySource b    WITH(NOLOCK)  ON a.TC_InquirySourceId = b.ID		
		LEFT JOIN PQDealerAdLeads l   WITH(NOLOCK)  ON l.PushStatus=a.TC_NewCarInquiriesId	
		LEFT JOIN vwmmv v  WITH(NOLOCK)   ON v.VersionId=a.VersionId	
		LEFT JOIN Cities ci  WITH(NOLOCK)  ON ci.id=d.cityid	
		LEFT JOIN LA_Agencies ag ON ag.id=l.PlatformId	
		LEFT JOIN PqLeadClickSource_Master cs on cs.LeadClickSource=l.LeadClickSource	
		WHERE CONVERT(DATE,a.CreatedOn) =@previousDay 
		AND f.BranchId not in ('9350','9735','15418','15756','16345','16403','16183','10216','535','4815','20452')		
			  AND b.Product_Newcar = 1 --and (LeadClickSource!=500 or LeadClickSource is null)		
		GROUP BY d.id,d.organization,CONVERT(DATE,a.CreatedOn)

		UPDATE tt  
		SET Masking_Leads= b.Masking_Leads
		FROM #temp tt
		INNER JOIN  (  
						SELECT DISTINCT d.id DealerId,
						       COUNT(DISTINCT tc.Mobile) 'Masking_Leads'		
						FROM Dealers as d WITH(NOLOCK) 
						LEFT JOIN TC_CustomerDetails tc WITH(NOLOCK)  ON d.ID = TC.BranchId		
						LEFT JOIN TC_Lead as f  WITH(NOLOCK)  on TC.Id = F.TC_CustomerId		
						LEFT JOIN TC_InquiriesLead c  WITH(NOLOCK)  ON C.TC_LeadId = f.TC_LeadId		
						LEFT JOIN TC_NewCarInquiries a  WITH(NOLOCK)  ON a.TC_InquiriesLeadId = c.TC_InquiriesLeadId		
						LEFT JOIN TC_InquirySource b WITH(NOLOCK)  ON a.TC_InquirySourceId = b.ID		
						LEFT JOIN PQDealerAdLeads l WITH(NOLOCK) ON l.PushStatus=a.TC_NewCarInquiriesId	
						LEFT JOIN vwmmv v WITH(NOLOCK)  ON v.VersionId=a.VersionId	
						LEFT JOIN Cities ci WITH(NOLOCK)  ON ci.id=d.cityid	
						LEFT JOIN LA_Agencies ag WITH(NOLOCK)  ON ag.id=l.PlatformId	
						LEFT JOIN PqLeadClickSource_Master cs WITH(NOLOCK)  on cs.LeadClickSource=l.LeadClickSource	
						WHERE CONVERT(DATE,a.CreatedOn) =@previousDay 
						and f.BranchId not in ('9350','9735','15418','15756','16345','16403','16183','10216','535','4815','20452')		
						and b.Product_Newcar = 1 --and (LeadClickSource!=500 or LeadClickSource is null)		
						and b.id=6	-- CarWale Advertisement
						GROUP BY d.id,d.organization 
					) B ON B.dealerId= tt.dealerid

		

		SELECT DealerId , DealerName , Total_Leads ,Masking_Leads ,ROUND((CAST (Masking_Leads AS FLOAT)/Total_Leads)*100,2) [% Masking Leads] 
		FROM #temp
		WHERE  ((CAST (Masking_Leads AS FLOAT)/Total_Leads)*100 )<=10
		ORDER BY [% Masking Leads] ASC , Total_Leads DESC

		DROP TABLE #temp

--END 
END
