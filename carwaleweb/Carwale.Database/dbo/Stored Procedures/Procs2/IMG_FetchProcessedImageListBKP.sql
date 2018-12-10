IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[IMG_FetchProcessedImageListBKP]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[IMG_FetchProcessedImageListBKP]
GO

	-- =============================================
-- Author:		<Chetan Dev>
-- Create date: <6 Feb 2013>
-- Description:	<It takes comma separated image ids(itemids) and the categoryid.
-- It returns all the item ids which have been processed along with the thumb URLS>
-- =============================================
CREATE PROCEDURE [dbo].[IMG_FetchProcessedImageListBKP]

	@ImageList VARCHAR(1000),
	@CategoryId INT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	
	IF @CategoryId = 2	--for editcms
	BEGIN
		SELECT
			CEI.Id, CEI.HostUrl AS HostUrl, '' AS DirPath, CEI.ImagePathThumbnail AS ThumbNailURL 
			FROM Con_EditCms_Images AS CEI 
			join  dbo.fnSplitCSVValuesWithIdentity(@ImageList) AS FN ON FN.ListMember=CEI.Id 
			      AND CEI.StatusId > 2
	END
	
	IF(@CategoryId=1)--for tradingcars
	BEGIN
	SELECT
	CEI.Id, CEI.HostUrl AS HostUrl, CEI.DirectoryPath AS DirPath, CEI.ImageUrlThumbSmall AS ThumbNailURL 
			FROM TC_CarPhotos AS CEI 
			join  dbo.fnSplitCSVValuesWithIdentity(@ImageList) AS FN ON FN.ListMember=CEI.Id 
			      AND CEI.StatusId > 1
	END

	IF(@CategoryId=3)--for sellcarphotos
	BEGIN
	SELECT
	CEI.Id, CEI.HostUrl AS HostUrl, CEI.DirectoryPath AS DirPath, CEI.ImageUrlThumbSmall AS ThumbNailURL 
			FROM TC_SellCarPhotos AS CEI 
			join  dbo.fnSplitCSVValuesWithIdentity(@ImageList) AS FN ON FN.ListMember=CEI.Id 
			      AND CEI.StatusId > 1
	END

	IF(@CategoryId=5)--for carwale community
	BEGIN
	SELECT CEI.Id, CEI.HostUrl AS HostUrl, CEI.DirectoryPath AS DirPath, CEI.Small AS ThumbNailURL 
			FROM up_photos AS CEI 
			join  dbo.fnSplitCSVValuesWithIdentity(@ImageList) AS FN ON FN.ListMember=CEI.Id 
			      AND CEI.StatusId > 1
	END
END

