IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_INQReportSourceWise]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_INQReportSourceWise]
GO

	
-- Author : Suri On 31 Jan 2013
-- Description: Used for source wise count in dashboard
--Modified by Afrose, Selecting group source instead of source, added param @ApplicationId
--Modified by Manish on 10-12-2015 added with(nolock) keyword wherever not found and changed datatype from Bigint to int.
-- =============================================
CREATE PROCEDURE [dbo].[TC_INQReportSourceWise]
(
	@BranchId INT,
	@UserId INT,
	@ApplicationId SMALLINT
)
as
BEGIN
IF @ApplicationId=1
	-- Inquiry Source details
	WITH CTE AS
	(	
	    SELECT TIGS.GroupSourceName AS Source,TIGS.TC_InquiryGroupSourceId AS TC_InquirySourceId,COUNT(*) AS [COUNT]
		FROM	TC_BuyerInquiries AS TCBI WITH(NOLOCK)
		INNER JOIN TC_InquiriesLead AS TCIL WITH(NOLOCK) ON TCBI.TC_InquiriesLeadId=TCIL.TC_InquiriesLeadId
		INNER JOIN TC_InquirySource AS TCIS WITH(NOLOCK) ON TCIS.Id=TCBI.TC_InquirySourceId
		INNER JOIN TC_InquiryGroupSource AS TIGS WITH(NOLOCK) ON TCIS.TC_InquiryGroupSourceId=TIGS.TC_InquiryGroupSourceId
		WHERE	TCIL.BranchId=@BranchId AND TCIL.TC_LeadStageID <>3 AND TCBI.TC_LeadDispositionID IS NULL
		AND IsVisible=1 And IsVisibleCW=@ApplicationId
		GROUP BY TIGS.TC_InquiryGroupSourceId, TIGS.GroupSourceName
		UNION ALL
		SELECT TIGS.GroupSourceName AS Source,TIGS.TC_InquiryGroupSourceId AS TC_InquirySourceId,COUNT(*) AS [COUNT]
		FROM	TC_SellerInquiries AS TCSI WITH(NOLOCK)
		INNER JOIN TC_InquiriesLead AS TCIL WITH(NOLOCK) ON TCSI.TC_InquiriesLeadId=TCIL.TC_InquiriesLeadId
		INNER JOIN TC_InquirySource AS TCIS WITH(NOLOCK) ON TCIS.Id=TCSI.TC_InquirySourceId
		INNER JOIN TC_InquiryGroupSource AS TIGS WITH(NOLOCK) ON TCIS.TC_InquiryGroupSourceId=TIGS.TC_InquiryGroupSourceId
		WHERE TCIL.BranchId=@BranchId  AND TCIL.TC_LeadStageID <>3 AND TCSI.TC_LeadDispositionID IS NULL
		AND IsVisible=1 And IsVisibleCW=@ApplicationId
		GROUP BY TIGS.TC_InquiryGroupSourceId, TIGS.GroupSourceName
		UNION ALL
		SELECT  TIGS.GroupSourceName AS Source,TIGS.TC_InquiryGroupSourceId AS TC_InquirySourceId,COUNT(*) AS [COUNT]
		FROM	TC_NewCarInquiries AS TCNI WITH(NOLOCK)
		INNER JOIN TC_InquiriesLead AS TCIL WITH(NOLOCK) ON TCNI.TC_InquiriesLeadId=TCIL.TC_InquiriesLeadId
		INNER JOIN TC_InquirySource AS TCIS WITH(NOLOCK) ON TCIS.Id=TCNI.TC_InquirySourceId
		INNER JOIN TC_InquiryGroupSource AS TIGS WITH(NOLOCK) ON TCIS.TC_InquiryGroupSourceId=TIGS.TC_InquiryGroupSourceId
		WHERE	TCIL.BranchId=@BranchId  AND TCIL.TC_LeadStageID <>3 AND TCNI.TC_LeadDispositionID IS NULL
		AND IsVisible=1 And IsVisibleCW=@ApplicationId
		GROUP BY TIGS.TC_InquiryGroupSourceId, TIGS.GroupSourceName
	)	SELECT   SOURCE AS SourceName,SUM([COUNT]) AS SourceCount FROM CTE WITH (NOLOCK) GROUP BY Source		

