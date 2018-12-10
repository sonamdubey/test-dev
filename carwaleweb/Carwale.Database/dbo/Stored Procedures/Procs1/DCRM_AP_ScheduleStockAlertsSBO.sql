IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[DCRM_AP_ScheduleStockAlertsSBO]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[DCRM_AP_ScheduleStockAlertsSBO]
GO

	

CREATE PROCEDURE [dbo].[DCRM_AP_ScheduleStockAlertsSBO]
	
	@DealerId	NUMERIC,
	@UserId		NUMERIC
	
AS

	BEGIN
		DECLARE @DateVal AS DATETIME
		DECLARE @TotalResponse AS INT
		DECLARE @TotalAge AS INT
		DECLARE @StockLastUpdated AS DATETIME
		DECLARE @TempVal AS NUMERIC
		
		SET @DateVal = GETDATE()
		
		--Dealership not contacted since 7 days
		SET @TempVal = 0
		SELECT TOP 1  @TempVal = DATEDIFF(dd, CalledDate, GETDATE())
		FROM DCRM_Calls DC, DCRM_ADM_UserDealers AS DAU
		WHERE DC.UserId = DAU.UserId AND DC.DealerId = DAU.DealerId 
			AND DC.DealerId = @DealerId AND DC.UserId <> 13 AND CalledDate IS NOT NULL
			AND DAU.RoleId = 4
		ORDER BY Id DESC
			
		IF @TempVal > 7 
			EXEC DCRM_ScheduleAlertsAndCalls @DealerId, 17, @UserId, @DateVal, 1, 13, 7
				
		--Dealership Not at all contacted yet
		SELECT TOP 1  Id
		FROM DCRM_Calls DC, DCRM_ADM_UserDealers AS DAU
		WHERE DC.UserId = DAU.UserId AND DC.DealerId = DAU.DealerId 
			AND DC.DealerId = @DealerId AND DC.UserId <> 13 AND CalledDate IS NOT NULL
			AND DAU.RoleId = 4
		ORDER BY Id DESC

		IF @@ROWCOUNT = 0
			EXEC DCRM_ScheduleAlertsAndCalls @DealerId, 17, @UserId, @DateVal, 1, 13, 7
			
		--Poor Response On Car(Total response on current stock/ total age of current stock cars)
			SELECT @TotalResponse = COUNT(UPI.Id) 
			FROM SellInquiries AS SI, UsedCarPurchaseInquiries AS UPI
			WHERE SI.StatusId = 1 AND SI.PackageExpiryDate >= CONVERT(Date,GETDATE())
				AND SI.ID = UPI.SellInquiryId AND SI.DealerId = @DealerId
				
			SELECT @TotalAge = SUM(DATEDIFF(dd,SI.entrydate,GETDATE()))
			FROM SellInquiries AS SI
			WHERE SI.StatusId = 1 AND SI.PackageExpiryDate >= CONVERT(Date,GETDATE())
				AND SI.DealerId = @DealerId
				
			IF @TotalResponse/@TotalAge < 3
				EXEC DCRM_ScheduleAlerts @DealerId, 8, @UserId, @DateVal, 1, 13
		
		--Stock not updated since 15 days
			SET @TempVal = 0
			SELECT @TempVal = IsNull(DateDiff(day, Max(LastUpdated), GetDate()), 0) 
			FROM SellInquiries WHERE DealerId = @DealerId
		
			IF @TempVal >= 15
				EXEC DCRM_ScheduleAlerts @DealerId, 9, @UserId, @DateVal, 1, 13
			
		--Get Some Data in a Temp Table
			DECLARE @StockData Table(InquiryId NUMERIC, TPhotos NUMERIC, Comments VarChar(500), StockAge NUMERIC)
			
			INSERT INTO @StockData							
			SELECT SI.ID, COUNT(CP.Id) AS TPhotos, SI.Comments, DATEDIFF(DD,SI.EntryDate,GETDATE()) AS StockAge
			FROM SellInquiries AS SI LEFT JOIN CarPhotos AS CP ON SI.ID = CP.InquiryId 
					AND CP.IsDealer = 1 AND CP.IsActive = 1
			WHERE SI.StatusId = 1 AND SI.PackageExpiryDate >= CONVERT(Date,GETDATE())
			AND SI.DealerId = @DealerId 
			GROUP BY SI.ID, SI.Comments,DATEDIFF(DD,SI.EntryDate,GETDATE())
			
		--Stock info incomplete - no photos 
			IF DAY(GETDATE()) = 1
				BEGIN
					SET @TempVal = 0
					SELECT @TempVal = COUNT(InquiryId) FROM @StockData WHERE TPhotos = 0
					IF @TempVal > 0
						EXEC DCRM_ScheduleAlerts @DealerId, 10, @UserId, @DateVal, 1, 13
				END
				
		--Stock info incomplete - few photos
			IF DATENAME(WEEKDAY, GETDATE()) = 'Monday'
				BEGIN
					SET @TempVal = 0
					SELECT @TempVal = COUNT(InquiryId) FROM @StockData WHERE TPhotos < 3 AND TPhotos > 0
					IF @TempVal > 0
						EXEC DCRM_ScheduleAlerts @DealerId, 11, @UserId, @DateVal, 1, 13 
				END
				
		--Stock info incomplete - no description
			IF DATENAME(WEEKDAY, GETDATE()) = 'Monday'
				BEGIN
					SET @TempVal = 0
					SELECT @TempVal = COUNT(InquiryId) FROM @StockData WHERE Comments = '' OR Comments IS NULL
					IF @TempVal > 0
						EXEC DCRM_ScheduleAlerts @DealerId, 12, @UserId, @DateVal, 1, 13 
				END
				
		--Stock age is beyond range(Average Stock age is more than 30 days)
			SET @TempVal = 0
			SELECT @TempVal = AVG(StockAge) FROM @StockData 
			IF @TempVal > 30
				EXEC DCRM_ScheduleAlerts @DealerId, 13, @UserId, @DateVal, 1, 13 
				
		--Stock not moving. (age of car in stock is beyond 30 days)
			SET @TempVal = 0
			SELECT @TempVal = COUNT(InquiryId) FROM @StockData WHERE StockAge > 30
			IF @TempVal > 0
				EXEC DCRM_ScheduleAlerts @DealerId, 14, @UserId, @DateVal, 1, 13 
			
		--No response on dealers cars since 3 days
			SET @TotalResponse = 0
			SELECT @TotalResponse = COUNT(UPI.Id) 
			FROM SellInquiries AS SI, UsedCarPurchaseInquiries AS UPI
			WHERE SI.StatusId = 1 AND SI.PackageExpiryDate >= CONVERT(Date,GETDATE())
				AND SI.ID = UPI.SellInquiryId AND SI.DealerId = @DealerId
				AND UPI.RequestDateTime BETWEEN DATEADD(DD,-4,GETDATE()) AND GETDATE()
			
			IF @TotalResponse = 0
				EXEC DCRM_ScheduleAlerts @DealerId, 15, @UserId, @DateVal, 1, 13 
					
		--No response on dealers cars since 7 days
			SET @TotalResponse = 0
			SELECT @TotalResponse = COUNT(UPI.Id) 
			FROM SellInquiries AS SI, UsedCarPurchaseInquiries AS UPI
			WHERE SI.StatusId = 1 AND SI.PackageExpiryDate >= CONVERT(Date,GETDATE())
				AND SI.ID = UPI.SellInquiryId AND SI.DealerId = @DealerId
				AND UPI.RequestDateTime BETWEEN DATEADD(DD,-7,GETDATE()) AND GETDATE()
			
			IF @TotalResponse = 0
				EXEC DCRM_ScheduleAlerts @DealerId, 16, @UserId, @DateVal, 1, 13 
								
		--Incomplete team assigned to dealership
			SET @TempVal = 0
			SELECT @TempVal = COUNT(DAU.DealerId) FROM DCRM_ADM_UserDealers AS DAU, DCRM_ADM_UserRoles DAR
			WHERE DAU.UserId = DAR.UserId AND DAR.RoleId = 5 AND DAU.DealerId = @DealerId
			
			IF @TempVal = 0
				EXEC DCRM_ScheduleAlerts @DealerId, 18, @UserId, @DateVal, 1, 13 
				
		--Car prices out of range
			IF DAY(GETDATE()) = 1
				BEGIN
					SET @TempVal = 0
					SELECT @TempVal = COUNT(DC.Id) FROM DCRM_CarsORData AS DC, SellInquiries AS SI
					WHERE DC.InquiryId = SI.ID AND SI.DealerId = @DealerId AND DC.ORPrice = 1
					
					IF @TempVal > 0
						EXEC DCRM_ScheduleAlerts @DealerId, 20, @UserId, @DateVal, 1, 13 
				END
				
		--Cars KM out of range
			SET @TempVal = 0
			SELECT @TempVal = COUNT(DC.Id) FROM DCRM_CarsORData AS DC, SellInquiries AS SI
			WHERE DC.InquiryId = SI.ID AND SI.DealerId = @DealerId AND DC.ORKm = 1
			
			IF @TempVal > 0
				EXEC DCRM_ScheduleAlerts @DealerId, 21, @UserId, @DateVal, 1, 13
				
		--Customer Calling Alert(If per dealer feedback in a month is less than 5)
		SET @TempVal = 0
		SELECT @TempVal = COUNT(DISTINCT InquiryId) 
		FROM DCRM_BuyerFeedback AS DB, SellInquiries AS SI 
		WHERE DB.InquiryId = SI.ID AND SI.DealerId = @DealerId 
		AND MONTH(FeedbackDate) = MONTH(GETDATE()) AND YEAR(FeedbackDate) = YEAR(GETDATE()) 
		
		IF @TempVal < 5
			EXEC DCRM_ScheduleAlerts @DealerId, 30, @UserId, @DateVal, 1, 13
	END



