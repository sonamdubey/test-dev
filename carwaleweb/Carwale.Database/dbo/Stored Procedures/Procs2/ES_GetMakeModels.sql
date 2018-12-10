IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[ES_GetMakeModels]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[ES_GetMakeModels]
GO

	-- =============================================
-- Author:		Ashwini
-- Create date: 16-Sept-2015
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[ES_GetMakeModels]
AS
BEGIN
	SET NOCOUNT ON;

	SELECT MK.ID AS MakeId
		,MK.NAME AS MakeName
		,M.id AS ModelId
		,M.NAME AS ModelName
		,R.RootId AS RootId
		,R.RootName AS RootName
		,M.MaskingName AS MaskingName
		,M.New
		,1 Used
		,MK.NAME + ' ' + M.NAME DisplayName
		,m.Futuristic
	FROM CarMakes MK WITH(NOLOCK)
	JOIN CarModels M WITH(NOLOCK) ON M.CarMakeId = MK.ID
	JOIN CarModelRoots R WITH(NOLOCK) ON R.RootId = M.RootId
	WHERE M.IsDeleted = 0
		AND MK.IsDeleted = 0
		AND M.new = 1
	
	UNION
	
	SELECT MK.ID AS MakeId
		,MK.NAME AS MakeName
		,0 AS ModelId
		,'' AS ModelName
		,R.RootId AS RootId
		,R.RootName AS RootName
		,'' AS MaskingName
		,0 New
		,1 Used
		,MK.NAME + ' ' + R.RootName DisplayName
		,MK.Futuristic
	FROM CarMakes MK WITH(NOLOCK)
	JOIN (
		SELECT rootid
			,rootname
			,MakeId
		FROM CarModelRoots WITH(NOLOCK)
		
		EXCEPT
		
		SELECT r.RootId AS rootid
			,r.RootName AS rootname
			,r.MakeId
		FROM CarMakes MK WITH(NOLOCK)
		JOIN CarModels M WITH(NOLOCK) ON M.CarMakeId = MK.ID
		JOIN CarModelRoots R WITH(NOLOCK) ON R.RootId = M.RootId
		WHERE m.IsDeleted = 0
			AND mk.IsDeleted = 0
			AND m.new = 1
		) R ON R.MakeId = mk.ID
	WHERE mk.IsDeleted = 0
	
	UNION
	
	SELECT ID AS MakeId
		,NAME AS MakeName
		,0 AS ModelId
		,'' AS ModelName
		,0 AS RootId
		,'' AS RootName
		,NAME AS MaskingName
		,1 AS New
		,1 Used
		,NAME AS MaskingName
		,Futuristic
	FROM CarMakes WITH (NOLOCK)
	WHERE IsDeleted = 0
END
