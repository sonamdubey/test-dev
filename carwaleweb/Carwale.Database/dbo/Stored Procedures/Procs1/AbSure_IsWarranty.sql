IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[AbSure_IsWarranty]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[AbSure_IsWarranty]
GO

	-- =============================================
-- Author:        Ashwini Dhamankar
-- Create date: 18-12-2014
-- Description:    To decide whether dealer has access to request warranty for particular stock or not
-- Modified By: Ashwini Dhamankar on 2-01-2015, Added filter of Fuel Type
-- Modified By: Ashwini Dhamankar on Jan 2,2015, Fetched CarIds from Absure_CarDetails to show view report option
-- Modified By: Ashwini Dhamankar on Jan 13,2015, Commented constraints of survey date.
-- Modified By: Tejashree Patil on 24 Feb 2015, Added parameters and checked condition for @RegistrationType.
-- Modified By Tejashree Patil on 12 March 2015, Allow inspection for manufactured CNG Cars.
-- Modified By Tejashree Patil on 13 March 2015, Selected only IsActive = 1 cars, Commeneted condition of ST.IsWarrantyRequested and ST.IsWarrantyAccepted.
-- Modified By: Yuga Hatolkar on March 23rd, 2015, Added Parameter @EligibleModelFor and fetched data respectively.
--MOdified By  : Vinay Kumar Prajapati 8th May 2015 , Get only those Stock_Id  which are eligible for Inspection.  
-- exec AbSure_IsWarranty 1,5,5,'2011-03-25 11:07:56.800',95,0,1,2,2
-- Modified By : Tejashree Patil on 17 July 2015, Eligibility crieteria of 24 monthson RegistrationDate instead of makeYear .
-- Modified By : Tejashree Patil on 14 Aug 2015, Changed eligibility criteria will be 6years & 85,000kms.
-- Modified By : Ruchira Patil on 9th Oct 2015, Only those Cars which are eligible for warranty should go for inspection. 
-- =============================================
CREATE PROCEDURE [dbo].[AbSure_IsWarranty]--for individual (absure.in)
    -- Add the parameters for the stored procedure here
    @IsWarranty            BIT = 1,
    @BranchId            BIGINT,
    @Kilometer            INT = NULL,
    @MakeYear            DATETIME = NULL,
    @ModelId            INT = NULL,
    @CarFitted            TINYINT = NULL,
    @FuelType            TINYINT = NULL,
    @RegistrationType    TINYINT = 1, --1: Dealer, 2: Individual
    @EligibleModelFor    TINYINT = 1  -- 1: By default Warranty
