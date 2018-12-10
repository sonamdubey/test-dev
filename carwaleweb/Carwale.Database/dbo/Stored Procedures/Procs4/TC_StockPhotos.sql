IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_StockPhotos]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_StockPhotos]
GO

	-- =============================================  
-- Author:  <Nilesh Utture>  
-- Create date: <25th October, 2012>  
-- Description: <Gives the CarName and returs status >  
/*DECLARE @Status TINYINT ,  
  @CarName VARCHAR(100)  
  EXEC TC_StockPhotos 3375, 5, @Status OUTPUT, @CarName OUTPUT
  SELECT @Status AS Status, @CarName AS CarName */
  -- Modified by vivek gupta on 18-12-2013, fetched youtube link
  -- Modified By vivek Gupta on 23-12-2013, added queries to get youtube link
  -- Modified By vivek Gupta on 6th jan,2014, commented previous query and added new query for youtubelink.
  -- Modified By Chetan Navin on 8th Sep 2015, added originalImagePath in select query
-- =============================================  
CREATE PROCEDURE [dbo].[TC_StockPhotos]  
 -- Add the parameters for the stored procedure here  
@StockId BIGINT,  
@BranchId BIGINT,  
@Status TINYINT OUTPUT,  
@CarName VARCHAR(100)OUTPUT,
@YoutubeLink VARCHAR(200) = NULL OUTPUT
AS  
BEGIN  
 -- SET NOCOUNT ON added to prevent extra result sets from  
 -- interfering with SELECT statements.  
 SET NOCOUNT ON;  
 IF NOT EXISTS(SELECT Id FROM TC_Stock WHERE Id=@StockId AND BranchId=@BranchId)  
  BEGIN  
   SET @Status=1  
  END  
 ELSE  
  BEGIN  
   -- Insert statements for procedure here  
     
   Select @CarName =( Ma.Name + ' ' + Mo.Name + ' ' + Ve.Name ) From TC_Stock St  
   Inner Join CarVersions Ve On Ve.Id = St.VersionId   
   Inner Join CarModels Mo On Mo.Id = Ve.CarModelId   
   Inner Join CarMakes Ma On Ma.Id = Mo.CarMakeId   
            WHERE St.Id = @StockId   
     
   SELECT Id, ImageUrlFull, ImageUrlThumb, ImageUrlThumbSmall, IsMain,DirectoryPath,HostUrl,StatusId,OriginalImgPath 
   FROM TC_CarPhotos   
   WHERE IsActive = 1 AND StockId = @StockId ORDER BY IsMain DESC, Id   
   SET @Status=0  

   -- Added by vivek gupta on 18-12-2013
    -- Modified By vivek Gupta on 23-12-2013
	-- Modified By vivek Gupta on 6th jan,2014, commented previous query and added new query for youtubelink.
   --SELECT @YoutubeLink = YoutubeVideo FROM SellInquiriesDetails WITH(NOLOCK) WHERE SellInquiryId = (SELECT SI.ID FROM SellInquiries SI WITH(NOLOCK) WHERE SI.TC_StockId = @StockId)
     SELECT @YoutubeLink = VideoUrl FROM TC_CarVideos WITH(NOLOCK) WHERE StockId = @StockId AND IsActive = 1

  END  
END  

---------------****************---------------------
