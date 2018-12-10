IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[IMG_ImageUpdateRQ]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[IMG_ImageUpdateRQ]
GO

	
-- Author  : chetan dev
-- Create date : 09/10/2012 14:00 PM  
-- Description : will set IsActive 1
-- AM Modified 6-11-2012 to insert PhotoId from TC_CarPhotos to CarPhotos
-- Suri Modified 23-11-2012 makeing car approved =1
-- Modified by Chetan -- Added Extra Category for Carwale Community
-- Modified by Manish on 20-05-2014 for inserting ImgUrlMedium value into CarPhotos table.
--Modified : Ranjeet || For Broker app Images || 26-May-2014
-- Modified by : Manish on 18-06-2014 added with (nolock) keyword.
-- Modified by : Manish on 09-07-2014 commented the whole sp as all the chages implemented in only on sp i.e. "IMG_CarPhotosUpdateReplicateRQ"
-- =============================================      
CREATE PROCEDURE [dbo].[IMG_ImageUpdateRQ]      

 -- Add the parameters for the stored procedure here      
 @PhotoId   BIGINT,
 @HostUrl VarChar(100),
 @ServerList VarChar(100),
 @MaxServers Int,
 @Category varchar(100)
  
AS      
BEGIN      
 -- SET NOCOUNT ON added to prevent extra result sets from      
 -- interfering with SELECT statements.      
SET NOCOUNT ON;  
-- updating final image name  

