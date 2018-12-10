IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_ReportDataDownloder]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_ReportDataDownloder]
GO

	-- =============================================
-- Author:		<Author,Sachin Bharti>
-- Create date: <Create Date,4th September 2013>
-- Description:	<Description,Get all the data for Special users reports>
-- Modified by manish on 17-09-2013 taking correct disposition for Booking Status
-- Modified By : Sachin Bharti(19th Sep 2013) Capturing car name at inquiry level and added column InquiryCreationDate
-- Modified By : Sachin Bharti(1st Oct 2013) Added query for Retail and Pending Deliveries
-- Modified By : Sachin Bharti(10 Oct 2013) Added two columns for ModelColorCode and ModelYear
-- Modified By : Vivek Singh(11th nov 2013) Changes done for where Clause for test Drive,Live Bookings and Pending Deliveries
-- Modified By : Vivek Singh (22 jan 2014) Added new columns Inq. Entry Date,Number of Test Drives Given,Booking Model Year,Booking Model Colour,Retail Entry Date,Retail Date,Retail Customer Name,Retail Customer Mobile no.,Retail Customer email id,Lost Date,Campaign Name,Exchange Car (Make, model, variant),Exchange Car Year,Exchange Car Km driven,Exchange Car Expected price,Campaign
-- Type 1 :	Total Inquiries
-- Type 2 :	Active Inquiries
-- Type 3 :	Test Drive
-- Type 4 :	Total Booked Leads
-- Type 5 :	Live Bookings Leads
-- Type 6 :	Delivery Leads
-- Type 7 :	Lost Leads
-- Type 8 :	Cancelled Leads
-- Type 9 :	Retail Leads
-- Type 10 : Pending Deliveries
-- Modified By: Tejashree Patil on 14 May 2014, Added colums RetailsDate,VTONumber,CampaignName. 
-- =============================================
CREATE PROCEDURE [dbo].[TC_ReportDataDownloder] 
	@BranchId	NUMERIC(18,0),
	@FromDate	Datetime ,
	@ToDate		DateTime , 
	@LeadType	INT,
	@RMID NUMERIC(18,0) = NULL,
	@AMID NUMERIC(18,0)	= NULL,
	@DealerId NUMERIC(18,0) = NULL,
	@MakeId		INT,
	@Designation INT
	
