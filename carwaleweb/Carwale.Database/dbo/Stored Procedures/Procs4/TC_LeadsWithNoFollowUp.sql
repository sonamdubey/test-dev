IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_LeadsWithNoFollowUp]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_LeadsWithNoFollowUp]
GO

	
--Author: Afrose
--Created Date: 31/12/2015
--Description: Returns count of those leads on whom no followup has been done in the selected date range
-- EXEC TC_LeadsWithNoFollowUp 5,'2015/10/10','2016/01/04',2
-- =============================================     
CREATE PROCEDURE [dbo].[TC_LeadsWithNoFollowUp]
	-- Add the parameters for the stored procedure here     
	@BranchId INT,
	@FromDate DATE,
	@ToDate DATE,
	@LeadInquiryTypeID INT	
	
AS
SET NOCOUNT ON 
BEGIN
 SELECT COUNT(DISTINCT TCL.TC_LeadId) AS LeadsWithNoFollowUp 
 FROM TC_ActiveCalls TCAC WITH(NOLOCK) 
 INNER JOIN TC_lead TCL WITH(NOLOCK) ON TCAC.TC_LeadId=TCL.TC_LeadId
 INNER JOIN TC_InquiriesLead  TCIL WITH(NOLOCK) ON TCIL.TC_LeadId = TCL.TC_LeadId AND TCIL.TC_LeadInquiryTypeId = @LeadInquiryTypeID
 WHERE ( CONVERT(DATE,TCAC.LastCallDate) < @FromDate OR CONVERT(DATE,TCAC.LastCallDate) > @ToDate) AND TCL.BranchId=@BranchId 
END