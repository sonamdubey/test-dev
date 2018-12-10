IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_INQReportOnDashboard]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_INQReportOnDashboard]
GO

	-- Author : Tejashree Patil On 31 Jan 2013: Inquiry report on Dashboard
-- Description:[TC_INQReportOnDashboard] 5,1
-- =============================================
CREATE PROCEDURE [dbo].[TC_INQReportOnDashboard]
(
	@BranchId BIGINT,
	@TC_UserId BIGINT
)
as
BEGIN
	-- Inquiry Source details
	WITH CTE AS
	(	SELECT  Source,TC_InquirySourceId,COUNT(*) AS [COUNT]
		FROM	TC_BuyerInquiries AS TCBI
		INNER JOIN TC_InquiriesLead AS TCIL WITH(NOLOCK)ON TCBI.TC_InquiriesLeadId=TCIL.TC_InquiriesLeadId
		INNER JOIN TC_InquirySource AS TCIS WITH(NOLOCK)ON TCIS.Id=TCBI.TC_InquirySourceId
		WHERE	TCIL.TC_UserId=@TC_UserId AND TCIL.TC_LeadStageID <>3 AND TCBI.TC_LeadDispositionID IS NULL
		GROUP BY TC_InquirySourceId, Source
		UNION ALL
		SELECT  Source,TC_InquirySourceId,COUNT(*) AS [COUNT]
		FROM	TC_SellerInquiries AS TCSI
		INNER JOIN TC_InquiriesLead AS TCIL WITH(NOLOCK)ON TCSI.TC_InquiriesLeadId=TCIL.TC_InquiriesLeadId
		INNER JOIN TC_InquirySource AS TCIS WITH(NOLOCK)ON TCIS.Id=TCSI.TC_InquirySourceId
		WHERE TCIL.TC_UserId=@TC_UserId  AND TCIL.TC_LeadStageID <>3 AND TCSI.TC_LeadDispositionID IS NULL
		GROUP BY TC_InquirySourceId, Source
		UNION ALL
		SELECT  Source,TC_InquirySourceId,COUNT(*) AS [COUNT]
		FROM	TC_NewCarInquiries AS TCNI
		INNER JOIN TC_InquiriesLead AS TCIL WITH(NOLOCK)ON TCNI.TC_InquiriesLeadId=TCIL.TC_InquiriesLeadId
		INNER JOIN TC_InquirySource AS TCIS WITH(NOLOCK)ON TCIS.Id=TCNI.TC_InquirySourceId
		WHERE	TCIL.TC_UserId=@TC_UserId AND TCIL.TC_LeadStageID <>3 AND TCNI.TC_LeadDispositionID IS NULL
		GROUP BY TC_InquirySourceId, Source
	)	SELECT   SOURCE AS SourceName,SUM([COUNT]) AS SourceCount FROM CTE GROUP BY Source
			
	
	--missedFollowupCount
	SELECT ISNULL(COUNT(TC_CallsId), 0) AS missedFollowupCount
	FROM   TC_ActiveCalls AC WITH(NOLOCK)
	WHERE  TC_UsersId = @TC_UserId 
		   AND  CONVERT(DATE, AC.ScheduledOn) < CONVERT(DATE, GETDATE())
	
	--pendingFollowupCount
	SELECT ISNULL(COUNT(TC_CallsId), 0) AS pendingFollowupCount
	FROM   TC_ActiveCalls AC WITH(NOLOCK)	
	WHERE  TC_UsersId = @TC_UserId 
		   --AND  CONVERT(DATE, AC.ScheduledOn) >= CONVERT(DATE, GETDATE())
		   AND AC.CallType=1	
END
