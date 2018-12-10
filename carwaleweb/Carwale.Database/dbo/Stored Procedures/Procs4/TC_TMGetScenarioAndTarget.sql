IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_TMGetScenarioAndTarget]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_TMGetScenarioAndTarget]
GO

	-- =============================================
-- Author	    :	Umesh Ojha
-- Create date	:	18 Dec 2013
-- Description	:	To get Scenario and legacy
-- Modified By  :	Checking null for CreatedBy
-- exec TC_TMGetScenarioAndTarget
-- =============================================
CREATE  PROCEDURE [dbo].[TC_TMGetScenarioAndTarget]
 AS
BEGIN	      
	SELECT  DISTINCT  [Year] AS Value,
	                  'Final Target For '+CONVERT(VARCHAR(4),[Year]) AS [Text],
	                  [Year] AS LegacyYear,
					  1 AS IsFinalTarget,
	                  9  AS CreatedBy  ---- taking hard coded Schin jain user id for release urgency short time to resolve this issue.
	from TC_DealersTarget	

	--EXEC TC_TMGetLegacyMaster

	SELECT	TC_TMDistributionPatternMasterId AS Value,
			LegacyName AS Text,			
			LegacyYear,0 AS IsFinalTarget,CreatedBy
	FROM TC_TMDistributionPatternMaster WITH (NOLOCK) 
	WHERE IsActive=1 AND IsDataHistorical=0	

END


