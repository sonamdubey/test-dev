IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_TaskListLoadCommon]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_TaskListLoadCommon]
GO

	--- Author:  SURENDRA    
-- Create date: 18-04-2013  
-- Description: This sp for Stocklist display 
-- [TC_TaskListLoadCommon] 5,4081,1,20,null,null
-- Modified by: Tejashree on 26-08-2013, Fetched complete name of a customer.
--Modified By: Vivek Gupta, added IsVerified Field from customer table to know if the customer is verified or not
-- =============================================     
CREATE PROCEDURE [dbo].[TC_TaskListLoadCommon] 
  -- Add the parameters for the stored procedure here     
  @BranchId        BIGINT, 
  --@UserId          BIGINT, 
  @StockId BIGINT =NULL,
  @FromIndex       INT, 
  @ToIndex         INT,
  @PageSource TINYINT, -- refer enum created in fron end  ,
  @FreshLead BIT=0  
AS 
BEGIN 
IF(@PageSource=1)
	BEGIN
      WITH Cte1 
           AS (SELECT C.id                  AS [CustomerId], 
                      (ISNULL(C.Salutation,'')+' '+ C.CustomerName+' '+ISNULL(C.LastName,'')) AS [CustomerName], 
                      C.Email, C.Mobile,TCAC.TC_LeadId, TC_InquiryStatusId, 
                      ScheduledOn AS [NextFollowUpDate],L.CarDetails  AS [InterestedIn], 
                      TCAC.CallType,TCAC.LastCallComment,LatestInquiryDate,
                      U.Username,U.Id AS UserId,
                      CASE TC_LeadInquiryTypeId WHEN 1 THEN 'Used Buy' 
												WHEN 2 THEN 'Used Sell' 
												WHEN 3 THEN 'New Buy' END AS InquiryType, 
                      row_number() 
                        OVER ( 
                                 Partition BY TCAC.TC_LeadId 
                                 ORDER BY TCAC.ScheduledOn DESC,L.LatestInquiryDate DESC) AS RowNum ,
					   C.IsVerified
               FROM TC_BuyerInquiries B WITH (NOLOCK) INNER JOIN TC_InquiriesLead L WITH (NOLOCK) ON B.TC_InquiriesLeadId =L.TC_InquiriesLeadId
               INNER JOIN TC_ActiveCalls  AS TCAC  WITH (NOLOCK) ON L.TC_LeadId=TCAC.TC_LeadId
               INNER JOIN TC_Users U WITH (NOLOCK) ON TCAC.TC_UsersId=U.Id 
               INNER JOIN TC_CustomerDetails AS C WITH (NOLOCK) ON TCAC.TC_LeadId = C.ActiveLeadId 
               WHERE
				ISNULL(L.TC_LeadStageId,0) <> 3 AND
                 L.BranchId=@BranchId  
					AND B.StockId=@StockId              
              ), 
           Cte2 
           AS (SELECT *, ROW_NUMBER() OVER ( ORDER BY cte1.LatestInquiryDate DESC,NextFollowUpDate DESC ) RowNumber 
               FROM   Cte1 WHERE  RowNum = 1)
         
              SELECT * INTO   #TblTemp FROM Cte2 

      SELECT * FROM   #TblTemp WHERE  rownumber BETWEEN @FromIndex AND @ToIndex 
      --ORDER BY LatestInquiryDate DESC,NextFollowUpDate DESC

      SELECT COUNT(*) AS RecordCount FROM  #TblTemp 
      DROP TABLE #TblTemp 		      
    END 
