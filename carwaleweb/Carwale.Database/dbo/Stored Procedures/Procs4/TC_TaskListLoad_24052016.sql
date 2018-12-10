IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_TaskListLoad_24052016]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_TaskListLoad_24052016]
GO

	



-- =============================================  
-- Modified By: Nilesh Utture on 24th Jan, 2013 9.0 pm Added extra condition "OR TCIL.TC_LeadDispositionID=4" in SELECT Query
--- Author:  Manish    
-- Create date:   10-01-2013  
-- Description:    
--execute [TC_TaskListLoad] 5,1,1,10,null,1212,null,NULL,NULL,NULL
--Modify by Surendra  on 27-02-2013  changing latestinquiry date
--Modify by Surendra  on 22-03-2013  adding condition of userid in inquiries lead table
--Modify by Tejashree Patil on 23 April 2013 fetched sourceId,TC_LeadStageId.
--Modify by Surendra on 26 April 2013 include inq source in select list
--Modified By Vivek Gupta on 8th May,2013 Added a parameter @SearchText AND @searchType coz more filtered data is needed,
--                    declared @SearchCar,@SearchSource,@SearchUser,@ChangedUserId.
--Modified By:Vivek Gupta on 15th,May,2013..Removed @ToFollowupdate condition for getting Current date in case of Null.
--Modified By Vivek Gupta on 30th MAy,2013..Added condition for preventing auto date assignment
--Modified By Manish Chourasiya on 31-05-2013 For correcting from date and to date filter and Ordering of the leads
--Modified By Umesh on 21-06-2013 For Change like condition to Equal to in where clause for Car,Source,User
--Modified By Manish on 16-07-2013 for changing diplay order in my task page.
--Added By Vivek on 21-08-2013,Added parameters for Advanced search
--Added By Vivek on 21-08-2013 Declared parameters for Advanced search
--Added By Vivek on 21-08-2013 Used conditions for new parameters for Advanced search
--Modified by Manish on 26-08-2013 advance search conditions commented in filter criteria because it is not using from Front end 
--Modified by: Tejashree on 26-08-2013, Fetched complete name of a customer.
--Modified by: Tejashree on 26-08-2013, Fetched UniqueCustomerId of a customer.
--Modified By: Nilesh Utture on 18-09-2013, Added column TC_NextActionId for use in API
--modified by vivek gupta on 1st nov , 2013, aded condition to get all data if @fromindex and @toindex are null
--modified by vivek gupta on 02-07-2014 aded @filterType = 5 for inquirytype search
--Modified By: Ashwini Dhamankar on Nov 8,2014. Changed New Car to New Vehicle
--Modified By: Vivek Gupta, added IsVerified Field from customer table to know if the customer is verified or not
--Modified By Vivek Gupta on 13-03-2015, uncommented Hot Warm Normal Search filter
--Modified By Vivek Gupta on 24-03-2015, added filter type 6 to get fres pending or followup leads
--Modified By Vivek Gupta on 09-04-2015, fetched booked leads by filter
-- Modified By : Suresh Prajapati on 18th Aug, 2015
-- Description : Added more filters for App request 
--@FilterType = 6 App Filter  only
--Modified By Vivek Gupta on 11-09-2015, stopped showing future leads, and changed logic of showing new lead, calltype=1 only are new leads now
--Modified By Khushaboo Patil on 5-10-2015 added order by for BucketLeadType
--Modified By : Ashwini Dhamankar on Nov 24,2015 (Fetched New Car Booked Leads)
--Modified By : Ruchira Patil on Nov 24,2015 (Fetched No of inquiries and no of follow up for the leads)
--Modified By : Vicky Gupta on 25/11/2015 (Added a new bucket for 2 months or older inquiries)
--Modified by : Ashwini Dhamankar on Dec 9,2015 (modified partitioning logic)\
--Modified By : Vivek Gupta on 22-12-2015, extracted exchange car details
--Modified By : Ashwini Dhamankar on March 8,2016 Fetched InquiryTypeId 
--Modified by : Kritika Choudhary on 14th March 2016, added select query for LeadDisposition name
--Modified By : Nilima More On 14th 2016(InqSourceId not in CarWale Advantage Masking Number, CarWale Advantage online,CarWale Advantage offline)
--EXECUTE [TC_TaskListLoad] 5,243,1,1000,null,null,null,null,null,null,1,null,null,null,null,10,null,null

