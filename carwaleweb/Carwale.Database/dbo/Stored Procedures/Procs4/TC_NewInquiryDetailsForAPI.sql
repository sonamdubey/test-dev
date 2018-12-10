IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_NewInquiryDetailsForAPI]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_NewInquiryDetailsForAPI]
GO

	
-- =======================================================================
-- Created By: Nilesh Utture
-- Created On: 30th April, 2013
-- Description:This SP will give buyer Inquiry on stock details for API 
-- EXEC TC_NewInquiryDetailsForAPI 7144,995,152
-- Modified By: Nilesh Utture on 6th May, 2013 Reffered BuyerInquiriesId as Id
-- Modified By: Nilesh Utture on 8th August, 2013 Added Fields which will give details about TD
-- Modified By: Nilesh Utture on 1st September, 2013 Added COLUMN NS_ShowClose
-- Modifeid By Vivek Gupta on 14-01-2015, added @ApplicationId
-- Modified By Vivek Gupta replaced NS_ShowClose with IsBooked
-- Modified By :Khushaboo Patil on 17th March , Added columns InquiriesLeadId,LeadInquiryTypeId
-- =======================================================================
CREATE PROCEDURE [dbo].[TC_NewInquiryDetailsForAPI] @TC_LeadId INT,@BranchId INT,@TC_UserId INT, @ApplicationId TINYINT = 1
AS
BEGIN 
  
  SELECT TC_InquiriesLeadId,
		BranchId,
		TC_CustomerId,
		TC_UserId,
		IsActive,
		TC_LeadId,
		TC_LeadInquiryTypeId,
		TC_LeadDispositionID
	  INTO #TC_InquiriesLead	
	  FROM TC_InquiriesLead as L1 WITH (NOLOCK) 
	  WHERE L1.BranchId=@BranchId ANd L1.TC_LeadId=@TC_LeadId
	  
	    SELECT DISTINCT(NI.TC_NewCarInquiriesId) AS Id, 
		          (CASE L.TC_LeadInquiryTypeId WHEN 3 THEN 'New' END) AS Type, L.TC_CustomerId AS NS_CustomerId, TDC.TC_TDCarsId AS NS_TDCarId, TDC.TC_UsersId AS NS_TDConsultantId,
		          V.Car,CONVERT(VARCHAR,NI.TDDate,112) AS NS_TDDate,NI.TDStatus AS NS_TDStatus, NI.TC_TDCalendarId AS NS_TDCalendarId,V.ModelId AS NS_ModelId,
		          NI.CityId AS NS_CityId,NI.VersionId AS NS_VersionId,NI.TC_InquirySourceId AS NS_SourceId,NI.TDDate AS 'Testdrive date',V.MakeId AS NS_MakeId,
		          NI.CreatedOn, -- Prefix "NS_" in column names denotes not to show them at mobile app. front end
		          SRC.Source AS Source, 
		          'Rs. ' +ISNULL(CONVERT(VARCHAR,NP.Price),'-')   ExShowroomPrice, 
		          'Rs. ' + ISNULL(CONVERT(VARCHAR,NP.Insurance),'-')  Insurance, 
				  'Rs. ' +CONVERT(VARCHAR,(ISNULL(NP.Price,0)+ISNULL(NP.Insurance,0)))  OnRoadPrice,
				  (	CASE  
						WHEN NI.BookingStatus = 32 AND NCB.InvoiceDate IS NULL THEN 'false'
						WHEN (NI.BookingStatus <> 32 OR NI.BookingStatus IS NULL) THEN 'true'
						WHEN NI.BookingStatus = 32 AND NCB.InvoiceDate IS NOT NULL THEN 'RetailDone'
					END) AS NS_ShowClose,
					ISNULL(NI.MostInterested,0) AS MostInterested,
					L.TC_InquiriesLeadId AS InquiriesLeadId,L.TC_LeadInquiryTypeId AS LeadInquiryTypeId
		          FROM            #TC_InquiriesLead L   
		          INNER JOIN      TC_NewCarInquiries  NI    WITH (NOLOCK)             ON L.TC_InquiriesLeadId =NI.TC_InquiriesLeadId  
		          INNER JOIN      vwAllMMV           V    WITH (NOLOCK)             ON NI.VersionId = V.VersionId
		          LEFT JOIN NewCarShowroomPrices NP WITH (NOLOCK) ON NI.VersionId = NP.CarVersionId AND NP.CityId = (SELECT CityId FROM Dealers WITH (NOLOCK) WHERE Id=@BranchId)
				  LEFT JOIN TC_InquirySource SRC WITH (NOLOCK) ON SRC.Id = NI.TC_InquirySourceId
				  LEFT JOIN TC_TDCalendar TDC WITH (NOLOCK) ON TDC.TC_TDCalendarId = NI.TC_TDCalendarId
				  LEFT JOIN TC_NewCarBooking NCB WITH(NOLOCK) ON NCB.TC_NewCarInquiriesId = NI.TC_NewCarInquiriesId
			WHERE L.BranchId = @BranchId 
		          AND L.TC_LeadId = @TC_LeadId 
		          AND L.TC_UserId = @TC_UserId 
		          AND (NI.TC_LeadDispositionId IS NULL OR NI.TC_LeadDispositionId = 4)
				  AND V.ApplicationId = ISNULL(@ApplicationId,1)
	DROP TABLE #TC_InquiriesLead	

END