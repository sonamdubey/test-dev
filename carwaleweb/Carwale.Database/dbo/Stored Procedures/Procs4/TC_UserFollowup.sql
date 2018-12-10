IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_UserFollowup]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_UserFollowup]
GO

	-- =============================================
-- Author:		Vivek Gupta
-- Create date: 3rd June, 2013
-- Description:	Gets user's assigned tasks for today.
-- TC_UserFollowup 5,1,'2013-06-05','2013-06-07 12:04:29.900'
-- Modified By Vivek Gupta on 16th sep,2013.. Added TC_LeadDisposition=4 in where condition
-- =============================================
CREATE PROCEDURE [dbo].[TC_UserFollowup]
@BranchId BIGINT,
@UserId BIGINT,
@FromFolloupdate DATETIME,
@ToFollowupdate DATETIME
AS
BEGIN

              SELECT 
						D.FirstName + D.LastName AS DealerName,
						D.Organization,
						D.Address1 + D.Address2 AS Address,
						--D.MobileNo,
						--D.EmailId,
						D.Pincode,
						TCU.UserName						


				 FROM Dealers D WITH(NOLOCK)
				 JOIN TC_Users TCU ON D.ID = TCU.BranchId
				 
				 WHERE D.ID= @BranchId
				 
				 AND TCU.Id = @UserId
				 AND TCU.IsActive = 1
             
              
              SELECT                
                      C.CustomerName        AS CustomerName,                       
                      C.Mobile,  
                      C.Location AS Address,  
                      --TCAC.ScheduledOn,                   
                      TS.Source,                 
                      (CASE TC_InquiryStatusId WHEN 1 THEN 'Hot' 
                                               WHEN 2 THEN 'Warm' 
                                               WHEN 3 THEN 'Normal' 
                                               ELSE 'Eagerness Not Set' END) AS EagerNess, 
                      TCAC.LastCallDate     AS LastFollowupDate, 
                      TCIL.CarDetails       AS ModelName, 
                      TCAC.LastCallComment AS LastFollowupStatus,
                      LatestInquiryDate AS EnqDate,                                           
                      CASE TC_LeadInquiryTypeId WHEN 1 THEN 'Used Buy' 
												WHEN 2 THEN 'Used Sell' 
												WHEN 3 THEN 'New Buy' END AS InquiryType
                     
               FROM           
                              TC_ActiveCalls         AS TCAC  WITH (NOLOCK) 
                      
                      JOIN    TC_CustomerDetails     AS C     WITH (NOLOCK) 
                                                                           ON TCAC.TC_LeadId = C.ActiveLeadId 
                      JOIN    TC_InquiriesLead       AS TCIL  WITH (NOLOCK) 
                                                                           ON TCAC.TC_LeadId = TCIL.TC_LeadId                       
					  JOIN    TC_InquirySource       AS TS 	  WITH(NOLOCK) 
						                                                   ON C.TC_InquirySourceId = TS.Id	                                                   
                                                                           
               WHERE 
                
                       
                         TCIL.TC_LeadStageId <> 3 
                AND      TCAC.TC_UsersId = @UserId
                AND      TCIL.TC_UserId=@UserId
                AND      TCIL.BranchId=@BranchId
                AND      ((@FromFolloupdate IS NULL) OR (TCAC.ScheduledOn >= @FromFolloupdate))
                AND      ((@ToFollowupdate IS NULL) OR (TCAC.ScheduledOn <= @ToFollowupdate))
                AND      (TCIL.TC_LeadDispositionID IS NULL  OR TCIL.TC_LeadDispositionID=4) ---Modified By Vivek Gupta on 16th sep,2013
               
               
               ORDER BY 
               
                      TCAC.ScheduledOn DESC

END
