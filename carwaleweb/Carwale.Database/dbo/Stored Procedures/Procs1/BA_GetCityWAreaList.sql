IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[BA_GetCityWAreaList]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[BA_GetCityWAreaList]
GO

	-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[BA_GetCityWAreaList]
	@CityId INT = NULL
AS
BEGIN
	SET NOCOUNT ON;
	SELECT A.ID AS ID,A.Name AS Name FROM Areas AS A  WITH(NOLOCK) WHERE A.IsDeleted = 0 AND A.CityId = @CityId

END
