IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[CW_GetUserEligibleAmount-v16_9_4]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[CW_GetUserEligibleAmount-v16_9_4]
GO

	-- =============================================
-- Author:		Mihir A Chheda
-- Create date: 01-08-2015
-- Description:	Calculate the Eligible loan amount based on user input values
-- Modified :    added new output parameter for HUB city name 
-- Modified : Rakesh Yadav on 13 Sep 2016, add new conditions like check for special offers for roi and remove city based ROI logic
-- =============================================
create PROCEDURE [dbo].[CW_GetUserEligibleAmount-v16_9_4]
    @FinanceLeadId INT = -1,
	@IncomeTypeId INT,
	@AnnualIncome NUMERIC(18,0),
	@VersionId INT,
	@UserDOB DATETIME,
	@CompanyId INT=NULL,
	@IsExistingHdfcCustomer BIT = 0,
	@CityId INT,
	@MaxEligibleAmount Float OUTPUT,
	@Tenor INT  OUTPUT,	
	@MaxTenor INT  OUTPUT,
	@LTV FLOAT OUTPUT,
	@ROI FLOAT  OUTPUT,
	@ProcessingFees FLOAT OUTPUT,
	@IsPermitted BIT OUTPUT,
	@CarPrice NUMERIC(18,0) OUTPUT
	
