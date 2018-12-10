IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_ReportZoneHourlyDataJob]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_ReportZoneHourlyDataJob]
GO

	-- =============================================
-- Author:		Manish
-- Create date: 19-06-2013
-- Description: Report for VW AutoBiz panel Usage 
-- Modified by Manish on 11-07-2013 this SP whole logic has been changed now we are capturing the day wise total raw data for volkswagen dealers.
-- Modified by Manish on 31-07-2013  Columns added CarDeliveryDate,CarDeliveryStatus
-- Modified by Manish on 19-08-2013  Columns added InquiryCreationDate
-- Modified by Manish on 27-08-2013  Columns added 
-- Modified by Manish on 29-08-2013  More Columns added 
-- Modified by Manish on 05-09-2013  Column added BookingCancelDate in TC_NewCarInquiries table
-- Modified by Manish on 11-09-2013  Column added InvoiceDate, CarVersionId
-- Modified by Manish on 19-09-2013  Column added InquiryCityId and change source from lead level to inquiry level
-- Modified by Manish on 10-10-2013  Column added BookedCarColourId,BookedCarMakeYear
-- Modified by Vivek Singh on 11-02-2014  Column added BookedName,BookedMobile,BookingEmail,InquiryDispositionDate,ExchangeCar(VersionId,KmsDriven,Makeyear,ExpectedPrice)
-- Modified By:Tejashree Patil on 14 May 2014, Added column NSCCampaignSchedulingId,DealerCampaignSchedulingId in Tc_LeadBasedSummary table.
-- Modified By: Manish Chourasiya on 13-11-2014 changed to add dealer id condition now this houly reports will use for Shaman Used Cars.
-- Modified By: Manish Chourasiya on 31-08-2015 added try and and catch block and increase size of field [LatestCarName].
-- =============================================
CREATE PROCEDURE  [dbo].[TC_ReportZoneHourlyDataJob]
AS 
BEGIN 

