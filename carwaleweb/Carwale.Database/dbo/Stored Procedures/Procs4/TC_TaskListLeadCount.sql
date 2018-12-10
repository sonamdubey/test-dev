IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_TaskListLeadCount]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_TaskListLeadCount]
GO

	-- =============================================  
-- Author:  Vivek Gupta    
-- Create date:   10-09-2015
-- Description:    Task list load lead counts bucket wise
--[dbo].[TC_TaskListLeadCount] 5,243,nULL,NULL,NULL,NULL,NULL
-- Modified By : Ashwini Dhamankar on Nov 30,2015 (Fetched Booked Lead Count)
-- Modified By : Ashwini Dhamankar on Dec 9,2015 (modify count logic)
-- Modified By : Khushaboo Patil on 5th Oct 2016 created temp table then insert data 
-- =============================================     
CREATE PROCEDURE [dbo].[TC_TaskListLeadCount]
	-- Add the parameters for the stored procedure here     
	@BranchId INT
	,@UserId  INT

	,@CustomerName VARCHAR(100)
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
	,@CarName VARCHAR(100) = NULL
	,@SourceName VARCHAR(50) = NULL
AS
SET NOCOUNT ON --Added By Afrose on 1-09-2015
BEGIN
		DECLARE @ApplicationId AS TINYINT

		SET @ApplicationId = (
				SELECT ApplicationId
				FROM Dealers WITH (NOLOCK)
				WHERE ID = @BranchId
				)

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

		SET @FromFolloupdate = convert(DATETIME, convert(VARCHAR(10), @FromFolloupdate, 120) + ' 00:00:00')
		SET @ToFollowupdate = convert(DATETIME, convert(VARCHAR(10), @ToFollowupdate, 120) + ' 23:59:59')

		--  Lead Scheduling for verifications 
		EXECUTE TC_LeadVerificationScheduling @TC_Usersid = @UserId
			,@DealerId = @BranchId

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
			SET @FromFolloupdate = convert(DATETIME, convert(VARCHAR(10), @FromFolloupdate, 120) + ' 00:00:00')
			SET @ToFollowupdate = convert(DATETIME, convert(VARCHAR(10), @FromFolloupdate, 120) + ' 23:59:59')
		END
		ELSE
			IF (
					@FromFolloupdate IS NULL
					AND @ToFollowupdate IS NOT NULL
					)
			BEGIN
				SET @FromFolloupdate = convert(DATETIME, convert(VARCHAR(10), @ToFollowupdate, 120) + ' 00:00:00')
				SET @ToFollowupdate = convert(DATETIME, convert(VARCHAR(10), @ToFollowupdate, 120) + ' 23:59:59')
			END

		IF (@IsSearchText <> 1)
		BEGIN
			IF (
					@FromFolloupdate IS NULL
					AND @ToFollowupdate IS NULL
					)
			BEGIN
				--SET @ToFollowupdate = convert(DATETIME, convert(VARCHAR(10), GETDATE(), 120) + ' 23:59:59')
				SET @ToFollowupdate = NULL
			END
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

		-------------------------------------------------------------------------------------------------------
		-- Added By Vivek on 27th June,2013 Declared parameters for Advanced search
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

		--------------------------------------------------------------------------------------------------------- 
		IF (@IsSearchText = 1)
		BEGIN
			SET @UserId = NULL
		END;

		CREATE TABLE  #TempRawDataAllLeads(TC_LeadId INT,OrderDate DATETIME,BucketTypeId INT)


		INSERT INTO  #TempRawDataAllLeads(TC_LeadId ,OrderDate ,BucketTypeId)
		SELECT 
			TCIL.TC_LeadId
			,(
			CASE 
				WHEN LatestInquiryDate > ScheduledOn
					THEN LatestInquiryDate
				ELSE ScheduledOn
				END
			) AS OrderDate
			,CASE 	
			WHEN	DATEDIFF(day,CONVERT(DATE, ScheduledOn),CONVERT(DATE, GETDATE())) >59 
					AND ISNULL(TCIL.TC_LeadDispositionId,0) <> 4 
			THEN	9  -- 2 Months or Older		

			WHEN CONVERT(DATE, ScheduledOn) < CONVERT(DATE, GETDATE())
				AND ISNULL(TCIL.TC_LeadDispositionId,0) <> 4
				THEN 5 -- i.e. Pending Lead       
				                         
			WHEN CONVERT(DATE, TCIL.CreatedDate) = CONVERT(DATE, GETDATE()) AND TCAC.CallType = 1
				THEN 2 -- i.e. New Lead

			WHEN CONVERT(DATE, ScheduledOn) = CONVERT(DATE, GETDATE())
				AND TCAC.CallType <> 1
				--CONVERT(DATE, TCIL.CreatedDate) <> CONVERT(DATE, GETDATE())
				AND ISNULL(TC_NextActionId,0) <> 1
				--AND TC_NextActionId = 2 -- I.E. Phone Call
				THEN 3 -- i.e. Call Today Lead

			WHEN TC_NextActionId = 1 -- I.E. Personal Visit
				AND CONVERT(DATE, ScheduledOn) = CONVERT(DATE, GETDATE())
				THEN 4 -- i.e. Personal Visit Lead

			WHEN	(TCIL.TC_LeadDispositionId = 4)   --Added by Ashwini Dhamankar on Nov 30,2015
			THEN 6  --Booked Leads 
						--ELSE 1
				END AS BucketTypeId
		FROM TC_ActiveCalls AS TCAC WITH (NOLOCK)
		INNER JOIN TC_CustomerDetails AS C WITH (NOLOCK) ON TCAC.TC_LeadId = C.ActiveLeadId
		INNER JOIN TC_InquiriesLead AS TCIL WITH (NOLOCK) ON TCAC.TC_LeadId = TCIL.TC_LeadId
		INNER JOIN TC_Users AS TU WITH (NOLOCK) ON TCIL.TC_UserId = TU.Id
		INNER JOIN TC_InquirySource AS TS WITH (NOLOCK) ON C.TC_InquirySourceId = TS.Id
                                                
		WHERE		
			(
				(@ToFollowupdate IS NULL)
				OR (TCAC.ScheduledON <= @ToFollowupdate)
				)
			AND TCIL.TC_LeadStageId <> 3
			AND (
				TCAC.TC_UsersId = @UserId
				OR @UserId IS NULL
				)
			AND (
				TCIL.TC_UserId = @UserId
				OR @UserId IS NULL
				)
			AND TCIL.BranchId = @BranchId
			AND (
				(@FromFolloupdate IS NULL)
				OR (TCAC.ScheduledON >= @FromFolloupdate)
				)
			AND (
				TCIL.TC_LeadDispositionID IS NULL
				OR TCIL.TC_LeadDispositionID = 4
				) -- Modified By: Nilesh Utture on 24th Jan, 2013 9.0 pm 
			AND (
				(@CustomerName IS NULL)
				OR (C.CustomerName = @CustomerName)
				)
			AND (
				(@SearchCar IS NULL)
				OR (TCIL.CarDetails = @SearchCar)
				)
			AND (
				(@SearchSource IS NULL)
				OR (
					TS.Source = @SearchSource
					AND TS.IsActive = 1
					AND (
						(
							TS.IsVisibleCW = 1
							AND @ApplicationId = 1
							)
						OR (
							TS.IsVisibleBW = 1
							AND @ApplicationId = 2
							)
						)
					)
				) --Modified by Afrose, for Carwale user				
			AND (
				(@SearchUser IS NULL)
				OR (TU.UserName = @SearchUser)
				)
			AND (
				(@CustomerMobile IS NULL)
				OR (C.Mobile = @CustomerMobile)
				)
			AND (
				(@CustomerEmail IS NULL)
				OR (C.Email = @CustomerEmail)
				)
			AND (
				(@SerchInquiryTypeId IS NULL)
				OR (TCIL.TC_LeadInquiryTypeId = @SerchInquiryTypeId)
				)
	
			AND (
				(@LeadIds IS NULL)
				OR (
					TCIL.TC_LeadId IN (
						SELECT listmember
						FROM [dbo].[fnSplitCSV](@LeadIds)
						)
					)
				)
			AND (
				(ISNULL(@InqPriorityId, TCIL.TC_InquiryStatusId) IS NULL)
				OR (@InqPriorityId = 0)
				OR (
					TCIL.TC_InquiryStatusId = @InqPriorityId
					AND @InqPriorityId <> 4
					)
				OR (
					@InqPriorityId = 4
					AND TCIL.TC_LeadDispositionID = 4
					)
				)
			AND (
				@LeadPriorityId IS NULL
				OR (
					@LeadPriorityId = 1
					AND TCIL.TC_LeadStageId = 1
					)
				OR (
					@LeadPriorityId = 2
					AND CONVERT(DATE, TCAC.ScheduledOn) < CONVERT(DATE, GETDATE())
					)
				OR (
					@LeadPriorityId = 3
					AND CONVERT(DATE, TCAC.ScheduledOn) = CONVERT(DATE, GETDATE())
					)
				)
			--------------------------------------------------------------------------------------------------------------------------
			;

		CREATE TABLE #TempAllUniqueData (TC_LeadId INT ,OrderDate DATETIME,BucketTypeId INT ,RowNumber INT)
		INSERT INTO  #TempAllUniqueData (TC_LeadId  ,OrderDate ,BucketTypeId ,RowNumber )
		SELECT T.TC_LeadId,T.OrderDate,T.BucketTypeId
				,CASE	WHEN T.BucketTypeId = 6 
						THEN 1
				ELSE
				CASE	WHEN 
						T.BucketTypeId <> 6 AND (SELECT ISNULL(BucketTypeId,0) FROM #TempRawDataAllLeads WHERE TC_LeadId = T.TC_LeadId AND BucketTypeId = 6) = 6
						THEN 0
				ELSE
						ROW_NUMBER() OVER (
						---line commented and chages made by manish on 16-07-2013
						PARTITION BY TC_LeadId ORDER BY OrderDate DESC -- ORDER BY  NextFollowUpDate DESC, LatestInquiryDate DESC
						) 
				END 
				END
				RowNumber
		FROM #TempRawDataAllLeads T

		CREATE TABLE #TempTabCounts (
		BucketName VARCHAR(30),
		LeadCount INT
		,PriorityOrder TINYINT
		)
		
		DECLARE @CurrentBucketId SMALLINT = 0
		DECLARE @CurrentPriorityOrder TINYINT = 0

	-- Iterate over all users
		WHILE (1 = 1)
		BEGIN
			-- Get next UserId
			SELECT TOP 1 @CurrentBucketId = TC_BucketLeadTypeId,@CurrentPriorityOrder = PriorityOrder
			FROM TC_BucketLeadType WITH (NOLOCK)
			WHERE TC_BucketLeadTypeId > @CurrentBucketId
				AND TC_BucketLeadTypeId <> 1
				AND TC_BucketLeadTypeId IN (
					2
					,3
					,4
					,5
					,6
					,9 
					)
			ORDER BY TC_BucketLeadTypeId

			-- Exit loop if no more Users
			IF @@ROWCOUNT = 0
				BREAK;

			DECLARE @CurrentBucketName VARCHAR(30)

			SELECT @CurrentBucketName = BucketName
			FROM TC_BucketLeadType WITH (NOLOCK)
			WHERE TC_BucketLeadTypeId = @CurrentBucketId

			DECLARE @CurrentBucketCount VARCHAR(10)

			SELECT @CurrentBucketCount = count(BucketTypeId)
			FROM #TempAllUniqueData
			WHERE BucketTypeId = @CurrentBucketId
				AND RowNumber = 1

			-- do your insert operation
			INSERT INTO #TempTabCounts
			VALUES (
				@CurrentBucketName,@CurrentBucketCount,@CurrentPriorityOrder
				)
		END

		--commented by Ashwini Dhanmankar on Dec 9,2015
		--SELECT BT.BucketName, COUNT(DISTINCT Temp.TC_LeadId) LeadCount, BT.PriorityOrder
		--INTO #TempLeadCount
		--FROM 
		--TC_BucketLeadType BT WITH(NOLOCK) 
		--LEFT JOIN  #TempRawDataAllLeads Temp		    
		--ON Temp.BucketTypeId = BT.TC_BucketLeadTypeId		
		--GROUP BY BT.BucketName,  BT.PriorityOrder

		DELETE FROM #TempTabCounts WHERE BucketName = 'ALL LEADS'
		
		INSERT INTO #TempTabCounts
		SELECT 'ALL LEADS', SUM(LeadCount),9
		FROM
		#TempTabCounts
		
		
		DELETE FROM #TempTabCounts WHERE BucketName = 'CLOSED LEADS'

		INSERT INTO #TempTabCounts
		SELECT 	'CLOSED LEADS', COUNT(TC_LeadId),7
		FROM TC_Lead WITH(NOLOCK)
		WHERE BranchId = @BranchId
		AND TC_LeadStageId = 3	

		----Added by : Ashwini Dhamankar on Dec 8,2015
		--DELETE FROM #TempLeadCount WHERE BucketName = 'UNASSIGNED LEADS'

		--INSERT INTO #TempLeadCount
		--SELECT 	'UNASSIGNED LEADS', COUNT(TC_LeadId),10
		--FROM TC_InquiriesLead WITH(NOLOCK)
		--WHERE BranchId = @BranchId
		--AND (TC_LeadStageId = 1 OR TC_LeadStageId IS NULL) AND TC_UserId IS NULL

		 
		--DELETE FROM #TempLeadCount WHERE BucketName = 'BOOKED LEADS'

		--INSERT INTO #TempLeadCount
		--SELECT 	'BOOKED LEADS', COUNT(TC_LeadId),7
		--FROM TC_InquiriesLead WITH(NOLOCK)
		--WHERE BranchId = @BranchId
		--AND TC_LeadDispositionID = 4

		
		DELETE FROM #TempTabCounts WHERE BucketName = 'INVALID LEADS'

		INSERT INTO #TempTabCounts
		SELECT 	'INVALID LEADS', COUNT(TC_LeadId),8
		FROM TC_InquiriesLead WITH(NOLOCK)
		WHERE BranchId = @BranchId
		AND TC_LeadDispositionID IN (1,74,70)


		SELECT * FROM #TempTabCounts ORDER BY PriorityOrder

		DROP TABLE #TempRawDataAllLeads
		DROP TABLE #TempTabCounts
END

