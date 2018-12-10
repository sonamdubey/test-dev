IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_TDCalDatewiseSummary]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_TDCalDatewiseSummary]
GO

	-- =============================================  
-- Author:  Surendra
-- Create date: 18-10-2012
-- Description: Return datewise report of calendar based on User,Car and Areawise withing particalr details
-- EXEC [TC_TDCalDatewiseSummary] 1,1,5, '2012-01-01','2012-10-17',1,20
-- =============================================  
CREATE  Procedure [dbo].[TC_TDCalDatewiseSummary]   
 -- Add the parameters for the stored procedure here  
 @SearchId INT,-- (It can be UserId,CarId,AreaID depending On)
 @SearchType TinyInt,--(1=Userwise,2=Areawise,3=Carwise)
 @BranchId BIGINT,   
 @FromDate DATE,  
 @ToDate DATE,  
 @FromIndex INT,  
 @ToIndex INT
   
AS  
BEGIN  
 -- SET NOCOUNT ON added to prevent extra result sets from  
 -- interfering with SELECT statements.  
 SET NOCOUNT ON;  
  IF @SearchType=1
  BEGIN
		-- Insert statements for procedure here  
		SELECT *
		FROM(  
	  SELECT TDC.TDDate,
	  (SUM(Case WHEN(TDC.TDStatus IS NULL) THEN 0 ELSE 1 END)) AS Total,	 
	  SUM(Case TDC.TDStatus WHEN 39 THEN 1 ELSE 0 END) Confirmed,    
	  SUM(Case TDC.TDStatus WHEN 28 THEN 1 ELSE 0 END) Completed,    
	  SUM(Case TDC.TDStatus WHEN 27 THEN 1 ELSE 0 END) Cancelled, 
	  ROW_NUMBER() OVER(ORDER BY TDC.TDDate) AS RNO  
	  FROM TC_TDCalendar TDC  INNER JOIN TC_Users U ON TDC.TC_UsersId=U.Id
	  WHERE TDC.BranchId=@BranchId AND TDC.TC_UsersId=@SearchId AND TDC.TDDate BETWEEN @FromDate AND @ToDate  AND TDStatus<>0
	  GROUP BY TDC.TDDate)AS Tab 
	  WHERE RNO BETWEEN @FromIndex AND @ToIndex  
	 
	  Select COUNT(distinct TDDate) as RecordCount  FROM TC_TDCalendar TDC  
	  WHERE TDC.BranchId=@BranchId AND TDC.TC_UsersId=@SearchId AND TDC.TDDate BETWEEN @FromDate AND @ToDate  AND TDStatus<>0
  END
  
  IF @SearchType=2
  BEGIN
	  SELECT *
		FROM(
		SELECT TDC.TDDate, 
	  (SUM(Case WHEN(TDC.TDStatus IS NULL) THEN 0 ELSE 1 END)) AS Total,	
	  SUM(Case TDC.TDStatus WHEN 39 THEN 1 ELSE 0 END) Confirmed,    
	  SUM(Case TDC.TDStatus WHEN 28 THEN 1 ELSE 0 END) Completed,    
	  SUM(Case TDC.TDStatus WHEN 27 THEN 1 ELSE 0 END) Cancelled, 
	  ROW_NUMBER() OVER(ORDER BY TDC.TDDate) AS RNO  
	  FROM TC_TDCalendar TDC 
	  WHERE TDC.BranchId=@BranchId AND TDC.ArealId=@SearchId AND TDC.TDDate BETWEEN @FromDate AND @ToDate  AND TDStatus<>0
	  GROUP BY TDC.TDDate)AS Tab 
	  WHERE RNO BETWEEN @FromIndex AND @ToIndex  
	  
	  Select COUNT(distinct TDDate) as RecordCount  FROM TC_TDCalendar TDC  
	  WHERE TDC.BranchId=@BranchId AND TDC.ArealId=@SearchId AND TDC.TDDate BETWEEN @FromDate AND @ToDate  AND TDStatus<>0
  END
  
  IF @SearchType=3
  BEGIN
	  SELECT *
		FROM(  
	  SELECT TDC.TDDate, 
	  (SUM(Case WHEN(TDC.TDStatus IS NULL) THEN 0 ELSE 1 END)) AS Total,	
	  SUM(Case TDC.TDStatus WHEN 39 THEN 1 ELSE 0 END) Confirmed,    
	  SUM(Case TDC.TDStatus WHEN 28 THEN 1 ELSE 0 END) Completed,    
	  SUM(Case TDC.TDStatus WHEN 27 THEN 1 ELSE 0 END) Cancelled, 
	  ROW_NUMBER() OVER(ORDER BY TDC.TDDate) AS RNO  
	  FROM TC_TDCalendar TDC  
	  WHERE TDC.BranchId=@BranchId AND TDC.TC_TDCarsId=@SearchId  AND TDC.TDDate BETWEEN @FromDate AND @ToDate  AND TDStatus<>0
	  GROUP BY TDC.TDDate)AS Tab 
	  WHERE RNO BETWEEN @FromIndex AND @ToIndex  
	  
	  Select COUNT(distinct TDDate) as RecordCount  FROM TC_TDCalendar TDC  
	  WHERE TDC.BranchId=@BranchId AND TDC.TC_TDCarsId=@SearchId  AND TDC.TDDate BETWEEN @FromDate AND @ToDate  AND TDStatus<>0
  END
END