AS
BEGIN
	
	IF @RMID IS NULL AND @Designation = 3
		SET @RMID = @BranchId
	IF @AMID IS NULL AND @Designation = 4
		SET @AMID = @BranchId 
	
	--Total Inquiries
	IF @LeadType = 1 
		BEGIN
			SELECT DISTINCT
				REPLACE(ISNULL(TSU.UserName,'-'), CHAR(9), '')AS RM, 
				REPLACE(ISNULL(TSU1.UserName,'-'), CHAR(9), '')AS AM, 
				REPLACE(ISNULL(D.Organization,'-'), CHAR(9), '') AS DealerName,  
				ISNULL(D.DealerCode, '-') DealerCode,				
				ISNULL(TC.UniqueCustomerId,'-')AS UniqueCustomerId,
				dbo.RemoveSpecialCharsExceptBlank(ISNULL(TC.CustomerName, '-')) AS CustomerName, 
				ISNULL(TC.Mobile, '-') AS Mobile, 
				ISNULL(TC.Email,'-') AS Email,
				REPLACE(ISNULL(TBS.Source,'-'), CHAR(9), '') AS Source, 
				REPLACE(ISNULL(TBS.Eagerness,'-'), CHAR(9), '')AS Eagerness,
				REPLACE(ISNULL(TBS.CompanyName,'-'), CHAR(9), '')AS CompanyName,
				REPLACE(ISNULL(VWW.Car,'-'), CHAR(9), '')AS CarName,--Added by Sachin Bharti(19th Sept 2013)
				TBS.InquiryCreationDate AS InquiryCreationDate ,--Added by Sachin Bharti(19th Sept 2013)
				TBS.CreatedDate AS LeadCreationDate,--Added by Sachin Bharti(19th Sept 2013)
				ISNULL(C.Name,'-')AS City,
				REPLACE(ISNULL(TU.UserName,'-'), CHAR(9), '') AS UserConsultant,
				dbo.RemoveSpecialCharsExceptBlank(ISNULL(TBS.LastCallComment,'-')) AS LastCallComment,
				TBS.TestDriveDate AS TestDriveDate,
				--(SELECT COUNT(*) FROM TC_TDCalendar TD WHERE TD.TC_NewCarInquiriesId= TBS.TC_NewCarInquiriesId)  AS NumberOfTDs,--Added by vivek singh (22nd jan 2014)
				TBS.BookingEntryDate BookingEntryDate,
				TBS.BookingDate AS BookingDate,
				TBS.BookingAmt AS BookingAmt,  
				REPLACE(ISNULL(TCLD.Name,'-'),CHAR(9),'') AS BookingStatus, ---- modified by manish on 17-09-2013 changing table TLD to TCLD
				TBS.BookingCancelDate,
				TBS.InvoiceDate AS RetailDate,
				TBS.PromisedDeliveryDate AS PromisedDeliveryDate,
				TBS.DeliveryEntryDate AS DeliveryEntryDate,
				TBS.CarDeliveryDate AS CarDeliveryDate,
				dbo.RemoveSpecialCharsExceptBlank(ISNULL(TBS.EngineNumber,'-')) AS EngineNumber,
				dbo.RemoveSpecialCharsExceptBlank(ISNULL(TBS.InsuranceCoverNumber,'-'))AS InsuranceCoverNumber,
				dbo.RemoveSpecialCharsExceptBlank(ISNULL(TBS.InvoiceNumber,'-')) AS InvoiceNumber,
				TBS.InvoiceDate AS RetailDate,ISNULL(TV.CarVersionCode,'-')AS ModelCode,
				--dbo.RemoveSpecialCharsExceptBlank(ISNULL(TBS.InvoiceNumber,'-')) AS InvoiceNumber,
				dbo.RemoveSpecialCharsExceptBlank(ISNULL(TBS.RegistrationNo,'-'))AS RegistrationNo,
				dbo.RemoveSpecialCharsExceptBlank(ISNULL(TBS.PanNo,'-'))AS PanNo,
				dbo.RemoveSpecialCharsExceptBlank(ISNULL(TBS.VinNO,'-')) AS VinNO, 
				ISNULL(TBS.BookingName,'-') AS RetailCustomerName,    ----Added by vivek singh (22nd jan 2014)
			    ISNULL(TBS.BookingMobile,'-') AS RetailCustomerMobileNo,
				ISNULL(TBS.BookingEmail,'-') AS RetailCustomerEmail,  
				TBS.InquiryDispositionDate AS LostDate,                                                 
				ISNULL(TLD.Name,'-') AS LostReason,
				ISNULL(VW.Make,'-') AS LostToMake ,
				ISNULL(VW.Model,'-') AS LostToModel,
				ISNULL(VW.Version,'-') AS LostToVersion, 
				ISNULL(TSD.SubDispositionName,'-') AS LostSubDisposition,
				--Added by Sachin Bharti(10th Oct 2013)
				ISNULL(VC.ColorCode,'-') AS ModelColorCode,
				ISNULL(TBS.BookedCarMakeYear,'-')AS ModelYear,
				ISNULL(VEX.Make,'-') AS ExchangeCarMake,      --Added by vivek singh (22nd jan 2014)
			    ISNULL(VEX.Model,'-') AS ExchangeCarModel,
				ISNULL(VEX.Version,'-') AS ExchangeCarversion,
				TBS.ExchangeCarMakeYear AS ExchangeCarYear,
				ISNULL(TBS.ExchangeCarKms,'-') AS ExchangeCarKMdriven,
				ISNULL(TBS.ExchangeCarExpectedPrice,'-') AS ExchangeCarExpectedPrice,
				ISNULL(TU.VTONumbers,'-') AS VTONumbers,-- Modified By: Tejashree Patil on 14 May 2014,
				ISNULL(VWC.MainCampaignName+' ' + VWC.SubCampaignName,'-') AS CampaignName
			FROM 
				DEALERS as D WITH (NOLOCK)
				INNER JOIN TC_LeadBasedSummary TBS WITH (NOLOCK) ON D.ID = TBS.DealerId
				INNER JOIN TC_BrandZone TBZ WITH (NOLOCK) ON D.TC_BrandZoneId = TBZ.TC_BrandZoneId AND TBZ.MakeId = @Makeid 
				LEFT JOIN TC_SpecialUsers TSU WITH (NOLOCK) ON D.TC_RMId = TSU.TC_SpecialUsersId AND TSU.IsActive = 1
				LEFT JOIN TC_SpecialUsers TSU1 WITH (NOLOCK) ON D.TC_AMId = TSU1.TC_SpecialUsersId AND TSU1.IsActive = 1
				LEFT JOIN TC_Lead TL(NOLOCK) ON TBS.TC_LeadId = TL.TC_LeadId
				LEFT JOIN TC_CustomerDetails TC(NOLOCK) ON TL.TC_CustomerId = TC.Id
				LEFT JOIN TC_Users TU(NOLOCK) ON TU.Id = TBS.TC_UsersId
				LEFT JOIN TC_LeadDisposition TLD(NOLOCK) ON TLD.TC_LeadDispositionId = TBS.InquiryDispositionId
				LEFT JOIN TC_LeadDisposition AS TCLD WITH (NOLOCK) ON  TCLD.TC_LeadDispositionId=TBS.BookingStatus  ------ modified by manish on 17-09-2013 taking dispostion for booking status
				LEFT JOIN TC_LeadSubDisposition TSD(NOLOCK) ON TSD.TC_LeadSubDispositionId = TBS.InquirySubDispositionId 
				LEFT JOIN vwMMV VW(NOLOCK) ON VW.VersionId = TBS.LostVersionId
				LEFT JOIN vwMMV VWW(NOLOCK) ON VWW.VersionId = TBS.CarVersionId--Added By Sachin Bharti(19th Sept 2013)
				LEFT JOIN Cities C(NOLOCK) ON C.ID = TBS.InquiryCityId -- Changed By Sachin Bharti(19th Sept 2013)
				LEFT JOIN TC_VersionsCode TV(NOLOCK) ON TV.CarVersionId = TBS.CarVersionId
				LEFT JOIN TC_VersionColourCode VC(NOLOCK) ON VC.VersionColorsId = TBS.BookedCarColourId --Added by Sachin Bharti (10th Oct 2013)
				--LEFT JOIN TC_ExchangeNewCar TENC WITH(NOLOCK) ON TENC.TC_NewCarInquiriesId=TBS.TC_NewCarInquiriesId --Added by Vivek Singh (22nd jan 2014)
				LEFT JOIN vwMMV VEX WITH(NOLOCK) ON VEX.VersionId = TBS.ExchangeCarVersionId  --Added by Vivek Singh (22nd jan 2014)
				--LEFT JOIN TC_NewCarBooking  AS NB   WITH(NOLOCK) ON NB.TC_NewCarInquiriesId = TBS.TC_NewCarInquiriesId --Added by Vivek Singh (22nd jan 2014)
				LEFT JOIN TC_CampaignScheduling CS WITH(NOLOCK) ON CS.TC_CampaignSchedulingId=TBS.NSCCampaignSchedulingId --By Tejashree on 14/5/2014
				LEFT JOIN TC_vwCampaignMaster VWC WITH(NOLOCK) ON VWC.TC_SubCampaignId=CS.TC_SubCampaignId
			WHERE TBS.CreatedDate BETWEEN @FromDate AND @ToDate 
			AND (D.TC_RMID = @RMID OR @RMID IS NULL) AND (D.TC_AMId = @AMID OR @AMID IS NULL)
			AND (D.ID =@DealerId OR @DealerId IS NULL)
		END
	--Active Inquiries
	IF @LeadType = 2 
		BEGIN
				SELECT DISTINCT 
				REPLACE(ISNULL(TSU.UserName,'-'), CHAR(9), '')AS RM, 
				REPLACE(ISNULL(TSU1.UserName,'-'), CHAR(9), '')AS AM, 
				REPLACE(ISNULL(D.Organization,'-'), CHAR(9), '') AS DealerName,  
				ISNULL(D.DealerCode, '-') DealerCode,				
				ISNULL(TC.UniqueCustomerId,'-')AS UniqueCustomerId,
				dbo.RemoveSpecialCharsExceptBlank(ISNULL(TC.CustomerName, '-')) AS CustomerName, 
				ISNULL(TC.Mobile, '-') AS Mobile, 
				ISNULL(TC.Email,'-') AS Email,
				REPLACE(ISNULL(TBS.Source,'-'), CHAR(9), '') AS Source, 
				REPLACE(ISNULL(TBS.Eagerness,'-'), CHAR(9), '')AS Eagerness,
				REPLACE(ISNULL(TBS.CompanyName,'-'), CHAR(9), '')AS CompanyName,
				--REPLACE(ISNULL(TBS.LatestCarName,'-'), CHAR(9), '')AS LatestCarName,
				REPLACE(ISNULL(VWW.Car,'-'), CHAR(9), '')AS CarName,--Added by Sachin Bharti(19th Sept 2013)
				TBS.InquiryCreationDate AS InquiryCreationDate ,--Added by Sachin Bharti(19th Sept 2013)
				TBS.CreatedDate AS LeadCreationDate,--Added by Sachin Bharti(19th Sept 2013)
				ISNULL(C.Name,'-')AS City,
				REPLACE(ISNULL(TU.UserName,'-'), CHAR(9), '') AS UserConsultant,
				dbo.RemoveSpecialCharsExceptBlank(ISNULL(TBS.LastCallComment,'-')) AS LastCallComment,
				TBS.TestDriveDate AS TestDriveDate,
				--(SELECT COUNT(*) FROM TC_TDCalendar TD WHERE TD.TC_NewCarInquiriesId= TBS.TC_NewCarInquiriesId)  AS NumberOfTDs,--Added by vivek singh (22nd jan 2014)
				TBS.BookingEntryDate BookingEntryDate,
				TBS.BookingDate AS BookingDate,
				TBS.BookingAmt AS BookingAmt,  
				REPLACE(ISNULL(TCLD.Name,'-'),CHAR(9),'') AS BookingStatus, ---- modified by manish on 17-09-2013 changing table TLD to TCLD
				TBS.BookingCancelDate,
				TBS.PromisedDeliveryDate AS PromisedDeliveryDate,
				TBS.DeliveryEntryDate AS DeliveryEntryDate,
				TBS.CarDeliveryDate AS CarDeliveryDate,
				dbo.RemoveSpecialCharsExceptBlank(ISNULL(TBS.EngineNumber,'-')) AS EngineNumber,
				dbo.RemoveSpecialCharsExceptBlank(ISNULL(TBS.InsuranceCoverNumber,'-'))AS InsuranceCoverNumber,
				dbo.RemoveSpecialCharsExceptBlank(ISNULL(TBS.InvoiceNumber,'-')) AS InvoiceNumber,
				TBS.InvoiceDate AS RetailDate,ISNULL(TV.CarVersionCode,'-')AS ModelCode,
				--dbo.RemoveSpecialCharsExceptBlank(ISNULL(TBS.InvoiceNumber,'-')) AS InvoiceNumber,
				dbo.RemoveSpecialCharsExceptBlank(ISNULL(TBS.RegistrationNo,'-'))AS RegistrationNo,
				dbo.RemoveSpecialCharsExceptBlank(ISNULL(TBS.PanNo,'-'))AS PanNo,
				dbo.RemoveSpecialCharsExceptBlank(ISNULL(TBS.VinNO,'-')) AS VinNO, 
				ISNULL(TBS.BookingName,'-') AS RetailCustomerName,    --Added by vivek singh (22nd jan 2014)
			    ISNULL(TBS.BookingMobile,'-') AS RetailCustomerMobileNo,
				ISNULL(TBS.BookingEmail,'-') AS RetailCustomerEmail,  
				TBS.InquiryDispositionDate AS LostDate,                                                    
				ISNULL(TLD.Name,'-') AS LostReason,
				ISNULL(VW.Make,'-') AS LostToMake ,
				ISNULL(VW.Model,'-') AS LostToModel,
				ISNULL(VW.Version,'-') AS LostToVersion, 
				ISNULL(TSD.SubDispositionName,'-') AS LostSubDisposition,
				--Added by Sachin Bharti(10th Oct 2013)
				ISNULL(VC.ColorCode,'-') AS ModelColorCode,
				ISNULL(TBS.BookedCarMakeYear,'-')AS ModelYear,
				ISNULL(VEX.Make,'-') AS ExchangeCarMake,      --Added by vivek singh (22nd jan 2014)
			    ISNULL(VEX.Model,'-') AS ExchangeCarModel,
				ISNULL(VEX.Version,'-') AS ExchangeCarversion,
				TBS.ExchangeCarMakeYear AS ExchangeCarYear,
				ISNULL(TBS.ExchangeCarKms,'-') AS ExchangeCarKMdriven,
				ISNULL(TBS.ExchangeCarExpectedPrice,'-') AS ExchangeCarExpectedPrice,
				ISNULL(TU.VTONumbers,'-') AS VTONumbers,-- Modified By: Tejashree Patil on 14 May 2014,
				ISNULL(VWC.MainCampaignName+' ' + VWC.SubCampaignName,'-') AS CampaignName
			FROM 
				DEALERS as D WITH (NOLOCK)
				INNER JOIN TC_LeadBasedSummary TBS WITH (NOLOCK) ON D.ID = TBS.DealerId
				INNER JOIN TC_BrandZone TBZ WITH (NOLOCK) ON D.TC_BrandZoneId = TBZ.TC_BrandZoneId AND TBZ.MakeId = @Makeid 
				LEFT JOIN TC_SpecialUsers TSU WITH (NOLOCK) ON D.TC_RMId = TSU.TC_SpecialUsersId AND TSU.IsActive = 1
				LEFT JOIN TC_SpecialUsers TSU1 WITH (NOLOCK) ON D.TC_AMId = TSU1.TC_SpecialUsersId AND TSU1.IsActive = 1
				LEFT JOIN TC_Lead TL(NOLOCK) ON TBS.TC_LeadId = TL.TC_LeadId
				LEFT JOIN TC_CustomerDetails TC(NOLOCK) ON TL.TC_CustomerId = TC.Id
				LEFT JOIN TC_Users TU(NOLOCK) ON TU.Id = TBS.TC_UsersId
				LEFT JOIN TC_LeadDisposition TLD(NOLOCK) ON TLD.TC_LeadDispositionId = TBS.InquiryDispositionId
				LEFT JOIN TC_LeadDisposition AS TCLD WITH (NOLOCK) ON  TCLD.TC_LeadDispositionId=TBS.BookingStatus  ------ modified by manish on 17-09-2013 taking dispostion for booking status
				LEFT JOIN TC_LeadSubDisposition TSD(NOLOCK) ON TSD.TC_LeadSubDispositionId = TBS.InquirySubDispositionId 
				LEFT JOIN vwMMV VW(NOLOCK) ON VW.VersionId = TBS.LostVersionId
				LEFT JOIN vwMMV VWW(NOLOCK) ON VWW.VersionId = TBS.CarVersionId--Added By Sachin Bharti(19th Sept 2013)
				LEFT JOIN Cities C(NOLOCK) ON C.ID = TBS.InquiryCityId -- Changed By Sachin Bharti(19th Sept 2013)
				LEFT JOIN TC_VersionsCode TV(NOLOCK) ON TV.CarVersionId = TBS.CarVersionId
				LEFT JOIN TC_VersionColourCode VC(NOLOCK) ON VC.VersionColorsId = TBS.BookedCarColourId --Added by Sachin Bharti (10th Oct 2013)
				--LEFT JOIN TC_ExchangeNewCar TENC WITH(NOLOCK) ON TENC.TC_NewCarInquiriesId=TBS.TC_NewCarInquiriesId --Added by Vivek Singh (22nd jan 2014)
				LEFT JOIN vwMMV VEX WITH(NOLOCK) ON VEX.VersionId = TBS.ExchangeCarVersionId  --Added by Vivek Singh (22nd jan 2014)
				--LEFT JOIN TC_NewCarBooking  AS NB   WITH(NOLOCK) ON NB.TC_NewCarInquiriesId = TBS.TC_NewCarInquiriesId --Added by Vivek Singh (22nd jan 2014)
				LEFT JOIN TC_CampaignScheduling CS WITH(NOLOCK) ON CS.TC_CampaignSchedulingId=TBS.NSCCampaignSchedulingId --By Tejashree on 14/5/2014
				LEFT JOIN TC_vwCampaignMaster VWC WITH(NOLOCK) ON VWC.TC_SubCampaignId=CS.TC_SubCampaignId
			WHERE TBS.TC_LeadStageId<>3 AND ISNULL(TBS.TC_LeadDispositionID,0)<>4 
				AND (D.TC_RMID = @RMID OR @RMID IS NULL) AND (D.TC_AMId = @AMID OR @AMID IS NULL)
				AND (D.ID =@DealerId OR @DealerId IS NULL)
		END
	--Test Drive
	IF @LeadType = 3 
		BEGIN
				SELECT DISTINCT 
				REPLACE(ISNULL(TSU.UserName,'-'), CHAR(9), '')AS RM, 
				REPLACE(ISNULL(TSU1.UserName,'-'), CHAR(9), '')AS AM, 
				REPLACE(ISNULL(D.Organization,'-'), CHAR(9), '') AS DealerName,  
				ISNULL(D.DealerCode, '-') DealerCode,				
				ISNULL(TC.UniqueCustomerId,'-')AS UniqueCustomerId,
				dbo.RemoveSpecialCharsExceptBlank(ISNULL(TC.CustomerName, '-')) AS CustomerName, 
				ISNULL(TC.Mobile, '-') AS Mobile, 
				ISNULL(TC.Email,'-') AS Email,
				REPLACE(ISNULL(TBS.Source,'-'), CHAR(9), '') AS Source, 
				REPLACE(ISNULL(TBS.Eagerness,'-'), CHAR(9), '')AS Eagerness,
				REPLACE(ISNULL(TBS.CompanyName,'-'), CHAR(9), '')AS CompanyName,
				--REPLACE(ISNULL(TBS.LatestCarName,'-'), CHAR(9), '')AS LatestCarName,
				REPLACE(ISNULL(VWW.Car,'-'), CHAR(9), '')AS CarName,--Added by Sachin Bharti(19th Sept 2013)
				TBS.InquiryCreationDate AS InquiryCreationDate ,--Added by Sachin Bharti(19th Sept 2013)
				TBS.CreatedDate AS LeadCreationDate,--Added by Sachin Bharti(19th Sept 2013)
				ISNULL(C.Name,'-')AS City,
				REPLACE(ISNULL(TU.UserName,'-'), CHAR(9), '') AS UserConsultant,
				dbo.RemoveSpecialCharsExceptBlank(ISNULL(TBS.LastCallComment,'-')) AS LastCallComment,
				TBS.TestDriveDate AS TestDriveDate,
				--(SELECT COUNT(*) FROM TC_TDCalendar TD WHERE TD.TC_NewCarInquiriesId= TBS.TC_NewCarInquiriesId)  AS NumberOfTDs,--Added by vivek singh (22nd jan 2014)
				TBS.BookingEntryDate BookingEntryDate,
				TBS.BookingDate AS BookingDate,
				TBS.BookingAmt AS BookingAmt,  
				REPLACE(ISNULL(TCLD.Name,'-'),CHAR(9),'') AS BookingStatus, ---- modified by manish on 17-09-2013 changing table TLD to TCLD
				TBS.BookingCancelDate,
				TBS.PromisedDeliveryDate AS PromisedDeliveryDate,
				TBS.DeliveryEntryDate AS DeliveryEntryDate,
				TBS.CarDeliveryDate AS CarDeliveryDate,
				dbo.RemoveSpecialCharsExceptBlank(ISNULL(TBS.EngineNumber,'-')) AS EngineNumber,
				dbo.RemoveSpecialCharsExceptBlank(ISNULL(TBS.InsuranceCoverNumber,'-'))AS InsuranceCoverNumber,
				dbo.RemoveSpecialCharsExceptBlank(ISNULL(TBS.InvoiceNumber,'-')) AS InvoiceNumber,
				TBS.InvoiceDate AS RetailDate,ISNULL(TV.CarVersionCode,'-')AS ModelCode,
				--dbo.RemoveSpecialCharsExceptBlank(ISNULL(TBS.InvoiceNumber,'-')) AS InvoiceNumber,
				dbo.RemoveSpecialCharsExceptBlank(ISNULL(TBS.RegistrationNo,'-'))AS RegistrationNo,
				dbo.RemoveSpecialCharsExceptBlank(ISNULL(TBS.PanNo,'-'))AS PanNo,
				dbo.RemoveSpecialCharsExceptBlank(ISNULL(TBS.VinNO,'-')) AS VinNO, 
				ISNULL(TBS.BookingName,'-') AS RetailCustomerName,    --Added by vivek singh (22nd jan 2014)
			    ISNULL(TBS.BookingMobile,'-') AS RetailCustomerMobileNo,
				ISNULL(TBS.BookingEmail,'-') AS RetailCustomerEmail,   
				TBS.InquiryDispositionDate AS LostDate,      
				ISNULL(TLD.Name,'-') AS LostReason,
				ISNULL(VW.Make,'-') AS LostToMake ,
				ISNULL(VW.Model,'-') AS LostToModel,
				ISNULL(VW.Version,'-') AS LostToVersion, 
				ISNULL(TSD.SubDispositionName,'-') AS LostSubDisposition,
				--Added by Sachin Bharti(10th Oct 2013)
				ISNULL(VC.ColorCode,'-') AS ModelColorCode,
				ISNULL(TBS.BookedCarMakeYear,'-')AS ModelYear,
				ISNULL(VEX.Make,'-') AS ExchangeCarMake,      --Added by vivek singh (22nd jan 2014)
			    ISNULL(VEX.Model,'-') AS ExchangeCarModel,
				ISNULL(VEX.Version,'-') AS ExchangeCarversion,
				TBS.ExchangeCarMakeYear AS ExchangeCarYear,
				ISNULL(TBS.ExchangeCarKms,'-') AS ExchangeCarKMdriven,
				ISNULL(TBS.ExchangeCarExpectedPrice,'-') AS ExchangeCarExpectedPrice,
				ISNULL(TU.VTONumbers,'-') AS VTONumbers,-- Modified By: Tejashree Patil on 14 May 2014,
				ISNULL(VWC.MainCampaignName+' ' + VWC.SubCampaignName,'-') AS CampaignName
			FROM 
				DEALERS as D WITH (NOLOCK)
				INNER JOIN TC_LeadBasedSummary TBS WITH (NOLOCK) ON D.ID = TBS.DealerId
				INNER JOIN TC_BrandZone TBZ WITH (NOLOCK) ON D.TC_BrandZoneId = TBZ.TC_BrandZoneId AND TBZ.MakeId = @Makeid 
				LEFT JOIN TC_SpecialUsers TSU WITH (NOLOCK) ON D.TC_RMId = TSU.TC_SpecialUsersId AND TSU.IsActive = 1
				LEFT JOIN TC_SpecialUsers TSU1 WITH (NOLOCK) ON D.TC_AMId = TSU1.TC_SpecialUsersId AND TSU1.IsActive = 1
				LEFT JOIN TC_Lead TL(NOLOCK) ON TBS.TC_LeadId = TL.TC_LeadId
				LEFT JOIN TC_CustomerDetails TC(NOLOCK) ON TL.TC_CustomerId = TC.Id
				LEFT JOIN TC_Users TU(NOLOCK) ON TU.Id = TBS.TC_UsersId
				LEFT JOIN TC_LeadDisposition TLD(NOLOCK) ON TLD.TC_LeadDispositionId = TBS.InquiryDispositionId
				LEFT JOIN TC_LeadDisposition AS TCLD WITH (NOLOCK) ON  TCLD.TC_LeadDispositionId=TBS.BookingStatus  ------ modified by manish on 17-09-2013 taking dispostion for booking status
				LEFT JOIN TC_LeadSubDisposition TSD(NOLOCK) ON TSD.TC_LeadSubDispositionId = TBS.InquirySubDispositionId
				LEFT JOIN vwMMV VW(NOLOCK) ON VW.VersionId = TBS.LostVersionId
				LEFT JOIN vwMMV VWW(NOLOCK) ON VWW.VersionId = TBS.CarVersionId--Added By Sachin Bharti(19th Sept 2013)
				LEFT JOIN Cities C(NOLOCK) ON C.ID = TBS.InquiryCityId -- Changed By Sachin Bharti(19th Sept 2013)
				LEFT JOIN TC_VersionsCode TV(NOLOCK) ON TV.CarVersionId = TBS.CarVersionId
				LEFT JOIN TC_VersionColourCode VC(NOLOCK) ON VC.VersionColorsId = TBS.BookedCarColourId --Added by Sachin Bharti (10th Oct 2013)
				--LEFT JOIN TC_ExchangeNewCar TENC WITH(NOLOCK) ON TENC.TC_NewCarInquiriesId=TBS.TC_NewCarInquiriesId --Added by Vivek Singh (22nd jan 2014)
				LEFT JOIN vwMMV VEX WITH(NOLOCK) ON VEX.VersionId = TBS.ExchangeCarVersionId  --Added by Vivek Singh (22nd jan 2014)
				--LEFT JOIN TC_NewCarBooking  AS NB   WITH(NOLOCK) ON NB.TC_NewCarInquiriesId = TBS.TC_NewCarInquiriesId --Added by Vivek Singh (22nd jan 2014)
				LEFT JOIN TC_CampaignScheduling CS WITH(NOLOCK) ON CS.TC_CampaignSchedulingId=TBS.NSCCampaignSchedulingId --By Tejashree on 14/5/2014
				LEFT JOIN TC_vwCampaignMaster VWC WITH(NOLOCK) ON VWC.TC_SubCampaignId=CS.TC_SubCampaignId
			WHERE TBS.TestDriveStatus = 28 AND TBS.TestDriveDate BETWEEN @FromDate AND @ToDate --Added (TBS.TestDriveStatus = 28) by Vivek Singh on 11th Nov 2013 
				AND (D.TC_RMID = @RMID OR @RMID IS NULL) AND (D.TC_AMId = @AMID OR @AMID IS NULL)
				AND (D.ID =@DealerId OR @DealerId IS NULL)
		END
	--Booked Lead
	IF @LeadType = 4 
		BEGIN
				SELECT DISTINCT 
				REPLACE(ISNULL(TSU.UserName,'-'), CHAR(9), '')AS RM, 
				REPLACE(ISNULL(TSU1.UserName,'-'), CHAR(9), '')AS AM, 
				REPLACE(ISNULL(D.Organization,'-'), CHAR(9), '') AS DealerName,  
				ISNULL(D.DealerCode, '-') DealerCode,				
				ISNULL(TC.UniqueCustomerId,'-')AS UniqueCustomerId,
				dbo.RemoveSpecialCharsExceptBlank(ISNULL(TC.CustomerName, '-')) AS CustomerName, 
				ISNULL(TC.Mobile, '-') AS Mobile, 
				ISNULL(TC.Email,'-') AS Email,
				REPLACE(ISNULL(TBS.Source,'-'), CHAR(9), '') AS Source, 
				REPLACE(ISNULL(TBS.Eagerness,'-'), CHAR(9), '')AS Eagerness,
				REPLACE(ISNULL(TBS.CompanyName,'-'), CHAR(9), '')AS CompanyName,
				--REPLACE(ISNULL(TBS.LatestCarName,'-'), CHAR(9), '')AS LatestCarName,
				REPLACE(ISNULL(VWW.Car,'-'), CHAR(9), '')AS CarName,--Added by Sachin Bharti(19th Sept 2013)
				TBS.InquiryCreationDate AS InquiryCreationDate ,--Added by Sachin Bharti(19th Sept 2013)
				TBS.CreatedDate AS LeadCreationDate,--Added by Sachin Bharti(19th Sept 2013)
				ISNULL(C.Name,'-')AS City,
				REPLACE(ISNULL(TU.UserName,'-'), CHAR(9), '') AS UserConsultant,
				dbo.RemoveSpecialCharsExceptBlank(ISNULL(TBS.LastCallComment,'-')) AS LastCallComment,
				TBS.TestDriveDate AS TestDriveDate,
				--(SELECT COUNT(*) FROM TC_TDCalendar TD WHERE TD.TC_NewCarInquiriesId= TBS.TC_NewCarInquiriesId)  AS NumberOfTDs,--Added by vivek singh (22nd jan 2014)
				TBS.BookingEntryDate BookingEntryDate,
				TBS.BookingDate AS BookingDate,
				TBS.BookingAmt AS BookingAmt,
				REPLACE(ISNULL(TCLD.Name,'-'),CHAR(9),'') AS BookingStatus, ---- modified by manish on 17-09-2013 changing table TLD to TCLD
				TBS.BookingCancelDate,
				TBS.PromisedDeliveryDate AS PromisedDeliveryDate,
				TBS.DeliveryEntryDate AS DeliveryEntryDate,
				TBS.CarDeliveryDate AS CarDeliveryDate,
				dbo.RemoveSpecialCharsExceptBlank(ISNULL(TBS.EngineNumber,'-')) AS EngineNumber,
				dbo.RemoveSpecialCharsExceptBlank(ISNULL(TBS.InsuranceCoverNumber,'-'))AS InsuranceCoverNumber,
				dbo.RemoveSpecialCharsExceptBlank(ISNULL(TBS.InvoiceNumber,'-')) AS InvoiceNumber,
				TBS.InvoiceDate AS RetailDate,ISNULL(TV.CarVersionCode,'-')AS ModelCode,
				--dbo.RemoveSpecialCharsExceptBlank(ISNULL(TBS.InvoiceNumber,'-')) AS InvoiceNumber,
				dbo.RemoveSpecialCharsExceptBlank(ISNULL(TBS.RegistrationNo,'-'))AS RegistrationNo,
				dbo.RemoveSpecialCharsExceptBlank(ISNULL(TBS.PanNo,'-'))AS PanNo,
				dbo.RemoveSpecialCharsExceptBlank(ISNULL(TBS.VinNO,'-')) AS VinNO, 
				ISNULL(TBS.BookingName,'-') AS RetailCustomerName,    --Added by vivek singh (22nd jan 2014)
			    ISNULL(TBS.BookingMobile,'-') AS RetailCustomerMobileNo,
				ISNULL(TBS.BookingEmail,'-') AS RetailCustomerEmail,   
			    TBS.InquiryDispositionDate AS LostDate,           
				ISNULL(TLD.Name,'-') AS LostReason,
				ISNULL(VW.Make,'-') AS LostToMake ,
				ISNULL(VW.Model,'-') AS LostToModel,
				ISNULL(VW.Version,'-') AS LostToVersion, 
				ISNULL(TSD.SubDispositionName,'-') AS LostSubDisposition,
				--Added by Sachin Bharti(10th Oct 2013)
				ISNULL(VC.ColorCode,'-') AS ModelColorCode,
				ISNULL(TBS.BookedCarMakeYear,'-')AS ModelYear,
				ISNULL(VEX.Make,'-') AS ExchangeCarMake,      --Added by vivek singh (22nd jan 2014)
			    ISNULL(VEX.Model,'-') AS ExchangeCarModel,
				ISNULL(VEX.Version,'-') AS ExchangeCarversion,
				TBS.ExchangeCarMakeYear AS ExchangeCarYear,
				ISNULL(TBS.ExchangeCarKms,'-') AS ExchangeCarKMdriven,
				ISNULL(TBS.ExchangeCarExpectedPrice,'-') AS ExchangeCarExpectedPrice,				
				ISNULL(TU.VTONumbers,'-') AS VTONumbers,-- Modified By: Tejashree Patil on 14 May 2014,
				ISNULL(VWC.MainCampaignName+' ' + VWC.SubCampaignName,'-') AS CampaignName
			FROM 
				DEALERS as D WITH (NOLOCK)
				INNER JOIN TC_LeadBasedSummary TBS WITH (NOLOCK) ON D.ID = TBS.DealerId
				INNER JOIN TC_BrandZone TBZ WITH (NOLOCK) ON D.TC_BrandZoneId = TBZ.TC_BrandZoneId AND TBZ.MakeId = @Makeid 
				LEFT JOIN TC_SpecialUsers TSU WITH (NOLOCK) ON D.TC_RMId = TSU.TC_SpecialUsersId AND TSU.IsActive = 1
				LEFT JOIN TC_SpecialUsers TSU1 WITH (NOLOCK) ON D.TC_AMId = TSU1.TC_SpecialUsersId AND TSU1.IsActive = 1
				LEFT JOIN TC_Lead TL(NOLOCK) ON TBS.TC_LeadId = TL.TC_LeadId
				LEFT JOIN TC_CustomerDetails TC(NOLOCK) ON TL.TC_CustomerId = TC.Id
				LEFT JOIN TC_Users TU(NOLOCK) ON TU.Id = TBS.TC_UsersId
				LEFT JOIN TC_LeadDisposition TLD(NOLOCK) ON TLD.TC_LeadDispositionId = TBS.InquiryDispositionId
				LEFT JOIN TC_LeadDisposition AS TCLD WITH (NOLOCK) ON  TCLD.TC_LeadDispositionId=TBS.BookingStatus  ------ modified by manish on 17-09-2013 taking dispostion for booking status
				LEFT JOIN TC_LeadSubDisposition TSD(NOLOCK) ON TSD.TC_LeadSubDispositionId = TBS.InquirySubDispositionId 
				LEFT JOIN vwMMV VW(NOLOCK) ON VW.VersionId = TBS.LostVersionId
				LEFT JOIN vwMMV VWW(NOLOCK) ON VWW.VersionId = TBS.CarVersionId--Added By Sachin Bharti(19th Sept 2013)
				LEFT JOIN Cities C(NOLOCK) ON C.ID = TBS.InquiryCityId -- Changed By Sachin Bharti(19th Sept 2013)
				LEFT JOIN TC_VersionsCode TV(NOLOCK) ON TV.CarVersionId = TBS.CarVersionId
				LEFT JOIN TC_VersionColourCode VC(NOLOCK) ON VC.VersionColorsId = TBS.BookedCarColourId --Added by Sachin Bharti (10th Oct 2013)
				--LEFT JOIN TC_ExchangeNewCar TENC WITH(NOLOCK) ON TENC.TC_NewCarInquiriesId=TBS.TC_NewCarInquiriesId --Added by Vivek Singh (22nd jan 2014)
				LEFT JOIN vwMMV VEX WITH(NOLOCK) ON VEX.VersionId = TBS.ExchangeCarVersionId  --Added by Vivek Singh (22nd jan 2014)
				--LEFT JOIN TC_NewCarBooking  AS NB   WITH(NOLOCK) ON NB.TC_NewCarInquiriesId = TBS.TC_NewCarInquiriesId --Added by Vivek Singh (22nd jan 2014)
				LEFT JOIN TC_CampaignScheduling CS WITH(NOLOCK) ON CS.TC_CampaignSchedulingId=TBS.NSCCampaignSchedulingId --By Tejashree on 14/5/2014
				LEFT JOIN TC_vwCampaignMaster VWC WITH(NOLOCK) ON VWC.TC_SubCampaignId=CS.TC_SubCampaignId
			WHERE TBS.BookingStatus=32 AND TBS.TC_LeadDispositionID=4 AND D.IsDealerActive= 1  AND (D.TC_RMID = @RMID OR @RMID IS NULL) AND (D.TC_AMId = @AMID OR @AMID IS NULL)
				AND TBS.BookingDate BETWEEN @FromDate AND @ToDate AND (D.ID =@DealerId OR @DealerId IS NULL)
		END
	--Live Bookings
	IF @LeadType = 5 
		BEGIN
				SELECT DISTINCT 
				REPLACE(ISNULL(TSU.UserName,'-'), CHAR(9), '')AS RM, 
				REPLACE(ISNULL(TSU1.UserName,'-'), CHAR(9), '')AS AM, 
				REPLACE(ISNULL(D.Organization,'-'), CHAR(9), '') AS DealerName,  
				ISNULL(D.DealerCode, '-') DealerCode,				
				ISNULL(TC.UniqueCustomerId,'-')AS UniqueCustomerId,
				dbo.RemoveSpecialCharsExceptBlank(ISNULL(TC.CustomerName, '-')) AS CustomerName, 
				ISNULL(TC.Mobile, '-') AS Mobile, 
				ISNULL(TC.Email,'-') AS Email,
				REPLACE(ISNULL(TBS.Source,'-'), CHAR(9), '') AS Source, 
				REPLACE(ISNULL(TBS.Eagerness,'-'), CHAR(9), '')AS Eagerness,
				REPLACE(ISNULL(TBS.CompanyName,'-'), CHAR(9), '')AS CompanyName,
				--REPLACE(ISNULL(TBS.LatestCarName,'-'), CHAR(9), '')AS LatestCarName,
				REPLACE(ISNULL(VWW.Car,'-'), CHAR(9), '')AS CarName,--Added by Sachin Bharti(19th Sept 2013)
				TBS.InquiryCreationDate AS InquiryCreationDate ,--Added by Sachin Bharti(19th Sept 2013)
				TBS.CreatedDate AS LeadCreationDate,--Added by Sachin Bharti(19th Sept 2013)
				ISNULL(C.Name,'-')AS City,
				REPLACE(ISNULL(TU.UserName,'-'), CHAR(9), '') AS UserConsultant,
				dbo.RemoveSpecialCharsExceptBlank(ISNULL(TBS.LastCallComment,'-')) AS LastCallComment,
				TBS.TestDriveDate AS TestDriveDate,
				--(SELECT COUNT(*) FROM TC_TDCalendar TD WHERE TD.TC_NewCarInquiriesId= TBS.TC_NewCarInquiriesId)  AS NumberOfTDs,--Added by vivek singh (22nd jan 2014)
				TBS.BookingEntryDate BookingEntryDate,
				TBS.BookingDate AS BookingDate,
				TBS.BookingAmt AS BookingAmt,
				REPLACE(ISNULL(TCLD.Name,'-'),CHAR(9),'') AS BookingStatus, ---- modified by manish on 17-09-2013 changing table TLD to TCLD
				TBS.BookingCancelDate,  
				TBS.PromisedDeliveryDate AS PromisedDeliveryDate,
				TBS.DeliveryEntryDate AS DeliveryEntryDate,
				TBS.CarDeliveryDate AS CarDeliveryDate,
				dbo.RemoveSpecialCharsExceptBlank(ISNULL(TBS.EngineNumber,'-')) AS EngineNumber,
				dbo.RemoveSpecialCharsExceptBlank(ISNULL(TBS.InsuranceCoverNumber,'-'))AS InsuranceCoverNumber,
				dbo.RemoveSpecialCharsExceptBlank(ISNULL(TBS.InvoiceNumber,'-')) AS InvoiceNumber,
				TBS.InvoiceDate AS RetailDate,ISNULL(TV.CarVersionCode,'-')AS ModelCode,
				--dbo.RemoveSpecialCharsExceptBlank(ISNULL(TBS.InvoiceNumber,'-')) AS InvoiceNumber,
				dbo.RemoveSpecialCharsExceptBlank(ISNULL(TBS.RegistrationNo,'-'))AS RegistrationNo,
				dbo.RemoveSpecialCharsExceptBlank(ISNULL(TBS.PanNo,'-'))AS PanNo,
				dbo.RemoveSpecialCharsExceptBlank(ISNULL(TBS.VinNO,'-')) AS VinNO, 
				ISNULL(TBS.BookingName,'-') AS RetailCustomerName,    --Added by vivek singh (22nd jan 2014)
			    ISNULL(TBS.BookingMobile,'-') AS RetailCustomerMobileNo,
				ISNULL(TBS.BookingEmail,'-') AS RetailCustomerEmail,   
				TBS.InquiryDispositionDate AS LostDate,      
				ISNULL(TLD.Name,'-') AS LostReason,
				ISNULL(VW.Make,'-') AS LostToMake ,
				ISNULL(VW.Model,'-') AS LostToModel,
				ISNULL(VW.Version,'-') AS LostToVersion, 
				ISNULL(TSD.SubDispositionName,'-') AS LostSubDisposition,
				--Added by Sachin Bharti(10th Oct 2013)
				ISNULL(VC.ColorCode,'-') AS ModelColorCode,
				ISNULL(TBS.BookedCarMakeYear,'-')AS ModelYear,
				ISNULL(VEX.Make,'-') AS ExchangeCarMake,      --Added by vivek singh (22nd jan 2014)
			    ISNULL(VEX.Model,'-') AS ExchangeCarModel,
				ISNULL(VEX.Version,'-') AS ExchangeCarversion,
				TBS.ExchangeCarMakeYear AS ExchangeCarYear,
				ISNULL(TBS.ExchangeCarKms,'-') AS ExchangeCarKMdriven,
				ISNULL(TBS.ExchangeCarExpectedPrice,'-') AS ExchangeCarExpectedPrice,
				ISNULL(TU.VTONumbers,'-') AS VTONumbers,-- Modified By: Tejashree Patil on 14 May 2014,
				ISNULL(VWC.MainCampaignName+' ' + VWC.SubCampaignName,'-') AS CampaignName
			FROM 
				DEALERS as D WITH (NOLOCK)
				INNER JOIN TC_LeadBasedSummary TBS WITH (NOLOCK) ON D.ID = TBS.DealerId
				INNER JOIN TC_BrandZone TBZ WITH (NOLOCK) ON D.TC_BrandZoneId = TBZ.TC_BrandZoneId AND TBZ.MakeId = @Makeid 
				LEFT JOIN TC_SpecialUsers TSU WITH (NOLOCK) ON D.TC_RMId = TSU.TC_SpecialUsersId AND TSU.IsActive = 1
				LEFT JOIN TC_SpecialUsers TSU1 WITH (NOLOCK) ON D.TC_AMId = TSU1.TC_SpecialUsersId AND TSU1.IsActive = 1
				LEFT JOIN TC_Lead TL(NOLOCK) ON TBS.TC_LeadId = TL.TC_LeadId
				LEFT JOIN TC_CustomerDetails TC(NOLOCK) ON TL.TC_CustomerId = TC.Id
				LEFT JOIN TC_Users TU(NOLOCK) ON TU.Id = TBS.TC_UsersId
				LEFT JOIN TC_LeadDisposition TLD(NOLOCK) ON TLD.TC_LeadDispositionId = TBS.InquiryDispositionId
				LEFT JOIN TC_LeadDisposition AS TCLD WITH (NOLOCK) ON  TCLD.TC_LeadDispositionId=TBS.BookingStatus  ------ modified by manish on 17-09-2013 taking dispostion for booking status
				LEFT JOIN TC_LeadSubDisposition TSD(NOLOCK) ON TSD.TC_LeadSubDispositionId = TBS.InquirySubDispositionId 
				LEFT JOIN vwMMV VW(NOLOCK) ON VW.VersionId = TBS.LostVersionId
				LEFT JOIN vwMMV VWW(NOLOCK) ON VWW.VersionId = TBS.CarVersionId--Added By Sachin Bharti(19th Sept 2013)
				LEFT JOIN Cities C(NOLOCK) ON C.ID = TBS.InquiryCityId -- Changed By Sachin Bharti(19th Sept 2013)
				LEFT JOIN TC_VersionsCode TV(NOLOCK) ON TV.CarVersionId = TBS.CarVersionId
				LEFT JOIN TC_VersionColourCode VC(NOLOCK) ON VC.VersionColorsId = TBS.BookedCarColourId --Added by Sachin Bharti (10th Oct 2013)
				--LEFT JOIN TC_ExchangeNewCar TENC WITH(NOLOCK) ON TENC.TC_NewCarInquiriesId=TBS.TC_NewCarInquiriesId --Added by Vivek Singh (22nd jan 2014)
				LEFT JOIN vwMMV VEX WITH(NOLOCK) ON VEX.VersionId = TBS.ExchangeCarVersionId  --Added by Vivek Singh (22nd jan 2014)
				--LEFT JOIN TC_NewCarBooking  AS NB   WITH(NOLOCK) ON NB.TC_NewCarInquiriesId = TBS.TC_NewCarInquiriesId --Added by Vivek Singh (22nd jan 2014)
				LEFT JOIN TC_CampaignScheduling CS WITH(NOLOCK) ON CS.TC_CampaignSchedulingId=TBS.NSCCampaignSchedulingId --By Tejashree on 14/5/2014
				LEFT JOIN TC_vwCampaignMaster VWC WITH(NOLOCK) ON VWC.TC_SubCampaignId=CS.TC_SubCampaignId
			WHERE ISNULL(TBS.BookingStatus,0)=32 
			AND ISNULL(TBS.CarDeliveryStatus,0) <> 77 
			AND TBS.TC_LeadStageId<>3
			AND TBS.BookingDate <= @ToDate  --Added (TBS.BookingDate <= @ToDate) by Vivek Singh on 11th Nov 2013 
		    AND (D.TC_RMID = @RMID OR @RMID IS NULL) 
			AND (D.TC_AMId = @AMID OR @AMID IS NULL)
			AND (D.ID =@DealerId OR @DealerId IS NULL)
			AND TBS.InvoiceDate IS NULL --- Condition added by Sachin on 01-10-2013 since in live booking we don't need to capture retail data
		END
	--Delivery Leads
	IF @LeadType = 6
		BEGIN
				SELECT DISTINCT 
				REPLACE(ISNULL(TSU.UserName,'-'), CHAR(9), '')AS RM, 
				REPLACE(ISNULL(TSU1.UserName,'-'), CHAR(9), '')AS AM, 
				REPLACE(ISNULL(D.Organization,'-'), CHAR(9), '') AS DealerName,  
				ISNULL(D.DealerCode, '-') DealerCode,				
				ISNULL(TC.UniqueCustomerId,'-')AS UniqueCustomerId,
				dbo.RemoveSpecialCharsExceptBlank(ISNULL(TC.CustomerName, '-')) AS CustomerName, 
				ISNULL(TC.Mobile, '-') AS Mobile, 
				ISNULL(TC.Email,'-') AS Email,
				REPLACE(ISNULL(TBS.Source,'-'), CHAR(9), '') AS Source, 
				REPLACE(ISNULL(TBS.Eagerness,'-'), CHAR(9), '')AS Eagerness,
				REPLACE(ISNULL(TBS.CompanyName,'-'), CHAR(9), '')AS CompanyName,
				--REPLACE(ISNULL(TBS.LatestCarName,'-'), CHAR(9), '')AS LatestCarName,
				REPLACE(ISNULL(VWW.Car,'-'), CHAR(9), '')AS CarName,--Added by Sachin Bharti(19th Sept 2013)
				TBS.InquiryCreationDate AS InquiryCreationDate ,--Added by Sachin Bharti(19th Sept 2013)
				TBS.CreatedDate AS LeadCreationDate,--Added by Sachin Bharti(19th Sept 2013)
				ISNULL(C.Name,'-')AS City,
				REPLACE(ISNULL(TU.UserName,'-'), CHAR(9), '') AS UserConsultant,
				dbo.RemoveSpecialCharsExceptBlank(ISNULL(TBS.LastCallComment,'-')) AS LastCallComment,
				TBS.TestDriveDate AS TestDriveDate,
				--(SELECT COUNT(*) FROM TC_TDCalendar TD WHERE TD.TC_NewCarInquiriesId= TBS.TC_NewCarInquiriesId)  AS NumberOfTDs,--Added by vivek singh (22nd jan 2014)
				TBS.BookingEntryDate BookingEntryDate,
				TBS.BookingDate AS BookingDate,
				TBS.BookingAmt AS BookingAmt,
				REPLACE(ISNULL(TCLD.Name,'-'),CHAR(9),'') AS BookingStatus, ---- modified by manish on 17-09-2013 changing table TLD to TCLD
				TBS.BookingCancelDate,  
				TBS.PromisedDeliveryDate AS PromisedDeliveryDate,
				TBS.DeliveryEntryDate AS DeliveryEntryDate,
				TBS.CarDeliveryDate AS CarDeliveryDate,
				dbo.RemoveSpecialCharsExceptBlank(ISNULL(TBS.EngineNumber,'-')) AS EngineNumber,
				dbo.RemoveSpecialCharsExceptBlank(ISNULL(TBS.InsuranceCoverNumber,'-'))AS InsuranceCoverNumber,
				dbo.RemoveSpecialCharsExceptBlank(ISNULL(TBS.InvoiceNumber,'-')) AS InvoiceNumber,
				TBS.InvoiceDate AS RetailDate,ISNULL(TV.CarVersionCode,'-')AS ModelCode,
				--dbo.RemoveSpecialCharsExceptBlank(ISNULL(TBS.InvoiceNumber,'-')) AS InvoiceNumber,
				dbo.RemoveSpecialCharsExceptBlank(ISNULL(TBS.RegistrationNo,'-'))AS RegistrationNo,
				dbo.RemoveSpecialCharsExceptBlank(ISNULL(TBS.PanNo,'-'))AS PanNo,
				dbo.RemoveSpecialCharsExceptBlank(ISNULL(TBS.VinNO,'-')) AS VinNO,
				ISNULL(TBS.BookingName,'-') AS RetailCustomerName,    --Added by vivek singh (22nd jan 2014)
			    ISNULL(TBS.BookingMobile,'-') AS RetailCustomerMobileNo,
				ISNULL(TBS.BookingEmail,'-') AS RetailCustomerEmail,   
				TBS.InquiryDispositionDate AS LostDate,      
				ISNULL(TLD.Name,'-') AS LostReason,
				ISNULL(VW.Make,'-') AS LostToMake ,
				ISNULL(VW.Model,'-') AS LostToModel,
				ISNULL(VW.Version,'-') AS LostToVersion, 
				ISNULL(TSD.SubDispositionName,'-') AS LostSubDisposition,
				--Added by Sachin Bharti(10th Oct 2013)
				ISNULL(VC.ColorCode,'-') AS ModelColorCode,
				ISNULL(TBS.BookedCarMakeYear,'-')AS ModelYear,
				ISNULL(VEX.Make,'-') AS ExchangeCarMake,      --Added by vivek singh (22nd jan 2014)
			    ISNULL(VEX.Model,'-') AS ExchangeCarModel,
				ISNULL(VEX.Version,'-') AS ExchangeCarversion,
				TBS.ExchangeCarMakeYear AS ExchangeCarYear,
				ISNULL(TBS.ExchangeCarKms,'-') AS ExchangeCarKMdriven,
				ISNULL(TBS.ExchangeCarExpectedPrice,'-') AS ExchangeCarExpectedPrice,
				ISNULL(TU.VTONumbers,'-') AS VTONumbers,-- Modified By: Tejashree Patil on 14 May 2014,
				ISNULL(VWC.MainCampaignName+' ' + VWC.SubCampaignName,'-') AS CampaignName
			FROM 
				DEALERS as D WITH (NOLOCK)
				INNER JOIN TC_LeadBasedSummary TBS WITH (NOLOCK) ON D.ID = TBS.DealerId
				INNER JOIN TC_BrandZone TBZ WITH (NOLOCK) ON D.TC_BrandZoneId = TBZ.TC_BrandZoneId AND TBZ.MakeId = @Makeid 
				LEFT JOIN TC_SpecialUsers TSU WITH (NOLOCK) ON D.TC_RMId = TSU.TC_SpecialUsersId AND TSU.IsActive = 1
				LEFT JOIN TC_SpecialUsers TSU1 WITH (NOLOCK) ON D.TC_AMId = TSU1.TC_SpecialUsersId AND TSU1.IsActive = 1
				LEFT JOIN TC_Lead TL(NOLOCK) ON TBS.TC_LeadId = TL.TC_LeadId
				LEFT JOIN TC_CustomerDetails TC(NOLOCK) ON TL.TC_CustomerId = TC.Id
				LEFT JOIN TC_Users TU(NOLOCK) ON TU.Id = TBS.TC_UsersId
				LEFT JOIN TC_LeadDisposition TLD(NOLOCK) ON TLD.TC_LeadDispositionId = TBS.InquiryDispositionId
				LEFT JOIN TC_LeadDisposition AS TCLD WITH (NOLOCK) ON  TCLD.TC_LeadDispositionId=TBS.BookingStatus  ------ modified by manish on 17-09-2013 taking dispostion for booking status
				LEFT JOIN TC_LeadSubDisposition TSD(NOLOCK) ON TSD.TC_LeadSubDispositionId = TBS.InquirySubDispositionId 
				LEFT JOIN vwMMV VW(NOLOCK) ON VW.VersionId = TBS.LostVersionId
				LEFT JOIN vwMMV VWW(NOLOCK) ON VWW.VersionId = TBS.CarVersionId--Added By Sachin Bharti(19th Sept 2013)
				LEFT JOIN Cities C(NOLOCK) ON C.ID = TBS.InquiryCityId -- Changed By Sachin Bharti(19th Sept 2013)
				LEFT JOIN TC_VersionsCode TV(NOLOCK) ON TV.CarVersionId = TBS.CarVersionId
				LEFT JOIN TC_VersionColourCode VC(NOLOCK) ON VC.VersionColorsId = TBS.BookedCarColourId --Added by Sachin Bharti (10th Oct 2013)
				--LEFT JOIN TC_ExchangeNewCar TENC WITH(NOLOCK) ON TENC.TC_NewCarInquiriesId=TBS.TC_NewCarInquiriesId --Added by Vivek Singh (22nd jan 2014)
				LEFT JOIN vwMMV VEX WITH(NOLOCK) ON VEX.VersionId = TBS.ExchangeCarVersionId  --Added by Vivek Singh (22nd jan 2014)				
				--LEFT JOIN TC_NewCarBooking  AS NB   WITH(NOLOCK) ON NB.TC_NewCarInquiriesId = TBS.TC_NewCarInquiriesId --Added by Vivek Singh (22nd jan 2014)
				LEFT JOIN TC_CampaignScheduling CS WITH(NOLOCK) ON CS.TC_CampaignSchedulingId=TBS.NSCCampaignSchedulingId --By Tejashree on 14/5/2014
				LEFT JOIN TC_vwCampaignMaster VWC WITH(NOLOCK) ON VWC.TC_SubCampaignId=CS.TC_SubCampaignId
			WHERE TBS.CarDeliveryStatus = 77 AND TBS.CarDeliveryDate BETWEEN @FromDate AND @ToDate 
				AND (D.TC_RMID = @RMID OR @RMID IS NULL) AND (D.TC_AMId = @AMID OR @AMID IS NULL)
				AND (D.ID =@DealerId OR @DealerId IS NULL)
		END
	--Lost Leads
	IF @LeadType = 7
		BEGIN
				SELECT DISTINCT
				REPLACE(ISNULL(TSU.UserName,'-'), CHAR(9), '')AS RM, 
				REPLACE(ISNULL(TSU1.UserName,'-'), CHAR(9), '')AS AM, 
				REPLACE(ISNULL(D.Organization,'-'), CHAR(9), '') AS DealerName,  
				ISNULL(D.DealerCode, '-') DealerCode,				
				ISNULL(TC.UniqueCustomerId,'-')AS UniqueCustomerId,
				dbo.RemoveSpecialCharsExceptBlank(ISNULL(TC.CustomerName, '-')) AS CustomerName, 
				ISNULL(TC.Mobile, '-') AS Mobile, 
				ISNULL(TC.Email,'-') AS Email,
				REPLACE(ISNULL(TBS.Source,'-'), CHAR(9), '') AS Source, 
				REPLACE(ISNULL(TBS.Eagerness,'-'), CHAR(9), '')AS Eagerness,
				REPLACE(ISNULL(TBS.CompanyName,'-'), CHAR(9), '')AS CompanyName,
				--REPLACE(ISNULL(TBS.LatestCarName,'-'), CHAR(9), '')AS LatestCarName,
				REPLACE(ISNULL(VWW.Car,'-'), CHAR(9), '')AS CarName,--Added by Sachin Bharti(19th Sept 2013)
				TBS.InquiryCreationDate AS InquiryCreationDate ,--Added by Sachin Bharti(19th Sept 2013)
				TBS.CreatedDate AS LeadCreationDate,--Added by Sachin Bharti(19th Sept 2013)
				ISNULL(C.Name,'-')AS City,
				REPLACE(ISNULL(TU.UserName,'-'), CHAR(9), '') AS UserConsultant,
				dbo.RemoveSpecialCharsExceptBlank(ISNULL(TBS.LastCallComment,'-')) AS LastCallComment,
				TBS.TestDriveDate AS TestDriveDate,
				--(SELECT COUNT(*) FROM TC_TDCalendar TD WHERE TD.TC_NewCarInquiriesId= TBS.TC_NewCarInquiriesId)  AS NumberOfTDs,--Added by vivek singh (22nd jan 2014)
				TBS.BookingEntryDate BookingEntryDate,
				TBS.BookingDate AS BookingDate,
				TBS.BookingAmt AS BookingAmt,
				REPLACE(ISNULL(TCLD.Name,'-'),CHAR(9),'') AS BookingStatus, ---- modified by manish on 17-09-2013 changing table TLD to TCLD
				TBS.BookingCancelDate,  
				TBS.PromisedDeliveryDate AS PromisedDeliveryDate,
				TBS.DeliveryEntryDate AS DeliveryEntryDate,
				TBS.CarDeliveryDate AS CarDeliveryDate,
				dbo.RemoveSpecialCharsExceptBlank(ISNULL(TBS.EngineNumber,'-')) AS EngineNumber,
				dbo.RemoveSpecialCharsExceptBlank(ISNULL(TBS.InsuranceCoverNumber,'-'))AS InsuranceCoverNumber,
				dbo.RemoveSpecialCharsExceptBlank(ISNULL(TBS.InvoiceNumber,'-')) AS InvoiceNumber,
				TBS.InvoiceDate AS RetailDate,ISNULL(TV.CarVersionCode,'-')AS ModelCode,
				--dbo.RemoveSpecialCharsExceptBlank(ISNULL(TBS.InvoiceNumber,'-')) AS InvoiceNumber,
				dbo.RemoveSpecialCharsExceptBlank(ISNULL(TBS.RegistrationNo,'-'))AS RegistrationNo,
				dbo.RemoveSpecialCharsExceptBlank(ISNULL(TBS.PanNo,'-'))AS PanNo,
				dbo.RemoveSpecialCharsExceptBlank(ISNULL(TBS.VinNO,'-')) AS VinNO, 
				ISNULL(TBS.BookingName,'-') AS RetailCustomerName,    --Added by vivek singh (22nd jan 2014)
			    ISNULL(TBS.BookingMobile,'-') AS RetailCustomerMobileNo,
				ISNULL(TBS.BookingEmail,'-') AS RetailCustomerEmail,   
				TBS.InquiryDispositionDate AS LostDate,          
				ISNULL(TLD.Name,'-') AS LostReason,
				ISNULL(VW.Make,'-') AS LostToMake ,
				ISNULL(VW.Model,'-') AS LostToModel,
				ISNULL(VW.Version,'-') AS LostToVersion, 
				ISNULL(TSD.SubDispositionName,'-') AS LostSubDisposition,
				--Added by Sachin Bharti(10th Oct 2013)
				ISNULL(VC.ColorCode,'-') AS ModelColorCode,
				ISNULL(TBS.BookedCarMakeYear,'-')AS ModelYear,
				ISNULL(VEX.Make,'-') AS ExchangeCarMake,      --Added by vivek singh (22nd jan 2014)
			    ISNULL(VEX.Model,'-') AS ExchangeCarModel,
				ISNULL(VEX.Version,'-') AS ExchangeCarversion,
				TBS.ExchangeCarMakeYear AS ExchangeCarYear,
				ISNULL(TBS.ExchangeCarKms,'-') AS ExchangeCarKMdriven,
				ISNULL(TBS.ExchangeCarExpectedPrice,'-') AS ExchangeCarExpectedPrice,
				ISNULL(TU.VTONumbers,'-') AS VTONumbers,-- Modified By: Tejashree Patil on 14 May 2014,
				ISNULL(VWC.MainCampaignName+' ' + VWC.SubCampaignName,'-') AS CampaignName
			FROM 
				DEALERS as D WITH (NOLOCK)
				INNER JOIN TC_LeadBasedSummary TBS WITH (NOLOCK) ON D.ID = TBS.DealerId
				INNER JOIN TC_BrandZone TBZ WITH (NOLOCK) ON D.TC_BrandZoneId = TBZ.TC_BrandZoneId AND TBZ.MakeId = @Makeid 
				LEFT JOIN TC_SpecialUsers TSU WITH (NOLOCK) ON D.TC_RMId = TSU.TC_SpecialUsersId AND TSU.IsActive = 1
				LEFT JOIN TC_SpecialUsers TSU1 WITH (NOLOCK) ON D.TC_AMId = TSU1.TC_SpecialUsersId AND TSU1.IsActive = 1
				LEFT JOIN TC_Lead TL(NOLOCK) ON TBS.TC_LeadId = TL.TC_LeadId
				LEFT JOIN TC_CustomerDetails TC(NOLOCK) ON TL.TC_CustomerId = TC.Id
				LEFT JOIN TC_Users TU(NOLOCK) ON TU.Id = TBS.TC_UsersId
				LEFT JOIN TC_LeadDisposition TLD(NOLOCK) ON TLD.TC_LeadDispositionId = TBS.InquiryDispositionId
				LEFT JOIN TC_LeadDisposition AS TCLD WITH (NOLOCK) ON  TCLD.TC_LeadDispositionId=TBS.BookingStatus  ------ modified by manish on 17-09-2013 taking dispostion for booking status
				LEFT JOIN TC_LeadSubDisposition TSD(NOLOCK) ON TSD.TC_LeadSubDispositionId = TBS.InquirySubDispositionId 
				LEFT JOIN vwMMV VW(NOLOCK) ON VW.VersionId = TBS.LostVersionId
				LEFT JOIN vwMMV VWW(NOLOCK) ON VWW.VersionId = TBS.CarVersionId--Added By Sachin Bharti(19th Sept 2013)
				LEFT JOIN Cities C(NOLOCK) ON C.ID = TBS.InquiryCityId -- Changed By Sachin Bharti(19th Sept 2013)
				LEFT JOIN TC_VersionsCode TV(NOLOCK) ON TV.CarVersionId = TBS.CarVersionId
				LEFT JOIN TC_VersionColourCode VC(NOLOCK) ON VC.VersionColorsId = TBS.BookedCarColourId --Added by Sachin Bharti (10th Oct 2013)
				--LEFT JOIN TC_ExchangeNewCar TENC WITH(NOLOCK) ON TENC.TC_NewCarInquiriesId=TBS.TC_NewCarInquiriesId --Added by Vivek Singh (22nd jan 2014)
				LEFT JOIN vwMMV VEX WITH(NOLOCK) ON VEX.VersionId = TBS.ExchangeCarVersionId --Added by Vivek Singh (22nd jan 2014)
				--LEFT JOIN TC_NewCarBooking  AS NB   WITH(NOLOCK) ON NB.TC_NewCarInquiriesId = TBS.TC_NewCarInquiriesId --Added by Vivek Singh (22nd jan 2014)
				LEFT JOIN TC_CampaignScheduling CS WITH(NOLOCK) ON CS.TC_CampaignSchedulingId=TBS.NSCCampaignSchedulingId --By Tejashree on 14/5/2014
				LEFT JOIN TC_vwCampaignMaster VWC WITH(NOLOCK) ON VWC.TC_SubCampaignId=CS.TC_SubCampaignId
			WHERE TBS.TC_LeadStageId=3 AND  TBS.TC_LeadDispositionID<>4 AND (D.TC_RMID = @RMID OR @RMID IS NULL) AND (D.TC_AMId = @AMID OR @AMID IS NULL)
					AND TBS.LeadClosedDate BETWEEN @FromDate AND @ToDate
					AND (D.ID =@DealerId OR @DealerId IS NULL)
		END
		
	--Cancelled Leads
	IF @LeadType = 8
		BEGIN
				SELECT DISTINCT
				REPLACE(ISNULL(TSU.UserName,'-'), CHAR(9), '')AS RM, 
				REPLACE(ISNULL(TSU1.UserName,'-'), CHAR(9), '')AS AM, 
				REPLACE(ISNULL(D.Organization,'-'), CHAR(9), '') AS DealerName,  
				ISNULL(D.DealerCode, '-') DealerCode,				
				ISNULL(TC.UniqueCustomerId,'-')AS UniqueCustomerId,
				dbo.RemoveSpecialCharsExceptBlank(ISNULL(TC.CustomerName, '-')) AS CustomerName, 
				ISNULL(TC.Mobile, '-') AS Mobile, 
				ISNULL(TC.Email,'-') AS Email,
				REPLACE(ISNULL(TBS.Source,'-'), CHAR(9), '') AS Source, 
				REPLACE(ISNULL(TBS.Eagerness,'-'), CHAR(9), '')AS Eagerness,
				REPLACE(ISNULL(TBS.CompanyName,'-'), CHAR(9), '')AS CompanyName,
				--REPLACE(ISNULL(TBS.LatestCarName,'-'), CHAR(9), '')AS LatestCarName,
				REPLACE(ISNULL(VWW.Car,'-'), CHAR(9), '')AS CarName,--Added by Sachin Bharti(19th Sept 2013)
				TBS.InquiryCreationDate AS InquiryCreationDate ,--Added by Sachin Bharti(19th Sept 2013)
				TBS.CreatedDate AS LeadCreationDate,--Added by Sachin Bharti(19th Sept 2013)
				ISNULL(C.Name,'-')AS City,
				REPLACE(ISNULL(TU.UserName,'-'), CHAR(9), '') AS UserConsultant,
				dbo.RemoveSpecialCharsExceptBlank(ISNULL(TBS.LastCallComment,'-')) AS LastCallComment,
				TBS.TestDriveDate AS TestDriveDate,
				--(SELECT COUNT(*) FROM TC_TDCalendar TD WHERE TD.TC_NewCarInquiriesId= TBS.TC_NewCarInquiriesId)  AS NumberOfTDs,--Added by vivek singh (22nd jan 2014)
				TBS.BookingEntryDate BookingEntryDate,
				TBS.BookingDate AS BookingDate,
				TBS.BookingAmt AS BookingAmt,
				REPLACE(ISNULL(TCLD.Name,'-'),CHAR(9),'') AS BookingStatus, ---- modified by manish on 17-09-2013 changing table TLD to TCLD
				TBS.BookingCancelDate,
				TBS.PromisedDeliveryDate AS PromisedDeliveryDate,
				TBS.DeliveryEntryDate AS DeliveryEntryDate,
				TBS.CarDeliveryDate AS CarDeliveryDate,
				dbo.RemoveSpecialCharsExceptBlank(ISNULL(TBS.EngineNumber,'-')) AS EngineNumber,
				dbo.RemoveSpecialCharsExceptBlank(ISNULL(TBS.InsuranceCoverNumber,'-'))AS InsuranceCoverNumber,
				dbo.RemoveSpecialCharsExceptBlank(ISNULL(TBS.InvoiceNumber,'-')) AS InvoiceNumber,
				TBS.InvoiceDate AS RetailDate,ISNULL(TV.CarVersionCode,'-')AS ModelCode,
				--dbo.RemoveSpecialCharsExceptBlank(ISNULL(TBS.InvoiceNumber,'-')) AS InvoiceNumber,
				dbo.RemoveSpecialCharsExceptBlank(ISNULL(TBS.RegistrationNo,'-'))AS RegistrationNo,
				dbo.RemoveSpecialCharsExceptBlank(ISNULL(TBS.PanNo,'-'))AS PanNo,
				dbo.RemoveSpecialCharsExceptBlank(ISNULL(TBS.VinNO,'-')) AS VinNO, 
				ISNULL(TBS.BookingName,'-') AS RetailCustomerName,    --Added by vivek singh (22nd jan 2014)
			    ISNULL(TBS.BookingMobile,'-') AS RetailCustomerMobileNo,
				ISNULL(TBS.BookingEmail,'-') AS RetailCustomerEmail,   
				TBS.InquiryDispositionDate AS LostDate,      
				ISNULL(TLD.Name,'-') AS LostReason,
				ISNULL(VW.Make,'-') AS LostToMake ,
				ISNULL(VW.Model,'-') AS LostToModel,
				ISNULL(VW.Version,'-') AS LostToVersion, 
				TBS.InquiryDispositionDate AS LostDate,      
				--Added by Sachin Bharti(10th Oct 2013)
				ISNULL(VC.ColorCode,'-') AS ModelColorCode,
				ISNULL(TBS.BookedCarMakeYear,'-')AS ModelYear,
				ISNULL(VEX.Make,'-') AS ExchangeCarMake,      --Added by vivek singh (22nd jan 2014)
			    ISNULL(VEX.Model,'-') AS ExchangeCarModel,
				ISNULL(VEX.Version,'-') AS ExchangeCarversion,
				TBS.ExchangeCarMakeYear AS ExchangeCarYear,
				ISNULL(TBS.ExchangeCarKms,'-') AS ExchangeCarKMdriven,
				ISNULL(TBS.ExchangeCarExpectedPrice,'-') AS ExchangeCarExpectedPrice,
				ISNULL(TU.VTONumbers,'-') AS VTONumbers,-- Modified By: Tejashree Patil on 14 May 2014,
				ISNULL(VWC.MainCampaignName+' ' + VWC.SubCampaignName,'-') AS CampaignName
			FROM 
				DEALERS as D WITH (NOLOCK)
				INNER JOIN TC_LeadBasedSummary TBS WITH (NOLOCK) ON D.ID = TBS.DealerId
				INNER JOIN TC_BrandZone TBZ WITH (NOLOCK) ON D.TC_BrandZoneId = TBZ.TC_BrandZoneId AND TBZ.MakeId = @Makeid 
				LEFT JOIN TC_SpecialUsers TSU WITH (NOLOCK) ON D.TC_RMId = TSU.TC_SpecialUsersId AND TSU.IsActive = 1
				LEFT JOIN TC_SpecialUsers TSU1 WITH (NOLOCK) ON D.TC_AMId = TSU1.TC_SpecialUsersId AND TSU1.IsActive = 1
				LEFT JOIN TC_Lead TL(NOLOCK) ON TBS.TC_LeadId = TL.TC_LeadId
				LEFT JOIN TC_CustomerDetails TC(NOLOCK) ON TL.TC_CustomerId = TC.Id
				LEFT JOIN TC_Users TU(NOLOCK) ON TU.Id = TBS.TC_UsersId
				LEFT JOIN TC_LeadDisposition TLD(NOLOCK) ON TLD.TC_LeadDispositionId = TBS.InquiryDispositionId
				LEFT JOIN TC_LeadDisposition AS TCLD WITH (NOLOCK) ON  TCLD.TC_LeadDispositionId=TBS.BookingStatus  ------ modified by manish on 17-09-2013 taking dispostion for booking status
				LEFT JOIN TC_LeadSubDisposition TSD(NOLOCK) ON TSD.TC_LeadSubDispositionId = TBS.InquirySubDispositionId 
				LEFT JOIN vwMMV VW(NOLOCK) ON VW.VersionId = TBS.LostVersionId
				LEFT JOIN vwMMV VWW(NOLOCK) ON VWW.VersionId = TBS.CarVersionId--Added By Sachin Bharti(19th Sept 2013)
				LEFT JOIN Cities C(NOLOCK) ON C.ID = TBS.InquiryCityId -- Changed By Sachin Bharti(19th Sept 2013)
				LEFT JOIN TC_VersionsCode TV(NOLOCK) ON TV.CarVersionId = TBS.CarVersionId
				LEFT JOIN TC_VersionColourCode VC(NOLOCK) ON VC.VersionColorsId = TBS.BookedCarColourId --Added by Sachin Bharti (10th Oct 2013)
				--LEFT JOIN TC_ExchangeNewCar TENC WITH(NOLOCK) ON TENC.TC_NewCarInquiriesId=TBS.TC_NewCarInquiriesId --Added by Vivek Singh (22nd jan 2014)
				LEFT JOIN vwMMV VEX WITH(NOLOCK) ON VEX.VersionId = TBS.ExchangeCarVersionId  --Added by Vivek Singh (22nd jan 2014)
				--LEFT JOIN TC_NewCarBooking  AS NB   WITH(NOLOCK) ON NB.TC_NewCarInquiriesId = TBS.TC_NewCarInquiriesId --Added by Vivek Singh (22nd jan 2014)
				LEFT JOIN TC_CampaignScheduling CS WITH(NOLOCK) ON CS.TC_CampaignSchedulingId=TBS.NSCCampaignSchedulingId --By Tejashree on 14/5/2014
				LEFT JOIN TC_vwCampaignMaster VWC WITH(NOLOCK) ON VWC.TC_SubCampaignId=CS.TC_SubCampaignId
			WHERE ISNULL(TBS.BookingStatus,0)= 31 AND TBS.BookingCancelDate BETWEEN @FromDate AND @ToDate AND
					(D.TC_RMID = @RMID OR @RMID IS NULL) AND (D.TC_AMId = @AMID OR @AMID IS NULL)
					AND (D.ID =@DealerId OR @DealerId IS NULL)
		END

	-- Added by Sachin Bharti (10th Oct 2013)
	-- Retail Leads
	IF @LeadType = 9
		BEGIN
				SELECT DISTINCT
				REPLACE(ISNULL(TSU.UserName,'-'), CHAR(9), '')AS RM, 
				REPLACE(ISNULL(TSU1.UserName,'-'), CHAR(9), '')AS AM, 
				REPLACE(ISNULL(D.Organization,'-'), CHAR(9), '') AS DealerName,  
				ISNULL(D.DealerCode, '-') DealerCode,				
				ISNULL(TC.UniqueCustomerId,'-')AS UniqueCustomerId,
				dbo.RemoveSpecialCharsExceptBlank(ISNULL(TC.CustomerName, '-')) AS CustomerName, 
				ISNULL(TC.Mobile, '-') AS Mobile, 
				ISNULL(TC.Email,'-') AS Email,
				REPLACE(ISNULL(TBS.Source,'-'), CHAR(9), '') AS Source, 
				REPLACE(ISNULL(TBS.Eagerness,'-'), CHAR(9), '')AS Eagerness,
				REPLACE(ISNULL(TBS.CompanyName,'-'), CHAR(9), '')AS CompanyName,
				REPLACE(ISNULL(VWW.Car,'-'), CHAR(9), '')AS CarName,
				TBS.InquiryCreationDate AS InquiryCreationDate ,
				TBS.CreatedDate AS LeadCreationDate,
				ISNULL(C.Name,'-')AS City,
				REPLACE(ISNULL(TU.UserName,'-'), CHAR(9), '') AS UserConsultant,
				dbo.RemoveSpecialCharsExceptBlank(ISNULL(TBS.LastCallComment,'-')) AS LastCallComment,
				TBS.TestDriveDate AS TestDriveDate,
				--(SELECT COUNT(*) FROM TC_TDCalendar TD WHERE TD.TC_NewCarInquiriesId= TBS.TC_NewCarInquiriesId)  AS NumberOfTDs,--Added by vivek singh (22nd jan 2014)
				TBS.BookingEntryDate BookingEntryDate,
				TBS.BookingDate AS BookingDate,
				TBS.BookingAmt AS BookingAmt,
				REPLACE(ISNULL(TCLD.Name,'-'),CHAR(9),'') AS BookingStatus, 
				TBS.BookingCancelDate,
				TBS.PromisedDeliveryDate AS PromisedDeliveryDate,
				TBS.DeliveryEntryDate AS DeliveryEntryDate,
				TBS.CarDeliveryDate AS CarDeliveryDate,
				dbo.RemoveSpecialCharsExceptBlank(ISNULL(TBS.EngineNumber,'-')) AS EngineNumber,
				dbo.RemoveSpecialCharsExceptBlank(ISNULL(TBS.InsuranceCoverNumber,'-'))AS InsuranceCoverNumber,
				dbo.RemoveSpecialCharsExceptBlank(ISNULL(TBS.InvoiceNumber,'-')) AS InvoiceNumber,
				TBS.InvoiceDate AS RetailDate,ISNULL(TV.CarVersionCode,'-')AS ModelCode,
				--dbo.RemoveSpecialCharsExceptBlank(ISNULL(TBS.InvoiceNumber,'-')) AS InvoiceNumber,
				dbo.RemoveSpecialCharsExceptBlank(ISNULL(TBS.RegistrationNo,'-'))AS RegistrationNo,
				dbo.RemoveSpecialCharsExceptBlank(ISNULL(TBS.PanNo,'-'))AS PanNo,
				dbo.RemoveSpecialCharsExceptBlank(ISNULL(TBS.VinNO,'-')) AS VinNO, 
			    ISNULL(TBS.BookingName,'-') AS RetailCustomerName,    --Added by vivek singh (22nd jan 2014)
			    ISNULL(TBS.BookingMobile,'-') AS RetailCustomerMobileNo,
				ISNULL(TBS.BookingEmail,'-') AS RetailCustomerEmail,   
				TBS.InquiryDispositionDate AS LostDate,          
				ISNULL(TLD.Name,'-') AS LostReason,
				ISNULL(VW.Make,'-') AS LostToMake ,
				ISNULL(VW.Model,'-') AS LostToModel,
				ISNULL(VW.Version,'-') AS LostToVersion, 
				ISNULL(TSD.SubDispositionName,'-') AS LostSubDisposition,
				--Added by Sachin Bharti(10th Oct 2013)
				ISNULL(VC.ColorCode,'-') AS ModelColorCode,
				ISNULL(TBS.BookedCarMakeYear,'-')AS ModelYear,
				ISNULL(VEX.Make,'-') AS ExchangeCarMake,      --Added by vivek singh (22nd jan 2014)
			    ISNULL(VEX.Model,'-') AS ExchangeCarModel,
				ISNULL(VEX.Version,'-') AS ExchangeCarversion,
				TBS.ExchangeCarMakeYear AS ExchangeCarYear,
				ISNULL(TBS.ExchangeCarKms,'-') AS ExchangeCarKMdriven,
				ISNULL(TBS.ExchangeCarExpectedPrice,'-') AS ExchangeCarExpectedPrice,
				ISNULL(TU.VTONumbers,'-') AS VTONumbers,-- Modified By: Tejashree Patil on 14 May 2014,
				ISNULL(VWC.MainCampaignName+' ' + VWC.SubCampaignName,'-') AS CampaignName
			FROM 
				DEALERS as D WITH (NOLOCK)
				INNER JOIN TC_LeadBasedSummary TBS WITH (NOLOCK) ON D.ID = TBS.DealerId
				INNER JOIN TC_BrandZone TBZ WITH (NOLOCK) ON D.TC_BrandZoneId = TBZ.TC_BrandZoneId AND TBZ.MakeId = @Makeid 
				LEFT JOIN TC_SpecialUsers TSU WITH (NOLOCK) ON D.TC_RMId = TSU.TC_SpecialUsersId AND TSU.IsActive = 1
				LEFT JOIN TC_SpecialUsers TSU1 WITH (NOLOCK) ON D.TC_AMId = TSU1.TC_SpecialUsersId AND TSU1.IsActive = 1
				LEFT JOIN TC_Lead TL(NOLOCK) ON TBS.TC_LeadId = TL.TC_LeadId
				LEFT JOIN TC_CustomerDetails TC(NOLOCK) ON TL.TC_CustomerId = TC.Id
				LEFT JOIN TC_Users TU(NOLOCK) ON TU.Id = TBS.TC_UsersId
				LEFT JOIN TC_LeadDisposition TLD(NOLOCK) ON TLD.TC_LeadDispositionId = TBS.InquiryDispositionId
				LEFT JOIN TC_LeadDisposition AS TCLD WITH (NOLOCK) ON  TCLD.TC_LeadDispositionId=TBS.BookingStatus  
				LEFT JOIN TC_LeadSubDisposition TSD(NOLOCK) ON TSD.TC_LeadSubDispositionId = TBS.InquirySubDispositionId 
				LEFT JOIN vwMMV VW(NOLOCK) ON VW.VersionId = TBS.LostVersionId
				LEFT JOIN vwMMV VWW(NOLOCK) ON VWW.VersionId = TBS.CarVersionId
				LEFT JOIN Cities C(NOLOCK) ON C.ID = TBS.InquiryCityId 
				LEFT JOIN TC_VersionsCode TV(NOLOCK) ON TV.CarVersionId = TBS.CarVersionId
				LEFT JOIN TC_VersionColourCode VC(NOLOCK) ON VC.VersionColorsId = TBS.BookedCarColourId --Added by Sachin Bharti (10th Oct 2013)
				--LEFT JOIN TC_ExchangeNewCar TENC WITH(NOLOCK) ON TENC.TC_NewCarInquiriesId=TBS.TC_NewCarInquiriesId --Added by Vivek Singh (22nd jan 2014)
				LEFT JOIN vwMMV VEX WITH(NOLOCK) ON VEX.VersionId = TBS.ExchangeCarVersionId  --Added by Vivek Singh (22nd jan 2014)
				--LEFT JOIN TC_NewCarBooking  AS NB   WITH(NOLOCK) ON NB.TC_NewCarInquiriesId = TBS.TC_NewCarInquiriesId --Added by Vivek Singh (22nd jan 2014)
				LEFT JOIN TC_CampaignScheduling CS WITH(NOLOCK) ON CS.TC_CampaignSchedulingId=TBS.NSCCampaignSchedulingId --By Tejashree on 14/5/2014
				LEFT JOIN TC_vwCampaignMaster VWC WITH(NOLOCK) ON VWC.TC_SubCampaignId=CS.TC_SubCampaignId
			WHERE TBS.InvoiceDate BETWEEN @FromDate AND @ToDate AND TBS.Invoicedate  IS NOT NULL AND
				(D.TC_RMID = @RMID OR @RMID IS NULL) AND (D.TC_AMId = @AMID OR @AMID IS NULL)
				AND (D.ID =@DealerId OR @DealerId IS NULL)
		END

	--Pending Deliveries Leads
	IF @LeadType = 10
		BEGIN
				SELECT DISTINCT
				REPLACE(ISNULL(TSU.UserName,'-'), CHAR(9), '')AS RM, 
				REPLACE(ISNULL(TSU1.UserName,'-'), CHAR(9), '')AS AM, 
				REPLACE(ISNULL(D.Organization,'-'), CHAR(9), '') AS DealerName,  
				ISNULL(D.DealerCode, '-') DealerCode,				
				ISNULL(TC.UniqueCustomerId,'-')AS UniqueCustomerId,
				dbo.RemoveSpecialCharsExceptBlank(ISNULL(TC.CustomerName, '-')) AS CustomerName, 
				ISNULL(TC.Mobile, '-') AS Mobile, 
				ISNULL(TC.Email,'-') AS Email,
				REPLACE(ISNULL(TBS.Source,'-'), CHAR(9), '') AS Source, 
				REPLACE(ISNULL(TBS.Eagerness,'-'), CHAR(9), '')AS Eagerness,
				REPLACE(ISNULL(TBS.CompanyName,'-'), CHAR(9), '')AS CompanyName,
				REPLACE(ISNULL(VWW.Car,'-'), CHAR(9), '')AS CarName,
				TBS.InquiryCreationDate AS InquiryCreationDate ,
				TBS.CreatedDate AS LeadCreationDate,
				ISNULL(C.Name,'-')AS City,
				REPLACE(ISNULL(TU.UserName,'-'), CHAR(9), '') AS UserConsultant,
				dbo.RemoveSpecialCharsExceptBlank(ISNULL(TBS.LastCallComment,'-')) AS LastCallComment,
				TBS.TestDriveDate AS TestDriveDate,
				--(SELECT COUNT(*) FROM TC_TDCalendar TD WHERE TD.TC_NewCarInquiriesId= TBS.TC_NewCarInquiriesId)  AS NumberOfTDs,--Added by vivek singh (22nd jan 2014)
				TBS.BookingEntryDate BookingEntryDate,
				TBS.BookingDate AS BookingDate,
				TBS.BookingAmt AS BookingAmt,
				REPLACE(ISNULL(TCLD.Name,'-'),CHAR(9),'') AS BookingStatus,
				TBS.BookingCancelDate,
				TBS.PromisedDeliveryDate AS PromisedDeliveryDate,
				TBS.DeliveryEntryDate AS DeliveryEntryDate,
				TBS.CarDeliveryDate AS CarDeliveryDate,
				dbo.RemoveSpecialCharsExceptBlank(ISNULL(TBS.EngineNumber,'-')) AS EngineNumber,
				dbo.RemoveSpecialCharsExceptBlank(ISNULL(TBS.InsuranceCoverNumber,'-'))AS InsuranceCoverNumber,
				dbo.RemoveSpecialCharsExceptBlank(ISNULL(TBS.InvoiceNumber,'-')) AS InvoiceNumber,
				TBS.InvoiceDate AS RetailDate,ISNULL(TV.CarVersionCode,'-')AS ModelCode,
				--dbo.RemoveSpecialCharsExceptBlank(ISNULL(TBS.InvoiceNumber,'-')) AS InvoiceNumber,
				dbo.RemoveSpecialCharsExceptBlank(ISNULL(TBS.RegistrationNo,'-'))AS RegistrationNo,
				dbo.RemoveSpecialCharsExceptBlank(ISNULL(TBS.PanNo,'-'))AS PanNo,
				dbo.RemoveSpecialCharsExceptBlank(ISNULL(TBS.VinNO,'-')) AS VinNO, 
				ISNULL(TBS.BookingName,'-') AS RetailCustomerName,    --Added by vivek singh (22nd jan 2014)
			    ISNULL(TBS.BookingMobile,'-') AS RetailCustomerMobileNo,
				ISNULL(TBS.BookingEmail,'-') AS RetailCustomerEmail,   
				TBS.InquiryDispositionDate AS LostDate,       
				ISNULL(TLD.Name,'-') AS LostReason,
				ISNULL(VW.Make,'-') AS LostToMake ,
				ISNULL(VW.Model,'-') AS LostToModel,
				ISNULL(VW.Version,'-') AS LostToVersion, 
				ISNULL(TSD.SubDispositionName,'-') AS LostSubDisposition,
				--Added by Sachin Bharti(10th Oct 2013)
				ISNULL(VC.ColorCode,'-') AS ModelColorCode,
				ISNULL(TBS.BookedCarMakeYear,'-')AS ModelYear,
				ISNULL(VEX.Make,'-') AS ExchangeCarMake,      --Added by vivek singh (22nd jan 2014)
			    ISNULL(VEX.Model,'-') AS ExchangeCarModel,
				ISNULL(VEX.Version,'-') AS ExchangeCarversion,
				TBS.ExchangeCarMakeYear AS ExchangeCarYear,
				ISNULL(TBS.ExchangeCarKms,'-') AS ExchangeCarKMdriven,
				ISNULL(TBS.ExchangeCarExpectedPrice,'-') AS ExchangeCarExpectedPrice,
				ISNULL(TU.VTONumbers,'-') AS VTONumbers,-- Modified By: Tejashree Patil on 14 May 2014,
				ISNULL(VWC.MainCampaignName+' ' + VWC.SubCampaignName,'-') AS CampaignName
			FROM 
				DEALERS as D WITH (NOLOCK)
				INNER JOIN TC_LeadBasedSummary TBS WITH (NOLOCK) ON D.ID = TBS.DealerId
				INNER JOIN TC_BrandZone TBZ WITH (NOLOCK) ON D.TC_BrandZoneId = TBZ.TC_BrandZoneId AND TBZ.MakeId = @Makeid 
				LEFT JOIN TC_SpecialUsers TSU WITH (NOLOCK) ON D.TC_RMId = TSU.TC_SpecialUsersId AND TSU.IsActive = 1
				LEFT JOIN TC_SpecialUsers TSU1 WITH (NOLOCK) ON D.TC_AMId = TSU1.TC_SpecialUsersId AND TSU1.IsActive = 1
				LEFT JOIN TC_Lead TL(NOLOCK) ON TBS.TC_LeadId = TL.TC_LeadId
				LEFT JOIN TC_CustomerDetails TC(NOLOCK) ON TL.TC_CustomerId = TC.Id
				LEFT JOIN TC_Users TU(NOLOCK) ON TU.Id = TBS.TC_UsersId
				LEFT JOIN TC_LeadDisposition TLD(NOLOCK) ON TLD.TC_LeadDispositionId = TBS.InquiryDispositionId
				LEFT JOIN TC_LeadDisposition AS TCLD WITH (NOLOCK) ON  TCLD.TC_LeadDispositionId=TBS.BookingStatus 
				LEFT JOIN TC_LeadSubDisposition TSD(NOLOCK) ON TSD.TC_LeadSubDispositionId = TBS.InquirySubDispositionId 
				LEFT JOIN vwMMV VW(NOLOCK) ON VW.VersionId = TBS.LostVersionId
				LEFT JOIN vwMMV VWW(NOLOCK) ON VWW.VersionId = TBS.CarVersionId
				LEFT JOIN Cities C(NOLOCK) ON C.ID = TBS.InquiryCityId 
				LEFT JOIN TC_VersionsCode TV(NOLOCK) ON TV.CarVersionId = TBS.CarVersionId
				LEFT JOIN TC_VersionColourCode VC(NOLOCK) ON VC.VersionColorsId = TBS.BookedCarColourId --Added by Sachin Bharti (10th Oct 2013)
				--LEFT JOIN TC_ExchangeNewCar TENC WITH(NOLOCK) ON TENC.TC_NewCarInquiriesId=TBS.TC_NewCarInquiriesId --Added by Vivek Singh (22nd jan 2014)
				LEFT JOIN vwMMV VEX WITH(NOLOCK) ON VEX.VersionId = TBS.ExchangeCarVersionId  --Added by Vivek Singh (22nd jan 2014)
				--LEFT JOIN TC_NewCarBooking  AS NB   WITH(NOLOCK) ON NB.TC_NewCarInquiriesId = TBS.TC_NewCarInquiriesId --Added by Vivek Singh (22nd jan 2014)
				LEFT JOIN TC_CampaignScheduling CS WITH(NOLOCK) ON CS.TC_CampaignSchedulingId=TBS.NSCCampaignSchedulingId --By Tejashree on 14/5/2014
				LEFT JOIN TC_vwCampaignMaster VWC WITH(NOLOCK) ON VWC.TC_SubCampaignId=CS.TC_SubCampaignId
			WHERE TBS.Invoicedate  IS NOT NULL AND ISNULL(TBS.CarDeliveryStatus,0)<>77 AND TBS.InvoiceDate <= @ToDate --Added (TBS.InvoiceDate <= @ToDate) by Vivek Singh on 11th Nov 2013 
				AND (D.TC_RMID = @RMID OR @RMID IS NULL) AND (D.TC_AMId = @AMID OR @AMID IS NULL)
				AND (D.ID =@DealerId OR @DealerId IS NULL)
		END
END

