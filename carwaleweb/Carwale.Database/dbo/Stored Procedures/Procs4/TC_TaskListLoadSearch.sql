IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_TaskListLoadSearch]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_TaskListLoadSearch]
GO

	
-- =====================================================================================================
-- Author : Suresh Prajapati
-- Create date : 20th June 2016
-- Description : To get MyTask searched data
-- EXEC TC_TaskListLoadSearch 20553,88927,21,40,null,null,null,null,null,null,6,null,null,null,null,33,null,null,6,0
-- EXEC TC_TaskListLoadSearch 5,243,1,20,null,null,null,null,null,null,1,null,null,null,null,1,null,null,3,0,NULL
-- exec [TC_TaskListLoadSearch_V3.0] 20466,88916,1,10,'good',NULL,NULL,NULL,NULL,'good',1,NULL,NULL,NULL,NULL,1,NULL,NULL,4  --service lead
-- Modified By : Ashwini Dhamankar on June 30,2016 (Added @LeadInquiryTypeId parameter)
-- Modified By : Chetan Navin on Jul 14,2016 (Added like in where clause in case of car)
-- Modifed By  : Suresh Prajapati on18th July, 2016
-- Description : Added TC_BusinessTypeId in select
-- Modifed By  : Suresh Prajapati on 26th July, 2016
-- Description : Added conditions for Advantage Leads.
-- Modified By : Nilima More On 23rd Aug 2016,added registrationNumber in tempTable.
-- Modified By : Suresh Prajapati on 15th Sept, 2016
-- Description : 1. Added Funnels Logic
--				 2. Added parameter @MasterDispositionId for Funnel filter
-- ======================================================================================================
CREATE PROCEDURE [dbo].[TC_TaskListLoadSearch]
	-- Add the parameters for the stored procedure here     
	@BranchId INT
	,@UserId INT
	,@FromIndex INT = NULL
	,@ToIndex INT = NULL
	,
	--@Type TINYINT , 
	@CustomerName VARCHAR(100)
	,@CustomerMobile VARCHAR(50)
	,@CustomerEmail VARCHAR(100)
	,@FromFolloupdate AS DATETIME
	,@ToFollowupdate AS DATETIME
	,@SearchText VARCHAR(50) = NULL
	,@FilterType TINYINT = 1
	,
	-----------------------------------------------------
	--Added By Vivek on 27th June,2013,Added parameters for Advanced search
	@InqStatus VARCHAR(10) = NULL
	,@InqPriority VARCHAR(10) = NULL --HOT,WARM,NORMAL,BOOKED
	,@InqAddedDate AS DATETIME = NULL
	,
	-----------------------------------------------------
	@LeadIds AS VARCHAR(MAX) = NULL
	,@LeadBucketId AS SMALLINT = 1 --All Leads
	,@CarName VARCHAR(100) = NULL
	,@SourceName VARCHAR(50) = NULL
	-----------------------------------------------------
	,@BusinessTypeId TINYINT = 3 --can be null only for leadinquirytypeid 3 (existing) for service lead - 5(mandatory)
	,@MasterDispositionId INT = 0
	,@LeadDispositionId INT = NULL
AS
SET NOCOUNT ON

