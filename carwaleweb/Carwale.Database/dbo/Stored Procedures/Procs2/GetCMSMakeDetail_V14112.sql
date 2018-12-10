IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[GetCMSMakeDetail_V14112]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[GetCMSMakeDetail_V14112]
GO

	--=================================
-- Created by Natesh Kumar on 11/13/14
-- for getting makename and makeid to fill dropdown list whose roadtest or comparison test is available

--=================================
CREATE PROCEDURE [dbo].[GetCMSMakeDetail_V14112]
@ApplicationId TINYINT
,@CategoryIds VARCHAR(60)

AS
BEGIN
---for makename and makeid in carwale
	IF (@ApplicationId = 1)
		BEGIN
			SELECT DISTINCT CM.Name AS MakeName
			,CM.ID AS MakeId 
			,null AS MakeMaskingName
			FROM 
			CarMakes CM with (NOLOCK)
			INNER JOIN Con_EditCms_Cars CC WITH (NOLOCK) ON CC.MakeId = CM.ID
			INNER JOIN Con_EditCms_Basic CB WITH (NOLOCK) ON CC.BasicId = CB.Id		
					AND CB.IsActive = 1
					AND CB.IsPublished = 1
			INNER JOIN CarModels BM WITH (NOLOCK) ON CC.ModelId = BM.ID

			WHERE CB.ApplicationID = @ApplicationId
				AND CB.CategoryId IN (
								SELECT ListMember
								FROM fnSplitCSVValuesWithIdentity(@CategoryIds)
								)
				AND CM.IsDeleted = 0
				AND BM.IsDeleted = 0
				AND CM.Futuristic = 0
				AND BM.Futuristic = 0
			ORDER BY CM.Name ASC				 
		END

	ELSE
	---for makename and makeid in bikewale
		IF (@ApplicationId = 2)
			BEGIN
				SELECT DISTINCT CM.Name AS MakeName
				,CM.ID AS MakeId 
				,CM.MaskingName AS MakeMaskingName
				FROM 
				BikeMakes CM with (NOLOCK)
				INNER JOIN Con_EditCms_Cars CC WITH (NOLOCK) ON CC.MakeId = CM.ID
				INNER JOIN Con_EditCms_Basic CB WITH (NOLOCK) ON CC.BasicId = CB.Id
						AND CB.IsActive = 1
						AND CB.IsPublished = 1
				INNER JOIN BikeModels BM WITH (NOLOCK) ON CC.ModelId = BM.ID

				WHERE CB.ApplicationID = @ApplicationId
					AND CB.CategoryId IN (
									SELECT ListMember
									FROM fnSplitCSVValuesWithIdentity(@CategoryIds)
									) 
					AND CM.IsDeleted = 0
					AND BM.IsDeleted = 0
					AND CM.Futuristic = 0
					AND BM.Futuristic = 0
				ORDER BY CM.Name ASC
			END
END




/****** Object:  StoredProcedure [CD].[GetPQFeaturedVersionIDByVersionID_API]    Script Date: 2/16/2015 5:10:46 PM ******/
-- SET ANSI_NULLS ON
