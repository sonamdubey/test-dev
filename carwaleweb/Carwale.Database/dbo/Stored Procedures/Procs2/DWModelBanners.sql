IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[DWModelBanners]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[DWModelBanners]
GO

	
CREATE PROCEDURE [dbo].[DWModelBanners]
(
@ModelId numeric(18,0),
@DealerId numeric(18,0),
@IsBanner BIT=1
)
AS 
--Author:Rakesh Yadav on 09 Jun 2015
--Desc: get banner images for dealer model(modelId is id of TC_DealerModels)
--Modified by Rakesh Yadav on 21 July 2015 added filter IsBanner=1
--Modified by Komal Manjare on 22 july 2015 added check for gallery images
--Value of IsBanner is default 0 so it will fetch gallery images if IsBanner is not specified
--Modified By Komal Manjare To include thumb images
--modified By Rakesh Yadav on 05 Aug 2015 To include OriginalImgPath
BEGIN

	SELECT HostUrl+ImgPath+ImgName AS URL,HostUrl,OriginalImgPath
	FROM 
	Microsite_DealerModelBanners WITH (NOLOCK)
	WHERE DealerId=@DealerId AND DWModelId=@ModelId AND IsActive=1 AND ISNULL(IsBanner,0)=@IsBanner
	ORDER BY IsMainImg desc,SortOrder ASC
END
