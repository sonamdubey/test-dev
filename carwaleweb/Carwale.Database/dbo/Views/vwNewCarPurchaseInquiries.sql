IF EXISTS(
SELECT *
   FROM sys.views
     WHERE schema_id = SCHEMA_ID('dbo'))
     name = 'vwNewCarPurchaseInquiries' AND
     DROP VIEW dbo.vwNewCarPurchaseInquiries
GO

	


----------- Modified By Kundan Dombale On 04-11-2015 
----------- Added archived tbale 'NewCarPurchaseInquiries01012014_TO_30062015' with union

		CREATE view [dbo].[vwNewCarPurchaseInquiries]
		
		AS
 			SELECT	Id,
					CustomerId,
					CarVersionId,
					Color,
					NoOfCars,
					BuyTime,
					Comments,
					RequestDateTime,
					IsApproved,
					IsFake,
					StatusId,
					IsForwarded,
					IsRejected,
					IsViewed,
					IsMailSend,
					TestdriveDate,
					TestDriveLocation,
					LatestOffers,
					ForwardedLead,
					SourceId,
					ReqDateTimeDatePart,
					VisitedDealership,
					CRM_LeadId,
					ClientIP,
					PQPageId,
					LTSRC,
					UtmaCookie,
					UtmzCookie
			FROM NewCarPurchaseInquiries WITH (NOLOCK)

			UNION 

			SELECT	Id,
					CustomerId,
					CarVersionId,
					Color,
					NoOfCars,
					BuyTime,
					Comments,
					RequestDateTime,
					IsApproved,
					IsFake,
					StatusId,
					IsForwarded,
					IsRejected,
					IsViewed,
					IsMailSend,
					TestdriveDate,
					TestDriveLocation,
					LatestOffers,
					ForwardedLead,
					SourceId,
					ReqDateTimeDatePart,
					VisitedDealership,
					CRM_LeadId,
					ClientIP,
					PQPageId,
					LTSRC,
					NULL UtmaCookie,
					NULL UtmzCookie
			FROM [CWArchiveTables].[dbo].[NewCarPurchaseInquiries01012014_TO_30062015] WITH (NOLOCK)
		 
			UNION 

			SELECT	Id,
					CustomerId,
					CarVersionId,
					Color,
					NoOfCars,
					BuyTime,
					Comments,
					RequestDateTime,
					IsApproved,
					IsFake,
					StatusId,
					IsForwarded,
					IsRejected,
					IsViewed,
					IsMailSend,
					TestdriveDate,
					TestDriveLocation,
					LatestOffers,
					ForwardedLead,
					SourceId,
					ReqDateTimeDatePart,
					VisitedDealership,
					NULL  CRM_LeadId,                                 -- dummy Values
					NULL  ClientIP,                                  -- dummy Values
					NULL   PQPageId,								   -- dummy Values		
					NULL   LTSRC,								   -- dummy Values			
					NULL    UtmaCookie,                                  -- dummy Values
					NULL    UtmzCookie                               ---dummy Values  
			FROM NewCarPurchaseInquiries_History WITH (NOLOCK)

