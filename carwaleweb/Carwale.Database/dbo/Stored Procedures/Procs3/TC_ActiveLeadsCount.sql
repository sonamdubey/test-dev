IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_ActiveLeadsCount]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_ActiveLeadsCount]
GO

	--	Author	:	Sachin Bharti(13th March 2013)
--	Purpose :	To get total leads count of inquiries on basis 
--				of Source , Executives , Car Models
-- Modifie by Manish on 17-06-2013 for talling the count for volkswagon
-- Modified By: Nilesh Utture on 19th June, 2013 Data is only populated based on the users(@ReportingUsersList) who report to the logged in user
-- Modified by  : Nilesh utture on 12th July, 2013 Added is active condition
-- Modified By: Nilesh Utture on 18th July, 2013 Changed size 0f @ReportingUsersList to MAX	
-- Modified By: Nilesh Utture on 1st August, 2013 Changed count fetching logic for Source wise, Executive wise Leads. Also commented old logic for both of these
-- Modified By: Vivek Gupta on 05-11-2014, Added joins in all queries to match counts of totalleadscount and activeleadsconut
-- Modified By: Afrose on 7-06-2015, selecting group source from table TC_InquiryGroupSource instead of TC_InquirySource
-- Modified By : Nilima More On 26th July 2016 ,added condition for application id.
--	============================================================

CREATE  PROCEDURE [dbo].[TC_ActiveLeadsCount]
@FromDate	DateTime = NULL,
@ToDate	DateTime = NULL,
@EAGERNESS	VARCHAR(15),
@BRANCHID	INT,
@TYPE		INT,
@ReportingUsersList VARCHAR(MAX) = '-1'  -- '-1': No user is reporting to logged in user, -- Modified By: Nilesh Utture on 18th July, 2013 Changed size 0f @ReportingUsersList to MAX	
										 --	NULL: in case if logged in user is Admin, 
										 -- Else comma separated user List 
