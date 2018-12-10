IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_TMGetModelVersionWiseTarget]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_TMGetModelVersionWiseTarget]
GO

	-- =============================================
-- Author	:	    Manish Chourasiya
-- Create date	:	13-11-2013
-- Description	:	Get Model version wise cars for the second screen for AutoBiz2 	
-- EXEC  [dbo].[TC_TMGetModelVersionWiseTarget] 1,40000,NULL,2013
-- =============================================
CREATE  PROCEDURE [dbo].[TC_TMGetModelVersionWiseTarget] 
	@TC_TMDistributionPatternMasterId INT,
	@AnnualTarget INT,
	@TC_SpecialUsersId INT =NULL,
	@Year SMALLINT
 AS
	BEGIN
	       SET NOCOUNT ON;

		DECLARE @IsDataHistorical BIT,
		        @TotalTargetInTargetScenario INT

		SELECT @IsDataHistorical=IsDataHistorical  
		FROM TC_TMDistributionPatternMaster 
		WHERE TC_TMDistributionPatternMasterId=@TC_TMDistributionPatternMasterId
		
		 IF NOT EXISTS (SELECT TC_TMCheckUserLoginId FROM TC_TMCheckUserLogin WHERE TC_SpecialUsersId=@TC_SpecialUsersId AND IsUserLogged=1 )
		 
		 BEGIN
			 TRUNCATE TABLE [TC_TMIntermediateLegacyDetail];

			 TRUNCATE TABLE [TC_TMCheckUserLogin];

			 DELETE FROM  TC_TMMarketParameter WHERE TC_TMDistributionPatternMasterId IS NULL;

			 INSERT INTO TC_TMCheckUserLogin (TC_SpecialUsersId,
  											  IsUserLogged,
											  CreatedOn,
											  LastUpdatedOn)
                                      VALUES (@TC_SpecialUsersId,
									           1,
											   GETDATE(),
											   GETDATE()
									          )
			  		  
			
			
			IF (@IsDataHistorical=1)
			BEGIN 
			   INSERT INTO [TC_TMIntermediateLegacyDetail] (DealerId,
															CarVersionId,
															Month,
															Year,
															Target
														  )
							 SELECT DealerId,
									CarVersionId,
									Month,
									@Year,
									ROUND((TargetPercentage*@AnnualTarget)/100.000000000000,0) AS [Target]
							 FROM  TC_TMDistributionPercentage  WITH (NOLOCK) 
							 WHERE TC_TMDistributionPatternMasterId=@TC_TMDistributionPatternMasterId
			 END 
			  ELSE 
			    BEGIN 
				
					SELECT @TotalTargetInTargetScenario=SUM(Target)
					FROM TC_TMTargetScenarioDetail  WITH (NOLOCK) 
					WHERE  TC_TMDistributionPatternMasterId=@TC_TMDistributionPatternMasterId;
				
					INSERT INTO [TC_TMIntermediateLegacyDetail] (DealerId,
																CarVersionId,
																Month,
																Year,
																Target
															  )
								 SELECT DealerId,
										CarVersionId,
										Month,
										@Year,
										ROUND(@AnnualTarget * ((Target * 1.000000)/@TotalTargetInTargetScenario),0) AS [Target]
								 FROM  TC_TMTargetScenarioDetail   WITH (NOLOCK) 
								 WHERE TC_TMDistributionPatternMasterId=@TC_TMDistributionPatternMasterId
			    END 
		 


		      EXEC [dbo].[TC_TMRoundOffHandling] 
					 	@ActualTarget=@AnnualTarget ,
						@TC_BrandZoneId=NULL,
						@CarModelId =NULL,
						@TC_AMId =NULL,
						@DealerId  =NULL,
						@CarVersionId  =NULL,
						@StartMonth =1,
						@EndMonth  =12,
						@MonthId =NULL,
	                    @TC_TMDistributionPatternMasterId=NULL
		
		
			  SELECT   CM.ID   CarId,
					   CM.Name CarName,
					   CV.Id   FieldId,
					   CV.Name FieldName,				
				   -- VC.CarVersionCode VersionCode,	   
					   ROUND( SUM(Target),0) AS Target
			   FROM  [TC_TMIntermediateLegacyDetail]  AS TM  WITH (NOLOCK) 
						 JOIN  Dealers AS D  WITH (NOLOCK)  ON D.ID=TM.DealerId
						 JOIN  TC_BrandZone  AS TCB  WITH (NOLOCK)  ON TCB.TC_BrandZoneId=D.TC_BrandZoneId
						 JOIN  CarVersions AS CV  WITH (NOLOCK) ON    CV.ID=TM.CarVersionId
						 JOIN  CarModels AS CM WITH (NOLOCK) ON CV.CarModelId=CM.ID
					--	 JOIN TC_VersionsCode AS VC ON VC.CarVersionId=CV.ID
					--	 JOIN TC_SpecialUsers AS S ON S.TC_SpecialUsersId=D.TC_AMId
			   GROUP BY CM.ID,CM.Name,CV.ID,CV.Name --,VC.CarVersionCode
			   ORDER BY CM.ID,CV.ID
		 END
	 
	END
