IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_GetNotificationAlertDetails]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_GetNotificationAlertDetails]
GO
	--============================================
--Author: Tejashree Patil on 24 Dec 2015 at 7.30pm
--Description: To get details for sending notification of inquiry floowup reschedule, personal visit, ShowroomVisit. 
-- EXEC TC_GetNotificationAlertDetails 243,1
--============================================
CREATE PROCEDURE [dbo].[TC_GetNotificationAlertDetails]
@TC_UserId INT,
@IsAlertSend INT = NULL 
--@LeadId INT = 10183
AS
IF(@IsAlertSend = 1)
BEGIN
SELECT	TOP 1 ISNULL(CD.CustomerName,'') + ' ' + ISNULL(CD.LastName,'') CustomerName, CD.Mobile CustomerMobileNo, IL.CarDetails LatestCarDetails, AC.ScheduledOn  ScheduledDateTime, 
		CT.name CallType, AC.TC_CallsId CallId, U.Id UserId,l.TC_LeadId LeadId--, CA.Name CallAction
FROM	TC_ActiveCalls AC  WITH(NOLOCK)
		INNER JOIN TC_Calls C WITH(NOLOCK) ON C.TC_CallsId = AC.TC_CallsId
		INNER JOIN TC_Users U WITH(NOLOCK) ON U.Id = AC.TC_UsersId
		INNER JOIN TC_Lead L WITH(NOLOCK) ON L.TC_LeadId = AC.TC_LeadId
		INNER JOIN TC_CustomerDetails CD WITH(NOLOCK) ON CD.Id = L.TC_CustomerId
		INNER JOIN TC_CallType CT WITH(NOLOCK) ON CT.TC_CallTypeId = AC.CallType
		INNER JOIN TC_InquiriesLead IL WITH(NOLOCK) ON IL.TC_LeadId = L.TC_LeadId
		--LEFT JOIN TC_CallAction CA WITH(NOLOCK) ON CA.TC_CallActionId = C.TC_CallActionId
WHERE	AC.TC_UsersId = @TC_UserId 
		--AND (@LeadId IS NULL OR AC.TC_LeadId = @LeadId)
		AND CONVERT(DATE,AC.ScheduledOn) = CONVERT(DATE,GETDATE()) ORDER BY c.CreatedOn desc
		END
		ELSE 
		IF(@IsAlertSend = 2)
		BEGIN
		SELECT DISTINCT ISNULL(CD.CustomerName,'') + ' ' + ISNULL(CD.LastName,'') CustomerName, CD.Mobile CustomerMobileNo, IL.CarDetails LatestCarDetails, AC.ScheduledOn  ScheduledDateTime, 
		CT.name CallType, AC.TC_CallsId CallId, U.Id UserId,l.TC_LeadId LeadId--, CA.Name CallAction
FROM	TC_ActiveCalls AC  WITH(NOLOCK)
		INNER JOIN TC_Calls C WITH(NOLOCK) ON C.TC_CallsId = AC.TC_CallsId
		INNER JOIN TC_Users U WITH(NOLOCK) ON U.Id = AC.TC_UsersId
		INNER JOIN TC_Lead L WITH(NOLOCK) ON L.TC_LeadId = AC.TC_LeadId
		INNER JOIN TC_CustomerDetails CD WITH(NOLOCK) ON CD.Id = L.TC_CustomerId
		INNER JOIN TC_CallType CT WITH(NOLOCK) ON CT.TC_CallTypeId = AC.CallType
		INNER JOIN TC_InquiriesLead IL WITH(NOLOCK) ON IL.TC_LeadId = L.TC_LeadId
		--LEFT JOIN TC_CallAction CA WITH(NOLOCK) ON CA.TC_CallActionId = C.TC_CallActionId
WHERE	AC.TC_UsersId = @TC_UserId 
		--AND (@LeadId IS NULL OR AC.TC_LeadId = @LeadId)
		AND CONVERT(DATE,AC.ScheduledOn) = CONVERT(DATE,GETDATE()) 
		END

--------------------------------------------------------------------------------------
