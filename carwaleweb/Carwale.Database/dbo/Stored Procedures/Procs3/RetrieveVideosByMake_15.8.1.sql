IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[RetrieveVideosByMake_15]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[RetrieveVideosByMake_15]
GO

	
-- =============================================
-- Author:		Rohan Sapkal	
-- Create date: 31st July 2014
-- Description:	Retrieve Videos of Specific car Make
-- Modified by: Natesh on 20-7-2014 Added Application id flag for CMS merging
-- Modified by: Rohan S on 16-09-2014 , Handled start/end index as NULL default
-- Modified by Rohan.s on 18-12-2014 , Added columns to select thumbnail from CDN -on 24-12-2014 changed select n join statement for Con_editcms_images
-- Modified by Ashwini Todkar on 5 Aug 2015 retrieved original image path and host url and versioning
-- =============================================
CREATE PROCEDURE [dbo].[RetrieveVideosByMake_15.8.1]
	-- Add the parameters for the stored procedure here
	@MakeId int,
	@ApplicationID int,
	@StartIndex int = NULL,
	@EndIndex int =NULL
AS
BEGIN
DECLARE @ThumbnailCategory numeric = (SELECT Id from Con_PhotoCategory with (nolock) where Name='VideoThumbNail' and ApplicationId=@ApplicationID)
	select * from (SELECT DISTINCT  Top(1000) CV.basicid,
                   CV.videourl,
				   CV.VideoId,
                   CV.likes,
                   CV.views,
                   CB.description,
                   CV.duration,
                   CB.title,
				   CB.Url,
                   C.name              AS Make,
                   CM.name             AS Model,
				   CM.MaskingName,
				   dbo.Getgoogletags (CB.id) AS Tag,
                   CC.id               AS VideoCarId,
				   CS.Name as SubCatName,
				   Row_number() OVER (ORDER BY CV.basicid  desc) as RowNo
				   ,CC.MakeId as MakeId
				   ,CC.ModelId as ModelId
				   ,CI.HostURL as HostUrl --Modified by Ashwini
				   ,CI.VideoPathThumbNail as OriginalImgPath --Modified by Ashwini
				   ,NULL as ThumbnailDirPath
                    FROM   con_editcms_videos AS CV with(nolock)
                           INNER JOIN con_editcms_basic AS CB with(nolock)
                                   ON CB.id = CV.basicid
                           INNER JOIN con_editcms_cars AS CC with(nolock)
                                   ON CC.basicid = CV.basicid
                           INNER JOIN carmakes C with(nolock)
                                   ON CC.makeid = C.id
                           INNER JOIN carmodels CM with(nolock)
                                   ON CC.modelid = CM.id
						   Inner Join Con_EditCms_BasicSubCategories CBS with(nolock)
								ON CBS.BasicId=CV.BasicId
						   inner join Con_editCms_subcategories CS with(nolock)
								ON CBS.SubCategoryId=CS.Id
							LEFT JOIN Con_EditCms_Images CI WITH (NOLOCK) ON (CI.BasicId=CV.BasicId and CI.ImageCategoryId=@ThumbnailCategory)
                    WHERE  CV.isactive = 1
                           AND CC.isactive = 1
						   AND cS.CategoryId=13
                           AND CB.ispublished = 1
						   AND CB.ApplicationID=@ApplicationID
                           AND CB.isactive = 1
                           AND 
						   --( 
						   CC.makeid = @MakeId
                           --       OR @MakeId IS NULL )
                         --  AND ( CC.modelid = @ModelId
                         --         OR @ModelId IS NULL )
						 AND (CI.IsMainImage=1 OR CI.IsMainImage is NULL)
						) AS T
					WHERE ((T.RowNo BETWEEN @startindex AND @endindex) OR @startindex IS NULL OR @endindex IS NULL)
					--ORDER BY basicid DESC
END


/****** Object:  StoredProcedure [dbo].[RetrieveVideosByModel_14.11.3]    Script Date: 26-12-2014 11:03:46 ******/
SET ANSI_NULLS ON


