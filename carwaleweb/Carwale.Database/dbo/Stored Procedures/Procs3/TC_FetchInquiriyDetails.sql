IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_FetchInquiriyDetails]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_FetchInquiriyDetails]
GO
	
-- =============================================
-- Created By: Manish Chourasiya
-- Created Date:13-03-2013
--- Description: For display counts on Dashboard of TC. The whole sp has been changed due to change in login from lead to inquiry based
-- Modified By: Ashwini Dhamankar on Oct 7,2014 //Added a filter on InquiryTypeId to avoid duplicacy of data.
---Modified by vivek rajak on 16/07/2015 to add excel inquiries count.
-- Modified by :Ashwini Dhamankar on Dec 15,2015 (added constraint of MostInterested=1)
-- exec [TC_FetchInquiriyDetails] 5,'2015-10-01 12:00:00 AM','2015-12-15 11:59:59 PM',3
-- =============================================
CREATE PROCEDURE [dbo].[TC_FetchInquiriyDetails] 
(
@BranchId INT,
@FromDate DATETIME,
@ToDate DATETIME,
@TC_LeadInquiryTypeID TINYINT =NULL
)
AS
BEGIN
	DECLARE @OtherInqCount1 INT
	DECLARE @ServiceInqCount2 INT

	SELECT @otherInqCount1=COUNT(*) 
	FROM TC_OtherRequests O WITH(NOLOCK) INNER JOIN TC_CustomerDetails C WITH(NOLOCK) ON O.TC_CustomerId=C.Id
	WHERE C.BranchId=@BranchId AND O.TC_InquiryTypeId <> 5 AND O.CreatedDate BETWEEN @FromDate AND @TODate  --Added a filter on InquiryTypeId to avoid duplicacy of data //Ashwini Dhamankar

	SELECT @ServiceInqCount2=COUNT(*) 
	FROM TC_ServiceRequests S WITH(NOLOCK) INNER JOIN TC_CustomerDetails C WITH(NOLOCK) ON S.TC_CustomerId=C.Id
	WHERE C.BranchId=@BranchId AND S.CreatedDate BETWEEN @FromDate AND @TODate

	DECLARE @ExcelLeadCount INT


IF (@TC_LeadInquiryTypeID=1)
BEGIN
		SET @ExcelLeadCount =
						(SELECT COUNT(TC_ImportBuyerInquiriesId)
						FROM TC_ImportBuyerInquiries B WITH(NOLOCK)
						WHERE B.BranchId = @BranchId 
						AND B.IsDeleted=0 
						--AND B.UserId=@UserId 
						AND B.TC_BuyerInquiriesId IS NULL
						AND 
						(@FromDate IS NULL OR EntryDate > @FromDate) 
						AND 
						(@ToDate IS NULL OR EntryDate < @ToDate)
						)


	SELECT 
		COUNT(DISTINCT (CASE WHEN  ISNULL(TCBI.MostInterested,0)=1  THEN TCBI.TC_BuyerInquiriesId END)) AS TotalLeads ,
		COUNT(DISTINCT(CASE WHEN  ISNULL(TCL.TC_LeadStageId,0)=0 AND ISNULL(TCBI.MostInterested,0)=1  THEN TCBI.TC_BuyerInquiriesId END)) AS UnAssigned,   --modified
		COUNT(DISTINCT(CASE WHEN ((TCL.TC_LeadDispositionId=3 OR TCL.TC_LeadDispositionId=1 OR TCBI.TC_LeadDispositionId IS NOT  NULL)
			   AND ((TCBI.TC_LeadDispositionId<>4 AND TCBI.TC_LeadDispositionId<>34) OR TCBI.TC_LeadDispositionId IS NULL) AND ISNULL(TCBI.MostInterested,0)=1)  THEN TCBI.TC_BuyerInquiriesId END)) AS  Closed,  --modified
		--COUNT(DISTINCT(CASE WHEN     TCL.TC_LeadDispositionId= 3  AND TCL.TC_LeadStageId = 3                THEN TCL.TC_LeadId END)) AS NILeads,
		COUNT(DISTINCT(CASE WHEN     TCL.TC_LeadDispositionId= 41  AND  ISNULL(TCBI.MostInterested,0)=1 THEN TCBI.TC_BuyerInquiriesId END)) AS Archived,
		--COUNT(DISTINCT(CASE WHEN (( TCL.TC_LeadStageId = 3) OR  ((TCBI.TC_LeadDispositionId IS NOT  NULL  and  TCBI.TC_LeadDispositionId<>42)  AND TCL.TC_LeadStageId <>3 ) )  THEN TCBI.TC_BuyerInquiriesId END)) AS Closed,
		COUNT(DISTINCT(CASE WHEN     ((TCBI.TC_LeadDispositionId=4 or TCBI.TC_LeadDispositionId=34) AND  ISNULL(TCBI.MostInterested,0)=1)  THEN TCBI.TC_BuyerInquiriesId END)) AS Converted,
		COUNT(DISTINCT(CASE WHEN     TCL.TC_LeadStageId = 1 AND  ISNULL(TCBI.MostInterested,0)=1  THEN TCBI.TC_BuyerInquiriesId END)) AS VerificationStage,
		COUNT(DISTINCT(CASE WHEN     TCL.TC_LeadStageId = 2 AND TCBI.TC_LeadDispositionId IS NULL  AND ISNULL(TCBI.MostInterested,0)=1 THEN TCBI.TC_BuyerInquiriesId END)) AS ConsultationStage, --folowup
		(@otherInqCount1 + @ServiceInqCount2)  as 'OtherInq',
		@ExcelLeadCount AS ExcelLeadCount
	FROM TC_Lead  AS TCL WITH(NOLOCK)
	JOIN TC_InquiriesLead AS TCIL WITH(NOLOCK)  ON TCL.TC_LeadId=TCIL.TC_LeadId
	JOIN TC_BuyerInquiries AS TCBI WITH(NOLOCK) ON TCIL.TC_InquiriesLeadId=TCBI.TC_InquiriesLeadId
								   AND  TCL.BranchId= @BranchId
								   AND  TCIL.BranchId=@BranchId
								   AND  TCIL.TC_LeadInquiryTypeId=1
								   AND  TCBI.CreatedOn BETWEEN @FromDate AND @TODate
