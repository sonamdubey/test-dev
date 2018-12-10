IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[DCRM_AP_DailyAlertsSBO]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[DCRM_AP_DailyAlertsSBO]
GO

	
CREATE PROCEDURE [dbo].[DCRM_AP_DailyAlertsSBO]
	
	AS

	BEGIN
		DECLARE @DateVal AS DATETIME
		DECLARE @NumberRecords AS INT
		DECLARE @RowCount AS INT

		DECLARE @TempDealers Table(RowID INT IDENTITY(1, 1), DealerId NUMERIC, UserId NUMERIC, IsWKitSent BIT, 
									IsTCDealer BIT, IsTCTrainingGiven BIT, LiveStock NUMERIC, PackageExpDay NUMERIC)
		
		SET @DateVal = GETDATE()
		
		INSERT INTO @TempDealers
		SELECT DISTINCT D.ID AS DealerId, DAU.UserId, D.IsWKitSent, D.IsTCDealer,
						 D.IsTCTrainingGiven, 
						 (SELECT COUNT(SI.Id) FROM  SellInquiries AS SI 
							WHERE D.ID = SI.DealerId AND SI.StatusId = 1 
							AND SI.PackageExpiryDate >= CONVERT(Date,GETDATE())) AS LiveStock, 
							DATEDIFF(dd,GETDATE(), CCP.ExpiryDate) AS PackageExpDay
		FROM Dealers AS D, ConsumerCreditPoints AS CCP,
			DCRM_ADM_UserDealers AS DAU
		WHERE D.ID = DAU.DealerId AND DAU.RoleId = 4 AND D.Status = 0
			AND D.ID = CCP.ConsumerId AND CCP.ConsumerType = 1 AND CCP.ExpiryDate >= CONVERT(Date,GETDATE()) 
		GROUP BY D.ID, DAU.UserId, D.IsWKitSent, D.IsTCDealer, D.IsTCTrainingGiven, DATEDIFF(dd,GETDATE(), CCP.ExpiryDate)
		
		-- Get the number of records in the temporary table
		SET @NumberRecords = @@ROWCOUNT
		SET @RowCount = 1

		DECLARE @DealerId AS NUMERIC
		DECLARE @UserId AS NUMERIC
		DECLARE @IsWKitSent AS BIT
		DECLARE @IsTCDealer AS BIT
		DECLARE @IsTCTrainingGiven AS BIT
		DECLARE @LiveStock AS NUMERIC
		DECLARE @PackageExpDay AS NUMERIC
		
		WHILE @RowCount <= @NumberRecords
			BEGIN
				SELECT @DealerId = DealerId, @UserId = UserId, @IsWKitSent = IsWKitSent, 
						@IsTCDealer = IsTCDealer, @IsTCTrainingGiven = IsTCTrainingGiven,
						@LiveStock = LiveStock, @PackageExpDay = PackageExpDay
				FROM @TempDealers
				WHERE RowID = @RowCount
			
				IF @DealerId > 0 AND @DealerId IS NOT NULL AND @UserId > 0 AND @UserId IS NOT NULL
					BEGIN
						--Welcome kit not Sent
						IF @IsWKitSent = 0 
							EXEC DCRM_ScheduleAlerts @DealerId, 1, @UserId, @DateVal, 1, 13 
						
						--TC software Training Pending	
						IF @IsTCDealer = 1 AND @IsTCTrainingGiven = 0 
							EXEC DCRM_ScheduleAlerts @DealerId, 25,@UserId, @DateVal, 1, 13 
								
						--Zero Stock
						IF @LiveStock = 0
							EXEC DCRM_ScheduleAlerts @DealerId, 3, @UserId, @DateVal, 1, 13 
						
						--Dealership package expiring in 7 days
						IF @PackageExpDay = 7
							EXEC DCRM_ScheduleAlerts @DealerId, 5, @UserId, @DateVal, 1, 13 
							
						--Dealership package expiring in 3 days
						IF @PackageExpDay = 3
							EXEC DCRM_ScheduleAlerts @DealerId, 6, @UserId, @DateVal, 1, 13 
							
						--Dealership package expiring in 1 days
						IF @PackageExpDay = 1
							EXEC DCRM_ScheduleAlerts @DealerId, 7, @UserId, @DateVal, 1, 13 
							
						--Check Stock Status and schedule related alerts
						EXEC DCRM_AP_ScheduleStockAlertsSBO @DealerId, @UserId
						
					END
	 
				SET @RowCount = @RowCount + 1
			END
		--DROP TABLE @TempDealers
	END

