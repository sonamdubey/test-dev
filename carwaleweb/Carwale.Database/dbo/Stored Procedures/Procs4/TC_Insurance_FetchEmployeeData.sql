IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_Insurance_FetchEmployeeData]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_Insurance_FetchEmployeeData]
GO

	-- =============================================
-- Author:		<Khushaboo Patil>
-- Create date: <19th oct 2016>
-- Description:	<Fetch Employee data>
--TC_Insurance_FetchEmployeeData 20553,'2016-10-19','2016-10-20 23:59:59.657',null
-- =============================================
create PROCEDURE [dbo].[TC_Insurance_FetchEmployeeData]
	@DealerId INT,
	@FromDate DATETIME = NULL,
	@ToDate DATETIME = NULL ,
	@ReportingUsersList VARCHAR(MAX) = NULL--'88929,88930'
AS
BEGIN

	CREATE TABLE #TempUsers (UserId INT)
		
	IF (@ReportingUsersList IS NOT NULL)
		BEGIN 
			INSERT INTO #TempUsers (UserId)
			SELECT ListMember AS UserId FROM fnSplitCSV(@ReportingUsersList)
		END 

	SELECT  REPLACE(TU.UserName,'''','') UserName,TU.lvl ,TU.Id UserId
			,COUNT(DISTINCT TL.TC_LeadId) TotalInquiriesCnt -- ALL ACTIVE INQUIRIES
			,COUNT(DISTINCT CASE WHEN LD.TC_MasterDispositionId  = 1 THEN IL.TC_LeadId END) ConfirmationPendingCnt -- TC_MasterDispositionId  = 1 FOR ConfirmationPending
			,COUNT(DISTINCT CASE WHEN LD.TC_MasterDispositionId  = 2 THEN IL.TC_LeadId END) PaymentPendingCnt -- TC_MasterDispositionId  = 2 FOR PaymentPending
			,COUNT(DISTINCT CASE WHEN IL.TC_LeadDispositionId  = 135 THEN IL.TC_LeadId END) PaymentFailedCnt --  TC_LeadDispositionId  = 135 FOR  PaymentFailed
			,COUNT(DISTINCT CASE WHEN LD.TC_MasterDispositionId  = 3 THEN IL.TC_LeadId END) RenewalPendingCnt -- TC_MasterDispositionId  = 3 FOR RenewalPending
			,COUNT(DISTINCT CASE WHEN LD.TC_MasterDispositionId  = 5 THEN IL.TC_LeadId END) RenewalCompletedCnt -- TC_MasterDispositionId  = 5 FOR  RenewalCompleted
			,COUNT(DISTINCT CASE WHEN LD.TC_MasterDispositionId  = 4 THEN IL.TC_LeadId END) LostCnt -- TC_MasterDispositionId  = 4 FOR Lost
			,COUNT(DISTINCT CASE WHEN TL.BucketTypeId  = 26 THEN TL.TC_LeadId END) PendingFirstFollowUpCnt -- BucketTypeId  = 26 FOR NEW (NEW INQ WITH NO FOLLOW UP)
			,COUNT(DISTINCT CASE WHEN TL.BucketTypeId  = 28 THEN TL.TC_LeadId END) MissedFollowUpsCnt -- BucketTypeId  = 28 FOR PENDING (INQ HAVING SCHEDULEDON < GETDATE())
	FROM	TC_Users TU WITH(NOLOCK)
			LEFT JOIN TC_InquiriesLead IL WITH(NOLOCK) ON IL.TC_UserId = TU.Id AND IL.CreatedDate BETWEEN @FromDate AND @ToDate
			LEFT JOIN TC_LeadDisposition LD WITH(NOLOCK) ON LD.TC_LeadDispositionId = IL.TC_LeadDispositionId
			LEFT JOIN TC_TaskLists TL WITH(NOLOCK) ON TL.TC_LeadId = IL.TC_LeadId
	WHERE	(TU.ID IN (SELECT UserId FROM #TempUsers) OR @ReportingUsersList IS NULL)	AND 
			TU.BranchId = @DealerId 
			AND TU.IsActive = 1 
			AND IsCarwaleUser = 0 
	GROUP BY TU.Id,TU.UserName,TU.lvl , TU.NodeCode
	ORDER BY TU.NodeCode ASC

	DROP TABLE #TempUsers
END

