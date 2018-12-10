IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_GetAppNotifications]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_GetAppNotifications]
GO
	-- =============================================
-- Author:		<Khushaboo Patil>
-- Create date: <27/01/2016>
-- Description:	<Get App Notifications>
-- =============================================
CREATE PROCEDURE [dbo].[TC_GetAppNotifications]
AS
BEGIN
	SELECT DISTINCT AN.ID AS [TC_AppNotificationId],
			CD.id  AS [CustomerId], 
            CD.CustomerName AS [CustomerName], 
            CD.Email,    
            CD.Mobile,  
            CD.TC_InquirySourceId, 
            AC.TC_LeadId, 
            TC_InquiryStatusId, 
            ScheduledOn AS [NextFollowUpDate], 
            IL.CarDetails AS [InterestedIn], 
            AC.CallType,
            AC.ActionComments,
            LatestInquiryDate,
            TS.Source AS InquirySource,
            AC.TC_UsersId AS LeadOwnerId,
            IL.TC_LeadStageId,
			EN.MakeYear AS ExchangeCarMakeYear,
			ISNULL(VW1.Make,'') + ' ' +ISNULL(VW1.Model,'')+ ' '+ ISNULL(VW1.Version,'') AS ExchangeCar,
            CASE TC_LeadInquiryTypeId WHEN 1 THEN 'Used Buy' 
									WHEN 2 THEN 'Used Sell' 
									WHEN 3 THEN 'New Buy' END AS InquiryType,
			TU.UniqueId AS Abk, -- Abk is Unique key for user
			AN.RecordType
	FROM TC_AppNotification AN  WITH(NOLOCK) 
	INNER JOIN TC_Calls AC WITH(NOLOCK) ON AC.TC_CallsId = AN.RecordId
	INNER JOIN TC_Lead TL WITH(NOLOCK) ON TL.TC_LeadId = AC.TC_LeadId
	INNER JOIN TC_InquiriesLead IL WITH(NOLOCK) ON IL.TC_LeadId = TL.TC_LeadId
	INNER JOIN TC_Users TU WITH(NOLOCK) ON AC.TC_UsersId = TU.Id	
	INNER JOIN Dealers D WITH(NOLOCK) ON D.ID = TU.BranchId
	INNER JOIN TC_CustomerDetails CD WITH(NOLOCK) ON CD.Id = TL.TC_CustomerId	
	INNER JOIN TC_InquirySource TS WITH(NOLOCK) ON CD.TC_InquirySourceId = TS.Id	   
	LEFT JOIN TC_NewCarInquiries NI WITH(NOLOCK) ON NI.TC_InquiriesLeadId = IL.TC_InquiriesLeadId
	LEFT JOIN TC_ExchangeNewCar EN WITH(NOLOCK) ON EN.TC_NewCarInquiriesId = NI.TC_NewCarInquiriesId  
	LEFT JOIN vwAllMMV VW1 WITH(NOLOCK) ON VW1.VersionId = EN.CarVersionId AND VW1.ApplicationId = D.ApplicationId
	WHERE  ( RecordType = 1 OR RecordType = 2 ) 
END

------------------------------


/****** Object:  StoredProcedure [dbo].[TC_StockDetailsFetch]    Script Date: 12-Apr-16 5:56:57 PM ******/
SET ANSI_NULLS ON
