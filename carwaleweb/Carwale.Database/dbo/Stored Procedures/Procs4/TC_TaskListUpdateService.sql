IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_TaskListUpdateService]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_TaskListUpdateService]
GO

	
-- =========================================================================================
-- Owner      :  Deepak    
-- Create date : 20-June-2016
-- Modified By : Ashwini Dhamankar on June 30,2016 (Added logic for bucket types of service)
-- Modified By : Deepak Tripathi on July 26,2016 (Added vwAllMMV to get CarName)
-- Modified By : Suresh Prajapari on 27th July, 2016
-- Description : Added Bucket creation for Advantage Leads.
-- Modified By : Ashwini Dhamankar on Aug 8,2016 (Modified buckettype logic,leaddispositionid based)
-- Modified By : Nilima More on 9th Aug 2016,add drop requested bucket(25 bucket id and leaddispositionid = 115).
-- Modified By : Khushaboo Patil on 10 Aug 2016 show drop cancel and drop complete in completed tab
-- Modified By : Khushaboo patil on 16th Aug 2016 added condition to show pickup cancelled in book tab 
-- Modifed  By : Nilima More On 23rd August 2016,fetch registration number and insert into TC_taskList,added bucket change logic for insurance.
-- Modifed  By : Nilima More On 6th Sept 2016,added call today logic for insurance and service.
-- Modified By : Komal Manjare 14 sept 2016 added leaddispositionId for payment failed
-- Modified By : Suresh Prajapati on 15th Sept, 2016
-- Description : Categorized Insurance leads into 'New', 'Call Today' and 'All' buckets only
-- Modified By : Ashwini Dhamankar on Sept 28,2016 (added join with TC_LeadInquiryType)
-- =========================================================================================
CREATE PROCEDURE [dbo].[TC_TaskListUpdateService]
	@CallId BIGINT
	,@ApplicationId TINYINT = 1
