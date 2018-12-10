IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_TaskListSendToExcel]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_TaskListSendToExcel]
GO

	-- Author		:	Surendra
-- Create date	:	04-02-2013  
-- Description	:	Send to excel funtionality in task list page    
-- execute [TC_TaskListSendToExcel] 968,106,null,null,null,'2013-01-31 00:00:00.000','2013-02-04 00:00:00.000' 
-- Modified By : Vivek Gupta on 8th May,2013 Added a parameter @SearchText
-- Modified By: Vivek Gupta on 5th july,2013 , Added Parametes for Advanced search options.
-- Modified By Vivek Gupta on 21-02-2014, Added source in select query
-- Modified by vivek gupta on 24-07-2014 aded @filterType = 5 for inquirytype search
--Modified By: Ashwini Dhamankar on Nov 8,2014. Changed New Car to New Vehicle
--Modified By Vivek Gupta on 24-03-2015, added filter type 6 to get fres pending or followup leads
--Modified By Vicky Gupta on 03-08-2015, Fetched Make,Model,Version
--Modified By Vicky Gupta :24/11/2015, send city of customer from newCarInquiry table, if city not exist ther, then take from TC_CustomerDetails
-- 5,243,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,1,NULL,NULL,NULL,NULL,1
-- exec TC_TaskListSendToExcel 5,243,NULL,NULL,NULL,NULL,NULL,NULL,1,NULL,NULL,NULL,null,3

-- =============================================================================================================     
CREATE PROCEDURE [dbo].[TC_TaskListSendToExcel]
	-- Add the parameters for the stored procedure here     
	@BranchId BIGINT
	,@UserId BIGINT
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
	@InqStatus VARCHAR(10)
	,@InqPriority VARCHAR(10)
	,@InqAddedDate AS DATETIME
	,
	-----------------------------------------------------
	@LeadIds AS VARCHAR(MAX) = NULL
	,@LeadBucketId AS SMALLINT = 1
