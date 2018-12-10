IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[GetEditCmsArticleImages]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[GetEditCmsArticleImages]
GO

	
-- =============================================
-- Author		:		Vikas
-- Create date	:		22/08/2012
-- Description	:		To get the images for a particular article
-- =============================================
CREATE PROCEDURE [dbo].[GetEditCmsArticleImages] 	
	@BasicId Numeric
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	Select CP.Name As CategoryName, Cma.Name As MakeName, Cmo.Name As ModelName, CI.HostUrl, CI.ImagePathLarge, CI.ImagePathThumbnail, CI.Caption 
    From Con_EditCms_Images CI 
    Inner Join Con_PhotoCategory CP On CP.Id = CI.ImageCategoryId 
    Left Join CarModels Cmo On Cmo.Id = CI.ModelId 
    Left Join CarMakes Cma On Cma.Id = CI.MakeId 
    Where BasicId = @BasicId And IsActive = 1 Order By Sequence
    
END

