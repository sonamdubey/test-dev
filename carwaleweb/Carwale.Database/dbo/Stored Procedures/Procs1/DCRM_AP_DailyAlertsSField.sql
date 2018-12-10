IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[DCRM_AP_DailyAlertsSField]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[DCRM_AP_DailyAlertsSField]
GO

	

CREATE PROCEDURE [dbo].[DCRM_AP_DailyAlertsSField]
	
	AS

	BEGIN
		DECLARE @DateVal AS DATETIME
		DECLARE @NumberRecords AS INT
		DECLARE @RowCount AS INT

		DECLARE @TempDealers Table(RowID INT IDENTITY(1, 1), DealerId NUMERIC, UserId NUMERIC, IsWKitSent BIT, 
							IsTCDealer BIT, IsTCTrainingGiven BIT, LiveStock NUMERIC, LastServiceVisit Numeric)
		
		SET @DateVal = GETDATE()
		
		INSERT INTO @TempDealers
		SELECT DISTINCT D.ID AS DealerId, DAU.UserId, D.IsWKitSent, D.IsTCDealer,
						 D.IsTCTrainingGiven,
						 (SELECT COUNT(SI.Id) FROM  SellInquiries AS SI 
							WHERE D.ID = SI.DealerId AND SI.StatusId = 1 
							AND SI.PackageExpiryDate >= CONVERT(Date,GETDATE())) AS LiveStock, 
						 DATEDIFF(dd, LastServiceVisit, GETDATE())
		FROM Dealers AS D, ConsumerCreditPoints AS CCP,
			DCRM_ADM_UserDealers AS DAU
		WHERE D.ID = DAU.DealerId AND DAU.RoleId = 5 AND D.Status = 0
			AND D.ID = CCP.ConsumerId AND CCP.ConsumerType = 1 AND CCP.ExpiryDate >= CONVERT(Date,GETDATE()) 
		GROUP BY D.ID, DAU.UserId, D.IsWKitSent, D.IsTCDealer, D.IsTCTrainingGiven, DATEDIFF(dd, LastServiceVisit, GETDATE())
			
		-- Get the number of records in the temporary table
		SET @NumberRecords = @@ROWCOUNT
		SET @RowCount = 1

		DECLARE @DealerId AS NUMERIC
		DECLARE @UserId AS NUMERIC
		DECLARE @IsWKitSent AS BIT
		DECLARE @IsTCDealer AS BIT
		DECLARE @IsTCTrainingGiven AS BIT
		DECLARE @LiveStock AS NUMERIC
		DECLARE @TempVal AS NUMERIC
		DECLARE @LastServiceVisit AS NUMERIC
		
		WHILE @RowCount <= @NumberRecords
			BEGIN
				SELECT @DealerId = DealerId, @UserId = UserId, @IsWKitSent = IsWKitSent, 
						@IsTCDealer = IsTCDealer, @IsTCTrainingGiven = IsTCTrainingGiven,
						@LiveStock = LiveStock, @LastServiceVisit = LastServiceVisit
				FROM @TempDealers
				WHERE RowID = @RowCount
				
				IF @DealerId > 0 AND @DealerId IS NOT NULL AND @UserId > 0 AND @UserId IS NOT NULL
					BEGIN
						--TC software Training Pending	
						IF @IsTCDealer = 1 AND @IsTCTrainingGiven = 0 
							BEGIN
								IF DAY(GETDATE()) = 1
									EXEC DCRM_ScheduleAlertsAndCalls @DealerId, 2,@UserId, @DateVal, 1, 13, 9 
							END
						
						--Zero Stock	
						IF @LiveStock = 0 
							EXEC DCRM_ScheduleAlertsAndCalls @DealerId, 4,@UserId, @DateVal, 1, 13, 9 
							
						--Incomplete team assigned to dealership
						SET @TempVal = 0
						SELECT @TempVal = COUNT(DAU.DealerId) FROM DCRM_ADM_UserDealers AS DAU, DCRM_ADM_UserRoles DAR
						WHERE DAU.UserId = DAR.UserId AND DAR.RoleId = 4 AND DAU.DealerId = @DealerId
						
						IF @TempVal = 0
							EXEC DCRM_ScheduleAlertsAndCalls @DealerId, 19, @UserId, @DateVal, 1, 13, 9
					
						--No Service visit since last 30 Day
						IF @LastServiceVisit >= 30 
							EXEC DCRM_ScheduleAlertsAndCalls @DealerId, 22, @UserId, @DateVal, 1, 13, 9
						
						--Check Stock Status and schedule related alerts
						EXEC DCRM_AP_ScheduleStockAlertsSField @DealerId, @UserId
					
					END
	 
				SET @RowCount = @RowCount + 1
			END
	END


