IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[CW_GetUserEligibilityCriteria]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[CW_GetUserEligibilityCriteria]
GO

	
-- =============================================
-- Author:		Supreksha
-- Create date: 26-10-2016
-- Description:	Check for loan eligibility 
-- =============================================
CREATE PROCEDURE [dbo].[CW_GetUserEligibilityCriteria]
    @StabilityTime SMALLINT,
    @ResidenceTypeId SMALLINT,
	@IncomeTypeId INT = NULL,
	@UserDOB DATETIME = NULL,
	@AnnualIncome NUMERIC(18,0) = NULL,
	@CustomerExp SMALLINT = NULL	
	
AS
BEGIN

	DECLARE @UserAge INT=NULL
	DECLARE @CW_AgeGroupId INT=NULL
	DECLARE @IsPermitted BIT = 0
	DECLARE @MinIncomeTable INT = NULL
	
	SET @MinIncomeTable = ISNULL((SELECT MIN(MinIncome) FROM   CW_MinIncomeMultiplier WITH (NOLOCK)),NULL)
	IF(@UserDOB IS NOT NULL)
	BEGIN
	    SET @UserAge=DATEDIFF(YEAR,CONVERT(DATE,@UserDOB),CONVERT(DATE,GETDATE()))

		SET @CW_AgeGroupId=ISNULL((SELECT cwa.Id 
						FROM   CW_AgeGroup cwa WITH (NOLOCK)
						JOIN   CW_AgeGroupIncomeMapping cwam  WITH (NOLOCK)
						ON     cwa.id=cwam.CW_AgeGroupId
						WHERE  @UserAge BETWEEN cwa.MinAge AND cwa.MaxAge AND cwam.CW_IncomeTypesId=@IncomeTypeId
						),-1)
	END

	IF(@ResidenceTypeId IS NOT NULL AND @StabilityTime IS NOT NULL)
	BEGIN
       IF EXISTS (SELECT 1
	      FROM CW_MinIncomeMultiplier  M  WITH (NOLOCK)
          JOIN CW_ResidenceType R WITH (NOLOCK) ON M.ResidenceTypeId=R.ID	
          JOIN CW_IncomeTypes I WITH (NOLOCK) ON M.CW_IncomeTypeId=I.Id
          WHERE   
	      (@IncomeTypeId IS NULL OR @IncomeTypeId =0 OR M.CW_IncomeTypeId=@IncomeTypeId)
          AND (@CW_AgeGroupId IS NULL OR M.CW_AgeGroupId=@CW_AgeGroupId)
          AND (@AnnualIncome IS NULL OR @AnnualIncome = 0 OR (@MinIncomeTable <= @AnnualIncome AND @CW_AgeGroupId IS NULL) OR (M.MinIncome <= @AnnualIncome AND M.CW_AgeGroupId=@CW_AgeGroupId))
	      AND (@CustomerExp IS NULL OR  I.Minexperience <= @CustomerExp)
          AND M.IsActive=1
          AND M.ResidenceTypeId = @ResidenceTypeId
          AND R.MinStability <= @StabilityTime)
	   BEGIN
		  SET @IsPermitted=1
	   END	      
	END
	SELECT @IsPermitted as IsPermitted
END
