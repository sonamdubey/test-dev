IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_FetchInqCountForReports]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_FetchInqCountForReports]
GO

	
-- =============================================
-- Created By: Vivek Gupta
-- Created Date:23-05-2013
-- Description: For display counts on Repors of TC based on inquiries for usedbuyer and used seller.
-- Modified By: Nilesh Utture on 30th May, 2013 Changed month format
-- Modified By: Afrose, changed source to groupsource on select, TIGS.GroupSourceName
-- Modified By : Manish on 18-01-2016 added WITH(NOLOCK) keyword wherever not found.
-- =============================================
CREATE PROCEDURE [dbo].[TC_FetchInqCountForReports]
(
@BranchId INT,
@FromDate DATETIME,
@ToDate DATETIME,
@TC_LeadInquiryTypeID TINYINT
)
AS
BEGIN	
	DECLARE @FollowUpPendingCount INT
	IF (@TC_LeadInquiryTypeID=1)
	BEGIN
		SELECT @FollowUpPendingCount = COUNT(DISTINCT(B.TC_BuyerInquiriesId)) FROM TC_Calls C  WITH (NOLOCK) INNER JOIN TC_InquiriesLead IL WITH (NOLOCK) 
			ON C.TC_LeadId = IL.TC_LeadId 
			INNER JOIN TC_BuyerInquiries B WITH (NOLOCK)  ON B.TC_InquiriesLeadId = IL.TC_InquiriesLeadId 
			WHERE 
			IL.TC_LeadInquiryTypeId = @TC_LeadInquiryTypeID
			AND IL.BranchId = @BranchId
			AND C.IsActionTaken = 0
			AND C.ScheduledOn BETWEEN @FromDate AND @ToDate
		SELECT 
			COUNT(DISTINCT TCBI.TC_BuyerInquiriesId) AS Total ,
			
			COUNT(DISTINCT(CASE WHEN ( (   TCBI.TC_LeadDispositionId<>1
			                           AND TCBI.TC_LeadDispositionId<>3
			                           AND TCBI.TC_LeadDispositionId<>4) AND TCBI.TC_LeadDispositionId IS NOT NULL)
			                           THEN TCBI.TC_BuyerInquiriesId END)) AS  Lost,			                           
			COUNT(DISTINCT(CASE WHEN (TCBI.TC_LeadDispositionId=4 or TCBI.TC_LeadDispositionId=34)  THEN TCBI.TC_BuyerInquiriesId END)) AS Converted,
			
			COUNT(DISTINCT(CASE WHEN     TCIL.TC_LeadStageId = 1 AND TCIL.TC_UserId IS NOT NULL AND TCBI.TC_LeadDispositionId IS NULL    THEN TCBI.TC_BuyerInquiriesId END)) AS VerificationStage,
			COUNT(DISTINCT(CASE WHEN     TCIL.TC_LeadStageId = 2 AND TCBI.TC_LeadDispositionId IS NULL  THEN TCBI.TC_BuyerInquiriesId END)) AS ConsultationStage,
			
			COUNT(DISTINCT(CASE WHEN ( TCBI.TC_LeadDispositionId IS NULL AND TCIL.TC_InquiryStatusId =1 AND TCIL.TC_UserId IS NOT NULL) THEN TCBI.TC_BuyerInquiriesId END)) AS Hot,
			COUNT(DISTINCT(CASE WHEN ( TCBI.TC_LeadDispositionId IS NULL AND TCIL.TC_InquiryStatusId =2 AND TCIL.TC_UserId IS NOT NULL) THEN TCBI.TC_BuyerInquiriesId END)) AS Warm,
			COUNT(DISTINCT(CASE WHEN ( TCBI.TC_LeadDispositionId IS NULL AND TCIL.TC_InquiryStatusId =3 AND TCIL.TC_UserId IS NOT NULL) THEN TCBI.TC_BuyerInquiriesId END)) AS Normal,	
			COUNT(DISTINCT(CASE WHEN ( TCBI.TC_LeadDispositionId IS NULL AND TCIL.TC_InquiryStatusId IS NULL AND TCIL.TC_UserId IS NOT NULL) THEN TCBI.TC_BuyerInquiriesId END)) AS NotSet,		
			
			@FollowUpPendingCount AS FollowupPending,
			COUNT(DISTINCT(CASE WHEN  ISNULL(TCL.TC_LeadStageId,0)=0  THEN TCBI.TC_BuyerInquiriesId END)) AS UnAssigned,
			
			COUNT(DISTINCT(CASE WHEN (TCBI.TC_LeadDispositionId = 1)  THEN TCBI.TC_BuyerInquiriesId END)) AS Fake,
			COUNT(DISTINCT(CASE WHEN (TCBI.TC_LeadDispositionId = 3)  THEN TCBI.TC_BuyerInquiriesId END)) AS NotInterested
			
			
		FROM TC_Lead  AS TCL WITH(NOLOCK)
		JOIN TC_InquiriesLead AS TCIL WITH(NOLOCK)  ON TCL.TC_LeadId=TCIL.TC_LeadId
		JOIN TC_BuyerInquiries AS TCBI WITH(NOLOCK) ON TCIL.TC_InquiriesLeadId=TCBI.TC_InquiriesLeadId
									   AND  TCL.BranchId = @BranchId
									   AND  TCIL.BranchId = @BranchId
									   AND  TCIL.TC_LeadInquiryTypeId=1
									   AND  TCBI.CreatedOn BETWEEN @FromDate AND @TODate
									   
		SELECT  TIGS.GroupSourceName,COUNT(DISTINCT TNI.TC_BuyerInquiriesId) AS Total
			FROM TC_InquiriesLead TIL WITH(NOLOCK)
			INNER JOIN TC_BuyerInquiries TNI WITH(NOLOCK) ON TNI.TC_InquiriesLeadId = TIL.TC_InquiriesLeadId
			INNER JOIN TC_InquirySource TIS WITH(NOLOCK) ON TIS.Id = TNI.TC_InquirySourceId
			INNER JOIN TC_InquiryGroupSource AS TIGS WITH(NOLOCK)ON TIS.TC_InquiryGroupSourceId=TIGS.TC_InquiryGroupSourceId 
			WHERE  TNI.CreatedOn BETWEEN @FromDate AND @ToDate
			AND TIL.TC_LeadInquiryTypeId = @TC_LeadInquiryTypeID and TIL.BranchId = @BranchId --LeadInquiryTypeId is 3 for New Car inquiries only 
			GROUP BY TIGS.TC_InquiryGroupSourceId, TIGS.GroupSourceName
			ORDER BY TIGS.TC_InquiryGroupSourceId, TIGS.GroupSourceName		

		-- Modified By: Nilesh Utture on 30th May, 2013 
		SELECT CONVERT(VARCHAR,DAY(B.CreatedOn)) + '-' + CONVERT(char(3),B.CreatedOn, 0) AS DAY ,COUNT(DISTINCT B.TC_BuyerInquiriesId) AS Total 
			FROM TC_BuyerInquiries B WITH(NOLOCK)
			INNER JOIN TC_InquiriesLead IL WITH (NOLOCK)   ON IL.TC_InquiriesLeadId = B.TC_InquiriesLeadId
			WHERE IL.BranchId = @BranchId AND B.CreatedOn BETWEEN @FromDate AND @ToDate 
			GROUP BY DAY(B.CreatedOn), CONVERT(char(3),B.CreatedOn, 0)
			ORDER BY CONVERT(char(3),B.CreatedOn, 0), DAY(B.CreatedOn)
	END
	ELSE IF (@TC_LeadInquiryTypeID=2)
	BEGIN
		SELECT @FollowUpPendingCount = COUNT(DISTINCT(S.TC_SellerInquiriesId)) FROM TC_Calls C WITH (NOLOCK)  
		   INNER JOIN TC_InquiriesLead IL WITH (NOLOCK) 
			ON C.TC_LeadId = IL.TC_LeadId 
			INNER JOIN TC_SellerInquiries S WITH (NOLOCK)  ON S.TC_InquiriesLeadId = IL.TC_InquiriesLeadId 
			WHERE 
			IL.TC_LeadInquiryTypeId = @TC_LeadInquiryTypeID
			AND IL.BranchId = @BranchId
			AND C.IsActionTaken = 0
			AND C.ScheduledOn BETWEEN @FromDate AND @ToDate
			
		SELECT 					
			COUNT(DISTINCT TCSI.TC_SellerInquiriesId) AS Total ,
			
			COUNT(DISTINCT(CASE WHEN (   TCSI.TC_LeadDispositionID<>1
			                          AND TCSI.TC_LeadDispositionID<>3
			                          AND TCSI.TC_LeadDispositionID<>4
			                          AND TCSI.TC_LeadDispositionID IS NOT NULL)  THEN TCSI.TC_SellerInquiriesId END)) AS  Lost,
			COUNT(DISTINCT(CASE WHEN (TCSI.TC_LeadDispositionId=4 or TCSI.TC_LeadDispositionId=34)  THEN TCSI.TC_SellerInquiriesId END)) AS Converted,		
				
			COUNT(DISTINCT(CASE WHEN     TCIL.TC_LeadStageId = 1 AND TCIL.TC_UserId IS NOT NULL  AND TCSI.TC_LeadDispositionId IS NULL   THEN TCSI.TC_SellerInquiriesId END)) AS VerificationStage,
			COUNT(DISTINCT(CASE WHEN     TCIL.TC_LeadStageId = 2 AND TCSI.TC_LeadDispositionId IS NULL  THEN TCSI.TC_SellerInquiriesId END)) AS ConsultationStage,
			
			COUNT(DISTINCT(CASE WHEN (TCSI.TC_LeadDispositionId IS NULL AND TCIL.TC_InquiryStatusId =1 AND TCIL.TC_UserId IS NOT NULL) THEN TCSI.TC_SellerInquiriesId END)) AS Hot,
			COUNT(DISTINCT(CASE WHEN (TCSI.TC_LeadDispositionId IS NULL AND TCIL.TC_InquiryStatusId =2 AND TCIL.TC_UserId IS NOT NULL) THEN TCSI.TC_SellerInquiriesId END)) AS Warm,
			COUNT(DISTINCT(CASE WHEN (TCSI.TC_LeadDispositionId IS NULL AND TCIL.TC_InquiryStatusId =3 AND TCIL.TC_UserId IS NOT NULL) THEN TCSI.TC_SellerInquiriesId END)) AS Normal,		
			COUNT(DISTINCT(CASE WHEN ( TCSI.TC_LeadDispositionId IS NULL AND TCIL.TC_InquiryStatusId IS NULL AND TCIL.TC_UserId IS NOT NULL) THEN TCSI.TC_SellerInquiriesId END)) AS NotSet,		
			
			@FollowUpPendingCount AS FollowupPending,
			COUNT(DISTINCT(CASE WHEN  ISNULL(TCL.TC_LeadStageId,0)=0  THEN TCSI.TC_SellerInquiriesId END)) AS UnAssigned,
			
			COUNT(DISTINCT(CASE WHEN (TCSI.TC_LeadDispositionId = 1)  THEN TCSI.TC_SellerInquiriesId END)) AS Fake,
			COUNT(DISTINCT(CASE WHEN (TCSI.TC_LeadDispositionId = 3)  THEN TCSI.TC_SellerInquiriesId END)) AS NotInterested
			
			
		FROM TC_Lead  AS TCL WITH(NOLOCK)
		JOIN TC_InquiriesLead AS TCIL WITH(NOLOCK)  ON TCL.TC_LeadId=TCIL.TC_LeadId
		JOIN TC_SellerInquiries AS TCSI WITH(NOLOCK) ON TCIL.TC_InquiriesLeadId=TCSI.TC_InquiriesLeadId
									   AND  TCL.BranchId= @BranchId
									   AND  TCIL.BranchId=@BranchId
									   AND  TCIL.TC_LeadInquiryTypeId=2
									   AND  TCSI.CreatedOn BETWEEN @FromDate AND @TODate
	
		SELECT  TIGS.GroupSourceName,COUNT(DISTINCT TNI.TC_SellerInquiriesId) AS Total
			FROM TC_InquiriesLead TIL WITH(NOLOCK)
			INNER JOIN TC_SellerInquiries TNI WITH(NOLOCK) ON TNI.TC_InquiriesLeadId = TIL.TC_InquiriesLeadId
			INNER JOIN TC_InquirySource TIS WITH(NOLOCK) ON TIS.Id = TNI.TC_InquirySourceId
			INNER JOIN TC_InquiryGroupSource AS TIGS WITH(NOLOCK)ON TIS.TC_InquiryGroupSourceId=TIGS.TC_InquiryGroupSourceId
			WHERE  TNI.CreatedOn BETWEEN @FromDate AND @ToDate
			AND TIL.TC_LeadInquiryTypeId = @TC_LeadInquiryTypeID and TIL.BranchId = @BranchId --LeadInquiryTypeId is 3 for New Car inquiries only 
			GROUP BY TIGS.TC_InquiryGroupSourceId, TIGS.GroupSourceName
			ORDER BY TIGS.TC_InquiryGroupSourceId, TIGS.GroupSourceName		
		
		-- Modified By: Nilesh Utture on 30th May, 2013 	
		SELECT CONVERT(VARCHAR,DAY(S.CreatedOn)) + '-' + CONVERT(char(3),S.CreatedOn, 0) AS DAY ,COUNT(DISTINCT S.TC_SellerInquiriesId) AS Total 
			FROM TC_SellerInquiries S WITH(NOLOCK)
			INNER JOIN TC_InquiriesLead IL WITH (NOLOCK)   ON IL.TC_InquiriesLeadId = S.TC_InquiriesLeadId
			WHERE IL.BranchId = @BranchId AND S.CreatedOn BETWEEN @FromDate AND @ToDate
			GROUP BY DAY(S.CreatedOn), CONVERT(char(3),S.CreatedOn, 0)
			ORDER BY CONVERT(char(3),S.CreatedOn, 0), DAY(S.CreatedOn)
	END						   
END
