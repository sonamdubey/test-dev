IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_TMCopyDealersTarget]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_TMCopyDealersTarget]
GO

	-- =============================================
-- Author	    :	Vinayak Patil
-- Create date	:	18-11-2013
-- Description	:	Copy the target of the dealer from another dealer.
 -- =============================================
CREATE PROCEDURE [dbo].[TC_TMCopyDealersTarget] 
@DealerId INT,
@TC_SpecialUsersId INT,
@OpeningMonth TINYINT,
@CopyFromDealerId INT,
@TargetPercentageChange FLOAT,
@TC_TMDistributionPatternMasterId INT,
@NewTargetValue INT,
@IsDealer BIT,
@CopyFromTC_TMDistributionPatternMasterId INT=NULL,
@LaunchYear SMALLINT = NULL
AS
  BEGIN      
		DECLARE @Month Smallint
		
		IF @LaunchYear IS NULL
			SET @LaunchYear = YEAR(GETDATE())

		  --------------- This is used to store all the input parameteres in the table TC_TMTargetCopyData
		   INSERT INTO TC_TMTargetCopyData
					(TC_SpecialUsersId,
					 CopyFrom,
					 CopyTo,
					 StartMonth,
					 TargetValue,
					 EntryDate,
					 IsDealer,
					 TC_TMDistributionPatternMasterId)
		VALUES      (@TC_SpecialUsersId,
					 @CopyFromDealerId,
					 @DealerId,
					 @OpeningMonth,
					 @NewTargetValue,
					 GETDATE(),
					 @IsDealer,
					 @TC_TMDistributionPatternMasterId)  


		-- Copying data while doing a setup not saved it as a scenario yet
		--Added By Deepak on 4th Dec 2013
		IF @TC_TMDistributionPatternMasterId = -1
			BEGIN
			   
			   IF NOT EXISTS(SELECT TOP 1 * 
				FROM   TC_TMIntermediateLegacyDetail 
				WHERE  DealerId = @DealerId AND MONTH = 1) 
					BEGIN 
					  SET @Month = 1 
					  
					  WHILE (@Month <= 12) 
						BEGIN 
							INSERT INTO TC_TMIntermediateLegacyDetail(DealerId, CarVersionId, Month, Year, Target) 
							SELECT @DealerId, CV.ID, @Month, @LaunchYear, 0
							FROM CarVersions CV
							WHERE CV.CarModelId IN(SELECT ID FROM CarModels CM WHERE CM.CarMakeId = 20)
							
							SET @Month = @Month + 1
						END 
				  END 

				
				
				
				--------------- Used to delete the data from TC_TMIntermediateLegacyDetail table if the data for user already exists
				--------------- to which user wants to apply the target 
		
				DELETE FROM TC_TMIntermediateLegacyDetail 
				WHERE 
					   [Month] >=@OpeningMonth
					   AND DealerId = @DealerId 
					   
				------------------ Copying data into TC_TMIntermediateLegacyDetail for the desired user-------------------------
	
				INSERT INTO TC_TMIntermediateLegacyDetail 
						(DealerId, 
						 CarVersionId, 
						 Month, 
						 Year, 
						 Target)
				SELECT @DealerId, 
						CarVersionId, 
						Month, 
						Year, 
						Target 
				FROM   TC_TMIntermediateLegacyDetail WITH(NOLOCK)
				WHERE  [Month] >=@OpeningMonth
					   AND DealerId = @CopyFromDealerId

			-------------Updating the new target for the desired user--------------------------------------
			  IF (@TargetPercentageChange <> 0)
			   BEGIN
					UPDATE TC_TMIntermediateLegacyDetail 
					SET    Target =  ROUND (Target+ (Target/100)* @TargetPercentageChange,0)
					WHERE  [Month] >=@OpeningMonth
						   AND DealerId = @DealerId 
	   
				   EXEC [dbo].[TC_TMRoundOffHandling] 
					 					@ActualTarget=@NewTargetValue ,
										@TC_BrandZoneId=NULL,
										@CarModelId =NULL,
										@TC_AMId =NULL,
										@DealerId  =@DealerId,
										@CarVersionId  =NULL,
										@StartMonth =@OpeningMonth,
										@EndMonth  =12,
										@MonthId=NULL,
										@TC_TMDistributionPatternMasterId=NULL
	   
	   
				END 
			END
		ELSE
			BEGIN
				IF NOT EXISTS(SELECT TOP 1 * 
				FROM   TC_TMTargetScenarioDetail 
				WHERE  DealerId = @DealerId AND TC_TMDistributionPatternMasterId = @CopyFromTC_TMDistributionPatternMasterId ) 
					BEGIN 
					  SET @Month = 1 
					  
					  WHILE (@Month <= 12) 
						BEGIN 
							INSERT INTO TC_TMTargetScenarioDetail(DealerId, CarVersionId, Month, Year, Target, TC_TMDistributionPatternMasterId) 
							SELECT @DealerId, CV.ID, @Month, @LaunchYear, 0, @CopyFromTC_TMDistributionPatternMasterId
							FROM CarVersions CV
							WHERE CV.CarModelId IN(SELECT ID FROM CarModels CM WHERE CM.CarMakeId = 20)
							
							SET @Month = @Month + 1
						END 
				  END 
				
				
				--------------- Used to delete the data from TC_TMTargetScenarioDetail table if the data for user already exists
				--------------- to which user wants to apply the target 
		
				DELETE FROM TC_TMTargetScenarioDetail 
				WHERE  TC_TMDistributionPatternMasterId = @TC_TMDistributionPatternMasterId 
					   AND  [Month] >=@OpeningMonth
					   AND DealerId = @DealerId 
					   
				------------------ Copying data into TC_TMTargetScenarioDetail for the desired user-------------------------
	
				INSERT INTO TC_TMTargetScenarioDetail 
						(DealerId, 
						 CarVersionId, 
						 Month, 
						 Year, 
						 Target,
						 TC_TMDistributionPatternMasterId)
				SELECT @DealerId, 
						CarVersionId, 
						Month, 
						Year, 
						Target,
						@TC_TMDistributionPatternMasterId 
				FROM   TC_TMTargetScenarioDetail WITH(NOLOCK)
				WHERE  TC_TMDistributionPatternMasterId = @CopyFromTC_TMDistributionPatternMasterId 
					   AND  [Month] >=@OpeningMonth
					   AND DealerId = @CopyFromDealerId

			-------------Updating the new target for the desired user--------------------------------------
			  IF (@TargetPercentageChange <> 0)
			   BEGIN
					UPDATE TC_TMTargetScenarioDetail 
					SET    Target =  ROUND (Target+ (Target/100)* @TargetPercentageChange,0)
					WHERE  TC_TMDistributionPatternMasterId = @TC_TMDistributionPatternMasterId 
						   AND  [Month] >=@OpeningMonth
						   AND DealerId = @DealerId 
	   
				   EXEC [dbo].[TC_TMRoundOffHandling] 
					 					@ActualTarget=@NewTargetValue ,
										@TC_BrandZoneId=NULL,
										@CarModelId =NULL,
										@TC_AMId =NULL,
										@DealerId  =@DealerId,
										@CarVersionId  =NULL,
										@StartMonth =@OpeningMonth,
										@EndMonth  =12,
										@MonthId=NULL,
										@TC_TMDistributionPatternMasterId=@TC_TMDistributionPatternMasterId
	   
	   
				END 
			END
  END
