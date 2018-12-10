IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[IMG_TargetPaths]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[IMG_TargetPaths]
GO

	
-- Author		:	Chetan Dev
-- Create date	:	01/03/2013 15:58:26 
-- Description	:	This sp fetch all paths of image
--Modified : Ranjeet || For Broker app Images || 26-May
--modified by chetan dev 26 th may 2014 functionality for extra medium image
-- Modified By : Chetan dev on 29 July 2014
-- Modified : Added new category editcms20 and code under this.
-- Modified By :Tejashree Patil on 11 Sept 2014, added ImgPathCustom600,ImgPathCustom300 in Microsite_Images table. 
-- Modified By :Tejashree Patil on 17 Oct 2014, Fetched Origional path for website. 
-- Modified By :Tejashree Patil on 17 Oct 2014, New Category ManageWebsite47. 
-- Modified By :Vivek Gupta on 11-11-2014, New Category ManageWebsite48. 
-- Modified By : Ruchira Patil on 17th Dec 2014 (for AbSure Iamges)
-- Modified By : Vinay Kumar Prajapati on 23rd Desc 2014(Model videos Thumb nail in 'editcms' category)
-- Modified By : Khushaboo Patil on 2-6-2015, New Category dealerprofilephoto. 
-- Modified By : Ashwini Todkar on 8 July 2015 retrieved ImagePathSmall for editcms category 
-- Modified By : Afrose on 21st July 2015, new category BugsScreenShot
-- Modified By  : Vinay kumar prajapati  for absurereportproblem photos 

-- =============================================    
CREATE PROCEDURE [dbo].[IMG_TargetPaths]    

 -- Add the parameters for the stored procedure here    
 @Category VARCHAR(100),
 @Id Int
 

As
BEGIN    
 -- SET NOCOUNT ON added to prevent extra result sets from    
 -- interfering with SELECT statements.    
SET NOCOUNT ON;


IF(@Category='tradingcars')
BEGIN
SELECT DirectoryPath,ImageUrlFull As Large, ImageUrlThumb As Thumbnail,ImageUrlThumbSmall As Small, '' As Original,
'' As  Custom , ImageUrlMedium As Medium --added by chetan 
FROM TC_CarPhotos WITH(NOLOCK)
WHERE Id = @Id
END

IF(@Category='editcms')
BEGIN

SELECT ''As DirectoryPath, ImagePathThumbnail As Thumbnail,ImagePathCustom As Custom, ImagePathLarge As Large,ImagePathOriginal As Original ,ImagePathSmall As Small, VideoPathThumbNail AS VideoThumbNail --modified by Ashwini Todkar retrieved ImagePathSmall
FROM Con_EditCms_Images WITH(NOLOCK)
WHERE Id = @Id
END

IF(@Category='editcms20')
BEGIN

SELECT ''As DirectoryPath, ImagePathCustom88 As Custom88,ImagePathCustom140 As Custom140,ImagePathCustom200 As Custom200
FROM Con_EditCms_Images WITH(NOLOCK)
WHERE Id = @Id
END



IF(@Category='sellcarinquiry')

BEGIN
SELECT  DirectoryPath,ImageUrlFull As Large, ImageUrlThumb As Thumbnail,ImageUrlThumbSmall As Small, '' As Original,
'' As  Custom 
FROM TC_SellCarPhotos WITH(NOLOCK)
WHERE Id = @Id
END

IF(@category='managewebsite40' or @category='managewebsite41' or @category='managewebsite42' or @category='managewebsite43' or @Category ='managewebsite44'or @Category ='managewebsite45'or @Category ='managewebsite46' or @category='managewebsite47' or @Category = 'managewebsite48')
BEGIN
SELECT  DirectoryPath,LargeImage As Large, ThumbImage As Thumbnail,'' As Small, ThumbImage As Original,ImgPathCustom600 As Custom600,ImgPathCustom300 As Custom300
FROM Microsite_Images WITH(NOLOCK)
WHERE Id = @Id
END

IF(@Category='cwcommunity')
BEGIN
SELECT DirectoryPath,Small As Small , Thumbnail As Thumbnail, Medium As Medium, Large As Large,
XL As XL , XXL As XXL , '' As Original,'' As  Custom  from UP_Photos WITH(NOLOCK)
WHERE ID = @Id
END

IF(@Category='forumsrealimage')
BEGIN
SELECT DirectoryPath,SmallUrl As Small, ThumbnailUrl As Thumbnail, MediumUrl As Medium From UserProfile WITH(NOLOCK)
WHERE UserId = @Id
END

IF(@Category='usedsellcars')
BEGIN
SELECT DirectoryPath,ImageUrlThumb As Medium, ImageUrlThumbSmall As Thumbnail, ImageUrlFull As Large , ImageUrlMedium As Small From CarPhotos WITH(NOLOCK)
WHERE id = @Id --added by chetan 
END


----Modified : Ranjeet || For Broker app Images ||
IF(@Category='brokerapp')
BEGIN
SELECT BIM.Dir AS DirectoryPath,BIM.Small As Small, BIM.Medium As Medium, BIM.Large As Large  From BA_ImageSize AS BIM WITH(NOLOCK)
WHERE id = @Id
END
END

--Modified by : Ruchira Patil on 17th Dec 2014 (for absure images)
IF(@Category='absure')
BEGIN
SELECT DirectoryPath,ImageUrlLarge As Large, ImageUrlThumb As Thumbnail,ImageUrlSmall As Small,ImageUrlOriginal As Original, ImageUrlExtraLarge As XL
FROM AbSure_CarPhotos WITH(NOLOCK)
WHERE AbSure_CarPhotosId = @Id
END

IF(@Category = 'dealerprofilephoto')
BEGIN
	SELECT '' AS DirectoryPath , ProfilePhotoUrl As Small
	FROM Dealers WHERE ID = @Id
END
IF(@Category = 'dmsscreenshot')
BEGIN
	SELECT '' AS DirectoryPath , DMSScreenShotUrl As Small
	FROM TC_NewCarInquiries WHERE TC_NewCarInquiriesId = @Id
END

IF(@Category = 'bugsscreenshot')
BEGIN
	SELECT '' AS DirectoryPath , BugScreenShotStatusId As Small
	FROM TC_BugFeedback WHERE TC_Bug_Id=@Id

END

--Modified by :Vinay Kumar Prajapati (Report Problem or suggestion in absure)
IF(@Category='absurereportproblem')
BEGIN
	SELECT DirectoryPath,ImageUrlLarge As Large,ImageUrlOriginal As Original,ImageUrlExtraLarge As XL
	FROM Absure_ReportProblemPhotos WITH(NOLOCK)
	WHERE Absure_ReportProblemPhotosId = @Id
END