AS
BEGIN


	IF @TYPE = 1 -- To get the counts of total leads on Source wise basis
		BEGIN								
			-- Modified By: Nilesh Utture on 1st August, 2013
			SELECT  ISNULL(TIGS.GroupSourceName, 'Source not available') AS FieldName,ISNULL(TIGS.TC_InquiryGroupSourceId,'-1') AS LeadEagerness,COUNT(DISTINCT TIL.TC_LeadId) AS Total 
			FROM DEALERS as D WITH (NOLOCK)																									   -- Modified By: Nilesh Utture on 09th July, 2013 and 12th July, 2013
				 INNER JOIN TC_Users U WITH (NOLOCK) ON D.ID = U.BranchId /*AND  D.IsDealerActive= 1*/ AND D.ID = @BranchId AND U.BranchId = @BranchId AND U.lvl <> 0 AND U.IsActive = 1 AND (U.Id IN (SELECT ListMember AS UserId FROM fnSplitCSV(@ReportingUsersList)) OR @ReportingUsersList IS NULL)
				 LEFT JOIN TC_InquiriesLead AS TIL WITH (NOLOCK) ON U.ID=TIL.TC_UserId /*AND TCIL.CreatedDate BETWEEN @FromDate AND @EndDate*/ AND (TIL.TC_UserId IN (SELECT ListMember AS UserId FROM fnSplitCSV(@ReportingUsersList)) OR @ReportingUsersList IS NULL)
				 LEFT JOIN TC_InquirySource TIS WITH(NOLOCK) ON TIL.InqSourceId = TIS.Id
				 LEFT JOIN TC_InquiryGroupSource AS TIGS WITH(NOLOCK)ON TIS.TC_InquiryGroupSourceId=TIGS.TC_InquiryGroupSourceId
				 LEFT  JOIN TC_InquiryStatus TST WITH(NOLOCK) ON TIL.TC_InquiryStatusId = TST.TC_InquiryStatusId 
				 WHERE  TIL.CreatedDate BETWEEN @FromDate AND @ToDate
						AND (@EAGERNESS is null or ISNULL(TIL.TC_InquiryStatusId,-1)=@EAGERNESS)
						AND TIL.TC_LeadStageId <> 3 AND ISNULL(TIL.TC_LeadDispositionID, 0) <> 4
						AND TIL.TC_LeadInquiryTypeId = 3 and TIL.BranchId = @BRANCHID --LeadInquiryTypeId is 3 for New Car inquiries only 
						AND (TIL.TC_UserId IN (SELECT ListMember AS UserId FROM fnSplitCSV(@ReportingUsersList)) OR @ReportingUsersList IS NULL) -- Modified By: Nilesh Utture on 19th June, 2013
		--	AND TST.TC_InquiryStatusId IN ( SELECT ListMember FROM fnSplitCSV(@EAGERNESS ) )
			GROUP BY TIGS.TC_InquiryGroupSourceId, TIGS.GroupSourceName
			ORDER BY TIGS.TC_InquiryGroupSourceId
		END
	IF @TYPE = 2 -- To get the counts of total leads on basis of Executives
		BEGIN			
			-- Modified By: Nilesh Utture on 1st August, 2013 
			SELECT  ISNULL(U.UserName,'Not Assigned') AS FieldName,COUNT(DISTINCT TIL.TC_InquiriesLeadId) AS Total,ISNULL(TST.TC_InquiryStatusId,-1) AS LeadEagerness
			FROM DEALERS as D WITH (NOLOCK)																									   -- Modified By: Nilesh Utture on 09th July, 2013 and 12th July, 2013
				 INNER JOIN TC_Users U WITH (NOLOCK) ON D.ID = U.BranchId /*AND  D.IsDealerActive= 1*/ AND D.ID = @BranchId AND U.BranchId = @BranchId AND U.lvl <> 0 AND U.IsActive = 1 AND (U.Id IN (SELECT ListMember AS UserId FROM fnSplitCSV(@ReportingUsersList)) OR @ReportingUsersList IS NULL)
				 LEFT JOIN TC_InquiriesLead AS TIL WITH (NOLOCK) ON U.ID=TIL.TC_UserId /*AND TCIL.CreatedDate BETWEEN @FromDate AND @EndDate*/ AND (TIL.TC_UserId IN (SELECT ListMember AS UserId FROM fnSplitCSV(@ReportingUsersList)) OR @ReportingUsersList IS NULL)
			AND  (@EAGERNESS is null or ISNULL(TIL.TC_InquiryStatusId,-1)=@EAGERNESS)
			LEFT JOIN TC_InquiryStatus TST WITH(NOLOCK) ON TIL.TC_InquiryStatusId = TST.TC_InquiryStatusId AND TST.IsActive = 1
			WHERE TIL.CreatedDate BETWEEN @FromDate AND @ToDate
			AND TIL.TC_LeadStageId <> 3 AND ISNULL(TIL.TC_LeadDispositionID, 0) <> 4
			AND TIL.TC_LeadInquiryTypeId = 3 and TIL.BranchId = @BRANCHID --LeadInquiryTypeId is 3 for New Car inquiries only 
			AND U.IsActive = 1 -- Modified by  : Nilesh utture on 12th July, 2013 Added is active condition
			AND (TIL.TC_UserId IN (SELECT ListMember AS UserId FROM fnSplitCSV(@ReportingUsersList)) OR @ReportingUsersList IS NULL) -- Modified By: Nilesh Utture on 19th June, 2013
			--AND TST.TC_InquiryStatusId IN (  SELECT ListMember FROM fnSplitCSV(@EAGERNESS ) )
			GROUP BY U.UserName,TST.TC_InquiryStatusId
			ORDER BY U.UserName
		END
	IF @TYPE = 3 -- To get the counts of total leads on basis of Car Versions
		BEGIN
				SELECT ISNULL(VW.Model, 'No Model Alloted') AS FieldName,COUNT(DISTINCT  TIL.TC_InquiriesLeadId) AS Total,ISNULL(TST.TC_InquiryStatusId,-1) AS LeadEagerness
			FROM DEALERS as D WITH (NOLOCK)																									   -- Modified By: Nilesh Utture on 09th July, 2013 and 12th July, 2013
			INNER JOIN TC_Users U WITH (NOLOCK) ON D.ID = U.BranchId /*AND  D.IsDealerActive= 1*/ AND D.ID = @BranchId AND U.BranchId = @BranchId AND U.lvl <> 0 AND U.IsActive = 1 AND (U.Id IN (SELECT ListMember AS UserId FROM fnSplitCSV(@ReportingUsersList)) OR @ReportingUsersList IS NULL)
			LEFT JOIN TC_InquiriesLead AS TIL WITH (NOLOCK) ON U.ID=TIL.TC_UserId /*AND TCIL.CreatedDate BETWEEN @FromDate AND @EndDate*/ AND (TIL.TC_UserId IN (SELECT ListMember AS UserId FROM fnSplitCSV(@ReportingUsersList)) OR @ReportingUsersList IS NULL)
			LEFT JOIN TC_NewCarInquiries TNI WITH(NOLOCK) ON TNI.TC_InquiriesLeadId = TIL.TC_InquiriesLeadId
			AND  (@EAGERNESS is null or ISNULL(TIL.TC_InquiryStatusId,-1)=@EAGERNESS)
			LEFT JOIN vwAllMMV VW WITH(NOLOCK) ON VW.VersionId = TNI.VersionId AND VW.ApplicationId = D.ApplicationId
			LEFT JOIN TC_InquiryStatus TST WITH(NOLOCK) ON TIL.TC_InquiryStatusId = TST.TC_InquiryStatusId 
			WHERE TIL.CreatedDate BETWEEN @FromDate AND @ToDate
			AND TIL.TC_LeadStageId <> 3 AND ISNULL(TIL.TC_LeadDispositionID, 0) <> 4 -- -- Modified By: Nilesh Utture on 1st August, 2013. Showing only Active Leads
			AND TIL.TC_LeadInquiryTypeId = 3 and TIL.BranchId = @BRANCHID --LeadInquiryTypeId is 3 for New Car inquiries only 
			AND (TIL.TC_UserId IN (SELECT ListMember AS UserId FROM fnSplitCSV(@ReportingUsersList)) OR @ReportingUsersList IS NULL) -- Modified By: Nilesh Utture on 19th June, 2013
			--AND TST.TC_InquiryStatusId IN (  SELECT ListMember FROM fnSplitCSV(@EAGERNESS ) )
			GROUP BY VW.Model,TST.TC_InquiryStatusId
		END

END
