IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[IMG_FetchProcessedImageList_15]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[IMG_FetchProcessedImageList_15]
GO

	-- =============================================
-- Author:		<Chetan Dev>
-- Create date: <6 Feb 2013>
-- Description:	<It takes comma separated image ids(itemids) and the categoryid.
-- It returns all the item ids which have been processed along with the thumb URLS>
-- Modified by Raghu on 07-02-2014 real image for forums.
-- Added By Ruchira Patil on 17th Dec 2014(for absure images)
-- Added column AvtOriginalImgPath and RealOriginalImgPath for forums
-- Modified by Sanjay On 19/08/2015 change thumbnailpath to OriginalImgPath for edit cms
-- =============================================
CREATE PROCEDURE [dbo].[IMG_FetchProcessedImageList_15.8.1]

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
			CEI.Id, CEI.HostUrl AS HostUrl, CEI.OriginalImgPath AS OriginalImgPath 
			FROM Con_EditCms_Images AS CEI WITH (NOLOCK)
			join  dbo.fnSplitCSVValuesWithIdentity(@ImageList) AS FN ON FN.ListMember=CEI.Id 
			      AND CEI.StatusId > 2
	END
	
	IF(@CategoryId=1)--for tradingcars
	BEGIN
	SELECT
	CEI.Id, CEI.HostUrl AS HostUrl, CEI.DirectoryPath AS DirPath, CEI.ImageUrlThumbSmall AS ThumbNailURL 
			FROM TC_CarPhotos AS CEI WITH (NOLOCK)
			join  dbo.fnSplitCSVValuesWithIdentity(@ImageList) AS FN ON FN.ListMember=CEI.Id 
			      AND CEI.StatusId > 1
	END

	IF(@CategoryId=3)--for sellcarphotos
	BEGIN
	SELECT
	CEI.Id, CEI.HostUrl AS HostUrl, CEI.DirectoryPath AS DirPath, CEI.ImageUrlThumbSmall AS ThumbNailURL 
			FROM TC_SellCarPhotos AS CEI WITH (NOLOCK) 
			join  dbo.fnSplitCSVValuesWithIdentity(@ImageList) AS FN ON FN.ListMember=CEI.Id 
			      AND CEI.StatusId > 1
	END

	IF(@CategoryId=5)--for carwale community
	BEGIN
	SELECT CEI.Id, CEI.HostUrl AS HostUrl, CEI.DirectoryPath AS DirPath, CEI.Small AS ThumbNailURL, CEI.OriginalImgPath
			FROM up_photos AS CEI WITH (NOLOCK)
			join  dbo.fnSplitCSVValuesWithIdentity(@ImageList) AS FN ON FN.ListMember=CEI.Id 
			      AND CEI.StatusId > 1
	END

	IF(@CategoryId=6)--for forums real image
	BEGIN
	SELECT CEI.UserId AS Id, CEI.HostUrl AS HostUrl, CEI.DirectoryPath AS DirPath,CEI.AvtarPhoto AS AvtarPhoto, CEI.RealPhoto AS ThumbNailURL,CEI.AvtOriginalImgPath, CEI.RealOriginalImgPath 
			FROM UserProfile AS CEI WITH (NOLOCK)
			--WHERE CEI.UserId in (select * from SplitTextRS(@ImageList,','))
			join  dbo.fnSplitCSVValuesWithIdentity(@ImageList) 
			AS FN ON FN.ListMember=CEI.UserId 
			      AND CEI.StatusId > 1
	END

	IF(@CategoryId=7)--for used sell cars
	BEGIN
	SELECT CEI.Id, CEI.HostUrl AS HostUrl, CEI.DirectoryPath AS DirPath, CEI.ImageUrlThumbSmall AS ThumbNailURL, CEI.OriginalImgPath
			FROM CarPhotos AS CEI WITH (NOLOCK)
			join  dbo.fnSplitCSVValuesWithIdentity(@ImageList) AS FN ON FN.ListMember=CEI.Id 
			      AND CEI.StatusId > 1
	END

	IF(@CategoryId=49)--for absure image
	BEGIN
	SELECT CEI.AbSure_CarPhotosId, CEI.HostUrl AS HostUrl, CEI.DirectoryPath AS DirPath, CEI.ImageUrlSmall AS ThumbNailURL 
			FROM AbSure_CarPhotos AS CEI WITH (NOLOCK)
			join  dbo.fnSplitCSVValuesWithIdentity(@ImageList) AS FN ON FN.ListMember=CEI.AbSure_CarPhotosId 
			      AND CEI.StatusId > 1
	END
END
