IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_InquiryDetailsNewCar]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_InquiryDetailsNewCar]
GO

	-- =============================================
-- Created By: Nilesh Utture
-- Created Date:04th July, 2013
-- Description: Reports for New Car Inquiries below are the report ids for parametre	
-- Modified By: Nilesh Utture on 19th July, 2013 6:38 PM Added TU.IsActive = 1 to each count
-- Modified By: Nilesh Utture on 22nd July, 2013 12:18 PM Applied INNER JOIN ON TC_Users Instead of LEFT JOIN 
-- Modified By: Nilesh Utture on 25th July, 2013 11:49 AM BookedLead AND Lost Lead are shown on basis of event creation date
--				AND Booking data is shown Inquiry wise instead of Lead wise		
-- Modified BY: Nilesh Utture on 1st August, 2013 Fetched Pending TD COUNT based on TDDATE, Added count for Delivered Leads & TD Completed		
-- Modified By: Nilesh Utture on 6th August, 2013 Delivered car will not be included in Bookings
-- Modified By: Nilesh Utture on 26th August, 2013 Addded query to get total Bookings and removed date clause on live bookings
-- 1. Total
-- 2. ActiveHotLead
-- 3. ActiveWarmLead
-- 4. ActiveNormalLead 
-- 5. ActiveNotSet
-- 6. LiveBookedLead
-- 7. Lost
-- 8. PendingFollowUp
-- 9. PendingTestDrive
--10. DeliveredLead
--11. TD Completed
--12. Total booked Leads
--13. Retail
--14. Pending Deliveries
--Modified by: Tejashree on 26-08-2013, Fetched complete name of a customer.
--Modified By: Nilesh Utture on 04-09-2013, Added some extra fields in SELECT clause and applied joins in all cases
-- Modifiedb By : Manish on 06-09-2013 separate query for Pending followup
-- Modified By: Nilesh Utture on 19-09-2013 Added Retail, Pending Deliveries, Changed logic of live bookings, Pending TD's and Pending Followup's
-- Modified by vivek singh on 18-12-2013 added new columns Customer Unique ID,Address,Inq. Entry Date,Number of Test Drives Given,Booking Model Year,Booking Model Colour,Retail Entry Date,Retail Date,Retail Customer Name,Retail Customer Mobile no.,Retail Customer email id,Lost Date,Campaign Name,Exchange Car (Make, model, variant),Exchange Car Year,Exchange Car Km driven,Exchange Car Expected price,Offer,Finance
-- Modified by vivek singh on 03-02-2014 changed the finance column from TCNI.isfinancerequired  to NB.IsLoanRequired
-- Modified by vivek singh on 06-02-2014 changed columns fetched for retailcustomername,mobile and email to bookingcustomername and booking mobile no
-- Tejashree Patil on 16 Jun 2014, Fetched Tc_LeadStageId and TCIL.TC_UserId.
-- Modfied By Vivek Gupta on 16-07-2014, added @leadValue =15 with lost leads for getting details of Rejected leads
-- Modified By : Tejashree Patil on 13 Nov 2014, Added @ApplicationId to identify application and joined with vwAllMMV view.
-- Modified By Vivek Gupta on 03-08-2015, replaced Inner join vwAllMMV with left join vwallMMV
-- [TC_InquiryDetailsNewCar] 14132,'2014-11-01 12:10:40.323','2014-11-17 12:10:40.323',2,13100,2
-- Modified by vivek gupta on 20-08-2015, changed date filter condition from tcil.CreatedDate with tcni.CreatedOn
-- Vicky Gupta :24/11/2015, send city of customer from newCarInquiry table, if city not exist ther, then take from TC_CustomerDetails
--Modified By : Afrose on 14-12-2015, selected last call comment in case of lost or rejected
--Modified By : Ashwini Dhamankar on Dec 17,2015 (Added constraint of ISNULL(TCNI.MostInterested,0) = 1 to fetch only one inquiry of one lead)
-- Modified By : Ashwini Dhamankar on Jan 25,2016 (Removed condition of MostInterested in booking(6) and totalbooking(12) status)
--EXEC TC_InquiryDetailsNewCar 5,'2015-10-01 12:00:00 AM','2015-12-15 11:59:59 PM',15,-1
-- Modified by: kritika Choudhary on 4th feb 2016, added TC_LeadDispositionReason,DMSScreenShotHostUrl and DMSScreenShotUrl in lost lead case
-- Modified By : Ashwini Dhamankar on July 22,2016 (Fetched LastComments for delivered abd retail leads and disposition comment for all leads)
 -- Modified by ruchira patil on 2nd aug 2016(changed the date filter for total leads to avoid count mismatch)
 -- Modified By : Nilima More On 4th oct 2016,modified followup pending conditon.
 --exec [TC_InquiryDetailsNewCar]5,'2016/10/01','2016/10/04',1,null,1
 -- Modified By : Ruchira Patil on 4th Oct 2016 (modified ActiveHotLead condition)
 -- Modified By : Nilima More On 13th oct 2016,modified lost and rejected count conditon.
 -- Modified By : Ruchira Patil on 2nd Nov 2016 (fetched comments from TC_NewCarInquiries when LastCallComment is null for leadvalue = 1)
-- =============================================
CREATE  PROCEDURE [dbo].[TC_InquiryDetailsNewCar] 
  (
	@BranchId INT,
	@FromDate DATETIME,
	@ToDate  DATETIME,
	@LeadValue TINYINT,
	@UserIdList VARCHAR(MAX),
	@ApplicationId TINYINT = 1-- Modified By : Tejashree Patil on 30-10-2014, Added @ApplicationId to identify application and joined with vwAllMMV view.
   )
