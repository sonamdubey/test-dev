IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[GetDealerAreaList]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[GetDealerAreaList]
GO

	----------------------------------------------
-- Author : Sadhana Upadhyay on 5 Nov 2014
-- Summary : To get dealer Area mapping detail by city id
-- EXEC GetDealerAreaList 1
----------------------------------------------
CREATE PROCEDURE [dbo].[GetDealerAreaList] 
@CityId INT
AS
BEGIN
	SELECT A.id AS AreaId
		,A.NAME AS AreaName
		,A.CityId
		,A.PinCode
		,ISNULL(DAM.DealerId,0) AS DealerId
		,D.Organization
		,BM.Name AS MakeName
		,COUNT(DAM.DealerId) OVER (PARTITION BY DAM.areaId) AS DealerCount
		,ROW_NUMBER() OVER ( PARTITION BY A.id ORDER BY A.id ) As DealerRank
	FROM Areas A WITH(NOLOCK)
	LEFT JOIN BW_DealerAreaMapping DAM WITH(NOLOCK) ON A.ID = DAM.AreaId AND DAM.IsActive = 1
	LEFT JOIN dealers D WITH(NOLOCK) ON d.id = DAM.DealerId AND D.ApplicationId=2 AND D.IsDealerActive=1
	LEFT JOIN TC_DealerMakes DM ON DM.DealerId = D.ID
	LEFT JOIN BikeMakes BM ON BM.ID = DM.MakeId
	WHERE A.CityId = @CityId AND A.IsDeleted = 0
	ORDER BY A.Name
END