BEGIN
	-------------------------------------------------------------- Search Filter Paramteres Logic ---------------------------------------------------------
	DECLARE @DealerDeals BIT = 0

	IF EXISTS (
			SELECT DealerId
			FROM TC_Deals_Dealers WITH (NOLOCK)
			WHERE DealerId = @BranchId
			)
	BEGIN
		SET @DealerDeals = 1
	END

	DECLARE @IsSearchText BIT = 0;

	IF (
			(
				@CustomerName IS NOT NULL
				OR @CustomerMobile IS NOT NULL
				OR @CustomerEmail IS NOT NULL
				OR @SearchText IS NOT NULL
				)
			)
	BEGIN
		SET @IsSearchText = 1
	END

	SET @FromFolloupdate = CONVERT(DATETIME, CONVERT(VARCHAR(10), @FromFolloupdate, 120) + ' 00:00:00')
	SET @ToFollowupdate = CONVERT(DATETIME, CONVERT(VARCHAR(10), @ToFollowupdate, 120) + ' 23:59:59')

	-- Lead Scheduling for verifications 
	-- EXECUTE TC_LeadVerificationScheduling @TC_Usersid = @UserId
	--	,@DealerId = @BranchId
	DECLARE @SearchCar VARCHAR(50) = NULL;
	DECLARE @SearchSource VARCHAR(50) = NULL;
	DECLARE @SearchUser VARCHAR(50) = NULL;
	DECLARE @SerchInquiryTypeId SMALLINT = NULL;
	DECLARE @LeadPriorityId SMALLINT = NULL;

	IF (
			@FromFolloupdate IS NOT NULL
			AND @ToFollowupdate IS NULL
			)
	BEGIN
		SET @FromFolloupdate = CONVERT(DATETIME, CONVERT(VARCHAR(10), @FromFolloupdate, 120) + ' 00:00:00')
			--SET @ToFollowupdate = convert(DATETIME, convert(VARCHAR(10), @FromFolloupdate, 120) + ' 23:59:59')
	END
	ELSE
		IF (
				@FromFolloupdate IS NULL
				AND @ToFollowupdate IS NOT NULL
				)
		BEGIN
			SET @FromFolloupdate = CONVERT(DATETIME, CONVERT(VARCHAR(10), @ToFollowupdate, 120) + ' 00:00:00')
				-- SET @ToFollowupdate = convert(DATETIME, convert(VARCHAR(10), @ToFollowupdate, 120) + ' 23:59:59')
		END

	IF (@FilterType = 2)
	BEGIN
		SET @SearchCar = @SearchText
	END;
	ELSE
		IF (@FilterType = 3)
		BEGIN
			SET @SearchSource = @SearchText
		END;
		ELSE
			IF (@FilterType = 4)
			BEGIN
				SET @SearchUser = @SearchText
				SET @UserId = NULL
			END;
			ELSE
				IF (@FilterType = 5)
				BEGIN
					IF @SearchText = 'Buyer'
						SET @SerchInquiryTypeId = 1
					ELSE
						IF @SearchText = 'Seller'
							SET @SerchInquiryTypeId = 2
						ELSE
							IF @SearchText = 'New Vehicle' --Modified By: Ashwini Dhamankar on Nov 8,2014. Changed New Car to New Vehicle
								SET @SerchInquiryTypeId = 3
				END;
				ELSE
					IF (@FilterType = 6)
					BEGIN
						-- i.e App Request
						--@FilterType = 6 App Filter  only
						IF (@SearchText IS NOT NULL)
						BEGIN
							IF @SearchText LIKE '%Fresh%'
								SET @LeadPriorityId = 1
							ELSE
								IF @SearchText LIKE '%Pending%'
									SET @LeadPriorityId = 2
								ELSE
									IF @SearchText LIKE '%Follow%'
										SET @LeadPriorityId = 3
						END

						SET @SearchCar = @CarName
						SET @SearchSource = @SourceName
					END;

	DECLARE @InqStatusId INT = NULL
	DECLARE @InqPriorityId INT = 0

	IF (@InqStatus = 'Booked')
	BEGIN
		SET @InqStatusId = 0
	END

	IF (
			@InqPriority <> 'Not Set'
			AND @InqPriority <> 'Booked'
			AND @InqPriority IS NOT NULL
			)
	BEGIN
		SELECT @InqPriorityId = TC_InquiryStatusId
		FROM TC_InquiryStatus WITH (NOLOCK)
		WHERE STATUS LIKE @InqPriority
	END
	ELSE
		IF (@InqPriority = 'Not Set')
		BEGIN
			SET @InqPriorityId = NULL
		END
		ELSE
			IF (@InqPriority = 'Booked')
			BEGIN
				SET @InqPriorityId = 4 -- to fetch booked inquiries
			END

	DECLARE @FromInqAddedDate DATETIME
	DECLARE @ToInqAddedDate DATETIME

	IF (@InqAddedDate IS NOT NULL)
	BEGIN
		SET @FromInqAddedDate = convert(DATETIME, convert(VARCHAR(10), GETDATE(), 120) + ' 00:00:00')
		SET @ToInqAddedDate = convert(DATETIME, convert(VARCHAR(10), GETDATE(), 120) + ' 23:59:59')
	END

	IF (@IsSearchText = 1)
	BEGIN
		SET @UserId = NULL
	END;

	-------------------------------------------------------------- Search Filter Paramteres Logic Ends Here -----------------------------------------------
	-------------------------------------------------------------- MyTask Current Bucket Data Logic -------------------------------------------------------
	-- Fetching All Data with Applied Filters and storing into a temp table
	SELECT CustomerId
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
	INTO #TempRawDataAllLeads
	FROM TC_TaskLists TTL WITH (NOLOCK)
	WHERE TTL.TC_LeadStageId <> 3
		AND (
			TTL.UserId = @UserId
			OR @UserId IS NULL
			)
		AND TTL.BranchId = @BranchId
		AND (
			(@ToFollowupdate IS NULL)
			OR (TTL.ScheduledON <= @ToFollowupdate)
			)
		AND (
			(@FromFolloupdate IS NULL)
			OR (TTL.ScheduledON >= @FromFolloupdate)
			)
		AND (
			TTL.TC_LeadDispositionID IS NULL
			OR TTL.TC_LeadDispositionID = 4
			OR TTL.TC_BusinessTypeId IN (
				4
				,6
				) --insurance and service,added by Nilima On 24 Aug. 
			)
		AND (
			(@CustomerName IS NULL)
			OR (TTL.CustomerName = @CustomerName)
			)
		AND (
			(@SearchCar IS NULL)
			OR (TTL.Car LIKE '%' + @SearchCar + '%')
			)
		AND (
			(@SearchSource IS NULL)
			OR (TTL.InquirySourceName = @SearchSource)
			)
		AND (
			(@SearchUser IS NULL)
			OR (TTL.AssignedTo = @SearchUser)
			)
		AND (
			(@CustomerMobile IS NULL)
			OR (TTL.CustomerMobile = @CustomerMobile)
			)
		AND (
			(@CustomerEmail IS NULL)
			OR (TTL.CustomerEmail = @CustomerEmail)
			)
		AND (
			(@SerchInquiryTypeId IS NULL)
			OR (TTL.TC_LeadInquiryTypeId = @SerchInquiryTypeId)
			)
		AND (
			(@LeadIds IS NULL)
			OR (
				TTL.TC_LeadId IN (
					SELECT listmember
					FROM [dbo].[fnSplitCSV](@LeadIds)
					)
				)
			)
		AND (
			(ISNULL(@InqPriorityId, TTL.TC_InquiryStatusId) IS NULL)
			OR (@InqPriorityId = 0)
			OR (
				TTL.TC_InquiryStatusId = @InqPriorityId
				AND @InqPriorityId <> 4
				)
			OR (
				@InqPriorityId = 4
				AND TTL.TC_LeadDispositionID = 4
				)
			)
		AND (
			@LeadPriorityId IS NULL
			OR (
				@LeadPriorityId = 1
				AND TTL.TC_LeadStageId = 1
				)
			OR (
				@LeadPriorityId = 2
				AND CONVERT(DATE, TTL.ScheduledOn) < CONVERT(DATE, GETDATE())
				)
			OR (
				@LeadPriorityId = 3
				AND CONVERT(DATE, TTL.ScheduledOn) = CONVERT(DATE, GETDATE())
				)
			)
		AND TTL.TC_BusinessTypeId = @BusinessTypeId --Added By Deepak on 14th July 2016
		AND (
			@LeadDispositionId IS NULL
			OR TTL.TC_LeadDispositionId =@LeadDispositionId
			)
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
	-- Get Current Bucket Data from Filtered 
	SELECT TOP (@ToIndex) CustomerId
		,CustomerName
		,Email
		,Mobile
		,TC_InquirySourceId
		,TC_LeadId
		,TC_InquiryStatusId
		,NextFollowUpDate
		,InterestedIn
		,CallType
		,LastCallComment
		,LatestInquiryDate
		,OrderDate
		,InquirySource
		,UserId
		,TC_LeadStageId
		,TC_NextActionId
		,InquiryType
		,InquiryTypeId
		,IsVerified
		,LeadCreationDate
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
		,TAL.TC_LeadDispositionId
		,TC_BusinessTypeId
		,RegistrationNumber
		,ROW_NUMBER() OVER (
			ORDER BY OrderDate DESC
			) NumberForPaging
	INTO #TempRawDataBucketLeads
	FROM #TempRawDataAllLeads TAL WITH (NOLOCK)
	--LEFT JOIN TC_LeadDisposition AS LD WITH (NOLOCK) ON TAL.TC_LeadDispositionId = LD.TC_LeadDispositionId
	--INNER JOIN TC_MasterLeadDisposition AS MLD WITH (NOLOCK) ON MLD.TC_MasterLeadDispositionId = LD.TC_MasterDispositionId
	WHERE (
			@LeadBucketId IN (
				1
				,17 -- modified by : Ashwini Dhamankar on June 30,2016
				,18
				,33 -- Insurance All leads,Added by : Nilima More on Aug 24,2016
				)
			OR TAL.BucketTypeId = @LeadBucketId
			)

	-- Filter data and applying pagination	
	SELECT *
	FROM #TempRawDataBucketLeads
	WHERE (
			(
				@FromIndex IS NULL
				AND @ToIndex IS NULL
				)
			OR (
				NumberForPaging BETWEEN @FromIndex
					AND @ToIndex
				)
			)
	ORDER BY NumberForPaging ASC

	-------------------------------------------------------------- MyTask Current Bucket's Data Logic Ends Here -----------------------------
	-------------------------------------------------------------- Bucket Count Logic -------------------------------------------------------
	-- Creating a temporary table to hold all bucket name with total counts
	SELECT TBL.TC_BucketLeadTypeId
		,TBL.BucketName
		,ISNULL(SUM(CASE 
					WHEN TTL.BucketTypeId = TBL.TC_BucketLeadTypeId
						THEN 1
					ELSE 0
					END), 0) AS TotalLead
	INTO #TempBucketData
	FROM TC_BucketLeadType AS TBL WITH (NOLOCK)
	LEFT JOIN #TempRawDataAllLeads TTL WITH (NOLOCK) ON TBL.TC_BucketLeadTypeId NOT IN (
			1
			,17
			,18
			,33 -- Insurance All leads,Added by : Nilima More on Aug 24,2016
			)
		AND TBL.IsActive = 1
	--LEFT JOIN TC_LeadDisposition AS LD WITH (NOLOCK) ON TTL.TC_LeadDispositionId = LD.TC_LeadDispositionId
	--INNER JOIN TC_MasterLeadDisposition AS MLD WITH (NOLOCK) ON MLD.TC_MasterLeadDispositionId = LD.TC_MasterDispositionId
	WHERE TBL.TC_BusinessTypeId = @BusinessTypeId
	GROUP BY TBL.TC_BucketLeadTypeId
		,TBL.BucketName

	-------------------------------------------------------------- Bucket Count Logic Ends Here -------------------------------------------------------
	DECLARE @AllLeadCount INT

	-- To Get Current Bucket Lead's Total Count (i.e. If 'NEW' tab is selected, then get total number of NEW lead counts)
	SELECT @AllLeadCount = COUNT(*)
	FROM #TempRawDataAllLeads TAL
	WHERE @LeadBucketId IN (
			1
			,17
			,18
			,33 -- Insurance All leads,Added by : Nilima More on Aug 24,2016
			)
		OR TAL.BucketTypeId = @LeadBucketId
		
	-------------------------------------------------------------- Bucket Tab Creation Logic -------------------------------------------------------
	CREATE TABLE #TempTabCounts (
		BucketName VARCHAR(30)
		,TC_BucketLeadTypeId SMALLINT
		)

	INSERT INTO #TempTabCounts
	SELECT 'ALL (' + CONVERT(VARCHAR(30), COUNT(TC_CallsId)) + ')'
		,CASE @BusinessTypeId
			WHEN 3
				THEN 1 -- All Sales
			WHEN 4
				THEN 17 -- All Service --modified by : Ashwini Dhamankar on June 30,2016 (17- all service leads)
			WHEN 5
				THEN 18 -- All Advantage
			WHEN 6
				THEN 33 -- Insurance All leads,Added by : Nilima More on Aug 24,2016
			END
	FROM #TempRawDataAllLeads AS TTL WITH (NOLOCK)

	INSERT INTO #TempTabCounts
	SELECT BucketName + ' (' + CONVERT(VARCHAR(30), TotalLead) + ')'
		,TC_BucketLeadTypeId
	FROM #TempBucketData AS TTL WITH (NOLOCK)
	WHERE TTL.TC_BucketLeadTypeId NOT IN (
			1
			,17
			,18
			,33 -- Insurance All leads,Added by : Nilima More on Aug 24,2016
			)

	SELECT @AllLeadCount AS RecordCount

	SELECT TBL.*
	FROM #TempTabCounts AS TBL
	INNER JOIN TC_BucketLeadType BL WITH (NOLOCK) ON BL.TC_BucketLeadTypeId = TBL.TC_BucketLeadTypeId
		AND BL.IsActive = 1
		AND BL.TC_BusinessTypeId = @BusinessTypeId --Added By Deepak on 14th July 2016
	ORDER BY BL.PriorityOrder

	--------------------------------------------------- Funnel Logic Starts Here ---------------------------------------------------------------------
	SELECT MLD.TC_MasterLeadDispositionId AS MasterLeadDispositionId
		,MLD.NAME AS FunnelName
		,ISNULL(COUNT(TL.TC_LeadId), 0) AS TotalCount
		,MLD.PriorityOrder
	INTO #TempFunnelData
	FROM TC_MasterLeadDisposition AS MLD WITH (NOLOCK)
	INNER JOIN TC_LeadDisposition AS LD WITH (NOLOCK) ON MLD.TC_MasterLeadDispositionId = LD.TC_MasterDispositionId
	LEFT JOIN #TempRawDataAllLeads AS TL WITH (NOLOCK) ON LD.TC_LeadDispositionId = TL.TC_LeadDispositionId
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
		,'ALL'
		,COUNT(TC_CallsId)
		,0
	FROM #TempRawDataAllLeads AS AL

	SELECT MasterLeadDispositionId
		,FunnelName
		,TotalCount
	FROM #TempFunnelData

	---------------------------------------------------------Funnel Logic Ends Here ---------------------------------------------------------------
	--Drop Temp Tables
	DROP TABLE #TempBucketData

	DROP TABLE #TempTabCounts

	DROP TABLE #TempRawDataAllLeads
END

