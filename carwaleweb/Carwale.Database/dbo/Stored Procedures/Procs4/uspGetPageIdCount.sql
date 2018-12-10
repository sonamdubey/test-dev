IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[uspGetPageIdCount]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[uspGetPageIdCount]
GO

	 
 -- =============================================
-- Author:		<Kundan Dombale>
-- Create date: <20-11-2015>
-- Description:	<Alert for PQPageId and Its Count ,if Traffic less than benchmark >
-- Modified by kundan - commented cte part and used temp table  on 13/01/16 @5:24PM
--					  --  added hourpart column PQPageIdPricequoteCount and is used to calculate pqpageidcount on every 4 hour basis from 7 to 23
-- =============================================
 CREATE PROCEDURE [dbo].[uspGetPageIdCount] 
  AS
  BEGIN 
		-- SET NOCOUNT ON added to prevent extra result sets from
		-- interfering with SELECT statements.
		SET NOCOUNT ON;
		DECLARE @StartTime As DATETIME,
				@ENDTIME   As DATETIME
		SET @StartTime = dateADD(HH,-4, getdate())   -- to get data from past 4 hour
	    SET @ENDTIME=  getdate()                     -- to current time
		
		--PRINT @startTime 
		--PRINT @endtime
		     CREATE TABLE #temp(
								ID int IDENTITY(1,1) ,
								PQPageId int,
								PageDescription varchar(100),
								pqPageId4HourCount int,
								[Expected pqPageId4HourCount] int,
								[Deviation in %] numeric(10,2) 
						 )
			 --;WITH CTE AS (	
				--					SELECT PQIM.PQPageID
				--							,PQIM.PageURL as PageDescription
				--							,COUNT(NCPI.PQPageID)AS pqPageId4HourCount
				--					FROM dbo.NewCarPurchaseInquiries AS NCPI WITH (NOLOCK) 
				--					RIGHT JOIN  dbo.PQPageIds_MasterTblFrom3rdNov2015 AS PQIM  WITH (NOLOCK) ON PQIM.PQPageID = NCPI.PQPageId
				--		            WHERE NCPI.RequestDateTIme   between @Starttime and @Endtime
				--					  AND NCPI.PQPageId IS NOT NULL                                                                   
				--					GROUP BY PQIM.PQPageId,PQIM.PageURL 
	   -- 						)


									SELECT PQIM.PQPageID
										  ,PQIM.PageURL as PageDescription
										  ,COUNT(NCPI.PQPageID)AS pqPageId4HourCount 
										  INTO #CTE
									FROM dbo.NewCarPurchaseInquiries AS NCPI WITH (NOLOCK) 
									RIGHT JOIN  dbo.PQPageIds_MasterTblFrom3rdNov2015 AS PQIM  WITH (NOLOCK) ON PQIM.PQPageID = NCPI.PQPageId
						            WHERE NCPI.RequestDateTIme   between @Starttime and @Endtime
									  AND NCPI.PQPageId IS NOT NULL                                                                   
									GROUP BY PQIM.PQPageId,PQIM.PageURL 


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
							FROM #CTE CT WITH (NOLOCK) 
							INNER JOIN dbo.PQPageIdPricequoteCount PQP WITH (NOLOCK)  ON PQP.pqPageId = CT.PQPageId
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
				
									UNION ALL
									SELECT 0 As PQPageId  ,
											'TOTAL Leads IN 4hours',
											SUM(pqPageId4HourCount) ASpqPageId4HourCount,
											SUM([Expected pqPageId4HourCount])[Expected pqPageId4HourCount],
											((SUM(CAST ( [Expected pqPageId4HourCount]  as float))  -  SUM(CAST(pqPageId4HourCount as float))) 
												        / ISNULL(nullif(sum ([Expected pqPageId4HourCount]),0),1))*100 AS  [Deviation in %] 
											FROM  #temp WITH(NOLOCK)
										) T
										ORDER BY 
										CASE WHEN  T.PageDescription ='TOTAL Leads IN 4hours' THEN 'PageDescription' END  DESC,
													T.[Deviation in %]  desc
				DROP TABLE #temp
				DROP TABLE #CTE

END