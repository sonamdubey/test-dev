IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[BhartiAxa_GetAllStates]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[BhartiAxa_GetAllStates]
GO

	-- =============================================
-- Author:		Akansha
-- Create date: 27.02.2014
-- Description:	Get all the States for bharti axa
-- =============================================
CREATE PROCEDURE [dbo].[BhartiAxa_GetAllStates]
AS
BEGIN
	SELECT DISTINCT RTL.Cw_state Text, RTL.CarwaleStateId AS Value
	FROM [RTO Location] RTL with(nolock)
	Order by Text
END


