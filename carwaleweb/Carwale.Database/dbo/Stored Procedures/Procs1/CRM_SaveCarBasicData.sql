IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[CRM_SaveCarBasicData]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[CRM_SaveCarBasicData]
GO

	CREATE PROCEDURE [dbo].[CRM_SaveCarBasicData]

	@CarBasicDataId			Numeric,
	@LeadId					Numeric,
	@CarVersionId			Numeric,
	@ExShowroomPrice		Numeric,
	@Insurance				Numeric,
	@RTO					Numeric,
	@PQId					Numeric,
	@CityId					Numeric,
	@UpdatedById			Numeric,

	@IsProductExplained		Bit,
	@IsPQMailed				Bit,
	@IsPQMailedExternal		Bit,
	@IsPQMailInternalReq	Bit,
	@IsPQMailExternalReq	Bit,
	@IsFinalized			Bit,
	@PriceQuoteNotRequired	Bit,
	@IsPQRearrange			Bit,
	@IsPENotRequired		Bit,
	
	@ExpectedBuyingDate		DateTime,
	@CreatedOn				DateTime,
	@UpdatedOn				DateTime,
	@InterestedInId			Numeric,
	@NewCarBasicDataId		Numeric OutPut,
	@SourceId				Int,
	@SourceCategory			Int,
	@IsVisitedDealer		Bit = null		
 AS
	
BEGIN
	SET @NewCarBasicDataId = -1
	IF @CarBasicDataId = -1

		BEGIN
			
			--Insert New Data 
			INSERT INTO CRM_CarBasicData
			(
				LeadId, VersionId, ExShowroom, Insurance, RTO, PQId,
				CityId, UpdatedBy, IsProductExplained, IsPQMailed, IsPQMailedExternal,
				IsPQMailInternalReq, IsPQMailExternalReq, PriceQuoteNotRequired,
				IsFinalized, ExpectedBuyingDate, CreatedOn, UpdatedOn, IsPQRearrange, IsPENotRequired, 
				IsVisitedDealer, SourceId, SourceCategory
			)
			VALUES
			(
				@LeadId, @CarVersionId, @ExShowroomPrice, @Insurance, @RTO, @PQId,
				@CityId, @UpdatedById, @IsProductExplained, @IsPQMailed, @IsPQMailedExternal,
				@IsPQMailInternalReq, @IsPQMailExternalReq, @PriceQuoteNotRequired,
				@IsFinalized, @ExpectedBuyingDate, @CreatedOn, @UpdatedOn, @IsPQRearrange, @IsPENotRequired, 
				@IsVisitedDealer, @SourceId, @SourceCategory
			)
			
			SET @NewCarBasicDataId = SCOPE_IDENTITY()
			
			--Update lead status if customer has already visited dealership
			IF @IsVisitedDealer IS NOT NULL
				BEGIN
					SELECT IsVisitedDealer FROM CRM_Leads WITH(NOLOCK) WHERE ID = @LeadId AND IsVisitedDealer = 1
					IF @@ROWCOUNT = 0
						BEGIN
							UPDATE CRM_Leads SET IsVisitedDealer = @IsVisitedDealer WHERE ID = @LeadId
						END
				END
			
			IF @NewCarBasicDataId <> -1

				BEGIN
					INSERT INTO CRM_ActiveItems
						(InterestedInId, ItemId, Priority) 
					VALUES
						(@InterestedInId, @NewCarBasicDataId, 5)
				END
				
			--Update Car Group
			--Added By Deepak Tripathi on 30th Dec 2013
			DECLARE @FLCGroupId INT = -1
			DECLARE @MakeId INT
			DECLARE @ModelId INT
			DECLARE @TempSourceId INT
			
			IF @SourceCategory = 3
				SET @TempSourceId = @SourceId				
			ELSE 
				SET @TempSourceId = 1
		
			SELECT @MakeId = MakeId, @ModelId = ModelId FROM vwMMV WHERE VersionId = @CarVersionId
			SELECT TOP 1 @FLCGroupId = CF.GroupId FROM CRM_ADM_FLCRules CF
			WHERE IsActive = 1 
					AND (MakeId = @MakeId OR MakeId = -1) 
					AND(ModelId = @ModelId OR ModelId = -1) 
					AND (CityId = @CityId OR CityId = -1) 
					AND (SourceId = @TempSourceId OR SourceId = -1)
			ORDER BY Rank DESC
			
			UPDATE CRM_CarBasicData SET FLCGroupId = @FLCGroupId WHERE ID = @NewCarBasicDataId
			
		
		END

	ELSE
		
		BEGIN
			UPDATE CRM_CarBasicData
			SET VersionId = @CarVersionId, ExShowroom = @ExShowroomPrice, Insurance = @Insurance, 
				RTO = @RTO, PQId = @PQId, CityId = @CityId, UpdatedBy = @UpdatedById, 
				IsProductExplained = @IsProductExplained, IsFinalized = @IsFinalized, 
				ExpectedBuyingDate = @ExpectedBuyingDate, UpdatedOn = @UpdatedOn,
				IsPENotRequired = @IsPENotRequired
			WHERE Id = @CarBasicDataId
			
			SET @NewCarBasicDataId = 1
			
			IF @IsPQMailedExternal = 1
			BEGIN 
				UPDATE CRM_CarBasicData SET IsPQMailedExternal = 1, IsPQRearrange = 0 WHERE Id = @CarBasicDataId
			END
			
			IF @IsPQMailExternalReq = 1
			BEGIN 
				UPDATE CRM_CarBasicData SET IsPQMailExternalReq = 1 WHERE Id = @CarBasicDataId
			END
			
			IF @PriceQuoteNotRequired = 1
			BEGIN 
				UPDATE CRM_CarBasicData SET PriceQuoteNotRequired = 1, IsPQRearrange = 0 WHERE Id = @CarBasicDataId
			END
			
			IF @IsPQRearrange = 1
			BEGIN 
				UPDATE CRM_CarBasicData SET PriceQuoteNotRequired = 0, IsPQMailedExternal = 0,
					IsPQRearrange = 1 WHERE Id = @CarBasicDataId
			END
		END
END












