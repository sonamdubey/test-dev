IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_LeadTaskDetails]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_LeadTaskDetails]
GO

	
-- =============================================  
-- Author:  Umesh Ojha    
-- Create date: 10-10-2013  
-- Description: Getting Data for details of the lead
-- Modified By : Chetan Navin on 10th Aug, 2016 (To fetch DeviceTokenIOS of user) 
-- ============================================
CREATE procEDURE [dbo].[TC_LeadTaskDetails]

@LeadId	 BIGINT

AS
BEGIN 
			SELECT U.GCMRegistrationId,U.DeviceTokenIOS FROM TC_Users U WITH (NOLOCK)
			Inner Join TC_ActiveCalls TA WITH (NOLOCK) ON U.Id = TA.TC_UsersId
			WHERE TA.TC_LeadId= @LeadId

			SELECT C.id  AS [CustomerId], 
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
												WHEN 3 THEN 'New Buy' END AS InquiryType
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
                                                                           
               WHERE              
					TCIL.TC_LeadId = @LeadId

END