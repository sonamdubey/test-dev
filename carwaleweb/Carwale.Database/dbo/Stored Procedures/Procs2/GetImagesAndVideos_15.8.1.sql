IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[GetImagesAndVideos_15]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[GetImagesAndVideos_15]
GO

	
-- =============================================
-- Author:		Akansha
-- Create date: 08.10.2013
-- Description:	Get Latest Photos and Videos based on Publish date
-- Modified by : Akansha
-- Description : Added Masking name column
-- Modified by: Natesh on 20-7-2014 Added Application id flag for CMS merging
-- =============================================
CREATE PROCEDURE [dbo].[GetImagesAndVideos_15.8.1] -- EXEC GetImagesAndVideos 8
	@Top INT = 8,
	@ApplicationId int
AS
BEGIN
	WITH CTE
	AS (
		SELECT TOP (10) EB.Id
			,EB.CategoryId
			,EB.Title
			,EB.Description
			,EI.HostUrl
			,EI.OriginalImgPath
			,MK.NAME MakeName
			,MO.NAME ModelName
			,MO.MaskingName
			,ROW_NUMBER() OVER (
				PARTITION BY EB.ID ORDER BY EB.ID DESC
				) ROW_NO
		FROM Con_EditCms_basic EB
		INNER JOIN Con_EditCms_Cars TC ON TC.BasicId = EB.ID
		INNER JOIN CarModels MO ON MO.Id = TC.ModelId
		INNER JOIN CarMakes MK ON MK.Id = MO.CarMakeId
		LEFT JOIN Con_EditCms_Images EI ON EI.BasicId = EB.Id AND EB.IsPublished = 1
			AND EI.IsActive =1
			And EI.IsMainImage=1
		WHERE EB.CategoryId IN (
				10
				--,13
				)
			AND EB.ApplicationID = @ApplicationId
			AND EB.IsPublished = 1 
		ORDER BY EB.PublishedDate DESC
		)
	SELECT TOP (@Top) *
	FROM CTE
	WHERE Row_No = 1
END