AS
BEGIN
    -- SET NOCOUNT ON added to prevent extra result sets from
    -- interfering with SELECT statements.
    SET NOCOUNT ON;
        
    SELECT @EligibleModelFor = CASE 
                                    WHEN ISNULL(D.IsWarranty,0) = 1        THEN 1 
                                    WHEN ISNULL(D.IsInspection,0) = 1    THEN 2 
                                    WHEN ISNULL(D.IsInspection,0) = 1 AND ISNULL(D.IsWarranty,0) = 1    THEN 3 
                               END 
    FROM    Dealers D WITH(NOLOCK) 
    WHERE    Id = @BranchId

    -- Insert statements for procedure here
    IF(@IsWarranty = 1 AND ISNULL(@RegistrationType,1) = 1)--Dealer
    BEGIN
        DECLARE @StockIds VARCHAR(MAX)
        DECLARE @CarIds VARCHAR(MAX)
        DECLARE @CarInspection VARCHAR(MAX)
        -- Modified By : Tejashree Patil on 14 Aug 2015, Changed eligibility criteria will be 6years & 85,000kms.
        SELECT    @StockIds = COALESCE(@StockIds+',','') + CONVERT(VARCHAR,ST.Id )
        FROM    TC_Stock ST WITH(NOLOCK)
                INNER JOIN CarVersions V WITH(NOLOCK) ON  V.ID = ST.VersionId  
                INNER JOIN AbSure_EligibleModels AE WITH(NOLOCK) ON AE.ModelId = V.CarModelId
                INNER JOIN  TC_CarCondition CC WITH(NOLOCK) ON CC.StockId = ST.Id
        WHERE   ST.BranchId = @BranchId AND
                ST.Kms <= 85000 AND 
                --DATEDIFF(year,ST.MakeYear,GETDATE()) <= 7 AND
                (DATEDIFF(MONTH,ST.MakeYear,GETDATE()) > 24 AND DATEDIFF(MONTH,ST.MakeYear,GETDATE()) <= 72) AND
                --(ISNULL(ST.IsWarrantyRequested,0) <> 1) AND
                --(ISNULL(ST.IsWarrantyAccepted,0) <> 1) AND
                V.CarFuelType IN (1,2,3) AND
                CC.Owners <> 0
                AND AE.IsActive = 1
                AND (
                        ( @EligibleModelFor = 1 AND ISNULL(AE.IsEligibleWarranty,1) = 1) 
                        --OR ( @EligibleModelFor = 2 AND AE.IsEligibleCertification = 1) OR 
                        --( @EligibleModelFor = 3 AND (AE.IsEligibleCertification = 1 AND AE.IsEligibleWarranty = 1))
                    )
        SELECT @StockIds StockId

        
        SELECT  @CarIds = COALESCE(@CarIds+',','') + CONVERT(VARCHAR(max),ISNULL(ABC.StockId,0)) + '_' +  CONVERT(VARCHAR(max),ABC.Id )
        FROM    AbSure_CarDetails ABC WITH(NOLOCK)
        WHERE   --ABC.SurveyDate IS NOT NULL AND
                --DATEDIFF(day,ABC.SurveyDate,GETDATE()) <= 30 AND      --Commented by Ashwini Dhamankar on 13/02/2015         
                ABC.IsSurveyDone = 1 AND ABC.DealerId = @BranchId
                AND ISNULL(ABC.IsActive,1) = 1 -- Modified By Tejashree Patil on 13 March 2015
        
        -- Modified By : Tejashree Patil on 14 Aug 2015, Changed eligibility criteria will be 6years & 85,000kms.
        SELECT  @CarIds AbSure_CarDetailsId
       -- Added by Vinay Kumar prajapati , 8th may 2015.
        --Get All Stock_Id which are eligible for inspection except the "Tata Cars having make_Id 16".
        SELECT    @CarInspection = COALESCE(@CarInspection+',','') + CONVERT(VARCHAR,ST.Id )
        FROM    TC_Stock ST WITH(NOLOCK)
                INNER JOIN CarVersions V WITH(NOLOCK) ON  V.ID = ST.VersionId 
                INNER JOIN CarModels AS CM WITH(NOLOCK) ON CM.ID=V.CarModelId 
                INNER JOIN AbSure_EligibleModels AE WITH(NOLOCK) ON AE.ModelId = V.CarModelId
                INNER JOIN  TC_CarCondition CC WITH(NOLOCK) ON CC.StockId = ST.Id
        WHERE   ST.BranchId = @BranchId AND
                ST.Kms <= 85000 AND 
                (DATEDIFF(MONTH,ST.MakeYear,GETDATE()) > 24 AND DATEDIFF(MONTH,ST.MakeYear,GETDATE()) <= 72) AND
                V.CarFuelType IN (1,2,3) AND
                CC.Owners <> 0
                AND AE.IsActive = 1
                --AND ISNULL(AE.IsEligibleCertification,0) = 1  -- only for certification
                AND CM.CarMakeId <> 16 -- Remove all Tata cars 
                
            SELECT  @CarInspection AbSure_CarInspection    
          --SELECT  '610948' AbSure_CarInspection    

    END
    ELSE
    IF(@RegistrationType = 2)--Individual (absure.in)
    BEGIN
        -- Modified By : Tejashree Patil on 14 Aug 2015, Changed eligibility criteria will be 6years & 85,000kms.
        SELECT    1 IsEligible, 
                CAST(ISNULL(AE.SilverPrice,0) + (ISNULL(AE.SilverPrice,0) * 14/100) As decimal(10, 2))    SilverPrice,
                CAST(ISNULL(AE.GoldPrice,0) + (ISNULL(AE.GoldPrice,0) * 14/100) As decimal(10, 2))    GoldPrice
        FROM    AbSure_EligibleModels AE WITH(NOLOCK)
        WHERE   @Kilometer <= 85000 AND 
                --DATEDIFF(year,@MakeYear,GETDATE()) <= 7 AND
                (DATEDIFF(MONTH,@MakeYear,GETDATE()) > 24 AND DATEDIFF(MONTH,@MakeYear,GETDATE()) <= 72) AND
                @FuelType IN (1,2,3) AND 
                AE.ModelId = @ModelId AND
                ISNULL(@CarFitted,0) IN(0,4) AND--ISNULL(@CarFitted,0) = 0 AND--NOT IN (1,2) AND 
                AE.IsActive = 1
                AND (
                        ( @EligibleModelFor = 1 AND ISNULL(AE.IsEligibleWarranty,1) = 1)
                        -- OR ( @EligibleModelFor = 2 AND AE.IsEligibleCertification = 1) OR 
                        --( @EligibleModelFor = 3 AND (AE.IsEligibleCertification = 1 AND AE.IsEligibleWarranty = 1))
                    )
    END
END