AS
BEGIN
	IF NOT EXISTS (SELECT TC_CallsId FROM TC_TaskLists WITH (NOLOCK) WHERE TC_CallsId = @CallId)
		BEGIN
			INSERT INTO TC_TaskLists (
				TC_InquiriesLeadId
				,BranchId
				,TC_CallsId
				,CustomerId
				,CustomerName
				,CustomerEmail
				,CustomerMobile
				,InqSourceId
				,TC_LeadId
				,TC_InquiryStatusId
				,ScheduledOn
				,InterestedIn
				,TC_CallTypeId
				,LastCallComment
				,LatestInquiryDate
				,OrderDate
				,InquirySourceName
				,UserId
				,TC_LeadStageId
				,TC_LeadDispositionId
				,TC_NextActionId
				,InquiryTypeName
				,TC_LeadInquiryTypeId
				,IsVerified
				,TC_InquiriesLeadCreateDate
				,BucketTypeId
				,ExchangeCar
				,Eagerness
				,Location
				,Car
				,LeadAge
				,AssignedTo
				,TC_BusinessTypeId --added by : Deepak on July 14,2016
				,TC_NextActionDate --added by : Ashwini Dhamankar on July 13,2016
				,RegistrationNumber
				)
			SELECT TOP 1 TCIL.TC_InquiriesLeadId
				,TCIL.BranchId
				,TCAC.TC_CallsId
				,C.id
				,C.CustomerName
				,C.Email
				,C.Mobile
				,C.TC_InquirySourceId
				,tcac.TC_LeadId
				,TCIL.TC_InquiryStatusId
				,TCAC.ScheduledOn
				,VM.Car
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
				,TCAC.TC_NextActionId
				,LIT.LeadInquiryType AS InquiryType   --modified by : Ashwini Dhamankar on Sept 28,2016 AS InquiryType
				,TCIL.TC_LeadInquiryTypeId AS InquiryTypeId
				,C.IsVerified
				,TCIL.CreatedDate AS LeadCreationDate
				--Lead Bucket Logic Starts
				,CASE 
					-- New Lead - 11
					WHEN CONVERT(DATE, TCIL.CreatedDate) = CONVERT(DATE, GETDATE())
						AND TCAC.CallType = 1
						THEN  11 -- Service 
									
							-- Call Today Lead - 12 -- added By : Nilima More On 6th Sept 2016,added call today logic for insurance and service.
					WHEN CONVERT(DATE, ScheduledOn) = CONVERT(DATE, GETDATE())
						AND (
							CONVERT(DATE, TCIL.CreatedDate) <> CONVERT(DATE, GETDATE())
							OR ISNULL(TCAC.CallType, 0) <> 1
							) -- Added By Deepak on 19th May 2016
						AND ISNULL(TC_NextActionId, 0) = 2
						THEN 12 -- Service
									
							--Car pick up requests for Service leads - 13
					WHEN (
							TCIL.TC_LeadDispositionID = 106 -- Modified By : Ashwini Dhamankar on Aug 8,2016
							AND DATEDIFF(DD, GETDATE(), TCAC.TC_NextActionDate) <= 0
							)
						THEN 13

							--Drop Requested for service leads- 25
					WHEN (
							 TCIL.TC_LeadDispositionID = 115 --Drop Requested
							AND DATEDIFF(DD, GETDATE(), TCAC.TC_NextActionDate) <= 0
							)
						THEN 25 --added by Nilima on 9th Aug 2016,add drop requested bucket.
							
							-- Service completed - 16
					WHEN (
								TCIL.TC_LeadDispositionID IN (
								104
								,116
								,117
								) -- added by Khushaboo Patil on 10 Aug 2016 show drop cancel and drop complete in completed tab
							)
						THEN 16

							-- Service booked = 15
					WHEN (
							 TCIL.TC_LeadDispositionID IN (
								102
								,106
								,107
								,118
								) --service booked,pickup requested whose req date is not today's date and pick up completed and cancelled
							) --modified by khushaboo patil on 16th Aug 2016 added condition to show pickup cancelled in book tab  	
						THEN 15 --added by : Ashwini Dhamankar on Aug 8,2016 
					
							-- Pending Leads - 5, 21
					WHEN CONVERT(DATE, ScheduledOn) < CONVERT(DATE, GETDATE())
						AND ISNULL(TCIL.TC_LeadDispositionId, 0) <> 4
						THEN 14 -- Service
						
					END AS BucketTypeId
				--Lead Bucket Logic Ends
				,'' AS ExchangeCar
				,S.STATUS AS Eagerness --Added by: Kritika Choudhary on 15th April 2016
				,C.Location AS Location --Added by: Kritika Choudhary on 15th April 2016
				,VM.Car AS Car --Added by: Kritika Choudhary on 15th April 2016
				,CASE 
					WHEN TCIL.TC_LeadDispositionId IN (
							4
							,32
							)
						THEN ''
					ELSE (DATEDIFF(day, TCIL.CreatedDate, GETDATE()))
					END AS LeadAge
				,TU.UserName AS AssignedTo
				,TCAC.TC_BusinessTypeId
				,TCAC.TC_NextActionDate --Added By Deepak on 14th July 2016
				,TCIL.RegistrationNumber --Added By Nilima More On Aug 23,2016

			FROM TC_ActiveCalls AS TCAC WITH (NOLOCK)
				INNER JOIN TC_Lead AS TL WITH (NOLOCK) ON TCAC.TC_LeadId = TL.TC_LeadId
				INNER JOIN TC_CustomerDetails AS C WITH (NOLOCK) ON TL.TC_CustomerId = C.id
				INNER JOIN TC_InquiriesLead AS TCIL WITH (NOLOCK) ON TL.TC_LeadId = TCIL.TC_LeadId
				INNER JOIN TC_Users AS TU WITH (NOLOCK) ON TCAC.TC_UsersId = TU.Id
				INNER JOIN Dealers AS D WITH (NOLOCK) ON D.ID = C.BranchId
				INNER JOIN TC_LeadInquiryType AS LIT WITH (NOLOCK) ON TCIL.TC_LeadInquiryTypeId = LIT.TC_LeadInquiryTypeId   --added by : Ashwini Dhamankar on Sept 28,2016
				LEFT JOIN vwAllMMV VM WITH (NOLOCK) ON VM.VersionId = TCIL.LatestVersionId AND vm.ApplicationId = D.ApplicationId
				LEFT JOIN TC_InquirySource AS TS WITH (NOLOCK) ON C.TC_InquirySourceId = TS.Id
				LEFT JOIN TC_InquiryStatus AS S WITH (NOLOCK) ON TCIL.TC_InquiryStatusId = S.TC_InquiryStatusId
			WHERE TCAC.TC_CallsId = @CallId
			ORDER BY TCIL.TC_InquiriesLeadId DESC
		END
END



