IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[uspDynamicInsertionPQPageIdPricequoteCount]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[uspDynamicInsertionPQPageIdPricequoteCount]
GO

	-- =============================================  
--  Author:  Kundan
-- Create date:   01-12-2015
-- Description:  On Every sunday  the table PQPageIdPricequoteCount is truncated and the new count of pqpageid (past 30 days) is inserted into it from NewCarPurchaseInquiries
-- =============================================   
CREATE PROCEDURE [dbo].[uspDynamicInsertionPQPageIdPricequoteCount]
	AS
  
BEGIN 
	SET NOCOUNT ON ;
--IF  (DATENAME(WEEKDAY, getdate())='Sunday'  AND  datePart(hour,getdate()) = 11 ) 
	BEGIN TRY	
			TRUNCATE TABLE PQPageIdPricequoteCount;
	        
			INSERT INTO PQPageIdPricequoteCount (pqPageId,HourPart,pqPageId4HourCount)
			SELECT  NCPI.PQPageID,
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
				    COUNT( NCPI.PQPageID ) / 30 AS pqPageIdHourCount
					FROM vwForAlerNewCarPurchaseInquiries AS NCPI WITH (NOLOCK) 
					WHERE NCPI.ReqDateTimeDatePart between GETDATE()-30 and GETDATE()
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
			END TRY
			BEGIN CATCH
				   INSERT INTO CarWaleWebSiteExceptions (   ModuleName,
															SPName,
															ErrorMsg,
															TableName,
															CreatedOn)
											VALUES ( 'Count of PQPageId if Traffic less than benchmark Alert',
													 'dbo.uspDynamicInsertionPQPageIdPricequoteCount',
													  ERROR_MESSAGE(),
													  'PQPageIdPricequoteCount',
													   GETDATE()  
													 )
			  END CATCH 
END