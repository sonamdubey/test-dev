IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[uspInsuranceAlert]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[uspInsuranceAlert]
GO

	-- =============================================  
--  Author:  Kundan
-- Create date:   01-12-2015
-- Description:  Display ClientName and Count when it goes below Benchmark 
-- =============================================   
 
CREATE PROCEDURE  [dbo].[uspInsuranceAlert]
	 
AS
 
  
 BEGIN
		DECLARE @ClientID int =3,
				@StartTime As DATETIME,
				@ENDTIME   As DATETIME
			
		SET @StartTime = DATEADD( MINUTE,- DATEPART(MINUTE, GETDATE())-238, GETDATE())
			
		SET @ENDTIME= DATEADD( MINUTE,- DATEPART(MINUTE, GETDATE())-3, GETDATE())
		
		;WITH CTE AS	(
								SELECT PL.ClientId,
									   COUNT(DISTINCT PL.Mobile) AS [Count]
								FROM carwale_com.dbo.INS_PremiumLeads PL WITH(NOLOCK)				
								WHERE PL.RequestDateTime BETWEEN @Starttime AND @Endtime AND 
									  PL.ClientId IN (4,5)   
									  AND LeadSource<>2       
								GROUP BY PL.ClientId                 
							
							UNION ALL 

								SELECT @ClientID,
									   COUNT(DISTINCT BL.CustMobile) AS [Count] 
								FROM carwale_com.[dbo].[BhartiAxa_Leads] BL WITH(NOLOCK)				
								WHERE BL.RequestDateTime BETWEEN @Starttime AND @Endtime    
							
							 
					   )

		SELECT   CT.ClientId
				,LCT.ClientName
				,LCT.[Hour]
				,CT.[Count]
				,LCT.LeadCount As ExpectedCount
				,ROUND(((CAST(LCT.LeadCount AS float)- CAST(CT.COUNT As FLOAT))
								                                /CAST(LCT.LeadCount AS float))*100,2)AS [DeviationIn %] 
		INTO #temp
		FROM CTE AS CT WITH(NOLOCK)
		JOIN INS_LEADCOUNTTARGET AS LCT WITH(NOLOCK) ON CT.ClientId =LCT.ClientId 
		WHERE LCT.[Hour]=DATEPART(HOUR,GETDATE())
		--	AND LCT.LeadCount >CT.[Count] 

		SELECT ClientId
			  ,ClientName
			  ,[Count]
			  ,ExpectedCount
			  ,[DeviationIn %]
	   FROM   (
							 SELECT  ClientId
									,ClientName
									,[Count]
									,ExpectedCount
									,[DeviationIn %]  FROM #temp

							 UNION 
							 SELECT  0 AS ClientId 
									,'TotalCount' 
									,SUM([Count])as [Count]
									,SUM(ExpectedCount) as ExpectedCount
									,ROUND(((SUM(CAST(ExpectedCount AS FLOAT))-SUM(CAST([Count] AS FLOAT )))
																			/SUM(ExpectedCount))*100,2) AS [DeviationIn %]
									FROM #temp
						)T
		 ORDER BY 
		 CASE WHEN  T.ClientName ='TotalCount' THEN 'ClientName' END DESC ,
					T.[DeviationIn %]  desc 
		DROP table #temp  
END
				