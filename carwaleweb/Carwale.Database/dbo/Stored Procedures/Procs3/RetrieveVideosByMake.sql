IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[RetrieveVideosByMake]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[RetrieveVideosByMake]
GO

	-- =============================================
-- Author:		Rohan Sapkal	
-- Create date: 31st July 2014
-- Description:	Retrieve Videos of Specific car Make
-- Modified by: Natesh on 20-7-2014 Added Application id flag for CMS merging
-- Modified by: Rohan S on 16-09-2014 , Handled start/end index as NULL default
-- =============================================
CREATE PROCEDURE [dbo].[RetrieveVideosByMake]
	-- Add the parameters for the stored procedure here
	@MakeId int,
	@ApplicationID int,
	@StartIndex int = NULL,
	@EndIndex int =NULL
AS
BEGIN
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
						   AND CB.ApplicationID=@ApplicationID
                           AND CB.isactive = 1
                           AND 
						   --( 
						   CC.makeid = @MakeId
                           --       OR @MakeId IS NULL )
                         --  AND ( CC.modelid = @ModelId
                         --         OR @ModelId IS NULL )
						) AS T
					WHERE ((T.RowNo BETWEEN @startindex AND @endindex) OR @startindex IS NULL OR @endindex IS NULL)
					--ORDER BY basicid DESC
END