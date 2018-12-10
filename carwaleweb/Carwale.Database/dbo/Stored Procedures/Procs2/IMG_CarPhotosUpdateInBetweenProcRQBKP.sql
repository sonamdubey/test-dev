IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[IMG_CarPhotosUpdateInBetweenProcRQBKP]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[IMG_CarPhotosUpdateInBetweenProcRQBKP]
GO

	

-- Author  : chetan dev
-- Create date : 09/10/2012 14:00 PM  
-- Description : will set hosturls
-- =============================================      
CREATE PROCEDURE [dbo].[IMG_CarPhotosUpdateInBetweenProcRQBKP]      

 -- Add the parameters for the stored procedure here      
 @PhotoId   BIGINT,
 @HostUrl VarChar(100),
 @ServerList VarChar(100),
 @Category varchar(100)
  
AS      
BEGIN      
 -- SET NOCOUNT ON added to prevent extra result sets from      
 -- interfering with SELECT statements.      
SET NOCOUNT ON;  
IF(@Category='editcms')
-- updating final image name
BEGIN  
UPDATE Con_EditCms_Images  
SET HostUrl = @HostUrl
WHERE Id=@PhotoId  

UPDATE IMG_AllCarPhotos 
SET ItemStorage =@ServerList
WHERE ItemId = @PhotoId
END

IF(@Category='tradingcars')
BEGIN
UPDATE TC_CarPhotos set HostUrl = @HostUrl
WHERE Id=@PhotoId  

UPDATE IMG_AllCarPhotos SET ItemStorage =@ServerList
WHERE ItemId = @PhotoId

UPDATE CarPhotos SET HostURL =@HostUrl
WHERE TC_CarPhotoId = @PhotoId  
END

IF(@Category='carversion')
BEGIN
UPDATE carversions SET HostURL = @HostUrl 
WHERE ID = @PhotoId
END
    
IF(@Category='expectedlaunch')
BEGIN
UPDATE CarModels SET HostURL = @HostUrl 
WHERE ID = @PhotoId
END

IF(@Category='featuredlisting')
BEGIN
UPDATE Con_FeaturedListings SET HostUrl = @HostUrl
WHERE CarId = @PhotoId
END

IF(@Category ='carcomparisionlist')
BEGIN
UPDATE Con_CarComparisonList SET HostURL = @HostUrl
WHERE ID = @PhotoId
END

IF(@Category='topsellingcars')
BEGIN
UPDATE Con_TopSellingCars SET HostURL=@HostUrl
WHERE ModelId=@PhotoId
END

IF(@Category='addbrand')
BEGIN
UPDATE Acc_Brands SET HostURL=@HostUrl
WHERE Id=@PhotoId
END

IF(@Category='additems')
BEGIN
UPDATE Acc_Items SET HostURL=@HostUrl
WHERE Id=@PhotoId
END

IF(@Category='additionalitems')
BEGIN
UPDATE Acc_ItemsAdditionalImages SET HostURL=@HostUrl
WHERE Id=@PhotoId
END

IF(@Category='showroomphotos')
BEGIN
UPDATE ShowRoomPhotos SET HostURL=@HostUrl
WHERE Id=@PhotoId
END

IF(@Category='dealercertification')
BEGIN
UPDATE Classified_CertifiedOrg SET HostURL=@HostUrl
WHERE Id=@PhotoId
END

IF(@Category='sellcarinquiry')
BEGIN

UPDATE TC_SellCarPhotos SET HostUrl = @HostUrl
WHERE Id=@PhotoId 
--update all carphotos
UPDATE IMG_AllCarPhotos SET ItemStorage =@ServerList
WHERE ItemId = @PhotoId
END

IF(@category='managewebsite40' or @category='managewebsite41' or @category='managewebsite42' or @category='managewebsite43' or @category='managewebsite44'or @category='managewebsite45')
BEGIN
UPDATE Microsite_Images SET HostURL = @HostUrl
WHERE Id = @PhotoId
UPDATE IMG_AllCarPhotos SET  ItemStorage =@ServerList 
WHERE ItemId = @PhotoId

END
IF(@Category ='managewebsite')
BEGIN
UPDATE Microsite_Images SET HostURL = @HostUrl
WHERE Id = @PhotoId
END

IF(@Category='cwcommunity')
BEGIN
UPDATE UP_Photos SET HostURL = @HostUrl
WHERE Id = @PhotoId
UPDATE IMG_AllCarPhotos SET  ItemStorage =@ServerList 
WHERE ItemId = @PhotoId
END

END      
  
  

  


