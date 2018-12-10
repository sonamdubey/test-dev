IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_GetLeadDispositions]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_GetLeadDispositions]
GO

	-- =============================================
-- Author:		Vivek Gupta
-- Create date: 26th April,2013
-- Description:	Fetch Lead Dispositions
-- Modified BY: Tejashree Patil on 11 July 2013, Added parameter @InquiryType to get dispositions to close inquiry.
-- Modified By: Nilesh Utture on 12th September, 2013 Added SELECT statement to fetch Lead Sub dispositions
-- =============================================
CREATE PROCEDURE [dbo].[TC_GetLeadDispositions]
	@InquiryType SMALLINT = NULL
AS
BEGIN

SELECT 
       TL.Name,
       TL.TC_LeadDispositionId,
       TL.TC_LeadInquiryTypeId 
FROM   
       TC_LeadDisposition TL WITH(NOLOCK) 
WHERE 
			   IsActive=1
		 AND   IsClosed=1
		 AND   TC_LeadInquiryTypeId IS NOT NULL     
		 AND   TC_LeadDispositionId NOT IN (32,42,77) 
		 AND   ((@InquiryType IS NULL) OR (TC_LeadInquiryTypeId = @InquiryType)) -- Modified BY : Tejashree Patil on 11 July 2013
		 
		 
SELECT 
       TSD.TC_LeadSubDispositionId,
       TSD.SubDispositionName AS Name,
       TSD.TC_LeadDispositionId
FROM   
       TC_LeadSubDisposition TSD WITH(NOLOCK) 
WHERE 
	   IsActive=1

END
