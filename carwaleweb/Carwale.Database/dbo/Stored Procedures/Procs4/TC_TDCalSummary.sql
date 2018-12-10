IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_TDCalSummary]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_TDCalSummary]
GO

	-- =============================================    
-- Author:  <Reshma Shetty>    
-- Create date: <16/10/2012>    
-- Description: <Returns counts for various users of a particular dealer for a particular date range>    
-- EXEC [TC_TDCalSummary] 5, '2013-01-01','2013-03-04',1,20,1  
-- Modified By: Nilesh Utture on 30th October, 2012 Added extra check parameter TDStatus<>0   
-- Modified By: Nilesh Utture on 4th March, 2013 Added extra check TDC.TDStatus IS NOT NULL to retrive proper Record count
-- =============================================    
CREATE PROCEDURE [dbo].[TC_TDCalSummary]     
 -- Add the parameters for the stored procedure here    
 @BranchId BIGINT,    
 @FromDate DATE,    
 @ToDate DATE,    
 @FromIndex INT,    
 @ToIndex INT,  
 @Type INT  
     
AS    
BEGIN    
 -- SET NOCOUNT ON added to prevent extra result sets from    
 -- interfering with SELECT statements.    
 SET NOCOUNT ON;    
  IF @Type=1  
  BEGIN  
  -- Insert statements for procedure here    
	 SELECT *   
	  FROM(  
	   SELECT TDC.TC_UsersId,U.UserName,    
	   (SUM(Case WHEN(TDC.TDStatus IS NULL) THEN 0 ELSE 1 END)) AS Total,      
	   SUM(Case TDC.TDStatus WHEN 39 THEN 1 ELSE 0 END) Confirmed,    
	   SUM(Case TDC.TDStatus WHEN 28 THEN 1 ELSE 0 END) Completed,    
	   SUM(Case TDC.TDStatus WHEN 27 THEN 1 ELSE 0 END) Cancelled,  
	   ROW_NUMBER() OVER(ORDER BY TDC.TC_UsersId) AS RNO    
	   FROM TC_TDCalendar TDC  INNER JOIN TC_Users U ON TDC.TC_UsersId=U.Id  
	   WHERE TDC.BranchId=@BranchId AND TDC.TDDate BETWEEN  @FromDate AND @ToDate AND TDStatus<>0   
	   GROUP BY TDC.TC_UsersId, U.UserName) AS Res where RNO BETWEEN @FromIndex AND @ToIndex  
	   UNION  
	   SELECT * FROM(SELECT -1 AS TC_UsersId ,'ALL' AS UserName ,  
	   (SUM(Case WHEN(TDC.TDStatus IS NULL) THEN 0 ELSE 1 END)) AS Total,    
	   SUM(Case TDC.TDStatus WHEN 39 THEN 1 ELSE 0 END) Confirmed,    
	   SUM(Case TDC.TDStatus WHEN 28 THEN 1 ELSE 0 END) Completed,    
	   SUM(Case TDC.TDStatus WHEN 27 THEN 1 ELSE 0 END) Cancelled,  
	   0 AS RNO    
	   FROM TC_TDCalendar TDC  INNER JOIN TC_Users U ON TDC.TC_UsersId=U.Id  
	   WHERE TDC.BranchId=@BranchId AND TDC.TDDate BETWEEN  @FromDate AND @ToDate)AS TotalRes   
	    
	   SELECT COUNT(DISTINCT TC_UsersId) AS RecordCount  FROM TC_TDCalendar TDC    
	   WHERE TDC.BranchId=@BranchId AND TDC.TDDate BETWEEN @FromDate AND @ToDate AND TDC.TDStatus IS NOT NULL
  END  
    
  IF @Type=2  
  BEGIN  
	 SELECT *  
	  FROM(    
	   SELECT TDC.ArealId,TDC.AreaName,   
	   (SUM(Case WHEN(TDC.TDStatus IS NULL) THEN 0 ELSE 1 END)) AS Total,      
	   SUM(Case TDC.TDStatus WHEN 39 THEN 1 ELSE 0 END) Confirmed,    
	   SUM(Case TDC.TDStatus WHEN 28 THEN 1 ELSE 0 END) Completed,    
	   SUM(Case TDC.TDStatus WHEN 27 THEN 1 ELSE 0 END) Cancelled,    
	   ROW_NUMBER() OVER(ORDER BY TDC.AreaName) AS RNO    
	   FROM TC_TDCalendar TDC   
	   WHERE TDC.BranchId=@BranchId AND TDC.TDDate BETWEEN @FromDate AND @ToDate AND TDStatus<>0 
	   GROUP BY TDC.ArealId,TDC.AreaName)AS Res   
	   WHERE RNO BETWEEN @FromIndex AND @ToIndex    
	   UNION  
	   SELECT * FROM(SELECT -1 AS ArealId ,'ALL' AS AreaName ,  
	   (SUM(Case WHEN(TDC.TDStatus IS NULL) THEN 0 ELSE 1 END)) AS Total,    
	   SUM(Case TDC.TDStatus WHEN 39 THEN 1 ELSE 0 END) Confirmed,    
	   SUM(Case TDC.TDStatus WHEN 28 THEN 1 ELSE 0 END) Completed,    
	   SUM(Case TDC.TDStatus WHEN 27 THEN 1 ELSE 0 END) Cancelled,  
	   0 AS RNO    
	   FROM TC_TDCalendar TDC   
	   WHERE TDC.BranchId=@BranchId AND TDC.TDDate BETWEEN  @FromDate AND @ToDate)AS TotalRes   
	     
	   SELECT COUNT(DISTINCT ArealId) as RecordCount  FROM TC_TDCalendar TDC    
	   WHERE TDC.BranchId=@BranchId AND TDC.TDDate BETWEEN @FromDate AND @ToDate AND TDC.TDStatus IS NOT NULL
  END  
    
  IF @Type=3  
  BEGIN  
	 SELECT *  
	  FROM(    
	   SELECT TDC.TC_TDCarsId,TDC.TDCarDetails,   
	   (SUM(Case WHEN(TDC.TDStatus IS NULL) THEN 0 ELSE 1 END)) AS Total,    
	   SUM(Case TDC.TDStatus WHEN 39 THEN 1 ELSE 0 END) Confirmed,    
	   SUM(Case TDC.TDStatus WHEN 28 THEN 1 ELSE 0 END) Completed,    
	   SUM(Case TDC.TDStatus WHEN 27 THEN 1 ELSE 0 END) Cancelled,    
	   ROW_NUMBER() OVER(ORDER BY TDC.TDCarDetails) AS RNO    
	   FROM TC_TDCalendar TDC    
	   WHERE TDC.BranchId=@BranchId AND TDC.TDDate BETWEEN @FromDate AND @ToDate AND TDStatus<>0 
	   GROUP BY TDC.TC_TDCarsId,TDC.TDCarDetails)AS Res   
	   WHERE RNO BETWEEN @FromIndex AND @ToIndex    
	   UNION  
	   SELECT * FROM(SELECT -1 AS TC_TDCarsId ,'ALL' AS AreaName ,  
	   (SUM(Case WHEN(TDC.TDStatus IS NULL) THEN 0 ELSE 1 END)) AS Total,    
	   SUM(Case TDC.TDStatus WHEN 39 THEN 1 ELSE 0 END) Confirmed,    
	   SUM(Case TDC.TDStatus WHEN 28 THEN 1 ELSE 0 END) Completed,    
	   SUM(Case TDC.TDStatus WHEN 27 THEN 1 ELSE 0 END) Cancelled,  
	   0 AS RNO    
	   FROM TC_TDCalendar TDC   
	   WHERE TDC.BranchId=@BranchId AND TDC.TDDate BETWEEN  @FromDate AND @ToDate)AS TotalRes   
	     
	   SELECT COUNT(DISTINCT TC_TDCarsId) AS RecordCount  FROM TC_TDCalendar TDC    
	   WHERE TDC.BranchId=@BranchId AND TDC.TDDate BETWEEN @FromDate AND @ToDate  AND TDC.TDStatus IS NOT NULL
  END  
END 





SET ANSI_NULLS ON
