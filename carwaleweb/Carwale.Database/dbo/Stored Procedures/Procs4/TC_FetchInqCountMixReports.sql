IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_FetchInqCountMixReports]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_FetchInqCountMixReports]
GO

	
-- =============================================
-- Created By: Upendra Kumar
-- Created Date:11 Jan ,2016
-- Description: For display counts on IOS LeadREports testdrivecompleted and lead with no folloup.
-- EXEC TC_FetchInqCountMixReports 3838,'2006/01/01','2015/12/30', 1
-- =============================================
CREATE  PROCEDURE [dbo].[TC_FetchInqCountMixReports]
(
@BranchId INT,
@FromDate DATE,
@ToDate DATE,
@LeadInquiryTypeID TINYINT
)
AS
BEGIN	
	DECLARE @FollowUpPendingCount INT
	DECLARE @thirtyDaysOldLeadCount INT,@sixtyDaysOldLeadCount INT,@Booked INT   --,@leadsWithNoFolloup INT   -- declare all to get output as one table
    
	IF (@LeadInquiryTypeID=1)
	 BEGIN
		SELECT @FollowUpPendingCount = COUNT(DISTINCT(B.TC_BuyerInquiriesId)) 									  
        FROM TC_Calls C WITH(NOLOCK) 
		INNER JOIN TC_InquiriesLead IL WITH(NOLOCK) 	ON C.TC_LeadId = IL.TC_LeadId 
		INNER JOIN TC_BuyerInquiries B WITH(NOLOCK)     ON B.TC_InquiriesLeadId = IL.TC_InquiriesLeadId 
		WHERE IL.TC_LeadInquiryTypeId = @LeadInquiryTypeID
			 AND IL.BranchId = @BranchId
			 AND C.IsActionTaken = 0
			 AND CONVERT(DATE,C.ScheduledOn) BETWEEN @FromDate AND @ToDate

    --- this for get the count of leads whis creation date is between 30 and 60   AND greter then 60
     SELECT @thirtyDaysOldLeadCount = COUNT(DISTINCT (CASE WHEN (CreatedOn <= dateadd(dd,-30,getdate()) AND CreatedOn >= dateadd(dd,-60,getdate())) THEN TC_BuyerInquiriesId END)),
	       @sixtyDaysOldLeadCount = COUNT(DISTINCT (CASE WHEN (CreatedOn <= dateadd(dd,-60,getdate())) THEN TC_BuyerInquiriesId END)),
		   @Booked = COUNT(DISTINCT (CASE WHEN (BookingStatus = 34 AND CONVERT(DATE,BookingDate) BETWEEN @FromDate AND @ToDate) THEN TC_BuyerInquiriesId END))
	  FROM (SELECT  TCBI.TC_BuyerInquiriesId ,TCBI.CreatedOn,TCBI.BookingDate,TCBI.BookingStatus
				FROM TC_Lead  AS TCL WITH(NOLOCK)
				JOIN TC_InquiriesLead AS TCIL WITH(NOLOCK)  ON TCL.TC_LeadId=TCIL.TC_LeadId
				JOIN TC_BuyerInquiries AS TCBI WITH(NOLOCK) ON TCIL.TC_InquiriesLeadId=TCBI.TC_InquiriesLeadId
									   AND  TCL.BranchId = @BranchId
									   AND  TCIL.BranchId = @BranchId
									   AND  TCIL.TC_LeadInquiryTypeId=1)  AS tblTemp

		SELECT 	COUNT(DISTINCT TCBI.TC_BuyerInquiriesId) AS Total ,
			
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
				COUNT(DISTINCT(CASE WHEN (TCBI.TC_LeadDispositionId = 3)  THEN TCBI.TC_BuyerInquiriesId END)) AS NotInterested,
				@thirtyDaysOldLeadCount AS ThirtyDaysOldLeadCount,
				@sixtyDaysOldLeadCount AS SixtyDaysOldLeadCount,
				@Booked AS Booked

				FROM TC_Lead  AS TCL WITH(NOLOCK)
				JOIN TC_InquiriesLead AS TCIL WITH(NOLOCK)  ON TCL.TC_LeadId=TCIL.TC_LeadId
				JOIN TC_BuyerInquiries AS TCBI WITH(NOLOCK) ON TCIL.TC_InquiriesLeadId=TCBI.TC_InquiriesLeadId
									   AND  TCL.BranchId = @BranchId
									   AND  TCIL.BranchId = @BranchId
									   AND  TCIL.TC_LeadInquiryTypeId=1
									   AND  CONVERT(DATE, TCBI.CreatedOn) BETWEEN @FromDate AND @TODate									   
		
	 END
	ELSE IF (@LeadInquiryTypeID=2)
	  BEGIN
		SELECT @FollowUpPendingCount = COUNT(DISTINCT(S.TC_SellerInquiriesId)) 
		FROM TC_Calls C WITH(NOLOCK) 
		INNER JOIN TC_InquiriesLead IL WITH(NOLOCK) ON C.TC_LeadId = IL.TC_LeadId 
		INNER JOIN TC_SellerInquiries S WITH(NOLOCK)  ON S.TC_InquiriesLeadId = IL.TC_InquiriesLeadId 
			WHERE  IL.TC_LeadInquiryTypeId = @LeadInquiryTypeID
					AND IL.BranchId = @BranchId
					AND C.IsActionTaken = 0
					AND CONVERT(DATE,C.ScheduledOn) BETWEEN @FromDate AND @ToDate

   --- this for get the count of leads whis creation date is between 30 and 60   AND greter then 60
	SELECT @thirtyDaysOldLeadCount = COUNT(DISTINCT (CASE WHEN (CreatedOn <= dateadd(dd,-30,getdate()) AND CreatedOn >= dateadd(dd,-60,getdate())) THEN TC_SellerInquiriesId END)),
	       @sixtyDaysOldLeadCount = COUNT(DISTINCT (CASE WHEN (CreatedOn <= dateadd(dd,-60,getdate())) THEN TC_SellerInquiriesId END)),
		   @Booked = COUNT(DISTINCT (CASE WHEN (PurchasedStatus = 33 AND CONVERT(DATE,PurchasedDate) BETWEEN @FromDate AND @ToDate) THEN TC_SellerInquiriesId END))
	  FROM (SELECT  TCSI.TC_SellerInquiriesId ,TCSI.CreatedOn,TCSI.PurchasedDate ,TCSI.PurchasedStatus
				FROM TC_Lead  AS TCL WITH(NOLOCK)
					JOIN TC_InquiriesLead AS TCIL WITH(NOLOCK)  ON TCL.TC_LeadId=TCIL.TC_LeadId
					JOIN TC_SellerInquiries AS TCSI WITH(NOLOCK) ON TCIL.TC_InquiriesLeadId=TCSI.TC_InquiriesLeadId
									   AND  TCL.BranchId= @BranchId
									   AND  TCIL.BranchId=@BranchId
									   AND  TCIL.TC_LeadInquiryTypeId=2)  AS tblTemp
			
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
			COUNT(DISTINCT(CASE WHEN (TCSI.TC_LeadDispositionId = 3)  THEN TCSI.TC_SellerInquiriesId END)) AS NotInterested,
			@thirtyDaysOldLeadCount AS ThirtyDaysOldLeadCount,
			@sixtyDaysOldLeadCount AS SixtyDaysOldLeadCount,
			COUNT(DISTINCT(CASE WHEN(TCIL.TC_LeadDispositionId=4) THEN TCIL.TC_LeadDispositionId END)) AS Booked
			
			
		FROM TC_Lead  AS TCL WITH(NOLOCK)
		JOIN TC_InquiriesLead AS TCIL WITH(NOLOCK)  ON TCL.TC_LeadId=TCIL.TC_LeadId
		JOIN TC_SellerInquiries AS TCSI WITH(NOLOCK) ON TCIL.TC_InquiriesLeadId=TCSI.TC_InquiriesLeadId
									   AND  TCL.BranchId= @BranchId
									   AND  TCIL.BranchId=@BranchId
									   AND  TCIL.TC_LeadInquiryTypeId=2
									   AND  CONVERT(DATE, TCSI.CreatedOn) BETWEEN @FromDate AND @TODate
	END	
  ELSE IF (@LeadInquiryTypeID=3)
	 BEGIN
		SELECT @FollowUpPendingCount = COUNT(DISTINCT(NB.TC_NewCarInquiriesId)) 									  
        FROM TC_Calls C WITH(NOLOCK) 
		INNER JOIN TC_InquiriesLead IL WITH(NOLOCK) 	ON C.TC_LeadId = IL.TC_LeadId 
		INNER JOIN TC_NewCarInquiries NB WITH(NOLOCK)     ON NB.TC_InquiriesLeadId = IL.TC_InquiriesLeadId 
		WHERE IL.TC_LeadInquiryTypeId = @LeadInquiryTypeID
			 AND IL.BranchId = @BranchId
			 AND C.IsActionTaken = 0
			 AND CONVERT(DATE,C.ScheduledOn) BETWEEN @FromDate AND @ToDate

    --- this for get the count of leads whis creation date is between 30 and 60   AND greter then 60
      SELECT @thirtyDaysOldLeadCount = COUNT(DISTINCT (CASE WHEN (CreatedOn <= dateadd(dd,-30,getdate()) AND CreatedOn >= dateadd(dd,-60,getdate())) THEN TC_NewCarInquiriesId END)),
	       @sixtyDaysOldLeadCount = COUNT(DISTINCT (CASE WHEN (CreatedOn <= dateadd(dd,-60,getdate())) THEN TC_NewCarInquiriesId END)),
		   @Booked = COUNT(DISTINCT (CASE WHEN (BookingStatus = 32 AND CONVERT(DATE,BookingDate) BETWEEN @FromDate AND @ToDate) THEN TC_NewCarInquiriesId END))
	   FROM (SELECT  TCNI.TC_NewCarInquiriesId ,TCNI.CreatedOn,TCNI.BookingDate,TCNI.BookingStatus
				 FROM TC_Lead  AS TCL WITH(NOLOCK)
				 JOIN TC_InquiriesLead AS TCIL WITH(NOLOCK)  ON TCL.TC_LeadId=TCIL.TC_LeadId
				 JOIN TC_NewCarInquiries AS TCNI WITH(NOLOCK) ON TCIL.TC_InquiriesLeadId=TCNI.TC_InquiriesLeadId
									   AND  TCL.BranchId = @BranchId
									   AND  TCIL.BranchId = @BranchId
									   AND  TCIL.TC_LeadInquiryTypeId=3)  AS tblTemp

		SELECT 	COUNT(DISTINCT TCNI.TC_NewCarInquiriesId) AS Total ,
			
				COUNT(DISTINCT(CASE WHEN ( (   TCNI.TC_LeadDispositionId<>1
			                           AND TCNI.TC_LeadDispositionId<>3
			                           AND TCNI.TC_LeadDispositionId<>4) AND TCNI.TC_LeadDispositionId IS NOT NULL)
			                           THEN TCNI.TC_NewCarInquiriesId END)) AS  Lost,			                           
				COUNT(DISTINCT(CASE WHEN (TCNI.TC_LeadDispositionId=4 or TCNI.TC_LeadDispositionId=34)  THEN TCNI.TC_NewCarInquiriesId END)) AS Converted,
			
				COUNT(DISTINCT(CASE WHEN     TCIL.TC_LeadStageId = 1 AND TCIL.TC_UserId IS NOT NULL AND TCNI.TC_LeadDispositionId IS NULL    THEN TCNI.TC_NewCarInquiriesId END)) AS VerificationStage,
				COUNT(DISTINCT(CASE WHEN     TCIL.TC_LeadStageId = 2 AND TCNI.TC_LeadDispositionId IS NULL  THEN TCNI.TC_NewCarInquiriesId END)) AS ConsultationStage,
			
				COUNT(DISTINCT(CASE WHEN ( TCNI.TC_LeadDispositionId IS NULL AND TCIL.TC_InquiryStatusId =1 AND TCIL.TC_UserId IS NOT NULL) THEN TCNI.TC_NewCarInquiriesId END)) AS Hot,
				COUNT(DISTINCT(CASE WHEN ( TCNI.TC_LeadDispositionId IS NULL AND TCIL.TC_InquiryStatusId =2 AND TCIL.TC_UserId IS NOT NULL) THEN TCNI.TC_NewCarInquiriesId END)) AS Warm,
				COUNT(DISTINCT(CASE WHEN ( TCNI.TC_LeadDispositionId IS NULL AND TCIL.TC_InquiryStatusId =3 AND TCIL.TC_UserId IS NOT NULL) THEN TCNI.TC_NewCarInquiriesId END)) AS Normal,	
				COUNT(DISTINCT(CASE WHEN ( TCNI.TC_LeadDispositionId IS NULL AND TCIL.TC_InquiryStatusId IS NULL AND TCIL.TC_UserId IS NOT NULL) THEN TCNI.TC_NewCarInquiriesId END)) AS NotSet,		
			
				@FollowUpPendingCount AS FollowupPending,
				COUNT(DISTINCT(CASE WHEN  ISNULL(TCL.TC_LeadStageId,0)=0  THEN TCNI.TC_NewCarInquiriesId END)) AS UnAssigned,
			
				COUNT(DISTINCT(CASE WHEN (TCNI.TC_LeadDispositionId = 1)  THEN TCNI.TC_NewCarInquiriesId END)) AS Fake,
				COUNT(DISTINCT(CASE WHEN (TCNI.TC_LeadDispositionId = 3)  THEN TCNI.TC_NewCarInquiriesId END)) AS NotInterested,
				@thirtyDaysOldLeadCount AS ThirtyDaysOldLeadCount,
				@sixtyDaysOldLeadCount AS SixtyDaysOldLeadCount,
				@Booked AS Booked

				FROM TC_Lead  AS TCL WITH(NOLOCK)
				JOIN TC_InquiriesLead AS TCIL WITH(NOLOCK)  ON TCL.TC_LeadId=TCIL.TC_LeadId
				JOIN TC_NewCarInquiries AS TCNI WITH(NOLOCK) ON TCIL.TC_InquiriesLeadId=TCNI.TC_InquiriesLeadId
									   AND  TCL.BranchId = @BranchId
									   AND  TCIL.BranchId = @BranchId
									   AND  TCIL.TC_LeadInquiryTypeId=3
									   AND  CONVERT(DATE, TCNI.CreatedOn) BETWEEN @FromDate AND @TODate									   
		
	 END

	EXEC TC_GetTestDriveCount @BranchId,@FromDate,@ToDate	
 	EXEC TC_LeadsWithNoFollowUp @BranchId,@FromDate,@ToDate,@LeadInquiryTypeID			   
END


/****** Object:  StoredProcedure [dbo].[TC_BookingSummary]    Script Date: 16 07 2015 10:01:51 ******/
SET ANSI_NULLS ON

