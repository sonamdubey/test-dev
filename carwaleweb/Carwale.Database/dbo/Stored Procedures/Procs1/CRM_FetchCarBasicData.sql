IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[CRM_FetchCarBasicData]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[CRM_FetchCarBasicData]
GO

	CREATE PROCEDURE [dbo].[CRM_FetchCarBasicData]

	@CarBasicDataId			Numeric,
	@LeadId					Numeric OutPut,
	@CarMakeId				INT OutPut,
	@CarVersionId			Numeric OutPut,
	@CarName				VarChar(250) OutPut,
	@ExShowroomPrice		Numeric OutPut,
	@Insurance				Numeric OutPut,
	@RTO					Numeric OutPut,
	@PQId					Numeric OutPut,
	@CityId					Numeric OutPut,
	@CityName				VarChar(50) OutPut,
	@UpdatedById			Numeric OutPut,
	@UpdatedByName			VarChar(100) OutPut,
	@LeadSourceId			Numeric OutPut,
	@SourceCatId			Numeric OutPut,

	@IsProductExplained		Bit OutPut,
	@IsPQMailed				Bit OutPut,
	@IsPQMailedExternal		Bit OutPut,
	@IsPQMailInternalReq	Bit OutPut,
	@IsPQMailExternalReq	Bit OutPut,
	@IsFinalized			Bit OutPut,
	@PriceQuoteNotRequired	Bit OutPut,
	@IsPENotRequired		Bit OutPut,
	
	@ExpectedBuyingDate		DateTime OutPut,
	@CreatedOn				DateTime OutPut,
	@UpdatedOn				DateTime OutPut,
	@PQStatus				Numeric OutPut,
	
	@PQRequestDate			DateTime OutPut,
	@PQCompleteDate			DateTime OutPut,
	
	@PEStatus				Int OutPut,
	@PECompleteDate			DateTime OutPut,
	@CarModelId             INT = NULL OUTPUT
				
 AS
	DECLARE @ISPECompleted AS BIT
	
	
BEGIN

	SELECT	
		@LeadId				= CBD.LeadId,
		@CarVersionId		= VersionId,
		@CarMakeId			= CMA.Id,
		@CarModelId         = CMO.Id,
		@CarName			= (CMA.Name + ' ' + CMO.Name + ' ' + CV.Name),
		@ExShowroomPrice	= ExShowroom,
		@Insurance			= Insurance,
		@RTO				= RTO,
		@PQId				= PQId,
		@CityId				= C.Id,
		@CityName			= C.Name,
		@UpdatedById		= UpdatedBy,
		@UpdatedByName		= OU.UserName,
		@LeadSourceId		= CLS.SourceId,
		@SourceCatId		= CLS.CategoryId,
		
		@IsProductExplained	= IsProductExplained,
		@IsPQMailed			= IsPQMailed,
		@IsPQMailedExternal = IsPQMailedExternal,
		@IsPQMailInternalReq= IsPQMailInternalReq,
		@IsPQMailExternalReq= IsPQMailExternalReq,
		@IsFinalized		= IsFinalized,
		@PriceQuoteNotRequired = PriceQuoteNotRequired,
		@IsPENotRequired		= IsPENotRequired,

		@ExpectedBuyingDate	= ExpectedBuyingDate,
		@CreatedOn			= CreatedOn,
		@UpdatedOn			= UpdatedOn,
		@PQStatus			= (SELECT Top 1 EventType FROM CRM_EventLogs WITH(NOLOCK) WHERE ItemId = @CarBasicDataId AND EventType IN(6,36,43,44,49,55) ORDER BY Id DESC)
		

	FROM ((((((CRM_CarBasicData AS CBD WITH(NOLOCK) LEFT JOIN CarVersions AS CV WITH(NOLOCK) ON CBD.VersionId = CV.Id)
			LEFT JOIN CarModels AS CMO WITH(NOLOCK) ON CV.CarModelId = CMO.Id)
			LEFT JOIN CarMakes AS CMA WITH(NOLOCK) ON CMO.CarMakeId = CMA.Id) 
			LEFT JOIN OprUsers AS OU WITH(NOLOCK) ON CBD.UpdatedBy = OU.Id)
			LEFT JOIN Cities AS C WITH(NOLOCK) ON CBD.CityId = C.Id)
			LEFT JOIN CRM_LeadSource AS CLS WITH(NOLOCK) ON CBD.LeadId = CLS.LeadId)

	WHERE CBD.Id = @CarBasicDataId
	
	SELECT Top 1 @PQRequestDate = PQRequestDate, @PQCompleteDate = PQCompleteDate
	FROM CRM_CarPQLog WITH(NOLOCK) WHERE CBDId = @CarBasicDataId ORDER BY ID DESC
	
	SELECT Top 1 @PECompleteDate = PECompleteDate, 
	@IsPENotRequired = IsPENotRequired,  @ISPECompleted = IsPECompleted
	FROM CRM_CarPELog WITH(NOLOCK) WHERE CBDId = @CarBasicDataId ORDER BY ID DESC
	
	IF @@ROWCOUNT <> 0
		BEGIN
			SET @PEStatus = 59
				
			IF @IsPENotRequired = 1
				SET @PEStatus = 58
			ELSE IF @ISPECompleted = 1
				SET @PEStatus = 5
		END
END










