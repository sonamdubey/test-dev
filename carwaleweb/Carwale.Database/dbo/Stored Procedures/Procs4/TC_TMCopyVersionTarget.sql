IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_TMCopyVersionTarget]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_TMCopyVersionTarget]
GO

	-- =============================================
-- Author	    :	Vinayak Patil
-- Create date	:	18-11-2013
-- Description	:	Copy the target of the version from another version.
 -- =============================================
CREATE PROCEDURE [dbo].[TC_TMCopyVersionTarget] 
@VersionId INT,
@LaunchMonth TINYINT,
@TC_SpecialUsersId INT,
@CopyFromVersionId INT,
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
			
		  --------------- This is used to store all the input parameteres in the table TC_TMTargetCopyData----------------------

		   INSERT INTO TC_TMTargetCopyData
					(TC_SpecialUsersId,
					 CopyFrom,
					 CopyTo,
					 StartMonth,
					 TargetValue,
					 EntryDate,
					 IsDealer,
					 TC_TMDistributionPatternMasterId)
		     VALUES (@TC_SpecialUsersId,
					 @CopyFromVersionId,
					 @VersionId,
					 @LaunchMonth,
					 @NewTargetValue,
					 GETDATE(),
					 @IsDealer,
					 @TC_TMDistributionPatternMasterId)  
    
		-- Copying data while doing a setup not saved it as a scenario yet
		--Added By Deepak on 4th Dec 2013
		
		IF @TC_TMDistributionPatternMasterId = -1
			BEGIN
				--------------- Used to delete the data from TC_TMTargetScenarioDetail table if the data for version already exists----
				--------------- to which user wants to apply the target ------------------------------------------------------------
		 
				IF NOT EXISTS(SELECT TOP 1 * 
				FROM   TC_TMIntermediateLegacyDetail 
				WHERE  CarVersionId = @VersionId AND MONTH = 1) 
					BEGIN 
					  SET @Month = 1 
					  
					  WHILE (@Month <= 12) 
						BEGIN 
							INSERT INTO TC_TMIntermediateLegacyDetail(DealerId, CarVersionId, Month, Year, Target) 
							SELECT D.ID, @VersionId, @Month, @LaunchYear, 0
							FROM Dealers D
							WHERE TC_BrandZoneId IN(SELECT TC_BrandZoneId FROM TC_BrandZone TZ WHERE TZ.MakeId = 20)
							
							SET @Month = @Month + 1
						END 
				  END 


				
				 DELETE FROM TC_TMIntermediateLegacyDetail 
				 WHERE    [Month]>=@LaunchMonth 
						   AND CarVersionId = @VersionId 

				------------------ Copying data into TC_TMTargetScenarioDetail for the desired version-------------------------

				 INSERT INTO TC_TMIntermediateLegacyDetail 
							(DealerId, 
							 CarVersionId, 
							 Month, 
							 Year, 
							 Target) 
				SELECT DealerId, 
						@VersionId, 
						Month, 
						Year, 
						Target 
				 FROM   TC_TMIntermediateLegacyDetail WITH(NOLOCK) 
				 WHERE    [Month]>=@LaunchMonth 
						AND CarVersionId = @CopyFromVersionId



				-------------Updating the new target for the desired version-----------------------------------
					 IF (@TargetPercentageChange <> 0)
					  BEGIN
							 UPDATE TC_TMIntermediateLegacyDetail 
							 SET    Target = ROUND( (Target + (@TargetPercentageChange*Target)/100),0)
							 WHERE  [Month]>=@LaunchMonth 
									AND CarVersionId = @VersionId

							EXEC [dbo].[TC_TMRoundOffHandling] 
									@ActualTarget=@NewTargetValue ,
									@TC_BrandZoneId=NULL,
									@CarModelId =NULL,
									@TC_AMId =NULL,
									@DealerId  =NULL,
									@CarVersionId  =@VersionId,
									@StartMonth =@LaunchMonth,
									@EndMonth  =12,
									@MonthId=NULL,
									@TC_TMDistributionPatternMasterId=NULL
					  END 
			END
		ELSE
			BEGIN
			
				--------------- Used to delete the data from TC_TMTargetScenarioDetail table if the data for version already exists----
				--------------- to which user wants to apply the target ------------------------------------------------------------
		 
			IF NOT EXISTS(SELECT TOP 1 * 
				FROM   TC_TMTargetScenarioDetail 
				WHERE  CarVersionId = @VersionId AND TC_TMDistributionPatternMasterId = @CopyFromTC_TMDistributionPatternMasterId ) 
					BEGIN 
					  SET @Month = 1 
					  
					  WHILE (@Month <= 12) 
						BEGIN 
							INSERT INTO TC_TMTargetScenarioDetail(DealerId, CarVersionId, Month, Year, Target, TC_TMDistributionPatternMasterId) 
							SELECT D.ID, @VersionId, @Month, @LaunchYear, 0, @CopyFromTC_TMDistributionPatternMasterId
							FROM Dealers D
							WHERE TC_BrandZoneId IN(SELECT TC_BrandZoneId FROM TC_BrandZone TZ WHERE TZ.MakeId = 20)
							
							SET @Month = @Month + 1
						END 
				  END 
				  
				 DELETE FROM TC_TMTargetScenarioDetail 
				 WHERE  TC_TMDistributionPatternMasterId = @TC_TMDistributionPatternMasterId 
						   AND  [Month]>=@LaunchMonth 
						   AND CarVersionId = @VersionId 

				------------------ Copying data into TC_TMTargetScenarioDetail for the desired version-------------------------

				 INSERT INTO TC_TMTargetScenarioDetail 
							(DealerId, 
							 CarVersionId, 
							 Month, 
							 Year, 
							 Target,
							 TC_TMDistributionPatternMasterId) 
				SELECT DealerId, 
						@VersionId, 
						Month, 
						Year, 
						Target,
						@TC_TMDistributionPatternMasterId 
				 FROM   TC_TMTargetScenarioDetail WITH(NOLOCK) 
				 WHERE  TC_TMDistributionPatternMasterId = @CopyFromTC_TMDistributionPatternMasterId 
						AND  [Month]>=@LaunchMonth 
						AND CarVersionId = @CopyFromVersionId



				-------------Updating the new target for the desired version-----------------------------------
					 IF (@TargetPercentageChange <> 0)
					  BEGIN
							 UPDATE TC_TMTargetScenarioDetail 
							 SET    Target = ROUND( (Target + (@TargetPercentageChange*Target)/100),0)
							 WHERE  TC_TMDistributionPatternMasterId = @TC_TMDistributionPatternMasterId 
									AND  [Month]>=@LaunchMonth 
									AND CarVersionId = @VersionId

							EXEC [dbo].[TC_TMRoundOffHandling] 
									@ActualTarget=@NewTargetValue ,
									@TC_BrandZoneId=NULL,
									@CarModelId =NULL,
									@TC_AMId =NULL,
									@DealerId  =NULL,
									@CarVersionId  =@VersionId,
									@StartMonth =@LaunchMonth,
									@EndMonth  =12,
									@MonthId=NULL,
									@TC_TMDistributionPatternMasterId=@TC_TMDistributionPatternMasterId
					  END 
			END

  END
