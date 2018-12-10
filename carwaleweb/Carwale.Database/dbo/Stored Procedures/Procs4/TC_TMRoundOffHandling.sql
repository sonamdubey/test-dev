IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_TMRoundOffHandling]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_TMRoundOffHandling]
GO

	-- =============================================
-- Author	    :	Manish Chourasiya
-- Create date	:	18-11-2013
-- Description	:	Get Pagewise details for Target Management 	
--Edited By Deepak on 7th Dec 2013, Included Year for Area manager approaval
 -- =============================================
CREATE PROCEDURE [dbo].[TC_TMRoundOffHandling] 
	@ActualTarget INT,
	@TC_BrandZoneId TINYINT=NULL,
	@CarModelId INT=NULL,
	@TC_AMId INT=NULL,
	@DealerId INT =NULL,
	@CarVersionId INT =NULL,
	@StartMonth TINYINT=NULL,
	@EndMonth TINYINT =NULL,
	@MonthId TINYINT=NULL,
	@TC_TMDistributionPatternMasterId INT =NULL,
	@IsTargetChangeFromAM BIT=0,
	@Year INT = NULL
AS
	BEGIN 

	/*set @StartMonth=1;
	set @EndMonth=12;*/

	 DECLARE @TotalTargerNeedtoAdjust INT,
             @TargetAfterRoundOff INT

  IF (@TC_TMDistributionPatternMasterId IS NULL AND @IsTargetChangeFromAM=0)
    BEGIN 

		SELECT @TargetAfterRoundOff=SUM(TM.Target)
		FROM  [TC_TMIntermediateLegacyDetail]  AS TM WITH(NOLOCK)
						 JOIN  Dealers AS D WITH(NOLOCK) ON D.ID=TM.DealerId
						 JOIN  TC_BrandZone  AS TCB WITH(NOLOCK) ON TCB.TC_BrandZoneId=D.TC_BrandZoneId
						 JOIN  CarVersions AS CV WITH(NOLOCK) ON CV.ID=TM.CarVersionId
						 JOIN  CarModels AS CM WITH(NOLOCK) ON CV.CarModelId=CM.ID
						-- JOIN TC_VersionsCode AS VC WITH(NOLOCK) ON VC.CarVersionId=CV.ID
						 JOIN TC_SpecialUsers AS S WITH(NOLOCK) ON S.TC_SpecialUsersId=D.TC_AMId
						 WHERE 
							(D.TC_BrandZoneId=@TC_BrandZoneId OR @TC_BrandZoneId IS NULL)
						AND (CM.Id=@CarModelId OR @CarModelId IS NULL)
						AND (CV.Id=@CarVersionId OR @CarVersionId IS NULL)
						AND (D.TC_AMId=@TC_AMId  OR @TC_AMId IS NULL)
						AND (D.ID=@DealerId OR @DealerId IS NULL)
						AND TM.[Month] BETWEEN @StartMonth AND @EndMonth
						AND (TM.[Month]=@MonthId OR @MonthId IS NULL)

	 PRINT 'TEST'
	 PRINT @TargetAfterRoundOff

	WHILE ((@ActualTarget-@TargetAfterRoundOff)<>0)
	BEGIN

	   SET @TotalTargerNeedtoAdjust = @TargetAfterRoundOff-@ActualTarget

	    IF (@TotalTargerNeedtoAdjust<>0)
	   BEGIN 
		
		PRINT @TotalTargerNeedtoAdjust;

			WITH CTE AS 
			(
			  SELECT  TOP ( ABS(@TotalTargerNeedtoAdjust)) TM.TC_TMIntermediateLegacyDetailId , 
							CASE WHEN ( TM.Target- (1* SIGN(@TotalTargerNeedtoAdjust)))<0 THEN 0
							ELSE  ( TM.Target- (1* SIGN(@TotalTargerNeedtoAdjust))) END  AS RevisedTarget
			  FROM  [TC_TMIntermediateLegacyDetail]  AS TM WITH(NOLOCK)
						 JOIN  Dealers AS D WITH(NOLOCK) ON D.ID=TM.DealerId
						 JOIN  TC_BrandZone  AS TCB WITH(NOLOCK) ON TCB.TC_BrandZoneId=D.TC_BrandZoneId
						 JOIN  CarVersions AS CV WITH(NOLOCK) ON CV.ID=TM.CarVersionId
						 JOIN  CarModels AS CM  WITH(NOLOCK) ON CV.CarModelId=CM.ID
						-- JOIN TC_VersionsCode AS VC WITH(NOLOCK)  ON VC.CarVersionId=CV.ID
						 JOIN TC_SpecialUsers AS S WITH(NOLOCK) ON S.TC_SpecialUsersId=D.TC_AMId
						 WHERE 
							(D.TC_BrandZoneId=@TC_BrandZoneId OR @TC_BrandZoneId IS NULL)
						AND (CM.Id=@CarModelId OR @CarModelId IS NULL)
						AND (D.TC_AMId=@TC_AMId  OR @TC_AMId IS NULL)
						AND (D.ID=@DealerId OR @DealerId IS NULL)
						AND (CV.Id=@CarVersionId OR @CarVersionId IS NULL)
						AND TM.[Month] BETWEEN @StartMonth AND @EndMonth
						AND (TM.[Month]=@MonthId OR @MonthId IS NULL)
						ORDER BY TM.Target DESC,TM.[Month]  --,TM.DealerId
			)  UPDATE  TM   SET TM.Target =CTE.RevisedTarget
			   FROM  [TC_TMIntermediateLegacyDetail]  AS TM 
				JOIN CTE  ON TM.TC_TMIntermediateLegacyDetailId=CTE.TC_TMIntermediateLegacyDetailId
	    END  

		SELECT @TargetAfterRoundOff=SUM(TM.Target)
		FROM  [TC_TMIntermediateLegacyDetail]  AS TM WITH(NOLOCK) 
						 JOIN  Dealers AS D WITH(NOLOCK)  ON D.ID=TM.DealerId
						 JOIN  TC_BrandZone  AS TCB WITH(NOLOCK)  ON TCB.TC_BrandZoneId=D.TC_BrandZoneId
						 JOIN  CarVersions AS CV WITH(NOLOCK) ON CV.ID=TM.CarVersionId
						 JOIN  CarModels AS CM WITH(NOLOCK) ON CV.CarModelId=CM.ID
						 --JOIN TC_VersionsCode AS VC WITH(NOLOCK) ON VC.CarVersionId=CV.ID
						 JOIN TC_SpecialUsers AS S WITH(NOLOCK) ON S.TC_SpecialUsersId=D.TC_AMId
						 WHERE 
							(D.TC_BrandZoneId=@TC_BrandZoneId OR @TC_BrandZoneId IS NULL)
						AND (CM.Id=@CarModelId OR @CarModelId IS NULL)
						AND (D.TC_AMId=@TC_AMId  OR @TC_AMId IS NULL)
						AND (D.ID=@DealerId OR @DealerId IS NULL)
						AND (CV.Id=@CarVersionId OR @CarVersionId IS NULL)
						AND TM.[Month] BETWEEN @StartMonth AND @EndMonth
						AND (TM.[Month]=@MonthId OR @MonthId IS NULL)


						print @TargetAfterRoundOff

						
	  END
	END 
  ELSE IF ((@TC_TMDistributionPatternMasterId IS NOT NULL AND @IsTargetChangeFromAM=0))
   
    BEGIN 

		SELECT @TargetAfterRoundOff=SUM(TM.Target)
		FROM  TC_TMTargetScenarioDetail  AS TM WITH(NOLOCK)
						 JOIN  Dealers AS D  WITH(NOLOCK) ON D.ID=TM.DealerId
						 JOIN  TC_BrandZone  AS TCB  WITH(NOLOCK) ON TCB.TC_BrandZoneId=D.TC_BrandZoneId
						 JOIN  CarVersions AS CV WITH(NOLOCK) ON CV.ID=TM.CarVersionId
						 JOIN  CarModels AS CM WITH(NOLOCK) ON CV.CarModelId=CM.ID
						-- JOIN TC_VersionsCode AS VC WITH(NOLOCK) ON VC.CarVersionId=CV.ID
						 JOIN TC_SpecialUsers AS S WITH(NOLOCK) ON S.TC_SpecialUsersId=D.TC_AMId
						 WHERE 
							(D.TC_BrandZoneId=@TC_BrandZoneId OR @TC_BrandZoneId IS NULL)
						AND (CM.Id=@CarModelId OR @CarModelId IS NULL)
						AND (D.TC_AMId=@TC_AMId  OR @TC_AMId IS NULL)
						AND (D.ID=@DealerId OR @DealerId IS NULL)
						AND (CV.Id=@CarVersionId OR @CarVersionId IS NULL)
						AND TM.[Month] BETWEEN @StartMonth AND @EndMonth
						AND (TM.[Month]=@MonthId OR @MonthId IS NULL)
						AND TC_TMDistributionPatternMasterId=@TC_TMDistributionPatternMasterId

	 PRINT 'TEST'
	 PRINT @TargetAfterRoundOff

	WHILE ((@ActualTarget-@TargetAfterRoundOff)<>0)
	BEGIN

	   SET @TotalTargerNeedtoAdjust = @TargetAfterRoundOff-@ActualTarget

	    IF (@TotalTargerNeedtoAdjust<>0)
	   BEGIN 
		
		PRINT @TotalTargerNeedtoAdjust;

			WITH CTE AS 
			(
			  SELECT  TOP ( ABS(@TotalTargerNeedtoAdjust)) TM.TC_TMTargetScenarioDetailId , 
							 CASE WHEN ( TM.Target- (1* SIGN(@TotalTargerNeedtoAdjust)))<0 THEN 0
							ELSE  ( TM.Target- (1* SIGN(@TotalTargerNeedtoAdjust))) END  AS RevisedTarget
			  FROM  TC_TMTargetScenarioDetail  AS TM
						 JOIN  Dealers AS D WITH(NOLOCK) ON D.ID=TM.DealerId
						 JOIN  TC_BrandZone  AS TCB WITH(NOLOCK)  ON TCB.TC_BrandZoneId=D.TC_BrandZoneId
						 JOIN  CarVersions AS CV WITH(NOLOCK) ON CV.ID=TM.CarVersionId
						 JOIN  CarModels AS CM WITH(NOLOCK) ON CV.CarModelId=CM.ID
						-- JOIN TC_VersionsCode AS VC WITH(NOLOCK) ON VC.CarVersionId=CV.ID
						 JOIN TC_SpecialUsers AS S WITH(NOLOCK) ON S.TC_SpecialUsersId=D.TC_AMId
						 WHERE 
							(D.TC_BrandZoneId=@TC_BrandZoneId OR @TC_BrandZoneId IS NULL)
						AND (CM.Id=@CarModelId OR @CarModelId IS NULL)
						AND (D.TC_AMId=@TC_AMId  OR @TC_AMId IS NULL)
						AND (D.ID=@DealerId OR @DealerId IS NULL)
						AND (CV.Id=@CarVersionId OR @CarVersionId IS NULL)
						AND TM.[Month] BETWEEN @StartMonth AND @EndMonth
						AND (TM.[Month]=@MonthId OR @MonthId IS NULL)
						AND TC_TMDistributionPatternMasterId=@TC_TMDistributionPatternMasterId
						ORDER BY TM.Target DESC,TM.[Month]  --,TM.DealerId
			)  UPDATE  TM   SET TM.Target =CTE.RevisedTarget
			   FROM  TC_TMTargetScenarioDetail  AS TM 
				JOIN CTE  ON TM.TC_TMTargetScenarioDetailId=CTE.TC_TMTargetScenarioDetailId
	    END  

		SELECT @TargetAfterRoundOff=SUM(TM.Target)
		FROM  TC_TMTargetScenarioDetail  AS TM WITH(NOLOCK) 
						 JOIN  Dealers AS D WITH(NOLOCK) ON D.ID=TM.DealerId
						 JOIN  TC_BrandZone  AS TCB WITH(NOLOCK)  ON TCB.TC_BrandZoneId=D.TC_BrandZoneId
						 JOIN  CarVersions AS CV WITH(NOLOCK) ON CV.ID=TM.CarVersionId
						 JOIN  CarModels AS CM WITH(NOLOCK)  ON CV.CarModelId=CM.ID
						 --JOIN TC_VersionsCode AS VC WITH(NOLOCK)  ON VC.CarVersionId=CV.ID
						 JOIN TC_SpecialUsers AS S WITH(NOLOCK) ON S.TC_SpecialUsersId=D.TC_AMId
						 WHERE 
							(D.TC_BrandZoneId=@TC_BrandZoneId OR @TC_BrandZoneId IS NULL)
						AND (CM.Id=@CarModelId OR @CarModelId IS NULL)
						AND (D.TC_AMId=@TC_AMId  OR @TC_AMId IS NULL)
						AND (D.ID=@DealerId OR @DealerId IS NULL)
						AND (CV.Id=@CarVersionId OR @CarVersionId IS NULL)
						AND TM.[Month] BETWEEN @StartMonth AND @EndMonth
						AND (TM.[Month]=@MonthId OR @MonthId IS NULL)
						AND TC_TMDistributionPatternMasterId=@TC_TMDistributionPatternMasterId


						print @TargetAfterRoundOff

						
	  END
	 

	END 

  ELSE IF (@IsTargetChangeFromAM=1)
    BEGIN 
    	SELECT @TargetAfterRoundOff=SUM(TM.Target)
		FROM  TC_TMAMTargetChangeApprovalReq  AS TM WITH(NOLOCK) 
						 JOIN  Dealers AS D WITH(NOLOCK) ON D.ID=TM.DealerId
						 JOIN  TC_BrandZone  AS TCB WITH(NOLOCK) ON TCB.TC_BrandZoneId=D.TC_BrandZoneId
						 JOIN  CarVersions AS CV WITH(NOLOCK)  ON CV.ID=TM.CarVersionId
						 JOIN  CarModels AS CM WITH(NOLOCK) ON CV.CarModelId=CM.ID
						-- JOIN TC_VersionsCode AS VC WITH(NOLOCK) ON VC.CarVersionId=CV.ID
						 JOIN TC_SpecialUsers AS S WITH(NOLOCK)  ON S.TC_SpecialUsersId=D.TC_AMId
						 WHERE 
							(D.TC_BrandZoneId=@TC_BrandZoneId OR @TC_BrandZoneId IS NULL)
						AND (CM.Id=@CarModelId OR @CarModelId IS NULL)
						AND (CV.Id=@CarVersionId OR @CarVersionId IS NULL)
						AND (D.TC_AMId=@TC_AMId  OR @TC_AMId IS NULL)
						AND (D.ID=@DealerId OR @DealerId IS NULL)
						AND TM.[Month] BETWEEN @StartMonth AND @EndMonth
						AND (TM.[Month]=@MonthId OR @MonthId IS NULL)
						AND TM.Year = @Year

	 PRINT 'TEST'
	 PRINT @TargetAfterRoundOff

	WHILE ((@ActualTarget-@TargetAfterRoundOff)<>0)
	BEGIN

	   SET @TotalTargerNeedtoAdjust = @TargetAfterRoundOff-@ActualTarget

	    IF (@TotalTargerNeedtoAdjust<>0)
	   BEGIN 
		
		PRINT @TotalTargerNeedtoAdjust;

			WITH CTE AS 
			(
			  SELECT  TOP ( ABS(@TotalTargerNeedtoAdjust)) TM.TC_TMAMTargetChangeApprovalReqId , 
							 CASE WHEN ( TM.Target- (1* SIGN(@TotalTargerNeedtoAdjust)))<0 THEN 0
							ELSE  ( TM.Target- (1* SIGN(@TotalTargerNeedtoAdjust))) END  AS RevisedTarget
			  FROM  TC_TMAMTargetChangeApprovalReq  AS TM WITH(NOLOCK)
						 JOIN  Dealers AS D  WITH(NOLOCK) ON D.ID=TM.DealerId
						 JOIN  TC_BrandZone  AS TCB WITH(NOLOCK) ON TCB.TC_BrandZoneId=D.TC_BrandZoneId
						 JOIN  CarVersions AS CV WITH(NOLOCK) ON CV.ID=TM.CarVersionId
						 JOIN  CarModels AS CM WITH(NOLOCK) ON CV.CarModelId=CM.ID
						-- JOIN TC_VersionsCode AS VC WITH(NOLOCK) ON VC.CarVersionId=CV.ID
						 JOIN TC_SpecialUsers AS S WITH(NOLOCK) ON S.TC_SpecialUsersId=D.TC_AMId
						 WHERE 
							(D.TC_BrandZoneId=@TC_BrandZoneId OR @TC_BrandZoneId IS NULL)
						AND (CM.Id=@CarModelId OR @CarModelId IS NULL)
						AND (D.TC_AMId=@TC_AMId  OR @TC_AMId IS NULL)
						AND (D.ID=@DealerId OR @DealerId IS NULL)
						AND (CV.Id=@CarVersionId OR @CarVersionId IS NULL)
						AND TM.[Month] BETWEEN @StartMonth AND @EndMonth
						AND (TM.[Month]=@MonthId OR @MonthId IS NULL)
						AND TM.Year = @Year
						ORDER BY TM.Target DESC,TM.[Month]  --,TM.DealerId
			)  UPDATE  TM   SET TM.Target =CTE.RevisedTarget
			   FROM  TC_TMAMTargetChangeApprovalReq  AS TM 
				JOIN CTE  ON TM.TC_TMAMTargetChangeApprovalReqId=CTE.TC_TMAMTargetChangeApprovalReqId
	    END  

		SELECT @TargetAfterRoundOff=SUM(TM.Target)
		FROM  TC_TMAMTargetChangeApprovalReq  AS TM WITH(NOLOCK)
						 JOIN  Dealers AS D WITH(NOLOCK) ON D.ID=TM.DealerId
						 JOIN  TC_BrandZone  AS TCB WITH(NOLOCK) ON TCB.TC_BrandZoneId=D.TC_BrandZoneId
						 JOIN  CarVersions AS CV WITH(NOLOCK) ON CV.ID=TM.CarVersionId
						 JOIN  CarModels AS CM WITH(NOLOCK) ON CV.CarModelId=CM.ID
						 --JOIN TC_VersionsCode AS VC  WITH(NOLOCK) ON VC.CarVersionId=CV.ID
						 JOIN TC_SpecialUsers AS S ON S.TC_SpecialUsersId=D.TC_AMId
						 WHERE 
							(D.TC_BrandZoneId=@TC_BrandZoneId OR @TC_BrandZoneId IS NULL)
						AND (CM.Id=@CarModelId OR @CarModelId IS NULL)
						AND (D.TC_AMId=@TC_AMId  OR @TC_AMId IS NULL)
						AND (D.ID=@DealerId OR @DealerId IS NULL)
						AND (CV.Id=@CarVersionId OR @CarVersionId IS NULL)
						AND TM.[Month] BETWEEN @StartMonth AND @EndMonth
						AND (TM.[Month]=@MonthId OR @MonthId IS NULL)
						AND TM.Year = @Year

	        END 
	    
		 

	END

	END