END
ELSE IF (@TC_LeadInquiryTypeID=2)
BEGIN
			SET @ExcelLeadCount =
						  (SELECT COUNT(TC_ImportSellerInquiriesId)
						   FROM TC_ImportSellerInquiries B WITH(NOLOCK)
						   WHERE BranchId = @BranchId
						   AND B.IsDeleted=0 
							--AND B.UserId=@UserId 
							AND B.TC_ImportSellerInquiriesId IS NULL
							AND 
							(@FromDate IS NULL OR EntryDate > @FromDate) 
							AND 
							(@ToDate IS NULL OR EntryDate < @ToDate)
							)
	SELECT 
		COUNT(DISTINCT TCL.TC_LeadId) AS TotalLeads ,
		COUNT(DISTINCT(CASE WHEN  ISNULL(TCL.TC_LeadStageId,0)=0  THEN TCBI.TC_InquiriesLeadId END)) AS UnAssigned,   --modified
		COUNT(DISTINCT(CASE WHEN ((TCL.TC_LeadDispositionId=3 OR TCL.TC_LeadDispositionId=1 OR TCBI.TC_LeadDispositionId IS NOT  NULL)
		   AND (TCBI.TC_LeadDispositionId<>4 OR TCBI.TC_LeadDispositionId IS NULL)) THEN TCBI.TC_InquiriesLeadId END)) AS  Closed,
		--COUNT(DISTINCT(CASE WHEN     TCL.TC_LeadDispositionId= 3  AND TCL.TC_LeadStageId = 3                THEN TCL.TC_LeadId END)) AS NILeads,
		COUNT(DISTINCT(CASE WHEN     TCL.TC_LeadDispositionId= 41  THEN TCBI.TC_InquiriesLeadId END)) AS Archived,
		--COUNT(DISTINCT(CASE WHEN (( TCL.TC_LeadStageId = 3) OR  ((TCBI.TC_LeadDispositionId IS NOT  NULL  and  TCBI.TC_LeadDispositionId<>42)  AND TCL.TC_LeadStageId <>3 ) )  THEN TCBI.TC_BuyerInquiriesId END)) AS Closed,
		COUNT(DISTINCT(CASE WHEN     TCBI.TC_LeadDispositionId=4  THEN TCBI.TC_InquiriesLeadId END)) AS Converted,
		COUNT(DISTINCT(CASE WHEN     TCL.TC_LeadStageId = 1       THEN TCBI.TC_InquiriesLeadId END)) AS VerificationStage,
		COUNT(DISTINCT(CASE WHEN     TCL.TC_LeadStageId = 2 AND TCBI.TC_LeadDispositionId IS NULL  THEN TCBI.TC_InquiriesLeadId END)) AS ConsultationStage,
		(@otherInqCount1 + @ServiceInqCount2)  as 'OtherInq',
		@ExcelLeadCount AS ExcelLeadCount
	FROM TC_Lead  AS TCL WITH(NOLOCK)
	JOIN TC_InquiriesLead AS TCIL WITH(NOLOCK)  ON TCL.TC_LeadId=TCIL.TC_LeadId
	JOIN TC_SellerInquiries AS TCBI WITH(NOLOCK) ON TCIL.TC_InquiriesLeadId=TCBI.TC_InquiriesLeadId
								   AND  TCL.BranchId= @BranchId
								   AND  TCIL.BranchId=@BranchId
								   AND  TCIL.TC_LeadInquiryTypeId=2
								   AND  TCBI.CreatedOn BETWEEN @FromDate AND @TODate

