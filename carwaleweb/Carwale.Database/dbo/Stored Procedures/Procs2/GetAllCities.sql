IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[GetAllCities]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[GetAllCities]
GO

	
-- =============================================
-- Author:		<Anuj Dhar>
-- Create date: <12/01/2016>
-- Description:	<Fetch all cities>
-- =============================================
CREATE PROCEDURE [dbo].[GetAllCities]
AS
BEGIN
	SELECT ID AS Id, StateId, Name
	FROM Cities WITH (NOLOCK)
	WHERE IsDeleted = 0 
	ORDER BY Name
END

