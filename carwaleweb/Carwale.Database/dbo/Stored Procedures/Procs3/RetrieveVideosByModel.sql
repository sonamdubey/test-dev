IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[RetrieveVideosByModel]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[RetrieveVideosByModel]
GO

	-- =============================================
-- Author:		Rohan Sapkal	
-- Create date: 31st July 2014
-- Description:	Retrieve Videos of specific car Model
-- Modified by: Natesh on 20-7-2014 Added Application id flag for CMS merging
-- Modified by Rohan on 12-9-2014 ,default value of index parameters set to NULL
-- =============================================
CREATE PROCEDURE [dbo].[RetrieveVideosByModel]
	-- Add the parameters for the stored procedure here
 @ModelId int =0,
 @ApplicationID INT,
 @startindex int =NULL,
 @endindex int =NULL
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
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
                    WHERE  CV.isactive = 1
                           AND CC.isactive = 1
						   AND cS.CategoryId=13
                           AND CB.ispublished = 1
                           AND CB.isactive = 1
						   AND CB.ApplicationID=@ApplicationID
                           AND 
						   --( 
						   --CC.makeid = @MakeId
                           --       OR @MakeId IS NULL )
                         --  AND
						  --(
						   CC.modelid = @ModelId
                         --         OR @ModelId IS NULL )
						) AS T
					WHERE ((T.RowNo BETWEEN @startindex AND @endindex) OR @startindex IS NULL OR @endindex IS NULL)
					--ORDER BY basicid DESC
END