DECLARE @TmpTable   TABLE  ([DealerId] INT,
							[TC_LeadId] [BIGINT] ,
							[CreatedDate] [datetime] ,
							[Organization] [varchar](100) ,
							[Eagerness] [varchar](50),
							[EagernessId] [TINYINT] ,
							[Source] [varchar](100) ,
							[SourceId] [TINYINT] ,
							[TC_LeadStageId] [TINYINT],
							[TC_LeadDispositionID] [SMALLINT] ,
							[CarModel] [varchar](80) ,
							[CarModelId] [INT] ,
							[ScheduledOn] [datetime],
							[TestDriveDate] [date] ,
							[TestDriveStatus] [tinyint],  
							[TC_NewCarInquiriesId] [BIGINT],
							[BookingStatus] [TINYINT],
							[BookingDate] [DATETIME],
							[LeadClosedDate] [DATETIME],
							[InquiryDispositionId] [SMALLINT],
							[InquirySubDispositionId] [SMALLINT],
							[LostVersionId] [BIGINT],
							[CarDeliveryStatus] [SMALLINT],
							[CarDeliveryDate] [DATETIME],
							[TC_UsersId]     [INT],
                            [LatestCarName]   [VARCHAR](130),
                            [LastCallComment] VARCHAR(MAX),
                            [InquiryCreationDate] [DATETIME],
                            CompanyName VARCHAR(150),  -- Columns added InquiryCreationDate by Manish on 19-08-2013  
							BookingEventDate DATETIME,  --Below All columns added by Manish on 27-08-2013
							BookingAmt [decimal](18, 0),
							PromisedDeliveryDate DATETIME,
							DeliveryEventDate DATETIME,
							PanNo VARCHAR(25),
							VinNO VARCHAR(50),
							EngineNumber [varchar](100),
							InsuranceCoverNumber [varchar](100),
							InvoiceNumber [varchar](100),
							RegistrationNo [varchar](100),
							BookingCancelDate DATETIME,
							InvoiceDate DATE ,
							CarVersionId INT,
							InquiryCityId INT,
							BookedCarColourId INT,
							BookedCarMakeYear INT,
							BookingName [varchar](50),
							BookingMobile [varchar](50),
							BookingEmail [varchar](100),
							InquiryDispositionDate DATETIME,
							ExchangeCarVersionId INT,
							ExchangeCarKms INT,
							ExchangeCarMakeYear DATE,
							ExchangeCarExpectedPrice INT,
							NSCCampaignSchedulingId INT, -- Columns added by Tejashree on 14 May 2014. 
							DealerCampaignSchedulingId INT
						   )

    BEGIN TRY 
            INSERT INTO @TmpTable
				       ([DealerId] ,
						[TC_LeadId]  ,
						[CreatedDate]  ,
						[Organization] ,
						[Eagerness] ,
						[EagernessId] ,
						[Source]  ,
						[SourceId]  ,
						[TC_LeadStageId],
						[TC_LeadDispositionID],
						[CarModel]  ,
						[CarModelId] ,
						[ScheduledOn] ,
						[TestDriveDate]  ,
						[TestDriveStatus],
						[TC_NewCarInquiriesId],
						[BookingStatus],
						[BookingDate],
						[LeadClosedDate],
						[InquiryDispositionId],
						[InquirySubDispositionId],
						[LostVersionId],
						[CarDeliveryStatus],
						[CarDeliveryDate],
						[TC_UsersId]    , 
						[LatestCarName]  ,
						[LastCallComment],
						[InquiryCreationDate],
						CompanyName ,
						BookingEventDate,
						BookingAmt,
						PromisedDeliveryDate,
						DeliveryEventDate,
						PanNo,
						VinNO,
						EngineNumber,        
						InsuranceCoverNumber,
						InvoiceNumber,       
						RegistrationNo,
						BookingCancelDate,
						InvoiceDate  ,
						CarVersionId ,
						InquiryCityId,
						BookedCarColourId,
					    BookedCarMakeYear,
					    BookingName,
						BookingMobile,
					    BookingEmail,
						InquiryDispositionDate,
						ExchangeCarVersionId,
						ExchangeCarKms,
						ExchangeCarMakeYear,
						ExchangeCarExpectedPrice,
						NSCCampaignSchedulingId , -- Columns added by Tejashree on 14 May 2014. 
						DealerCampaignSchedulingId 
						)
						
                SELECT            D.ID AS DealerId, 
 								  TCIL.TC_LeadId,
								  TCIL.CreatedDate, 
								  D.Organization, 
								  TCIS.Status AS Eagerness,
								  TCIL.TC_InquiryStatusId AS EagernessId, 
								  TCS.Source AS Source,
								  TCS.Id AS SourceId,
								  TCIL.TC_LeadStageId,
								  TCIL.TC_LeadDispositionID,
								  CM.Name   AS CarModel,
								  CM.ID   AS CarModelId,
								  TCAC.ScheduledOn   ScheduledOn,
								  TCTD.TDDate AS TestDriveDate,
								  TCTD.TDStatus AS TestDriveStatus,
								  TCNCI.TC_NewCarInquiriesId,  
								  TCNCI.BookingStatus, 
								  TCNCI.BookingDate, 
								  TCL.LeadClosedDate,
								  TCNCI.TC_LeadDispositionId,
								  TCNCI.TC_SubDispositionId,
								  TCNCI.LostVersionId,
								  TCNCI.CarDeliveryStatus,
								  TCNCI.CarDeliveryDate,
								  TCIL.TC_UserId ,    
								  TCIL.CarDetails  ,
								  TCAC.LastCallComment,
								  TCNCI.CreatedOn,
								  TCNCI.CompanyName,
								  TCNCI.BookingEventDate,
								  TCNCB.Payment,
								  TCNCB.PrefDeliveryDate,
								  TCNCB.DeliveryEntryDate,
								  TCNCB.PanNo,
								  TCNCB.ChassisNumber,
								  TCNCB.EngineNumber,
								  TCNCB.InsuranceCoverNumber,
								  TCNCB.InvoiceNumber,
								  TCNCB.RegistrationNo,
								  TCNCI.BookingCancelDate,
								  TCNCB.InvoiceDate,
								  TCNCI.VersionId,
								  TCNCI.CityId,
								  TCNCB.CarColorId,
					              TCNCB.ModelYear,
								  TCNCB.BookingName,
								  TCNCB.BookingMobile,
								  TCNCB.Email,
								  TCNCI.DispositionDate,
								  TENC.CarVersionId,
								  TENC.Kms,
								  TENC.MakeYear,
								  TENC.ExpectedPrice,
								  TCNCI.NSCCampaignSchedulingId,-- Columns added by Tejashree on 14 May 2014. 
								  TCNCI.DealerCampaignSchedulingId
				          FROM DEALERS    AS D     WITH (NOLOCK)
		--	INNER JOIN TC_BrandZone       AS TBZ   WITH (NOLOCK) ON  D.TC_BrandZoneId = TBZ.TC_BrandZoneId AND TBZ.MakeId = 20 AND  D.IsDealerActive= 1
		--	INNER JOIN TC_SpecialUsers    AS TSU   WITH (NOLOCK) ON  D.TC_RMId = TSU.TC_SpecialUsersId AND TBZ.MakeId = 20 AND  D.IsDealerActive= 1
			LEFT JOIN TC_Lead             AS TCL   WITH (NOLOCK) ON  D.ID=TCL.BranchId
			LEFT JOIN TC_InquiriesLead    AS TCIL  WITH (NOLOCK) ON  TCL.TC_LeadId=TCIL.TC_LeadId  
			LEFT JOIN TC_ActiveCalls      AS TCAC  WITH (NOLOCK) ON  TCAC.TC_LeadId=TCIL.TC_LeadId
			LEFT JOIN TC_NewCarInquiries  AS TCNCI WITH (NOLOCK) ON  TCIL.TC_InquiriesLeadId=TCNCI.TC_InquiriesLeadId
			LEFT JOIN TC_TDCalendar       AS TCTD  WITH (NOLOCK) ON  TCNCI.TC_TDCalendarId=TCTD.TC_TDCalendarId 
			LEFT JOIN TC_InquirySource    AS TCS   WITH (NOLOCK) ON  TCNCI.TC_InquirySourceId=TCS.Id  -- Modified by Manish on 19-09-2013 change TCL.TC_InquirySourceId to TCNCI.TC_InquirySourceId
			LEFT JOIN TC_InquiryStatus    AS TCIS  WITH (NOLOCK) ON  TCIL.TC_InquiryStatusId=TCIS.TC_InquiryStatusId
			LEFT JOIN CarVersions         AS CV    WITH (NOLOCK) ON  TCNCI.VersionId=CV.ID
			LEFT JOIN CarModels           AS CM    WITH (NOLOCK) ON  CV.CarModelId=CM.ID
			LEFT JOIN TC_NewCarBooking    AS TCNCB WITH (NOLOCK) ON  TCNCI.TC_NewCarInquiriesId=TCNCB.TC_NewCarInquiriesId
			LEFT JOIN TC_ExchangeNewCar   AS TENC  WITH (NOLOCK) ON  TENC.TC_NewCarInquiriesId=TCNCI.TC_NewCarInquiriesId
			WHERE D.ID=50
			AND D.IsDealerActive=1
  
  
  TRUNCATE TABLE TC_LeadBasedSummary
  
  INSERT INTO TC_LeadBasedSummary
							   ([DealerId] ,
								[TC_LeadId]  ,
								[CreatedDate]  ,
								[Organization] ,
								[Eagerness] ,
								[EagernessId] ,
								[Source]  ,
								[SourceId]  ,
								[TC_LeadStageId],
								[TC_LeadDispositionID],
								[CarModel]  ,
								[CarModelId] ,
								[ScheduledOn] ,
								[TestDriveDate]  ,
								[TestDriveStatus],
								[TC_NewCarInquiriesId],
								[BookingStatus],
								[BookingDate],
								[LeadClosedDate],
								[InquiryDispositionId],
								[InquirySubDispositionId],
								[LostVersionId],
								[CarDeliveryStatus],
						        [CarDeliveryDate],
						        [TC_UsersId]    , 
						        [LatestCarName]  ,
						        [LastCallComment],
						        [InquiryCreationDate],
						        CompanyName ,
						        BookingEntryDate,
						        BookingAmt,
						        PromisedDeliveryDate,
						        DeliveryEntryDate,
						        PanNo,
						        VinNO,
						        EngineNumber,        
						        InsuranceCoverNumber,
						        InvoiceNumber,       
						        RegistrationNo,
						        BookingCancelDate ,
						        InvoiceDate ,
						        CarVersionId,
						        InquiryCityId,
								BookedCarColourId,
					            BookedCarMakeYear,
								BookingName,
						        BookingMobile,
					            BookingEmail,
						        InquiryDispositionDate,
								ExchangeCarVersionId,
						        ExchangeCarKms,
						        ExchangeCarMakeYear,
						        ExchangeCarExpectedPrice,
								NSCCampaignSchedulingId,-- Columns added by Tejashree on 14 May 2014. 
								DealerCampaignSchedulingId
						       )
                  SELECT 
								[DealerId] ,
								[TC_LeadId]  ,
								[CreatedDate]  ,
								[Organization] ,
								[Eagerness] ,
								[EagernessId] ,
								[Source]  ,
								[SourceId]  ,
								[TC_LeadStageId],
								[TC_LeadDispositionID],
								[CarModel]  ,
								[CarModelId] ,
								[ScheduledOn] ,
								[TestDriveDate]  ,
								[TestDriveStatus],
								[TC_NewCarInquiriesId],
								[BookingStatus],
								[BookingDate],
								[LeadClosedDate],
								[InquiryDispositionId] ,
								[InquirySubDispositionId],
								[LostVersionId],
								[CarDeliveryStatus],
						        [CarDeliveryDate],
						        [TC_UsersId]    , 
						        [LatestCarName]  ,
						        [LastCallComment],
						        [InquiryCreationDate],
						        CompanyName ,
						        BookingEventDate,
						        BookingAmt,
						        PromisedDeliveryDate,
						        DeliveryEventDate,
						        PanNo,
						        VinNO,
						        EngineNumber,        
						        InsuranceCoverNumber,
						        InvoiceNumber,       
						        RegistrationNo,
						        BookingCancelDate,
						        InvoiceDate ,
						        CarVersionId,
						        InquiryCityId,
								BookedCarColourId,
					            BookedCarMakeYear,  
								BookingName,
						        BookingMobile,
					            BookingEmail,
						        InquiryDispositionDate,
								ExchangeCarVersionId,
						        ExchangeCarKms,
								ExchangeCarMakeYear,
								ExchangeCarExpectedPrice,
								NSCCampaignSchedulingId,-- Columns added by Tejashree on 14 May 2014. 
								DealerCampaignSchedulingId   
                        FROM @TmpTable

	END TRY 

    BEGIN CATCH
	         INSERT INTO TC_Exceptions
                      (Programme_Name,
                       TC_Exception,
                       TC_Exception_Date,
                       InputParameters)
         VALUES('TC_ReportZoneHourlyDataJob',
                 (ERROR_MESSAGE()+'ERROR_NUMBER(): '+CONVERT(VARCHAR,ERROR_NUMBER())),
                  GETDATE(),
                  NULL
                 )
	END CATCH
END		
	

