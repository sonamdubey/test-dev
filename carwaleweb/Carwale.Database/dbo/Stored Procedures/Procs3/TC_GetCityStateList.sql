IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_GetCityStateList]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_GetCityStateList]
GO
	-- =============================================
-- Author:		Ashwini Dhamankar
-- Create date: April 13,2016
-- Description:	To get city-state
-- =============================================
CREATE PROCEDURE [dbo].[TC_GetCityStateList]
AS
BEGIN
	SET NOCOUNT ON;

    SELECT C.ID AS Value, C.Name+ ' - ' + S.Name AS Text 
	FROM Cities C WITH(NOLOCK)
	INNER JOIN States S WITH(NOLOCK) ON S.ID= C.StateId
	WHERE C.IsDeleted = 0 AND S.IsDeleted = 0 ORDER BY Text
END

-----------------------------


