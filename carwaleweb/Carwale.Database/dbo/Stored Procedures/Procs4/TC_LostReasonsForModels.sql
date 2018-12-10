IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_LostReasonsForModels]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_LostReasonsForModels]
GO

	---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------


--	Author	:	Sachin Bharti(14th March 2013)
--	Purpose	:	To get count of lossed leads based on reasons 
--				on particulr Car Model
-- Modified By: Nilesh Utture on 19th June, 2013 Data is only populated based on the users(@ReportingUsersList) who report to the logged in user
-- Modified by : Sachin Bharti (24th June 2013) Add date range filter
-- Modified By : Nilesh Utture on 9th July, 2013 Added TIL.CreatedDate in condition and commented /*TNI.BookingDate */
-- Modified By: Nilesh Utture on 18th July, 2013 Changed size 0f @ReportingUsersList to MAX	
-- Modified By : Ruchira Patil on 10th Dec 2015, Fetched TC_LeadDispositionId
-- EXEC TC_LostReasonsForModels 5,-1,1,'11/13/2015','12/14/2015',null
--	=========================================================
CREATE PROCEDURE [dbo].[TC_LostReasonsForModels]
@BRANCHID	INT,
@MODELID	INT,
@TYPE		INT,
@FromDate	DATETIME = NULL,
@ToDate		DATETIME = NULL,
@ReportingUsersList VARCHAR(MAX) = '-1'  -- '-1': No user is reporting to logged in user, -- Modified By: Nilesh Utture on 18th July, 2013 Changed size 0f @ReportingUsersList to MAX	
										 --	NULL: in case if logged in user is Admin, 
										 -- Else comma separated user List 
AS
BEGIN
	IF @TYPE = 1 --to get reasons on all models for which Dealer works 
		BEGIN
			SELECT TLD.Name AS Reason,COUNT(DISTINCT TIL.TC_InquiriesLeadId) AS TOTAL
			,TLD.TC_LeadDispositionId DispositionId --Added by ruchira Patil on 10th Dec 2015
			FROM TC_NewCarInquiries TNI WITH(NOLOCK)
			INNER JOIN TC_InquiriesLead TIL WITH(NOLOCK) ON TIL.TC_InquiriesLeadId = TNI.TC_InquiriesLeadId
			INNER JOIN vwMMV VW WITH(NOLOCK) ON  VW.VersionId = TNI.VersionId
			INNER JOIN TC_LeadDisposition TLD WITH(NOLOCK) ON TLD.TC_LeadDispositionId = TNI.TC_LeadDispositionId and TNI.TC_LeadDispositionId<>4--to remove converted leads
			WHERE TIL.TC_LeadInquiryTypeId = 3 AND TIL.BranchId = @BRANCHID  --for new car inquiries only
			AND (TIL.TC_UserId IN (SELECT ListMember AS UserId FROM fnSplitCSV(@ReportingUsersList)) OR @ReportingUsersList IS NULL) -- Modified By: Nilesh Utture on 19th June, 2013
			AND /*TNI.BookingDate */ TIL.CreatedDate BETWEEN @FromDate AND @ToDate -- Added By Sachin Bharti
			GROUP BY TLD.Name,TLD.TC_LeadDispositionId   -- Modified By : Nilesh Utture on 9th July, 2013
		END
	ELSE IF @TYPE = 2 --to get reasons on model that user selected for which dealer works
		BEGIN
			SELECT TLD.Name AS Reason,COUNT(DISTINCT TIL.TC_InquiriesLeadId) AS TOTAL 
			,TLD.TC_LeadDispositionId DispositionId  --Added by ruchira Patil on 10th Dec 2015
			FROM TC_NewCarInquiries TNI WITH(NOLOCK)
			INNER JOIN TC_InquiriesLead TIL WITH(NOLOCK) ON TIL.TC_InquiriesLeadId = TNI.TC_InquiriesLeadId
			INNER JOIN vwMMV VW WITH(NOLOCK) ON VW.VersionId = TNI.VersionId
			INNER JOIN TC_LeadDisposition TLD WITH(NOLOCK) ON TLD.TC_LeadDispositionId = TNI.TC_LeadDispositionId and TNI.TC_LeadDispositionId<>4--to remove converted leads
			WHERE TIL.TC_LeadInquiryTypeId = 3 AND TIL.BranchId = @BRANCHID AND VW.ModelId = @MODELID --for new car inquiries only
			AND (TIL.TC_UserId IN (SELECT ListMember AS UserId FROM fnSplitCSV(@ReportingUsersList)) OR @ReportingUsersList IS NULL) -- Modified By: Nilesh Utture on 19th June, 2013
			AND /*TNI.BookingDate */ TIL.CreatedDate BETWEEN @FromDate AND @ToDate -- Added By Sachin Bharti
			GROUP BY TLD.Name,TLD.TC_LeadDispositionId   -- Modified By : Nilesh Utture on 9th July, 2013
		END
END





