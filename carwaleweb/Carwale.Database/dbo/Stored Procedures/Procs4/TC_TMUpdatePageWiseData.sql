IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_TMUpdatePageWiseData]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_TMUpdatePageWiseData]
GO

	-- =============================================
-- Author	    :	Deepak Tripathi
-- Create date	:	11-12-2013
-- Description	:	Update and Get Pagewise details for Target Management 	
---PAGE ID =0   Month Model wise
---PAGE ID =1   Zone  Model wise
---PAGE ID =2   AM Model wise
---PAGE ID =3   Dealer Model wise
---PAGE ID =4   AM Version wise
---PAGE ID =5   Dealer Version wise
---PAGE ID =6   Zone Version wise
---PAGE ID =7   Version  Month wise
---PAGE ID =8   Model Version wise
 -- =============================================
CREATE PROCEDURE [dbo].[TC_TMUpdatePageWiseData] 
	@PageId TINYINT,
	@TC_BrandZoneId TINYINT=NULL,
	@carModelId INT=NULL,
	@TC_AMId INT=NULL,
	@TC_SpecialUsersId INT,
	@TC_TMPageWisePercentageChange TC_TMPageWisePercentageChange READONLY,   ---ID IDENTITY Column,FieldId,CarId,PercentageChange,NewValue
	@DealerId INT =NULL,
	@StartMonth TINYINT=NULL,
	@EndMonth TINYINT =NULL

AS
	BEGIN

