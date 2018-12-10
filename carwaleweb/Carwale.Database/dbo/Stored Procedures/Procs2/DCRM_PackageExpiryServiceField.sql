IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[DCRM_PackageExpiryServiceField]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[DCRM_PackageExpiryServiceField]
GO

	
--Author	:	Sachin Bharti(6th April 2013)
--Purpose	:	Used to transfer Package Expired Dealers to Sales or  Service field	
--				executives if Dealer is not exist DCRM_SalesDealer and have either
--				of Sales or Service field Executives exist for Dealer

CREATE PROCEDURE [dbo].[DCRM_PackageExpiryServiceField]
	@DealerIds	VARCHAR(200),
	@Result		INT OUTPUT
	AS

	BEGIN
		DECLARE	@PitchProduct	INT,
				@ClosingAmt		NUMERIC,
				@PitchDuration	INT,
				@PkgExpiryDate	DATETIME,
				@DealerSource	SMALLINT,
				@PkgDetails		VARCHAR(100),
				@CallerId		NUMERIC,
				@DealerId		NUMERIC,
				@DealerType		INT,
				@CurrentDate AS DATETIME,
				@DealerIndx VARCHAR(50)
		
		--SET Current DateTime
		SET @CurrentDate = GETDATE()	
		
		IF @DealerIds <> ''
			BEGIN	  
				WHILE @DealerIds <> ''
					BEGIN
						SET @DealerIndx = CHARINDEX(',' , @DealerIds)
						IF  @DealerIndx > 0
							BEGIN
								SET @DealerId = LEFT(@DealerIds, @DealerIndx-1)  
								SET @DealerIds = RIGHT(@DealerIds, LEN(@DealerIds)- @DealerIndx)
							
								SELECT @DealerSource = ISNULL(D.DealerSource,'31') ,@PkgExpiryDate=CCP.ExpiryDate,@CallerId = DUD.UserId ,
									@PkgDetails= (SELECT TOP 1 Convert(VarChar, Cpr.PackageId) + ':' + Convert(VarChar, Cpr.ActualValidity/30) + ':' + Convert(VarChar, Cpr.ActualAmount) AS PackageDetails 
									FROM Packages Pkg WITH (NOLOCK), ConsumerPackageRequests Cpr WITH (NOLOCK)
									WHERE Pkg.Id = Cpr.PackageId  AND ConsumerType = 1 AND Cpr.IsActive = 1 AND Cpr.IsApproved = 1
									AND Pkg.IsActive = 1 AND Cpr.ConsumerId = D.id  AND Pkg.InqPtCategoryId = CCP.PackageType  AND Pkg.IsStockBased = 1
									ORDER BY Cpr.ID DESC) 
								FROM ConsumerCreditPoints CCP WITH (NOLOCK)
								INNER JOIN Dealers AS D WITH (NOLOCK) ON CCP.ConsumerId = D.ID AND CCP.ConsumerType = 1 
								INNER JOIN DCRM_ADM_UserDealers DUD WITH (NOLOCK) ON D.ID = DUD.DealerId AND DUD.RoleId = 5
								WHERE D.ID = @DealerId
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
								VALUES (@DealerId,@CurrentDate,31,@DealerType,10,1,ISNULL(@PitchProduct,'-1'),@PitchDuration,@ClosingAmt,13,@CurrentDate,-1,@CallerId,-1,0,@PkgExpiryDate, GETDATE()+ 30)
								
								SET @Result = SCOPE_IDENTITY()
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
							END
					END
			END
	END
	




