IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[AbSure_CheckEligibility]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[AbSure_CheckEligibility]
GO

	-- =============================================
-- Author: Vinay Kumar Prajapati 
-- Purpose  : To Get eligible warranty  option . 
-- exec AbSure_CheckEligibility 708960,422,'HR 51'
-- Modified By : Ashwini Dhamankar on Oct 5,2015 (Added @RegNo)
-- Modified By : Ruchira Patil on 9th Oct 2015, Only those Cars which are eligible for warranty should go for inspection. 
-- Modified By : Ruchira Patil on 23rd Oct 2015, allowed inspection for selected dealers 
-- =============================================
CREATE PROCEDURE [dbo].[AbSure_CheckEligibility] --for dealers autobiz.
(
	-- Add the parameters for the stored procedure here
	@StockId	  BIGINT,
	@BranchId     BIGINT ,
	@RegNumber VARCHAR(50) = NULL
)	
AS  
    DECLARE @IsEligible         VARCHAR(2)='0'  -- 1 for Eligible  and 0 for not eligible
	DECLARE @EligibleFor        VARCHAR(2)='0' -- 1 for warranty ,2 for Inspection , 0 for NA
	DECLARE @ReportId           VARCHAR(15) = '-1'  --Give Option For report
	DECLARE @CarDetailsId       VARCHAR(15) = '-1' -- this is is use for view certification 

	DECLARE @IsDealerWarranty   BIT   =  0
	DECLARE @IsDealerInspection BIT   =  0
	DECLARE @IsCarWarranty      BIT   =  0
	DECLARE @IsCarInspection    BIT   =  0

BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	SELECT @IsDealerInspection = ISNULL(D.IsInspection,1), @IsDealerWarranty=ISNULL(D.IsWarranty,0) 
	FROM Dealers AS D WITH(NOLOCK)  WHERE D.ID = @BranchId

	DECLARE @CarId BIGINT, @MappedRegNo VARCHAR(50),@MasterCarId BIGINT,@Status INT,@CancelReason VARCHAR(250),@IsDuplicateCar BIT = 0,@CancelledReason VARCHAR(250)
	SELECT @CancelledReason = Reason FROM AbSure_ReqCancellationReason WITH(NOLOCK) WHERE Id = 7;

	SELECT	@MappedRegNo = RegNumber,@CarId = Id,@Status = Status,@CancelReason = CancelReason
	FROM	AbSure_CarDetails WITH(NOLOCK)
	WHERE	StockId = @StockId

	IF(@Status = 3 AND @CancelReason = @CancelledReason)
            BEGIN
                SET @IsDuplicateCar = 1
            END
	
	--IF EXISTS (SELECT AbSure_StockRegNumberMappingId FROM AbSure_StockRegNumberMapping WITH(NOLOCK) WHERE RegistrationNumber = @MappedRegNo)
	IF(@IsDuplicateCar = 1)
	BEGIN
		SELECT @MasterCarId = dbo.Absure_GetMasterCarId(@MappedRegNo,@CarId)  

		SELECT	@BranchId = DealerId,@StockId = StockId
		FROM	AbSure_CarDetails ACD WITH(NOLOCK)
		WHERE   ACD.Id = @MasterCarId
	END

	--Check Dealer is Eligible for Warranty  or Inspection
	IF @IsDealerInspection = 1 OR @IsDealerWarranty = 1
		BEGIN
			
			SELECT	 @IsCarWarranty = ISNULL(AE.IsEligibleWarranty,0), @IsCarInspection = ISNULL(AE.IsEligibleCertification,0)
			FROM	TC_Stock ST WITH(NOLOCK)
					INNER JOIN CarVersions V WITH(NOLOCK) ON  V.ID = ST.VersionId  
					INNER JOIN AbSure_EligibleModels AE WITH(NOLOCK) ON AE.ModelId = V.CarModelId
					INNER JOIN  TC_CarCondition CC WITH(NOLOCK) ON CC.StockId = ST.Id
			WHERE   ST.RegNo = @RegNumber AND ST.Id = @StockId AND
					--ST.Kms <= 98000 AND 
					--(DATEDIFF(MONTH,ST.MakeYear,GETDATE()) >= 20 AND DATEDIFF(MONTH,ST.MakeYear,GETDATE()) <= 84) AND
					--V.CarFuelType IN (1,2,3) AND
					--CC.Owners <> 0 AND 
					AE.IsActive = 1

			IF @@ROWCOUNT <> 0
				BEGIN

				IF @IsCarWarranty=1 AND @IsDealerWarranty=1 AND @IsDealerInspection=1 -- Car warranty and dealer eligible for  warranty And inspection Both
						BEGIN
						SET @IsEligible='1'
						SET @EligibleFor='1'
						END
      --           ELSE IF @IsCarInspection=1 AND @IsDealerWarranty=1 AND @IsDealerInspection=1-- Car inspection and dealer eligible for  warranty And inspection Both
						--BEGIN
						--SET @IsEligible='1'
						--SET @EligibleFor='2'
						--END
				    
					ELSE IF @IsCarWarranty=1 AND @IsDealerWarranty=1 -- Car waranty and dealer warranty
						BEGIN
						SET @IsEligible='1'
						SET @EligibleFor='1'
						END
					ELSE IF @BranchId IN (SELECT DealerId FROM AbSure_EligibleDealerForInspection WITH (NOLOCK) WHERE Isactive = 1) AND (@IsCarInspection =1 OR @IsCarWarranty = 1)-- allow inspection for some dealers hvaing entry in AbSure_EligibleDealerForInspection
						BEGIN
							SET @IsEligible='1'
							SET @EligibleFor='1'
						END
					--ELSE IF @IsCarWarranty=1 AND @IsDealerInspection=1 -- Car warranty and dealer inspection 
					--		BEGIN
					--			SET @IsEligible='1'
					--			SET @EligibleFor='2'
					--		END
					--ELSE IF @IsCarInspection=1 AND @IsDealerWarranty=1 --Car inspection and dealer warranty
					--	BEGIN
					--		SET @IsEligible='2'
					--		SET @EligibleFor='0'
					--	END
					--ELSE IF @IsCarInspection=1 AND @IsDealerInspection=1 --Car inspection and dealer inspection
					--BEGIN
					--	SET @IsEligible='1'
					--	SET @EligibleFor='2'
					--END
          END 

		  -- For Report Link Take CardetailsId
		    SELECT  @ReportId= CONVERT(VARCHAR(10),ISNULL(ABC.Id,-1))
			FROM    AbSure_CarDetails ABC WITH(NOLOCK)
			WHERE ABC.IsSurveyDone = 1 AND (ABC.StockId=@StockId OR ((ABC.RegNumber = @MappedRegNo) AND @IsDuplicateCar = 1)) AND ISNULL(ABC.IsActive,1) = 1 

			-- For warranty option  Link Take CardetailsId
		    SELECT  @CarDetailsId = CONVERT(VARCHAR(10),ISNULL(ABC.Id,-1))
			FROM    AbSure_CarDetails ABC WITH(NOLOCK)
			WHERE   (ABC.StockId=@StockId OR ABC.RegNumber = @MappedRegNo) AND ISNULL(ABC.IsActive,1) = 1 
	

			

		  SELECT   @IsEligible  IsEligible
		  SELECT   @EligibleFor EligibleFor
		  SELECT   @ReportId    ReportId
		  SELECT   @CarDetailsId  CarId
		  SELECT   @IsDuplicateCar IsDuplicateCar
    END
END
