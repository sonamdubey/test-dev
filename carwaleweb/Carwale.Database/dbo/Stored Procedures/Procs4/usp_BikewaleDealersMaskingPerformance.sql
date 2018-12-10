IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[usp_BikewaleDealersMaskingPerformance]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[usp_BikewaleDealersMaskingPerformance]
GO

	 -- =============================================
-- Author:		Kundan
-- Create date: 09-05-2016
-- Description:	To report BikeWale dealers  with a lesser percentage of masking number leads
-- Modifications: Chirag Perla:on 13-07-2016 Changed query to bring correct numbers for leads. Changed Date field from TC_NewCarInquiries(CreatedOn) to TC_InquiriesLead(CreatedDate) which is used in Date Range, and added Distinct while taking count of the leads
-- 
 -- =============================================
CREATE PROCEDURE  [dbo].[usp_BikewaleDealersMaskingPerformance]
(  
		@StartDate DATETIME,
		@EndDate DATETIME
)
AS 
BEGIN 
	
	SET NOCOUNT ON ;

	 
	SET @StartDate = CONVERT(DATETIME,CONVERT(varchar(10),@StartDate,120)+ ' 00:00:00')	
	SET @EndDate = CONVERT(DATETIME,CONVERT(varchar(10),@EndDate,120)+ ' 23:59:59');

	SELECT DISTINCT d.id DealerId,		
					d.Organization as DealerName,
					count(Distinct c.TC_InquiriesLeadId) as 'Total_Leads',
					0 as 'Masking_Leads'
				INTO #temp
				FROM dealers AS D with (nolock)
				JOIN TC_CustomerDetails tc with (nolock) ON D.ID = TC.BranchId and D.ApplicationId=2	
				JOIN TC_Lead AS F with (nolock)  ON TC.Id = F.TC_CustomerId		
				JOIN TC_InquiriesLead c with (nolock)  ON C.TC_LeadId = f.TC_LeadId		
				JOIN TC_NewCarInquiries a with (nolock)  ON a.TC_InquiriesLeadId = c.TC_InquiriesLeadId
				WHERE c.CreatedDate  between  @StartDate and @EndDate
				group by d.id,d.Organization 


	update tt
	set tt.Masking_Leads=z.Masking_Leads
	from #temp tt
	inner join 
	( SELECT DISTINCT d.id DealerId,
								count(Distinct c.TC_InquiriesLeadId) as 'Masking_Leads' 
		
				FROM dealers AS D with (nolock) 
				JOIN TC_CustomerDetails tc with (nolock)  ON D.ID = TC.BranchId and D.ApplicationId=2	
				JOIN TC_Lead AS F with (nolock)  ON TC.Id = F.TC_CustomerId		
				JOIN TC_InquiriesLead c with (nolock)  ON C.TC_LeadId = f.TC_LeadId		
				JOIN TC_NewCarInquiries a with (nolock)  ON a.TC_InquiriesLeadId = c.TC_InquiriesLeadId		
				JOIN TC_InquirySource b with (nolock)  ON c.InqSourceId = b.ID		
				WHERE c.CreatedDate  between  @StartDate and @EndDate
				and b.id=6 
				group by d.id
	)z on tt.DealerId=z.DealerId


				SELECT DealerId , DealerName , Total_Leads ,Masking_Leads ,ROUND((CAST (Masking_Leads AS FLOAT)/Total_Leads)*100,2) [% Masking Leads] 
				FROM #temp
				WHERE  cast(((CAST (Masking_Leads AS FLOAT)/Total_Leads)*100) as decimal(18,2))<=10
				ORDER BY [% Masking Leads] ASC , Total_Leads DESC

				DROP TABLE #temp

END 
 
