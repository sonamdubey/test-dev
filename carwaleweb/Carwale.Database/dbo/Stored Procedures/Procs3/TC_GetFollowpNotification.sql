IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_GetFollowpNotification]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_GetFollowpNotification]
GO
	
-- =============================================  
-- Author      : Chetan Navin
-- Create date : 12-08-2016
-- Description : To get followup Notification details on the basis of scheduledon 
-- ============================================
CREATE PROCEDURE [dbo].[TC_GetFollowpNotification]
AS
BEGIN 
	SELECT	C.id  AS [CustomerId], 
			(ISNULL(C.Salutation,'')+' '+ C.CustomerName+' '+ISNULL(C.LastName,''))        AS [CustomerName], --Modified by: Tejashree on 26-08-2013, 
			C.Email,    
			C.Mobile,  
			C.TC_InquirySourceId,                 
			tcac.TC_LeadId, 
			TC_InquiryStatusId, 
			ScheduledOn           AS [NextFollowUpDate], 
			TCIL.CarDetails       AS [InterestedIn], 
			TCAC.CallType,
			TCAC.LastCallComment,
			LatestInquiryDate,
			(CASE  WHEN LatestInquiryDate > ScheduledOn THEN LatestInquiryDate ELSE ScheduledOn END) AS OrderDate,
			TS.Source AS InquirySource,
			TCIL.TC_UserId AS UserId,
			TCIL.TC_LeadStageId,
			TCIL.TC_LeadDispositionID,
			UniqueCustomerId,
			CASE TC_LeadInquiryTypeId WHEN 1 THEN 'Used Buy' 
									WHEN 2 THEN 'Used Sell' 
									WHEN 3 THEN 'New Buy' END AS InquiryType,
			TU.GCMRegistrationId
        FROM           
                        TC_ActiveCalls         AS TCAC  WITH (NOLOCK) 
                      
                JOIN    TC_CustomerDetails     AS C     WITH (NOLOCK) 
                                                                    ON TCAC.TC_LeadId = C.ActiveLeadId 
                JOIN    TC_InquiriesLead       AS TCIL  WITH (NOLOCK) 
                                                                    ON TCAC.TC_LeadId = TCIL.TC_LeadId 
                JOIN    TC_Users               AS TU    WITH(NOLOCK) 
						                                            ON TCIL.TC_UserId = TU.Id	
				JOIN    TC_InquirySource       AS TS 	  WITH(NOLOCK) 
						                                            ON C.TC_InquirySourceId = TS.Id	                                                   
                                                                           
        WHERE	TCAC.ScheduledOn BETWEEN GETDATE() AND DATEADD(MINUTE,75,GETDATE())
END