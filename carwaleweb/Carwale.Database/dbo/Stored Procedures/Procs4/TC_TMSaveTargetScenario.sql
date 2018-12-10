IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_TMSaveTargetScenario]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_TMSaveTargetScenario]
GO

	-- =============================================
-- Author	    :	Manish Chourasiya
-- Create date	:	07-11-2013
-- Description	:	For saving Target Scenario
-- =============================================
CREATE PROCEDURE [dbo].[TC_TMSaveTargetScenario]
@TargetScenarioName VARCHAR(80),
@TC_SpecialUserId INT,
@TargetScenarioId INT OUTPUT
 AS
	BEGIN
	     
		DECLARE   @TC_TMDistributionPatternMasterId INT
		DECLARE   @Year SMALLINT

		 SELECT TOP 1 @Year=[Year] FROM TC_TMIntermediateLegacyDetail;

         INSERT INTO TC_TMDistributionPatternMaster
												 (LegacyName,
												 IsDataHistorical,
												 LegacyYear,
												 CreatedOn,
												 CreatedBy,
												 IsTargetApplied,
												 IsActive)
						                  VALUES ( @TargetScenarioName+' '+CONVERT(VARCHAR(20),GETDATE(), 100),
										           0,
												   @Year,
												   GETDATE(),
												   @TC_SpecialUserId,
												   0,
												   1
										         )

          SET @TargetScenarioId=SCOPE_IDENTITY()

		  INSERT INTO [dbo].[TC_TMTargetScenarioDetail]
														(DealerId,
														CarVersionId,
														[Month],
														[Year],
														Target,
                                                        TC_TMDistributionPatternMasterId)
		   SELECT  DealerId,
		           CarVersionId,
                   [Month],
				   [Year],
				   [Target],
				   @TargetScenarioId
	      FROM TC_TMIntermediateLegacyDetail WITH(NOLOCK) ;

		 TRUNCATE TABLE TC_TMIntermediateLegacyDetail;

		 TRUNCATE TABLE TC_TMCheckUserLogin;

		 ---------- TO UPDATE ALL THE RECORDS FROM THE TC_TMTargetCopyData TABLE WHICH SAVES INFO ABOUT ALL THE 
		---------- TARGETS COPIED BY THAT PARTICULAR USER WITHOUT SAVING IT IN TC_TMTargetScenarioDetail
		
		UPDATE TC_TMTargetCopyData
	    SET    TC_TMDistributionPatternMasterId = @TargetScenarioId
		WHERE TC_TMDistributionPatternMasterId = -1  

		RETURN @TargetScenarioId



	END
