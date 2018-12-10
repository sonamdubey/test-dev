IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[Con_EditCms_UnPublishArticle]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[Con_EditCms_UnPublishArticle]
GO

	
-- Created By : Ashish G. Kamble on 20 Aug 2013
-- Description : Proc to unpublish the article.
-- Modified By : Ashwini Todkar on 24 July 2014 
-- added insert query to log the unpublished articles in Con_Editcms_UnpublishedArticleLog rather than Con_EditCms_Basic
-- Modified by:Rakesh yadav On 7 jan 2016, added output parameter @ModelId and @MakeId
CREATE PROCEDURE [dbo].[Con_EditCms_UnPublishArticle] @BasicId INT
	,@CustomerId INT
	,@ReasonToUnpublish VARCHAR(500),
	@ModelId INT= 0 OUTPUT,
	@MakeId INT= 0 OUTPUT
AS
BEGIN
	----Update road test to publish in basic table  
	--UPDATE Con_EditCms_Basic
	--SET IsPublished = 0
	--	,UnPublishedDate = GETDATE()
	--	,UnPublishedBy = @CustomerId
	--	,ReasonToUnpublish = @ReasonToUnpublish
	--WHERE ID = @BasicId
		DECLARE @IsPublished BIT

	SELECT @IsPublished = IsPublished,@ModelId=CMO.ID,@MakeId=CMA.ID
	FROM Con_EditCms_Basic AS CEB WITH(NOLOCK)
	LEFT JOIN Con_EditCms_Cars CEC WITH(NOLOCK) On CEB.Id = CEC.BasicId  
     LEFT JOIN CarMakes CMA WITH(NOLOCK) ON CEC.MakeId = CMA.ID  
     LEFT JOIN CarModels CMO WITH(NOLOCK) ON CEC.ModelId = CMO.ID  
     LEFT JOIN CarVersions CV WITH(NOLOCK) ON CEC.VersionId = CV.ID 
	WHERE CEB.Id = @BasicId 

	IF @IsPublished = 1
	BEGIN
		INSERT INTO Con_Editcms_UnpublishedArticleLog (
			BasicId
			,ReasonToUnpublish
			,UnpublishBy
			,UnpublishDate
			)
		VALUES (
			@BasicId
			,@ReasonToUnpublish
			,@CustomerId
			,GETDATE()
			)

		UPDATE Con_EditCms_Basic
		SET IsPublished = 0
		WHERE Id = @BasicId 
	END
END

