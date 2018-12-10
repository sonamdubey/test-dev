IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[DCRM_ServiceDashboardData]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[DCRM_ServiceDashboardData]
GO

	--USE [CarWale]
--GO
--/****** Object:  StoredProcedure [dbo].[DCRM_DealerStockResponseAnalysis_ServiceDashboard]    Script Date: 08/31/2012 10:34:27 ******/
--SET ANSI_NULLS ON
--GO
--SET QUOTED_IDENTIFIER ON
--GO
---- =============================================
---- Author:		Chetan Kane
---- Create date: 
---- Description:	
---- =============================================

CREATE PROCEDURE [dbo].[DCRM_ServiceDashboardData]  
(  
	@DealerId INT,
	@From DATETIME,
	@To DATETIME,
	@SelectCode INT = 1
)  
AS   
 BEGIN 
		IF(@SelectCode = 1)
		BEGIN	 
			WITH TAB AS
			(
			SELECT DISTINCT DATEDIFF(WEEK ,@From,Entrydate) AS WeekNo,
					DSRA.Entrydate AS EntDate,
					ISNULL(AVG(CWStockCount)OVER(Partition BY DATEDIFF(WEEK ,@From,Entrydate)),0) AS TotalStockPerWeek,
					ISNULL(SUM(Response)OVER(Partition BY DATEDIFF(WEEK ,@From,Entrydate)),0) AS TotalResponsePerWeek,
					WeeklyResponsePerCar = ISNULL(CASE WHEN SUM(Response)OVER(Partition BY DATEDIFF(WEEK ,@From,Entrydate))=0 THEN 0 ELSE
					ROUND( SUM(Response)OVER(Partition BY DATEDIFF(WEEK ,@From,Entrydate))
					/(CONVERT(FLOAT,(AVG(CWStockCount)OVER(Partition BY DATEDIFF(WEEK ,@From,Entrydate))))),1)END,0),
					D.CityId AS CityId
			FROM DealerStockResponseAnalysis DSRA 
				 INNER JOIN Dealers D ON D.ID=DSRA.DealerId
			WHERE DealerId = @DealerId
				AND Entrydate BETWEEN @From AND @To)
				
			SELECT DISTINCT TAB.WeekNo,TAB.TotalStockPerWeek,TAB.TotalResponsePerWeek,TAB.WeeklyResponsePerCar,
			CONVERT(CHAR(12),MIN(EntDate)OVER(Partition BY DATEDIFF(WEEK ,@From,Entrydate)),106) + ' TO ' + CONVERT(CHAR(12),MAX(EntDate)OVER(Partition BY DATEDIFF(WEEK ,@From,Entrydate)),106) as DateRange,
			AVGResponseincity = (CASE WHEN SUM(Response)OVER(Partition BY DATEDIFF(WEEK ,@From,Entrydate))=0 THEN 0 ELSE
			ROUND( SUM(Response)OVER(Partition BY DATEDIFF(WEEK ,@From,Entrydate))
										/(CONVERT(FLOAT,(SUM(CWStockCount)OVER(Partition BY DATEDIFF(WEEK ,@From,Entrydate))))),1)END) 
										* DATEDIFF(DAY , MIN(EntDate)OVER(Partition BY DATEDIFF(WEEK ,@From,Entrydate)),MAX(DATEADD(dd,1,EntDate))OVER(Partition BY DATEDIFF(WEEK ,@From,Entrydate)))
			FROM  DealerStockResponseAnalysis  DSRA  
				  INNER JOIN Dealers D ON D.ID=DSRA.DealerId
				  INNER JOIN TAB ON TAB.CityId=D.CityId AND DATEDIFF(WEEK ,@From,Entrydate)=TAB.WeekNo
		END
		
		IF(@SelectCode = 2)
		BEGIN	 
			WITH TAB AS
			(SELECT DISTINCT DATEDIFF(WEEK ,@From,Entrydate) AS WeekNo,
					DSRA.Entrydate AS EntDate,
					ISNULL(AVG(PhotoCount)OVER(Partition BY DATEDIFF(WEEK ,@From,Entrydate)),0) AS TotalStockPerWeek,
					ISNULL(SUM(ResponsePhotoCount)OVER(Partition BY DATEDIFF(WEEK ,@From,Entrydate)),0) AS TotalResponsePerWeek,
					WeeklyResponsePerCar = ISNULL(CASE WHEN AVG(ResponsePhotoCount)OVER(Partition BY DATEDIFF(WEEK ,@From,Entrydate))=0 THEN 0 ELSE
					ROUND( AVG(ResponsePhotoCount)OVER(Partition BY DATEDIFF(WEEK ,@From,Entrydate))
					/(CONVERT(FLOAT,(AVG(PhotoCount)OVER(Partition BY DATEDIFF(WEEK ,@From,Entrydate))))),1)END,0),
					D.CityId AS CityId
			FROM DealerStockResponseAnalysis DSRA 
				 INNER JOIN Dealers D ON D.ID=DSRA.DealerId
			WHERE DealerId = @DealerId
				AND Entrydate BETWEEN @From AND @To)
				
			SELECT DISTINCT TAB.WeekNo,TAB.TotalStockPerWeek,TAB.TotalResponsePerWeek,TAB.WeeklyResponsePerCar,
			CONVERT(CHAR(12),MIN(EntDate)OVER(Partition BY DATEDIFF(WEEK ,@From,Entrydate)),106) + ' TO ' + CONVERT(CHAR(12),MAX(EntDate)OVER(Partition BY DATEDIFF(WEEK ,@From,Entrydate)),106) as DateRange,
			AVGResponseincity = CASE WHEN AVG(ResponsePhotoCount)OVER(Partition BY DATEDIFF(WEEK ,@From,Entrydate))=0 THEN 0 ELSE
			ROUND( AVG(ResponsePhotoCount)OVER(Partition BY DATEDIFF(WEEK ,@From,Entrydate))
										/(CONVERT(FLOAT,(AVG(PhotoCount)OVER(Partition BY DATEDIFF(WEEK ,@From,Entrydate))))),1)END
			FROM  DealerStockResponseAnalysis  DSRA  
				  INNER JOIN Dealers D ON D.ID=DSRA.DealerId
				  INNER JOIN TAB ON TAB.CityId=D.CityId AND DATEDIFF(WEEK ,@From,Entrydate)=TAB.WeekNo
		END
		
		IF(@SelectCode = 3)
		BEGIN	 
			WITH TAB AS
			(SELECT DISTINCT DATEDIFF(WEEK ,@From,Entrydate) AS WeekNo,
					DSRA.Entrydate AS EntDate,
					ISNULL(AVG(NoPhotoCount)OVER(Partition BY DATEDIFF(WEEK ,@From,Entrydate)),0) AS TotalStockPerWeek,
					ISNULL(SUM(ResponseNoPhotoCount)OVER(Partition BY DATEDIFF(WEEK ,@From,Entrydate)),0) AS TotalResponsePerWeek,
					WeeklyResponsePerCar = ISNULL(CASE WHEN AVG(ResponseNoPhotoCount)OVER(Partition BY DATEDIFF(WEEK ,@From,Entrydate))=0 THEN 0 ELSE
					ROUND( AVG(ResponseNoPhotoCount)OVER(Partition BY DATEDIFF(WEEK ,@From,Entrydate))
					/(CONVERT(FLOAT,(AVG(NoPhotoCount)OVER(Partition BY DATEDIFF(WEEK ,@From,Entrydate))))),1)END,0),
					D.CityId AS CityId
			FROM DealerStockResponseAnalysis DSRA 
				 INNER JOIN Dealers D ON D.ID=DSRA.DealerId
			WHERE DealerId = @DealerId
				AND Entrydate BETWEEN @From AND @To)
				
			SELECT DISTINCT TAB.WeekNo,TAB.TotalStockPerWeek,TAB.TotalResponsePerWeek,TAB.WeeklyResponsePerCar,
			CONVERT(CHAR(12),MIN(EntDate)OVER(Partition BY DATEDIFF(WEEK ,@From,Entrydate)),106) + ' TO ' + CONVERT(CHAR(12),MAX(EntDate)OVER(Partition BY DATEDIFF(WEEK ,@From,Entrydate)),106) as DateRange,
			AVGResponseincity = CASE WHEN AVG(ResponseNoPhotoCount)OVER(Partition BY DATEDIFF(WEEK ,@From,Entrydate))=0 THEN 0 ELSE
			ROUND( AVG(ResponseNoPhotoCount)OVER(Partition BY DATEDIFF(WEEK ,@From,Entrydate))
										/(CONVERT(FLOAT,(AVG(NoPhotoCount)OVER(Partition BY DATEDIFF(WEEK ,@From,Entrydate))))),1)END
			FROM  DealerStockResponseAnalysis  DSRA  
				  INNER JOIN Dealers D ON D.ID=DSRA.DealerId
				  INNER JOIN TAB ON TAB.CityId=D.CityId AND DATEDIFF(WEEK ,@From,Entrydate)=TAB.WeekNo
		END
		
		IF(@SelectCode = 4)
		BEGIN	 
			WITH TAB AS
			(SELECT DISTINCT DATEDIFF(WEEK ,@From,Entrydate) AS WeekNo,
					DSRA.Entrydate AS EntDate,
					ISNULL(AVG(DescrCount)OVER(Partition BY DATEDIFF(WEEK ,@From,Entrydate)),0) AS TotalStockPerWeek,
					ISNULL(SUM(ResponseDescrCount)OVER(Partition BY DATEDIFF(WEEK ,@From,Entrydate)),0) AS TotalResponsePerWeek,
					WeeklyResponsePerCar = ISNULL(CASE WHEN AVG(ResponseDescrCount)OVER(Partition BY DATEDIFF(WEEK ,@From,Entrydate))=0 THEN 0 ELSE
					ROUND( AVG(ResponseDescrCount)OVER(Partition BY DATEDIFF(WEEK ,@From,Entrydate))
					/(CONVERT(FLOAT,(AVG(DescrCount)OVER(Partition BY DATEDIFF(WEEK ,@From,Entrydate))))),1)END,0),
					D.CityId AS CityId
			FROM DealerStockResponseAnalysis DSRA 
				 INNER JOIN Dealers D ON D.ID=DSRA.DealerId
			WHERE DealerId = @DealerId
				AND Entrydate BETWEEN @From AND @To)
				
			SELECT DISTINCT TAB.WeekNo,TAB.TotalStockPerWeek,TAB.TotalResponsePerWeek,TAB.WeeklyResponsePerCar,
			CONVERT(CHAR(12),MIN(EntDate)OVER(Partition BY DATEDIFF(WEEK ,@From,Entrydate)),106) + ' TO ' + CONVERT(CHAR(12),MAX(EntDate)OVER(Partition BY DATEDIFF(WEEK ,@From,Entrydate)),106) as DateRange,
			AVGResponseincity = CASE WHEN AVG(ResponseDescrCount)OVER(Partition BY DATEDIFF(WEEK ,@From,Entrydate))=0 THEN 0 ELSE
			ROUND( AVG(ResponseDescrCount)OVER(Partition BY DATEDIFF(WEEK ,@From,Entrydate))
										/(CONVERT(FLOAT,(AVG(DescrCount)OVER(Partition BY DATEDIFF(WEEK ,@From,Entrydate))))),1)END
			FROM  DealerStockResponseAnalysis  DSRA  
				  INNER JOIN Dealers D ON D.ID=DSRA.DealerId
				  INNER JOIN TAB ON TAB.CityId=D.CityId AND DATEDIFF(WEEK ,@From,Entrydate)=TAB.WeekNo
		END
		
		
		IF(@SelectCode = 5)
		BEGIN	 
			WITH TAB AS
			(SELECT DISTINCT DATEDIFF(WEEK ,@From,Entrydate) AS WeekNo,
					DSRA.Entrydate AS EntDate,
					ISNULL(AVG(NoDescrCount)OVER(Partition BY DATEDIFF(WEEK ,@From,Entrydate)),0) AS TotalStockPerWeek,
					ISNULL(SUM(ResponseNoDescrCount)OVER(Partition BY DATEDIFF(WEEK ,@From,Entrydate)),0) AS TotalResponsePerWeek,
					WeeklyResponsePerCar = ISNULL(CASE WHEN AVG(ResponseNoDescrCount)OVER(Partition BY DATEDIFF(WEEK ,@From,Entrydate))=0 THEN 0 ELSE
					ROUND( AVG(ResponseNoDescrCount)OVER(Partition BY DATEDIFF(WEEK ,@From,Entrydate))
					/(CONVERT(FLOAT,(AVG(NoDescrCount)OVER(Partition BY DATEDIFF(WEEK ,@From,Entrydate))))),1)END,0),
					D.CityId AS CityId
			FROM DealerStockResponseAnalysis DSRA 
				 INNER JOIN Dealers D ON D.ID=DSRA.DealerId
			WHERE DealerId = @DealerId
				AND Entrydate BETWEEN @From AND @To)
				
			SELECT DISTINCT TAB.WeekNo,TAB.TotalStockPerWeek,TAB.TotalResponsePerWeek,TAB.WeeklyResponsePerCar,
			CONVERT(CHAR(12),MIN(EntDate)OVER(Partition BY DATEDIFF(WEEK ,@From,Entrydate)),106) + ' TO ' + CONVERT(CHAR(12),MAX(EntDate)OVER(Partition BY DATEDIFF(WEEK ,@From,Entrydate)),106) as DateRange,
			AVGResponseincity = CASE WHEN AVG(ResponseNoDescrCount)OVER(Partition BY DATEDIFF(WEEK ,@From,Entrydate))=0 THEN 0 ELSE
			ROUND( AVG(Response)OVER(Partition BY DATEDIFF(WEEK ,@From,Entrydate))
										/(CONVERT(FLOAT,(AVG(NoDescrCount)OVER(Partition BY DATEDIFF(WEEK ,@From,Entrydate))))),1)END
			FROM  DealerStockResponseAnalysis  DSRA  
				  INNER JOIN Dealers D ON D.ID=DSRA.DealerId
				  INNER JOIN TAB ON TAB.CityId=D.CityId AND DATEDIFF(WEEK ,@From,Entrydate)=TAB.WeekNo
		END
		
		
		--IF(@SelectCode = 6)
		--BEGIN	 
		--	WITH TAB AS
		--	(SELECT DISTINCT DATEDIFF(WEEK ,@From,Entrydate) AS WeekNo,
		--			DSRA.Entrydate AS EntDate,
		--			SUM(OverAgeCount)OVER(Partition BY DATEDIFF(WEEK ,@From,Entrydate)) AS TotalStockPerWeek,
		--			SUM(ResponseOverAgeCount)OVER(Partition BY DATEDIFF(WEEK ,@From,Entrydate)) AS TotalResponsePerWeek,
		--			WeeklyResponsePerCar = CASE WHEN AVG(ResponseOverAgeCount)OVER(Partition BY DATEDIFF(WEEK ,@From,Entrydate))=0 THEN 0 ELSE
		--			ROUND( AVG(ResponseOverAgeCount)OVER(Partition BY DATEDIFF(WEEK ,@From,Entrydate))
		--			/(CONVERT(FLOAT,(AVG(OverAgeCount)OVER(Partition BY DATEDIFF(WEEK ,@From,Entrydate))))),1)END,
		--			D.CityId AS CityId
		--	FROM DealerStockResponseAnalysis DSRA 
		--		 INNER JOIN Dealers D ON D.ID=DSRA.DealerId
		--	WHERE DealerId = @DealerId
		--		AND Entrydate BETWEEN @From AND @To)
				
		--	SELECT DISTINCT TAB.WeekNo,TAB.TotalStockPerWeek,TAB.TotalResponsePerWeek,TAB.WeeklyResponsePerCar,
		--	CONVERT(CHAR(12),MIN(EntDate)OVER(Partition BY DATEDIFF(WEEK ,@From,Entrydate)),106) + ' TO ' + CONVERT(CHAR(12),MAX(EntDate)OVER(Partition BY DATEDIFF(WEEK ,@From,Entrydate)),106) as DateRange,
		--	AVGResponseincity = CASE WHEN AVG(ResponseOverAgeCount)OVER(Partition BY DATEDIFF(WEEK ,@From,Entrydate))=0 THEN 0 ELSE
		--	ROUND( AVG(ResponseOverAgeCount)OVER(Partition BY DATEDIFF(WEEK ,@From,Entrydate))
		--								/(CONVERT(FLOAT,(AVG(OverAgeCount)OVER(Partition BY DATEDIFF(WEEK ,@From,Entrydate))))),1)END
		--	FROM  DealerStockResponseAnalysis  DSRA  
		--		  INNER JOIN Dealers D ON D.ID=DSRA.DealerId
		--		  INNER JOIN TAB ON TAB.CityId=D.CityId AND DATEDIFF(WEEK ,@From,Entrydate)=TAB.WeekNo
		--END
		
		--IF(@SelectCode = 7)
		--BEGIN	 
		--	WITH TAB AS
		--	(SELECT DISTINCT DATEDIFF(WEEK ,@From,Entrydate) AS WeekNo,
		--			DSRA.Entrydate AS EntDate,
		--			SUM(OverAgeNoCount)OVER(Partition BY DATEDIFF(WEEK ,@From,Entrydate)) AS TotalStockPerWeek,
		--			SUM(ResponseNoOverAgeCount)OVER(Partition BY DATEDIFF(WEEK ,@From,Entrydate)) AS TotalResponsePerWeek,
		--			WeeklyResponsePerCar = CASE WHEN AVG(ResponseNoOverAgeCount)OVER(Partition BY DATEDIFF(WEEK ,@From,Entrydate))=0 THEN 0 ELSE
		--			ROUND( AVG(ResponseNoOverAgeCount)OVER(Partition BY DATEDIFF(WEEK ,@From,Entrydate))
		--			/(CONVERT(FLOAT,(AVG(OverAgeNoCount)OVER(Partition BY DATEDIFF(WEEK ,@From,Entrydate))))),1)END,
		--			D.CityId AS CityId
		--	FROM DealerStockResponseAnalysis DSRA 
		--		 INNER JOIN Dealers D ON D.ID=DSRA.DealerId
		--	WHERE DealerId = @DealerId
		--		AND Entrydate BETWEEN @From AND @To)
				
		--	SELECT DISTINCT TAB.WeekNo,TAB.TotalStockPerWeek,TAB.TotalResponsePerWeek,TAB.WeeklyResponsePerCar,
		--	CONVERT(CHAR(12),MIN(EntDate)OVER(Partition BY DATEDIFF(WEEK ,@From,Entrydate)),106) + ' TO ' + CONVERT(CHAR(12),MAX(EntDate)OVER(Partition BY DATEDIFF(WEEK ,@From,Entrydate)),106) as DateRange,
		--	AVGResponseincity = CASE WHEN AVG(ResponseNoOverAgeCount)OVER(Partition BY DATEDIFF(WEEK ,@From,Entrydate))=0 THEN 0 ELSE
		--	ROUND( AVG(ResponseNoOverAgeCount)OVER(Partition BY DATEDIFF(WEEK ,@From,Entrydate))
		--								/(CONVERT(FLOAT,(AVG(OverAgeNoCount)OVER(Partition BY DATEDIFF(WEEK ,@From,Entrydate))))),1)END
		--	FROM  DealerStockResponseAnalysis  DSRA  
		--		  INNER JOIN Dealers D ON D.ID=DSRA.DealerId
		--		  INNER JOIN TAB ON TAB.CityId=D.CityId AND DATEDIFF(WEEK ,@From,Entrydate)=TAB.WeekNo
		--END
		
		
		--IF(@SelectCode = 8)
		--BEGIN	 
		--	WITH TAB AS
		--	(SELECT DISTINCT DATEDIFF(WEEK ,@From,Entrydate) AS WeekNo,
		--			DSRA.Entrydate AS EntDate,
		--			SUM(OverKMCount)OVER(Partition BY DATEDIFF(WEEK ,@From,Entrydate)) AS TotalStockPerWeek,
		--			SUM(ResponseOverKMCount)OVER(Partition BY DATEDIFF(WEEK ,@From,Entrydate)) AS TotalResponsePerWeek,
		--			WeeklyResponsePerCar = CASE WHEN AVG(ResponseOverKMCount)OVER(Partition BY DATEDIFF(WEEK ,@From,Entrydate))=0 THEN 0 ELSE
		--			ROUND( AVG(ResponseOverKMCount)OVER(Partition BY DATEDIFF(WEEK ,@From,Entrydate))
		--			/(CONVERT(FLOAT,(AVG(OverKMCount)OVER(Partition BY DATEDIFF(WEEK ,@From,Entrydate))))),1)END,
		--			D.CityId AS CityId
		--	FROM DealerStockResponseAnalysis DSRA 
		--		 INNER JOIN Dealers D ON D.ID=DSRA.DealerId
		--	WHERE DealerId = @DealerId
		--		AND Entrydate BETWEEN @From AND @To)
				
		--	SELECT DISTINCT TAB.WeekNo,TAB.TotalStockPerWeek,TAB.TotalResponsePerWeek,TAB.WeeklyResponsePerCar,
		--	CONVERT(CHAR(12),MIN(EntDate)OVER(Partition BY DATEDIFF(WEEK ,@From,Entrydate)),106) + ' TO ' + CONVERT(CHAR(12),MAX(EntDate)OVER(Partition BY DATEDIFF(WEEK ,@From,Entrydate)),106) as DateRange,
		--	AVGResponseincity = CASE WHEN AVG(ResponseOverKMCount)OVER(Partition BY DATEDIFF(WEEK ,@From,Entrydate))=0 THEN 0 ELSE
		--	ROUND( AVG(ResponseOverKMCount)OVER(Partition BY DATEDIFF(WEEK ,@From,Entrydate))
		--								/(CONVERT(FLOAT,(AVG(OverKMCount)OVER(Partition BY DATEDIFF(WEEK ,@From,Entrydate))))),1)END
		--	FROM  DealerStockResponseAnalysis  DSRA  
		--		  INNER JOIN Dealers D ON D.ID=DSRA.DealerId
		--		  INNER JOIN TAB ON TAB.CityId=D.CityId AND DATEDIFF(WEEK ,@From,Entrydate)=TAB.WeekNo
		--END
		
		
		--IF(@SelectCode = 9)
		--BEGIN	 
		--	WITH TAB AS
		--	(SELECT DISTINCT DATEDIFF(WEEK ,@From,Entrydate) AS WeekNo,
		--			DSRA.Entrydate AS EntDate,
		--			SUM(OverNoKMCount)OVER(Partition BY DATEDIFF(WEEK ,@From,Entrydate)) AS TotalStockPerWeek,
		--			SUM(ResponseNoOverKMCount)OVER(Partition BY DATEDIFF(WEEK ,@From,Entrydate)) AS TotalResponsePerWeek,
		--			WeeklyResponsePerCar = CASE WHEN AVG(ResponseNoOverKMCount)OVER(Partition BY DATEDIFF(WEEK ,@From,Entrydate))=0 THEN 0 ELSE
		--			ROUND( AVG(ResponseNoOverKMCount)OVER(Partition BY DATEDIFF(WEEK ,@From,Entrydate))
		--			/(CONVERT(FLOAT,(AVG(OverNoKMCount)OVER(Partition BY DATEDIFF(WEEK ,@From,Entrydate))))),1)END,
		--			D.CityId AS CityId
		--	FROM DealerStockResponseAnalysis DSRA 
		--		 INNER JOIN Dealers D ON D.ID=DSRA.DealerId
		--	WHERE DealerId = @DealerId
		--		AND Entrydate BETWEEN @From AND @To)
				
		--	SELECT DISTINCT TAB.WeekNo,TAB.TotalStockPerWeek,TAB.TotalResponsePerWeek,TAB.WeeklyResponsePerCar,
		--	CONVERT(CHAR(12),MIN(EntDate)OVER(Partition BY DATEDIFF(WEEK ,@From,Entrydate)),106) + ' TO ' + CONVERT(CHAR(12),MAX(EntDate)OVER(Partition BY DATEDIFF(WEEK ,@From,Entrydate)),106) as DateRange,
		--	AVGResponseincity = CASE WHEN AVG(ResponseNoOverKMCount)OVER(Partition BY DATEDIFF(WEEK ,@From,Entrydate))=0 THEN 0 ELSE
		--	ROUND( AVG(ResponseNoOverKMCount)OVER(Partition BY DATEDIFF(WEEK ,@From,Entrydate))
		--								/(CONVERT(FLOAT,(AVG(OverNoKMCount)OVER(Partition BY DATEDIFF(WEEK ,@From,Entrydate))))),1)END
		--	FROM  DealerStockResponseAnalysis  DSRA  
		--		  INNER JOIN Dealers D ON D.ID=DSRA.DealerId
		--		  INNER JOIN TAB ON TAB.CityId=D.CityId AND DATEDIFF(WEEK ,@From,Entrydate)=TAB.WeekNo
		--END
		
		
		--IF(@SelectCode = 10)
		--BEGIN	 
		--	WITH TAB AS
		--	(SELECT DISTINCT DATEDIFF(WEEK ,@From,Entrydate) AS WeekNo,
		--			DSRA.Entrydate AS EntDate,
		--			SUM(OverPriceCount)OVER(Partition BY DATEDIFF(WEEK ,@From,Entrydate)) AS TotalStockPerWeek,
		--			SUM(ResponseOverPriceCount)OVER(Partition BY DATEDIFF(WEEK ,@From,Entrydate)) AS TotalResponsePerWeek,
		--			WeeklyResponsePerCar = CASE WHEN AVG(ResponseOverPriceCount)OVER(Partition BY DATEDIFF(WEEK ,@From,Entrydate))=0 THEN 0 ELSE
		--			ROUND( AVG(ResponseOverPriceCount)OVER(Partition BY DATEDIFF(WEEK ,@From,Entrydate))
		--			/(CONVERT(FLOAT,(AVG(OverPriceCount)OVER(Partition BY DATEDIFF(WEEK ,@From,Entrydate))))),1)END,
		--			D.CityId AS CityId
		--	FROM DealerStockResponseAnalysis DSRA 
		--		 INNER JOIN Dealers D ON D.ID=DSRA.DealerId
		--	WHERE DealerId = @DealerId
		--		AND Entrydate BETWEEN @From AND @To)
				
		--	SELECT DISTINCT TAB.WeekNo,TAB.TotalStockPerWeek,TAB.TotalResponsePerWeek,TAB.WeeklyResponsePerCar,
		--	CONVERT(CHAR(12),MIN(EntDate)OVER(Partition BY DATEDIFF(WEEK ,@From,Entrydate)),106) + ' TO ' + CONVERT(CHAR(12),MAX(EntDate)OVER(Partition BY DATEDIFF(WEEK ,@From,Entrydate)),106) as DateRange,
		--	AVGResponseincity = CASE WHEN AVG(ResponseOverPriceCount)OVER(Partition BY DATEDIFF(WEEK ,@From,Entrydate))=0 THEN 0 ELSE
		--	ROUND( AVG(ResponseOverPriceCount)OVER(Partition BY DATEDIFF(WEEK ,@From,Entrydate))
		--								/(CONVERT(FLOAT,(AVG(OverPriceCount)OVER(Partition BY DATEDIFF(WEEK ,@From,Entrydate))))),1)END
		--	FROM  DealerStockResponseAnalysis  DSRA  
		--		  INNER JOIN Dealers D ON D.ID=DSRA.DealerId
		--		  INNER JOIN TAB ON TAB.CityId=D.CityId AND DATEDIFF(WEEK ,@From,Entrydate)=TAB.WeekNo
		--END
		
		
		--IF(@SelectCode = 11)
		--BEGIN	 
		--	WITH TAB AS
		--	(SELECT DISTINCT DATEDIFF(WEEK ,@From,Entrydate) AS WeekNo,
		--			DSRA.Entrydate AS EntDate,
		--			SUM(OverNoPriceCount)OVER(Partition BY DATEDIFF(WEEK ,@From,Entrydate)) AS TotalStockPerWeek,
		--			SUM(ResponseNoOverPriceCount)OVER(Partition BY DATEDIFF(WEEK ,@From,Entrydate)) AS TotalResponsePerWeek,
		--			WeeklyResponsePerCar = CASE WHEN AVG(ResponseNoOverPriceCount)OVER(Partition BY DATEDIFF(WEEK ,@From,Entrydate))=0 THEN 0 ELSE
		--			ROUND( AVG(ResponseNoOverPriceCount)OVER(Partition BY DATEDIFF(WEEK ,@From,Entrydate))
		--			/(CONVERT(FLOAT,(AVG(OverNoPriceCount)OVER(Partition BY DATEDIFF(WEEK ,@From,Entrydate))))),1)END,
		--			D.CityId AS CityId
		--	FROM DealerStockResponseAnalysis DSRA 
		--		 INNER JOIN Dealers D ON D.ID=DSRA.DealerId
		--	WHERE DealerId = @DealerId
		--		AND Entrydate BETWEEN @From AND @To)
				
		--	SELECT DISTINCT TAB.WeekNo,TAB.TotalStockPerWeek,TAB.TotalResponsePerWeek,TAB.WeeklyResponsePerCar,
		--	CONVERT(CHAR(12),MIN(EntDate)OVER(Partition BY DATEDIFF(WEEK ,@From,Entrydate)),106) + ' TO ' + CONVERT(CHAR(12),MAX(EntDate)OVER(Partition BY DATEDIFF(WEEK ,@From,Entrydate)),106) as DateRange,
		--	AVGResponseincity = CASE WHEN AVG(ResponseNoOverPriceCount)OVER(Partition BY DATEDIFF(WEEK ,@From,Entrydate))=0 THEN 0 ELSE
		--	ROUND( AVG(ResponseNoOverPriceCount)OVER(Partition BY DATEDIFF(WEEK ,@From,Entrydate))
		--								/(CONVERT(FLOAT,(AVG(OverNoPriceCount)OVER(Partition BY DATEDIFF(WEEK ,@From,Entrydate))))),1)END
		--	FROM  DealerStockResponseAnalysis  DSRA  
		--		  INNER JOIN Dealers D ON D.ID=DSRA.DealerId
		--		  INNER JOIN TAB ON TAB.CityId=D.CityId AND DATEDIFF(WEEK ,@From,Entrydate)=TAB.WeekNo
		--END
		
		
		--IF(@SelectCode = 12)
		--BEGIN	 
		--	WITH TAB AS
		--	(SELECT DISTINCT DATEDIFF(WEEK ,@From,Entrydate) AS WeekNo,
		--			DSRA.Entrydate AS EntDate,
		--			SUM(StandardStock)OVER(Partition BY DATEDIFF(WEEK ,@From,Entrydate)) AS TotalStockPerWeek,
		--			SUM(ResponseStandardStock)OVER(Partition BY DATEDIFF(WEEK ,@From,Entrydate)) AS TotalResponsePerWeek,
		--			WeeklyResponsePerCar = CASE WHEN AVG(ResponseStandardStock)OVER(Partition BY DATEDIFF(WEEK ,@From,Entrydate))=0 THEN 0 ELSE
		--			ROUND( AVG(ResponseStandardStock)OVER(Partition BY DATEDIFF(WEEK ,@From,Entrydate))
		--			/(CONVERT(FLOAT,(AVG(StandardStock)OVER(Partition BY DATEDIFF(WEEK ,@From,Entrydate))))),1)END,
		--			D.CityId AS CityId
		--	FROM DealerStockResponseAnalysis DSRA 
		--		 INNER JOIN Dealers D ON D.ID=DSRA.DealerId
		--	WHERE DealerId = @DealerId
		--		AND Entrydate BETWEEN @From AND @To)
				
		--	SELECT DISTINCT TAB.WeekNo,TAB.TotalStockPerWeek,TAB.TotalResponsePerWeek,TAB.WeeklyResponsePerCar,
		--	CONVERT(CHAR(12),MIN(EntDate)OVER(Partition BY DATEDIFF(WEEK ,@From,Entrydate)),106) + ' TO ' + CONVERT(CHAR(12),MAX(EntDate)OVER(Partition BY DATEDIFF(WEEK ,@From,Entrydate)),106) as DateRange,
		--	AVGResponseincity =CASE WHEN AVG(ResponseStandardStock)OVER(Partition BY DATEDIFF(WEEK ,@From,Entrydate))=0 THEN 0 ELSE
		--	ROUND( AVG(ResponseStandardStock)OVER(Partition BY DATEDIFF(WEEK ,@From,Entrydate))
		--								/(CONVERT(FLOAT,(AVG(StandardStock)OVER(Partition BY DATEDIFF(WEEK ,@From,Entrydate))))),1)END 
		--	FROM  DealerStockResponseAnalysis  DSRA  
		--		  INNER JOIN Dealers D ON D.ID=DSRA.DealerId
		--		  INNER JOIN TAB ON TAB.CityId=D.CityId AND DATEDIFF(WEEK ,@From,Entrydate)=TAB.WeekNo
		--END
		
		
		--IF(@SelectCode = 13)
		--BEGIN	 
		--	WITH TAB AS
		--	(SELECT DISTINCT DATEDIFF(WEEK ,@From,Entrydate) AS WeekNo,
		--			DSRA.Entrydate AS EntDate,
		--			SUM(NoStandardStock)OVER(Partition BY DATEDIFF(WEEK ,@From,Entrydate)) AS TotalStockPerWeek,
		--			SUM(ResponseNoStandardStock)OVER(Partition BY DATEDIFF(WEEK ,@From,Entrydate)) AS TotalResponsePerWeek,
		--			WeeklyResponsePerCar = CASE WHEN AVG(ResponseNoStandardStock)OVER(Partition BY DATEDIFF(WEEK ,@From,Entrydate))=0 THEN 0 ELSE
		--			ROUND( AVG(ResponseNoStandardStock)OVER(Partition BY DATEDIFF(WEEK ,@From,Entrydate))
		--			/(CONVERT(FLOAT,(AVG(NoStandardStock)OVER(Partition BY DATEDIFF(WEEK ,@From,Entrydate))))),1)END,
		--			D.CityId AS CityId
		--	FROM DealerStockResponseAnalysis DSRA 
		--		 INNER JOIN Dealers D ON D.ID=DSRA.DealerId
		--	WHERE DealerId = @DealerId
		--		AND Entrydate BETWEEN @From AND @To)
				
		--	SELECT DISTINCT TAB.WeekNo,TAB.TotalStockPerWeek,TAB.TotalResponsePerWeek,TAB.WeeklyResponsePerCar,
		--	CONVERT(CHAR(12),MIN(EntDate)OVER(Partition BY DATEDIFF(WEEK ,@From,Entrydate)),106) + ' TO ' + CONVERT(CHAR(12),MAX(EntDate)OVER(Partition BY DATEDIFF(WEEK ,@From,Entrydate)),106) as DateRange,
		--	AVGResponseincity = CASE WHEN AVG(ResponseNoStandardStock)OVER(Partition BY DATEDIFF(WEEK ,@From,Entrydate))=0 THEN 0 ELSE
		--	ROUND( AVG(ResponseNoStandardStock)OVER(Partition BY DATEDIFF(WEEK ,@From,Entrydate))
		--								/(CONVERT(FLOAT,(AVG(NoStandardStock)OVER(Partition BY DATEDIFF(WEEK ,@From,Entrydate))))),1)END 
		--	FROM  DealerStockResponseAnalysis  DSRA  
		--		  INNER JOIN Dealers D ON D.ID=DSRA.DealerId
		--		  INNER JOIN TAB ON TAB.CityId=D.CityId AND DATEDIFF(WEEK ,@From,Entrydate)=TAB.WeekNo
		--END
 END
 
 
 
 



