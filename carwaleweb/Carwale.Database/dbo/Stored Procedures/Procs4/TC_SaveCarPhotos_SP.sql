IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_SaveCarPhotos_SP]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_SaveCarPhotos_SP]
GO

	---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
-- ModifiedBy:	Surendra Chouksey
-- Create date: 28-11-2011
-- Description:	New Parameter HostUrl is added
-- =============================================
-- ModifiedBy:	Binumon George
-- Create date: 08-11-2011
-- Description:	Added ModifiedBy parameter and modified date.
-- =============================================  
-- Modified By Surendra ,date 08/08/2011
-- Author:  Vikas C  
-- Create date: 2011/03/02 12:15 PM Monday  
-- Description: This SP used to maintain record of uploaded images in the Trading Cars Software	    
    -- image will get upload in three sizes i.e. 640x428|300x225|80x60      
-- =============================================  
CREATE PROCEDURE [dbo].[TC_SaveCarPhotos_SP]  
 -- Add the parameters for the stored procedure here  
 @StockId   NUMERIC,  
 @ImageUrlFull  VARCHAR(100),  
 @ImageUrlThumb  VARCHAR(100),  
 @ImageUrlThumbSmall VARCHAR(100),  
 @IsMain    BIT,
 @DirPath   VARCHAR(200), 
 @PhotoId   NUMERIC OUTPUT,
 @HostUrl VARCHAR(100)
AS  
BEGIN  
 -- SET NOCOUNT ON added to prevent extra result sets from  
 -- interfering with SELECT statements.  
 SET NOCOUNT ON;  
   
 IF NOT EXISTS( SELECT TOP 1 Id FROM TC_CarPhotos WHERE StockId = @StockId AND IsActive = 1 AND IsMain = 1 )  
 BEGIN  
  SET @IsMain = 1  
 END  
   
 INSERT INTO TC_CarPhotos(StockId, ImageUrlFull, ImageUrlThumb, ImageUrlThumbSmall, IsMain,DirectoryPath,HostUrl)  
 VALUES(@StockId, @ImageUrlFull, @ImageUrlThumb, @ImageUrlThumbSmall, @IsMain,@DirPath,@HostUrl)  
    
 SET @PhotoId = SCOPE_IDENTITY()  
END  



