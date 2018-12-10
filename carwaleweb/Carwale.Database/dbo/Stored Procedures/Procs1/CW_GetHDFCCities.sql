IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[CW_GetHDFCCities]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[CW_GetHDFCCities]
GO

	CREATE PROCEDURE [dbo].[CW_GetHDFCCities]
AS
--Author:Rakesh yadav on 02 Aug 2015
--desc: show all spoke and city
BEGIN

	SELECT CC.Id AS CCId, S.ID AS StateId,S.Name AS StateName,C.ID AS CityId,C.Name AS CityName,SC.ID AS SpokeCityId, SC.Name AS SpokeCity,CT.Category,CT.Id AS CatId,CC.IsActive
	FROM 
	CW_CarCities CC
	JOIN Cities C ON CC.CW_CityId=C.ID
	JOIN Cities SC ON CC.SpokeCityId=SC.ID
	JOIN States S ON C.StateId=S.ID
	JOIN CW_CityCategory CT ON CC.CatId=CT.Id

END