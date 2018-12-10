IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_TaskData_V16]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_TaskData_V16]
GO

	
-- =============================================
-- Author	   : Suresh Prajapati
-- Create date : 29th June, 2016
-- Description : This SP is used to get top 20 records for MyTask page
-- Modifed By : Suresh Prajapati on 18th July, 2016
-- Description : Added TC_BusinessTypeId in select
-- Modifed By : Suresh Prajapati on 26th July, 2016
-- Description : Added @LeadBucketId condition for Advantage leads
-- exec TC_TaskData_V16.9.1 88927,20553,200,33,6,3
-- exec TC_TaskData_V16.9.1 243,5,200,1,3,0,null
-- exec [TC_TaskData_V16.9.1] 88927,20553,20,33,6,2,120,1   -- insurance from dashboard
-- Modify By : Nilima More On Aug 23,2016,fetch RegistrationNumber from TC_taskList.
-- Modify By : Nilima More On 24th Aug 2016,add all insurance lead condition.
-- Modified By : Suresh Prajapati on 15th Sept, 2016
-- Description : 1. Added parameter @MasterDispositionId for Funnel filter
-- Modified By : Ashwini Dhamankar on Sept 22,2016 (added parameter @IsToday to fetch only today's scheduled leads)
-- Modified By : Tejashree Patil on 28 Sept 2016, Feedback calling bussinesstype lead changes done.
-- =============================================
CREATE PROCEDURE [dbo].[TC_TaskData_V16.10.1]
	-- Add the parameters for the stored procedure here
	@UserId INT
	,@BranchId INT
	,@TopCount INT
	,@LeadBucketId SMALLINT
	,@BusinessTypeId TINYINT = 3
	,@MasterDispositionId INT = 0
	,@LeadDispositionId INT = NULL
	,@IsToday BIT = 0
AS
BEGIN
	DECLARE @ScheduledOn DATETIME = CONVERT(DATETIME, CONVERT(VARCHAR(10), GETDATE(), 120) + ' 23:59:59')

	IF @LeadBucketId = 1
		SET @ScheduledOn = DATEADD(YY, 2, CONVERT(DATETIME, CONVERT(VARCHAR(10), GETDATE(), 120) + ' 23:59:59'))

	SELECT TOP (@TopCount) CustomerId
		,CustomerName
		,CustomerEmail AS Email
		,CustomerMobile AS Mobile
		,InqSourceId AS TC_InquirySourceId
		,TC_LeadId
		,TC_InquiryStatusId
		,ScheduledOn AS NextFollowUpDate
		,InterestedIn
		,TC_CallTypeId AS CallType
		,LastCallComment
		,LatestInquiryDate
		,OrderDate
		,InquirySourceName AS InquirySource
		,UserId
		,TC_LeadStageId
		,TC_NextActionId
		,InquiryTypeName AS InquiryType
		,TTL.TC_LeadInquiryTypeId AS InquiryTypeId
		,IsVerified
		,TC_InquiriesLeadCreateDate AS LeadCreationDate
		,BucketTypeId
		,ExchangeCar
		,Eagerness
		,Location
		,Car
		,LeadAge
		,AssignedTo
		,TC_InquiriesLeadId
		,BranchId
		,TC_CallsId
		,TTL.TC_LeadDispositionId
		,TC_BusinessTypeId
		,RegistrationNumber
	FROM TC_TaskLists TTL WITH (NOLOCK)
	--LEFT JOIN TC_LeadDisposition AS LD WITH (NOLOCK) ON TTL.TC_LeadDispositionId = LD.TC_LeadDispositionId
	--LEFT JOIN TC_MasterLeadDisposition AS MLD WITH (NOLOCK) ON MLD.TC_MasterLeadDispositionId = LD.TC_MasterDispositionId
	WHERE TTL.ScheduledON <= @ScheduledOn
		AND (
			TTL.UserId = @UserId
			OR @UserId IS NULL
			)
		AND TTL.BranchId = @BranchId
		AND (
			@LeadBucketId IN (
				1 -- Sales
				,17 -- Service
				,18 -- Advantage
				,33 -- Insurance -- Added by Nilima More On 24th Aug 2016,all insurance lead
				,34 -- FeedBack Calling -- Added By : Tejashree Patil on 28 Sept 2016
				)
			OR (TTL.BucketTypeId = @LeadBucketId)
			)
		AND (TTL.TC_BusinessTypeId = @BusinessTypeId) --Added By Deepak on 14th July 2016
		AND (
			(
				(ISNULL(@MasterDispositionId, 0) = 0)
				or TTL.TC_LeadDispositionId IN (
					SELECT TC_LeadDispositionId
					FROM TC_LeadDisposition WITH (NOLOCK)
					WHERE TC_MasterDispositionId = @MasterDispositionId
					)
				)
			)
		AND (
				@LeadDispositionId IS NULL
				OR
				(
					@LeadDispositionId IN (120,121) --   120 - check pick up , 121 - pay at showroom
					AND TTL.TC_LeadDispositionId = @LeadDispositionId
					AND ((@IsToday = 1 AND DATEDIFF(dd,TTL.TC_NextActionDate, GETDATE()) = 0) OR (@IsToday = 0 AND DATEDIFF(dd,TTL.TC_NextActionDate, GETDATE()) > 0)) 
					  --added by : Ashwini Dhamankar on Sept 22,2016
				)
				OR (TTL.TC_LeadDispositionId = @LeadDispositionId AND @LeadDispositionId NOT IN (120,121))
			)
			
	ORDER BY OrderDate DESC
END


