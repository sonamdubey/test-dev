IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_TMUpdatePageWiseDataForApproval]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_TMUpdatePageWiseDataForApproval]
GO

	-- =============================================
-- Author	    :	Vinayk Patil
-- Create date	:	25-11-2013
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

--Edited By Deepak on 8th December 2013
 -- =============================================
CREATE PROCEDURE [dbo].[TC_TMUpdatePageWiseDataForApproval] 
	@PageId TINYINT,
	@TC_AMId INT=NULL,
	@TC_TMPageWisePercentageChange TC_TMPageWisePercentageChange READONLY,   ---ID IDENTITY Column,FieldId,CarId,PercentageChange,NewValue
	@DealerId INT =NULL,
	@StartMonth TINYINT=NULL,
	@EndMonth TINYINT =NULL,
	@Year SMALLINT,
	@CarModelId INT = NULL
	
AS
	BEGIN
		-- To See if data exists in [TC_TMAMTargetChangeApprovalReq] for AM for that year
		-- IF data does not exist then first insert data and then update
		IF NOT EXISTS (SELECT TOP 1 TC_DealersTargetId  FROM   [TC_TMAMTargetChangeApprovalReq] WHERE  TC_AMId = @TC_AMId  AND [Year] = @Year)
			BEGIN
				-- Inserting data into [TC_TMAMTargetChangeApprovalReq] table against
				-- Area Manager for that year
				INSERT INTO [TC_TMAMTargetChangeApprovalReq]
						  (TC_DealersTargetId,
						   DealerId,
						   Month,
						   Year,
						   Target,
						   CreatedBy,
						   IsDeleted,
						   TC_TargetTypeId,
						   CarVersionId,
						   TC_AMId
						   )
				  SELECT DT.TC_DealersTargetId,
						 DT.DealerId,
						 DT.Month,
						 DT.Year,
						 DT.Target,
						 DT.CreatedBy,
						 DT.IsDeleted,
						 DT.TC_TargetTypeId,
						 DT.CarVersionId,
						 @TC_AMId  
				  FROM   Dealers D JOIN TC_DealersTarget DT   ON  D.ID = DT.DealerId 
				  WHERE  D.TC_AMId = @TC_AMId
						 AND D.IsDealerActive = 1
						 AND TC_BrandZoneId IS NOT NULL
						 AND [Year] = @Year
						 AND DT.TC_TargetTypeId = 4
			END  
			-- Update Statement Starts from here
	   
		DECLARE @TargetAfterRoundOff INT,
              @ActualTarget INT,
			  @WhileLoopControl INT=1,
			  @TotalWhileLoopCount INT,
			  @FieldId INT,
			  @CarId INT

	    SELECT @TotalWhileLoopCount=COUNT(Id) FROM @TC_TMPageWisePercentageChange;
	    
	    --Update Data First
		IF (@PageId=0)  --Month Model wise
			BEGIN
				UPDATE I SET I.Target =  ROUND (I.Target+ (I.Target/100.000000000)* P.PercentageChange,0)
	            FROM [TC_TMAMTargetChangeApprovalReq]  I
					 JOIN Dealers           AS D   ON I.DealerId=D.Id
					 JOIN CarVersions       AS V   ON V.Id=I.CarVersionId
					 JOIN CarModels         AS M   ON M.Id=V.CarModelId
					 JOIN @TC_TMPageWisePercentageChange AS P ON P.FieldId=I.[Month] AND P.CarId=M.ID
				 WHERE (D.TC_AMId=@TC_AMId  OR @TC_AMId IS NULL)
						AND (D.ID=@DealerId OR @DealerId IS NULL)
						AND I.Year = @Year
		   END
		ELSE IF (@PageId=3) --Dealer Model wise
			BEGIN
			    UPDATE I SET I.Target =  ROUND (I.Target+ (I.Target/100.000000000)* P.PercentageChange,0)
	            FROM [TC_TMAMTargetChangeApprovalReq]  I
					 JOIN Dealers           AS D   ON I.DealerId=D.Id
					 JOIN CarVersions       AS V   ON V.Id=I.CarVersionId
					 JOIN CarModels         AS M   ON M.Id=V.CarModelId
					 JOIN @TC_TMPageWisePercentageChange AS P ON D.Id=P.FieldId AND P.CarId=M.ID
				WHERE (D.TC_AMId=@TC_AMId  OR @TC_AMId IS NULL)
					AND I.Year = @Year
					AND I.[Month] BETWEEN @StartMonth AND @EndMonth
			END 
	   ELSE IF (@PageId=5) --- Dealer  Version wise
			BEGIN 
				UPDATE I SET I.Target =  ROUND (I.Target+ (I.Target/100.000000000)* P.PercentageChange,0)
	            FROM [TC_TMAMTargetChangeApprovalReq]  I
					JOIN Dealers           AS D   ON I.DealerId=D.Id
					JOIN CarVersions       AS V   ON V.Id=I.CarVersionId
					JOIN CarModels         AS M   ON M.Id=V.CarModelId
					JOIN @TC_TMPageWisePercentageChange AS P ON D.Id=P.FieldId AND P.CarId=V.ID
				  WHERE (D.TC_AMId=@TC_AMId  OR @TC_AMId IS NULL)
					AND I.Year = @Year
					AND I.[Month] BETWEEN @StartMonth AND @EndMonth
			END
		ELSE IF (@PageId=7)  --Version  Month wise
			BEGIN
				UPDATE I SET I.Target =  ROUND (I.Target+ (I.Target/100.000000000)* P.PercentageChange,0)
	            FROM [TC_TMAMTargetChangeApprovalReq]  I
	             JOIN Dealers           AS D   ON I.DealerId=D.Id
		         JOIN CarVersions       AS V   ON V.Id=I.CarVersionId
		         JOIN CarModels         AS M   ON M.Id=V.CarModelId
				 JOIN @TC_TMPageWisePercentageChange AS P ON P.FieldId=I.[Month] AND P.CarId=V.ID	
				 WHERE (D.TC_AMId=@TC_AMId  OR @TC_AMId IS NULL)
						AND (D.ID=@DealerId OR @DealerId IS NULL)
						AND I.Year = @Year			
			END 
			
		--Round of data first
		WHILE (@WhileLoopControl<=@TotalWhileLoopCount)
		BEGIN 
			SELECT  @FieldId=FieldId, @CarId=CarId, @ActualTarget=NewValue FROM @TC_TMPageWisePercentageChange WHERE ID=@WhileLoopControl
			
			--Month Model wise update
			DECLARE @RoundModelId INT,
					@RoundVersionId INT, 
					@RoundDealerId INT,
					@RoundStartMonth INT,
					@RoundEndMonth INT,
					@RoundMonthId INT,
					@RoundAMId	INT
					
			IF @PageId = 0
				BEGIN
					SET @RoundModelId = @CarId
					SET @RoundVersionId = NULL
					SET @RoundDealerId = @DealerId
					SET @RoundStartMonth = @StartMonth
					SET @RoundEndMonth = @EndMonth
					SET @RoundMonthId = @FieldId
					SET @RoundAMId	= @TC_AMId
				END
			ELSE IF @PageId = 3
				BEGIN
					SET @RoundModelId = @CarId
					SET @RoundVersionId = NULL
					SET @RoundDealerId = @FieldId
					SET @RoundMonthId = NULL
					SET @RoundAMId	= NULL
					SET @RoundStartMonth = @StartMonth
					SET @RoundEndMonth = @EndMonth
					
					--IF @Year = YEAR(GETDATE())
					--	BEGIN
					--		SET @RoundStartMonth = MONTH(GETDATE())
					--		SET @RoundEndMonth = 12
					--	END
					--ELSE
					--	BEGIN
					--		SET @RoundStartMonth = 1
					--		SET @RoundEndMonth = 12
					--	END
				END
			ELSE IF @PageId = 5
				BEGIN
					SET @RoundModelId = NULL
					SET @RoundVersionId = @CarId
					SET @RoundDealerId = @FieldId
					SET @RoundMonthId = NULL
					SET @RoundAMId	= NULL
					SET @RoundStartMonth = @StartMonth
					SET @RoundEndMonth = @EndMonth
					
					--IF @Year = YEAR(GETDATE())
					--	BEGIN
					--		SET @RoundStartMonth = MONTH(GETDATE())
					--		SET @RoundEndMonth = 12
					--	END
					--ELSE
					--	BEGIN
					--		SET @RoundStartMonth = 1
					--		SET @RoundEndMonth = 12
					--	END
				END
			ELSE IF @PageId = 7
				BEGIN
					SET @RoundModelId = NULL
					SET @RoundVersionId = @CarId
					SET @RoundDealerId = @DealerId
					SET @RoundStartMonth = @StartMonth
					SET @RoundEndMonth = @EndMonth
					SET @RoundMonthId = @FieldId
					SET @RoundAMId	= @TC_AMId
				END
			
			--SELECT @ActualTarget, @RoundModelId, @TC_AMId, @RoundDealerId, @RoundVersionId, @RoundMonthId, @StartMonth,  @EndMonth, @Year
			EXEC [dbo].[TC_TMRoundOffHandling] 
		 		@ActualTarget=@ActualTarget ,
				@TC_BrandZoneId=NULL,
				@CarModelId = @RoundModelId,
				@TC_AMId =@TC_AMId,
				@DealerId  = @RoundDealerId,
				@CarVersionId = @RoundVersionId,
				@StartMonth =@RoundStartMonth,
				@EndMonth  = @RoundEndMonth,
				@MonthId=@RoundMonthId,
				@TC_TMDistributionPatternMasterId=NULL,
				@IsTargetChangeFromAM =1,
				@Year = @Year
		 
			SET @WhileLoopControl=@WhileLoopControl+1;
		END 
		
		--Return Data
		--SELECT @PageId, @CarModelId, @DealerId, @StartMonth, @EndMonth, @Year
		EXEC [dbo].[TC_TMPageWiseDataForApproval]
				@PageId = @PageId,
				@CarModelId = @CarModelId,
				@TC_AMId = @TC_AMId,
				@DealerId = @DealerId,
				@StartMonth = @StartMonth,
				@EndMonth = @EndMonth,
				@Year = @Year

 END
