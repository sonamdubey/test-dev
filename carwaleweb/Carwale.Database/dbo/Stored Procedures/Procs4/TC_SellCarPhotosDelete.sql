IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_SellCarPhotosDelete]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_SellCarPhotosDelete]
GO

	-- =============================================    
-- Author:  Nilesh Utture
-- Create date: MAR 02, 2011
-- Description: SP to remove selected image, if selected image was main image     
--              then mark next available image as a main image    
-- =============================================    
CREATE PROCEDURE [dbo].[TC_SellCarPhotosDelete]    
 -- Add the parameters for the stored procedure here      
 @SellerInquiriesId  BIGINT,    
 @PhotoId  BIGINT  
AS    
BEGIN    
 DECLARE @IsMainPhoto Bit,@ImageUrlFull VARCHAR(100),@InquiryId INT
 SELECT @IsMainPhoto = IsMain,@ImageUrlFull=ImageUrlFull  FROM TC_SellCarPhotos WHERE Id = @PhotoId  
 
/**************************************************************************************/ 
	-- Manage TC_CarPhotos @ Trading Car App
/**************************************************************************************/     
 -- Remove the image    
 UPDATE TC_SellCarPhotos SET IsActive = 0 WHERE Id = @PhotoId
 
  -- If it was main photo you just deleted, make the next photo as a main photo    
 IF @IsMainPhoto = 1    
  BEGIN    
   UPDATE TC_SellCarPhotos SET IsMain = 1 WHERE TC_SellerInquiriesId = @SellerInquiriesId AND     
   Id IN( SELECT TOP 1 Id FROM TC_SellCarPhotos WHERE IsActive = 1 AND TC_SellerInquiriesId = @SellerInquiriesId )    
  END 
END    




