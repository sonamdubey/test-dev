IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_FetchEmployeeCountForReports]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_FetchEmployeeCountForReports]
GO

	-- =============================================
-- Created By: Vivek Gupta
-- Created Date:23-05-2013
---Description: For display counts on Repors of TC based on inquiries for usedbuyer and used seller.
-- Modified by Vivek Gupta on 13th dec,2013, added select statements to retrieve data related to call reports for both usedbuyer and used seller.
-- Modified By: Vivek Gupta on 20-12-2013, added condition A.ActionComments <> 'Customer verified'
-- Modified By: Manish on 15-12-2015 added with (nolock) keyword wherever not found.
-- Modifed By: Deepak Tripathi on 16th May 2016 - Changed logic behind sell inquiries Schedule/Active calls
-- Modeifed By Deepak on 17th May 2016, added distinct
--Modeifed By Deepak on 20h May 2016, added S.TC_LeadDispositionId IS NULL
-- =============================================
CREATE PROCEDURE [dbo].[TC_FetchEmployeeCountForReports]
(
@BranchId INT,
@FromDate DATETIME,
@ToDate DATETIME,
@TC_LeadInquiryTypeID TINYINT
)
AS
BEGIN	
	
	IF (@TC_LeadInquiryTypeID=1)
	BEGIN	
		SELECT U.UserName,U.Id,
				COUNT(DISTINCT(CASE WHEN     IL.TC_LeadStageId = 1 AND S.TC_LeadDispositionId IS NULL  THEN S.TC_BuyerInquiriesId END)) +
				COUNT(DISTINCT(CASE WHEN     IL.TC_LeadStageId = 2 AND S.TC_LeadDispositionId IS NULL  THEN S.TC_BuyerInquiriesId END)) AS Active,
				COUNT(DISTINCT(CASE WHEN (S.TC_LeadDispositionId=4)  THEN S.TC_BuyerInquiriesId END)) AS Converted,
				COUNT(DISTINCT(CASE WHEN (   S.TC_LeadDispositionID<>1
											  AND S.TC_LeadDispositionID<>3
											  AND S.TC_LeadDispositionID<>4
											  AND S.TC_LeadDispositionID IS NOT NULL)  THEN S.TC_BuyerInquiriesId END)) AS  Lost,
				COUNT(DISTINCT(CASE WHEN     IL.TC_LeadStageId = 1  AND IL.TC_UserId IS NOT NULL  AND S.TC_LeadDispositionID IS NULL  THEN S.TC_BuyerInquiriesId END)) AS NotContacted
		FROM TC_InquiriesLead IL  WITH (NOLOCK)
					  INNER JOIN TC_BuyerInquiries S WITH (NOLOCK) ON S.TC_InquiriesLeadId = IL.TC_InquiriesLeadId
					  INNER JOIN TC_Users U  WITH (NOLOCK) ON IL.TC_UserId = U.Id
					  WHERE IL.BranchId = @BranchId AND  S.CreatedOn BETWEEN @FromDate AND @ToDate
					  GROUP BY U.UserName,U.Id


       -- Modified By Vivek Gupta on 16th , dec, 2013  
		SELECT  U.UserName, U.Id,
		        COUNT(DISTINCT(CASE WHEN  (IL.TC_LeadStageId = 1 OR IL.TC_LeadStageId = 2) AND S.TC_LeadDispositionId IS NULL AND (S.CreatedOn BETWEEN @FromDate AND @ToDate) THEN S.TC_BuyerInquiriesId END)) AS CurrentInquiries,
				COUNT((CASE WHEN  (A.ScheduledOn BETWEEN @FromDate AND @ToDate) AND (S.CreatedOn BETWEEN @FromDate AND @ToDate) AND (ISNULL(A.ScheduledOn,0) <>  ISNULL(A.ActionTakenOn,1)) AND (A.ActionComments <> 'Customer Verified') AND IL.TC_LeadStageId <> 3  THEN S.TC_BuyerInquiriesId END)) AS Scheduled,
				COUNT((CASE WHEN  (A.ScheduledOn BETWEEN @FromDate AND @ToDate) AND (ISNULL(A.ScheduledOn,0) <>  ISNULL(A.ActionTakenOn,1)) AND (A.ActionComments <> 'Customer Verified') AND (S.CreatedOn BETWEEN @FromDate AND @ToDate) AND A.IsActionTaken = 1 AND IL.TC_LeadStageId <> 3  THEN S.TC_BuyerInquiriesId END)) AS Called,				
				COUNT(DISTINCT(CASE WHEN  (IL.TC_LeadStageId = 1 OR IL.TC_LeadStageId = 2) AND S.TC_LeadDispositionId IS NULL AND S.CreatedOn < @FromDate THEN S.TC_BuyerInquiriesId END)) AS CarryForwardInquiries,
				COUNT((CASE WHEN  (A.ScheduledOn <= @ToDate) AND (S.CreatedOn < @FromDate) AND IL.TC_LeadStageId <> 3 AND (ISNULL(A.ScheduledOn,0) <>  ISNULL(A.ActionTakenOn,1)) AND (A.ActionComments <> 'Customer Verified')  THEN S.TC_BuyerInquiriesId END)) AS ScheduledCarry,
				COUNT((CASE WHEN  (A.ScheduledOn <= @ToDate) AND (S.CreatedOn < @FromDate) AND (ISNULL(A.ScheduledOn,0) <>  ISNULL(A.ActionTakenOn,1)) AND (A.ActionComments <> 'Customer Verified') AND A.IsActionTaken = 1 AND IL.TC_LeadStageId <> 3  THEN S.TC_BuyerInquiriesId END)) AS CalledCarry
		FROM TC_InquiriesLead IL  WITH (NOLOCK)
					  INNER JOIN TC_BuyerInquiries S WITH (NOLOCK) ON S.TC_InquiriesLeadId = IL.TC_InquiriesLeadId
					  INNER JOIN TC_Users U WITH (NOLOCK)  ON IL.TC_UserId = U.Id
					  JOIN TC_Calls AS A WITH (NOLOCK)  ON A.TC_LeadId = IL.TC_LeadId AND IL.IsActive = 1
					  WHERE IL.BranchId = @BranchId
					  GROUP BY U.UserName,U.Id 

	END
	ELSE IF (@TC_LeadInquiryTypeID=2)
	BEGIN
		SELECT U.UserName,U.Id,
				COUNT(DISTINCT(CASE WHEN     IL.TC_LeadStageId = 1       THEN S.TC_SellerInquiriesId END)) +
				COUNT(DISTINCT(CASE WHEN     IL.TC_LeadStageId = 2 AND S.TC_LeadDispositionId IS NULL  THEN S.TC_SellerInquiriesId END)) AS Active,
				COUNT(DISTINCT(CASE WHEN (S.TC_LeadDispositionId=4)  THEN S.TC_SellerInquiriesId END)) AS Converted,
				COUNT(DISTINCT(CASE WHEN (   S.TC_LeadDispositionID<>1
											  AND S.TC_LeadDispositionID<>3
											  AND S.TC_LeadDispositionID<>4
											  AND S.TC_LeadDispositionID IS NOT NULL)  THEN S.TC_SellerInquiriesId END)) AS  Lost,
				COUNT(DISTINCT(CASE WHEN     IL.TC_LeadStageId = 1 AND IL.TC_UserId IS NOT NULL AND S.TC_LeadDispositionID IS NULL  THEN S.TC_SellerInquiriesId END)) AS NotContacted
		FROM TC_InquiriesLead IL WITH (NOLOCK)
					  INNER JOIN TC_SellerInquiries S WITH (NOLOCK)  ON S.TC_InquiriesLeadId = IL.TC_InquiriesLeadId
					  INNER JOIN TC_Users U  WITH (NOLOCK) ON IL.TC_UserId = U.Id
					  WHERE IL.BranchId = @BranchId AND S.CreatedOn BETWEEN @FromDate AND @ToDate
					  GROUP BY U.UserName, U.Id

        -- Modified By Vivek Gupta on 16th , dec, 2013 
        --Modeifed By Deepak on 16th May 2016
		 --Modeifed By Deepak on 17th May 2016, added distinct
		 --Modeifed By Deepak on 20h May 2016, added S.TC_LeadDispositionId IS NULL
		SELECT  U.UserName, U.Id,
		        COUNT(DISTINCT(CASE WHEN  IL.TC_LeadStageId <> 3 AND S.TC_LeadDispositionId IS NULL AND (S.CreatedOn BETWEEN @FromDate AND @ToDate) THEN S.TC_SellerInquiriesId END)) AS CurrentInquiries,
				COUNT(DISTINCT(CASE WHEN  IL.TC_LeadStageId <> 3 AND S.TC_LeadDispositionId IS NULL AND S.CreatedOn < @FromDate THEN S.TC_SellerInquiriesId END)) AS CarryForwardInquiries,
				COUNT(DISTINCT(CASE WHEN  IL.TC_LeadStageId <> 3 AND S.TC_LeadDispositionId IS NULL AND A.ActionComments <> 'Customer Verified' AND (A.ScheduledOn BETWEEN @FromDate AND @ToDate) AND (S.CreatedOn BETWEEN @FromDate AND @ToDate) AND ISNULL(A.IsActionTaken,0) <> 1 THEN A.TC_CallsId END)) AS Scheduled,
				COUNT(DISTINCT(CASE WHEN  IL.TC_LeadStageId <> 3 AND S.TC_LeadDispositionId IS NULL AND A.ActionComments <> 'Customer Verified' AND (A.ScheduledOn BETWEEN @FromDate AND @ToDate) AND (S.CreatedOn BETWEEN @FromDate AND @ToDate) AND ISNULL(A.IsActionTaken,0) = 1  THEN A.TC_CallsId END)) AS Called,				
				COUNT(DISTINCT(CASE WHEN  IL.TC_LeadStageId <> 3 AND S.TC_LeadDispositionId IS NULL AND A.ActionComments <> 'Customer Verified' AND (A.ScheduledOn <= @ToDate) AND (S.CreatedOn < @FromDate)  AND ISNULL(A.IsActionTaken,0) <> 1   THEN A.TC_CallsId END)) AS ScheduledCarry,
				COUNT(DISTINCT(CASE WHEN  IL.TC_LeadStageId <> 3 AND S.TC_LeadDispositionId IS NULL AND A.ActionComments <> 'Customer Verified' AND (A.ActionTakenOn <= @ToDate) AND (S.CreatedOn < @FromDate) AND ISNULL(A.IsActionTaken,0) = 1  THEN A.TC_CallsId END)) AS CalledCarry  

			FROM TC_InquiriesLead IL  WITH (NOLOCK) 
					  INNER JOIN TC_SellerInquiries S WITH (NOLOCK)  ON S.TC_InquiriesLeadId = IL.TC_InquiriesLeadId
					  INNER JOIN TC_Users U WITH (NOLOCK) ON IL.TC_UserId = U.Id
					  JOIN TC_Calls AS A WITH (NOLOCK) ON A.TC_LeadId = IL.TC_LeadId AND IL.IsActive = 1
					  WHERE IL.BranchId = @BranchId
					  GROUP BY U.UserName,U.Id 
					  
					  
					  
					  				
				

	END						   
END