--EXECUTE [TC_TaskListLoad] 5,243,1,1000,'BHarath','8454045742','dealerclassifieds@carwale.com',null,null,'Buyer',5,null,null,null,null,1,null,null
--Modified by : Kritika Choudhary on 15th April 2016, added eagerness,car and location in select query also added join with TC_InquiryStatus
--Modified by : Kritika Choudhary on 2nd May 2016, added leadage
--Modified By : Khushaboo Patil on 12/05/2016 show future scheduled leads in All leads tab and last two months leads when date range is null
--Modified: (CONVERT(DATE, TCIL.CreatedDate) <> CONVERT(DATE, GETDATE()) OR ISNULL(TCAC.CallType,0) <> 1) -- Added By Deepak on 19th May 2016
-- Modified by Manish on 24-05-2016 reverted changes.
-- =============================================     
CREATE PROCEDURE [dbo].[TC_TaskListLoad_24052016]
	-- Add the parameters for the stored procedure here     
	@BranchId INT
	,@UserId  INT
	,@FromIndex INT=NULL
	,@ToIndex INT=NULL
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
AS
SET NOCOUNT ON --Added By Afrose on 1-09-2015
BEGIN
	DECLARE @ApplicationId AS TINYINT
	DECLARE @DealerDeals BIT = 0

	IF EXISTS  (SELECT DealerId FROM TC_Deals_Dealers WITH(NOLOCK) WHERE DealerId = @BranchId)
	BEGIN
		SET @DealerDeals = 1
	END

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
		--SET @ToFollowupdate = convert(DATETIME, convert(VARCHAR(10), @FromFolloupdate, 120) + ' 23:59:59')
	END
	ELSE
		IF (
				@FromFolloupdate IS NULL
				AND @ToFollowupdate IS NOT NULL
				)
		BEGIN
			SET @FromFolloupdate = convert(DATETIME, convert(VARCHAR(10), @ToFollowupdate, 120) + ' 00:00:00')
			-- SET @ToFollowupdate = convert(DATETIME, convert(VARCHAR(10), @ToFollowupdate, 120) + ' 23:59:59')
		END

	IF (@IsSearchText <> 1)
	BEGIN
		IF (
				@FromFolloupdate IS NULL
				AND @ToFollowupdate IS NULL
				)
		BEGIN
			-- SET @ToFollowupdate = convert(DATETIME, convert(VARCHAR(10), GETDATE(), 120) + ' 23:59:59')
			--SET @ToFollowupdate = NULL 
			print(1);
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
	-------------------------------------------------------------------------------------------------------------
	--Added by ruchira patil on 24th nov 2015
	SELECT SUM(DISTINCT ID) NoInq,1 NoFollowUp,IC.TC_LeadId LeadId
	INTO #TempTbl1
	FROM
	(
		SELECT  count(B.TC_BuyerInquiriesId) ID ,L.TC_LeadId
			FROM        TC_InquiriesLead L WITH (NOLOCK)
			INNER JOIN  TC_BuyerInquiries B WITH (NOLOCK)  ON L.TC_InquiriesLeadId=B.TC_InquiriesLeadId 
			INNER JOIN  TC_Stock          S WITH (NOLOCK)  ON B.StockId = S.Id  
			INNER JOIN  vwAllMMV          V WITH (NOLOCK)  ON V.VersionId = S.VersionId 
			WHERE L.BranchId=@BranchId 
			AND  (
				(@LeadIds IS NULL)
				OR (
					L.TC_LeadId IN (
						SELECT listmember
						FROM [dbo].[fnSplitCSV](@LeadIds)
						)
					)
				) 
			AND V.ApplicationId = 1
			group by L.TC_LeadId
		UNION ALL  
			SELECT count(B.TC_BuyerInquiriesId) ID,L.TC_LeadId
			FROM       TC_InquiriesLead   L WITH (NOLOCK)  
			INNER JOIN TC_BuyerInquiries   B WITH (NOLOCK)  ON L.TC_InquiriesLeadId=B.TC_InquiriesLeadId   
			WHERE L.BranchId=@BranchId 
			AND B.StockId IS NULL  
			AND  (
				(@LeadIds IS NULL)
				OR (
					L.TC_LeadId IN (
						SELECT listmember
						FROM [dbo].[fnSplitCSV](@LeadIds)
						)
					)
				) 
			group by L.TC_LeadId
		UNION ALL   
			SELECT count(SL.TC_SellerInquiriesId) ID,L.TC_LeadId
		    	FROM            TC_InquiriesLead  L WITH (NOLOCK)  
				INNER JOIN      TC_SellerInquiries SL WITH (NOLOCK) ON L.TC_InquiriesLeadId=SL.TC_InquiriesLeadId   
				INNER JOIN      vwAllMMV           V  WITH (NOLOCK) ON SL.CarVersionId = V.VersionId
				LEFT OUTER JOIN TC_Stock           ST WITH (NOLOCK) ON ST.TC_SellerInquiriesId = SL.TC_SellerInquiriesId  
			WHERE  L.BranchId=@BranchId 
				AND  (
				(@LeadIds IS NULL)
				OR (
					L.TC_LeadId IN (
						SELECT listmember
						FROM [dbo].[fnSplitCSV](@LeadIds)
						)
					)
				) 
				AND V.ApplicationId = 1
			group by L.TC_LeadId
		UNION ALL   
			SELECT  count(N.TC_NewCarInquiriesId) ID,L.TC_LeadId
			FROM            TC_NewCarInquiries N   WITH (NOLOCK) 
			INNER JOIN      TC_InquiriesLead  L      WITH (NOLOCK) ON L.TC_InquiriesLeadId =N.TC_InquiriesLeadId 
			LEFT JOIN      vwAllMMV           V		 WITH (NOLOCK) ON N.VersionId = V.VersionId
			WHERE L.BranchId=@BranchId   
				AND  (
				(@LeadIds IS NULL)
				OR (
					L.TC_LeadId IN (
						SELECT listmember
						FROM [dbo].[fnSplitCSV](@LeadIds)
						)
					)
				) 
				 AND ISNULL(V.ApplicationId,1) = 1
					group by L.TC_LeadId
					) 
	AS IC
	--LEFT JOIN TC_Calls TC WITH (NOLOCK) ON TC.TC_LeadId = IC.TC_LeadId AND TC.IsActionTaken=1
	GROUP BY IC.TC_LeadId
	-------------------------------------------------------------------------------------------------------------

	SELECT C.id AS [CustomerId]
		,(ISNULL(C.Salutation, '') + ' ' + C.CustomerName + ' ' + ISNULL(C.LastName, '')) AS [CustomerName]
		--Modified by: Tejashree on 26-08-2013 
		,C.Email
		,C.Mobile
		,C.TC_InquirySourceId
		,tcac.TC_LeadId
		,TCIL.TC_InquiryStatusId
		,ScheduledOn AS [NextFollowUpDate]
		,TCIL.CarDetails AS [InterestedIn]
		,TCAC.CallType
		,TCAC.LastCallComment
		,LatestInquiryDate
		,(
			CASE 
				WHEN LatestInquiryDate > ScheduledOn
					THEN LatestInquiryDate
				ELSE ScheduledOn
				END
			) AS OrderDate
		,TS.Source AS InquirySource
		,TCIL.TC_UserId AS UserId
		,TCIL.TC_LeadStageId
		,TCIL.TC_LeadDispositionID
		,UniqueCustomerId
		,--column added  by Tejashree on 26-08-2013
		TCAC.TC_NextActionId
		,--column added  by Nilesh on 18-09-2013
		CASE TCIL.TC_LeadInquiryTypeId
			WHEN 1
				THEN 'Used Buy'
			WHEN 2
				THEN 'Used Sell'
			WHEN 3
				THEN 'New Buy'
			END AS InquiryType
		,TCIL.TC_LeadInquiryTypeId AS InquiryTypeId
		,C.IsVerified
		,TCIL.CreatedDate AS LeadCreationDate
		,TCAC.ScheduledOn
		,CASE 
			WHEN	DATEDIFF(day,CONVERT(DATE, ScheduledOn),CONVERT(DATE, GETDATE())) >59 
					AND ISNULL(TCIL.TC_LeadDispositionId,0) <> 4 
					AND TCIL.InqSourceId NOT IN (140,134,146,147,148) --Modified By: Nilima More On 14th 2016(InqSourceId not in CarWale Advantage Masking Number, CarWale Advantage online,CarWale Advantage offline)
			THEN	9  -- 2 Months or Older

			WHEN	CONVERT(DATE, ScheduledOn) < CONVERT(DATE, GETDATE()) 
					AND ISNULL(TCIL.TC_LeadDispositionId,0) <> 4
					AND TCIL.InqSourceId NOT IN (140,134,146,147,148)
			THEN 5 -- i.e. Pending Lead  
			                         
			WHEN	CONVERT(DATE, TCIL.CreatedDate) = CONVERT(DATE, GETDATE()) 
					AND TCAC.CallType = 1 AND TCIL.InqSourceId NOT IN (134,140,146,147,148) 
			THEN 2 -- i.e. New Lead

			WHEN	CONVERT(DATE, ScheduledOn) = CONVERT(DATE, GETDATE())
					AND --ISNULL(TCAC.CallType,0) <> 1 -- Commented on 19th May 2016 By Deepak
					(CONVERT(DATE, TCIL.CreatedDate) <> CONVERT(DATE, GETDATE()) OR ISNULL(TCAC.CallType,0) <> 1) -- Added By Deepak on 19th May 2016
					AND ISNULL(TC_NextActionId,0) <> 1
					AND TCIL.InqSourceId NOT IN (140,134,146,147,148) 
					--AND TC_NextActionId = 2 -- I.E. Phone Call
			THEN 3 -- i.e. Call Today Lead

			WHEN	TC_NextActionId = 1 -- I.E. Personal Visit
					AND CONVERT(DATE, ScheduledOn) = CONVERT(DATE, GETDATE())
					AND TCIL.TC_LeadStageId <> 3
					AND TCIL.InqSourceId NOT IN (140,134,146,147,148) 
					THEN 4 -- i.e. Personal Visit Lead
					--ELSE 1
			WHEN	(TCIL.TC_LeadDispositionId = 4 AND TCIL.InqSourceId NOT IN (140,134,146,147,148))    --added by Ashwini Dhamankar
			THEN 6  --Booked Leads 

			WHEN	(TCIL.InqSourceId IN (140,134,146,147,148))    
			THEN 10  -- Advantage Leads 

			END AS BucketTypeId
			,ISNULL(T.NoInq,0) NoInq --Column Added By Ruchira Patil on 24Th Nov 2015
			,ISNULL(T.NoFollowUp,0) NoFollowUp --Column Added By Ruchira Patil on 24Th Nov 2015
			,CASE 
			  WHEN TCIL.TC_LeadInquiryTypeId =  3 AND @ApplicationId = 1 THEN (SELECT 'Exchange: ' + ISNULL(VW.Car,'NA') + ' , ' + CONVERT(VARCHAR,ISNULL(EXNC.ExpectedPrice,0)) FROM TC_ExchangeNewCar EXNC WITH(NOLOCK) JOIN vwMMV VW WITH(NOLOCK) ON VW.VersionId = EXNC.CarVersionId WHERE EXNC.TC_NewCarInquiriesId IN (SELECT TOP 1 TC_NewCarInquiriesId FROM TC_NewCarInquiries NCI WITH(NOLOCK) WHERE NCI.TC_InquiriesLeadId = TCIL.TC_InquiriesLeadId AND ISNULL(NCI.TC_NewCarExchangeId,0) <> 0))
			  WHEN TCIL.TC_LeadInquiryTypeId <>  3 OR @ApplicationId = 2 THEN ''		 
			END AS ExchangeCar
			,S.Status AS Eagerness --Added by: Kritika Choudhary on 15th April 2016
			,C.Location AS Location --Added by: Kritika Choudhary on 15th April 2016
			,TCIL.CarDetails AS Car --Added by: Kritika Choudhary on 15th April 2016
	        ,CASE WHEN TCIL.TC_LeadDispositionId IN(4,32) THEN '' ELSE (CONVERT(VARCHAR, DATEDIFF(day,TCIL.CreatedDate,GETDATE())) + ' Days') END AS LeadAge
	INTO #TempRawDataAllLeads
	FROM TC_ActiveCalls AS TCAC WITH (NOLOCK)
	INNER JOIN TC_CustomerDetails AS C WITH (NOLOCK) ON TCAC.TC_LeadId = C.ActiveLeadId
	INNER JOIN TC_InquiriesLead AS TCIL WITH (NOLOCK) ON TCAC.TC_LeadId = TCIL.TC_LeadId
	INNER JOIN TC_Users AS TU WITH (NOLOCK) ON TCIL.TC_UserId = TU.Id
	INNER JOIN TC_InquirySource AS TS WITH (NOLOCK) ON C.TC_InquirySourceId = TS.Id
	LEFT JOIN TC_InquiryStatus AS S WITH (NOLOCK) ON TCIL.TC_InquiryStatusId = S.TC_InquiryStatusId --Added by: Kritika Choudhary on 15th April 2016, for inquiry status
	LEFT JOIN #TempTbl1 T ON T.LeadId = TCIL.TC_LeadId
	--  INNER JOIN TC_Lead AS TL WITH (NOLOCK) ON TL.TC_LeadId = TCAC.TC_LeadId                                                   
	WHERE
		--TCAC.ScheduledON BETWEEN  @FromFolloupdate AND @ToFollowupdate 
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

	--WITH Cte2
	--AS (
	--commented by Ashwini Dhamankar on Dec 9,2015
			--SELECT *
			--	,ROW_NUMBER() OVER (
			--		---line commented and chages made by manish on 16-07-2013
			--		PARTITION BY TC_LeadId ORDER BY OrderDate DESC -- ORDER BY  NextFollowUpDate DESC, LatestInquiryDate DESC
			--		) RowNumber
			--INTO #TempAllUniqueData
			--FROM #TempRawDataAllLeads

	--)
	--WHERE  RowNum = 1

	--Added by Ashwini Dhamankar on Dec 9,2015 (If there are two inquiries of same leadId one is in pending and other is in booking state then consider only booked one)
		SELECT *
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
		INTO #TempAllUniqueData
		FROM #TempRawDataAllLeads T