AS
	BEGIN
	
	DECLARE @InquiryStatusID TINYINT=NULL
	DECLARE @TC_LeadDispositionID SMALLINT =0
	DECLARE @ScheduledOn DATETIME=NULL
	
	IF @UserIdList = '-1'
		SET @UserIdList = NULL
	--------------Below code is used for deciding inquiry status as per lead value changes done on 04-09-2013 by Nilesh
	 IF (@LeadValue =2)
	     SET @InquiryStatusID=1
	 IF ( @LeadValue =3 )
	     SET @InquiryStatusID=2
	 IF ( @LeadValue =4 )
	     SET @InquiryStatusID=3
	 IF ( @LeadValue =5 )
	     SET @InquiryStatusID= 0  -----Used for the cases where eagerness not set
	  IF ( @LeadValue =6 )
	     SET @TC_LeadDispositionID=4  ---Used for Booked active inquiries but not delivered
	  IF ( @LeadValue =8 )
	     SET @ScheduledOn=GETDATE()  ---Used for Pending followups
	
	
	   IF @LeadValue=1 --Total
			BEGIN 
			 SELECT  TCNI.TC_NewCarInquiriesId, 
					   TC.Id , 
					   (ISNULL(TC.Salutation,'')+' '+ TC.CustomerName+' '+ISNULL(TC.LastName,'')) CustomerName, 
					   TC.Email, 
					   TC.Mobile, 
					   COALESCE(C.Name,TC.Location) AS Location, -- Added by vicky gupta on 24/11/2015
					   TCNI.CreatedOn AS EntryDate, 
					   TU.UserName,
					   VW.Car AS Car,
					   TCIS.Source AS 'Source Name',
					   CASE(TCIL.TC_InquiryStatusId)
								WHEN 1 THEN 'Hot'
								WHEN 2 THEN 'Warm'
								WHEN 3 THEN 'Normal'
					   END AS 'Eagerness',
					   CASE
							WHEN (TCNI.TC_LeadDispositionID IS NULL)  THEN 'Active'
							WHEN (TCNI.TC_LeadDispositionID IN (4)) THEN 'Booked'
							WHEN (TCNI.TC_LeadDispositionID IN (41) AND NB.BookingStatus = 32 )THEN 'Booked'
							WHEN (TCNI.TC_LeadDispositionID IS NOT NULL AND TCNI.TC_LeadDispositionID <> 4) THEN 'Closed'
					   END AS 'Status',
					   CASE WHEN AC.LastCallComment IS NULL THEN TCNI.Comments ELSE AC.LastCallComment END AS LastComment, -- Modified By : Ruchira Patil on 2nd Nov 2016 
					   AC.ScheduledOn AS NextFollowupDate,
					   ----------- Modified By: Nilesh Utture on 04-09-2013, Added following field in all cases till end of SP -----------
					   TCNI.CompanyName,
					   TCNI.TDDate,
					   TCNI.BookingEventDate AS BookingEntryDate,
					   TCNI.BookingDate,
					   NB.Payment AS BookingAmount,
					   NB.PrefDeliveryDate AS PromisedDeliveryDate,
					   NB.DeliveryEntryDate,
					   NB.DeliveryDate,
					   NB.PanNo,
					   NB.ChassisNumber,
					   TLD.Name AS LostReason,
					   TSD.SubDispositionName AS LostSubdisposition,
					   V.Make,
					   V.Model,
					   V.Version,
					   NB.InsuranceCoverNumber,
					   NB.EngineNumber,
					   NB.InvoiceNumber,
					   NB.RegistrationNo,
					   ----------- Modified By: Nilesh Utture on 04-09-2013, Added following field in all cases till end of SP -----------
					   ISNULL(TC.UniqueCustomerId,'') AS UniqueCustomerId, --Modified by: Vivek singh on 18-12-2013 added new columns
					   ISNULL(TC.Address,'') AS Address,
					   --ISNULL(TCIL.CreatedDate,'-') AS InquiryEntryDate,		
					   (SELECT COUNT(*) FROM TC_TDCalendar TD WITH(NOLOCK) WHERE TD.TC_NewCarInquiriesId= TCNI.TC_NewCarInquiriesId)  AS NumberOfTDs,
					  NB.ModelYear AS BookingModelYear,
					   --ISNULL(VC.Color,'')AS BookingModelColour,
					   ISNULL(VC.VersionColor,'')AS BookingModelColour,
					   NB.InvoiceDate AS RetailDate,
					   ISNULL(NB.BookingName,'') AS RetailCustomerName,          --06-02-2014 changed columns fetched for retailcustomername,mobile and email to bookingcustomername and booking mobile no
					   ISNULL(NB.BookingMobile,'') AS RetailCustomerMobileNo,
					   ISNULL(NB.Email,'') AS RetailCustomerEmail,             --06-02-2014 changed columns fetched for retailcustomername,mobile and email to bookingcustomername and booking mobile no
					   TCNI.DispositionDate AS LostDate,
					   ISNULL(VEX.Make,'') AS ExchangeCarMake,
					   ISNULL(VEX.Model,'') AS ExchangeCarModel,
					   ISNULL(VEX.Version,'') AS ExchangeCarversion,
					   TENC.MakeYear AS ExchangeCarYear,
					   ISNULL(TENC.Kms,'') AS ExchangeCarKMdriven,
					   ISNULL(TENC.ExpectedPrice,'') AS ExchangeCarExpectedPrice,
					   ISNULL(TCO.OfferName,'') AS Offer,
					    ISNULL(NB.IsLoanRequired,'') AS Finance,
					   TCIL.TC_UserId,
					   TCIL.TC_LeadId,
					   TCIL.TC_LeadStageId, --Tejashree Patil on 16 Jun 2014
					   TCNI.TC_LeadDispositionReason AS LeadDispositionReason   --added by Ashwini Dhamankar on Jul 22,2016

				FROM TC_CustomerDetails AS TC   WITH(NOLOCK)  
					 --JOIN  TC_Lead          AS TL   WITH(NOLOCK) ON TL.TC_CustomerId= TC.Id --Tejashree Patil on 16 Jun 2014
					 --LEFT JOIN  TC_ActiveCalls   AS AC   WITH(NOLOCK) ON AC.TC_LeadId = TL.TC_LeadId
					 JOIN  TC_InquiriesLead AS TCIL WITH(NOLOCK) ON TC.Id=TCIL.TC_CustomerId AND TCIL.CreatedDate BETWEEN @FromDate AND @ToDate -- Modified by ruchira patil on 2nd aug 2016(changed the date filter on avoid count mismatch)
					 JOIN TC_NewCarInquiries AS TCNI WITH(NOLOCK)   ON TCNI.TC_InquiriesLeadId=TCIL.TC_InquiriesLeadId 
																	  --AND TCNI.CreatedOn BETWEEN @FromDate AND @ToDate
																	  AND ISNULL(TCNI.MostInterested,0) = 1
					 JOIN  TC_InquirySource AS TCIS WITH(NOLOCK) ON TCIS.Id = TCNI.TC_InquirySourceId 
							  AND TCIL.TC_LeadInquiryTypeId=3
							  AND  TC.BranchId	= @BranchId 
							  AND  TCIL.BranchId=  @BranchId 
							  AND ((TCIL.TC_UserId IN (SELECT ListMember AS UserId FROM fnSplitCSV(@UserIdList)))OR @UserIdList IS NULL)
					
					 INNER JOIN  TC_Users         AS TU   WITH(NOLOCK)  ON TU.Id = TCIL.TC_UserId AND ISNULL(TU.lvl,0) <> 0/*AND TU.IsActive = 1*/ -- Modified By: Nilesh Utture on 19th July, 2013 6:38 PM
					 --INNER JOIN vwMMV VW WITH(NOLOCK) ON VW.VersionId = TCNI.VersionId
					 LEFT JOIN vwAllMMV VW WITH(NOLOCK) ON VW.VersionId = TCNI.VersionId AND VW.ApplicationId=@ApplicationId AND VW.ApplicationId=@ApplicationId
					 LEFT JOIN  TC_ActiveCalls   AS AC   WITH(NOLOCK) ON AC.TC_LeadId = TCIL.TC_LeadId
					 ----------- Modified By: Nilesh Utture on 04-09-2013, applied joins in all cases till end of SP -----------
					 LEFT JOIN TC_NewCarBooking  AS NB   WITH(NOLOCK) ON NB.TC_NewCarInquiriesId = TCNI.TC_NewCarInquiriesId
					 LEFT JOIN TC_LeadDisposition TLD(NOLOCK) ON TLD.TC_LeadDispositionId = TCNI.TC_LeadDispositionID
					 LEFT JOIN TC_LeadSubDisposition TSD(NOLOCK) ON TSD.TC_LeadSubDispositionId = TCNI.TC_SubDispositionId
					 LEFT JOIN Cities C WITH(NOLOCK) ON C.Id = TCNI.CityId  
					 --LEFT JOIN vwMMV V WITH(NOLOCK) ON V.VersionId = TCNI.LostVersionId
					 LEFT JOIN vwAllMMV V WITH(NOLOCK) ON V.VersionId = TCNI.LostVersionId AND V.ApplicationId=@ApplicationId
					 LEFT JOIN TC_ExchangeNewCar TENC WITH(NOLOCK) ON TENC.TC_NewCarInquiriesId=TCNI.TC_NewCarInquiriesId
					 --LEFT JOIN vwMMV VEX WITH(NOLOCK) ON VEX.VersionId = TENC.CarVersionId
					 LEFT JOIN vwAllMMV VEX WITH(NOLOCK) ON VEX.VersionId = TENC.CarVersionId AND VEX.ApplicationId=@ApplicationId
					 LEFT JOIN TC_Offers TCO WITH(NOLOCK) ON TCO.TC_OffersId = NB.TC_OffersId
					 --LEFT JOIN VersionColors VC WITH(NOLOCK) ON VC.ID= NB.CarColorId
					 LEFT JOIN vwAllVersionColors VC WITH(NOLOCK) ON VC.VersionColorsId= NB.CarColorId AND VC.ApplicationId=@ApplicationId
					 ----------- Modified By: Nilesh Utture on 04-09-2013, applied joins in all cases till end of SP -----------
				ORDER BY TCNI.CreatedOn DESC
	   END 
	   ELSE IF (@LeadValue =2 OR @LeadValue=3 OR @LeadValue=4 OR @LeadValue =5 OR @LeadValue=6 ) --ActiveHotLead , Warm ,Normal, Eagerness not set
			BEGIN                                                                               --LiveBooking
			 SELECT  TCNI.TC_NewCarInquiriesId, 
					   TC.Id , 
					   (ISNULL(TC.Salutation,'')+' '+ TC.CustomerName+' '+ISNULL(TC.LastName,'')) CustomerName, 
					   TC.Email, 
					   TC.Mobile, 
					   COALESCE(C.Name,TC.Location) AS Location, -- Added by vicky gupta on 24/11/2015
					   TCNI.CreatedOn AS EntryDate, 
					   TU.UserName,
					   VW.Car AS Car,
					   TCIS.Source AS 'Source Name',
					   CASE(TCIL.TC_InquiryStatusId)
								WHEN 1 THEN 'Hot'
								WHEN 2 THEN 'Warm'
								WHEN 3 THEN 'Normal'
					   END AS 'Eagerness',
					   CASE
							WHEN (TCNI.TC_LeadDispositionID IS NULL)  THEN 'Active'
							WHEN (TCNI.TC_LeadDispositionID IN (41) AND NB.BookingStatus = 32 )THEN 'Booked'
							WHEN (TCNI.TC_LeadDispositionID IS NOT NULL AND TCNI.TC_LeadDispositionID <> 4) THEN 'Closed'
					   END AS 'Status',
					   AC.LastCallComment AS LastComment,
					   AC.ScheduledOn AS NextFollowupDate,
					   TCNI.CompanyName,
					   TCNI.TDDate,
					   TCNI.BookingEventDate AS BookingEntryDate,
					   TCNI.BookingDate,
					   NB.Payment AS BookingAmount,
					   NB.PrefDeliveryDate AS PromisedDeliveryDate,
					   NB.DeliveryEntryDate,
					   NB.DeliveryDate,
					   NB.PanNo,
					   NB.ChassisNumber,
					   TLD.Name AS LostReason,
					   TSD.SubDispositionName AS LostSubdisposition,
					   V.Make,
					   V.Model,
					   V.Version,
					   NB.InsuranceCoverNumber,
					   NB.EngineNumber,
					   NB.InvoiceNumber,
					   NB.RegistrationNo,
					   ISNULL(TC.UniqueCustomerId,'') AS UniqueCustomerId, --Modified by: Vivek singh on 18-12-2013 added new columns
					   ISNULL(TC.Address,'') AS Address,
					   --ISNULL(TCIL.CreatedDate,'-') AS InquiryEntryDate,		
					   (SELECT COUNT(*) FROM TC_TDCalendar TD WITH(NOLOCK) WHERE TD.TC_NewCarInquiriesId= TCNI.TC_NewCarInquiriesId)  AS NumberOfTDs,
					   NB.ModelYear AS BookingModelYear,
					   --ISNULL(VC.Color,'')AS BookingModelColour,
					   ISNULL(VC.VersionColor,'')AS BookingModelColour,
					   NB.InvoiceDate AS RetailDate,
					   ISNULL(NB.BookingName,'') AS RetailCustomerName,          --06-02-2014 changed columns fetched for retailcustomername,mobile and email to bookingcustomername and booking mobile no
					   ISNULL(NB.BookingMobile,'') AS RetailCustomerMobileNo,
					   ISNULL(NB.Email,'') AS RetailCustomerEmail,             --06-02-2014 changed columns fetched for retailcustomername,mobile and email to bookingcustomername and booking mobile no
					   TCNI.DispositionDate AS LostDate,
					   ISNULL(VEX.Make,'') AS ExchangeCarMake,
					   ISNULL(VEX.Model,'') AS ExchangeCarModel,
					   ISNULL(VEX.Version,'') AS ExchangeCarversion,
					   TENC.MakeYear AS ExchangeCarYear,
					   ISNULL(TENC.Kms,'') AS ExchangeCarKMdriven,
					   ISNULL(TENC.ExpectedPrice,'') AS ExchangeCarExpectedPrice,
					   ISNULL(TCO.OfferName,'') AS Offer,
					    ISNULL(NB.IsLoanRequired,'') AS Finance,
					   TCIL.TC_UserId,
					   TCIL.TC_LeadId,
					   TCIL.TC_LeadStageId --Tejashree Patil on 16 Jun 2014
					   ,TCNI.TC_LeadDispositionReason AS LeadDispositionReason   --added by Ashwini Dhamankar on Jul 22,2016
				FROM TC_CustomerDetails AS TC   WITH(NOLOCK)  
					 --JOIN  TC_Lead          AS TL   WITH(NOLOCK) ON TL.TC_CustomerId= TC.Id --Tejashree Patil on 16 Jun 2014
					 --LEFT JOIN  TC_ActiveCalls   AS AC   WITH(NOLOCK) ON AC.TC_LeadId = TL.TC_LeadId
					 JOIN  TC_InquiriesLead AS TCIL WITH(NOLOCK) ON TC.Id=TCIL.TC_CustomerId 
					 JOIN TC_NewCarInquiries AS TCNI WITH(NOLOCK)   ON TCNI.TC_InquiriesLeadId=TCIL.TC_InquiriesLeadId 
																	  AND ((    TCIL.CreatedDate BETWEEN @FromDate AND @ToDate
																	               AND @LeadValue<>6 AND ISNULL(TCNI.MostInterested,0) = 1
																	       ) OR  (@LeadValue=6 AND TCNI.BookingDate BETWEEN @FromDate AND @ToDate)
																	      ) 
																	 
					 JOIN  TC_InquirySource AS TCIS WITH(NOLOCK) ON TCIS.Id = TCNI.TC_InquirySourceId 
							  AND TCIL.TC_LeadInquiryTypeId=3
							  AND  TC.BranchId	= @BranchId 
							  AND  TCIL.BranchId=  @BranchId 
							  AND ((TCIL.TC_UserId IN (SELECT ListMember AS UserId FROM fnSplitCSV(@UserIdList)))OR @UserIdList IS NULL)
							  AND TCIL.TC_LeadStageId <> 3  --Ruchira Patil on 4th Oct 2016
							  -- AND ISNULL(TCNI.TC_LeadDispositionID,0) <> 4   --commented by Ruchira Patil on 4th Oct 2016
							  AND  ((@LeadValue = 6 AND ISNULL(TCNI.TC_LeadDispositionId,0)=@TC_LeadDispositionID) OR (@LeadValue <> 6 AND ISNULL(TCNI.TC_LeadDispositionId,0)<>4)) --Ruchira Patil on 4th Oct 2016
							  AND (ISNULL(TCIL.TC_InquiryStatusId,0 )= @InquiryStatusID OR @InquiryStatusID IS NULL)
							  AND ISNULL(TCNI.CarDeliveryStatus,0)<>77
					 INNER JOIN  TC_Users         AS TU   WITH(NOLOCK)  ON TU.Id = TCIL.TC_UserId AND ISNULL(TU.lvl,0) <> 0/*AND TU.IsActive = 1*/ -- Modified By: Nilesh Utture on 19th July, 2013 6:38 PM
					 --INNER JOIN  TC_ActiveCalls   AS AC   WITH(NOLOCK) ON AC.TC_LeadId = TCIL.TC_LeadId AND ( AC.ScheduledOn<@ScheduledOn OR @ScheduledOn IS NULL)
					 LEFT JOIN TC_TaskLists AS AC WITH (NOLOCK) ON AC.TC_LeadId=TCIL.TC_LeadId --Ruchira Patil on 4th Oct 2016
					 --INNER JOIN vwMMV VW WITH(NOLOCK) ON VW.VersionId = TCNI.VersionId
					 LEFT JOIN vwAllMMV VW WITH(NOLOCK) ON VW.VersionId = TCNI.VersionId AND VW.ApplicationId=@ApplicationId
					 LEFT JOIN TC_NewCarBooking  AS NB   WITH(NOLOCK) ON NB.TC_NewCarInquiriesId = TCNI.TC_NewCarInquiriesId 
					 LEFT JOIN TC_LeadDisposition TLD(NOLOCK) ON TLD.TC_LeadDispositionId = TCNI.TC_LeadDispositionID
					 LEFT JOIN TC_LeadSubDisposition TSD(NOLOCK) ON TSD.TC_LeadSubDispositionId = TCNI.TC_SubDispositionId
					 LEFT JOIN Cities C WITH(NOLOCK) ON C.Id = TCNI.CityId  
					 --LEFT JOIN vwMMV V WITH(NOLOCK) ON V.VersionId = TCNI.LostVersionId 
					 LEFT JOIN vwAllMMV V WITH(NOLOCK) ON V.VersionId = TCNI.LostVersionId AND V.ApplicationId=@ApplicationId
					 LEFT JOIN TC_ExchangeNewCar TENC WITH(NOLOCK) ON TENC.TC_NewCarInquiriesId=TCNI.TC_NewCarInquiriesId
					 --LEFT JOIN vwMMV VEX WITH(NOLOCK) ON VEX.VersionId = TENC.CarVersionId
					 LEFT JOIN vwAllMMV VEX WITH(NOLOCK) ON VEX.VersionId = TENC.CarVersionId AND VEX.ApplicationId=@ApplicationId
					 LEFT JOIN TC_Offers TCO WITH(NOLOCK) ON TCO.TC_OffersId = NB.TC_OffersId
					 --LEFT JOIN VersionColors VC WITH(NOLOCK) ON VC.ID= NB.CarColorId
					 LEFT JOIN vwAllVersionColors VC WITH(NOLOCK) ON VC.VersionColorsId= NB.CarColorId AND VC.ApplicationId=@ApplicationId
					 -------- Modified By: Nilesh Utture on 19-09-2013, Added WHERE Clause in order to show cummulative bookings till @ToDate ---------------------
					 WHERE (@LeadValue <> 6 OR (@LeadValue = 6 AND NB.BookingStatus = 32 AND NB.InvoiceDate IS NULL  AND NB.BookingDate <= @ToDate))
				ORDER BY TCNI.CreatedOn DESC
	   END	  
	   ELSE IF (@LeadValue=7 OR @LeadValue=15)--7=Lost , 15 = Rejected
			BEGIN
			 SELECT TCNI.TC_NewCarInquiriesId, 
					   TC.Id , 
					   (ISNULL(TC.Salutation,'')+' '+ TC.CustomerName+' '+ISNULL(TC.LastName,'')) CustomerName, 
					   TC.Email, 
					   TC.Mobile, 
					   COALESCE(C.Name,TC.Location) AS Location, -- Added by vicky gupta on 24/11/2015 
					   TCNI.CreatedOn AS EntryDate, 
					   TU.UserName,
					   VW.Car AS Car,
					   TCIS.Source AS 'Source Name',
					   CASE(TCIL.TC_InquiryStatusId)
								WHEN 1 THEN 'Hot'
								WHEN 2 THEN 'Warm'
								WHEN 3 THEN 'Normal'
					   END AS 'Eagerness',
					   CASE
							WHEN (TCNI.TC_LeadDispositionID IS NULL)  THEN 'Active'
							WHEN (TCNI.TC_LeadDispositionID IN (4,41)) THEN 'Closed'
							WHEN (TCNI.TC_LeadDispositionID IS NOT NULL AND TCNI.TC_LeadDispositionID <> 4) THEN 'Closed'
					   END AS 'Status',
					   (SELECT TOP 1 CL.ActionComments FROM TC_Calls CL WITH(NOLOCK) WHERE CL.TC_LeadId = TCIL.TC_LeadId AND CL.IsActionTaken = 1 AND ISNULL(CL.TC_CallActionId,0) <> 2 ORDER BY CL.TC_CallsId DESC) 
								AS LastComment,
					   --AC.ScheduledOn AS NextFollowupDate,
					   TCNI.CompanyName,
					   TCNI.TDDate,
					   TCNI.BookingEventDate AS BookingEntryDate,
					   TCNI.BookingDate,
					   '' AS BookingAmount,
					   '' AS PromisedDeliveryDate,
					   '' AS DeliveryEntryDate,
					   '' AS DeliveryDate,
					   '' AS PanNo,
					   '' AS ChassisNumber,
					   TLD.Name AS LostReason,
					   TSD.SubDispositionName AS LostSubdisposition,
					   V.Make,
					   V.Model,
					   V.Version,
					   '' AS InsuranceCoverNumber,
					   '' AS EngineNumber,
					   '' AS InvoiceNumber,
					   '' AS RegistrationNo,
					   ISNULL(TC.UniqueCustomerId,'') AS UniqueCustomerId, --Modified by: Vivek singh on 18-12-2013 added new columns
					   ISNULL(TC.Address,'') AS Address,
					   --ISNULL(TCIL.CreatedDate,'-') AS InquiryEntryDate,		
					   (SELECT COUNT(*) FROM TC_TDCalendar TD WITH(NOLOCK) WHERE TD.TC_NewCarInquiriesId= TCNI.TC_NewCarInquiriesId)  AS NumberOfTDs,
					   '' AS BookingModelYear,
					   '' AS BookingModelColour,
					   '' AS RetailDate,
					   '' AS RetailCustomerName,
					   '' AS RetailCustomerMobileNo,
					   '' AS RetailCustomerEmail,
					   TCNI.DispositionDate AS LostDate,
					   ISNULL(VEX.Make,'') AS ExchangeCarMake,
					   ISNULL(VEX.Model,'') AS ExchangeCarModel,
					   ISNULL(VEX.Version,'') AS ExchangeCarversion,
					   TENC.MakeYear AS ExchangeCarYear,
					   ISNULL(TENC.Kms,'') AS ExchangeCarKMdriven,
					   ISNULL(TENC.ExpectedPrice,'') AS ExchangeCarExpectedPrice,
					   '' AS Offer,
					   '' AS Finance,
					   TCIL.TC_UserId,
					   TCIL.TC_LeadId,
					   TCIL.TC_LeadStageId, --Tejashree Patil on 16 Jun 2014
					   --CASE 
					   --WHEN (TCIL.TC_LeadDispositionID = 88) THEN (SELECT TOP 1 DispositionReason FROM TC_DispositionLog WITH(NOLOCK) WHERE InqOrLeadId = TCIL.TC_LeadId AND DispositionReason IS NOT NULL ORDER BY EventCreatedOn DESC)
					   --WHEN (TCIL.TC_LeadDispositionID IS NOT NULL AND  TCIL.TC_LeadDispositionID <> 88) THEN (SELECT TOP 1 ActionComments FROM TC_Calls WITH(NOLOCK) WHERE TC_LeadId = TCIL.TC_LeadId AND IsActionTaken = 1 AND ISNULL(TC_CallActionId,0) <> 2 AND ISNULL(CallType,0) IN (1,2) ORDER BY TC_CallsId DESC) 
					   --END AS LastCallComment --Afrose
                        TCNI.TC_LeadDispositionReason AS LeadDispositionReason, TCNI.DMSScreenShotHostUrl, TCNI.DMSScreenShotUrl--Added by Kritika Choudhary on 4th feb 2016
   				FROM TC_CustomerDetails AS TC   WITH(NOLOCK)  
					 JOIN  TC_Lead          AS TL   WITH(NOLOCK) ON TL.TC_CustomerId= TC.Id
					 JOIN  TC_InquiriesLead AS TCIL WITH(NOLOCK) ON TL.TC_LeadId=TCIL.TC_LeadId 
					 JOIN  TC_LeadDisposition AS LD WITH(NOLOCK) ON LD.TC_LeadDispositionId = TCIL.TC_LeadDispositionID
					 JOIN  TC_InquirySource AS TCIS WITH(NOLOCK) ON TCIS.Id = TL.TC_InquirySourceId 
							  AND TCIL.TC_LeadInquiryTypeId=3
							  AND  TL.BranchId	= @BranchId 
							  AND  TCIL.BranchId=  @BranchId 
							  AND ((TCIL.TC_UserId IN (SELECT ListMember AS UserId FROM fnSplitCSV(@UserIdList)))OR @UserIdList IS NULL)
							  --AND TCIL.TC_LeadStageId=3
					 JOIN TC_NewCarInquiries AS TCNI WITH(NOLOCK)   ON TCNI.TC_InquiriesLeadId=TCIL.TC_InquiriesLeadId 
																	  AND TCNI.TC_LeadDispositionID <> 4  
																	  AND ((TCIL.TC_LeadDispositionID NOT IN (1,3,70,71,74) AND @LeadValue = 7)   --lost
																	                  OR (TCIL.TC_LeadDispositionID IN(1,3,70,71,74) AND @LeadValue = 15)   --rejected
																		   )
																	  AND TCNI.TC_LeadDispositionId IS NOT NULL
																	  AND TL.LeadClosedDate BETWEEN @FromDate AND @ToDate -- Modified By: Nilesh Utture on 25th July, 2013 11:49 AM
																	  AND ISNULL(TCNI.MostInterested,0) = 1
					 INNER JOIN  TC_Users         AS TU   WITH(NOLOCK)  ON TU.Id = TCIL.TC_UserId AND ISNULL(TU.lvl,0) <> 0 /*AND TU.IsActive = 1*/ -- Modified By: Nilesh Utture on 19th July, 2013 6:38 PM
					 INNER JOIN TC_LeadDisposition TLD(NOLOCK) ON TLD.TC_LeadDispositionId = TCNI.TC_LeadDispositionID
					 --INNER JOIN vwMMV VW WITH(NOLOCK) ON VW.VersionId = TCNI.VersionId
					 LEFT JOIN vwAllMMV VW WITH(NOLOCK) ON VW.VersionId = TCNI.VersionId AND VW.ApplicationId=@ApplicationId
					 --LEFT JOIN TC_NewCarBooking  AS NB   WITH(NOLOCK) ON NB.TC_NewCarInquiriesId = TCNI.TC_NewCarInquiriesId
					 LEFT JOIN TC_LeadSubDisposition TSD(NOLOCK) ON TSD.TC_LeadSubDispositionId = TCNI.TC_SubDispositionId
					 LEFT JOIN Cities C WITH(NOLOCK) ON C.Id = TCNI.CityId  
					 --LEFT JOIN vwMMV V WITH(NOLOCK) ON V.VersionId = TCNI.LostVersionId 
					 LEFT JOIN vwAllMMV V WITH(NOLOCK) ON V.VersionId = TCNI.LostVersionId AND V.ApplicationId=@ApplicationId
					 LEFT JOIN TC_ExchangeNewCar TENC WITH(NOLOCK) ON TENC.TC_NewCarInquiriesId=TCNI.TC_NewCarInquiriesId
					 --LEFT JOIN vwMMV VEX WITH(NOLOCK) ON VEX.VersionId = TENC.CarVersionId
					 LEFT JOIN vwAllMMV VEX WITH(NOLOCK) ON VEX.VersionId = TENC.CarVersionId AND VEX.ApplicationId=@ApplicationId
				ORDER BY TCNI.CreatedOn DESC
	   END  
	   ELSE IF (@LeadValue=9 OR @LeadValue=11 )--PendingTestDrive, Completed TD
			BEGIN
			 SELECT TCNI.TC_NewCarInquiriesId, 
					   TC.Id , 
					   (ISNULL(TC.Salutation,'')+' '+ TC.CustomerName+' '+ISNULL(TC.LastName,'')) CustomerName, 
					   TC.Email, 
					   TC.Mobile, 
					   COALESCE(C.Name,TC.Location) AS Location, -- Added by vicky gupta on 24/11/2015
					   TCNI.CreatedOn AS EntryDate, 
					   TU.UserName,
					   VW.Car AS Car,
					   TCIS.Source AS 'Source Name',
					   CASE(TCIL.TC_InquiryStatusId)
								WHEN 1 THEN 'Hot'
								WHEN 2 THEN 'Warm'
								WHEN 3 THEN 'Normal'
					   END AS 'Eagerness',
					   CASE
							WHEN (TCNI.TC_LeadDispositionID IS NULL)  THEN 'Active'
							WHEN (TCNI.TC_LeadDispositionID IN (4,41)) THEN 'Closed'
							WHEN (TCNI.TC_LeadDispositionID IS NOT NULL AND TCNI.TC_LeadDispositionID <> 4) THEN 'Closed'
					   END AS 'Status',
						AC.LastCallComment AS LastComment,
					   AC.ScheduledOn AS NextFollowupDate,
					   TCNI.CompanyName,
					   TCNI.TDDate,
					   TCNI.BookingEventDate AS BookingEntryDate,
					   TCNI.BookingDate,
					   NB.Payment AS BookingAmount,
					   NB.PrefDeliveryDate AS PromisedDeliveryDate,
					   NB.DeliveryEntryDate,
					   NB.DeliveryDate,
					   NB.PanNo,
					   NB.ChassisNumber,
					   TLD.Name AS LostReason,
					   TSD.SubDispositionName AS LostSubdisposition,
					   V.Make,
					   V.Model,
					   V.Version,
					   NB.InsuranceCoverNumber,
					   NB.EngineNumber,
					   NB.InvoiceNumber,
					   NB.RegistrationNo,
					   ISNULL(TC.UniqueCustomerId,'') AS UniqueCustomerId, --Modified by: Vivek singh on 18-12-2013 added new columns
					   ISNULL(TC.Address,'') AS Address,
					   --ISNULL(TCIL.CreatedDate,'-') AS InquiryEntryDate,		
					   (SELECT COUNT(*) FROM TC_TDCalendar TD WITH(NOLOCK)  WHERE TD.TC_NewCarInquiriesId= TCNI.TC_NewCarInquiriesId)  AS NumberOfTDs,
					   NB.ModelYear AS BookingModelYear,
					   --ISNULL(VC.Color,'')AS BookingModelColour,
					   ISNULL(VC.VersionColor,'')AS BookingModelColour,
					   NB.InvoiceDate RetailDate,
					  ISNULL(NB.BookingName,'') AS RetailCustomerName,          --06-02-2014 changed columns fetched for retailcustomername,mobile and email to bookingcustomername and booking mobile no
					   ISNULL(NB.BookingMobile,'') AS RetailCustomerMobileNo,
					   ISNULL(NB.Email,'') AS RetailCustomerEmail,             --06-02-2014 changed columns fetched for retailcustomername,mobile and email to bookingcustomername and booking mobile no
					   TCNI.DispositionDate AS LostDate,
					   ISNULL(VEX.Make,'') AS ExchangeCarMake,
					   ISNULL(VEX.Model,'') AS ExchangeCarModel,
					   ISNULL(VEX.Version,'') AS ExchangeCarversion,
					   TENC.MakeYear AS ExchangeCarYear,
					   ISNULL(TENC.Kms,'') AS ExchangeCarKMdriven,
					   ISNULL(TENC.ExpectedPrice,'') AS ExchangeCarExpectedPrice,
					   ISNULL(TCO.OfferName,'') AS Offer,
					    ISNULL(NB.IsLoanRequired,'') AS Finance,
					   TCIL.TC_UserId,
					   TCIL.TC_LeadId,
					   TCIL.TC_LeadStageId --Tejashree Patil on 16 Jun 2014
					   ,TCNI.TC_LeadDispositionReason AS LeadDispositionReason   --added by Ashwini Dhamankar on Jul 22,2016
				FROM TC_CustomerDetails AS TC   WITH(NOLOCK)  
					 --JOIN  TC_Lead          AS TL   WITH(NOLOCK) ON TL.TC_CustomerId= TC.Id --Tejashree Patil on 16 Jun 2014
					 --LEFT JOIN  TC_ActiveCalls   AS AC   WITH(NOLOCK) ON AC.TC_LeadId = TL.TC_LeadId
					 JOIN  TC_InquiriesLead AS TCIL WITH(NOLOCK) ON TC.Id=TCIL.TC_CustomerId
					 JOIN TC_NewCarInquiries AS TCNI WITH(NOLOCK)   ON TCNI.TC_InquiriesLeadId=TCIL.TC_InquiriesLeadId 
																	  AND ( ( TCNI.TDStatus <> 27 
																	          AND TCNI.TDStatus <> 28 AND @LeadValue=9 
																	          -------- Modified By: Nilesh Utture on 19-09-2013, showing cummulative Pending TD's till @ToDate
																	          AND TCNI.TDDate <= @ToDate
																	         )-- Modified BY: Nilesh Utture on 1st August, 2013  commented below 3 lines
																	          --Here using leadvalue for deciding the filter for pending/completed TD
																	         OR 
																	        ( TCNI.TDStatus = 28 AND @LeadValue=11  AND TCNI.TDDate BETWEEN @FromDate AND @ToDate)
																	       )
																		   AND ISNULL(TCNI.MostInterested,0) = 1
					 JOIN  TC_InquirySource AS TCIS WITH(NOLOCK) ON TCIS.Id = TCNI.TC_InquirySourceId 
							  AND TCIL.TC_LeadInquiryTypeId=3
							  AND  TC.BranchId	= @BranchId 
							  AND  TCIL.BranchId=  @BranchId 
							  AND ((TCIL.TC_UserId IN (SELECT ListMember AS UserId FROM fnSplitCSV(@UserIdList))) OR @UserIdList IS NULL)
					 --INNER JOIN TC_TDCalendar AS TCTD WITH (NOLOCK) ON  TCNI.TC_TDCalendarId=TCTD.TC_TDCalendarId 	
						--											  AND TCTD.TDDate<CONVERT(DATE,GETDATE())
						--											  AND ((TCTD.TDStatus<>27 AND TCTD.TDStatus<>28) OR TCTD.TDStatus IS NULL)										
					 INNER JOIN  TC_Users         AS TU   WITH(NOLOCK)  ON TU.Id = TCIL.TC_UserId AND ISNULL(TU.lvl,0) <> 0/*AND TU.IsActive = 1*/ -- Modified By: Nilesh Utture on 19th July, 2013 6:38 PM
					 --INNER JOIN vwMMV VW WITH(NOLOCK) ON VW.VersionId = TCNI.VersionId
					 LEFT JOIN vwAllMMV VW WITH(NOLOCK) ON VW.VersionId = TCNI.VersionId AND VW.ApplicationId=@ApplicationId
					 LEFT JOIN  TC_ActiveCalls   AS AC   WITH(NOLOCK) ON AC.TC_LeadId = TCIL.TC_LeadId
					 LEFT JOIN TC_NewCarBooking  AS NB   WITH(NOLOCK) ON NB.TC_NewCarInquiriesId = TCNI.TC_NewCarInquiriesId
					 LEFT JOIN TC_LeadDisposition TLD(NOLOCK) ON TLD.TC_LeadDispositionId = TCNI.TC_LeadDispositionID
					 LEFT JOIN TC_LeadSubDisposition TSD(NOLOCK) ON TSD.TC_LeadSubDispositionId = TCNI.TC_SubDispositionId
					 LEFT JOIN Cities C WITH(NOLOCK) ON C.Id = TCNI.CityId  
					 --LEFT JOIN vwMMV V WITH(NOLOCK) ON V.VersionId = TCNI.LostVersionId 
					 LEFT JOIN vwAllMMV V WITH(NOLOCK) ON V.VersionId = TCNI.LostVersionId AND V.ApplicationId=@ApplicationId
					 LEFT JOIN TC_ExchangeNewCar TENC WITH(NOLOCK) ON TENC.TC_NewCarInquiriesId=TCNI.TC_NewCarInquiriesId
					 --LEFT JOIN vwMMV VEX WITH(NOLOCK) ON VEX.VersionId = TENC.CarVersionId
					 LEFT JOIN vwAllMMV VEX WITH(NOLOCK) ON VEX.VersionId = TENC.CarVersionId AND VEX.ApplicationId=@ApplicationId
					 LEFT JOIN TC_Offers TCO WITH(NOLOCK) ON TCO.TC_OffersId = NB.TC_OffersId
					 --LEFT JOIN VersionColors VC WITH(NOLOCK) ON VC.ID= NB.CarColorId
					 LEFT JOIN vwAllVersionColors VC WITH(NOLOCK) ON VC.VersionColorsId= NB.CarColorId AND VC.ApplicationId=@ApplicationId
				ORDER BY TCNI.CreatedOn DESC
	   END 
	   ---------- Modified By: Nilesh Utture on 19-09-2013 Added Retail
	   ELSE IF (@LeadValue=10 OR @LeadValue = 13)--DeliveredLead, Retail
			BEGIN
			 SELECT TCNI.TC_NewCarInquiriesId, 
					   TC.Id , 
					   (ISNULL(TC.Salutation,'')+' '+ TC.CustomerName+' '+ISNULL(TC.LastName,'')) CustomerName, 
					   TC.Email, 
					   TC.Mobile, 
					   COALESCE(C.Name,TC.Location) AS Location, -- Added by vicky gupta on 24/11/2015 
					   TCNI.CreatedOn AS EntryDate, 
					   TU.UserName,
					   VW.Car AS Car,
					   TCIS.Source AS 'Source Name',
					   CASE(TCIL.TC_InquiryStatusId)
								WHEN 1 THEN 'Hot'
								WHEN 2 THEN 'Warm'
								WHEN 3 THEN 'Normal'
					   END AS 'Eagerness',
					   CASE
							WHEN (TCNI.TC_LeadDispositionID IS NULL)  THEN 'Active'
							WHEN (TCNI.TC_LeadDispositionID IN (4,41)) THEN 'Closed'
							WHEN (TCNI.TC_LeadDispositionID IS NOT NULL AND TCNI.TC_LeadDispositionID <> 4) THEN 'Closed'
					   END AS 'Status',
					   (SELECT TOP 1 CL.ActionComments FROM TC_Calls CL WITH(NOLOCK) WHERE CL.TC_LeadId = TCIL.TC_LeadId AND CL.IsActionTaken = 1 AND ISNULL(CL.TC_CallActionId,0) <> 2 ORDER BY CL.TC_CallsId DESC) 
								AS LastComment,
					   '' AS NextFollowupDate,
					   TCNI.CompanyName,
					   TCNI.TDDate,
					   TCNI.BookingEventDate AS BookingEntryDate,
					   TCNI.BookingDate,
					   NB.Payment AS BookingAmount,
					   NB.PrefDeliveryDate AS PromisedDeliveryDate,
					   NB.DeliveryEntryDate,
					   NB.DeliveryDate,
					   NB.PanNo,
					   NB.ChassisNumber,
					   TLD.Name AS LostReason,
					   TSD.SubDispositionName AS LostSubdisposition,
					   V.Make,
					   V.Model,
					   V.Version,
					   NB.InsuranceCoverNumber,
					   NB.EngineNumber,
					   NB.InvoiceNumber,
					   NB.RegistrationNo,
					   ISNULL(TC.UniqueCustomerId,'') AS UniqueCustomerId, --Modified by: Vivek singh on 18-12-2013 added new columns
					   ISNULL(TC.Address,'') AS Address,
					   --ISNULL(TCIL.CreatedDate,'-') AS InquiryEntryDate,		
					   (SELECT COUNT(*) FROM TC_TDCalendar TD WITH(NOLOCK) WHERE TD.TC_NewCarInquiriesId= TCNI.TC_NewCarInquiriesId)  AS NumberOfTDs,
					   NB.ModelYear AS BookingModelYear,
					   --ISNULL(VC.Color,'')AS BookingModelColour,
					   ISNULL(VC.VersionColor,'')AS BookingModelColour,
					   NB.InvoiceDate AS RetailDate,
					   ISNULL(NB.BookingName,'') AS RetailCustomerName,          --06-02-2014 changed columns fetched for retailcustomername,mobile and email to bookingcustomername and booking mobile no
					   ISNULL(NB.BookingMobile,'') AS RetailCustomerMobileNo,
					   ISNULL(NB.Email,'') AS RetailCustomerEmail,             --06-02-2014 changed columns fetched for retailcustomername,mobile and email to bookingcustomername and booking mobile no
					   TCNI.DispositionDate AS LostDate,
					   ISNULL(VEX.Make,'') AS ExchangeCarMake,
					   ISNULL(VEX.Model,'') AS ExchangeCarModel,
					   ISNULL(VEX.Version,'') AS ExchangeCarversion,
					   TENC.MakeYear AS ExchangeCarYear,
					   ISNULL(TENC.Kms,'') AS ExchangeCarKMdriven,
					   ISNULL(TENC.ExpectedPrice,'') AS ExchangeCarExpectedPrice,
					   ISNULL(TCO.OfferName,'') AS Offer,
					    ISNULL(NB.IsLoanRequired,'') AS Finance,
					   TCIL.TC_UserId,
					   TCIL.TC_LeadId,
					   TCIL.TC_LeadStageId --Tejashree Patil on 16 Jun 2014
					   ,TCNI.TC_LeadDispositionReason AS LeadDispositionReason   --added by Ashwini Dhamankar on Jul 22,2016
				FROM TC_CustomerDetails AS TC   WITH(NOLOCK)  
					 --JOIN  TC_Lead          AS TL   WITH(NOLOCK) ON TL.TC_CustomerId= TC.Id --Tejashree Patil on 16 Jun 2014
					 --LEFT JOIN  TC_ActiveCalls   AS AC   WITH(NOLOCK) ON AC.TC_LeadId = TL.TC_LeadId
					 JOIN  TC_InquiriesLead AS TCIL WITH(NOLOCK) ON TC.Id=TCIL.TC_CustomerId 
                     JOIN TC_NewCarInquiries AS TCNI WITH(NOLOCK)   ON TCNI.TC_InquiriesLeadId=TCIL.TC_InquiriesLeadId AND 
																	    ((@LeadValue = 10 AND TCNI.CarDeliveryStatus = 77 AND TCNI.CarDeliveryDate BETWEEN @FromDate AND @ToDate)
																		     OR
																		 (@LeadValue=13)
																		) -- Modified By: Nilesh Utture on 19-09-2013
																		AND ISNULL(TCNI.MostInterested,0) = 1
					 JOIN  TC_InquirySource AS TCIS WITH(NOLOCK) ON TCIS.Id = TCNI.TC_InquirySourceId 
							  AND  TCIL.TC_LeadInquiryTypeId=3
							  AND  TC.BranchId	= @BranchId 
							  AND  TCIL.BranchId=  @BranchId 
							  AND ((TCIL.TC_UserId IN (SELECT ListMember AS UserId FROM fnSplitCSV(@UserIdList)))OR @UserIdList IS NULL)
					--		  AND TCIL.TC_LeadDispositionID = 4  
					 INNER JOIN  TC_Users         AS TU   WITH(NOLOCK)  ON TU.Id = TCIL.TC_UserId AND ISNULL(TU.lvl,0) <> 0/*AND TU.IsActive = 1*/ -- Modified By: Nilesh Utture on 19th July, 2013 6:38 PM
					 --INNER JOIN vwMMV VW WITH(NOLOCK) ON VW.VersionId = TCNI.VersionId
					 LEFT JOIN vwAllMMV VW WITH(NOLOCK) ON VW.VersionId = TCNI.VersionId AND VW.ApplicationId=@ApplicationId
					 --LEFT JOIN   TC_ActiveCalls   AS AC   WITH(NOLOCK) ON AC.TC_LeadId = TL.TC_LeadId
					 LEFT JOIN TC_NewCarBooking  AS NB   WITH(NOLOCK) ON NB.TC_NewCarInquiriesId = TCNI.TC_NewCarInquiriesId
					 LEFT JOIN TC_LeadDisposition TLD(NOLOCK) ON TLD.TC_LeadDispositionId = TCNI.TC_LeadDispositionID
					 LEFT JOIN TC_LeadSubDisposition TSD(NOLOCK) ON TSD.TC_LeadSubDispositionId = TCNI.TC_SubDispositionId
					 LEFT JOIN Cities C WITH(NOLOCK) ON C.Id = TCNI.CityId  
					 --LEFT JOIN vwMMV V WITH(NOLOCK) ON V.VersionId = TCNI.LostVersionId
					 LEFT JOIN vwAllMMV V WITH(NOLOCK) ON V.VersionId = TCNI.LostVersionId AND V.ApplicationId=@ApplicationId
					 LEFT JOIN TC_ExchangeNewCar TENC WITH(NOLOCK) ON TENC.TC_NewCarInquiriesId=TCNI.TC_NewCarInquiriesId
					 --LEFT JOIN vwMMV VEX WITH(NOLOCK) ON VEX.VersionId = TENC.CarVersionId
					 LEFT JOIN vwAllMMV VEX WITH(NOLOCK) ON VEX.VersionId = TENC.CarVersionId AND VEX.ApplicationId=@ApplicationId
					 LEFT JOIN TC_Offers TCO WITH(NOLOCK) ON TCO.TC_OffersId = NB.TC_OffersId
					 --LEFT JOIN VersionColors VC WITH(NOLOCK) ON VC.ID= NB.CarColorId
					 LEFT JOIN vwAllVersionColors VC WITH(NOLOCK) ON VC.VersionColorsId= NB.CarColorId AND VC.ApplicationId=@ApplicationId
					 WHERE ((@LeadValue =10)  
								OR 
							(@LeadValue = 13 AND NB.InvoiceDate IS NOT NULL AND NB.InvoiceDate BETWEEN @FromDate AND @ToDate) 
						   )
				ORDER BY TCNI.CreatedOn DESC
				OPTION (RECOMPILE);
	   END
	   ELSE IF @LeadValue=12--TotalBookedLead
			BEGIN
			 SELECT TCNI.TC_NewCarInquiriesId, 
					   TC.Id , 
					   (ISNULL(TC.Salutation,'')+' '+ TC.CustomerName+' '+ISNULL(TC.LastName,'')) CustomerName, 
					   TC.Email, 
					   TC.Mobile, 
					   COALESCE(C.Name,TC.Location) AS Location, -- Added by vicky gupta on 24/11/2015 
					   TCNI.CreatedOn AS EntryDate, 
					   TU.UserName,
					   VW.Car AS Car,
					   TCIS.Source AS 'Source Name',
					   CASE(TCIL.TC_InquiryStatusId)
								WHEN 1 THEN 'Hot'
								WHEN 2 THEN 'Warm'
								WHEN 3 THEN 'Normal'
					   END AS 'Eagerness',
					   CASE
							WHEN (TCNI.TC_LeadDispositionID IS NULL)  THEN 'Active'
							WHEN (TCNI.TC_LeadDispositionID IN (4,41)) THEN 'Booked'
							WHEN (TCNI.TC_LeadDispositionID IS NOT NULL AND TCNI.TC_LeadDispositionID <> 4) THEN 'Closed'
					   END AS 'Status',
					   AC.LastCallComment AS LastComment, 
					   AC.ScheduledOn AS NextFollowupDate,
					   TCNI.CompanyName,
					   TCNI.TDDate,
					   TCNI.BookingEventDate AS BookingEntryDate,
					   TCNI.BookingDate,
					   NB.Payment AS BookingAmount,
					   NB.PrefDeliveryDate AS PromisedDeliveryDate,
					   NB.DeliveryEntryDate,
					   NB.DeliveryDate,
					   NB.PanNo,
					   NB.ChassisNumber,
					   TLD.Name AS LostReason,
					   TSD.SubDispositionName AS LostSubdisposition,
					   V.Make,
					   V.Model,
					   V.Version,
					   NB.InsuranceCoverNumber,
					   NB.EngineNumber,
					   NB.InvoiceNumber,
					   NB.RegistrationNo,
					   ISNULL(TC.UniqueCustomerId,'') AS UniqueCustomerId, --Modified by: Vivek singh on 18-12-2013 added new columns
					   ISNULL(TC.Address,'') AS Address,
					   --ISNULL(TCIL.CreatedDate,'-') AS InquiryEntryDate,		
					   (SELECT COUNT(*) FROM TC_TDCalendar TD WITH(NOLOCK) WHERE TD.TC_NewCarInquiriesId= TCNI.TC_NewCarInquiriesId)  AS NumberOfTDs,
					   NB.ModelYear AS BookingModelYear,
					   --ISNULL(VC.Color,'')AS BookingModelColour,
					   ISNULL(VC.VersionColor,'')AS BookingModelColour,
					   NB.InvoiceDate AS RetailDate,
					  ISNULL(NB.BookingName,'') AS RetailCustomerName,          --06-02-2014 changed columns fetched for retailcustomername,mobile and email to bookingcustomername and booking mobile no
					   ISNULL(NB.BookingMobile,'') AS RetailCustomerMobileNo,
					   ISNULL(NB.Email,'') AS RetailCustomerEmail,             --06-02-2014 changed columns fetched for retailcustomername,mobile and email to bookingcustomername and booking mobile no
					   TCNI.DispositionDate AS LostDate,
					   ISNULL(VEX.Make,'') AS ExchangeCarMake,
					   ISNULL(VEX.Model,'') AS ExchangeCarModel,
					   ISNULL(VEX.Version,'') AS ExchangeCarversion,
					   TENC.MakeYear AS ExchangeCarYear,
					   ISNULL(TENC.Kms,'') AS ExchangeCarKMdriven,
					   ISNULL(TENC.ExpectedPrice,'') AS ExchangeCarExpectedPrice,
					   ISNULL(TCO.OfferName,'') AS Offer,
					    ISNULL(NB.IsLoanRequired,'') AS Finance,
					   TCIL.TC_UserId,
					   TCIL.TC_LeadId,
					   TCIL.TC_LeadStageId --Tejashree Patil on 16 Jun 2014
					   ,TCNI.TC_LeadDispositionReason AS LeadDispositionReason   --added by Ashwini Dhamankar on Jul 22,2016
				FROM TC_CustomerDetails AS TC   WITH(NOLOCK)  
					 --JOIN  TC_Lead          AS TL   WITH(NOLOCK) ON TL.TC_CustomerId= TC.Id --Tejashree Patil on 16 Jun 2014
					 --LEFT JOIN  TC_ActiveCalls   AS AC   WITH(NOLOCK) ON AC.TC_LeadId = TL.TC_LeadId
					 JOIN  TC_InquiriesLead AS TCIL WITH(NOLOCK) ON TC.Id=TCIL.TC_CustomerId
					 JOIN TC_NewCarInquiries AS TCNI WITH(NOLOCK)   ON TCNI.TC_InquiriesLeadId=TCIL.TC_InquiriesLeadId AND TCNI.BookingStatus = 32
																	  AND TCNI.BookingDate BETWEEN @FromDate AND @ToDate 
																	  --AND ISNULL(TCNI.MostInterested,0) = 1
					 JOIN  TC_InquirySource AS TCIS WITH(NOLOCK) ON TCIS.Id = TCNI.TC_InquirySourceId 
							  AND TCIL.TC_LeadInquiryTypeId=3
							  AND  TC.BranchId	= @BranchId 
							  AND  TCIL.BranchId=  @BranchId 
							  AND ((TCIL.TC_UserId IN (SELECT ListMember AS UserId FROM fnSplitCSV(@UserIdList)))OR @UserIdList IS NULL)
							  AND TCIL.TC_LeadDispositionID IN (4,41)   --Uncommentd by : Ashwini Dhamankar on Feb 2,2016 (Because of count mismatch) 
					 INNER JOIN  TC_Users         AS TU   WITH(NOLOCK)  ON TU.Id = TCIL.TC_UserId AND ISNULL(TU.lvl,0) <> 0/*AND TU.IsActive = 1*/ 
					 --INNER JOIN vwMMV VW WITH(NOLOCK) ON VW.VersionId = TCNI.VersionId
					 LEFT JOIN vwAllMMV VW WITH(NOLOCK) ON VW.VersionId = TCNI.VersionId AND VW.ApplicationId=@ApplicationId
					 LEFT JOIN   TC_ActiveCalls   AS AC   WITH(NOLOCK) ON AC.TC_LeadId = TCIL.TC_LeadId
					 LEFT JOIN TC_NewCarBooking  AS NB   WITH(NOLOCK) ON NB.TC_NewCarInquiriesId = TCNI.TC_NewCarInquiriesId
					 LEFT JOIN TC_LeadDisposition TLD(NOLOCK) ON TLD.TC_LeadDispositionId = TCNI.TC_LeadDispositionID
					 LEFT JOIN TC_LeadSubDisposition TSD(NOLOCK) ON TSD.TC_LeadSubDispositionId = TCNI.TC_SubDispositionId
					 LEFT JOIN Cities C WITH(NOLOCK) ON C.Id = TCNI.CityId  
					 --LEFT JOIN vwMMV V WITH(NOLOCK) ON V.VersionId = TCNI.LostVersionId
					 LEFT JOIN vwAllMMV V WITH(NOLOCK) ON V.VersionId = TCNI.LostVersionId AND V.ApplicationId=@ApplicationId
					 LEFT JOIN TC_ExchangeNewCar TENC WITH(NOLOCK) ON TENC.TC_NewCarInquiriesId=TCNI.TC_NewCarInquiriesId
					 --LEFT JOIN vwMMV VEX WITH(NOLOCK) ON VEX.VersionId = TENC.CarVersionId
					 LEFT JOIN vwAllMMV VEX WITH(NOLOCK) ON VEX.VersionId = TENC.CarVersionId AND VEX.ApplicationId=@ApplicationId
					 LEFT JOIN TC_Offers TCO WITH(NOLOCK) ON TCO.TC_OffersId = NB.TC_OffersId
					 --LEFT JOIN VersionColors VC WITH(NOLOCK) ON VC.ID= NB.CarColorId 
					 LEFT JOIN vwAllVersionColors VC WITH(NOLOCK) ON VC.VersionColorsId= NB.CarColorId AND VC.ApplicationId=@ApplicationId
				ORDER BY TCNI.CreatedOn DESC
	   END
	   ELSE IF ( @LeadValue =8) 
			BEGIN                -- Pending followup

			 SELECT  TCNI.TC_NewCarInquiriesId, 
					   TC.Id , 
					   (ISNULL(TC.Salutation,'')+' '+ TC.CustomerName+' '+ISNULL(TC.LastName,'')) CustomerName, 
					   TC.Email, 
					   TC.Mobile, 
					   COALESCE(C.Name,TC.Location) AS Location, -- Added by vicky gupta on 24/11/2015
					   TCNI.CreatedOn AS EntryDate, 
					   TU.UserName,
					   VW.Car AS Car,
					   TCIS.Source AS 'Source Name',
					   CASE(TCIL.TC_InquiryStatusId)
								WHEN 1 THEN 'Hot'
								WHEN 2 THEN 'Warm'
								WHEN 3 THEN 'Normal'
					   END AS 'Eagerness',
					   CASE
							WHEN (TCNI.TC_LeadDispositionID IS NULL)  THEN 'Active'
							WHEN (TCNI.TC_LeadDispositionID IN (4,41)) THEN 'Closed'
							WHEN (TCNI.TC_LeadDispositionID IS NOT NULL AND TCNI.TC_LeadDispositionID <> 4) THEN 'Closed'
					   END AS 'Status',
					   AC.LastCallComment AS LastComment, 
					   AC.ScheduledOn AS NextFollowupDate,
					   TCNI.CompanyName,
					   TCNI.TDDate,
					   TCNI.BookingEventDate AS BookingEntryDate,
					   TCNI.BookingDate,
					   NB.Payment AS BookingAmount,
					   NB.PrefDeliveryDate AS PromisedDeliveryDate,
					   NB.DeliveryEntryDate,
					   NB.DeliveryDate,
					   NB.PanNo,
					   NB.ChassisNumber,
					   TLD.Name AS LostReason,
					   TSD.SubDispositionName AS LostSubdisposition,
					   V.Make,
					   V.Model,
					   V.Version,
					   NB.InsuranceCoverNumber,
					   NB.EngineNumber,
					   NB.InvoiceNumber,
					   NB.RegistrationNo,
					   ISNULL(TC.UniqueCustomerId,'') AS UniqueCustomerId, --Modified by: Vivek singh on 18-12-2013 added new columns
					   ISNULL(TC.Address,'') AS Address,
					   --ISNULL(TCIL.CreatedDate,'-') AS InquiryEntryDate,		
					   (SELECT COUNT(*) FROM TC_TDCalendar TD WITH(NOLOCK) WHERE TD.TC_NewCarInquiriesId= TCNI.TC_NewCarInquiriesId)  AS NumberOfTDs,
					   NB.ModelYear AS BookingModelYear,
					   --ISNULL(VC.Color,'')AS BookingModelColour,
					   ISNULL(VC.VersionColor,'')AS BookingModelColour,
					   NB.InvoiceDate AS RetailDate,
					    ISNULL(NB.BookingName,'') AS RetailCustomerName,          --06-02-2014 changed columns fetched for retailcustomername,mobile and email to bookingcustomername and booking mobile no
					   ISNULL(NB.BookingMobile,'') AS RetailCustomerMobileNo,
					   ISNULL(NB.Email,'') AS RetailCustomerEmail,             --06-02-2014 changed columns fetched for retailcustomername,mobile and email to bookingcustomername and booking mobile no
					   TCNI.DispositionDate AS LostDate,
					   ISNULL(VEX.Make,'') AS ExchangeCarMake,
					   ISNULL(VEX.Model,'') AS ExchangeCarModel,
					   ISNULL(VEX.Version,'') AS ExchangeCarversion,
					   TENC.MakeYear AS ExchangeCarYear,
					   ISNULL(TENC.Kms,'') AS ExchangeCarKMdriven,
					   ISNULL(TENC.ExpectedPrice,'') AS ExchangeCarExpectedPrice,
					   ISNULL(TCO.OfferName,'') AS Offer,
					    ISNULL(NB.IsLoanRequired,'') AS Finance,
					   TCIL.TC_UserId,
					   TCIL.TC_LeadId,
					   TCIL.TC_LeadStageId --Tejashree Patil on 16 Jun 2014
					   ,TCNI.TC_LeadDispositionReason AS LeadDispositionReason   --added by Ashwini Dhamankar on Jul 22,2016
				FROM TC_CustomerDetails AS TC   WITH(NOLOCK)  
					 --JOIN  TC_Lead          AS TL   WITH(NOLOCK) ON TL.TC_CustomerId= TC.Id --Tejashree Patil on 16 Jun 2014
					 --LEFT JOIN  TC_ActiveCalls   AS AC   WITH(NOLOCK) ON AC.TC_LeadId = TL.TC_LeadId
					 JOIN  TC_InquiriesLead AS TCIL WITH(NOLOCK) ON TC.Id=TCIL.TC_CustomerId 
					 LEFT JOIN TC_NewCarInquiries AS TCNI WITH(NOLOCK)   ON TCNI.TC_InquiriesLeadId=TCIL.TC_InquiriesLeadId AND ISNULL(TCNI.MostInterested,0) = 1
					 -------- below line commented by Nilesh utture on 19-09-2013 ------------
																	  -- AND (TCIL.CreatedDate BETWEEN @FromDate AND @ToDate) 
					 JOIN  TC_InquirySource AS TCIS WITH(NOLOCK) ON TCIS.Id = TCNI.TC_InquirySourceId 
							  AND TCIL.TC_LeadInquiryTypeId=3
							  AND  TC.BranchId	= @BranchId 
							  AND  TCIL.BranchId=  @BranchId 
							  AND ((TCIL.TC_UserId IN (SELECT ListMember AS UserId FROM fnSplitCSV(@UserIdList)))OR @UserIdList IS NULL)
							 --AND TCIL.TC_LeadStageId <> 3 
							 AND ISNULL(TCNI.TC_LeadDispositionID,0) <> 4  --added by : Nilima More On 4th Oct 2016,Modified pending follow up condition.
							--  AND ( TCNI.TC_LeadDispositionId =4  or TCNI.TC_LeadDispositionID IS NULL)
							 AND (ISNULL(TCIL.TC_InquiryStatusId,0 )= @InquiryStatusID OR @InquiryStatusID IS NULL)
							--  AND ISNULL(TCNI.CarDeliveryStatus,0)<>77  --added by : Nilima More On 4th Oct 2016,Modified pending follow up condition.
					 INNER JOIN  TC_Users         AS TU   WITH(NOLOCK)  ON TU.Id = TCIL.TC_UserId AND ISNULL(TU.lvl,0) <> 0/*AND TU.IsActive = 1*/ -- Modified By: Nilesh Utture on 19th July, 2013 6:38 PM
					 -------- Modified By: Nilesh Utture on 19-09-2013, showing cummulative Pending Followup's till @ToDate based on latest followup date
					 LEFT JOIN  TC_ActiveCalls   AS AC   WITH(NOLOCK) ON AC.TC_LeadId = TCIL.TC_LeadId --AND  AC.ScheduledOn <= @ToDate
					 --INNER JOIN vwMMV VW WITH(NOLOCK) ON VW.VersionId = TCNI.VersionId
					 LEFT JOIN vwAllMMV VW WITH(NOLOCK) ON VW.VersionId = TCNI.VersionId AND VW.ApplicationId=@ApplicationId
					 LEFT JOIN TC_NewCarBooking  AS NB   WITH(NOLOCK) ON NB.TC_NewCarInquiriesId = TCNI.TC_NewCarInquiriesId
					 LEFT JOIN TC_LeadDisposition TLD(NOLOCK) ON TLD.TC_LeadDispositionId = TCNI.TC_LeadDispositionID
					 LEFT JOIN TC_LeadSubDisposition TSD(NOLOCK) ON TSD.TC_LeadSubDispositionId = TCNI.TC_SubDispositionId
					 LEFT JOIN Cities C WITH(NOLOCK) ON C.Id = TCNI.CityId  
					 --LEFT JOIN vwMMV V WITH(NOLOCK) ON V.VersionId = TCNI.LostVersionId 
					 LEFT JOIN vwAllMMV V WITH(NOLOCK) ON V.VersionId = TCNI.LostVersionId AND V.ApplicationId=@ApplicationId
					 LEFT JOIN TC_ExchangeNewCar TENC WITH(NOLOCK) ON TENC.TC_NewCarInquiriesId=TCNI.TC_NewCarInquiriesId
					 --LEFT JOIN vwMMV VEX WITH(NOLOCK) ON VEX.VersionId = TENC.CarVersionId
					 LEFT JOIN vwAllMMV VEX WITH(NOLOCK) ON VEX.VersionId = TENC.CarVersionId AND VEX.ApplicationId=@ApplicationId
					 LEFT JOIN TC_Offers TCO WITH(NOLOCK) ON TCO.TC_OffersId = NB.TC_OffersId
					 --LEFT JOIN VersionColors VC WITH(NOLOCK) ON VC.ID= NB.CarColorId 
					 LEFT JOIN vwAllVersionColors VC WITH(NOLOCK) ON VC.VersionColorsId= NB.CarColorId AND VC.ApplicationId=@ApplicationId
					 WHERE DATEDIFF(DD,@ToDate, AC.ScheduledOn) < 0-- AC.ScheduledOn < @ToDate
				ORDER BY TCNI.CreatedOn DESC
	   END
	    ---------- Modified By: Nilesh Utture on 19-09-2013
	   ELSE IF (@LeadValue = 14) --Pending Deliveries
			BEGIN                                                                             
			 SELECT  TCNI.TC_NewCarInquiriesId, 
					   TC.Id , 
					   (ISNULL(TC.Salutation,'')+' '+ TC.CustomerName+' '+ISNULL(TC.LastName,'')) CustomerName, 
					   TC.Email, 
					   TC.Mobile, 
					   COALESCE(C.Name,TC.Location) AS Location, -- Added by vicky gupta on 24/11/2015
					   TCNI.CreatedOn AS EntryDate, 
					   TU.UserName,
					   VW.Car AS Car,
					   TCIS.Source AS 'Source Name',
					   CASE(TCIL.TC_InquiryStatusId)
								WHEN 1 THEN 'Hot'
								WHEN 2 THEN 'Warm'
								WHEN 3 THEN 'Normal'
					   END AS 'Eagerness',
					   CASE
							WHEN (TCNI.TC_LeadDispositionID IS NULL)  THEN 'Active'
							WHEN (TCNI.TC_LeadDispositionID IN (4,41)) THEN 'Closed'
							WHEN (TCNI.TC_LeadDispositionID IS NOT NULL AND TCNI.TC_LeadDispositionID <> 4) THEN 'Closed'
					   END AS 'Status',
					   AC.LastCallComment AS LastComment,
					   AC.ScheduledOn AS NextFollowupDate,
					   TCNI.CompanyName,
					   TCNI.TDDate,
					   TCNI.BookingEventDate AS BookingEntryDate,
					   TCNI.BookingDate,
					   NB.Payment AS BookingAmount,
					   NB.PrefDeliveryDate AS PromisedDeliveryDate,
					   NB.DeliveryEntryDate,
					   NB.DeliveryDate,
					   NB.PanNo,
					   NB.ChassisNumber,
					   TLD.Name AS LostReason,
					   TSD.SubDispositionName AS LostSubdisposition,
					   V.Make,
					   V.Model,
					   V.Version,
					   NB.InsuranceCoverNumber,
					   NB.EngineNumber,
					   NB.InvoiceNumber,
					   NB.RegistrationNo,
					   ISNULL(TC.UniqueCustomerId,'') AS UniqueCustomerId, --Modified by: Vivek singh on 18-12-2013 added new columns
					   ISNULL(TC.Address,'') AS Address,
					   --ISNULL(TCIL.CreatedDate,'-') AS InquiryEntryDate,		
					   (SELECT COUNT(*) FROM TC_TDCalendar TD WITH(NOLOCK) WHERE TD.TC_NewCarInquiriesId= TCNI.TC_NewCarInquiriesId)  AS NumberOfTDs,
					  NB.ModelYear AS BookingModelYear,
					   --ISNULL(VC.Color,'')AS BookingModelColour,
					   ISNULL(VC.VersionColor,'')AS BookingModelColour,
					   NB.InvoiceDate AS RetailDate,
					  ISNULL(NB.BookingName,'') AS RetailCustomerName,          --06-02-2014 changed columns fetched for retailcustomername,mobile and email to bookingcustomername and booking mobile no
					   ISNULL(NB.BookingMobile,'') AS RetailCustomerMobileNo,
					   ISNULL(NB.Email,'') AS RetailCustomerEmail,             --06-02-2014 changed columns fetched for retailcustomername,mobile and email to bookingcustomername and booking mobile no
					   TCNI.DispositionDate AS LostDate,
					   ISNULL(VEX.Make,'') AS ExchangeCarMake,
					   ISNULL(VEX.Model,'') AS ExchangeCarModel,
					   ISNULL(VEX.Version,'') AS ExchangeCarversion,
					   TENC.MakeYear AS ExchangeCarYear,
					   ISNULL(TENC.Kms,'') AS ExchangeCarKMdriven,
					   ISNULL(TENC.ExpectedPrice,'') AS ExchangeCarExpectedPrice,
					   ISNULL(TCO.OfferName,'') AS Offer,
					   ISNULL(NB.IsLoanRequired,'') AS Finance,
					   TCIL.TC_UserId,
					   TCIL.TC_LeadId,
					   TCIL.TC_LeadStageId --Tejashree Patil on 16 Jun 2014
					   ,TCNI.TC_LeadDispositionReason AS LeadDispositionReason   --added by Ashwini Dhamankar on Jul 22,2016
				FROM TC_CustomerDetails AS TC   WITH(NOLOCK)  
					 --JOIN  TC_Lead          AS TL   WITH(NOLOCK) ON TL.TC_CustomerId= TC.Id --Tejashree Patil on 16 Jun 2014
					 --LEFT JOIN  TC_ActiveCalls   AS AC   WITH(NOLOCK) ON AC.TC_LeadId = TL.TC_LeadId
					 JOIN  TC_InquiriesLead AS TCIL WITH(NOLOCK) ON TC.Id=TCIL.TC_CustomerId 
					 JOIN TC_NewCarInquiries AS TCNI WITH(NOLOCK)   ON TCNI.TC_InquiriesLeadId=TCIL.TC_InquiriesLeadId AND ISNULL(TCNI.MostInterested,0) = 1
					 JOIN  TC_InquirySource AS TCIS WITH(NOLOCK) ON TCIS.Id = TCNI.TC_InquirySourceId 
							  AND TCIL.TC_LeadInquiryTypeId=3
							  AND  TC.BranchId	= @BranchId 
							  AND  TCIL.BranchId=  @BranchId 
							  AND ((TCIL.TC_UserId IN (SELECT ListMember AS UserId FROM fnSplitCSV(@UserIdList)))OR @UserIdList IS NULL)
							  --AND TCIL.TC_LeadStageId <> 3 
							 -- AND ISNULL(TCNI.TC_LeadDispositionID,0) <> 4  
							  --AND  ISNULL(TCNI.TC_LeadDispositionId,0)= 4
					 INNER JOIN  TC_Users         AS TU   WITH(NOLOCK)  ON TU.Id = TCIL.TC_UserId AND ISNULL(TU.lvl,0) <> 0/*AND TU.IsActive = 1*/ 
					 INNER JOIN  TC_ActiveCalls   AS AC   WITH(NOLOCK) ON AC.TC_LeadId = TCIL.TC_LeadId
					 --INNER JOIN vwMMV VW WITH(NOLOCK) ON VW.VersionId = TCNI.VersionId
					 LEFT JOIN vwAllMMV VW WITH(NOLOCK) ON VW.VersionId = TCNI.VersionId AND VW.ApplicationId=@ApplicationId
					 LEFT JOIN TC_NewCarBooking  AS NB   WITH(NOLOCK) ON NB.TC_NewCarInquiriesId = TCNI.TC_NewCarInquiriesId 
					 LEFT JOIN TC_LeadDisposition TLD(NOLOCK) ON TLD.TC_LeadDispositionId = TCNI.TC_LeadDispositionID
					 LEFT JOIN TC_LeadSubDisposition TSD(NOLOCK) ON TSD.TC_LeadSubDispositionId = TCNI.TC_SubDispositionId
					 LEFT JOIN Cities C WITH(NOLOCK) ON C.Id = TCNI.CityId  
					 --LEFT JOIN vwMMV V WITH(NOLOCK) ON V.VersionId = TCNI.LostVersionId 
					 LEFT JOIN vwAllMMV V WITH(NOLOCK) ON V.VersionId = TCNI.LostVersionId AND V.ApplicationId=@ApplicationId
					 LEFT JOIN TC_ExchangeNewCar TENC WITH(NOLOCK) ON TENC.TC_NewCarInquiriesId=TCNI.TC_NewCarInquiriesId
					 --LEFT JOIN vwMMV VEX WITH(NOLOCK) ON VEX.VersionId = TENC.CarVersionId
					 LEFT JOIN vwAllMMV VEX WITH(NOLOCK) ON VEX.VersionId = TENC.CarVersionId AND VEX.ApplicationId=@ApplicationId
					 LEFT JOIN TC_Offers TCO WITH(NOLOCK) ON TCO.TC_OffersId = NB.TC_OffersId
					 --LEFT JOIN VersionColors VC WITH(NOLOCK) ON VC.ID= NB.CarColorId 
					 LEFT JOIN vwAllVersionColors VC WITH(NOLOCK) ON VC.VersionColorsId= NB.CarColorId AND VC.ApplicationId=@ApplicationId 
					 WHERE NB.InvoiceDate IS NOT NULL AND ISNULL(TCNI.CarDeliveryStatus,0)<>77 AND NB.InvoiceDate <= @ToDate

				ORDER BY TCNI.CreatedOn DESC
	   END
	   
	END

