IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[UCDDealerMaskingNumberPerformance]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[UCDDealerMaskingNumberPerformance]
GO

	  
  -- =============================================
-- Author:			Kundan Dombale
-- Create date:	    26-04-2016
-- Description:		To report of UCD dealers with a lesser percentage of masking number leads
-- Exec             [dbo].[ups_UcdDealerMaskingNumberPerformance]
-- =============================================


   CREATE  PROCEDURE [dbo].[UCDDealerMaskingNumberPerformance]
   AS 
   BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.

	SET NOCOUNT ON;
	
		DECLARE @previousDay as date = CAST (getdate()-1 as date)

	-- To get count of all other leads excluding masking number leads 

		SELECT	D.Id as DealerId,
				D.Organization As DealerName,
				COUNT(UCP.ID) as OtherLeads
		
		INTO #temp1

		FROM Dealers as D  WITH (NOLOCK)
		LEFT JOIN SELLINQUIRIES SI WITH(NOLOCK) ON D.ID =SI.DealerId
		LEFT JOIN USEDCARPURCHASEINQUIRIES UCP  WITH (NOLOCK) ON UCP.SELLINQUIRYID = SI.ID		
		WHERE	CAST(UCP.REQUESTDATETIME  AS DATE)= @previousDay
				AND  D.TC_DEALERTYPEID in ( 1,3)
		GROUP BY	D.Id,
					D.Organization
  
		--To get count of Masking Number Leads 
		 
       	SELECT  D.Id as DealerId,
				D.Organization As DealerName,
				COUNT(MM.MM_INQUIRIESID) AS Masking_Leads
		
		INTO  #temp2

		FROM DEALERS D  WITH (NOLOCK)
		LEFT JOIN  MM_INQUIRIES MM  WITH (NOLOCK) ON D.ID = MM.CONSUMERID 
		WHERE	D.TC_DEALERTYPEID in ( 1,3)
				AND CAST(MM.CALLSTARTDATE AS DATE) = @previousDay
		GROUP BY D.Id,
				 D.Organization

		--SELECT  D.Id as DealerId,D.Organization As DealerName, COUNT(DISTINCT TI.TC_InquiriesLeadId) AS Masking_Leads
		--INTO  #temp2
		--FROM TC_BuyerInquiries TBI
		--INNER JOIN TC_InquiriesLead TI
		--	ON TBI.TC_InquiriesLeadId = TI.TC_InquiriesLeadId
		--INNER JOIN TC_Lead TL
		--	ON TL.TC_LeadId = TI.TC_LeadId
		--INNER JOIN TC_CustomerDetails TD
		--	ON TD.Id = TL.TC_CustomerId
		--WHERE	D.TC_DEALERTYPEID in ( 1,3)
		--		AND CAST(MM.CALLSTARTDATE AS DATE) = @previousDay
		--GROUP BY D.Id,
		--		 D.Organization

	    


		SELECT  T1.DealerId,T1.DealerName,
		        (T1.OtherLeads+t2.Masking_Leads) As TotalLeads,
				t2.Masking_Leads,
				ROUND((CAST (t2.Masking_Leads AS FLOAT)/(T1.OtherLeads+t2.Masking_Leads))*100,2) [% Masking Leads] 
		FROM #temp1 as T1 
		JOIN #temp2 AS T2 ON T1.DealerId=T2.DealerId
		WHERE  ((CAST (t2.Masking_Leads AS FLOAT)/ (T1.OtherLeads+t2.Masking_Leads))*100 )<=10
		ORDER BY [% Masking Leads] ASC , TotalLeads DESC
			
		
		DROP TABLE #temp2
		DROP TABLE #temp1

END 
 


 