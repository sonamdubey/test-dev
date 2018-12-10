IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[UCAlert].[RemoveOldThreadForUsedCarMailer]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [UCAlert].[RemoveOldThreadForUsedCarMailer]
GO

	-- =============================================
-- Author:		Manish Chourasiya
-- Create date: 23-04-2014
-- Description:	Remove the old entries if second mailer thread starts.
-- =============================================
CREATE PROCEDURE [UCAlert].[RemoveOldThreadForUsedCarMailer]
AS
BEGIN
  
  WITH  CTE AS 
  (
	  SELECT  U.UsedCarAlert_Id,U.CustomerId, ROW_NUMBER() OVER(PARTITION BY U.CustomerId ORDER BY U.EntryDateTime DESC) ROWNUM
	  from  UCAlert.UserCarAlerts U WITH(NOLOCK)
	   WHERE CustomerId in 
                  ( SELECT A.CustomerId 
	                FROM UCAlert.RecommendUsedCarAlert AS A WITH(NOLOCK) 
	                JOIN UCAlert.RecommendUsedCarAlert AS B WITH(NOLOCK) ON A.CustomerId=B.CustomerId
	                WHERE A.IsFirstMail=0
	                AND B.IsFirstMail=1
                   )
   )
  UPDATE  A SET IsActive=0
  FROM UCAlert.UserCarAlerts AS A  WITH(NOLOCK)
  JOIN CTE ON CTE.UsedCarAlert_Id=A.UsedCarAlert_Id
           AND CTE.RowNum>1;
  
  DELETE FROM UCAlert.RecommendUsedCarAlert 
  WHERE CustomerId in 
  ( SELECT A.CustomerId 
	FROM UCAlert.RecommendUsedCarAlert AS A WITH(NOLOCK) 
	JOIN UCAlert.RecommendUsedCarAlert AS B WITH(NOLOCK) ON A.CustomerId=B.CustomerId
	WHERE A.IsFirstMail=0
	AND B.IsFirstMail=1
  )
  AND IsFirstMail=0;
  
  
             
  WITH  CTE AS 
  (
	  
	  SELECT U.UsedCarAlert_Id, ROW_NUMBER()OVER(PARTITION BY CustomerId ORDER BY EntryDateTime DESC) RowNum
	  FROM UCAlert.UserCarAlerts U WITH(NOLOCK) WHERE U.CustomerId IN 
	  ( 
		  SELECT  U.CustomerId
		  FROM  UCAlert.UserCarAlerts U WITH(NOLOCK)
		  WHERE U.IsActive=1
		   AND IsAutomated=1
		   AND (   U.EntryDateTime=CONVERT(DATE,GETDATE()-4) 
											 OR U.EntryDateTime=CONVERT(DATE,GETDATE())
											 OR U.EntryDateTime=CONVERT(DATE,GETDATE()-2)
											 OR U.EntryDateTime = CONVERT(DATE,GETDATE()-9)
											 OR U.EntryDateTime = CONVERT(DATE,GETDATE()-14)
											 OR U.EntryDateTime = CONVERT(DATE,GETDATE()-19)
											) 
				 GROUP BY U.CustomerId
				 HAVING COUNT(*)>1
          )
          AND U.IsAutomated=1
          AND U.IsActive=1
   )
 UPDATE A SET A.IsActive=0
  FROM UCAlert.UserCarAlerts AS A  WITH(NOLOCK)
  JOIN CTE ON CTE.UsedCarAlert_Id=A.UsedCarAlert_Id
  AND RowNum>1
  AND IsAutomated=1;
 

END 
