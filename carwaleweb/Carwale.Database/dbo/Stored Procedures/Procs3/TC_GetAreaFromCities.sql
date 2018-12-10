IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_GetAreaFromCities]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_GetAreaFromCities]
GO
	CREATE PROCEDURE [dbo].[TC_GetAreaFromCities]  
@CityId BIGINT  
AS  
BEGIN
	SET NOCOUNT ON;  
	SElECT A.ID AS Value,A.Name AS Text FROM Areas A	
	WHERE A.CityId = @CityId AND A.IsDeleted=0
	ORDER BY A.Name
END
