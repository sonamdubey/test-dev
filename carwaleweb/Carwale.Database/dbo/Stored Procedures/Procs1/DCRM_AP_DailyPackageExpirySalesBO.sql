IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[DCRM_AP_DailyPackageExpirySalesBO]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[DCRM_AP_DailyPackageExpirySalesBO]
GO

	

CREATE PROCEDURE [dbo].[DCRM_AP_DailyPackageExpirySalesBO]
	
	AS

	BEGIN
		DECLARE @NumberRecords AS INT
		DECLARE @RowCount AS INT
		DECLARE @CurrentDate AS DATETIME
		
		--SET Current DateTime
		SET @CurrentDate = GETDATE()
		
		DECLARE @TempDealers Table(RowID INT IDENTITY(1, 1), DealerId NUMERIC, DealerSource INT, ExpiryDate DATETIME, 
							DSalesId NUMERIC,CallerId NUMERIC, PackageDetails VARCHAR(200))		
		
		INSERT INTO @TempDealers
		SELECT DISTINCT(D.ID) AS DealerId, D.DealerSource, CC.ExpiryDate, DSD.Id AS DSalesId,DUD.UserId AS CallerId,
			(SELECT TOP 1 Convert(VarChar, Cpr.PackageId) + ':' + Convert(VarChar, Cpr.ActualValidity/30) + ':' + Convert(VarChar, Cpr.ActualAmount) 
				AS PackageDetails 
			FROM Packages Pkg WITH (NOLOCK), ConsumerPackageRequests Cpr WITH (NOLOCK)
			WHERE Pkg.Id = Cpr.PackageId  AND ConsumerType = 1 AND Cpr.IsActive = 1 AND Cpr.IsApproved = 1
				AND Pkg.IsActive = 1 AND Cpr.ConsumerId = D.id  AND Pkg.InqPtCategoryId = CC.PackageType  AND Pkg.IsStockBased = 1
			ORDER BY Cpr.ID DESC) Package
		FROM 
			ConsumerCreditPoints CC WITH (NOLOCK), Dealers AS D WITH (NOLOCK)
			LEFT JOIN DCRM_SalesDealer DSD WITH (NOLOCK) ON D.ID = DSD.DealerId AND DSD.LeadStatus = 1
			LEFT JOIN DCRM_ADM_UserDealers DUD WITH (NOLOCK) ON D.ID = DUD.DealerId AND DUD.RoleId = 5
		WHERE D.ID = CC.ConsumerId AND CC.ConsumerType = 1
			AND CC.ExpiryDate = CONVERT(VarChar, GETDATE()+20, 110)
		
		-- Get the number of records in the temporary table
		SET @NumberRecords = @@ROWCOUNT
		SET @RowCount = 1

		--Parameters added by vaibhav
		DECLARE @DealerSalesId	NUMERIC,
				@PitchProduct	INT,
				@ClosingAmt		NUMERIC,
				@PitchDuration	INT,
				@PkgExpiryDate	DATETIME,
				@DealerSource	SMALLINT,
				@PkgDetails		VARCHAR(50),
				@CallerId		NUMERIC,
				@DealerId		NUMERIC,
				@DealerType		INT
		
		WHILE @RowCount <= @NumberRecords
			BEGIN
				
				--Retrieve the package & other details of dealer from temporary table
				SELECT @DealerSalesId = DSalesId, @PkgDetails = PackageDetails,
						@PkgExpiryDate = ExpiryDate, @DealerSource = DealerSource,
						@CallerId = CallerId, @DealerId = DealerId
				FROM @TempDealers
				WHERE RowID = @RowCount
				
				--If dealer status is not open or dealer not present
				IF @DealerSalesId IS NULL OR @DealerSalesId < 0
					BEGIN
						SET @PkgDetails = LTRIM(RTRIM(@PkgDetails))
						
						--PitchingProduct
						DECLARE @Pos INT
						SET @Pos = CHARINDEX(':', @PkgDetails)
						SET @PitchProduct = LTRIM(RTRIM(LEFT(@PkgDetails, @Pos - 1)))
						
						--ClosingAmount
						SET @PkgDetails = RIGHT(@PkgDetails, LEN(@PkgDetails) - @Pos)
						SET @Pos = CHARINDEX(':', @PkgDetails, 1)
						SET @PitchDuration = LTRIM(RTRIM(LEFT(@PkgDetails, @Pos - 1)))
						
						--PitchDuration
						SET @PkgDetails = RIGHT(@PkgDetails, LEN(@PkgDetails) - @Pos)
						SET @ClosingAmt = @PkgDetails
						
						--Set the Dealer type according to pacakge expiry date of dealer
						DECLARE @DayDiff	INT
						SET @DayDiff = DATEDIFF(DD,@PkgExpiryDate,@CurrentDate)
						SELECT @DealerType = 
							CASE 
								WHEN @DayDiff <= 0 THEN 3 -- Renewal Before Expiry Date
								WHEN @DayDiff < 30 THEN 5 -- Expiry Within 30 Days
								WHEN @DayDiff < 90 THEN 2 -- Expiry Days above 30Days but Below 90Days
								WHEN @DayDiff > 90 THEN 4 -- Expiry Aboove 90Days
								ELSE 1 -- New Dealer 
							END

						--Insert the retrieved data into the table DCRM_SalesDealer
						INSERT INTO DCRM_SalesDealer (DealerId,EntryDate,LeadSource,DealerType,ClosingProbability,
									LeadStatus,PitchingProduct,PitchDuration,ClosingAmount,UpdatedBy,UpdatedOn,BOExecutive,FieldExecutive,LostReason,IsSalesAppointment,PackageExpiryDate, ClosingDate)
						VALUES (@DealerId,@CurrentDate,@DealerSource,@DealerType,10,1,@PitchProduct,@PitchDuration,@ClosingAmt,13,@CurrentDate,-1,-1,-1,0,@PkgExpiryDate, GETDATE()+ 30)
					END
					
				--Scheduling a new call  for the dealer
				--Check if any user is assigned for the dealer
				IF @CallerId > 0 AND @CallerId IS NOT NULL
					BEGIN
						DECLARE @NewCallId NUMERIC
						DECLARE @ExtCallId NUMERIC
						EXEC DCRM_ScheduleNewCall @DealerId,@CallerId,@CurrentDate,13,@CurrentDate,'Dealer Renewal Alert',NULL,3, @NewCallId OUTPUT, @ExtCallId OUTPUT, 2
					END
				ELSE
					BEGIN
						--IF no user assigned for dealer then save Dealer as Orphan Dealer.
						INSERT INTO DCRM_OrphanDealers (DealerId,Type)
						VALUES (@DealerId,1)
					END
					
				SET @RowCount = @RowCount + 1
			END
	END




