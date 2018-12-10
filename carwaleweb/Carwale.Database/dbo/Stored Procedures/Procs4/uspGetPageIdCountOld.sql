IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[uspGetPageIdCountOld]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[uspGetPageIdCountOld]
GO

	 
 -- =============================================
-- Author:		<Kundan Dombale>
-- Create date: <20-11-2015>
-- Description:	<Alert for PQPageId and Its Count ,if Traffic less than benchmark >
 
-- =============================================
 CREATE PROCEDURE [dbo].[uspGetPageIdCountOld] 
  AS
 BEGIN 
	 CREATE TABLE #temp(
						ID int IDENTITY(1,1) ,
						PQPageId int,
						PageDescription varchar(100),
						pqPageId4HourCount int,
						[Expected pqPageId4HourCount] int,
						[Deviation in %] numeric(10,2) 
				  ) 
				   
DECLARE @StartTime As DATETIME,
		@ENDTIME   As DATETIME
			
SET @StartTime = dateADD( minute,- datepart(minute, getdate())-238, getdate())
	
SET @ENDTIME= dateADD( minute,- datepart(minute, getdate())-3, getdate())

SET NOCOUNT ON;

		WITH CTE AS (
						SELECT PQIM.PQPageID
							   ,PQIM.PageURL as PageDescription
							   ,COUNT(NCPI.PQPageID)AS pqPageIdHourCount
									
						FROM dbo.NewCarPurchaseInquiries AS NCPI WITH (NOLOCK) 
						RIGHT JOIN  dbo.PQPageIds_MasterTblFrom3rdNov2015 AS PQIM  WITH (NOLOCK)
						ON PQIM.PQPageID = NCPI.PQPageId
						WHERE NCPI.RequestDateTIme between @Starttime and @Endtime                                                                   
						GROUP BY PQIM.PQPageId,PQIM.PageURL 
	    			)

				   INSERT INTO #temp (
										PQPageId ,
										PageDescription,
										pqPageId4HourCount,
										[Expected pqPageId4HourCount],
										[Deviation in %]   
									)

									SELECT  CT.PQPageId, 
											CT.PageDescription,
											CT.pqPageIdHourCount,
											PQP.pqPageId4HourCount ,
											ROUND(((CAST(PQP.pqPageId4HourCount As FLOAT)-Cast(CT.pqPageIdHourCount AS FLOAT) )
										                                                   /PQP.pqPageId4HourCount)*100,2) AS [Deviation in %]
											FROM CTE CT WITH (NOLOCK) 
											INNER JOIN dbo.PQPageIdPricequoteCount PQP WITH (NOLOCK)  ON PQP.pqPageId = CT.PQPageId
			
									SELECT * FROM (	SELECT  PQPageId ,
												PageDescription,
												pqPageId4HourCount  ,
												[Expected pqPageId4HourCount]  ,
												[Deviation in %] 
										FROM #temp 
										UNION  
									   SELECT 0 As PQPageId  ,
												'TOTAL Leads IN 4hours',
												SUM(pqPageId4HourCount) ASpqPageId4HourCount,
												SUM([Expected pqPageId4HourCount])[Expected pqPageId4HourCount],
						
												((SUM(CAST ([Expected pqPageId4HourCount]AS FLOAT))-SUM(CAST(pqPageId4HourCount AS FLOAT)))/SUM([Expected pqPageId4HourCount]))*100 AS  [Deviation in %] 
										FROM  #temp
									--	ORDER BY [Deviation in %] desc,PQPageId asc
									) T
									
									
									ORDER BY 
										 CASE WHEN  T.PageDescription ='TOTAL Leads IN 4hours' THEN 'PageDescription' END  DESC,
										
									  T.[Deviation in %]  desc
	END
