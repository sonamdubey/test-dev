IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_TMScenarioCount]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_TMScenarioCount]
GO

	-- =============================================
-- Author:		Vishal Srivastava AE 1830
-- Create date: 28 November 2013 1714 HRS IST
-- Description:	Get the total count of scenario 
-- =============================================
CREATE PROCEDURE [dbo].[TC_TMScenarioCount]
	-- Add the parameters for the stored procedure here
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT COUNT(TC_TMDistributionPatternMasterId) AS ScenarioCount
		FROM TC_TMDistributionPatternMaster AS TCTMDPM WITH(NOLOCK) 
			WHERE TCTMDPM.IsDataHistorical=0 
			--AND TCTMDPM.IsTargetApplied=0 
			AND TCTMDPM.IsActive=1
END