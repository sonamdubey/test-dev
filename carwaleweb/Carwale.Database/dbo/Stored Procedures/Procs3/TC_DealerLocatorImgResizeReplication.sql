IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_DealerLocatorImgResizeReplication]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_DealerLocatorImgResizeReplication]
GO

	--Modified By : Tejashree Patil on 28 Oct 2014
--Added condition ISNULL(Large, ThumbImage) instead of ISNULL(ThumbImage,Large)
--==============================================================================
CREATE PROCEDURE [dbo].[TC_DealerLocatorImgResizeReplication]
	@CategoryId INT
AS
BEGIN
	DECLARE @IsBanner BIT = 0
	IF	@CategoryId = 1--Banner
		SET @IsBanner = 1

	SELECT	CASE WHEN @CategoryId = 3 THEN HostURL+DirectoryPath+ThumbImage ELSE HostURL+DirectoryPath+LargeImage END ImageUrl ,
			I.ImageCategoryId, HostURL, DirectoryPath, I.Id, I.DealerId
	FROM	Microsite_Images I 
	WHERE	I.IsActive=1 
			AND I.StatusId=3
			--AND I.ImgPathCustom600 IS NULL AND I.ImgPathCustom300 IS NULL
			AND I.IsResize = 0
			AND I.ImageCategoryId = @CategoryId
	UNION ALL
	SELECT	ISNULL(HostURL+DirectoryPath+LargeImage,HostURL+DirectoryPath+ThumbImage) ImageUrl,I.ImageCategoryId, HostURL, DirectoryPath, I.Id, I.DealerId
	FROM	Microsite_Images I 
	WHERE	I.IsActive=1 
			AND I.StatusId=3
			--AND I.ImgPathCustom600 IS NULL AND I.ImgPathCustom300 IS NULL
			AND I.IsResize = 0
			AND	I.ImageCategoryId IS NULL
			AND I.IsBanner = @IsBanner
			AND ((@IsBanner = 0 AND I.DirectoryPath LIKE '%Miscellaneous%') OR (@IsBanner = 1 AND I.DirectoryPath LIKE '%Banner%'))
	--ORDER BY I.Id DESC
END