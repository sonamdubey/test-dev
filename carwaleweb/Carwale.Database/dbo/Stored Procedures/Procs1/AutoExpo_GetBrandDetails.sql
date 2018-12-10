IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[AutoExpo_GetBrandDetails]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[AutoExpo_GetBrandDetails]
GO

	-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[AutoExpo_GetBrandDetails] --exec AutoExpo_GetBrandDetails 18
	-- Add the parameters for the stored procedure here
	@MakeId numeric,
	@TopCount int=2

AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here

	  ---to retrieve news details
	   SELECT DISTINCT CB.Id AS BasicId
			,CB.AuthorName
			,CB.Description
			,CB.DisplayDate
			,CB.VIEWS
			,CB.Title
			,CB.Url
			,CEI.HostUrl
			,CEI.ImagePathThumbnail
			,CEI.ImagePathLarge
			,CEI.IsMainImage
			,ROW_NUMBER() OVER (
				ORDER BY DisplayDate DESC
				) AS Row_No
			,CB.IsSticky
			,SPC.FacebookCommentCount
		FROM Con_EditCms_Basic AS CB  WITH(NOLOCK)
		INNER JOIN Con_EditCms_Cars CC 
		    on CB.Id =CC.basicId and CC.MakeId=@MakeId
		LEFT JOIN Con_EditCms_Images CEI  WITH(NOLOCK) 
		    ON CEI.BasicId = CB.Id
		    AND CEI.IsMainImage = 1
			AND CEI.IsActive = 1
		LEFT JOIN SocialPluginsCount SPC WITH(NOLOCK) 
		    ON SPC.TypeId = CB.Id
			AND SPC.TypeCategoryId = 1
		WHERE CB.CategoryId = 9
			AND CB.IsActive = 1
			AND CB.IsPublished = 1
			
      --to get make description
	   SELECT MDescription FROM MakeDescriptions WHERE MakeId = @MakeId

	   --to get upcoming cars of brand
	    select top (2) MO.Id,MK.Name MakeName, Mo.Name AS ModelName,Mo.HostUrl+'/cars/'+ Mo.LargePic as ImgPath
            From ExpectedCarLaunches ECL 
                LEFT JOIN CarSynopsis Csy ON ECL.CarModelId = Csy.ModelId AND Csy.IsActive = 1 
                INNER JOIN CarModels Mo ON ECL.CarModelId = Mo.ID and Mo.IsDeleted=0
				INNER JOIN CarMakes MK ON MK.ID = ECL.CarMakeId and MK.IsDeleted=0
            where ECL.CarMakeId=@MakeId and Mo.Futuristic = 1 AND ECL.isLaunched = 0 AND ECL.IsDeleted = 0 

      --to retrieve top 2 latest and Featured news from autoexpo news
	  SELECT Top(@TopCount) CB.Id AS BasicId
			,CB.AuthorName
			,CB.Description
			,CB.DisplayDate
			,CB.VIEWS
			,CB.Title
			,CB.Url
			,CEI.HostUrl
			,CEI.ImagePathThumbnail
			,CEI.ImagePathLarge
			,CEI.IsMainImage
			,ROW_NUMBER() OVER (
				ORDER BY DisplayDate DESC
				) AS Row_No
			,CB.IsSticky
			,SPC.FacebookCommentCount
		FROM Con_EditCms_Basic AS CB  WITH(NOLOCK)
		INNER JOIN Con_EditCms_Cars CC 
		    on CB.Id =CC.basicId and CC.MakeId=@MakeId
		LEFT JOIN Con_EditCms_Images CEI  WITH(NOLOCK) 
		    ON CEI.BasicId = CB.Id
		    AND CEI.IsMainImage = 1
			AND CEI.IsActive = 1
		LEFT JOIN SocialPluginsCount SPC WITH(NOLOCK) 
		    ON SPC.TypeId = CB.Id
			AND SPC.TypeCategoryId = 1
		WHERE CB.CategoryId = 9
			AND CB.IsActive = 1
			AND CB.IsPublished = 1


			---to get carmodels---
		select Top(4)CMO.Id,CM.Name +' '+ CMO.Name as CarName,CM.Name as Make,CMO.Name as Model  from CarModels CMO
		Inner Join CarMakes CM On CM.Id=@MakeId and CMo.CarMakeId=CM.Id and CM.New=1 and CM.Futuristic=0 and CM.IsDeleted=0
		
END

