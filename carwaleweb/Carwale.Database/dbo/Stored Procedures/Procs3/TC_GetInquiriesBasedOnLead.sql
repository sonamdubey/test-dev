IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_GetInquiriesBasedOnLead]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_GetInquiriesBasedOnLead]
GO
	
-- =======================
-- Author:		<khushaboo patil>
-- Create date: <02/05/2016>
-- Description:	<Get Inquiries Based On Lead if carstatus = '3' car not list for td,'2' 
-- car is listed and scheduled for td on same time, '1' is listed but not scheduled >
-- exec TC_GetInquiriesBasedOnLead @LeadId= 25034,@FromDate  = '2016-06-08 13:30:00',@ToDate ='2016-06-08 14:00:00'
-- Modified By : Nilima More On 12th May 2016,Fetch source id for all td Inquiries.
-- Modified By : Khushaboo Patil on 8th Jun 2016 added TDC.TDStatus<> 27 condition (if td cancelled show car as available)
-- =======================
CREATE PROCEDURE [dbo].[TC_GetInquiriesBasedOnLead]
	@LeadId	INT,
	@FromDate DATETIME,
	@ToDate	  DATETIME
AS
BEGIN

	SELECT DISTINCT TDC.TDStatus,
	CASE WHEN ISNULL(TD.VersionId,0) <> 0 THEN 
	CASE WHEN TDC.TDStatus<> 27 AND ((@FromDate>= DATEADD(day,DATEDIFF(day, 0,TDC.TDDate),CAST(TDC.TDStartTime AS DATETIME)) AND  
	@FromDate < DATEADD(day,DATEDIFF(day, 0,TDC.TDDate),CAST(TDC.TDEndTime AS DATETIME))) OR 
	(@ToDate> DATEADD(day,DATEDIFF(day, 0,TDC.TDDate),CAST(TDC.TDStartTime AS DATETIME)) AND  
	@ToDate <= DATEADD(day,DATEDIFF(day, 0,TDC.TDDate),CAST(TDC.TDEndTime AS DATETIME))) )
	THEN '2' ELSE '1' END ELSE '3' END CarStausId,VW.MakeId,VW.Make,VW.ModelId,VW.Model,VW.VersionId,
	VW.Version,TD.TC_TDCarsId,NI.TC_NewCarInquiriesId,vw.Car AS CarName,NI.TC_InquirySourceId SourceId  -- Modified By : Nilima More On 12th May 2016,Fetch source id for all td Inquiries.
	INTO #TMPInquiries
	FROM TC_InquiriesLead IL WITH(NOLOCK)
	INNER JOIN TC_NewCarInquiries NI WITH(NOLOCK) ON NI.TC_InquiriesLeadId = IL.TC_InquiriesLeadId AND IL.TC_LeadInquiryTypeId = 3
	INNER JOIN TC_Users TU WITH(NOLOCK) ON TU.Id = IL.TC_UserId
	INNER JOIN vwAllMMV VW WITH(NOLOCK) ON VW.VersionId = NI.VersionId and ApplicationId = 1
	LEFT JOIN TC_TDCars TD WITH(NOLOCK) ON TD.VersionId = NI.VersionId AND TD.BranchId = IL.BranchId AND TD.IsActive = 1
	LEFT JOIN TC_TDCalendar TDC WITH(NOLOCK) ON TD.TC_TDCarsId = TDC.TC_TDCarsId
	WHERE IL.TC_LeadId = @LeadId
	AND
	(
		(
			(
				ISNULL(NI.BookingStatus,0) = 0 OR
				(NI.BookingStatus = 31
					AND (NI.TC_InquirySourceId <> 134 AND NI.TC_InquirySourceId <> 140 AND NI.TC_InquirySourceId <> 146)) 
				OR bookingStatus = 99 OR bookingStatus = 96
			)
			AND 
			(ISNULL(NI.TC_LeadDispositionId,0) = 0 OR NI.TC_LeadDispositionId = 4 )
		) -- inquiry should not be close
		AND	 
		ISNULL(NI.BookingStatus,0) <> 32  AND IL.TC_LeadStageId <> 3 -- inquiry should not be booked and lead should not be closed
		
	)
	SELECT MAX(CarStausId) CarStausId,MakeId,Make,ModelId,Model,VersionId,Version,TC_TDCarsId,TC_NewCarInquiriesId,CarName,SourceId,TDStatus
	INTO #TMPInquiriesWithAllTDCars
	FROM #TMPInquiries WITH(NOLOCK)
	GROUP BY MakeId,Make,ModelId,Model,VersionId,Version,TC_TDCarsId,TC_NewCarInquiriesId,CarName,SourceId,TDStatus

	
	SELECT CarStausId,MakeId,Make,ModelId,Model,VersionId,Version,TC_TDCarsId,TC_NewCarInquiriesId,CarName,SourceId,TDStatus from(
	SELECT CarStausId,MakeId,Make,ModelId,Model,VersionId,Version,TC_TDCarsId,TC_NewCarInquiriesId,CarName,SourceId,TDStatus,
	ROW_NUMBER ()OVER(PARTITION BY TC_NewCarInquiriesId ORDER BY CarStausId) rowno
	FROM #TMPInquiriesWithAllTDCars	
	GROUP BY CarStausId,MakeId,Make,ModelId,Model,VersionId,Version,TC_TDCarsId,TC_NewCarInquiriesId,CarName,SourceId,TDStatus	
	)tab where rowno = 1

	DROP TABLE #TMPInquiries
	DROP TABLE #TMPInquiriesWithAllTDCars

END
----------------------------------------------------------------------------------------------------------------------------------------------
