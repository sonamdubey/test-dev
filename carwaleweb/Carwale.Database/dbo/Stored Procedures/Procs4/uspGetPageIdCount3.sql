IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[uspGetPageIdCount3]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[uspGetPageIdCount3]
GO

	 
 -- =============================================
-- Author:		<Kundan Dombale>
-- Create date: <20-11-2015>
-- Description:	<Alert for PQPageId and Its Count ,if Traffic less than benchmark >
 
-- =============================================
 CREATE PROCEDURE [dbo].[uspGetPageIdCount3] 
  AS
  BEGIN 
		-- SET NOCOUNT ON added to prevent extra result sets from
		-- interfering with SELECT statements.
		SET NOCOUNT ON;
		DECLARE @StartTime As DATETIME,
				@ENDTIME   As DATETIME
		SET @StartTime = dateADD( minute,- datepart(minute, getdate())-238, getdate())
	
		SET @ENDTIME= dateADD( minute,- datepart(minute, getdate())-3, getdate())
		
		CREATE TABLE #temp(
								ID int IDENTITY(1,1) ,
								PQPageId int,
								PageDescription varchar(100),
								pqPageId4HourCount int,
								[Expected pqPageId4HourCount] int,
								[Deviation in %] numeric(10,2) 
						 )
			 
		---- On Every sunday @11am the table pqPageId4HourCount is truncated and the new count of pqpageid (past 30 days) is inserted into it from NewCarPurchaseInquiries
		IF  (DATENAME(WEEKDAY, getdate())='Friday'  --AND  datePart(hour,getdate()) = 11 ) 
		)
		BEGIN
		
				TRUNCATE TABLE PQPageIdPricequoteCount2;

				 INSERT INTO PQPageIdPricequoteCount2 (pqPageId,HourPart,pqPageId4HourCount)
		         SELECT NCPI.PQPageID,
			           CASE DATEPART(HOUR,NCPI.RequestDateTIme)
									WHEN 7 THEN 11
									WHEN 8 THEN 11
									WHEN 9 THEN 11
									WHEN 10 THEN 11

									WHEN 11 THEN 15
									WHEN 12 THEN 15
									WHEN 13 THEN 15
									WHEN 14 THEN 15  

									WHEN 15 THEN 19
									WHEN 16 THEN 19
									WHEN 17 THEN 19
									WHEN 18 THEN 19
									WHEN 19 THEN 19

									WHEN 20 THEN 23
									WHEN 21 THEN 23
									WHEN 22 THEN 23  
									WHEN 23 THEN 23  
									END HourPart,
								  CAST(COUNT( NCPI.PQPageID) AS FLOAT)/30 AS pqPageIdHourCount
									
	       FROM dbo.NewCarPurchaseInquiries AS NCPI WITH (NOLOCK) 
           WHERE NCPI.RequestDateTIme between GETDATE()-30 and GETDATE()
				AND datepart(hour,NCPI.RequestDateTime) BETWEEN 7 AND 23
		        AND PQPageID IS NOT NULL                                                               
           GROUP BY NCPI.PQPageId,
		   CASE DATEPART(HOUR,NCPI.RequestDateTIme)
												WHEN 7 THEN 11
												WHEN 8 THEN 11
												WHEN 9 THEN 11
												WHEN 10 THEN 11

												WHEN 11 THEN 15
												WHEN 12 THEN 15
												WHEN 13 THEN 15
												WHEN 14 THEN 15  

												WHEN 15 THEN 19
												WHEN 16 THEN 19
												WHEN 17 THEN 19
												WHEN 18 THEN 19
												WHEN 19 THEN 19

												WHEN 20 THEN 23
												WHEN 21 THEN 23
												WHEN 22 THEN 23  
												WHEN 23 THEN 23  
												END
	   END 


	   ;WITH CTE AS (
						SELECT PQIM.PQPageID
							   ,PQIM.PageURL as PageDescription
							   ,COUNT(NCPI.PQPageID)AS pqPageId4HourCount
									
						FROM dbo.NewCarPurchaseInquiries AS NCPI WITH (NOLOCK) 
						RIGHT JOIN  dbo.PQPageIds_MasterTblFrom3rdNov2015 AS PQIM  WITH (NOLOCK)
						ON PQIM.PQPageID = NCPI.PQPageId
						WHERE NCPI.RequestDateTIme between @Starttime and @Endtime
						--AND NCPI.PQPageId IS NOT NULL                                                                   
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
											CT.pqPageId4HourCount,
											PQP.pqPageId4HourCount AS [ExpectedPqPageId4HourCount],
											ROUND(((CAST (PQP.pqPageId4HourCount As FLOAT)-Cast(CT.pqPageId4HourCount AS FLOAT) )
										     / ISNULL(NULLIF(PQP.pqPageId4HourCount,0),1))*100,2) AS [Deviation in %]
											  

									FROM CTE CT WITH (NOLOCK) 
									INNER JOIN dbo.PQPageIdPricequoteCount2 PQP WITH (NOLOCK)  ON PQP.pqPageId = CT.PQPageId
								    WHERE PQP.HourPart=DATEPART(HOUR,GETDATE())
			
		SELECT PQPageId ,
			   PageDescription,
			   pqPageId4HourCount  ,
			   [Expected pqPageId4HourCount]  ,
			   [Deviation in %]
	   FROM (	
				SELECT  PQPageId ,
						PageDescription,
						pqPageId4HourCount ,
						[Expected pqPageId4HourCount]  ,
						[Deviation in %]
				FROM #temp 
				
				UNION  
				SELECT 0 As PQPageId  ,
					   'TOTAL Leads IN 4hours',
					   SUM(pqPageId4HourCount) ASpqPageId4HourCount,
					   SUM([Expected pqPageId4HourCount])[Expected pqPageId4HourCount],
					   --((SUM(CAST (ISNULL([Expected pqPageId4HourCount],0)AS FLOAT))-SUM(CAST(ISNULL(pqPageId4HourCount,0) AS FLOAT)))
					   --                        /SUM(ISNULL([Expected pqPageId4HourCount],1)))*100 AS  [Deviation in %] 
					    ((SUM(CAST ( [Expected pqPageId4HourCount]  as float))  
								-  SUM(CAST(pqPageId4HourCount as float))) /
								ISNULL(nullif(sum ([Expected pqPageId4HourCount]),0),1))*100 AS  [Deviation in %] 
								 --sum ([Expected pqPageId4HourCount]) )*100 AS  [Deviation in %]  


								FROM  #temp WITH(NOLOCK)
				 
				
			) T
			ORDER BY 
					CASE WHEN  T.PageDescription ='TOTAL Leads IN 4hours' THEN 'PageDescription' END  DESC,
								
					T.[Deviation in %]  desc
			DROP TABLE #temp
END
