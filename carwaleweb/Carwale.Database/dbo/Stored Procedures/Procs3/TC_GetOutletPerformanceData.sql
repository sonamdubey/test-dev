IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_GetOutletPerformanceData]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_GetOutletPerformanceData]
GO
	-- =============================================
-- Author		: Nilima More,	
-- Create date  : 23 Feb 2016
-- Description  : To fetch performance data for group users.
-- DECLARE @FROM DATETIME = GETDATE()-24,@TO DATETIME = GETDATE()
-- EXEC [TC_GetOutletPerformanceData] 4799,@FROM,@TO
-- Modified BY : Khushaboo Patil on 21/4/2016 commented ConvertedLeads calculation
-- =============================================
CREATE PROCEDURE [dbo].[TC_GetOutletPerformanceData] 
	@MasterDealerId INT ,
	@FromDate DATETIME ,
	@ToDate DATETIME
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	DECLARE @NoOfLeads INT = NULL,@LostLeads INT = NULL,@BookedLeads INT = NULL,@TDCount INT = NULL
	SET @ToDate = DATEADD(ms, -2, DATEADD(dd, 1, DATEDIFF(dd, 0,@ToDate))) 
			
			SELECT TAB.DealerId,TAB.NoOfLeads,LostLeads+TAB.BookedLeads ClosedLeads,TAB.BookedLeads,TAB.TDCount,
			--CASE WHEN (TAB.LostLeads + TAB.BookedLeads) = 0 
			--THEN 0 ELSE
			-- ROUND(CAST( CAST(TAB.BookedLeads AS FLOAT) / (TAB.LostLeads + TAB.BookedLeads) AS FLOAT) * 100,2) END ConvertedLeads,
			0 AS ConvertedLeads,
			TAB.OutletName,TAB.LostLeads FROM
			(
			SELECT  DAM.DealerId, 
			COUNT(DISTINCT(CASE WHEN TCL.LeadCreationDate BETWEEN @FromDate AND @ToDate THEN TCL.TC_LeadId END)) NoOfLeads,
			COUNT(DISTINCT(CASE WHEN TCIL.TC_LeadStageId = 3 AND
			((ISNULL(TCIL.TC_LeadDispositionID,0) NOT IN(1,3,4,70,71,74)) OR (ISNULL(TCIL.TC_LeadDispositionID,0) = 41 AND ISNULL(TCNI.BookingStatus,0) <> 32)) AND TCL.LeadClosedDate BETWEEN  @FromDate AND @ToDate  THEN TCIL.TC_LeadId END)) LostLeads,
			COUNT(DISTINCT(CASE WHEN ISNULL(TCNI.TC_LeadDispositionId,0) IN(4,41) AND ISNULL(TCNI.BookingStatus,0) = 32 AND TCNI.BookingDate  BETWEEN  @FromDate AND @ToDate THEN TCNI.TC_NewCarInquiriesId END)) BookedLeads ,
			cOUNT(DISTINCT(CASE WHEN ISNULL(TCNI.TDStatus ,0) =28 AND TCNI.TDDate  BETWEEN  @FromDate AND @ToDate THEN TCNI.TC_NewCarInquiriesId  END)) TDCount,
			D.Organization OutletName
			
			FROM TC_DealerAdmin DA WITH(NOLOCK)
			JOIN TC_DealerAdminMapping DAM WITH(NOLOCK) ON DA.Id = DAM.DealerAdminId
			LEFT JOIN TC_InquiriesLead TCIL WITH(NOLOCK) ON TCIL.BranchId = DAM.DealerId 
			LEFT JOIN TC_NewCarInquiries TCNI WITH(NOLOCK) ON TCNI.TC_InquiriesLeadId = TCIL.TC_InquiriesLeadId
			LEFT JOIN TC_LEAD TCL WITH(NOLOCK) ON TCL.TC_LeadId = TCIL.TC_LeadId
			LEFT JOIN Dealers D WITH(NOLOCK) ON D.ID = DAM.DealerId
			WHERE
			 DA.DealerId = @MasterDealerId AND DA.IsActive = 1 AND D.IsDealerActive = 1 AND D.IsDealerDeleted = 0 AND DAM.DEALERID  <> @MasterDealerId --AND D.ApplicationId = 1
			GROUP BY DAM.DealerId,D.Organization
			 )TAB
			

END
