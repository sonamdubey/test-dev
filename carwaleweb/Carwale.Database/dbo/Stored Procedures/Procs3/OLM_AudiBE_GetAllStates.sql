IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[OLM_AudiBE_GetAllStates]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[OLM_AudiBE_GetAllStates]
GO

	
-- =============================================================================================
-- Author:		Ashish G. Kamble
-- Create date: 29 July 2013
-- Description:	SP to get all states
-- EXEC OLM_AudiBE_GetAllStates
-- =============================================================================================
CREATE PROCEDURE [dbo].[OLM_AudiBE_GetAllStates]
AS
BEGIN
	
	SET NOCOUNT ON;

	Select Distinct CWS.ID AS StateId, CWS.Name AS StateName
	FROM States CWS	WITH (NOLOCK)
	WHERE CWS.IsDeleted = 0
	ORDER BY CWS.Name
	
END
