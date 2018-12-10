IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[GetAllCitiesInfo]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[GetAllCitiesInfo]
GO

	
CREATE PROCEDURE [dbo].[GetAllCitiesInfo]
AS
BEGIN
--created by rakesh yadav on 7 june 2016 to fetch all cities information
	SELECT C.ID AS CityId
		,C.NAME AS CityName
		,StateId
		,C.IsDeleted
		--,IsUniversal
		,Lattitude
		,Longitude
		,StdCode
		--,DefaultPinCode
		--,UsedCarRating
		,IsPopular
		--,CityImageUrl
		,CityEntryDate
		,CityMaskingName
		,BWCityOrder
		,S.Name as StateName
	FROM Cities C with(nolock)
	JOIN States S with(nolock) on C.StateId=S.ID
	where C.IsDeleted = 0 and S.IsDeleted=0
	ORDER BY C.Name
END

