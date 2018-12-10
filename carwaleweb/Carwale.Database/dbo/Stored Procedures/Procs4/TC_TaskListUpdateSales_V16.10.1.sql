IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_TaskListUpdateSales_V16]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_TaskListUpdateSales_V16]
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
-- Modified By: Tejashree Patil on 28 Sept 2016, Changes related feedback calling leads
-- =========================================================================================
CREATE PROCEDURE [dbo].[TC_TaskListUpdateSales_V16.10.1]
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
				,TCAC.TC_BusinessTypeId --added by : Deepak on July 14,2016
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
				,LIT.LeadInquiryType AS InquiryType   --modified by : Ashwini Dhamankar on Sept 28,2016
				,TCIL.TC_LeadInquiryTypeId AS InquiryTypeId
				,C.IsVerified
				,TCIL.CreatedDate AS LeadCreationDate
				--Lead Bucket Logic Starts
				,CASE 
					-- New Lead - 2, 24
					WHEN CONVERT(DATE, TCIL.CreatedDate) = CONVERT(DATE, GETDATE())
						AND TCAC.CallType = 1
						THEN (
								CASE TCAC.TC_BusinessTypeId
									WHEN 3
										THEN 2 -- Sales
									WHEN 5
										THEN 24 -- Advantage
									WHEN 7
										THEN 34 -- Feedback calling--added by : Tejashree Patil on July 13,2016
									END
								)

							--Booked Leads - 6, 22
					WHEN (TCIL.TC_LeadDispositionId = 4)
						THEN (
								CASE TCAC.TC_BusinessTypeId
									WHEN 3
										THEN 6 -- Sales										
									WHEN 5
										THEN 22 -- Advantage
									END
								)
				
							-- Call Today Lead - 3,12
					WHEN CONVERT(DATE, ScheduledOn) = CONVERT(DATE, GETDATE())
						AND (
							CONVERT(DATE, TCIL.CreatedDate) <> CONVERT(DATE, GETDATE())
							OR ISNULL(TCAC.CallType, 0) <> 1
							) -- Added By Deepak on 19th May 2016
						AND ISNULL(TC_NextActionId, 0) <> 1
						THEN (
								CASE TCAC.TC_BusinessTypeId
									WHEN 3
										THEN 3 -- Sales
									WHEN 5
										THEN 19 -- Advantage
									WHEN 7
										THEN 34 -- Feedback calling--added by : Tejashree Patil on July 13,2016
									END
								)

							-- Personal Visit Lead - 4
					WHEN TC_NextActionId = 1 -- I.E. Personal Visit
						AND CONVERT(DATE, ScheduledOn) = CONVERT(DATE, GETDATE())
						AND TCIL.TC_LeadStageId <> 3
						THEN (
								CASE TCAC.TC_BusinessTypeId
									WHEN 3
										THEN 4 -- Sales 
									WHEN 5
										THEN 20 -- Advantage
									END
								)

							-- Pending Leads - 5, 21
					WHEN CONVERT(DATE, ScheduledOn) < CONVERT(DATE, GETDATE())
						AND ISNULL(TCIL.TC_LeadDispositionId, 0) <> 4
						THEN (
								CASE TCAC.TC_BusinessTypeId
									WHEN 3
										THEN 5 -- Sales
									WHEN 5
										THEN 21 -- Advantage
									WHEN 7
										THEN 34 -- Feedback calling--added by : Tejashree Patil on July 13,2016
									END
								)

							-- 2 Months or Older - 9
					WHEN DATEDIFF(day, CONVERT(DATE, ScheduledOn), CONVERT(DATE, GETDATE())) > 59
						AND ISNULL(TCIL.TC_LeadDispositionId, 0) <> 4
						THEN (
								CASE TCAC.TC_BusinessTypeId
									WHEN 3
										THEN 9 -- Sales
									WHEN 5
										THEN 23 -- Advantage
									END
								)
					END AS BucketTypeId
				--Lead Bucket Logic Ends
				,CASE 
					WHEN TCIL.TC_LeadInquiryTypeId = 3
						AND D.ApplicationId = 1
						THEN (
								SELECT 'Exchange: ' + ISNULL(VW.Car, 'NA') + ' , ' + CONVERT(VARCHAR, ISNULL(EXNC.ExpectedPrice, 0))
								FROM TC_ExchangeNewCar EXNC WITH (NOLOCK)
								JOIN vwMMV VW WITH (NOLOCK) ON VW.VersionId = EXNC.CarVersionId
								WHERE EXNC.TC_NewCarInquiriesId IN (
										SELECT TOP 1 TC_NewCarInquiriesId
										FROM TC_NewCarInquiries NCI WITH (NOLOCK)
										WHERE NCI.TC_InquiriesLeadId = TCIL.TC_InquiriesLeadId
											AND ISNULL(NCI.TC_NewCarExchangeId, 0) <> 0
										)
								)
					WHEN TCIL.TC_LeadInquiryTypeId <> 3
						OR D.ApplicationId = 2
						THEN ''
					END AS ExchangeCar
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
	---------------------------------------


