IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[CRM_SaveVerificationOtherDetails]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[CRM_SaveVerificationOtherDetails]
GO

	
CREATE PROCEDURE [dbo].[CRM_SaveVerificationOtherDetails]

	@Type				 SMALLINT,
	@LeadId				 NUMERIC,
	@IIId				 NUMERIC,
	@timeSpan			 INT,
	@DealerId			 NUMERIC,
	@DealerName			 VARCHAR(250),
	@Eagerness			 INT,
	@IsPEDone			 BIT,
	@UpdatedBy			 NUMERIC,
	@UpdatedOn			 DATETIME,
	@PurchaseMode		 SMALLINT = -1,
	@PurchaseOnNameType	 SMALLINT = -1,
	@PurchaseOnName		 VARCHAR(50) = NULL,
	@CurrentCarOwned     VARCHAR(250) = NULL,
	@Occasion            VARCHAR(50) = NULL,
	@Usage				 VARCHAR(50) = NULL,
	@UsageType           VARCHAR(50) = NULL,
	@MonthlyUsageCity    VARCHAR(50) = NULL,
	@MonthlyUsageHighway VARCHAR(50) = NULL,
	@CarOwnership        VARCHAR(50) = NULL,
	@PurchaseContact     VARCHAR(15) = NULL,
	@CompanyName		 VARCHAR(50) = NULL,
	@IsMultipleBooking	 BIT = 0,
	@BookingCount		 BIGINT = 0
 AS
	
BEGIN
	IF @Type = 1
		BEGIN
			--Update Buy time in interested in 
			IF @timeSpan <> -1
			BEGIN
				UPDATE CRM_InterestedIn SET ClosingDate = GETDATE() + @timeSpan WHERE LEADID = @LeadId 
			END
			
			--Update Closing Probanility
			--UPDATE CRM_InterestedIn SET ClosingProbability = @Eagerness
			--WHERE ID = @IIId

			--Update IsMultipleBooking and BookingCount 	
			UPDATE CRM_InterestedIn SET IsMultipleBooking = @IsMultipleBooking,BookingCount = @BookingCount
			WHERE LEADID = @LeadId
			 
			--Update/save in new table
			UPDATE CRM_VerificationOthersLog 
			SET PurchaseTime = @timeSpan, --Eagerness = @Eagerness, 
				IsPEDone = @IsPEDone,
				 UpdatedBy = @UpdatedBy, UpdatedOn = @UpdatedOn,
				 PurchaseMode = CASE ISNULL(@PurchaseMode,0) WHEN -1 THEN PurchaseMode WHEN 0 THEN PurchaseMode ELSE @PurchaseMode END, PurchaseOnNameType = @PurchaseOnNameType, 
				 PurchaseOnName = @PurchaseOnName,CurrentCarOwned = @CurrentCarOwned,Occasion = @Occasion,Usage = @Usage , UsageType = @UsageType , MonthlyUsageCity = @MonthlyUsageCity ,
				 MonthlyUsageHighway =@MonthlyUsageHighway ,CarOwnership = @CarOwnership,PurchaseContact = @PurchaseContact ,CompanyName = @CompanyName , IsMultipleBooking = @IsMultipleBooking,BookingCount = @BookingCount
			WHERE LeadId = @LeadId
			
			IF @@ROWCOUNT = 0
				BEGIN
					INSERT INTO CRM_VerificationOthersLog(LeadId, PurchaseTime, Eagerness, UpdatedOn, UpdatedBy, IsPEDone, 
						PurchaseMode, PurchaseOnNameType, PurchaseOnName,CurrentCarOwned,Occasion,Usage,UsageType,MonthlyUsageCity,MonthlyUsageHighway,CarOwnership,PurchaseContact,CompanyName,IsMultipleBooking,BookingCount)
					VALUES(@LeadId, @timeSpan, @Eagerness, @UpdatedOn, @UpdatedBy, @IsPEDone, 
						@PurchaseMode, @PurchaseOnNameType, @PurchaseOnName,@CurrentCarOwned,@Occasion,@Usage,@UsageType,@MonthlyUsageCity,@MonthlyUsageHighway,@CarOwnership,@PurchaseContact,@CompanyName,@IsMultipleBooking,@BookingCount)
				END
		END
		
	IF @Type = 2
		BEGIN
			UPDATE CRM_VerificationOthersLog 
			SET DealerId = @DealerId, DealerName = @DealerName,
				UpdatedBy = @UpdatedBy, UpdatedOn = @UpdatedOn
			WHERE LeadId = @LeadId
				
			IF @@ROWCOUNT = 0
				BEGIN
					INSERT INTO CRM_VerificationOthersLog
						(LeadId, DealerId, DealerName, UpdatedOn, UpdatedBy)
					VALUES(@LeadId, @DealerId, @DealerName, @UpdatedOn, @UpdatedBy)
				END
		END
		
	IF @Type = 3
		BEGIN
			--Update Buy time in interested in 
			UPDATE CRM_InterestedIn SET ClosingDate = GETDATE() + @timeSpan,
				ClosingProbability = @Eagerness
			WHERE LEADID = @LeadId
			
			--Log Requests
			INSERT INTO CRM_VerificationOthersLog(LeadId, PurchaseTime, Eagerness, UpdatedOn, UpdatedBy, IsPEDone, DealerName)
				VALUES(@LeadId, @timeSpan, @Eagerness, @UpdatedOn, @UpdatedBy, @IsPEDone, @DealerName)
		END 
END