--Added by : Kritika Choudhary on 14th March 2016, for LeadDisposition name
		SELECT  ROW_NUMBER() OVER (PARTITION BY T.TC_LeadId ORDER BY EventCreatedOn DESC) Rows, 
				D.Name DispositionName,D.TC_LeadDispositionId LeadDispositionId ,T.*
		INTO	#TempLeadWithDisposition
		FROM	#TempAllUniqueData T  WITH (NOLOCK)
				LEFT JOIN TC_DispositionLog DL  WITH (NOLOCK) ON DL.TC_LeadId = T.TC_LeadId
				LEFT JOIN TC_LeadDisposition D  WITH (NOLOCK) ON D.TC_LeadDispositionId = DL.TC_LeadDispositionId
		ORDER BY T.TC_LeadId


	SELECT *
		,ROW_NUMBER() OVER (
			ORDER BY OrderDate DESC
			) NumberForPaging
	INTO #TblTemp --CURRENT TAB DATA
	FROM #TempLeadWithDisposition TL--All unique data
	WHERE TL.RowNumber = 1 AND TL.Rows = 1 --Added: rows=1 by Kritika Choudhary on 14th March 2016
		AND (
			(@LeadBucketId = 1 
				/*AND -- Added By : Khushaboo Patil on 12/05/2016 if fromfollowudate is null fetch only last two months leads
				( 
					CONVERT(DATE, ScheduledOn) >= CASE WHEN @FromFolloupdate IS NULL THEN CONVERT(DATE, GETDATE()-60)
												  ELSE CONVERT(DATE, ScheduledOn) END
				)*/
			)
			OR BucketTypeId = @LeadBucketId
			)

	DECLARE @RecordCount INT

	-- To get current selected tab's count
	SELECT @RecordCount = COUNT(*)
	FROM #TempLeadWithDisposition TL
	WHERE TL.RowNumber = 1 AND TL.Rows = 1 --Added: rows=1 by Kritika Choudhary on 14th March 2016
	     AND (
			(@LeadBucketId = 1 
				AND 
				( 
					CONVERT(DATE, ScheduledOn) >= CASE WHEN @FromFolloupdate IS NULL THEN CONVERT(DATE, GETDATE()-60)
												  ELSE CONVERT(DATE, ScheduledOn) END
				)
			)
			OR 
				BucketTypeId = @LeadBucketId
			)

	SELECT *
	FROM #TblTemp
	WHERE (
			@FromIndex IS NULL
			AND @ToIndex IS NULL
			) --modified by vivek gupta on 1st nov , 2013
		--this from index and toindex would be null when request comes from wsapis and fromdate and todate would not be null.
		--this change has been done to return all followups from currentdate to next 60 days.
		OR (
			NumberForPaging BETWEEN @FromIndex
				AND @ToIndex
			)

	--ORDER BY LatestInquiryDate DESC,NextFollowUpDate DESC			  
	SELECT @RecordCount AS RecordCount
		-- Modified By : Khushaboo Patil on 12/05/2016 if fromfollowudate is null fetch only last two months leads
		--,COUNT(*) AS AllLeadsCount
		,CASE WHEN @FromFolloupdate IS NOT NULL THEN COUNT(*) ELSE		
		SUM(CASE WHEN CONVERT(DATE, ScheduledOn) >= CONVERT(DATE, GETDATE()-60) THEN 1 ELSE 0 END)	
		END AS AllLeadsCount
		,SUM(CASE 
				WHEN BucketTypeId = 2
					THEN 1
				ELSE 0
				END) AS TotalNewLeads
		,SUM(CASE 
				WHEN BucketTypeId = 3
					THEN 1
				ELSE 0
				END) AS TotalCallTodayLeads
		,SUM(CASE 
				WHEN BucketTypeId = 4
					THEN 1
				ELSE 0
				END) AS TotalPersonalVisitLeads
		,SUM(CASE 
				WHEN BucketTypeId = 5
					THEN 1
				ELSE 0
				END) AS TotalPendingLeads
		,SUM(CASE 
				WHEN BucketTypeId = 6
					THEN 1
				ELSE 0
				END) AS TotalBookedLeads    --added by Ashwini Dhamankar
		,SUM(CASE 
				WHEN BucketTypeId = 9
					THEN 1
				ELSE 0
				END) AS TwoMonthsOlder 
		,SUM(CASE 
				WHEN BucketTypeId = 10 AND @DealerDeals = 1 -- added by vivek gupta
					THEN 1
				ELSE 0
				END) AS DealLeads 

	FROM #TempLeadWithDisposition AS TT1
	WHERE TT1.RowNumber = 1 AND TT1.Rows = 1 --Added: rows=1 by Kritika Choudhary on 14th March 2016

	
	CREATE TABLE #TempTabCounts (
		BucketName VARCHAR(30)
		,TC_BucketLeadTypeId SMALLINT
		)

	INSERT INTO #TempTabCounts
	SELECT 
	-- Modified By : Khushaboo Patil on 12/05/2016 if fromfollowudate is null fetch only last two months leads
	CASE WHEN @FromFolloupdate IS NOT NULL THEN  'ALL (' + CONVERT(VARCHAR(30), COUNT(*)) + ')' ELSE
	'ALL (' + CONVERT(VARCHAR(30),SUM(CASE WHEN CONVERT(DATE, ScheduledOn) >= CONVERT(DATE, GETDATE()-60) THEN 1 ELSE 0 END)) + ')'
	END
		,1
	FROM #TempLeadWithDisposition TL
	WHERE TL.RowNumber = 1 AND TL.Rows = 1 --Added: rows=1 by Kritika Choudhary on 14th March 2016

	DECLARE @CurrentBucketId SMALLINT = 0

	-- Iterate over all users
	WHILE (1 = 1)
	BEGIN
		-- Get next UserId
		SELECT TOP 1 @CurrentBucketId = TC_BucketLeadTypeId
		FROM TC_BucketLeadType WITH (NOLOCK)
		WHERE TC_BucketLeadTypeId > @CurrentBucketId
			AND TC_BucketLeadTypeId <> 1
			AND (
			      (TC_BucketLeadTypeId IN (2,3,4,5,6,9)	AND @DealerDeals = 0)
			  OR
			      (TC_BucketLeadTypeId IN (2,3,4,5,6,9,10) AND @DealerDeals = 1) -- bucket type 10 not to be shown to the dealers who does not have deal permision
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
		FROM #TempLeadWithDisposition TL
		WHERE BucketTypeId = @CurrentBucketId
			AND TL.RowNumber = 1 AND TL.Rows=1 --Added: rows=1 by Kritika Choudhary on 14th March 2016

		-- do your insert operation
		INSERT INTO #TempTabCounts
		VALUES (
			@CurrentBucketName + ' (' + @CurrentBucketCount + ')'
			,@CurrentBucketId
			)
	END

	--Modified By Khushaboo Patil on 5-10-2015 added order by for BucketLeadType
	SELECT TBL.*
	FROM #TempTabCounts TBL
	INNER JOIN TC_BucketLeadType BL WITH(NOLOCK) ON BL.TC_BucketLeadTypeId = TBL.TC_BucketLeadTypeId AND BL.IsActive = 1
	ORDER BY BL.PriorityOrder 

	DROP TABLE #TblTemp

	DROP TABLE #TempAllUniqueData

	DROP TABLE #TempTabCounts

	DROP TABLE #TempRawDataAllLeads
	DROP TABLE #TempTbl1
	DROP TABLE #TempLeadWithDisposition
END
	--------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------

---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------


/****** Object:  StoredProcedure [dbo].[TC_GetClosedLeadDetails]    Script Date: 12/9/2015 7:02:21 PM ******/
SET ANSI_NULLS ON



