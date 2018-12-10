IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_TaskListPageLoad]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_TaskListPageLoad]
GO

	
-- =====================================================================================================
-- Author : Suresh Prajapti
-- Create date : 23rd June, 2016
-- Description : This procedure is used to fetch top 20 MyTask page data for specified Bucket
-- Modified By : Ashwini Dhamankar on June 28,2016 (added @LeadInquiryTypeId parameter)
-- Modified By : Suresh Prajapati on 29th June, 2016
-- Description : Added SP call "TC_TaskData" for fetching top 20 records
-- Modified By : Suresh Prajapati on 30th June, 2016
-- Description : Added SheduleOn Condition for ALL Bucket count
-- EXEC [TC_TaskListPageLoad] 20553,88927,27,6,1
-- EXEC [TC_TaskListPageLoad] 5,243,1,3,0,NULL   -Sales
-- Modified By : Suresh Prajapati on 3rd Aug 2016
-- Description : Replaced Sum with COUNT and created index on TC_BucketTypeId for table #TempBucketData
-- Modified By : Suresh Prajapati on 15th Sept, 2016
-- Description : Added Funnels Logic
-- ======================================================================================================
CREATE PROCEDURE [dbo].[TC_TaskListPageLoad]
	-- Add the parameters for the stored procedure here     
	@BranchId INT
	,@UserId INT
	,@LeadBucketId SMALLINT
	,@BusinessTypeId TINYINT = 3
	,@MasterDispositionId INT = 0
	,@LeadDispositionId INT = NULL
AS
SET NOCOUNT ON

BEGIN
	DECLARE @ScheduledOn DATETIME = CONVERT(DATETIME, CONVERT(VARCHAR(10), GETDATE(), 120) + ' 23:59:59')

	-- Fetching All Data with Applied Filters and storing into a temp table
	EXEC TC_TaskData @UserId
		,@BranchId
		,20
		,@LeadBucketId
		,@BusinessTypeId
		,@MasterDispositionId
		,@LeadDispositionId

	-- Get Current Bucket Data from Filtered and applying pagination
	-- Creating a temporary table to hold all bucket name with total counts
	SELECT TBL.TC_BucketLeadTypeId
		,TBL.BucketName
		,COUNT(TTL.TC_CallsId) AS TotalLead -- Modified by Suresh 03-08-2016 replace sum with count.
	INTO #TempBucketData
	FROM TC_BucketLeadType AS TBL WITH (NOLOCK)
	INNER JOIN TC_TaskLists TTL WITH (NOLOCK) ON TBL.TC_BucketLeadTypeId = TTL.BucketTypeId
		AND TTL.BranchId = @BranchId
		AND (
			TTL.UserId = @UserId
			OR @UserId IS NULL
			)
		AND TTL.ScheduledOn <= @ScheduledOn
	WHERE TBL.IsActive = 1
		AND TBL.TC_BucketLeadTypeId NOT IN (
			1
			,17
			,18
			,33
			)
		AND TBL.TC_BusinessTypeId = @BusinessTypeId --Added By Deepak on 14th July 2016
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
	GROUP BY TBL.TC_BucketLeadTypeId
		,TBL.BucketName

	CREATE CLUSTERED INDEX ix_TempBucketData ON #TempBucketData (TC_BucketLeadTypeId);

	-------------------------------------------------------------- Bucket Count Logic Ends Here -------------------------------------------------------
	DECLARE @AllLeadCount INT

	-- To Get Current Bucket Lead's Total Count (i.e. If 'NEW' tab is selected, then get total number of NEW lead counts)
	IF @LeadBucketId IN (
			1
			,17
			,18
			,33
			) -- All, because all will not have future date constraint
	BEGIN
		SELECT @AllLeadCount = COUNT(tc_callsid)
		FROM TC_TaskLists TTL WITH (NOLOCK)
		--LEFT JOIN TC_LeadDisposition AS LD WITH (NOLOCK) ON TTL.TC_LeadDispositionId = LD.TC_LeadDispositionId
		--LEFT JOIN TC_MasterLeadDisposition AS MLD WITH (NOLOCK) ON MLD.TC_MasterLeadDispositionId = LD.TC_MasterDispositionId
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
					)
				OR (TTL.BucketTypeId = @LeadBucketId)
				) -- 1 is for Sales All and 17 is for Service All
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

	--All Data
	INSERT INTO #TempTabCounts (
		BucketName
		,TC_BucketLeadTypeId
		)
	SELECT 'ALL (' + CONVERT(VARCHAR(30), COUNT(TTL.TC_CallsId)) + ')'
		,CASE @BusinessTypeId
			WHEN 3
				THEN 1
			WHEN 4
				THEN 17
			WHEN 5
				THEN 18
			WHEN 6
				THEN 33
			END
	FROM TC_TaskLists AS TTL WITH (NOLOCK)
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
	
	UNION ALL
	
	SELECT TBL.BucketName + ' (' + CONVERT(VARCHAR(30), ISNULL(TTL.TotalLead, 0)) + ')'
		,TBL.TC_BucketLeadTypeId
	FROM TC_BucketLeadType AS TBL WITH (NOLOCK)
	LEFT JOIN #TempBucketData AS TTL WITH (NOLOCK) ON TBL.TC_BucketLeadTypeId = TTL.TC_BucketLeadTypeId
	WHERE TBL.TC_BucketLeadTypeId NOT IN (
			1
			,17
			,18
			,33
			)

	SELECT ISNULL(@AllLeadCount, 0) AS RecordCount

	SELECT TBL.BucketName
		,TBL.TC_BucketLeadTypeId
	FROM #TempTabCounts AS TBL
	INNER JOIN TC_BucketLeadType BL WITH (NOLOCK) ON BL.TC_BucketLeadTypeId = TBL.TC_BucketLeadTypeId
		AND BL.IsActive = 1
		AND BL.TC_BusinessTypeId = @BusinessTypeId --Added By Deepak on 14th July 2016
	ORDER BY BL.PriorityOrder

	----------------------- Funnel Stage Logic Starts Here --------------------
	SELECT MLD.TC_MasterLeadDispositionId AS MasterLeadDispositionId
		,MLD.NAME AS FunnelName
		,ISNULL(COUNT(TL.TC_TaskListsId), 0) AS TotalCount
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
	WHERE (
			TTL.UserId = @UserId
			OR @UserId IS NULL
			)
		AND TTL.BranchId = @BranchId
		AND TTL.TC_BusinessTypeId = @BusinessTypeId

	SELECT MasterLeadDispositionId
		,FunnelName
		,TotalCount
	FROM #TempFunnelData
	ORDER BY PriorityOrder

	------------------------ Funnel Stage Logic Ends Here ---------------------
	DROP TABLE #TempBucketData

	DROP TABLE #TempTabCounts

	DROP TABLE #TempFunnelData
END

