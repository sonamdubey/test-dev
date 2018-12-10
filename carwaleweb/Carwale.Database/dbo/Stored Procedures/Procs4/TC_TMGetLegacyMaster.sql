IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_TMGetLegacyMaster]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_TMGetLegacyMaster]
GO

	-- =============================================
-- Author	    :	Tejashree Patil
-- Create date	:	16th Oct 2013
-- Description	:	To get TMLegacy list.
-- =============================================
CREATE  PROCEDURE [dbo].[TC_TMGetLegacyMaster]
 AS
BEGIN
	      
	SELECT	TC_TMDistributionPatternMasterId AS Value,
			LegacyName AS Text,
			IsDataHistorical,
			LegacyYear,IsTargetApplied,CreatedBy
	FROM TC_TMDistributionPatternMaster WITH (NOLOCK) 
	WHERE IsActive=1		  

END
