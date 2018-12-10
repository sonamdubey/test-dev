IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[BhartiAxa_GetAllCities]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[BhartiAxa_GetAllCities]
GO

	
-- =============================================
-- Author:		Akansha
-- Create date: 27.02.2014
-- Description:	Get cities for particular state for bharti axa
-- =============================================
CREATE PROCEDURE [dbo].[BhartiAxa_GetAllCities] @StateId int
AS

BEGIN
	SELECT DISTINCT RTL.CarWaleCityId AS Value ,RTL.Cw_city AS Text
	FROM [RTO Location] RTL with(nolock)
	WHERE LOWER(RTL.CarwaleStateId) = @StateId AND Zone IS NOT NULL
	ORDER BY  Text
END



