IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[CW_GetUserEligibleAmount]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[CW_GetUserEligibleAmount]
GO

	-- =============================================
-- Author:		Mihir A Chheda
-- Create date: 01-08-2015
-- Description:	Calculate the Eligible loan amount based on user input values
-- Modified :    added new output parameter for HUB city name 
-- =============================================
CREATE PROCEDURE [dbo].[CW_GetUserEligibleAmount]
    @FinanceLeadId INT = -1,
	@IncomeTypeId INT,
	@AnnualIncome NUMERIC(18,0),
	@ModelId NUMERIC(18,0)=NULL,
	@CarPrice NUMERIC(18,0)=NULL,
	@UserDOB DATETIME,
	@CityId INT,
	@CompanyId INT=NULL,
	@MaxEligibleAmount Float OUTPUT,
	@Tenor INT  OUTPUT,	
	@MaxTenor INT  OUTPUT,
	@LTV FLOAT OUTPUT,
	@ROI FLOAT  OUTPUT,
	@ProcessingFees FLOAT OUTPUT,
	@HubCityName VARCHAR(50) OUTPUT,
	@IsPermitted BIT OUTPUT
	
AS
BEGIN
	
	SET @MaxEligibleAmount=0 
	SET @Tenor= 0 	
	SET @MaxTenor=  0
	SET @LTV= 0 
	SET @ROI= 0 
	SET @ProcessingFees= 0 
	SET @IsPermitted=0 

   	DECLARE @Multiplier INT=NULL	
	DECLARE @LTVBasedEligibleAmt DECIMAl=0
	DECLARE @UserBasedEligibleAmt DECIMAL=0
	DECLARE @UserAge INT
	DECLARE @CW_AgeGroupId INT
	DECLARE @CarSegment INT
	DECLARE @CarTier INT 
	DECLARE @CityCategoryId INT
	print @CompanyId
	SET @UserAge=DATEDIFF(YEAR,CONVERT(DATE,@UserDOB),CONVERT(DATE,GETDATE()))
	SET @CW_AgeGroupId=ISNULL((SELECT cwa.Id 
						FROM   CW_AgeGroup cwa WITH (NOLOCK)
						JOIN   CW_AgeGroupIncomeMapping cwam  WITH (NOLOCK)
						ON     cwa.id=cwam.CW_AgeGroupId
						WHERE  @UserAge BETWEEN cwa.MinAge AND cwa.MaxAge AND cwam.CW_IncomeTypesId=@IncomeTypeId
						),0)

	SELECT  @CarSegment=CW_CarSegmentId,@CarTier=CW_CarTierId 
	FROM    CW_CarModelDetails WITH (NOLOCK)
	WHERE   CarModelId=@ModelId

	SELECT  @CityCategoryId=CatId,@HubCityName=cc.Name
	FROM    CW_CarCities hc WITH (NOLOCK)
	JOIN    Cities cc  ON cc.ID=hc.CW_CityId
	WHERE   SpokeCityId=@CityId

	SELECT  @Multiplier=Multiplier 
	FROM    CW_MinIncomeMultiplier   WITH (NOLOCK)
	WHERE   CW_IncomeTypeId=@IncomeTypeId AND CW_AgeGroupId=@CW_AgeGroupId AND MinIncome<=@AnnualIncome AND CW_CarTierId=@CarTier  AND IsActive=1

	IF(@Multiplier IS NOT NULL)
	BEGIN
	  SET @IsPermitted=1

	  IF(ISNULL(@Tenor,0) <> 0)--IF(@Tenor IS NOT NULL)
	  BEGIN
		  SELECT TOP 1 @LTV=LTV FROM CW_NewCarLTV WITH (NOLOCK) WHERE CarModelId=@ModelId AND Tenor=@Tenor
	  END
	  ELSE
	  BEGIN
		 SELECT TOP 1 @LTV=LTV,@Tenor=Tenor FROM CW_NewCarLTV WITH (NOLOCK) WHERE CarModelId=@ModelId ORDER BY LTV DESC,Tenor DESC		 
	  END
	  SET @MaxTenor=(SELECT TOP 1 Tenor FROM CW_NewCarLTV WITH (NOLOCK) WHERE CarModelId=@ModelId AND LTV IS NOT NUll ORDER BY Tenor DESC)	 
	  SET @LTVBasedEligibleAmt=(@CarPrice*(@LTV/100))
	  IF(@CarSegment >=5 )
	  BEGIN
			SET @AnnualIncome=@AnnualIncome/2
	  END
	  SET @UserBasedEligibleAmt=(@AnnualIncome*@Multiplier)
	  
	  SET @MaxEligibleAmount=@UserBasedEligibleAmt

	  IF(@LTVBasedEligibleAmt < @MaxEligibleAmount)
	  BEGIN
	    SET @MaxEligibleAmount=@LTVBasedEligibleAmt
	  END

	  IF(@CompanyId IS NOT NULL AND @CompanyId <> 0)	 
	  BEGIN
		SELECT  @ProcessingFees=cc.ProcessingFees,@ROI=cc.ROI 
		FROM    CW_CompanyList cl WITH (NOLOCK)
		JOIN    CW_CompanyCategories cc WITH (NOLOCK)
		ON      cc.Id=cl.CW_CompanyCategoryId
		WHERE   cl.Id=@CompanyId		
	  END
	  IF(ISNULL(@ProcessingFees,0) = 0)--IF(@ProcessingFees IS NULL OR @ProcessingFees=0)
	  BEGIN
		SET @ProcessingFees=(SELECT ProcessingFees FROM CW_CarProcessingFees WITH (NOLOCK) WHERE @MaxEligibleAmount BETWEEN MinAmount AND MaxAmount AND IsActive=1)
		SET @ROI=(SELECT ROI FROM CW_NewCarROI WITH (NOLOCK) WHERE CW_CarSegmentId=@CarSegment AND CW_CityCategoryId=@CityCategoryId AND Tenor=@Tenor AND IsActive=1)
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
