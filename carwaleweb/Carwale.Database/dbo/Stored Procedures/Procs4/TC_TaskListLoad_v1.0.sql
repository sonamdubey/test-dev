IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_TaskListLoad_v1]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_TaskListLoad_v1]
GO

	-- =============================================  
-- Modified By: Nilesh Utture on 24th Jan, 2013 9.0 pm Added extra condition "OR TCIL.TC_LeadDispositionID=4" in SELECT Query
--- Author:  Manish    
-- Create date:   10-01-2013  
-- Description:    
--execute [TC_TaskListLoad] 5,1,1,10,null,null,null,NULL,NULL,NULL
--Modify by Surendra  on 27-02-2013  changing latestinquiry date
--Modify by Surendra  on 22-03-2013  adding condition of userid in inquiries lead table
--Modify by Tejashree Patil on 23 April 2013 fetched sourceId,TC_LeadStageId.
--Modify by Surendra on 26 April 2013 include inq source in select list
--Modified By Vivek Gupta on 8th May,2013 Added a parameter @SearchText AND @searchType coz more filtered data is needed,
--                    declared @SearchCar,@SearchSource,@SearchUser,@ChangedUserId.
--Modified By:Vivek Gupta on 15th,May,2013..Removed @ToFollowupdate condition for getting Current date in case of Null.
--Modified By Vivek Gupta on 30th MAy,2013..Added condition for preventing auto date assignment
--Modified By Manish Chourasiya on 31-05-2013 For correcting from date and to date filter and Ordering of the leads
-- =============================================     
create PROCEDURE [dbo].[TC_TaskListLoad_v1.0]  
  -- Add the parameters for the stored procedure here     
  @BranchId        BIGINT, 
  @UserId          BIGINT, 
  @FromIndex       INT, 
  @ToIndex         INT, 

  --@Type TINYINT , 
  @CustomerName    VARCHAR(100), 
  @CustomerMobile  VARCHAR(50), 
  @CustomerEmail   VARCHAR(100),
  @FromFolloupdate AS DATETIME, 
  @ToFollowupdate  AS DATETIME,
  @SearchText      VARCHAR(50) = NULL,
  @FilterType      TINYINT = 1
  
AS 
BEGIN 

        DECLARE  @IsSearchText bit=0;
        IF((@CustomerName IS NOT NULL OR @CustomerMobile IS NOT NULL OR @CustomerEmail IS NOT NULL OR @SearchText IS NOT NULL))
        BEGIN
            set @IsSearchText=1
        END
          print  CONVERT(VARCHAR,@BranchId )+ ' : '  + CONVERT(VARCHAR,@UserId ) + ' : '  + CONVERT(VARCHAR,@FromIndex ) + ' : '  + CONVERT(VARCHAR,@ToIndex ) 
    + ' : '  + @CustomerName + ' : '  + @CustomerMobile + ' : '  + @CustomerEmail + ' : '  + CONVERT(VARCHAR,@FromFolloupdate )         
    + ' : '  + CONVERT(VARCHAR,@ToFollowupdate )  + ' : '  + CONVERT(VARCHAR,@SearchText )          + ' : '  + CONVERT(VARCHAR,@FilterType )
        	
    SET  @FromFolloupdate = convert(datetime,convert(varchar(10),@FromFolloupdate,120)+ ' 00:00:00')	
    SET  @ToFollowupdate = convert(datetime,convert(varchar(10),@ToFollowupdate,120)+ ' 23:59:59')
    
    
      --  Lead Scheduling for verifications 
	EXECUTE TC_LeadVerificationScheduling @TC_Usersid=@UserId, @DealerId=@BranchId 

	DECLARE @SearchCar VARCHAR(50) = NULL;
	DECLARE @SearchSource VARCHAR(50) = NULL;
	DECLARE @SearchUser VARCHAR(50) = NULL;
	
	--DECLARE @Todate DATE=getdate(); 
	--DECLARE @ToDatetime DATETIME = convert(datetime,convert(varchar(10),GETDATE(),120)+ ' 23:59:59')
	--SELECT @ToDatetime = DATEADD(hh, 23, @toDatetime); 
	--SELECT @ToDatetime = DATEADD(MINUTE, 59, @toDatetime); 
   
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
     END
    
    
     --PRINT 'fromdate' + isnull(convert(varchar,@FromFolloupdate),'~')
     --PRINT 'todate' +isnull(convert(varchar,@ToFollowupdate),'~')
     
  	
	IF(@FilterType=2)
	BEGIN
	    SET @SearchCar = @SearchText
	END;
	ELSE IF(@FilterType=3)
	BEGIN
	    SET @SearchSource = @SearchText
	END;
    ELSE IF(@FilterType=4)
	BEGIN
	    SET @SearchUser = @SearchText
	    SET @UserId = NULL
	END; 


 --print  CONVERT(VARCHAR,@BranchId )+ ' : '  + CONVERT(VARCHAR,@UserId ) + ' : '  + CONVERT(VARCHAR,@FromIndex ) + ' : '  + CONVERT(VARCHAR,@ToIndex ) 
 --   + ' : '  + isnull(@CustomerName,'~') + ' : '  +ISNULL( @CustomerMobile,'~') + ' : '  +ISNULL( @CustomerEmail,'~') + ' : '  + 
 --   isnull(CONVERT(VARCHAR,@FromFolloupdate ) ,'~')        
 --   + ' : '  + isnull(CONVERT(VARCHAR,@ToFollowupdate ),'~')  + ' : '  + isnull(CONVERT(VARCHAR,@SearchText ),'~')
 --    + ' : '  + CONVERT(VARCHAR,@FilterType )         
 --   + ' : '  + isnull(@SearchCar,'~')
 --   + ' : '  + isnull(@SearchSource,'~')
 --   + ' : '  + isnull(@SearchUser ,'~');


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
                --TCAC.ScheduledON BETWEEN  @FromFolloupdate AND @ToFollowupdate 
                        (( @ToFollowupdate IS NULL ) OR (TCAC.ScheduledON <= @ToFollowupdate) )
                AND      TCIL.TC_LeadStageId <> 3 
                AND      ( TCAC.TC_UsersId = @UserId  OR  @UserId  IS NULL)
                AND      (TCIL.TC_UserId=@UserId OR  @UserId  IS NULL)
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
                          ORDER BY  cte1.LatestInquiryDate  desc,
                          --CONVERT(TIME,NextFollowUpDate) DESC,
                       NextFollowUpDate  DESC ) RowNumber
               FROM   Cte1 
               WHERE  RowNum = 1)
         
              SELECT * 
              INTO   #TblTemp 
              FROM   Cte2 

		      SELECT * 
		      FROM   #TblTemp 
		      WHERE  RowNumber BETWEEN @FromIndex AND @ToIndex 
		      --ORDER BY LatestInquiryDate DESC,NextFollowUpDate DESC

		      SELECT COUNT(*) AS RecordCount 
		      FROM   #TblTemp 

		      DROP TABLE #TblTemp 
    END
