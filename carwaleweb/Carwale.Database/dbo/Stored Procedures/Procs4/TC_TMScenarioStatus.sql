IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_TMScenarioStatus]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_TMScenarioStatus]
GO

	-- =============================================
-- Author:		Vishal Srivastava AE 1830
-- Create date: 28 November 2013
-- Description:	To check the user status of saved or pending scenario
-- =============================================
CREATE PROCEDURE [dbo].[TC_TMScenarioStatus] 
	-- Add the parameters for the stored procedure here
AS
BEGIN
	SELECT 
			TCTMCUL.TC_SpecialUsersId,
			TCSP.UserName,
			TCTMCUL.LastUpdatedOn
	FROM 
			TC_TMCheckUserLogin AS TCTMCUL WITH(NOLOCK)
			JOIN TC_SpecialUsers AS TCSP WITH(NOLOCK)
				ON TCTMCUL.TC_SpecialUsersId=TCSP.TC_SpecialUsersId
END