--	select top 1  1 id, 2 FieldId,3 CarId,50 PercentageChange, 40 NewValue from TC_Alerts
	--select * from @TC_TMPageWisePercentageChange;
        
	  DECLARE @TargetAfterRoundOff INT,
              @ActualTarget INT,
			  @WhileLoopControl INT=1,
			  @TotalWhileLoopCount INT,
			  @FieldId INT,
			  @CarId INT	  
		
		--Process to Update Data
        IF (@PageId=0)  --Month Model wise
			BEGIN
			   UPDATE I SET I.Target =  ROUND (I.Target+ (I.Target/100.000000000)* P.PercentageChange,0) 
	           FROM [TC_TMIntermediateLegacyDetail]  I
	             JOIN Dealers           AS D   ON I.DealerId=D.Id
		         JOIN CarVersions       AS V   ON V.Id=I.CarVersionId
				 JOIN TC_SpecialUsers   AS S   ON S.TC_SpecialUsersId=D.TC_AMId
				 JOIN TC_BrandZone      AS TCB ON TCB.TC_BrandZoneId=D.TC_BrandZoneId
				 JOIN @TC_TMPageWisePercentageChange AS P ON P.FieldId=I.[Month] AND P.CarId=V.CarModelId
				WHERE 
					(D.TC_BrandZoneId=@TC_BrandZoneId OR @TC_BrandZoneId IS NULL)
				AND (D.TC_AMId=@TC_AMId  OR @TC_AMId IS NULL)
				AND (D.ID=@DealerId OR @DealerId IS NULL)
			END	
		ELSE IF  (@PageId=1)  --Zone  Model wise
			BEGIN 
			    UPDATE I SET I.Target =  ROUND (I.Target+ (I.Target/100.000000000)* P.PercentageChange,0) 
	            FROM [TC_TMIntermediateLegacyDetail]  I
					 JOIN Dealers           AS D   ON I.DealerId=D.Id
					 JOIN CarVersions       AS V   ON V.Id=I.CarVersionId
					 JOIN TC_BrandZone      AS TCB ON TCB.TC_BrandZoneId=D.TC_BrandZoneId
					 JOIN @TC_TMPageWisePercentageChange AS P ON P.FieldId=TCB.TC_BrandZoneId AND P.CarId=V.CarModelId
				WHERE I.[Month] BETWEEN @StartMonth AND @EndMonth
			END			
		ELSE IF (@PageId=2)  --AM Model wise		
			BEGIN 

				UPDATE I SET I.Target =  ROUND (I.Target+ (I.Target/100.000000000)* P.PercentageChange,0) 
	            FROM [TC_TMIntermediateLegacyDetail]  I
	             JOIN Dealers           AS D   ON I.DealerId=D.Id
		         JOIN CarVersions       AS V   ON V.Id=I.CarVersionId
				 JOIN TC_SpecialUsers   AS S   ON S.TC_SpecialUsersId=D.TC_AMId
				 JOIN @TC_TMPageWisePercentageChange AS P ON S.TC_SpecialUsersId=P.FieldId AND P.CarId=V.CarModelId
				 WHERE 
					I.[Month] BETWEEN @StartMonth AND @EndMonth
			END			
		ELSE IF (@PageId=3) ---  Dealer Model wise
			BEGIN
				UPDATE I SET I.Target =  ROUND (I.Target+ (I.Target/100.000000000)* P.PercentageChange,0) 
	            FROM [TC_TMIntermediateLegacyDetail]  I
					 JOIN Dealers           AS D   ON I.DealerId=D.Id
					 JOIN CarVersions       AS V   ON V.Id=I.CarVersionId
					 JOIN @TC_TMPageWisePercentageChange AS P ON D.Id=P.FieldId AND P.CarId=V.CarModelId
				 WHERE  I.[Month] BETWEEN @StartMonth AND @EndMonth	
			END
		ELSE IF (@PageId=4) --- AM Version wise
			BEGIN
			    UPDATE I SET I.Target =  ROUND (I.Target+ (I.Target/100.000000000)* P.PercentageChange,0) 
	            FROM [TC_TMIntermediateLegacyDetail]  I
					 JOIN Dealers           AS D   ON I.DealerId=D.Id
					 JOIN TC_SpecialUsers   AS S   ON S.TC_SpecialUsersId=D.TC_AMId
					 JOIN @TC_TMPageWisePercentageChange AS P ON S.TC_SpecialUsersId=P.FieldId AND P.CarId=I.CarVersionId
				WHERE I.[Month] BETWEEN @StartMonth AND @EndMonth
			END
		ELSE IF (@PageId=5) --- Dealer  Version wise
			BEGIN
			    UPDATE I SET I.Target =  CASE P.PercentageChange WHEN 0 THEN (I.Target + P.NewValue) ELSE ROUND (I.Target+ (I.Target/100.000000000)* P.PercentageChange,0) END
	            FROM [TC_TMIntermediateLegacyDetail]  I
					 JOIN @TC_TMPageWisePercentageChange AS P ON I.DealerId=P.FieldId AND P.CarId=I.CarVersionId
				  WHERE I.[Month] BETWEEN @StartMonth AND @EndMonth
			END
		ELSE IF (@PageId=6) --- Zone Version wise
			BEGIN
			    UPDATE I SET I.Target =  ROUND (I.Target+ (I.Target/100.000000000)* P.PercentageChange,0) 
	            FROM [TC_TMIntermediateLegacyDetail]  I
	             JOIN Dealers           AS D   ON I.DealerId=D.Id
				 JOIN TC_BrandZone      AS TCB ON TCB.TC_BrandZoneId=D.TC_BrandZoneId
				 JOIN @TC_TMPageWisePercentageChange AS P ON P.FieldId=TCB.TC_BrandZoneId AND P.CarId=I.CarVersionId
				 WHERE I.[Month] BETWEEN @StartMonth AND @EndMonth	
			END
		ELSE IF (@PageId=7)  --Version  Month wise
			BEGIN
				UPDATE I SET I.Target =  CASE P.PercentageChange WHEN 0 THEN (I.Target + P.NewValue) ELSE ROUND (I.Target+ (I.Target/100.000000000)* P.PercentageChange,0) END
				FROM [TC_TMIntermediateLegacyDetail]  I
					 JOIN Dealers           AS D   ON I.DealerId=D.Id
					 JOIN TC_SpecialUsers   AS S   ON S.TC_SpecialUsersId=D.TC_AMId
					 JOIN TC_BrandZone      AS TCB ON TCB.TC_BrandZoneId=D.TC_BrandZoneId
					 JOIN @TC_TMPageWisePercentageChange AS P ON P.FieldId=I.[Month] AND P.CarId=I.CarVersionId	
				 WHERE 
					(D.TC_BrandZoneId=@TC_BrandZoneId OR @TC_BrandZoneId IS NULL)
					AND (D.TC_AMId=@TC_AMId  OR @TC_AMId IS NULL)
					AND (D.ID=@DealerId OR @DealerId IS NULL)
			END
		ELSE IF (@PageId=8)  --Model Version wise
			BEGIN
				UPDATE I SET I.Target =  ROUND (I.Target+ (I.Target/100.000000000)* P.PercentageChange,0)  
				FROM [TC_TMIntermediateLegacyDetail]  I
	             JOIN Dealers           AS D   ON I.DealerId=D.Id
		         JOIN CarVersions       AS V   ON V.Id=I.CarVersionId
		         JOIN CarModels         AS M   ON M.Id=V.CarModelId
				 JOIN TC_SpecialUsers   AS S   ON S.TC_SpecialUsersId=D.TC_AMId
				 JOIN TC_BrandZone      AS TCB ON TCB.TC_BrandZoneId=D.TC_BrandZoneId
				 JOIN @TC_TMPageWisePercentageChange AS P ON P.FieldId=M.ID AND P.CarId=I.CarVersionId
			END
			
		-- Process to Round off figures
		IF @PageId <> 5 AND @PageId <> 7
			BEGIN
				SELECT @TotalWhileLoopCount=COUNT(Id) FROM @TC_TMPageWisePercentageChange;
				WHILE (@WhileLoopControl<=@TotalWhileLoopCount)
					BEGIN 
						SELECT  @FieldId=FieldId, @CarId=CarId, @ActualTarget=NewValue FROM @TC_TMPageWisePercentageChange WHERE ID=@WhileLoopControl
						
						DECLARE @RoundModelId INT,
							@RoundVersionId INT, 
							@RoundDealerId INT,
							@RoundStartMonth INT,
							@RoundEndMonth INT,
							@RoundMonthId INT,
							@RoundAMId	INT,
							@RoundZoneId INT
					
						IF @PageId = 0
							BEGIN
								SET @RoundModelId = @CarId
								SET @RoundVersionId = NULL
								SET @RoundStartMonth = @FieldId
								SET @RoundEndMonth = @FieldId
								SET @RoundMonthId = @FieldId
								SET @RoundDealerId = @DealerId
								SET @RoundAMId	= @TC_AMId
								SET @RoundZoneId = @TC_BrandZoneId
							END
						ELSE IF @PageId = 1
							BEGIN
								SET @RoundModelId = @CarId
								SET @RoundVersionId = NULL
								SET @RoundStartMonth = @StartMonth
								SET @RoundEndMonth = @EndMonth
								SET @RoundMonthId = NULL
								SET @RoundDealerId = @DealerId
								SET @RoundAMId	= @TC_AMId
								SET @RoundZoneId = @FieldId
							END
						ELSE IF @PageId = 2
							BEGIN
								SET @RoundModelId = @CarId
								SET @RoundVersionId = NULL
								SET @RoundStartMonth = @StartMonth
								SET @RoundEndMonth = @EndMonth
								SET @RoundMonthId = NULL
								SET @RoundDealerId = @DealerId
								SET @RoundAMId	= @FieldId
								SET @RoundZoneId = @TC_BrandZoneId
							END
						ELSE IF @PageId = 3
							BEGIN
								SET @RoundModelId = @CarId
								SET @RoundVersionId = NULL
								SET @RoundStartMonth = @StartMonth
								SET @RoundEndMonth = @EndMonth
								SET @RoundMonthId = NULL
								SET @RoundDealerId = @FieldId
								SET @RoundAMId	= @TC_AMId
								SET @RoundZoneId = @TC_BrandZoneId
							END
						ELSE IF @PageId = 4
							BEGIN
								SET @RoundModelId = NULL
								SET @RoundVersionId = @CarId
								SET @RoundStartMonth = @StartMonth
								SET @RoundEndMonth = @EndMonth
								SET @RoundMonthId = NULL
								SET @RoundDealerId = @DealerId
								SET @RoundAMId	= @FieldId
								SET @RoundZoneId = @TC_BrandZoneId
							END
						ELSE IF @PageId = 5
							BEGIN
								SET @RoundModelId = NULL
								SET @RoundVersionId = @CarId
								SET @RoundStartMonth = @StartMonth
								SET @RoundEndMonth = @EndMonth
								SET @RoundMonthId = NULL
								SET @RoundDealerId = @FieldId
								SET @RoundAMId	= @TC_AMId
								SET @RoundZoneId = @TC_BrandZoneId
							END
						ELSE IF @PageId = 6
							BEGIN
								SET @RoundModelId = NULL
								SET @RoundVersionId = @CarId
								SET @RoundStartMonth = @StartMonth
								SET @RoundEndMonth = @EndMonth
								SET @RoundMonthId = NULL
								SET @RoundDealerId = @DealerId
								SET @RoundAMId	= @TC_AMId
								SET @RoundZoneId = @FieldId
							END
						ELSE IF @PageId = 7
							BEGIN
								SET @RoundModelId = NULL
								SET @RoundVersionId = @CarId
								SET @RoundStartMonth = @FieldId
								SET @RoundEndMonth = @FieldId
								SET @RoundMonthId = @FieldId
								SET @RoundDealerId = @DealerId
								SET @RoundAMId	= @TC_AMId
								SET @RoundZoneId = @TC_BrandZoneId
							END
						ELSE IF @PageId = 8
							BEGIN
								SET @RoundModelId = @FieldId
								SET @RoundVersionId = @CarId
								SET @RoundStartMonth = 1
								SET @RoundEndMonth = 12
								SET @RoundMonthId = NULL
								SET @RoundDealerId = @DealerId
								SET @RoundAMId	= null
								SET @RoundZoneId = NULL
							END
							
							
						-- Round off figures
						EXEC [dbo].[TC_TMRoundOffHandling] 
					 			@ActualTarget=@ActualTarget ,
								@TC_BrandZoneId=@RoundZoneId,
								@CarModelId =@RoundModelId,
								@TC_AMId = @RoundAMId,
								@DealerId  =@RoundDealerId,
								@CarVersionId  = @RoundVersionId,
								@StartMonth =@RoundStartMonth,
								@EndMonth  =@RoundEndMonth,
								@MonthId=@RoundMonthId,
								@TC_TMDistributionPatternMasterId =NULL
												
						SET @WhileLoopControl=@WhileLoopControl+1;
					END
			END	
			
			--Process to Get the Data
			EXEC [dbo].[TC_TMPageWiseData] 
				@PageId = @PageId,
				@TC_BrandZoneId = @TC_BrandZoneId,
				@CarModelId = @carModelId,
				@TC_AMId  = @TC_AMId,
				@DealerId = @DealerId,
				@StartMonth  = @StartMonth,
				@EndMonth = @EndMonth			 
	END


