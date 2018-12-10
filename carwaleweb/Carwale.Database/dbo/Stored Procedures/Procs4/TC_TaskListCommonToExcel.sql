IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_TaskListCommonToExcel]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_TaskListCommonToExcel]
GO

	
-- Author		:	Umesh
-- Modified By  :   Umesh for New paramaeters for Page source from where the page called
-- Create date	:	04-02-2013  
-- Description	:	Send to excel funtionality in task list page    
-- execute [TC_TaskListCommonToExcel] 968,106,1 
-- =============================================     
CREATE PROCEDURE [dbo].[TC_TaskListCommonToExcel] 
  -- Add the parameters for the stored procedure here     
  @BranchId        BIGINT, 
  --@UserId          BIGINT, 
  @StockId BIGINT,
  @PageSource TINYINT =NULL -- refer enum created in fron end  ,
 
AS 
BEGIN 
IF(@PageSource=1)
BEGIN
	SELECT C.id   AS [CustomerId], 
                      C.CustomerName        AS [CustomerName], 
                      C.Email, C.Mobile,TCAC.TC_LeadId, TC_InquiryStatusId, 
                      ScheduledOn AS [NextFollowUpDate],L.CarDetails  AS [InterestedIn], 
                      TCAC.CallType,TCAC.LastCallComment,LatestInquiryDate,
                      U.Username,
                      CASE TC_LeadInquiryTypeId WHEN 1 THEN 'Used Buy' 
												WHEN 2 THEN 'Used Sell' 
												WHEN 3 THEN 'New Buy' END AS InquiryType, 
                      row_number() 
                        OVER ( 
                                 Partition BY TCAC.TC_LeadId 
                                 ORDER BY TCAC.ScheduledOn DESC,L.LatestInquiryDate DESC) AS RowNum 
               FROM TC_BuyerInquiries B WITH (NOLOCK) INNER JOIN TC_InquiriesLead L WITH (NOLOCK) ON B.TC_InquiriesLeadId =L.TC_InquiriesLeadId
               INNER JOIN TC_ActiveCalls  AS TCAC  WITH (NOLOCK) ON L.TC_LeadId=TCAC.TC_LeadId
               INNER JOIN TC_Users U WITH (NOLOCK) ON TCAC.TC_UsersId=U.Id 
               INNER JOIN TC_CustomerDetails AS C WITH (NOLOCK) ON TCAC.TC_LeadId = C.ActiveLeadId 
               WHERE ISNULL(L.TC_LeadStageId,0) <> 3 AND L.BranchId=@BranchId 
					AND B.StockId=@StockId 
END        
  
END    