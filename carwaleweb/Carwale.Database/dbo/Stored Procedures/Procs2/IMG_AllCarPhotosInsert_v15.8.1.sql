IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[IMG_AllCarPhotosInsert_v15]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[IMG_AllCarPhotosInsert_v15]
GO

	
-- Author		:	Chetan Dev
-- Create date	:	01/03/2013 15:58:26 
-- Description	:	This SP used to maintain record of uploaded images in the Trading Cars Software       
--					image will get upload in three sizes i.e. 640x428|300x225|80x60 ,this record initially will inactive       
-- =============================================    
CREATE PROCEDURE [dbo].[IMG_AllCarPhotosInsert_v15.8.1]    
 -- Add the parameters for the stored procedure here    
 @ItemId   BIGINT,
 @OrigFileName VARCHAR(255),     
 @CategoryId INT,  
 @DirPath VARCHAR(255),
 @HostUrl varchar(255),
 @Url varchar(255) OUTPUT
   
AS    
BEGIN    
 -- SET NOCOUNT ON added to prevent extra result sets from    
 -- interfering with SELECT statements.    
SET NOCOUNT ON;
	
SET @Url = @HostUrl+@DirPath+@OrigFileName
     
--inserting record with inactive status,later once image will save in appropriate folder need to activate
INSERT INTO IMG_AllCarPhotos(CategoryId, ItemId, URL, StatusId, OriginalFilename, EntryDate)    
VALUES(@CategoryId,@ItemId,@Url,1,@OrigFileName,GETDATE()) 
      

END    



