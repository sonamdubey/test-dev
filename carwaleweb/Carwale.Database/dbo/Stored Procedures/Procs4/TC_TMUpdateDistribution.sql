IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_TMUpdateDistribution]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_TMUpdateDistribution]
GO

	-- =============================================
-- Author	    :	Manish Chourasiya
-- Create date	:	26-10-2013
-- Description	:	Get Pagewise details for Target Management
---- Alter by: Vishal Srivastava AE 1830
---- Date : 14-11-2013
---- Description: Added an insert statement for saving market parameter 	
-- Modified By: Nilesh Utture on 09-12-2013, Modified formula to calculate percentage
-- Modified By: Nilesh Utture on 12-12-2013, Added Functionality for round-up of target
 -- =============================================
CREATE  PROCEDURE [dbo].[TC_TMUpdateDistribution]
    @PercentageChange FLOAT,
	@TC_BrandZoneId TINYINT=NULL,
	@TC_AMId INT=NULL,
	@DealerId INT =NULL,
	@carModelId INT=NULL,
	@MonthStart TINYINT=NULL,
	@MonthEnd TINYINT=NULL,
	@IsZoneWise BIT =0,
	@TC_SpecialUsersId INT,
	@TC_TMDistributionPatternMasterId INT=NULL

AS
	BEGIN
    DECLARE @ActualTarget INT, -- Actual count which should be increased
			@Year INT,
			@TargetBeforeRoundOff INT -- Target present before increasing the percentage 
	
	---added insert statement for saving data in TC_MarketParameter
	 INSERT INTO TC_TMMarketParameter ([Date],
	                                 TC_SpecialUsersId,
									 Percentage,
									 TC_BrandZoneId,
									 CarModelId,
									 StartMonth,
									 EndMonth,
									 AMId,
									 DealerId,
									 TC_TMDistributionPatternMasterId)
	    	                VALUES (CONVERT(DATE,GETDATE()),
							        @TC_SpecialUsersId,
									@PercentageChange,
									@TC_BrandZoneId,
									@carModelId,
									@MonthStart,
									@MonthEnd,
									@TC_AMId,
									@DealerId,
									@TC_TMDistributionPatternMasterId)

    IF (@TC_TMDistributionPatternMasterId IS NULL)
		BEGIN 

			-- Modified By: Nilesh Utture on 12-12-2013
			SELECT @TargetBeforeRoundOff = SUM(I.Target), @Year = I.Year
			FROM TC_TMIntermediateLegacyDetail I
				JOIN Dealers D ON D.ID = I.DealerId 
					AND (D.TC_BrandZoneId = @TC_BrandZoneId OR @TC_BrandZoneId IS NULL)
					AND (D.TC_AMId = @TC_AMId OR @TC_AMId IS NULL)
					AND (D.ID = @DealerId OR @DealerId IS NULL)
				JOIN CarVersions V ON V.Id = I .CarVersionId
					AND (V.CarModelId = @carModelId OR @carModelId IS NULL)
				WHERE I.[Month] BETWEEN @MonthStart AND @MonthEnd
				GROUP BY I.Year
			
			-- Modified By: Nilesh Utture on 12-12-2013
			SET @ActualTarget = ROUND(@TargetBeforeRoundOff + ((@PercentageChange * @TargetBeforeRoundOff)/100),0)

			-- Modified By: Nilesh Utture on 09-12-2013
			UPDATE I SET I.Target =  ROUND (I.Target+ (I.Target/100.000000000)* @PercentageChange,0) --ROUND (I.Target+ (I.Target/100)* @PercentageChange,0)
			 FROM [TC_TMIntermediateLegacyDetail]  I
				JOIN Dealers           AS D   ON I.DealerId=D.Id
				JOIN CarVersions       AS V   ON V.Id=I.CarVersionId
				JOIN TC_SpecialUsers   AS S   ON S.TC_SpecialUsersId=D.TC_AMId
				JOIN TC_BrandZone      AS TCB ON TCB.TC_BrandZoneId=D.TC_BrandZoneId
				JOIN CarModels         AS M   ON M.Id=V.CarModelId
				WHERE 
					(D.TC_BrandZoneId=@TC_BrandZoneId OR @TC_BrandZoneId IS NULL)
				AND (V.CarModelId=@CarModelId OR @CarModelId IS NULL)
				AND (D.ID=@DealerId OR @DealerId IS NULL)
				AND (D.TC_AMId=@TC_AMId  OR @TC_AMId IS NULL) 
				AND I.[Month] BETWEEN @MonthStart AND @MonthEnd
			
			-- Modified By: Nilesh Utture on 12-12-2013,
			EXEC [dbo].[TC_TMRoundOffHandling] 
		 		@ActualTarget=@ActualTarget ,
				@TC_BrandZoneId=@TC_BrandZoneId,
				@CarModelId = @carModelId,
				@TC_AMId =@TC_AMId,
				@DealerId  = @DealerId,
				@CarVersionId = NULL,
				@StartMonth =@MonthStart,
				@EndMonth  = @MonthEnd,
				@MonthId=NULL,
				@TC_TMDistributionPatternMasterId=@TC_TMDistributionPatternMasterId,
				@IsTargetChangeFromAM = 0,
				@Year = @Year

			IF (@IsZoneWise=0)
				BEGIN 	 
					SELECT    CM.ID [CarId], 
						   CM.Name [CarName], 
						   CASE TM.[MONTH]  WHEN 1  THEN  'JAN'
											WHEN 2  THEN  'FEB'
											WHEN 3  THEN  'MAR'
											WHEN 4  THEN  'APR'
											WHEN 5  THEN  'MAY'
											WHEN 6  THEN  'JUN'
											WHEN 7  THEN  'JUL'
											WHEN 8  THEN  'AUG'
											WHEN 9  THEN  'SEP'
											WHEN 10  THEN  'OCT'
											WHEN 11  THEN  'NOV'
											WHEN 12  THEN  'DEC' END [FieldName],
											TM.Month [FieldId], 
											ROUND(SUM(Target),0) [Target]
									FROM  [TC_TMIntermediateLegacyDetail] AS TM WITH(NOLOCK)
									JOIN  Dealers AS D WITH(NOLOCK) ON D.ID=TM.DealerId
									JOIN  TC_BrandZone  AS TCB WITH(NOLOCK) ON TCB.TC_BrandZoneId=D.TC_BrandZoneId
									JOIN  CarVersions AS CV WITH(NOLOCK) ON CV.ID=TM.CarVersionId
									JOIN  CarModels AS CM WITH(NOLOCK)  ON CV.CarModelId=CM.ID
									GROUP BY  CM.ID,CM.Name,TM.Month
									ORDER BY CM.ID,[FieldId]
				END 		
			ELSE 
				BEGIN 
					SELECT    CM.ID [CarId], 
										CM.Name [CarName], 
										TCB.ZoneName  [FieldName], 
										TCB.TC_BrandZoneId [FieldId], 
										ROUND(SUM(Target),0) [Target]
								FROM  [TC_TMIntermediateLegacyDetail] AS TM WITH(NOLOCK)
								JOIN  Dealers AS D WITH(NOLOCK) ON D.ID=TM.DealerId
								JOIN  TC_BrandZone  AS TCB WITH(NOLOCK) ON TCB.TC_BrandZoneId=D.TC_BrandZoneId
								JOIN  CarVersions AS CV WITH(NOLOCK) ON CV.ID=TM.CarVersionId
								JOIN  CarModels AS CM WITH(NOLOCK)  ON CV.CarModelId=CM.ID
								GROUP BY  CM.ID,CM.Name,TCB.ZoneName,TCB.TC_BrandZoneId
								ORDER BY CM.ID,TCB.TC_BrandZoneId
				END 
		END 
    ELSE 
		BEGIN 

			-- Modified By: Nilesh Utture on 12-12-2013
			 SELECT @TargetBeforeRoundOff = SUM(I.Target), @Year = I.Year 
			 FROM TC_TMTargetScenarioDetail I
				JOIN Dealers D ON D.ID = I.DealerId 
					AND (D.TC_BrandZoneId = @TC_BrandZoneId OR @TC_BrandZoneId IS NULL)
					AND (D.TC_AMId = @TC_AMId OR @TC_AMId IS NULL)
					AND (D.ID = @DealerId OR @DealerId IS NULL)
					AND TC_TMDistributionPatternMasterId=@TC_TMDistributionPatternMasterId
				JOIN CarVersions V ON V.Id = I .CarVersionId
					AND (V.CarModelId = @carModelId OR @carModelId IS NULL)
				WHERE I.[Month] BETWEEN @MonthStart AND @MonthEnd
				GROUP BY I.Year

			-- Modified By: Nilesh Utture on 12-12-2013
			SET @ActualTarget = ROUND(@TargetBeforeRoundOff + ((@PercentageChange * @TargetBeforeRoundOff)/100),0)

			-- Modified By: Nilesh Utture on 09-12-2013
			UPDATE I SET I.Target = ROUND (I.Target+ (I.Target/100.000000000)* @PercentageChange,0) --ROUND (I.Target+ (I.Target/100)* @PercentageChange,0)
			FROM TC_TMTargetScenarioDetail  I WITH(NOLOCK) 
			JOIN Dealers           AS D  WITH(NOLOCK)  ON I.DealerId=D.Id
			JOIN CarVersions       AS V  WITH(NOLOCK)  ON V.Id=I.CarVersionId
			JOIN TC_SpecialUsers   AS S   WITH(NOLOCK) ON S.TC_SpecialUsersId=D.TC_AMId
			JOIN TC_BrandZone      AS TCB WITH(NOLOCK) ON TCB.TC_BrandZoneId=D.TC_BrandZoneId
			JOIN CarModels         AS M  WITH(NOLOCK)  ON M.Id=V.CarModelId
			WHERE 
				(D.TC_BrandZoneId=@TC_BrandZoneId OR @TC_BrandZoneId IS NULL)
			AND (V.CarModelId=@CarModelId OR @CarModelId IS NULL)
			AND (D.ID=@DealerId OR @DealerId IS NULL)
			AND (D.TC_AMId=@TC_AMId  OR @TC_AMId IS NULL) 
			AND I.[Month] BETWEEN @MonthStart AND @MonthEnd
			AND TC_TMDistributionPatternMasterId=@TC_TMDistributionPatternMasterId
	 		
			-- Modified By: Nilesh Utture on 12-12-2013
			EXEC [dbo].[TC_TMRoundOffHandling] 
		 			@ActualTarget=@ActualTarget ,
					@TC_BrandZoneId=@TC_BrandZoneId,
					@CarModelId = @carModelId,
					@TC_AMId =@TC_AMId,
					@DealerId  = @DealerId,
					@CarVersionId = NULL,
					@StartMonth =@MonthStart,
					@EndMonth  = @MonthEnd,
					@MonthId=NULL,
					@TC_TMDistributionPatternMasterId=@TC_TMDistributionPatternMasterId,
					@IsTargetChangeFromAM = 0,
					@Year = @Year

			 IF (@IsZoneWise=0)
				 BEGIN 
					 SELECT    CM.ID [CarId], 
						   CM.Name [CarName], 
						   CASE TM.[MONTH]  WHEN 1  THEN  'JAN'
											WHEN 2  THEN  'FEB'
											WHEN 3  THEN  'MAR'
											WHEN 4  THEN  'APR'
											WHEN 5  THEN  'MAY'
											WHEN 6  THEN  'JUN'
											WHEN 7  THEN  'JUL'
											WHEN 8  THEN  'AUG'
											WHEN 9  THEN  'SEP'
											WHEN 10  THEN  'OCT'
											WHEN 11  THEN  'NOV'
											WHEN 12  THEN  'DEC' END [FieldName],
											TM.Month [FieldId], 
											ROUND(SUM(Target),0) [Target]
									FROM  TC_TMTargetScenarioDetail AS TM WITH(NOLOCK) 
									JOIN  Dealers AS D WITH(NOLOCK) ON D.ID=TM.DealerId
									JOIN  TC_BrandZone  AS TCB WITH(NOLOCK) ON TCB.TC_BrandZoneId=D.TC_BrandZoneId
									JOIN  CarVersions AS CV WITH(NOLOCK) ON CV.ID=TM.CarVersionId
									JOIN  CarModels AS CM WITH(NOLOCK) ON CV.CarModelId=CM.ID
									WHERE TC_TMDistributionPatternMasterId=@TC_TMDistributionPatternMasterId
									GROUP BY  CM.ID,CM.Name,TM.Month
									ORDER BY CM.ID,[FieldId]
				 END 
			 ELSE 
				 BEGIN 
					 SELECT    CM.ID [CarId], 
										CM.Name [CarName], 
										TCB.ZoneName  [FieldName], 
										TCB.TC_BrandZoneId [FieldId], 
										ROUND(SUM(Target),0) [Target]
								FROM  TC_TMTargetScenarioDetail AS TM WITH(NOLOCK)
								JOIN  Dealers AS D WITH(NOLOCK)  ON D.ID=TM.DealerId
								JOIN  TC_BrandZone  AS TCB WITH(NOLOCK) ON TCB.TC_BrandZoneId=D.TC_BrandZoneId
								JOIN  CarVersions AS CV WITH(NOLOCK)  ON CV.ID=TM.CarVersionId
								JOIN  CarModels AS CM WITH(NOLOCK) ON CV.CarModelId=CM.ID
								WHERE TC_TMDistributionPatternMasterId=@TC_TMDistributionPatternMasterId
								GROUP BY  CM.ID,CM.Name,TCB.ZoneName,TCB.TC_BrandZoneId
								ORDER BY CM.ID,TCB.TC_BrandZoneId
				 END 
		END 
	END



/****** Object:  StoredProcedure [dbo].[TC_TMNSCApprovalButton]    Script Date: 13/12/2013 5:07:12 PM ******/
SET ANSI_NULLS ON
