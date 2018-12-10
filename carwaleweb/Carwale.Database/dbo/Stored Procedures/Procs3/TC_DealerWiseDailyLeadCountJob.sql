IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_DealerWiseDailyLeadCountJob]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_DealerWiseDailyLeadCountJob]
GO

	
-- =============================================
-- Author:		Manish
-- Create date: 19-06-2013
-- Description: Report for VW AutoBiz panel Usage 
-- =============================================
CREATE PROCEDURE  [dbo].[TC_DealerWiseDailyLeadCountJob]
AS 
BEGIN 
DECLARE @TmpTable TABLE (DealerId	int,
                         TotalLeadCount	int,
                         LeadCreationDate	date)


INSERT INTO @TmpTable   (DealerId	,
                         TotalLeadCount	,
                         LeadCreationDate)
SELECT D.ID AS DealerId,
       COUNT(TCIL.TC_InquiriesLeadId) AS TotalLeadCount,
       CONVERT(DATE,GETDATE()) AS LeadCreationDate
FROM DEALERS as D WITH (NOLOCK)
INNER JOIN TC_BrandZone TBZ WITH (NOLOCK) ON D.TC_BrandZoneId = TBZ.TC_BrandZoneId AND TBZ.MakeId = 20 AND  D.IsDealerActive= 1
        LEFT JOIN TC_InquiriesLead AS TCIL WITH (NOLOCK) ON D.ID=TCIL.BranchId 
                                                             AND TCIL.TC_LeadInquiryTypeId=3
                                                             AND CONVERT(DATE,CreatedDate)=CONVERT(DATE,GETDATE())
GROUP BY D.ID,CONVERT(DATE,CreatedDate)


DELETE FROM TC_DealerWiseDailyLeadCount WHERE LeadCreationDate=CONVERT(DATE,GETDATE());


INSERT INTO TC_DealerWiseDailyLeadCount   (DealerId	,
										   TotalLeadCount,
							    		  LeadCreationDate)
							   SELECT DealerId,	
									 TotalLeadCount	,
									 LeadCreationDate
								FROM @TmpTable


END 