ELSE IF(@PageSource=2) -- navigated from import used buy page
    BEGIN
		WITH Cte1 
           AS (SELECT C.id                  AS [CustomerId], 
                      C.CustomerName        AS [CustomerName], 
                      C.Email, C.Mobile,TCAC.TC_LeadId, TC_InquiryStatusId, 
                      ScheduledOn AS [NextFollowUpDate],L.CarDetails  AS [InterestedIn], 
                      TCAC.CallType,TCAC.LastCallComment,LatestInquiryDate,
                      U.Username,U.Id AS UserId,
                      CASE TC_LeadInquiryTypeId WHEN 1 THEN 'Used Buy' WHEN 2 THEN 'Used Sell' WHEN 3 THEN 'New Buy' END AS InquiryType, 
                      row_number() OVER ( Partition BY TCAC.TC_LeadId ,
					  C.IsVerified
					ORDER BY TCAC.ScheduledOn DESC,L.LatestInquiryDate DESC) AS RowNum 
					FROM TC_BuyerInquiries B WITH (NOLOCK) INNER JOIN TC_InquiriesLead L WITH (NOLOCK) 
					ON B.TC_InquiriesLeadId =L.TC_InquiriesLeadId AND B.TC_InquirySourceId IN(6,7,8,9)
					INNER JOIN TC_ActiveCalls  AS TCAC  WITH (NOLOCK) ON L.TC_LeadId=TCAC.TC_LeadId
					INNER JOIN TC_Users U WITH (NOLOCK) ON TCAC.TC_UsersId=U.Id 
					INNER JOIN TC_CustomerDetails AS C WITH (NOLOCK) ON TCAC.TC_LeadId = C.ActiveLeadId 
					WHERE L.BranchId=@BranchId 
					AND L.TC_LeadStageId=CASE WHEN @FreshLead=1 THEN 1 WHEN @FreshLead=0 THEN 0 END   
					--AND CONVERT. (B.CreatedOn=GETDATE     
              ), 
           Cte2 
           AS (SELECT *, ROW_NUMBER() OVER ( ORDER BY cte1.LatestInquiryDate DESC,NextFollowUpDate DESC ) RowNumber 
               FROM   Cte1 WHERE  RowNum = 1)
         
              SELECT * INTO   #TblTempBuyer FROM Cte2 

		SELECT * FROM   #TblTempBuyer WHERE  rownumber BETWEEN @FromIndex AND @ToIndex 
		--ORDER BY LatestInquiryDate DESC,NextFollowUpDate DESC

		SELECT COUNT(*) AS RecordCount FROM  #TblTempBuyer 
		DROP TABLE #TblTempBuyer     
    END

ELSE IF(@PageSource=3) -- navigated from import used sell page
    BEGIN
		WITH Cte1 
           AS (SELECT C.id                  AS [CustomerId], 
                      C.CustomerName        AS [CustomerName], 
                      C.Email, C.Mobile,TCAC.TC_LeadId, TC_InquiryStatusId, 
                      ScheduledOn AS [NextFollowUpDate],L.CarDetails  AS [InterestedIn], 
                      TCAC.CallType,TCAC.LastCallComment,LatestInquiryDate,
                      U.Username,U.Id AS UserId,
                      CASE TC_LeadInquiryTypeId WHEN 1 THEN 'Used Buy' WHEN 2 THEN 'Used Sell' WHEN 3 THEN 'New Buy' END AS InquiryType, 
                      row_number() OVER ( Partition BY TCAC.TC_LeadId ,
					  C.IsVerified
					ORDER BY TCAC.ScheduledOn DESC,L.LatestInquiryDate DESC) AS RowNum 
					FROM TC_SellerInquiries B WITH (NOLOCK) INNER JOIN TC_InquiriesLead L WITH (NOLOCK) 
					ON B.TC_InquiriesLeadId =L.TC_InquiriesLeadId AND B.TC_InquirySourceId IN(6,7,8,9)
					INNER JOIN TC_ActiveCalls  AS TCAC  WITH (NOLOCK) ON L.TC_LeadId=TCAC.TC_LeadId
					INNER JOIN TC_Users U WITH (NOLOCK) ON TCAC.TC_UsersId=U.Id 
					INNER JOIN TC_CustomerDetails AS C WITH (NOLOCK) ON TCAC.TC_LeadId = C.ActiveLeadId 
					WHERE L.BranchId=@BranchId 
					AND L.TC_LeadStageId=CASE WHEN @FreshLead=1 THEN 1 WHEN @FreshLead=0 THEN 0 END        
              ), 
           Cte2 
           AS (SELECT *, ROW_NUMBER() OVER ( ORDER BY cte1.LatestInquiryDate DESC,NextFollowUpDate DESC ) RowNumber 
               FROM   Cte1 WHERE  RowNum = 1)
         
              SELECT * INTO   #TblTempSeller FROM Cte2 

		SELECT * FROM   #TblTempSeller WHERE  rownumber BETWEEN @FromIndex AND @ToIndex 
		--ORDER BY LatestInquiryDate DESC,NextFollowUpDate DESC

		SELECT COUNT(*) AS RecordCount FROM  #TblTempSeller 
		DROP TABLE #TblTempSeller     
    END
    
ELSE IF(@PageSource=4) -- navigated from import new buy page
    BEGIN
		WITH Cte1 
           AS (SELECT C.id                  AS [CustomerId], 
                      C.CustomerName        AS [CustomerName], 
                      C.Email, C.Mobile,TCAC.TC_LeadId, TC_InquiryStatusId, 
                      ScheduledOn AS [NextFollowUpDate],L.CarDetails  AS [InterestedIn], 
                      TCAC.CallType,TCAC.LastCallComment,LatestInquiryDate,
                      U.Username,U.Id AS UserId,
                      CASE TC_LeadInquiryTypeId WHEN 1 THEN 'Used Buy' WHEN 2 THEN 'Used Sell' WHEN 3 THEN 'New Buy' END AS InquiryType, 
                      row_number() OVER ( Partition BY TCAC.TC_LeadId ,
					  C.IsVerified
					ORDER BY TCAC.ScheduledOn DESC,L.LatestInquiryDate DESC) AS RowNum 
					FROM TC_NewCarInquiries B WITH (NOLOCK) INNER JOIN TC_InquiriesLead L WITH (NOLOCK) 
					ON B.TC_InquiriesLeadId =L.TC_InquiriesLeadId AND B.TC_InquirySourceId IN(6,7,8,9)
					INNER JOIN TC_ActiveCalls  AS TCAC  WITH (NOLOCK) ON L.TC_LeadId=TCAC.TC_LeadId
					INNER JOIN TC_Users U WITH (NOLOCK) ON TCAC.TC_UsersId=U.Id 
					INNER JOIN TC_CustomerDetails AS C WITH (NOLOCK) ON TCAC.TC_LeadId = C.ActiveLeadId 
					WHERE L.BranchId=@BranchId 
					AND L.TC_LeadStageId=CASE WHEN @FreshLead=1 THEN 1 WHEN @FreshLead=0 THEN 0 END        
              ), 
           Cte2 
           AS (SELECT *, ROW_NUMBER() OVER ( ORDER BY cte1.LatestInquiryDate DESC,NextFollowUpDate DESC ) RowNumber 
               FROM   Cte1 WHERE  RowNum = 1)
         
              SELECT * INTO   #TblTempNewBuyer FROM Cte2 

		SELECT * FROM   #TblTempNewBuyer WHERE  rownumber BETWEEN @FromIndex AND @ToIndex 
		--ORDER BY LatestInquiryDate DESC,NextFollowUpDate DESC

		SELECT COUNT(*) AS RecordCount FROM  #TblTempNewBuyer 
		DROP TABLE #TblTempNewBuyer     
    END
    
END
  
  
  


