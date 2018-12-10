IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_TaskListPaging_V16_10_2]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_TaskListPaging_V16_10_2]
GO

	
-- =====================================================================================================
-- Author : Suresh Prajapti
-- Create date : 23rd June, 2016
-- Description : This procedure is used to fetch top 20 MyTask page data for specified Bucket and paging
-- EXEC TC_TaskListPaging 20553,88927,33,1,20,6,2,120
-- EXEC [dbo].[TC_TaskListPaging_V16.10.1] 20553,88927,33,1,20,6,2,121,1
-- EXEC TC_TaskListPaging 20466,88916,33,1,20,4,0,null   --service
-- Modified By : Ashwini Dhamankar on June 28,2016 (added @LeadInquiryTypeId parameter)
-- Modified By : Suresh Prajapati on 26th Jul, 2016
-- Description : Added Filters for Advantage MyTask.
-- Modifed By : Nilima More On 23rd August 2016,added RegistrationNumber in #tmpAllData . 
-- Modified By : Suresh Prajapati on 15th Sept, 2016
-- Description : Added Funnels Logic
-- Modified By : Ashwini Dhamankar on Sept 22,2016 (Added @IsToday Parameter)
-- Modified By : Tejashree Patil on 28 Sept 2016, Feedback calling bussinesstype lead changes done.
-- Modified by : Khushaboo Patil on 26 oct 2016 fetch ExpiryDate,TC_NextActionDate
-- modified By :  Ruchira Patil on 27 oct 2016 (updated funnel count query to fetch data based on @IsToday)
-- modified By :  Ruchira Patil on 3rd Nov 2016 (removed @MasterDispositionId condition in fetching the funnel count query)
-- ======================================================================================================
CREATE PROCEDURE [dbo].[TC_TaskListPaging_V16_10_2]
	-- Add the parameters for the stored procedure here
	@BranchId INT
	,@UserId INT
	,@LeadBucketId SMALLINT
	,@FromIndex INT = NULL
	,@ToIndex INT = NULL
	,@BusinessTypeId TINYINT = 3
	,@MasterDispositionId INT = 0
	,@LeadDispositionId INT = NULL
	,@IsToday BIT = 0
AS
SET NOCOUNT ON

