IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[GetCityNameByCityId]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[GetCityNameByCityId]
GO

	
-- =============================================
-- Author:		Kirtan Shetty
-- Create date: 23/7/14
-- Description:	Gets the City Name by City Id
-- =============================================
CREATE PROCEDURE [dbo].[GetCityNameByCityId]
	@CityId INT
AS
BEGIN
	SET NOCOUNT ON;

    SELECT Name AS CityName FROM Cities WITH(NOLOCK) WHERE Id = @CityId AND IsDeleted = 0 

END