--DECLARE @ImgUrlMedium VARCHAR(100)
/*
IF(@Category='editcms')
BEGIN
UPDATE Con_EditCms_Images  SET IsActive=1 , HostUrl = @HostUrl,StatusId = 2
WHERE Id=@PhotoId  

UPDATE IMG_AllCarPhotos SET StatusId = 2 , ItemStorage =@ServerList , MaxServers =@MaxServers
WHERE ItemId = @PhotoId

END
IF(@Category='tradingcars')
BEGIN

--update in trading cars table
UPDATE TC_CarPhotos SET IsActive=1 , HostUrl = @HostUrl,StatusId=2
WHERE Id=@PhotoId  
--update all carphotos
UPDATE IMG_AllCarPhotos SET StatusId = 2 , ItemStorage =@ServerList , MaxServers =@MaxServers
WHERE ItemId = @PhotoId

DECLARE 
	@InquiryId BIGINT,
	@StockId BIGINT,
	@ImageUrlFull  VARCHAR(100),
	@ImageUrlThumb  VARCHAR(100),
	@ImageUrlThumbSmall VARCHAR(100),
	@IsMain BIT,
	@HostUri VARCHAR(100),
	@DirPath VARCHAR(100)
	
SELECT 
	@StockId=StockId,
	@ImageUrlFull=ImageUrlFull,
	@ImageUrlThumb=ImageUrlThumb,	
	@ImageUrlThumbSmall=ImageUrlThumbSmall,
	@IsMain=IsMain ,
	@HostUri=HostUrl,
	@DirPath=DirectoryPath,
	@ImgUrlMedium=ImageUrlMedium          ---added ImageUrlMedium on 20-05-2014 by Manish
	
FROM TC_CarPhotos WITH(NOLOCK)
WHERE Id=@PhotoId 
	
SELECT @InquiryId = Id FROM SellInquiries Si WITH(NOLOCK) WHERE Si.Tc_StockId = @StockId AND StatusId=1
--Inserting the car photos details in the CarPhotos table
IF (@InquiryId IS NOT NULL)
BEGIN
    -- AM Modified 6-11-2012 to insert PhotoId from TC_CarPhotos to CarPhotos
	INSERT INTO CarPhotos(InquiryId,ImageUrlFull,ImageUrlThumb,ImageUrlThumbSmall,IsDealer,IsMain,IsActive,IsApproved,DirectoryPath, HostUrl,IsReplicated,TC_CarPhotoId,ImageUrlMedium)
					VALUES(@InquiryId,@ImageUrlFull,@ImageUrlThumb,@ImageUrlThumbSmall,1,@IsMain,1,1,@DirPath, @HostUri,0,@PhotoId,@ImgUrlMedium)   ---added ImageUrlMedium on 20-05-2014 by Manish
END


END
IF(@Category='carversion')
BEGIN
UPDATE CarVersions SET IsReplicated = 0 , HostUrl = @HostUrl,smallPic=CAST(@PhotoId AS VARCHAR(100)) + 's.jpg', largePic = CAST(@PhotoId AS VARCHAR(100)) + 'b.jpg'
WHERE Id=@PhotoId 
END

IF(@Category='expectedlaunch')
BEGIN
UPDATE CarModels SET HostURL = @HostUrl , IsReplicated =0
WHERE ID = @PhotoId
END

IF(@Category='featuredlisting')
BEGIN
UPDATE Con_FeaturedListings SET HostUrl = @HostUrl, IsReplicated =0
WHERE CarId = @PhotoId
END

IF(@Category ='carcomparisionlist')
BEGIN
UPDATE Con_CarComparisonList SET HostURL = @HostUrl, IsReplicated = 0
WHERE ID = @PhotoId
END
IF(@Category='topsellingcars')
BEGIN
UPDATE Con_TopSellingCars SET HostURL=@HostUrl, IsReplicated =0
WHERE ModelId=@PhotoId
END

IF(@Category='addbrand')
BEGIN
UPDATE Acc_Brands SET HostURL=@HostUrl, IsReplicated =0
WHERE Id=@PhotoId
END

IF(@Category='additems')
BEGIN
UPDATE Acc_Items SET HostURL=@HostUrl, IsReplicated =0
WHERE Id=@PhotoId
END

IF(@Category='additionalitems')
BEGIN
UPDATE Acc_ItemsAdditionalImages SET HostURL=@HostUrl, IsReplicated =0
WHERE Id=@PhotoId
END

IF(@Category='showroomphotos')
BEGIN
UPDATE ShowRoomPhotos SET HostURL=@HostUrl, IsReplicated =0
WHERE Id=@PhotoId
END

IF(@Category='dealercertification')
BEGIN
UPDATE Classified_CertifiedOrg SET HostURL=@HostUrl, IsReplicated =0
WHERE Id=@PhotoId
END

IF(@Category='sellcarinquiry')
BEGIN

UPDATE TC_SellCarPhotos SET IsActive=1,HostUrl = @HostUrl, StatusId = 2
WHERE Id=@PhotoId 
--update all carphotos
UPDATE IMG_AllCarPhotos SET StatusId = 2 , ItemStorage =@ServerList , MaxServers =@MaxServers
WHERE ItemId = @PhotoId
END

IF(@category='managewebsite40' or @category='managewebsite41' or @category='managewebsite42' or @category='managewebsite43' or @category='managewebsite44'or @category='managewebsite45'or @category='managewebsite46' )
BEGIN
UPDATE Microsite_Images SET HostURL = @HostUrl, StatusId = 2
WHERE Id = @PhotoId
UPDATE IMG_AllCarPhotos SET StatusId = 2 , ItemStorage =@ServerList , MaxServers =@MaxServers
WHERE ItemId = @PhotoId

END

IF(@Category ='managewebsite')
BEGIN
UPDATE Microsite_Images SET HostURL = @HostUrl, StatusId = 2
WHERE Id = @PhotoId
END

IF(@Category='cwcommunity')
BEGIN
UPDATE UP_Photos SET HostURL = @HostUrl, StatusId = 2,  Size500 = 1, Size1024 = 1,Size800 = 1
WHERE Id = @PhotoId
UPDATE IMG_AllCarPhotos SET StatusId = 2 , ItemStorage =@ServerList , MaxServers =@MaxServers
WHERE ItemId = @PhotoId
END
IF(@Category='avtarimage')
BEGIN
UPDATE UserProfile SET HostURL = @HostUrl, StatusId=2
WHERE UserId = @PhotoId
END

IF(@Category='forumsrealimage')
BEGIN
UPDATE UserProfile SET HostURL = @HostUrl, StatusId=2
WHERE UserId = @PhotoId
UPDATE IMG_AllCarPhotos SET StatusId = 2 , ItemStorage =@ServerList , MaxServers =@MaxServers
WHERE ItemId = @PhotoId
END

IF(@Category='usedsellcars')
BEGIN
UPDATE CarPhotos SET HostURL = @HostUrl, StatusId=2
WHERE Id = @PhotoId
UPDATE IMG_AllCarPhotos SET StatusId = 2 , ItemStorage =@ServerList , MaxServers =@MaxServers
WHERE ItemId = @PhotoId
END

--Added By Ranjeet || For Broker App images
IF(@Category='brokerapp')
BEGIN
UPDATE BA_ImageSize SET HostURL = @HostUrl, StatusId=2
WHERE Id = @PhotoId
UPDATE IMG_AllCarPhotos SET StatusId = 2 , ItemStorage =@ServerList , MaxServers =@MaxServers
WHERE ItemId = @PhotoId
END
*/

END      
  
  