BEGIN
	DECLARE @ScheduledOn DATETIME = DATEADD(YY, 2, CONVERT(DATETIME, CONVERT(VARCHAR(10), GETDATE(), 120) + ' 23:59:59'))
	DECLARE @TodaysDate DATETIME = CONVERT(DATETIME, CONVERT(VARCHAR(10), GETDATE(), 120) + ' 23:59:59')

	-------------------------------------------------------------- MyTask Current Bucket Data Logic -------------------------------------------------------
	-- Fetching All Data with Applied Filters and storing into a temp table
	SELECT TOP (@ToIndex) CustomerId
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
		,ROW_NUMBER() OVER (
			ORDER BY OrderDate DESC
			) NumberForPaging
		,RegistrationNumber
		,ExpiryDate
		,TC_NextActionDate
	INTO #tmpAllData
	FROM TC_TaskLists TTL WITH (NOLOCK)
	WHERE TTL.ScheduledON <= @ScheduledOn
		AND (
			TTL.UserId = @UserId
			OR @UserId IS NULL
			)
		AND TTL.BranchId = @BranchId
		AND (
			@LeadBucketId IN (
				1
				,17
				,18
				,33 --Insurance All, added by Nilima More On 24th Aug 2016
				,34 -- Feedback calling added by Tejashree Patil on 28 Sept 2016
				) --added by : Ashwini Dhamankar on June 29,2016
			OR (TTL.BucketTypeId = @LeadBucketId)
			)
		AND TTL.TC_BusinessTypeId = @BusinessTypeId --Added By Deepak on 14th July 2016
		AND (
			@LeadDispositionId IS NULL
			OR (
				@LeadDispositionId IN (
					120
					,121
					) --   120 - check pick up , 121 - pay at showroom
				AND TTL.TC_LeadDispositionId = @LeadDispositionId
				AND (
					(
						@IsToday = 1
						AND DATEDIFF(dd, TTL.TC_NextActionDate, GETDATE()) = 0
						)
					OR (
						@IsToday = 0
						AND DATEDIFF(dd, TTL.TC_NextActionDate, GETDATE()) > 0
						)
					)
				--added by : Ashwini Dhamankar on Sept 22,2016
				)
			OR (
				TTL.TC_LeadDispositionId = @LeadDispositionId
				AND @LeadDispositionId NOT IN (
					120
					,121
					)
				)
			)
		AND (
			(ISNULL(@MasterDispositionId, 0) = 0)
			OR TTL.TC_LeadDispositionId IN (
				SELECT TC_LeadDispositionId
				FROM TC_LeadDisposition WITH (NOLOCK)
				WHERE TC_MasterDispositionId = @MasterDispositionId
				)
			)
	ORDER BY OrderDate DESC

	-- Get Current Bucket Data from Filtered and applying pagination
	SELECT CustomerId
		,CustomerName
		,TTL.Email AS Email
		,Mobile AS Mobile
		,TC_InquirySourceId AS TC_InquirySourceId
		,TC_LeadId
		,TC_InquiryStatusId
		,NextFollowUpDate AS NextFollowUpDate
		,InterestedIn
		,CallType AS CallType
		,LastCallComment
		,LatestInquiryDate
		,OrderDate
		,InquirySource AS InquirySource
		,UserId
		,TC_LeadStageId
		,TC_NextActionId
		,InquiryType AS InquiryType
		,TTL.InquiryTypeId AS InquiryTypeId
		,IsVerified
		,LeadCreationDate AS LeadCreationDate
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
		,ExpiryDate
		,TC_NextActionDate
	FROM #tmpAllData AS TTL
	WHERE NumberForPaging BETWEEN @FromIndex
			AND @ToIndex

	-------------------------------------------------------------- MyTask Current Bucket's Data Logic Ends Here -----------------------------
	-------------------------------------------------------------- Bucket Count Logic -------------------------------------------------------
	-- Creating a temporary table to hold all bucket name with total counts
	SELECT TBL.TC_BucketLeadTypeId
		,TBL.BucketName
		,ISNULL(SUM(CASE 
					WHEN (TTL.BucketTypeId = TBL.TC_BucketLeadTypeId)
						THEN 1
					ELSE 0
					END), 0) AS TotalLead
	INTO #TempBucketData
	FROM TC_BucketLeadType AS TBL WITH (NOLOCK)
	LEFT JOIN TC_TaskLists TTL WITH (NOLOCK) ON TBL.TC_BucketLeadTypeId NOT IN (
			1
			,17
			,18
			,33 -- Insurance All leads,Added by : Nilima More on Aug 24,2016
			,34 -- Feedback calling added by Tejashree Patil on 28 Sept 2016
			)
		AND TBL.IsActive = 1
		--LEFT JOIN TC_LeadDisposition AS LD WITH (NOLOCK) ON TTL.TC_LeadDispositionId = LD.TC_LeadDispositionId
		--INNER JOIN TC_MasterLeadDisposition AS MLD WITH (NOLOCK) ON MLD.TC_MasterLeadDispositionId = LD.TC_MasterDispositionId
		AND TTL.BranchId = @BranchId
		AND (
			TTL.UserId = @UserId
			OR @UserId IS NULL
			)
		AND TTL.ScheduledOn <= @TodaysDate
		AND (
			(
				(ISNULL(@MasterDispositionId, 0) = 0)
				OR TTL.TC_LeadDispositionId IN (
					SELECT TC_LeadDispositionId
					FROM TC_LeadDisposition WITH (NOLOCK)
					WHERE TC_MasterDispositionId = @MasterDispositionId
					)
				)
			)
		AND (
			@LeadDispositionId IS NULL
			OR (
				@LeadDispositionId IN (
					120
					,121
					) --   120 - check pick up , 121 - pay at showroom
				AND TTL.TC_LeadDispositionId = @LeadDispositionId
				AND (
					(
						@IsToday = 1
						AND DATEDIFF(dd, TTL.TC_NextActionDate, GETDATE()) = 0
						)
					OR (
						@IsToday = 0
						AND DATEDIFF(dd, TTL.TC_NextActionDate, GETDATE()) > 0
						)
					)
				--added by : Ashwini Dhamankar on Sept 22,2016
				)
			OR (
				TTL.TC_LeadDispositionId = @LeadDispositionId
				AND @LeadDispositionId NOT IN (
					120
					,121
					)
				)
			)
	WHERE TBL.IsActive = 1
		AND TBL.TC_BucketLeadTypeId NOT IN (
			1
			,17
			,18
			,33 --Insurance All, added by Nilima More On 24th Aug 2016
			,34 -- Feedback calling added by Tejashree Patil on 28 Sept 2016
			)
		AND TBL.TC_BusinessTypeId = @BusinessTypeId --Added By Deepak on 14th July 2016
	GROUP BY TBL.TC_BucketLeadTypeId
		,TBL.BucketName

	-------------------------------------------------------------- Bucket Count Logic Ends Here -------------------------------------------------------
	DECLARE @AllLeadCount INT

	-- To Get Current Bucket Lead's Total Count (i.e. If 'NEW' tab is selected, then get total number of NEW lead counts)
	IF (
			@LeadBucketId IN (
				1
				,17
				,18
				,33
				,34 -- Feedback calling added by Tejashree Patil on 28 Sept 2016
				)
			) -- All, because all will not have future date constraint
	BEGIN
		SELECT @AllLeadCount = COUNT(tc_callsid)
		FROM TC_TaskLists TTL WITH (NOLOCK)
		--LEFT JOIN TC_LeadDisposition AS LD WITH (NOLOCK) ON TTL.TC_LeadDispositionId = LD.TC_LeadDispositionId
		--INNER JOIN TC_MasterLeadDisposition AS MLD WITH (NOLOCK) ON MLD.TC_MasterLeadDispositionId = LD.TC_MasterDispositionId
		WHERE (
				TTL.UserId = @UserId
				OR @UserId IS NULL
				)
			AND TTL.BranchId = @BranchId
			AND (
				@LeadBucketId IN (
					1
					,17
					,18
					,33
					,34 -- Feedback calling added by Tejashree Patil on 28 Sept 2016
					)
				OR (TTL.BucketTypeId = @LeadBucketId)
				) -- 1 is for Sales All and 17 is for Service All and 33 for insurance All
			AND TTL.TC_BusinessTypeId = @BusinessTypeId --Added By Deepak on 14th July 2016
			AND (
				(
					(ISNULL(@MasterDispositionId, 0) = 0)
					OR TTL.TC_LeadDispositionId IN (
						SELECT TC_LeadDispositionId
						FROM TC_LeadDisposition WITH (NOLOCK)
						WHERE TC_MasterDispositionId = @MasterDispositionId
						)
					)
				)
			AND (
				@LeadDispositionId IS NULL
				OR (
					@LeadDispositionId IN (
						120
						,121
						) --   120 - check pick up , 121 - pay at showroom
					AND TTL.TC_LeadDispositionId = @LeadDispositionId
					AND (
						(
							@IsToday = 1
							AND DATEDIFF(dd, TTL.TC_NextActionDate, GETDATE()) = 0
							)
						OR (
							@IsToday = 0
							AND DATEDIFF(dd, TTL.TC_NextActionDate, GETDATE()) > 0
							)
						)
					--added by : Ashwini Dhamankar on Sept 22,2016
					)
				OR (
					TTL.TC_LeadDispositionId = @LeadDispositionId
					AND @LeadDispositionId NOT IN (
						120
						,121
						)
					)
				)
	END
	ELSE -- Others
	BEGIN
		SELECT @AllLeadCount = TotalLead
		FROM #TempBucketData
		WHERE TC_BucketLeadTypeId = @LeadBucketId
	END

	-------------------------------------------------------------- Bucket Tab Creation Logic -------------------------------------------------------
	CREATE TABLE #TempTabCounts (
		BucketName VARCHAR(30)
		,TC_BucketLeadTypeId SMALLINT
		)

	INSERT INTO #TempTabCounts
	SELECT 'ALL (' + CONVERT(VARCHAR(30), COUNT(tc_callsid)) + ')'
		,CASE @BusinessTypeId
			WHEN 3
				THEN 1
			WHEN 4
				THEN 17
			WHEN 5
				THEN 18
			WHEN 6
				THEN 33 --added by Nilima More On 24th Aug 2016
			WHEN 7
				THEN 34 -- Feedback calling added by Tejashree Patil on 28 Sept 2016
			END --Added By Deepak on 14th July 2016
	FROM TC_TaskLists AS TTL WITH (NOLOCK)
	--LEFT JOIN TC_LeadDisposition AS LD WITH (NOLOCK) ON TTL.TC_LeadDispositionId = LD.TC_LeadDispositionId
	--INNER JOIN TC_MasterLeadDisposition AS MLD WITH (NOLOCK) ON MLD.TC_MasterLeadDispositionId = LD.TC_MasterDispositionId
	WHERE (
			TTL.UserId = @UserId
			OR @UserId IS NULL
			)
		AND TTL.BranchId = @BranchId
		AND TTL.TC_BusinessTypeId = @BusinessTypeId --Added By Deepak on 14th July 2016
		AND (
			(
				(ISNULL(@MasterDispositionId, 0) = 0)
				OR TTL.TC_LeadDispositionId IN (
					SELECT TC_LeadDispositionId
					FROM TC_LeadDisposition WITH (NOLOCK)
					WHERE TC_MasterDispositionId = @MasterDispositionId
					)
				)
			)
		AND (
			@LeadDispositionId IS NULL
			OR (
				@LeadDispositionId IN (
					120
					,121
					) --   120 - check pick up , 121 - pay at showroom
				AND TTL.TC_LeadDispositionId = @LeadDispositionId
				AND (
					(
						@IsToday = 1
						AND DATEDIFF(dd, TTL.TC_NextActionDate, GETDATE()) = 0
						)
					OR (
						@IsToday = 0
						AND DATEDIFF(dd, TTL.TC_NextActionDate, GETDATE()) > 0
						)
					)
				--added by : Ashwini Dhamankar on Sept 22,2016
				)
			OR (
				TTL.TC_LeadDispositionId = @LeadDispositionId
				AND @LeadDispositionId NOT IN (
					120
					,121
					)
				)
			)

	INSERT INTO #TempTabCounts
	SELECT BucketName + ' (' + CONVERT(VARCHAR(30), TotalLead) + ')'
		,TC_BucketLeadTypeId
	FROM #TempBucketData AS TTL WITH (NOLOCK)

	SELECT @AllLeadCount AS RecordCount

	--FROM #TempBucketData
	SELECT TBL.*
	FROM #TempTabCounts AS TBL
	INNER JOIN TC_BucketLeadType BL WITH (NOLOCK) ON BL.TC_BucketLeadTypeId = TBL.TC_BucketLeadTypeId
		AND BL.IsActive = 1
		AND BL.TC_BusinessTypeId = @BusinessTypeId --Added By Deepak on 14th July 2016
	ORDER BY BL.PriorityOrder

	----------------------- Funnel Stage Logic Starts Here --------------------
	SELECT MLD.TC_MasterLeadDispositionId AS MasterLeadDispositionId
		,MLD.NAME AS FunnelName
		,ISNULL(COUNT(DISTINCT TL.TC_LeadId), 0) AS TotalCount
		,MLD.PriorityOrder
	INTO #TempFunnelData
	FROM TC_MasterLeadDisposition AS MLD WITH (NOLOCK)
	INNER JOIN TC_LeadDisposition AS LD WITH (NOLOCK) ON MLD.TC_MasterLeadDispositionId = LD.TC_MasterDispositionId
	LEFT JOIN TC_TaskLists AS TL WITH (NOLOCK) ON LD.TC_LeadDispositionId = TL.TC_LeadDispositionId
		AND TL.BranchId = @BranchId
		AND (
			TL.UserId = @UserId
			OR @UserId IS NULL
			)
		AND TL.ScheduledOn <= @ScheduledOn
		AND (
			@LeadDispositionId IS NULL
			OR (
				@LeadDispositionId IN (
					120
					,121
					) --   120 - check pick up , 121 - pay at showroom
				AND TL.TC_LeadDispositionId = @LeadDispositionId
				AND (
					(
						@IsToday = 1
						AND DATEDIFF(dd, TL.TC_NextActionDate, GETDATE()) = 0
						)
					OR (
						@IsToday = 0
						AND DATEDIFF(dd, TL.TC_NextActionDate, GETDATE()) > 0
						)
					)
				--added by : Ashwini Dhamankar on Sept 22,2016
				)
			OR (
				TL.TC_LeadDispositionId = @LeadDispositionId
				AND @LeadDispositionId NOT IN (
					120
					,121
					)
				)
			) --modified By :  Ruchira Patil on 27 oct 2016
	WHERE MLD.IsVisible = 1
	GROUP BY MLD.TC_MasterLeadDispositionId
		,MLD.NAME
		,MLD.PriorityOrder

	INSERT INTO #TempFunnelData (
		MasterLeadDispositionId
		,FunnelName
		,TotalCount
		,PriorityOrder
		)
	SELECT 0
		,'All'
		,COUNT(TC_CallsId)
		,0
	FROM TC_TaskLists AS TTL WITH (NOLOCK)
	--LEFT JOIN TC_LeadDisposition AS LD WITH (NOLOCK) ON TTL.TC_LeadDispositionId = LD.TC_LeadDispositionId
	--INNER JOIN TC_MasterLeadDisposition AS MLD WITH (NOLOCK) ON MLD.TC_MasterLeadDispositionId = LD.TC_MasterDispositionId
	WHERE (
			TTL.UserId = @UserId
			OR @UserId IS NULL
			)
		AND TTL.BranchId = @BranchId
		AND TTL.TC_BusinessTypeId = @BusinessTypeId
		AND TtL.ScheduledOn <= @ScheduledOn
		AND (
			@LeadDispositionId IS NULL
			OR (
				@LeadDispositionId IN (
					120
					,121
					) --   120 - check pick up , 121 - pay at showroom
				AND TTL.TC_LeadDispositionId = @LeadDispositionId
				AND (
					(
						@IsToday = 1
						AND DATEDIFF(dd, TTL.TC_NextActionDate, GETDATE()) = 0
						)
					OR (
						@IsToday = 0
						AND DATEDIFF(dd, TTL.TC_NextActionDate, GETDATE()) > 0
						)
					)
				--added by : Ashwini Dhamankar on Sept 22,2016
				)
			OR (
				TTL.TC_LeadDispositionId = @LeadDispositionId
				AND @LeadDispositionId NOT IN (
					120
					,121
					)
				)
			) -- modified By :  Ruchira Patil on 27 oct 2016

	SELECT MasterLeadDispositionId
		,FunnelName
		,TotalCount
	FROM #TempFunnelData
	ORDER BY PriorityOrder

	------------------------ Funnel Stage Logic Ends Here ---------------------
	DROP TABLE #tmpAllData

	DROP TABLE #TempBucketData

	DROP TABLE #TempTabCounts

	DROP TABLE #TempFunnelData
END