AS
BEGIN
	DECLARE @ApplicationId TINYINT

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
							IF @SearchText = 'New Vehicle'
								SET @SerchInquiryTypeId = 3
				END;
				ELSE
					IF (@FilterType = 6)
					BEGIN
						IF @SearchText LIKE '%Fresh%'
							SET @LeadPriorityId = 1
						ELSE
							IF @SearchText LIKE '%Pending%'
								SET @LeadPriorityId = 2
							ELSE
								IF @SearchText LIKE '%Follow%'
									SET @LeadPriorityId = 3
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
			AND @InqPriority IS NOT NULL
			)
	BEGIN
		SELECT @InqPriorityId = TC_InquiryStatusId
		FROM TC_InquiryStatus
		WHERE STATUS LIKE @InqPriority
	END
	ELSE
		IF (@InqPriority = 'Not Set')
		BEGIN
			SET @InqPriorityId = NULL
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

	WITH CTE
	AS (
		SELECT TCAC.TC_LeadId
			,C.CustomerName AS [CustomerName]
			,C.Email
			,C.Mobile
			,S.STATUS AS Eagerness
			,ScheduledOn
			,TCIL.CarDetails AS [InterestedIn]
			,TCAC.LastCallComment
			,TS.Source
			,-- Modified By Vivek Gupta on 21-02-2014
			LatestInquiryDate
			,(
				CASE 
					WHEN LatestInquiryDate > ScheduledOn
						THEN LatestInquiryDate
					ELSE ScheduledOn
					END
				) AS OrderDate
			,VW.Make AS [Make]
			,VW.Model AS [Model]
			,VW.Version AS [Version]
			,COALESCE(CT.Name,C.Location) AS Location -- Added by vicky gupta on 24/11/2015
		--INTO #TempAllUniqueData
		FROM TC_ActiveCalls AS TCAC WITH (NOLOCK)
		JOIN TC_CustomerDetails AS C WITH (NOLOCK) ON TCAC.TC_LeadId = C.ActiveLeadId
		JOIN TC_InquiriesLead AS TCIL WITH (NOLOCK) ON TCAC.TC_LeadId = TCIL.TC_LeadId
		JOIN TC_Users AS TU WITH (NOLOCK) ON TCIL.TC_UserId = TU.Id
		JOIN TC_InquirySource AS TS WITH (NOLOCK) ON C.TC_InquirySourceId = TS.Id
		LEFT JOIN TC_InquiryStatus AS S WITH (NOLOCK) ON TCIL.TC_InquiryStatusId = S.TC_InquiryStatusId
		LEFT JOIN vwAllMMV AS VW WITH (NOLOCK) ON TCIL.LatestVersionId = VW.VersionId AND VW.ApplicationId = @ApplicationId
		LEFT JOIN TC_NewCarInquiries AS TCNI WITH(NOLOCK)   ON TCNI.TC_InquiriesLeadId=TCIL.TC_InquiriesLeadId  --- Added by vicky Gupta
		LEFT JOIN Cities CT WITH(NOLOCK) ON CT.Id = TCNI.CityId AND CT.IsDeleted = 0  -- Added by Vicky Gupta 
			
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
				OR (TS.Source = @SearchSource)
				)
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
			---------------------------------------------------------------------------------------------------------------------------
			--Added By Vivek on 27th June,2013 Used conditions for new parameters for Advanced search
			--Modified by Manish on 26-08-2013 advance search conditions commented in filter criteria because it is not using from Front end
			/* AND      ( ( @InqStatusId IS NULL )     OR ( TCIL.TC_InquiriesLeadId IN 

                                                                      (SELECT TCNI.TC_InquiriesLeadId  

																	   FROM TC_NewCarInquiries  AS TCNI

																	   JOIN TC_InquiriesLead AS TCIL ON TCNI.TC_InquiriesLeadId=TCIL.TC_InquiriesLeadId

																	   WHERE BookingStatus = 32 AND TCIL.BranchId=@BranchId

																	    	UNION                --These select statements take out all the booked inquiries

																	   SELECT TCBI.TC_InquiriesLeadId -- either it is new car booking or used car bookings

																	   FROM TC_BuyerInquiries TCBI

																	   JOIN TC_InquiriesLead AS TCIL ON TCBI.TC_InquiriesLeadId=TCIL.TC_InquiriesLeadId

																	   WHERE BookingStatus =34 AND TCIL.BranchId=@BranchId ))

															)

																	   

                AND      (	( ISNULL(@InqPriorityId,TCIL.TC_InquiryStatusId) IS NULL)  OR (@InqPriorityId = 0)  OR  

							( TCIL.TC_InquiryStatusId = @InqPriorityId ) )

				AND		 ((@InqAddedDate IS NULL) OR (TCIL.LatestInquiryDate BETWEEN @FromInqAddedDate AND @ToInqAddedDate))*/
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
				OR (TCIL.TC_InquiryStatusId = @InqPriorityId)
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
			AND (
				(@LeadBucketId = 1)
				OR (
					(@LeadBucketId = 2)
					AND CONVERT(DATE, TCIL.CreatedDate) = CONVERT(DATE, GETDATE()) AND TCAC.CallType = 1					
					)
				OR (
					(@LeadBucketId = 3)
						AND CONVERT(DATE, ScheduledOn) = CONVERT(DATE, GETDATE())
						AND CONVERT(DATE, TCIL.CreatedDate) <> CONVERT(DATE, GETDATE())
						AND ISNULL(TC_NextActionId,0) <> 1
						AND TCAC.CallType <> 1
					)
				OR (
					(@LeadBucketId = 4)
					AND TC_NextActionId = 1 -- I.E. Personal Visit
					AND CONVERT(DATE, ScheduledOn) = CONVERT(DATE, GETDATE())
					)
				OR (
					(@LeadBucketId = 9)
					AND DATEDIFF(day,CONVERT(DATE, ScheduledOn),CONVERT(DATE, GETDATE())) >59 
					)
				OR (
					(@LeadBucketId = 5)
					AND CONVERT(DATE, ScheduledOn) < CONVERT(DATE, GETDATE())
					)
				OR (
						(@LeadBucketId = 6)
					AND	(TCIL.TC_LeadDispositionId = 4)    --added by Ashwini Dhamankar --Booked Leads 
			   
				   )
				OR (  (@LeadBucketId = 9)
					AND DATEDIFF(day,CONVERT(DATE, ScheduledOn),CONVERT(DATE, GETDATE())) >59 
					AND ISNULL(TCIL.TC_LeadDispositionId,0) <> 4 
				  -- 2 Months or Older
				
					)
				)
		--ORDER BY OrderDate DESC
		)
	SELECT *
		,ROW_NUMBER() OVER (
			---line commented and chages made by manish on 16-07-2013
			PARTITION BY TC_LeadId ORDER BY OrderDate DESC -- ORDER BY  NextFollowUpDate DESC, LatestInquiryDate DESC
			) RowNumber
	INTO #TempAllUniqueData
	FROM CTE
	SELECT *
	FROM #TempAllUniqueData
	WHERE RowNumber = 1
		----------------------------------------------------------------------------------------------------------------------------
END