ELSE IF @ApplicationId=2
WITH CTE AS
	(	
	    SELECT TIGS.GroupSourceName AS Source,TIGS.TC_InquiryGroupSourceId AS TC_InquirySourceId,COUNT(*) AS [COUNT]
		FROM	TC_BuyerInquiries AS TCBI WITH(NOLOCK)
		INNER JOIN TC_InquiriesLead AS TCIL WITH(NOLOCK) ON TCBI.TC_InquiriesLeadId=TCIL.TC_InquiriesLeadId
		INNER JOIN TC_InquirySource AS TCIS WITH(NOLOCK) ON TCIS.Id=TCBI.TC_InquirySourceId
		INNER JOIN TC_InquiryGroupSource AS TIGS WITH(NOLOCK) ON TCIS.TC_InquiryGroupSourceId=TIGS.TC_InquiryGroupSourceId
		WHERE	TCIL.BranchId=@BranchId AND TCIL.TC_LeadStageID <>3 AND TCBI.TC_LeadDispositionID IS NULL
		AND IsVisible=1 And IsVisibleBW=@ApplicationId
		GROUP BY TIGS.TC_InquiryGroupSourceId, TIGS.GroupSourceName
		UNION ALL
		SELECT TIGS.GroupSourceName AS Source,TIGS.TC_InquiryGroupSourceId AS TC_InquirySourceId,COUNT(*) AS [COUNT]
		FROM	TC_SellerInquiries AS TCSI WITH(NOLOCK)
		INNER JOIN TC_InquiriesLead AS TCIL WITH(NOLOCK) ON TCSI.TC_InquiriesLeadId=TCIL.TC_InquiriesLeadId
		INNER JOIN TC_InquirySource AS TCIS WITH(NOLOCK) ON TCIS.Id=TCSI.TC_InquirySourceId
		INNER JOIN TC_InquiryGroupSource AS TIGS WITH(NOLOCK) ON TCIS.TC_InquiryGroupSourceId=TIGS.TC_InquiryGroupSourceId
		WHERE TCIL.BranchId=@BranchId  AND TCIL.TC_LeadStageID <>3 AND TCSI.TC_LeadDispositionID IS NULL
		AND IsVisible=1 And IsVisibleBW=@ApplicationId
		GROUP BY TIGS.TC_InquiryGroupSourceId, TIGS.GroupSourceName
		UNION ALL
		SELECT  TIGS.GroupSourceName AS Source,TIGS.TC_InquiryGroupSourceId AS TC_InquirySourceId,COUNT(*) AS [COUNT]
		FROM	TC_NewCarInquiries AS TCNI WITH(NOLOCK)
		INNER JOIN TC_InquiriesLead AS TCIL WITH(NOLOCK) ON TCNI.TC_InquiriesLeadId=TCIL.TC_InquiriesLeadId
		INNER JOIN TC_InquirySource AS TCIS WITH(NOLOCK) ON TCIS.Id=TCNI.TC_InquirySourceId
		INNER JOIN TC_InquiryGroupSource AS TIGS WITH(NOLOCK) ON TCIS.TC_InquiryGroupSourceId=TIGS.TC_InquiryGroupSourceId
		WHERE	TCIL.BranchId=@BranchId  AND TCIL.TC_LeadStageID <>3 AND TCNI.TC_LeadDispositionID IS NULL
		AND IsVisible=1 And IsVisibleBW=@ApplicationId
		GROUP BY TIGS.TC_InquiryGroupSourceId, TIGS.GroupSourceName
	)	SELECT   SOURCE AS SourceName,SUM([COUNT]) AS SourceCount FROM CTE WITH (NOLOCK) GROUP BY Source		

END
