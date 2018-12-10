IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[usp_UcdDealerMaskingNumberPerformance]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[usp_UcdDealerMaskingNumberPerformance]
GO

	 
 
 -- =============================================
-- Author:		Kundan
-- Create date: 09-05-2016
-- Description:	To report UCD dealers with a lesser percentage of masking number leads
-- -- =============================================

CREATE Procedure usp_UcdDealerMaskingNumberPerformance 
(	@StartDate DateTime,
    @EndDate DateTime
)
AS 

BEGIN 

		SET NOCOUNT ON; 
		SET @StartDate = CONVERT(DATETIME,CONVERT(varchar(10),@StartDate,120)+ ' 00:00:00')	
		SET @EndDate = CONVERT(DATETIME,CONVERT(varchar(10),@EndDate,120)+ ' 23:59:59');
 
		SET NOCOUNT ON;
		-- Total Leads
		SELECT   D.id AS DealerId
				,D.Organization AS DealerName
				,COUNT(DISTINCT TI.TC_InquiriesLeadId) As 'Total_Leads'
				,0 AS  'Masking_Leads'
		INTO #temp
		FROM TC_BuyerInquiries TBI WITH(NOLOCK)
		INNER JOIN TC_InquiriesLead TI WITH(NOLOCK) ON TBI.TC_InquiriesLeadId = TI.TC_InquiriesLeadId
		INNER JOIN TC_Lead TL WITH(NOLOCK) ON TL.TC_LeadId = TI.TC_LeadId
		INNER JOIN TC_CustomerDetails TD WITH(NOLOCK) ON TD.Id = TL.TC_CustomerId
		INNER JOIN Dealers AS D WITH(NOLOCK) ON D.ID=TI.BranchId  AND  D.TC_DealerTypeId IN (1,3)
		WHERE --TD.BranchId = 3888 AND 
			 TI.CreatedDate BETWEEN  @StartDate AND  @EndDate 
		GROUP BY  D.id  
				  ,D.Organization  

		-- Masking Leads
		UPDATE tt  
				SET Masking_Leads= b.Masking_Leads
				FROM #temp tt
				INNER JOIN  (
								SELECT  D.id AS DealerId
									   ,D.Organization AS DealerName
								       ,COUNT(DISTINCT TI.TC_InquiriesLeadId) As 'Masking_Leads'
								FROM TC_BuyerInquiries TBI WITH(NOLOCK)
								INNER JOIN TC_InquiriesLead TI  WITH(NOLOCK) ON TBI.TC_InquiriesLeadId = TI.TC_InquiriesLeadId
								INNER JOIN TC_Lead TL  WITH(NOLOCK) ON TL.TC_LeadId = TI.TC_LeadId
								INNER JOIN TC_CustomerDetails TD  WITH(NOLOCK) ON TD.Id = TL.TC_CustomerId
								INNER JOIN Dealers AS D  WITH(NOLOCK) ON D.Id= Ti.BranchId AND D.TC_DealerTypeId IN (1,3)
								WHERE  --TD.BranchId = 3888 AND 
									 TI.CreatedDate BETWEEN  @StartDate AND  @EndDate 
									AND CustomerName='unknown'
								GROUP BY D.id  
									   ,D.Organization  
								) AS B ON B.DealerId =TT.DealerId

						

								
		SELECT DealerId , DealerName , Total_Leads ,Masking_Leads ,ROUND((CAST (Masking_Leads AS FLOAT)/Total_Leads)*100,2) [% Masking Leads] 
		FROM #temp  WITH(NOLOCK)
		WHERE  ((CAST (Masking_Leads AS FLOAT)/Total_Leads)*100 )<=10
		ORDER BY [% Masking Leads] ASC , Total_Leads DESC

		DROP TABLE #temp;
								 
END ;