AS
BEGIN
	
	SET @MaxEligibleAmount=0 
	--SET @Tenor= 0 	
	SET @MaxTenor=  0
	SET @LTV= 0 
	SET @ROI= 0 
	SET @ProcessingFees= 0 
	SET @IsPermitted=0 
	SET @CarPrice = 0

   	DECLARE @Multiplier INT=NULL	
	DECLARE @LTVBasedEligibleAmt DECIMAl=0
	DECLARE @UserBasedEligibleAmt DECIMAL=0
	DECLARE @UserAge INT
	DECLARE @CW_AgeGroupId INT
	DECLARE @CarSegment INT
	DECLARE @CarTier INT
	DECLARE @ModelId INT  

	select @ModelId = CarModelId From CarVersions WITH (NOLOCK) where Id=@VersionId and New=1 and IsDeleted=0

	select @CarPrice = Price from NewCarShowroomPrices WITH (NOLOCK)  where CarVersionId=@VersionId and CityId=@CityId  and IsActive=1
	--Calculate customer age
	SET @UserAge=DATEDIFF(YEAR,CONVERT(DATE,@UserDOB),CONVERT(DATE,GETDATE()))

	--find customer age group using customer age
	SET @CW_AgeGroupId=ISNULL((SELECT cwa.Id 
						FROM   CW_AgeGroup cwa WITH (NOLOCK)
						JOIN   CW_AgeGroupIncomeMapping cwam  WITH (NOLOCK)
						ON     cwa.id=cwam.CW_AgeGroupId
						WHERE  @UserAge BETWEEN cwa.MinAge AND cwa.MaxAge AND cwam.CW_IncomeTypesId=@IncomeTypeId
						),0)

	-- fetch segment and tier of model
	SELECT  @CarSegment=CW_CarSegmentId,@CarTier=CW_CarTierId 
	FROM    CW_CarModelDetails WITH (NOLOCK)
	WHERE   CarModelId=@ModelId

	-- find multiplier using income type,age group,tier and anual income
	SELECT  @Multiplier=Multiplier 
	FROM    CW_MinIncomeMultiplier   WITH (NOLOCK)
	WHERE   CW_IncomeTypeId=@IncomeTypeId AND CW_AgeGroupId=@CW_AgeGroupId AND MinIncome<=@AnnualIncome AND CW_CarTierId=@CarTier  AND IsActive=1

	--if multiplier is not present then user is not elegible for loan 
	--else if multiplier is peresent then 
	IF(@Multiplier IS NOT NULL)
	BEGIN
	  SET @IsPermitted=1
	  
	  -- fetch LTV for perticular tenor if tenor is not given then fetch ltv and tenor for max LTV,Tenor values
	  IF(ISNULL(@Tenor,0) <> 0)--IF(@Tenor IS NOT NULL)
	  BEGIN
		  SELECT TOP 1 @LTV=LTV FROM CW_NewCarLTV WITH (NOLOCK) WHERE CarModelId=@ModelId AND Tenor=@Tenor
	  END
	  ELSE
	  BEGIN
		 SELECT TOP 1 @LTV=LTV,@Tenor=Tenor FROM CW_NewCarLTV WITH (NOLOCK) WHERE CarModelId=@ModelId ORDER BY LTV DESC,Tenor DESC		 
	  END

	  --find max tenor for model having LTV
	  SET @MaxTenor=(SELECT TOP 1 Tenor FROM CW_NewCarLTV WITH (NOLOCK) WHERE CarModelId=@ModelId AND LTV IS NOT NUll ORDER BY Tenor DESC)	 

	  --Calculate max loan amount for car
	  SET @LTVBasedEligibleAmt=(@CarPrice*(@LTV/100))
	  IF(@CarSegment >=6 )-- when to apply this???????
	  BEGIN
			SET @AnnualIncome=@AnnualIncome/2
	  END

	  --calculate loan amount customer is eligible for
	  SET @UserBasedEligibleAmt=(@AnnualIncome*@Multiplier)
	  
	  SET @MaxEligibleAmount=@UserBasedEligibleAmt

	  --decide loan amount which can be give, it will be minimum of maximum loan amount can be given for car and maximum laon amount can be given to customer
	  
	  IF(@LTVBasedEligibleAmt < @MaxEligibleAmount)
	  BEGIN
	    SET @MaxEligibleAmount=@LTVBasedEligibleAmt
	  END

	  --if customer works in Focus 100 or Focus 800 compnies
	  IF(@CompanyId IS NOT NULL AND @CompanyId <> 0)	 
	  BEGIN
		--if special offer exists for focus 100 and focus 800 companies
		SELECT  @ProcessingFees=cc.ProcessingFees,@ROI=so.ROI 
		FROM    
		CW_CompanyList cl WITH (NOLOCK) 
		JOIN CW_CompanyCategories cc WITH (NOLOCK) ON  cc.Id=cl.CW_CompanyCategoryId
		JOIN CW_FinanceSpecialOffers so WITH(NOLOCK) ON so.CW_CompanyCategoryId=cc.Id
		WHERE   cl.Id=@CompanyId 
				and @MaxEligibleAmount between so.MinLoanAmount and so.MaxLoanAmount
				and so.IsCampaignActive=1 
				and GETDATE() between so.StartDate and so.EndDate
	  END
	  ELSE -- if customer does not works in Focus 100 or Focus 800 compnies
	  BEGIN
		  --if special offer exists for non focus 100 and focus 800 company's employees
			SELECT  @ROI=so.ROI 
			FROM    
			CW_FinanceSpecialOffers so WITH(NOLOCK) 
			WHERE   @MaxEligibleAmount between so.MinLoanAmount and so.MaxLoanAmount 
					and so.CW_CompanyCategoryId=3
					and so.IsCampaignActive=1 
					and GETDATE() between so.StartDate and so.EndDate
					and @IsExistingHdfcCustomer <> 0 -- existing HDFC customer
					and @IncomeTypeId = 1 -- is salaried

			--calculate Processing fees for non focus 100 and 800 company's employee
			SET @ProcessingFees=(SELECT ProcessingFees FROM CW_CarProcessingFees WITH (NOLOCK) WHERE @MaxEligibleAmount BETWEEN MinAmount AND MaxAmount AND IsActive=1)
	  END

	  
	  IF(ISNULL(@ROI,0) = 0) -- If special offers does not exists
	  BEGIN
		SET @ROI=(SELECT ROI FROM CW_NewCarROI WITH (NOLOCK) WHERE CW_CarSegmentId=@CarSegment AND Tenor=@Tenor AND IsActive=1)
	  END
	END
	ELSE
	BEGIN
		SET @MaxEligibleAmount=0 
		SET @Tenor= 0 	
		SET @MaxTenor=  0
		SET @LTV= 0 
		SET @ROI= 0 
		SET @ProcessingFees= 0 
		SET @IsPermitted=0 

		IF ISNULL(@CarSegment, 0) = 0
		BEGIN
			UPDATE CW_FinanceLeads 
			SET    FailureReason='User is not eligible because of missing car data',
				UpdatedOn = GETDATE()
			WHERE  id=@FinanceLeadId
		END
		ELSE
		BEGIN
			UPDATE CW_FinanceLeads 
			SET    FailureReason='User is not eligible because of his income',
				UpdatedOn = GETDATE()
			WHERE  id=@FinanceLeadId
		END
	END
	IF @ROI IS NULL
	BEGIN
		SET @ROI = 0
		SET @IsPermitted=0 

		UPDATE CW_FinanceLeads 
		SET    FailureReason='User is not eligible because car data is not present or city is not valid',
				UpdatedOn = GETDATE()
		WHERE  id=@FinanceLeadId
	END
END


