IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_TaskListLoadForCRE]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_TaskListLoadForCRE]
GO

	-- =============================================  
--- Author:  Manish    
-- Create date:   10-01-2013  
-- Description:    
/*
@FilterType value 
1 : Customer Name
2 : Customer Mobile
3 : Customer Email
4 : Lead Owner
5 : Car Name
6 : Source
*/
--execute [TC_TaskListLoadForCRE] 5,1,1,10,null,null,null,NULL,NULL,NULL
-- =============================================     
CREATE PROCEDURE [dbo].[TC_TaskListLoadForCRE]  
  -- Add the parameters for the stored procedure here     
  @BranchId        BIGINT,
  @FromIndex       INT, 
  @ToIndex         INT, 
  @FromFolloupdate AS DATETIME, 
  @ToFollowupdate  AS DATETIME,
  @SearchText      VARCHAR(50) = NULL,
  @FilterType      TINYINT = 0
  
AS 
BEGIN 

DECLARE  @CustomerName    VARCHAR(100),   @CustomerMobile  VARCHAR(50),   @CustomerEmail   VARCHAR(100)
DECLARE @SearchCar VARCHAR(50) = NULL;
DECLARE @SearchSource VARCHAR(50) = NULL;
DECLARE @SearchUser VARCHAR(50) = NULL;
	
---------------------------------------------------------------------------------		
 IF(@FilterType=1)
	BEGIN
	    SET @CustomerName = @SearchText
	END;

ELSE IF(@FilterType=2)
	BEGIN
	    SET @CustomerMobile = @SearchText
	END;
ELSE IF(@FilterType=3)
	BEGIN
	    SET @CustomerEmail = @SearchText
	END;
ELSE IF(@FilterType=4)
	BEGIN
	    SET @SearchUser = @SearchText	 
	END; 
ELSE IF(@FilterType=5)
	BEGIN
	    SET @SearchCar = @SearchText
	END;
ELSE IF(@FilterType=6)
	BEGIN
	    SET @SearchSource  = @SearchText
	END;
--------------------------------------------------------------------------------------------------

    DECLARE  @IsSearchText bit=0;       
    IF( @FilterType <> 0 )
    BEGIN
        set @IsSearchText=1
    END
        	
    SET  @FromFolloupdate = convert(datetime,convert(varchar(10),@FromFolloupdate,120)+ ' 00:00:00')	
    SET  @ToFollowupdate = convert(datetime,convert(varchar(10),@ToFollowupdate,120)+ ' 23:59:59')
   
   IF(@FromFolloupdate IS NOT NULL AND @ToFollowupdate IS NULL)
	  BEGIN 
		   SET @FromFolloupdate = convert(datetime,convert(varchar(10),@FromFolloupdate,120)+ ' 00:00:00')
		   SET @ToFollowupdate = convert(datetime,convert(varchar(10),@FromFolloupdate,120)+ ' 23:59:59')       
	   END
		   
	ELSE IF(@FromFolloupdate IS NULL AND @ToFollowupdate IS NOT NULL)
	  BEGIN 
		   SET @FromFolloupdate = convert(datetime,convert(varchar(10),@ToFollowupdate,120)+ ' 00:00:00')
		   SET @ToFollowupdate = convert(datetime,convert(varchar(10),@ToFollowupdate,120)+ ' 23:59:59')       
	END
		
    IF(@IsSearchText <> 1)
    BEGIN    
		IF(@FromFolloupdate IS NULL AND @ToFollowupdate IS  NULL)
		  BEGIN 
			   SET @ToFollowupdate = convert(datetime,convert(varchar(10),GETDATE(),120)+ ' 23:59:59')      
		   END	   
     END;
  	
	

        WITH Cte1 
           AS (SELECT C.id                  AS [CustomerId], 
                      C.CustomerName        AS [CustomerName], 
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
                      (CASE  WHEN LatestInquiryDate > ScheduledOn THEN LatestInquiryDate ELSE ScheduledOn END) AS ORDERDATE,
                      TS.Source AS InquirySource,
                      TCIL.TC_UserId AS UserId,
                      TCIL.TC_LeadStageId,
                      CASE TC_LeadInquiryTypeId WHEN 1 THEN 'Used Buy' 
												WHEN 2 THEN 'Used Sell' 
												WHEN 3 THEN 'New Buy' END AS InquiryType, 
                      row_number() 
                        OVER ( 
                                 Partition BY TCAC.TC_LeadId 
                                 ORDER BY TCAC.ScheduledOn DESC,tcil.LatestInquiryDate DESC) AS RowNum 
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
                        (( @ToFollowupdate IS NULL ) OR (TCAC.ScheduledON <= @ToFollowupdate) )
                AND      TCIL.TC_LeadStageId <> 3 
               -- AND      ( TCAC.TC_UsersId = @UserId  OR  @UserId  IS NULL)
              --  AND      (TCIL.TC_UserId=@UserId OR  @UserId  IS NULL)
                AND      TCIL.BranchId=@BranchId
                AND      ( ( @FromFolloupdate IS NULL )      OR ( TCAC.ScheduledON >= @FromFolloupdate ) ) 
                AND      (TCIL.TC_LeadDispositionID IS NULL  OR TCIL.TC_LeadDispositionID=4) -- Modified By: Nilesh Utture on 24th Jan, 2013 9.0 pm 
                AND      ( ( @CustomerName IS NULL )         OR ( C.CustomerName = @CustomerName ) ) 
                AND      ( ( @SearchCar IS NULL )            OR (TCIL.CarDetails LIKE  '%' + @SearchCar + '%' ))
                AND      ( ( @SearchSource IS NULL )            OR (TS.Source LIKE  '%' + @SearchSource + '%' ))
                AND      ( ( @SearchUser IS NULL )            OR (TU.UserName LIKE  '%' + @SearchUser + '%' ))                
                AND      ( ( @CustomerMobile IS NULL )       OR ( C.Mobile = @CustomerMobile ) ) 
                AND      ( ( @CustomerEmail IS NULL )        OR ( C.Email = @CustomerEmail ) )), 
           Cte2 
           AS (SELECT *, 
                      ROW_NUMBER() 
                        OVER ( 
                          ORDER BY  NextFollowUpDate DESC, LatestInquiryDate DESC
                          --cte1.ORDERDATE  desc
                          --,
                          --CONVERT(TIME,NextFollowUpDate) DESC,
                       --NextFollowUpDate  DESC 
                       ) RowNumber
               FROM   Cte1 
               WHERE  RowNum = 1)
         
              SELECT * 
              INTO   #TblTemp 
              FROM   Cte2 ;

		      SELECT * 
		      FROM   #TblTemp 
		      WHERE  RowNumber BETWEEN @FromIndex AND @ToIndex 
		      --ORDER BY LatestInquiryDate DESC,NextFollowUpDate DESC

		      SELECT COUNT(*) AS RecordCount 
		      FROM   #TblTemp 

		      DROP TABLE #TblTemp 
    END