END
ELSE IF (@TC_LeadInquiryTypeID=3)
BEGIN

	SET @ExcelLeadCount =
						(SELECT COUNT(Id)
						FROM TC_ExcelInquiries B WITH(NOLOCK)
						WHERE BranchId = @BranchId
						AND B.IsDeleted=0 
						--AND B.UserId=@UserId 
						AND B.TC_NewCarInquiriesId IS NULL
						AND 
						(@FromDate IS NULL OR EntryDate > @FromDate) 
						AND 
						(@ToDate IS NULL OR EntryDate < @ToDate)
						)

	SELECT 
		COUNT(DISTINCT (CASE WHEN  ISNULL(TCBI.MostInterested,0)=1 THEN TCBI.TC_NewCarInquiriesId END)) AS TotalLeads,
		COUNT(DISTINCT(CASE WHEN  ISNULL(TCL.TC_LeadStageId,0)=0 AND ISNULL(TCBI.MostInterested,0)=1 THEN TCBI.TC_NewCarInquiriesId END)) AS UnAssigned,   --Modified by Ashwini
		COUNT(DISTINCT(CASE WHEN ((TCL.TC_LeadDispositionId=3 OR TCL.TC_LeadDispositionId=1 OR TCBI.TC_LeadDispositionId IS NOT  NULL)
		   AND (TCBI.TC_LeadDispositionId<>4 OR TCBI.TC_LeadDispositionId IS NULL) AND ISNULL(TCBI.MostInterested,0)=1)  THEN TCBI.TC_NewCarInquiriesId END)) AS Closed,
		--COUNT(DISTINCT(CASE WHEN     TCL.TC_LeadDispositionId= 3  AND TCL.TC_LeadStageId = 3                THEN TCL.TC_LeadId END)) AS NILeads,
		COUNT(DISTINCT(CASE WHEN     TCL.TC_LeadDispositionId= 41 AND   ISNULL(TCBI.MostInterested,0)=1 THEN TCBI.TC_NewCarInquiriesId END)) AS Archived,
		--COUNT(DISTINCT(CASE WHEN (( TCL.TC_LeadStageId = 3) OR  ((TCBI.TC_LeadDispositionId IS NOT  NULL  and  TCBI.TC_LeadDispositionId<>42)  AND TCL.TC_LeadStageId <>3 ) )  THEN TCBI.TC_BuyerInquiriesId END)) AS Closed,
		COUNT(DISTINCT(CASE WHEN     TCBI.TC_LeadDispositionId=4  AND ISNULL(TCBI.MostInterested,0)=1 THEN TCBI.TC_NewCarInquiriesId END)) AS Converted,
		COUNT(DISTINCT(CASE WHEN     TCL.TC_LeadStageId = 1 AND ISNULL(TCBI.MostInterested,0)=1      THEN TCBI.TC_NewCarInquiriesId END)) AS VerificationStage,
		COUNT(DISTINCT(CASE WHEN     TCL.TC_LeadStageId = 2 AND TCBI.TC_LeadDispositionId IS NULL AND ISNULL(TCBI.MostInterested,0)=1   THEN TCBI.TC_NewCarInquiriesId END)) AS ConsultationStage,
		(@otherInqCount1 + @ServiceInqCount2)  as 'OtherInq',
		@ExcelLeadCount AS ExcelLeadCount
	FROM TC_Lead  AS TCL WITH(NOLOCK)
	JOIN TC_InquiriesLead AS TCIL WITH(NOLOCK)  ON TCL.TC_LeadId=TCIL.TC_LeadId
	JOIN TC_NewCarInquiries AS TCBI WITH(NOLOCK) ON TCIL.TC_InquiriesLeadId=TCBI.TC_InquiriesLeadId
								   AND  TCL.BranchId= @BranchId
								   AND  TCIL.BranchId=@BranchId
								   AND  TCIL.TC_LeadInquiryTypeId=3
								   AND  TCBI.CreatedOn BETWEEN @FromDate AND @TODate
END
END

----------------------------------------------



/****** Object:  StoredProcedure [dbo].[TC_UnassigendLeadAndEnquiryReport]    Script Date: 12/22/2015 6:59:33 PM ******/
SET ANSI_NULLS ON
