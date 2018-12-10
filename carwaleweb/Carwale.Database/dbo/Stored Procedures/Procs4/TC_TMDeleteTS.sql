IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_TMDeleteTS]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_TMDeleteTS]
GO

	-- =============================================
-- Author	    :	Vinayak Patil
-- Create date	:	21-11-2013
-- Description	:	To take the backup of the target scenario.
-- =============================================
 CREATE PROCEDURE [dbo].[TC_TMDeleteTS]
 @TC_TMDistributionPatternMasterId INT,
 @TC_SpecialUsersId INT 


 AS
   BEGIN

		------------------- insert data into archive table as a backup before deleting it--------------------------------

		INSERT INTO TC_TMTargetScenarioArchive 
					(DealerId, 
					 CarVersionId, 
					 Month, 
					 Year, 
					 Target, 
					 TC_TMDistributionPatternMasterId, 
					 TC_SpecialUsersId) 
		SELECT DealerId, 
			   CarVersionId, 
			   Month, 
			   Year, 
			   Target, 
			   TC_TMDistributionPatternMasterId, 
			   @TC_SpecialUsersId 
		FROM   TC_TMTargetScenarioDetail WITH (NOLOCK)
		WHERE  TC_TMDistributionPatternMasterId = @TC_TMDistributionPatternMasterId 


			--------------------Delete data from TC_TMTargetScenarioDetail table--------------------------------

			DELETE FROM TC_TMTargetScenarioDetail 
			WHERE  TC_TMDistributionPatternMasterId = @TC_TMDistributionPatternMasterId 


			--------------------Update IsActive bit to 0 in TC_TMDistributionPatternMaster------------------------
			--------------------beacuse it is no longer in use because new record is created----------------------

			UPDATE TC_TMDistributionPatternMaster 
			SET    IsActive = 0 
			WHERE  TC_TMDistributionPatternMasterId = @TC_TMDistributionPatternMasterId 



END
