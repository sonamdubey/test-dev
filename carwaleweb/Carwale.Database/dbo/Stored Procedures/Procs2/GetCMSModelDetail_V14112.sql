IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[GetCMSModelDetail_V14112]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[GetCMSModelDetail_V14112]
GO

	--=================================
-- Created by Natesh Kumar on 11/13/14
-- for getting modelname and modelid to fill dropdown list whose roadtest or comparison test is available
-- exec [dbo].[GetCMSModelDetail_V14112] 2,'8',4
--=================================
CREATE PROCEDURE [dbo].[GetCMSModelDetail_V14112]
@ApplicationId TINYINT
,@CategoryIds VARCHAR(60)
,@MakeId INT

AS
BEGIN
---for modelname and modelid in carwale
	IF (@ApplicationId = 1)
		BEGIN
			SELECT DISTINCT CM.Name AS ModelName
			,CM.ID AS ModelId
			,CM.MaskingName AS MaskingName 
			FROM
			CarModels CM WITH (NOLOCK)
			LEFT JOIN Con_EditCms_Cars CC WITH (NOLOCK) ON CC.ModelId = CM.ID
			LEFT JOIN Con_EditCms_Basic CB WITH (NOLOCK) ON CC.BasicId = CB.Id
			LEFT JOIN CarMakes BM WITH (NOLOCK) ON CC.MakeId = BM.ID

			WHERE CB.ApplicationID = @ApplicationId
				AND CB.CategoryId IN (
								SELECT ListMember
								FROM fnSplitCSVValuesWithIdentity(@CategoryIds)
								) 
				AND CC.MakeId = @MakeId	
				AND CB.IsActive = 1
				AND CB.IsPublished = 1
				AND CM.IsDeleted = 0
				AND CM.Futuristic = 0
				AND BM.Futuristic = 0
			ORDER BY CM.Name ASC
		END

	ELSE
	---for modelname and modelid in bikewale
		IF (@ApplicationId = 2)
			BEGIN
				SELECT DISTINCT CM.Name AS ModelName
				,CM.ID AS ModelId 
				,CM.MaskingName AS MaskingName 
				FROM
				BikeModels CM WITH (NOLOCK)
				LEFT JOIN Con_EditCms_Cars CC WITH (NOLOCK) ON CC.ModelId = CM.ID
				LEFT JOIN Con_EditCms_Basic CB WITH (NOLOCK) ON CC.BasicId = CB.Id 
				LEFT JOIN BikeMakes BM WITH (NOLOCK) ON CC.MakeId = BM.ID
				WHERE CB.ApplicationID = @ApplicationId
					AND CB.CategoryId IN (
									SELECT ListMember
									FROM fnSplitCSVValuesWithIdentity(@CategoryIds)
									) 
					AND CC.MakeId = @MakeId	
					AND CB.IsActive = 1
					AND CB.IsPublished = 1
					AND CM.IsDeleted = 0
					AND CM.Futuristic = 0
					AND BM.Futuristic = 0
				ORDER BY CM.Name ASC
			END
END
