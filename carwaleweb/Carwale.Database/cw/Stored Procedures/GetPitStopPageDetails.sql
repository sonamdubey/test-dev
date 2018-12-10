IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[cw].[GetPitStopPageDetails]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [cw].[GetPitStopPageDetails]
GO

	-- =============================================            
-- Author:  <Ravi Koshal>            
-- Create date: <22/08/2013>            
-- Description: <Returns the details to be shown on viewing a pitstop page.            
--               Also increases the view count if the news has been viewed after being uploaded on CarWale.>    
-- Modified by: Natesh on 20-7-2014 Added Application id flag for CMS merging 
-- =============================================            
CREATE PROCEDURE [cw].[GetPitStopPageDetails] -- EXEC [cw].[GetPitStopPageDetails] 8793, 1,1           
 -- Add the parameters for the stored procedure here            
 @BasicId NUMERIC(18,0) = 0,            
 @IsPublished BIT = 1,   
 @ApplicationId INT  ,     
 @NextId NUMERIC(18,0) output ,         
 @NextUrl varchar(200) output,          
 @NextTitle varchar(250) output,          
 @PrevId NUMERIC(18,0) output,          
 @PrevUrl varchar(200) output,          
 @PrevTitle varchar(250) output,    
 @Tag varchar(100) output,    
 @AuthorName varchar(100) output,    
 @DisplayDate datetime output,    
 @Title varchar(250) output,    
 @url varchar(200) output,    
 @Views int output,    
 @Content varchar(max)  output,    
 @HostUrl varchar(250) output,    
 @ImagePathLarge varchar(100) output,    
 @ImagePathThumbnail varchar(100) output,    
 @MainImgCaption varchar(250) output,    
 @Caption varchar(250) output,    
 @Id numeric output,
 @FacebookCommentCount int output        
             
AS            
BEGIN            
 -- SET NOCOUNT ON added to prevent extra result sets from            
 -- interfering with SELECT statements.            
 SET NOCOUNT ON;            
             
 ---used to get number of views            
 if(@IsPublished=1)            
 BEGIN            
  Update Con_EditCms_Basic             
  Set [Views] = [Views] + 1             
  Where IsActive = 1 AND CategoryId = 12  AND             
    Id = @BasicId             
 END            
             
            
    -- used to get all parameters related to news.            
 SELECT TOP 1 @AuthorName=CB.AuthorName, @DisplayDate=CB.DisplayDate,@Title=CB.Title,@url=CB.Url, @Views=CB.Views,@Content= CPC.Data, @HostUrl=CEI.HostURL,             
  @ImagePathLarge=CEI.ImagePathLarge, @Caption=CEI.Caption, @MainImgCaption=CB.MainImgCaption,            
  @Tag=TG.Tag,@ImagePathThumbnail=CEI.ImagePathThumbnail,@Id=CB.Id,@FacebookCommentCount=SPC.FacebookCommentCount             
 FROM  Con_EditCms_Basic CB  WITH(NOLOCK)              
  INNER JOIN Con_EditCms_Pages CP WITH(NOLOCK)  ON CP.BasicId = CB.Id             
  INNER JOIN Con_EditCms_PageContent CPC WITH(NOLOCK)  ON CPC.PageId = CP.Id            
  LEFT JOIN Con_EditCms_BasicTags BT WITH(NOLOCK)  ON BT.BasicId = CB.Id            
  LEFT JOIN Con_EditCms_Tags TG WITH(NOLOCK)  ON TG.Id = BT.TagId              
  LEFT JOIN Con_EditCms_Images CEI WITH(NOLOCK)  ON CEI.BasicId = CB.Id AND CEI.IsMainImage = 1 AND CEI.IsActive = 1 
  LEFT JOIN SocialPluginsCount SPC WITH(NOLOCK) ON SPC.TypeId = CB.Id AND SPC.TypeCategoryId = 12    --  Modified By:Ravi koshal 8/20/2013 -- Added SocialPluginsCount.         
 Where  CB.IsActive = 1 AND CB.CategoryId = 12 AND CB.ApplicationID = @ApplicationId AND CP.BasicId = CB.Id AND  CP.IsActive = 1 AND             
 CPC.PageId = CP.Id AND CB.Id = @BasicId AND CB.IsPublished = @IsPublished             
           
  ---to retrieve previous news data         
 SELECT TOP 1 @PrevId=CB.Id, @PrevTitle=CB.Title ,@PrevUrl=CB.Url          
 FROM CON_EDITCMS_BASIC CB  WITH(NOLOCK)       
 INNER JOIN Con_EditCms_Pages CP WITH(NOLOCK)  ON CP.BasicId = CB.Id           
 WHERE CB.ID > @BasicId  AND CB.CATEGORYID=12  and CB.IsActive=1 and CB.IsPublished=1 AND CB.ApplicationID = @ApplicationId and CP.BasicId=CB.Id        
 ORDER BY CB.ID          
         
    --to retrieve next news data     
 SELECT TOP 1 @NextId=CB.Id ,@NextTitle= CB.Title,@NextUrl=CB.Url         
 FROM CON_EDITCMS_BASIC  CB   WITH(NOLOCK)      
 INNER JOIN Con_EditCms_Pages CP WITH(NOLOCK)  ON CP.BasicId = CB.Id         
 WHERE CB.ID < @BasicId  AND CB.CATEGORYID=12  and CB.IsActive=1 and CB.IsPublished=1 AND CB.ApplicationID = @ApplicationId and CP.BasicId=CB.Id        
 ORDER BY CB.ID  Desc       
           
END 

