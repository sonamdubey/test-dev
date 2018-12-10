IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[DO_CheckRuleForOffer]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[DO_CheckRuleForOffer]
GO

	-- =============================================
-- Author:		Vaibhav K
-- Create date: 21 Nov 2014
-- Description:	Check rules before saving dealer offer
-- =============================================
CREATE PROCEDURE [dbo].[DO_CheckRuleForOffer]
	-- Add the parameters for the stored procedure here
	@OfferId		NUMERIC(18, 0),
	@CityId			INT = NULL,
	@ZoneId			INT = NULL,	
	@MakeId			INT = NULL,
	@ModelId		VARCHAR(250) = NULL,
	@VersionId		VARCHAR(250) = NULL,
	@StartDate		DATETIME = NULL,
	@EndDate		DATETIME = NULL,
	@IsNewOffer		BIT OUTPUT
AS
BEGIN
		
	SET @IsNewOffer = 1		
	--DECLARE @CMKId INT = -1, @CMOId INT = -1, @CVId INT = -1, @CMKId2 INT = -1
	DECLARE @CarMake VARCHAR(50) = ''

	--IF @OfferId <> - 1
	--	BEGIN
	--		--while approving any offer from offers list page
	--		SELECT DO.* 
	--		FROM DealerOffers DO
	--	END
	--ELSE
	--	BEGIN
			--on saving offer for the first time
			--SELECT DO.* 
			--FROM DealerOffers DO
			--JOIN DealerOffersDealers DOD ON DO.ID = DOD.OfferId
			--JOIN DealerOffersVersion DOV ON DO.ID = DOV.OfferId
			--WHERE DO.IsActive = 1 AND DO.IsApproved = 1
			--AND (@StartDate NOT BETWEEN DO.StartDate AND DO.EndDate) 
			--AND (@EndDate NOT BETWEEN DO.StartDate AND DO.EndDate)
			--AND (DOD.ZoneId = @ZoneId OR (DOD.CityId = @CityId AND DOD.ZoneId IS NULL))

			--IF @@ROWCOUNT > 0
			--	SET @IsNewOffer = 0
				
			DECLARE @TempVersion INT, @IndxVersion INT
			IF @VersionId IS NOT NULL AND @VersionId <> ''-- AND @VersionId <> '-1'
				BEGIN
					WHILE @VersionId <> '' AND @IsNewOffer = 1
						BEGIN
							PRINT @ModelId
							PRINT 1
							SET @IndxVersion = CHARINDEX(',', @VersionId)

							IF @IndxVersion > 0
								BEGIN
									SET @TempVersion = LEFT(@VersionId, @IndxVersion - 1)
									SET @VersionId = RIGHT(@VersionId, LEN(@VersionId) - @IndxVersion)
								END
							ELSE
								BEGIN
									SET @TempVersion = @VersionId
									SET @VersionId = ''
								END

							--current version in variable @TempVersion	
							DECLARE @Models VARCHAR(250)						
							SET @Models = @ModelId
							DECLARE @TempModel INT, @IndxModel INT
							WHILE @Models <> '' AND @IsNewOffer = 1
								BEGIN
									SET @IndxModel = CHARINDEX(',', @Models)

									IF @IndxModel > 0
										BEGIN
											SET @TempModel = LEFT(@Models, @IndxModel - 1)
											SET @Models = RIGHT(@Models, LEN(@Models) - @IndxModel)
										END
									ELSE
										BEGIN
											SET @TempModel = @Models
											SET @Models = ''
										END
										
									SELECT DO.* 
									FROM 
										DealerOffers DO
										JOIN DealerOffersDealers DOD ON DO.ID = DOD.OfferId
										JOIN DealerOffersVersion DOV ON DO.ID = DOV.OfferId
									WHERE 
										DO.IsActive = 1 AND DO.IsApproved = 1 AND DO.OfferType = 1 
										AND CAST(DO.StartDate AS date) <= CAST(GETDATE() AS date)
										AND CAST(DO.EndDate AS date) > CAST(GETDATE() AS date)
										AND (DOD.ZoneId = @ZoneId OR (DOD.CityId = @CityId AND DOD.ZoneId IS NULL) OR (DOD.CityId = -1) OR (DOD.CityId <> -1 AND @CityId = -1))
										AND 
										(
											(DOV.MakeId = @MakeId AND DOV.ModelId = @TempModel AND DOV.VersionId = @TempVersion) OR 
											(DOV.MakeId = @MakeId AND DOV.ModelId = @TempModel AND DOV.VersionId = -1) OR 											
											(DOV.MakeId = @MakeId AND DOV.ModelId = -1) OR
											--model all versions
											(@TempVersion = -1 AND DOV.ModelId = @TempModel) OR
											--make all models all versions
											(@TempVersion = -1 AND @TempModel = -1 AND DOV.MakeId = @MakeId)
										)
										AND (@OfferId = -1 OR DOV.OfferId <> @OfferId)
										AND ISNULL(DO.ClaimedUnits,0) < DO.OfferUnits

										IF @@ROWCOUNT  <= 0
										BEGIN
											--current model in variable @TempModel
											SELECT DO.* 
											FROM 
												DealerOffers DO
												JOIN DealerOffersDealers DOD ON DO.ID = DOD.OfferId
												JOIN DealerOffersVersion DOV ON DO.ID = DOV.OfferId
											WHERE 
												DO.IsActive = 1 AND DO.IsApproved = 1 AND DO.OfferType = 1
												--AND (CAST(@StartDate AS date) BETWEEN CAST(DO.StartDate AS date) AND CAST(DO.EndDate AS date)) 
												--AND (CAST(@EndDate AS date) BETWEEN CAST(DO.StartDate AS date) AND CAST(DO.EndDate AS date))
												AND 
												(
												(CAST(@StartDate AS date) BETWEEN CAST(DO.StartDate AS date) AND CAST(DO.EndDate AS date)) 
												OR (CAST(@StartDate AS date) < CAST(DO.StartDate AS date))
												)
												AND 
												(
												(CAST(@EndDate AS date) BETWEEN CAST(DO.StartDate AS date) AND CAST(DO.EndDate AS date)) 
												OR (CAST(@EndDate AS date) > CAST(DO.EndDate AS date))
												)

												AND (DOD.ZoneId = @ZoneId OR (DOD.CityId = @CityId AND DOD.ZoneId IS NULL) OR (DOD.CityId = -1) OR (DOD.CityId <> -1 AND @CityId = -1))
												AND 
												(
													(DOV.MakeId = @MakeId AND DOV.ModelId = @TempModel AND DOV.VersionId = @TempVersion) OR 
													(DOV.MakeId = @MakeId AND DOV.ModelId = @TempModel AND DOV.VersionId = -1) OR 											
													(DOV.MakeId = @MakeId AND DOV.ModelId = -1) OR
													--model all versions
													(@TempVersion = -1 AND DOV.ModelId = @TempModel) OR
													--make all models all versions
													(@TempVersion = -1 AND @TempModel = -1 AND DOV.MakeId = @MakeId)
												)
												AND (@OfferId = -1 OR DOV.OfferId <> @OfferId)
												AND ISNULL(DO.ClaimedUnits,0) < DO.OfferUnits

										IF @@ROWCOUNT > 0
											BEGIN
												SET @IsNewOffer = 0
												--PRINT @MakeId
												--PRINT @TempModel
												--PRINT @TempVersion
												--PRINT @CityId
												--PRINT @ZoneId
												SELECT DISTINCT Make,Model,CASE @TempVersion WHEN -1 THEN NULL ELSE @TempVersion END AS Version FROM vwMMV 
												WHERE ModelId = @TempModel
												AND VersionId = CASE WHEN @TempVersion =-1 THEN VersionId ELSE @TempVersion END
												--(VersionId = @TempVersion) OR (ModelId = @TempModel AND @TempVersion=-1)
											END
									END
									ELSE
									BEGIN
										SET @IsNewOffer = 0
									END
								END --end of model loop
						END --end of version loop
				END --if version present
			--AND (DOV.VersionId = @VersionId)
			--AND (DOV.ModelId = @ModelId AND DOV.VersionId = -1)
		--END
END
