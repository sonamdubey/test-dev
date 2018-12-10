IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_ReportLeadsCustomerDetails]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_ReportLeadsCustomerDetails]
GO

	-- Author -- Sachin Bharti(14th August 2013)
---Modified By: Manish on 10-09-2013 since we need to take only open lead added condition tc_leadstateid<>3 in live booked leads
-- Mofified By: Manish on 10-09-2013 condition commented since no date filter for live bookings
-- Modified By: Sachin on 13-09-2013 change in report implementing target
-- Modified By: Manish on 16-09-2013 changing the logic of capturing Retails. Consider retail where invoice date is not null
-- Modified By: Sachin Bharti on 20-09-2013 Apply @ToDate constraint for LiveBookings,PendingDeliveries,PendingTestDrive,PendingFollowUp
-- Modified by: Manish on 23-09-2013 adding codition "ISNULL(TLS.CarDeliveryStatus,0)<>77" in live booking leads for handling old records also before Retail release 
-- Modified BY: Sachin Bharti(26th Sep 2013) Added a parameter for VersionId 
-- Modified By: Tejashree Patil on 20 May 2014, Fetched Reatil Date.
--=====================================================================

CREATE PROCEDURE [dbo].[TC_ReportLeadsCustomerDetails] 
@DealerId	NUMERIC(18,0) = NULL,
@UserId		NUMERIC(18,0) = NULL,
@FromDate	DATETIME,
@ToDate		DATETIME,
@ModelId    INT	=	NULL,
@Type		TINYINT,
@VersionId	NUMERIC(18,0) = NULL   --Sachin Bharti(26th Sep 2013) Added a parameter for VersionId 
AS 
BEGIN
	--Query for total leads count
	IF @Type = 1
		BEGIN
			SELECT	DISTINCT 
					TC.Id ,TC.CustomerName,TC.Email,TC.Mobile, 
					TC.Location,TLS.CreatedDate AS EntryDate,TU.UserName,
					TLS.LatestCarName AS Car,
					TLS.LastCallComment AS LastComment,
					TLS.ScheduledOn AS NextFollowupDate,
					TLS.Source AS 'Source Name',
					TLS.Eagerness,VW.Make AS LostToMake ,VW.Model AS LostToModel,VW.Version AS LostToVersion,
					TLD.Name AS LostReason,TSD.SubDispositionName AS LostSubDispotion,
					CASE
						WHEN (TLS.TC_LeadDispositionID IS NULL)  THEN 'Active'
						WHEN (TLS.TC_LeadDispositionID = 4) THEN 'Booked'
						WHEN (TLS.TC_LeadDispositionID IS NOT NULL AND TLS.TC_LeadDispositionID <> 4) THEN 'Closed'
					END AS 'Status',
					TLS.TestDriveDate ,TLS.BookingEntryDate,TLS.BookingDate,TLS.BookingAmt,TLS.PromisedDeliveryDate,TLS.DeliveryEntryDate,
					TLS.CarDeliveryDate,TLS.PanNo,TLS.VinNO,TLS.CompanyName,TLS.EngineNumber,TLS.InsuranceCoverNumber,TLS.InvoiceNumber,TLS.RegistrationNo,
					TLS.InvoiceDate  -- Modified By: Tejashree Patil on 20 May 2014
					FROM TC_LeadBasedSummary TLS(NOLOCK)
					INNER JOIN TC_Lead TL(NOLOCK) ON TLS.TC_LeadId = TL.TC_LeadId
					INNER JOIN TC_CustomerDetails TC(NOLOCK) ON TL.TC_CustomerId = TC.Id
					INNER JOIN TC_Users TU(NOLOCK) ON TU.Id = TLS.TC_UsersId
					LEFT JOIN TC_LeadDisposition TLD(NOLOCK) ON TLD.TC_LeadDispositionId = TLS.InquiryDispositionId
					LEFT JOIN TC_LeadSubDisposition TSD(NOLOCK) ON TSD.TC_LeadDispositionId = TLS.InquirySubDispositionId 
					LEFT JOIN vwMMV VW(NOLOCK) ON VW.VersionId = TLS.LostVersionId
					WHERE (TLS.DealerId = @DealerId OR @DealerId IS NULL) 
					AND  (TLS.TC_UsersId = @UserId OR @UserId IS NULL)
					AND  TLS.CreatedDate BETWEEN @FromDate AND @ToDate -- Changed By Sachin Bharti(1st Oct 2013)
					AND (TLS.CarModelId = @ModelId OR @ModelId IS NULL)
					AND (TLS.CarVersionId = @VersionId OR @VersionId IS NULL) -- Added by Sachin Bharti(26th Sep 2013)
					ORDER BY TC.CustomerName
		END
	--Query for Active Hot Lead
	IF @Type = 2
		BEGIN
			SELECT	DISTINCT 
					TC.Id , 
					TC.CustomerName, 
					TC.Email, 
					TC.Mobile, 
					TC.Location, 
					TLS.CreatedDate AS EntryDate, 
					TU.UserName,
					TLS.LatestCarName AS Car,
					TLS.LastCallComment AS LastComment,
					TLS.ScheduledOn AS NextFollowupDate,
					TLS.Source AS 'Source Name',
					TLS.Eagerness,VW.Make AS LostToMake ,VW.Model AS LostToModel,VW.Version AS LostToVersion,
					TLD.Name AS LostReason,TSD.SubDispositionName AS LostSubDispotion,
					CASE
						WHEN (TLS.TC_LeadDispositionID IS NULL)  THEN 'Active'
						WHEN (TLS.TC_LeadDispositionID = 4) THEN 'Booked'
						WHEN (TLS.TC_LeadDispositionID IS NOT NULL AND TLS.TC_LeadDispositionID <> 4) THEN 'Closed'
					END AS 'Status',
					TLS.TestDriveDate ,TLS.BookingEntryDate,TLS.BookingDate,TLS.BookingAmt,TLS.PromisedDeliveryDate,TLS.DeliveryEntryDate,
					TLS.CarDeliveryDate,TLS.PanNo,TLS.VinNO,TLS.CompanyName,TLS.EngineNumber,TLS.InsuranceCoverNumber,TLS.InvoiceNumber,TLS.RegistrationNo,
					TLS.InvoiceDate  -- Modified By: Tejashree Patil on 20 May 2014
					FROM TC_LeadBasedSummary TLS(NOLOCK)
					INNER JOIN TC_Lead TL(NOLOCK) ON TLS.TC_LeadId = TL.TC_LeadId
					INNER JOIN  TC_CustomerDetails TC(NOLOCK) ON TL.TC_CustomerId = TC.Id
					INNER JOIN TC_Users TU(NOLOCK) ON TU.Id = TLS.TC_UsersId
					LEFT JOIN TC_LeadDisposition TLD(NOLOCK) ON TLD.TC_LeadDispositionId = TLS.InquiryDispositionId
					LEFT JOIN TC_LeadSubDisposition TSD(NOLOCK) ON TSD.TC_LeadDispositionId = TLS.InquirySubDispositionId 
					LEFT JOIN vwMMV VW(NOLOCK) ON VW.VersionId = TLS.LostVersionId
					WHERE (TLS.DealerId = @DealerId OR @DealerId IS NULL) 
					AND  (TLS.TC_UsersId = @UserId OR @UserId IS NULL)
					AND TLS.TC_LeadStageId<>3 AND ISNULL(TLS.TC_LeadDispositionID,0)<>4  AND TLS.EagernessId = 1
					AND TLS.CreatedDate BETWEEN @FromDate AND @ToDate AND (TLS.CarModelId = @ModelId OR @ModelId IS NULL)
					AND (TLS.CarVersionId = @VersionId OR @VersionId IS NULL) -- Added by Sachin Bharti(26th Sep 2013)
					ORDER BY TC.CustomerName
		END
	--Query for Active Warm Lead
	IF @Type = 3
		BEGIN
			SELECT	DISTINCT 
					TC.Id , 
					TC.CustomerName, 
					TC.Email, 
					TC.Mobile, 
					TC.Location, 
					TLS.CreatedDate AS EntryDate, 
					TU.UserName,
					TLS.LatestCarName AS Car,
					TLS.LastCallComment AS LastComment,
					TLS.ScheduledOn AS NextFollowupDate,
					TLS.Source AS 'Source Name',
					TLS.Eagerness,VW.Make AS LostToMake ,VW.Model AS LostToModel,VW.Version AS LostToVersion,
					TLD.Name AS LostReason,TSD.SubDispositionName AS LostSubDispotion,
					CASE
						WHEN (TLS.TC_LeadDispositionID IS NULL)  THEN 'Active'
						WHEN (TLS.TC_LeadDispositionID = 4) THEN 'Booked'
						WHEN (TLS.TC_LeadDispositionID IS NOT NULL AND TLS.TC_LeadDispositionID <> 4) THEN 'Closed'
					END AS 'Status',
					TLS.TestDriveDate ,TLS.BookingEntryDate,TLS.BookingDate,TLS.BookingAmt,TLS.PromisedDeliveryDate,TLS.DeliveryEntryDate,
					TLS.CarDeliveryDate,TLS.PanNo,TLS.VinNO,TLS.CompanyName,TLS.EngineNumber,TLS.InsuranceCoverNumber,TLS.InvoiceNumber,TLS.RegistrationNo,
					TLS.InvoiceDate  -- Modified By: Tejashree Patil on 20 May 2014
					FROM TC_LeadBasedSummary TLS(NOLOCK)
					INNER JOIN TC_Lead TL(NOLOCK) ON TLS.TC_LeadId = TL.TC_LeadId
					INNER JOIN  TC_CustomerDetails TC(NOLOCK) ON TL.TC_CustomerId = TC.Id
					INNER JOIN TC_Users TU(NOLOCK) ON TU.Id = TLS.TC_UsersId
					LEFT JOIN TC_LeadDisposition TLD(NOLOCK) ON TLD.TC_LeadDispositionId = TLS.InquiryDispositionId
					LEFT JOIN TC_LeadSubDisposition TSD(NOLOCK) ON TSD.TC_LeadDispositionId = TLS.InquirySubDispositionId 
					LEFT JOIN vwMMV VW(NOLOCK) ON VW.VersionId = TLS.LostVersionId
					WHERE (TLS.DealerId = @DealerId OR @DealerId IS NULL) 
					AND  (TLS.TC_UsersId = @UserId OR @UserId IS NULL)
					AND TLS.TC_LeadStageId<>3 AND ISNULL(TLS.TC_LeadDispositionID,0)<>4  AND TLS.EagernessId = 2
					AND TLS.CreatedDate BETWEEN @FromDate AND @ToDate  AND (TLS.CarModelId = @ModelId OR @ModelId IS NULL)
					AND (TLS.CarVersionId = @VersionId OR @VersionId IS NULL) -- Added by Sachin Bharti(26th Sep 2013)
					ORDER BY TC.CustomerName
		END
	--Query for Active Normal Leads
	IF @Type = 4
		BEGIN
			SELECT	DISTINCT 
					TC.Id , 
					TC.CustomerName, 
					TC.Email, 
					TC.Mobile, 
					TC.Location, 
					TLS.CreatedDate AS EntryDate, 
					TU.UserName,
					TLS.LatestCarName AS Car,
					TLS.LastCallComment AS LastComment,
					TLS.ScheduledOn AS NextFollowupDate,
					TLS.Source AS 'Source Name',
					TLS.Eagerness,VW.Make AS LostToMake ,VW.Model AS LostToModel,VW.Version AS LostToVersion,
					TLD.Name AS LostReason,TSD.SubDispositionName AS LostSubDispotion,
					CASE
						WHEN (TLS.TC_LeadDispositionID IS NULL)  THEN 'Active'
						WHEN (TLS.TC_LeadDispositionID = 4) THEN 'Booked'
						WHEN (TLS.TC_LeadDispositionID IS NOT NULL AND TLS.TC_LeadDispositionID <> 4) THEN 'Closed'
					END AS 'Status',
					TLS.TestDriveDate ,TLS.BookingEntryDate,TLS.BookingDate,TLS.BookingAmt,TLS.PromisedDeliveryDate,TLS.DeliveryEntryDate,
					TLS.CarDeliveryDate,TLS.PanNo,TLS.VinNO,TLS.CompanyName,TLS.EngineNumber,TLS.InsuranceCoverNumber,TLS.InvoiceNumber,TLS.RegistrationNo,
					TLS.InvoiceDate  -- Modified By: Tejashree Patil on 20 May 2014
					FROM TC_LeadBasedSummary TLS(NOLOCK)
					INNER JOIN TC_Lead TL(NOLOCK) ON TLS.TC_LeadId = TL.TC_LeadId
					INNER JOIN  TC_CustomerDetails TC(NOLOCK) ON TL.TC_CustomerId = TC.Id
					INNER JOIN TC_Users TU(NOLOCK) ON TU.Id = TLS.TC_UsersId
					LEFT JOIN TC_LeadDisposition TLD(NOLOCK) ON TLD.TC_LeadDispositionId = TLS.InquiryDispositionId
					LEFT JOIN TC_LeadSubDisposition TSD(NOLOCK) ON TSD.TC_LeadDispositionId = TLS.InquirySubDispositionId 
					LEFT JOIN vwMMV VW(NOLOCK) ON VW.VersionId = TLS.LostVersionId
					WHERE (TLS.DealerId = @DealerId OR @DealerId IS NULL) 
					AND  (TLS.TC_UsersId = @UserId OR @UserId IS NULL)
					AND TLS.TC_LeadStageId<>3 AND ISNULL(TLS.TC_LeadDispositionID,0)<>4  AND TLS.EagernessId = 3
					AND TLS.CreatedDate BETWEEN @FromDate AND @ToDate AND (TLS.CarModelId = @ModelId OR @ModelId IS NULL)
					AND (TLS.CarVersionId = @VersionId OR @VersionId IS NULL) -- Added by Sachin Bharti(26th Sep 2013)
					ORDER BY TC.CustomerName
		END
	--Query for Active Not Set Lead
	IF @Type = 5
		BEGIN
			SELECT	DISTINCT 
					TC.Id , 
					TC.CustomerName, 
					TC.Email, 
					TC.Mobile, 
					TC.Location, 
					TLS.CreatedDate AS EntryDate, 
					TU.UserName,
					TLS.LatestCarName AS Car,
					TLS.LastCallComment AS LastComment,
					TLS.ScheduledOn AS NextFollowupDate,
					TLS.Source AS 'Source Name',
					TLS.Eagerness,VW.Make AS LostToMake ,VW.Model AS LostToModel,VW.Version AS LostToVersion,
					TLD.Name AS LostReason,TSD.SubDispositionName AS LostSubDispotion,
					CASE
						WHEN (TLS.TC_LeadDispositionID IS NULL)  THEN 'Active'
						WHEN (TLS.TC_LeadDispositionID = 4) THEN 'Booked'
						WHEN (TLS.TC_LeadDispositionID IS NOT NULL AND TLS.TC_LeadDispositionID <> 4) THEN 'Closed'
					END AS 'Status',
					TLS.TestDriveDate ,TLS.BookingEntryDate,TLS.BookingDate,TLS.BookingAmt,TLS.PromisedDeliveryDate,TLS.DeliveryEntryDate,
					TLS.CarDeliveryDate,TLS.PanNo,TLS.VinNO,TLS.CompanyName,TLS.EngineNumber,TLS.InsuranceCoverNumber,TLS.InvoiceNumber,TLS.RegistrationNo,
					TLS.InvoiceDate  -- Modified By: Tejashree Patil on 20 May 2014
					FROM TC_LeadBasedSummary TLS(NOLOCK)
					INNER JOIN TC_Lead TL(NOLOCK) ON TLS.TC_LeadId = TL.TC_LeadId
					INNER JOIN  TC_CustomerDetails TC(NOLOCK) ON TL.TC_CustomerId = TC.Id
					INNER JOIN TC_Users TU(NOLOCK) ON TU.Id = TLS.TC_UsersId
					LEFT JOIN TC_LeadDisposition TLD(NOLOCK) ON TLD.TC_LeadDispositionId = TLS.InquiryDispositionId
					LEFT JOIN TC_LeadSubDisposition TSD(NOLOCK) ON TSD.TC_LeadDispositionId = TLS.InquirySubDispositionId 
					LEFT JOIN vwMMV VW(NOLOCK) ON VW.VersionId = TLS.LostVersionId
					WHERE (TLS.DealerId = @DealerId OR @DealerId IS NULL) 
					AND  (TLS.TC_UsersId = @UserId OR @UserId IS NULL)
					AND TLS.TC_LeadStageId<>3 AND ISNULL(TLS.TC_LeadDispositionID,0)<>4  AND ISNULL(TLS.EagernessId,0) = 0
					AND TLS.CreatedDate BETWEEN @FromDate AND @ToDate AND (TLS.CarModelId = @ModelId OR @ModelId IS NULL) 
					AND (TLS.CarVersionId = @VersionId OR @VersionId IS NULL) -- Added by Sachin Bharti(26th Sep 2013)
					ORDER BY TC.CustomerName
		END
	--Query for TD_Completed Leads
	IF @Type = 6
		BEGIN
			SELECT	DISTINCT 
					TC.Id , 
					TC.CustomerName, 
					TC.Email, 
					TC.Mobile, 
					TC.Location, 
					TLS.CreatedDate AS EntryDate, 
					TU.UserName,
					TLS.LatestCarName AS Car,
					TLS.LastCallComment AS LastComment,
					TLS.ScheduledOn AS NextFollowupDate,
					TLS.Source AS 'Source Name',
					TLS.Eagerness,VW.Make AS LostToMake ,VW.Model AS LostToModel,VW.Version AS LostToVersion,
					TLD.Name AS LostReason,TSD.SubDispositionName AS LostSubDispotion,TLS.TC_NewCarInquiriesId,
					CASE
						WHEN (TLS.TC_LeadDispositionID IS NULL)  THEN 'Active'
						WHEN (TLS.TC_LeadDispositionID = 4) THEN 'Booked'
						WHEN (TLS.TC_LeadDispositionID IS NOT NULL AND TLS.TC_LeadDispositionID <> 4) THEN 'Closed'
					END AS 'Status',
					TLS.TestDriveDate ,TLS.BookingEntryDate,TLS.BookingDate,TLS.BookingAmt,TLS.PromisedDeliveryDate,TLS.DeliveryEntryDate,
					TLS.CarDeliveryDate,TLS.PanNo,TLS.VinNO,TLS.CompanyName,TLS.EngineNumber,TLS.InsuranceCoverNumber,TLS.InvoiceNumber,TLS.RegistrationNo,
					TLS.InvoiceDate  -- Modified By: Tejashree Patil on 20 May 2014
					FROM TC_LeadBasedSummary TLS(NOLOCK)
					INNER JOIN TC_Lead TL(NOLOCK) ON TLS.TC_LeadId = TL.TC_LeadId
					INNER JOIN  TC_CustomerDetails TC(NOLOCK) ON TL.TC_CustomerId = TC.Id
					INNER JOIN TC_Users TU(NOLOCK) ON TU.Id = TLS.TC_UsersId
					LEFT JOIN TC_LeadDisposition TLD(NOLOCK) ON TLD.TC_LeadDispositionId = TLS.InquiryDispositionId
					LEFT JOIN TC_LeadSubDisposition TSD(NOLOCK) ON TSD.TC_LeadDispositionId = TLS.InquirySubDispositionId 
					LEFT JOIN vwMMV VW(NOLOCK) ON VW.VersionId = TLS.LostVersionId
					WHERE (TLS.DealerId = @DealerId OR @DealerId IS NULL) 
					AND  (TLS.TC_UsersId = @UserId OR @UserId IS NULL)
					AND TLS.TestDriveStatus = 28 AND TLS.TestDriveDate BETWEEN @FromDate AND @ToDate 
					AND (TLS.CarModelId = @ModelId OR @ModelId IS NULL)
					AND (TLS.CarVersionId = @VersionId OR @VersionId IS NULL) -- Added by Sachin Bharti(26th Sep 2013)
					ORDER BY TC.CustomerName
		END
	--Query for Booked Leads 
	IF @Type = 7
		BEGIN
			SELECT	DISTINCT 
					TC.Id , 
					TC.CustomerName, 
					TC.Email, 
					TC.Mobile, 
					TC.Location, 
					TLS.CreatedDate AS EntryDate, 
					TU.UserName,
					TLS.LatestCarName AS Car,
					TLS.LastCallComment AS LastComment,
					TLS.ScheduledOn AS NextFollowupDate,
					TLS.Source AS 'Source Name',
					TLS.Eagerness,VW.Make AS LostToMake ,VW.Model AS LostToModel,VW.Version AS LostToVersion,
					TLD.Name AS LostReason,TSD.SubDispositionName AS LostSubDispotion,TLS.TC_NewCarInquiriesId,
					CASE
						WHEN (TLS.TC_LeadDispositionID IS NULL)  THEN 'Active'
						WHEN (TLS.TC_LeadDispositionID = 4) THEN 'Booked'
						WHEN (TLS.TC_LeadDispositionID IS NOT NULL AND TLS.TC_LeadDispositionID <> 4) THEN 'Closed'
					END AS 'Status',
					TLS.TestDriveDate ,TLS.BookingEntryDate,TLS.BookingDate,TLS.BookingAmt,TLS.PromisedDeliveryDate,TLS.DeliveryEntryDate,
					TLS.CarDeliveryDate,TLS.PanNo,TLS.VinNO,TLS.CompanyName,TLS.EngineNumber,TLS.InsuranceCoverNumber,TLS.InvoiceNumber,TLS.RegistrationNo,
					TLS.InvoiceDate  -- Modified By: Tejashree Patil on 20 May 2014
					FROM TC_LeadBasedSummary TLS(NOLOCK)
					INNER JOIN TC_Lead TL(NOLOCK) ON TLS.TC_LeadId = TL.TC_LeadId
					INNER JOIN  TC_CustomerDetails TC(NOLOCK) ON TL.TC_CustomerId = TC.Id
					INNER JOIN TC_Users TU(NOLOCK) ON TU.Id = TLS.TC_UsersId
					LEFT JOIN TC_LeadDisposition TLD(NOLOCK) ON TLD.TC_LeadDispositionId = TLS.InquiryDispositionId
					LEFT JOIN TC_LeadSubDisposition TSD(NOLOCK) ON TSD.TC_LeadDispositionId = TLS.InquirySubDispositionId 
					LEFT JOIN vwMMV VW(NOLOCK) ON VW.VersionId = TLS.LostVersionId
					WHERE (TLS.DealerId = @DealerId OR @DealerId IS NULL) 
					AND  (TLS.TC_UsersId = @UserId OR @UserId IS NULL)
					AND TLS.BookingStatus=32 AND TLS.TC_LeadDispositionID=4  
					AND TLS.BookingDate BETWEEN @FromDate AND @ToDate AND (TLS.CarModelId = @ModelId OR @ModelId IS NULL)
					AND (TLS.CarVersionId = @VersionId OR @VersionId IS NULL) -- Added by Sachin Bharti(26th Sep 2013)
					ORDER BY TC.CustomerName
		END
	--Query for Delivered Leads
	IF @Type = 8
		BEGIN
			SELECT	DISTINCT 
					TC.Id , 
					TC.CustomerName, 
					TC.Email, 
					TC.Mobile, 
					TC.Location, 
					TLS.CreatedDate AS EntryDate, 
					TU.UserName,
					TLS.LatestCarName AS Car,
					TLS.LastCallComment AS LastComment,
					TLS.ScheduledOn AS NextFollowupDate,
					TLS.Source AS 'Source Name',
					TLS.Eagerness,VW.Make AS LostToMake ,VW.Model AS LostToModel,VW.Version AS LostToVersion,
					TLD.Name AS LostReason,TSD.SubDispositionName AS LostSubDispotion,TLS.TC_NewCarInquiriesId,
					CASE
						WHEN (TLS.TC_LeadDispositionID IS NULL)  THEN 'Active'
						WHEN (TLS.TC_LeadDispositionID = 4) THEN 'Booked'
						WHEN (TLS.TC_LeadDispositionID IS NOT NULL AND TLS.TC_LeadDispositionID <> 4) THEN 'Closed'
					END AS 'Status',
					TLS.TestDriveDate ,TLS.BookingEntryDate,TLS.BookingDate,TLS.BookingAmt,TLS.PromisedDeliveryDate,TLS.DeliveryEntryDate,
					TLS.CarDeliveryDate,TLS.PanNo,TLS.VinNO,TLS.CompanyName,TLS.EngineNumber,TLS.InsuranceCoverNumber,TLS.InvoiceNumber,TLS.RegistrationNo,
					TLS.InvoiceDate  -- Modified By: Tejashree Patil on 20 May 2014
					FROM TC_LeadBasedSummary TLS(NOLOCK)
					INNER JOIN TC_Lead TL(NOLOCK) ON TLS.TC_LeadId = TL.TC_LeadId
					INNER JOIN  TC_CustomerDetails TC(NOLOCK) ON TL.TC_CustomerId = TC.Id
					INNER JOIN TC_Users TU(NOLOCK) ON TU.Id = TLS.TC_UsersId
					LEFT JOIN TC_LeadDisposition TLD(NOLOCK) ON TLD.TC_LeadDispositionId = TLS.InquiryDispositionId
					LEFT JOIN TC_LeadSubDisposition TSD(NOLOCK) ON TSD.TC_LeadDispositionId = TLS.InquirySubDispositionId 
					LEFT JOIN vwMMV VW(NOLOCK) ON VW.VersionId = TLS.LostVersionId
					WHERE (TLS.DealerId = @DealerId OR @DealerId IS NULL) 
					AND  (TLS.TC_UsersId = @UserId OR @UserId IS NULL)
					AND TLS.CarDeliveryStatus = 77 AND TLS.CarDeliveryDate BETWEEN @FromDate AND @ToDate 
					AND (TLS.CarModelId = @ModelId OR @ModelId IS NULL)
					AND (TLS.CarVersionId = @VersionId OR @VersionId IS NULL) -- Added by Sachin Bharti(26th Sep 2013)
					ORDER BY TC.CustomerName
		END
	--Query for Lost Leads
	IF @Type = 9
		BEGIN
			SELECT	DISTINCT 
					TC.Id , 
					TC.CustomerName, 
					TC.Email, 
					TC.Mobile, 
					TC.Location, 
					TLS.CreatedDate AS EntryDate, 
					TU.UserName,
					TLS.LatestCarName AS Car,
					TLS.LastCallComment AS LastComment,
					TLS.ScheduledOn AS NextFollowupDate,
					TLS.Source AS 'Source Name',
					TLS.Eagerness,VW.Make AS LostToMake ,VW.Model AS LostToModel,VW.Version AS LostToVersion,
					TLD.Name AS LostReason,TSD.SubDispositionName AS LostSubDispotion,
					CASE
						WHEN (TLS.TC_LeadDispositionID IS NULL)  THEN 'Active'
						WHEN (TLS.TC_LeadDispositionID = 4) THEN 'Booked'
						WHEN (TLS.TC_LeadDispositionID IS NOT NULL AND TLS.TC_LeadDispositionID <> 4) THEN 'Closed'
					END AS 'Status',
					TLS.TestDriveDate ,TLS.BookingEntryDate,TLS.BookingDate,TLS.BookingAmt,TLS.PromisedDeliveryDate,TLS.DeliveryEntryDate,
					TLS.CarDeliveryDate,TLS.PanNo,TLS.VinNO,TLS.CompanyName,TLS.EngineNumber,TLS.InsuranceCoverNumber,TLS.InvoiceNumber,TLS.RegistrationNo,
					TLS.InvoiceDate  -- Modified By: Tejashree Patil on 20 May 2014
					FROM TC_LeadBasedSummary TLS(NOLOCK)
					INNER JOIN TC_Lead TL(NOLOCK) ON TLS.TC_LeadId = TL.TC_LeadId
					INNER JOIN  TC_CustomerDetails TC(NOLOCK) ON TL.TC_CustomerId = TC.Id
					INNER JOIN TC_Users TU(NOLOCK) ON TU.Id = TLS.TC_UsersId
					LEFT JOIN TC_LeadDisposition TLD(NOLOCK) ON TLD.TC_LeadDispositionId = TLS.InquiryDispositionId
					LEFT JOIN TC_LeadSubDisposition TSD(NOLOCK) ON TSD.TC_LeadDispositionId = TLS.InquirySubDispositionId 
					LEFT JOIN vwMMV VW(NOLOCK) ON VW.VersionId = TLS.LostVersionId
					WHERE (TLS.DealerId = @DealerId OR @DealerId IS NULL) 
					AND  (TLS.TC_UsersId = @UserId OR @UserId IS NULL)
					AND TLS.TC_LeadStageId=3 AND  TLS.TC_LeadDispositionID<>4 
					AND TLS.LeadClosedDate BETWEEN @FromDate AND @ToDate AND (TLS.CarModelId = @ModelId OR @ModelId IS NULL)
					AND (TLS.CarVersionId = @VersionId OR @VersionId IS NULL) -- Added by Sachin Bharti(26th Sep 2013)
					ORDER BY TC.CustomerName
		END
	--Query for Pending Followp Lead
	IF @Type = 10
		BEGIN
			SELECT	DISTINCT 
					TC.Id , 
					TC.CustomerName, 
					TC.Email, 
					TC.Mobile, 
					TC.Location, 
					TLS.CreatedDate AS EntryDate, 
					TU.UserName,
					TLS.LatestCarName AS Car,
					TLS.LastCallComment AS LastComment,
					TLS.ScheduledOn AS NextFollowupDate,
					TLS.Source AS 'Source Name',
					TLS.Eagerness,VW.Make AS LostToMake ,VW.Model AS LostToModel,VW.Version AS LostToVersion,
					TLD.Name AS LostReason,TSD.SubDispositionName AS LostSubDispotion,
					CASE
						WHEN (TLS.TC_LeadDispositionID IS NULL)  THEN 'Active'
						WHEN (TLS.TC_LeadDispositionID = 4) THEN 'Booked'
						WHEN (TLS.TC_LeadDispositionID IS NOT NULL AND TLS.TC_LeadDispositionID <> 4) THEN 'Closed'
					END AS 'Status',
					TLS.TestDriveDate ,TLS.BookingEntryDate,TLS.BookingDate,TLS.BookingAmt,TLS.PromisedDeliveryDate,TLS.DeliveryEntryDate,
					TLS.CarDeliveryDate,TLS.PanNo,TLS.VinNO,TLS.CompanyName,TLS.EngineNumber,TLS.InsuranceCoverNumber,TLS.InvoiceNumber,TLS.RegistrationNo,
					TLS.InvoiceDate  -- Modified By: Tejashree Patil on 20 May 2014
					FROM TC_LeadBasedSummary TLS(NOLOCK)
					INNER JOIN TC_Lead TL(NOLOCK) ON TLS.TC_LeadId = TL.TC_LeadId
					INNER JOIN  TC_CustomerDetails TC(NOLOCK) ON TL.TC_CustomerId = TC.Id
					INNER JOIN TC_Users TU(NOLOCK) ON TU.Id = TLS.TC_UsersId
					LEFT JOIN TC_LeadDisposition TLD(NOLOCK) ON TLD.TC_LeadDispositionId = TLS.InquiryDispositionId
					LEFT JOIN TC_LeadSubDisposition TSD(NOLOCK) ON TSD.TC_LeadDispositionId = TLS.InquirySubDispositionId 
					LEFT JOIN vwMMV VW(NOLOCK) ON VW.VersionId = TLS.LostVersionId
					WHERE (TLS.DealerId = @DealerId OR @DealerId IS NULL) 
					AND  (TLS.TC_UsersId = @UserId OR @UserId IS NULL)
					--AND TLS.ScheduledOn<GETDATE()  
					AND TLS.ScheduledOn <= @ToDate -- Added by Sachin Bharti on 20-09-2013
					AND TLS.TC_LeadStageId<>3 -- Added by Sachin Bharti on 20-09-2013
					AND (TLS.CarModelId = @ModelId OR @ModelId IS NULL)
					AND (TLS.CarVersionId = @VersionId OR @VersionId IS NULL) -- Added by Sachin Bharti(26th Sep 2013)
					ORDER BY TC.CustomerName
		END
	--Query for PendingTestDrive Leads
	IF @Type = 11
		BEGIN
			SELECT	DISTINCT 
					TC.Id , 
					TC.CustomerName, 
					TC.Email, 
					TC.Mobile, 
					TC.Location, 
					TLS.CreatedDate AS EntryDate, 
					TU.UserName,
					TLS.LatestCarName AS Car,
					TLS.LastCallComment AS LastComment,
					TLS.ScheduledOn AS NextFollowupDate,
					TLS.Source AS 'Source Name',
					TLS.Eagerness,VW.Make AS LostToMake ,VW.Model AS LostToModel,VW.Version AS LostToVersion,
					TLD.Name AS LostReason,TSD.SubDispositionName AS LostSubDispotion,
					CASE
						WHEN (TLS.TC_LeadDispositionID IS NULL)  THEN 'Active'
						WHEN (TLS.TC_LeadDispositionID = 4) THEN 'Booked'
						WHEN (TLS.TC_LeadDispositionID IS NOT NULL AND TLS.TC_LeadDispositionID <> 4) THEN 'Closed'
					END AS 'Status',
					TLS.TestDriveDate ,TLS.BookingEntryDate,TLS.BookingDate,TLS.BookingAmt,TLS.PromisedDeliveryDate,TLS.DeliveryEntryDate,
					TLS.CarDeliveryDate,TLS.PanNo,TLS.VinNO,TLS.CompanyName,TLS.EngineNumber,TLS.InsuranceCoverNumber,TLS.InvoiceNumber,TLS.RegistrationNo,
					TLS.InvoiceDate  -- Modified By: Tejashree Patil on 20 May 2014
					FROM TC_LeadBasedSummary TLS(NOLOCK)
					INNER JOIN TC_Lead TL(NOLOCK) ON TLS.TC_LeadId = TL.TC_LeadId
					INNER JOIN  TC_CustomerDetails TC(NOLOCK) ON TL.TC_CustomerId = TC.Id
					INNER JOIN TC_Users TU(NOLOCK) ON TU.Id = TLS.TC_UsersId
					LEFT JOIN TC_LeadDisposition TLD(NOLOCK) ON TLD.TC_LeadDispositionId = TLS.InquiryDispositionId
					LEFT JOIN TC_LeadSubDisposition TSD(NOLOCK) ON TSD.TC_LeadDispositionId = TLS.InquirySubDispositionId 
					LEFT JOIN vwMMV VW(NOLOCK) ON VW.VersionId = TLS.LostVersionId
					WHERE (TLS.DealerId = @DealerId OR @DealerId IS NULL) 
					AND  (TLS.TC_UsersId = @UserId OR @UserId IS NULL)
					--AND TLS.TestDriveDate<CONVERT(DATE,GETDATE()) 
					AND ((TLS.TestDriveStatus<>27 AND TLS.TestDriveStatus<>28) OR TLS.TestDriveStatus IS NULL) 
					AND TLS.TestDriveDate <= @ToDate -- Added by Sachin Bharti on 20-09-2013
					AND TLS.TC_LeadStageId<>3 -- Added by Sachin Bharti on 20-09-2013
					AND (TLS.CarModelId = @ModelId OR @ModelId IS NULL)
					AND (TLS.CarVersionId = @VersionId OR @VersionId IS NULL) -- Added by Sachin Bharti(26th Sep 2013)
					ORDER BY TC.CustomerName
		END
		
	--Query for Live Booked Leads
	IF @Type = 12
		BEGIN
			SELECT	DISTINCT 
					TC.Id , 
					TC.CustomerName, 
					TC.Email, 
					TC.Mobile, 
					TC.Location, 
					TLS.CreatedDate AS EntryDate, 
					TU.UserName,
					TLS.LatestCarName AS Car,
					TLS.LastCallComment AS LastComment,
					TLS.ScheduledOn AS NextFollowupDate,
					TLS.Source AS 'Source Name',
					TLS.Eagerness,VW.Make AS LostToMake ,VW.Model AS LostToModel,VW.Version AS LostToVersion,
					TLD.Name AS LostReason,TSD.SubDispositionName AS LostSubDispotion,TLS.TC_NewCarInquiriesId,
					CASE
						WHEN (TLS.TC_LeadDispositionID IS NULL)  THEN 'Active'
						WHEN (TLS.TC_LeadDispositionID = 4) THEN 'Booked'
						WHEN (TLS.TC_LeadDispositionID IS NOT NULL AND TLS.TC_LeadDispositionID <> 4) THEN 'Closed'
					END AS 'Status',
					TLS.TestDriveDate ,TLS.BookingEntryDate,TLS.BookingDate,TLS.BookingAmt,TLS.PromisedDeliveryDate,TLS.DeliveryEntryDate,
					TLS.CarDeliveryDate,TLS.PanNo,TLS.VinNO,TLS.CompanyName,TLS.EngineNumber,TLS.InsuranceCoverNumber,TLS.InvoiceNumber,TLS.RegistrationNo,
					TLS.InvoiceDate  -- Modified By: Tejashree Patil on 20 May 2014
					FROM TC_LeadBasedSummary TLS(NOLOCK)
					INNER JOIN TC_Lead TL(NOLOCK) ON TLS.TC_LeadId = TL.TC_LeadId
					INNER JOIN  TC_CustomerDetails TC(NOLOCK) ON TL.TC_CustomerId = TC.Id
					INNER JOIN TC_Users TU(NOLOCK) ON TU.Id = TLS.TC_UsersId
					LEFT JOIN TC_LeadDisposition TLD(NOLOCK) ON TLD.TC_LeadDispositionId = TLS.InquiryDispositionId
					LEFT JOIN TC_LeadSubDisposition TSD(NOLOCK) ON TSD.TC_LeadDispositionId = TLS.InquirySubDispositionId 
					LEFT JOIN vwMMV VW(NOLOCK) ON VW.VersionId = TLS.LostVersionId
					WHERE (TLS.DealerId = @DealerId OR @DealerId IS NULL) 
					AND  (TLS.TC_UsersId = @UserId OR @UserId IS NULL)
					AND TLS.BookingStatus=32 AND TLS.TC_LeadDispositionID=4  
					AND TLS.Invoicedate  IS NULL 
					AND (TLS.CarModelId = @ModelId OR @ModelId IS NULL)
					AND TLS.TC_LeadStageId<>3 ---Condition Added by Manish on 10-09-2013 since we need to take only open lead
					AND TLS.BookingDate <= @ToDate -- Added by Sachin Bharti on 20-09-2013
					AND  ISNULL(TLS.CarDeliveryStatus,0)<>77 ---- Condition added by manish on 23-09-2013 for handling old records also before Retail release
					AND (TLS.CarVersionId = @VersionId OR @VersionId IS NULL) -- Added by Sachin Bharti(26th Sep 2013)
					ORDER BY TC.CustomerName
		END
	--Query for Retail Leads
	IF @Type = 13
		BEGIN
			SELECT	DISTINCT 
					TC.Id , 
					TC.CustomerName, 
					TC.Email, 
					TC.Mobile, 
					TC.Location, 
					TLS.CreatedDate AS EntryDate, 
					TU.UserName,
					TLS.LatestCarName AS Car,
					TLS.LastCallComment AS LastComment,
					TLS.ScheduledOn AS NextFollowupDate,
					TLS.Source AS 'Source Name',
					TLS.Eagerness,VW.Make AS LostToMake ,VW.Model AS LostToModel,VW.Version AS LostToVersion,
					TLD.Name AS LostReason,TSD.SubDispositionName AS LostSubDispotion,TLS.TC_NewCarInquiriesId,
					CASE
						WHEN (TLS.TC_LeadDispositionID IS NULL)  THEN 'Active'
						WHEN (TLS.TC_LeadDispositionID = 4) THEN 'Booked'
						WHEN (TLS.TC_LeadDispositionID IS NOT NULL AND TLS.TC_LeadDispositionID <> 4) THEN 'Closed'
					END AS 'Status',
					TLS.TestDriveDate ,TLS.BookingEntryDate,TLS.BookingDate,TLS.BookingAmt,TLS.PromisedDeliveryDate,TLS.DeliveryEntryDate,
					TLS.CarDeliveryDate,TLS.PanNo,TLS.VinNO,TLS.CompanyName,TLS.EngineNumber,TLS.InsuranceCoverNumber,TLS.InvoiceNumber,TLS.RegistrationNo,
					TLS.InvoiceDate  -- Modified By: Tejashree Patil on 20 May 2014
					FROM TC_LeadBasedSummary TLS(NOLOCK)
					INNER JOIN TC_Lead TL(NOLOCK) ON TLS.TC_LeadId = TL.TC_LeadId
					INNER JOIN  TC_CustomerDetails TC(NOLOCK) ON TL.TC_CustomerId = TC.Id
					INNER JOIN TC_Users TU(NOLOCK) ON TU.Id = TLS.TC_UsersId
					LEFT JOIN TC_LeadDisposition TLD(NOLOCK) ON TLD.TC_LeadDispositionId = TLS.InquiryDispositionId
					LEFT JOIN TC_LeadSubDisposition TSD(NOLOCK) ON TSD.TC_LeadDispositionId = TLS.InquirySubDispositionId 
					LEFT JOIN vwMMV VW(NOLOCK) ON VW.VersionId = TLS.LostVersionId
					WHERE (TLS.DealerId = @DealerId OR @DealerId IS NULL) 
					AND  (TLS.TC_UsersId = @UserId OR @UserId IS NULL)
					AND TLS.Invoicedate  IS NOT NULL 
					--AND ISNULL(TLS.CarDeliveryStatus,0)<>77 -- Modified By: Manish on 16-09-2013 changing the logic of capturing Retails. Consider retail where invoice date is not null
					AND TLS.InvoiceDate BETWEEN @FromDate AND @ToDate 
					AND (TLS.CarModelId = @ModelId OR @ModelId IS NULL)
					AND (TLS.CarVersionId = @VersionId OR @VersionId IS NULL) -- Added by Sachin Bharti(26th Sep 2013)
					ORDER BY TC.CustomerName
		END
	--Query for Pending Deliveries
	IF @Type = 14
		BEGIN
			SELECT	DISTINCT 
					TC.Id , 
					TC.CustomerName, 
					TC.Email, 
					TC.Mobile, 
					TC.Location, 
					TLS.CreatedDate AS EntryDate, 
					TU.UserName,
					TLS.LatestCarName AS Car,
					TLS.LastCallComment AS LastComment,
					TLS.ScheduledOn AS NextFollowupDate,
					TLS.Source AS 'Source Name',
					TLS.Eagerness,VW.Make AS LostToMake ,VW.Model AS LostToModel,VW.Version AS LostToVersion,
					TLD.Name AS LostReason,TSD.SubDispositionName AS LostSubDispotion,TLS.TC_NewCarInquiriesId,
					CASE
						WHEN (TLS.TC_LeadDispositionID IS NULL)  THEN 'Active'
						WHEN (TLS.TC_LeadDispositionID = 4) THEN 'Booked'
						WHEN (TLS.TC_LeadDispositionID IS NOT NULL AND TLS.TC_LeadDispositionID <> 4) THEN 'Closed'
					END AS 'Status',
					TLS.TestDriveDate ,TLS.BookingEntryDate,TLS.BookingDate,TLS.BookingAmt,TLS.PromisedDeliveryDate,TLS.DeliveryEntryDate,
					TLS.CarDeliveryDate,TLS.PanNo,TLS.VinNO,TLS.CompanyName,TLS.EngineNumber,TLS.InsuranceCoverNumber,TLS.InvoiceNumber,TLS.RegistrationNo,
					TLS.InvoiceDate -- Modified By: Tejashree Patil on 20 May 2014
					FROM TC_LeadBasedSummary TLS(NOLOCK)
					INNER JOIN TC_Lead TL(NOLOCK) ON TLS.TC_LeadId = TL.TC_LeadId
					INNER JOIN  TC_CustomerDetails TC(NOLOCK) ON TL.TC_CustomerId = TC.Id
					INNER JOIN TC_Users TU(NOLOCK) ON TU.Id = TLS.TC_UsersId
					LEFT JOIN TC_LeadDisposition TLD(NOLOCK) ON TLD.TC_LeadDispositionId = TLS.InquiryDispositionId
					LEFT JOIN TC_LeadSubDisposition TSD(NOLOCK) ON TSD.TC_LeadDispositionId = TLS.InquirySubDispositionId 
					LEFT JOIN vwMMV VW(NOLOCK) ON VW.VersionId = TLS.LostVersionId
					WHERE (TLS.DealerId = @DealerId OR @DealerId IS NULL) 
					AND  (TLS.TC_UsersId = @UserId OR @UserId IS NULL)
					AND TLS.Invoicedate  IS NOT NULL AND ISNULL(TLS.CarDeliveryStatus,0)<>77
					AND (TLS.CarModelId = @ModelId OR @ModelId IS NULL)
					AND TLS.InvoiceDate <= @ToDate -- Added by Sachin Bharti on 20-09-2013
					AND (TLS.CarVersionId = @VersionId OR @VersionId IS NULL) -- Added by Sachin Bharti(26th Sep 2013)
					ORDER BY TC.CustomerName
		END
END
