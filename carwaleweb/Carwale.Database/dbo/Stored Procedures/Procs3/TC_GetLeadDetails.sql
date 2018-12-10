IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_GetLeadDetails]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_GetLeadDetails]
GO
	
-- =============================================
-- Author:		Nilima More 
-- Create date: 18th Feb 2016.
-- Description:	To fetch details of all leads to be used in the Android app call module.
-- Modified By: Khushaboo Patil on 2/5/2016  added @leadInquiryType to fetch particular active leads 
-- EXEC [TC_GetLeadDetails] 5,3
-- =============================================
CREATE PROCEDURE [dbo].[TC_GetLeadDetails] 
	@BranchId INT,
	@LeadInquiryType TINYINT = NULL
AS
BEGIN
    SELECT DISTINCT C.Id,C.CustomerName , C.Email , C.Mobile, C.ActiveLeadId
	FROM TC_CustomerDetails  AS C WITH (NOLOCK) 
	INNER JOIN TC_Lead L WITH(NOLOCK) ON L.TC_CustomerId = C.Id
	INNER JOIN TC_InquiriesLead IL WITH(NOLOCK) ON IL.TC_LeadId = L.TC_LeadId
	WHERE C.BranchId = @BranchId
	AND C.IsleadActive = 1 
	AND C.ActiveLeadId IS NOT NULL
	AND L.TC_LeadStageId <> 3
	AND (@LeadInquiryType IS NULL OR IL.TC_LeadInquiryTypeId = @LeadInquiryType) -- Modified By: Khushaboo Patil on 2/5/2016  added @leadInquiryType to fetch particular active leads
	ORDER BY C.Id DESC 	 
END



