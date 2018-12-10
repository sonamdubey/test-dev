IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[DCRM_AP_ScheduleStockAlertsSField]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[DCRM_AP_ScheduleStockAlertsSField]
GO

	


CREATE PROCEDURE [dbo].[DCRM_AP_ScheduleStockAlertsSField]
	
	@DealerId	NUMERIC,
	@UserId		NUMERIC
	
AS

	BEGIN
		DECLARE @DateVal AS DATETIME
		DECLARE @TempVal AS NUMERIC
		
		SET @DateVal = GETDATE()
		
		--Get Some Data in a Temp Table
			DECLARE @StockData Table(InquiryId NUMERIC, TPhotos NUMERIC, Comments VarChar(500))
			
			INSERT INTO @StockData							
			SELECT SI.ID, COUNT(CP.Id) AS TPhotos, SI.Comments
			FROM SellInquiries AS SI LEFT JOIN CarPhotos AS CP ON SI.ID = CP.InquiryId 
					AND CP.IsDealer = 1 AND CP.IsActive = 1
			WHERE SI.StatusId = 1 AND SI.PackageExpiryDate >= CONVERT(Date,GETDATE())
			AND SI.DealerId = @DealerId 
			GROUP BY SI.ID, SI.Comments,DATEDIFF(DD,SI.EntryDate,GETDATE())
			
		--Stock info incomplete - no photos 
			IF (DAY(GETDATE()) = 1 OR DAY(GETDATE()) = 16)
				BEGIN
					SET @TempVal = 0
					SELECT @TempVal = COUNT(InquiryId) FROM @StockData WHERE TPhotos = 0
					IF @TempVal > 0
						EXEC DCRM_ScheduleAlertsAndCalls @DealerId, 28, @UserId, @DateVal, 1, 13, 9
				END
				
		--Stock info incomplete - no description
			IF (DAY(GETDATE()) = 1 OR DAY(GETDATE()) = 16)
				BEGIN
					SET @TempVal = 0
					SELECT @TempVal = COUNT(InquiryId) FROM @StockData WHERE Comments = '' OR Comments IS NULL
					IF @TempVal > 0
						EXEC DCRM_ScheduleAlertsAndCalls @DealerId, 29, @UserId, @DateVal, 1, 13, 9
				END
				
	END




