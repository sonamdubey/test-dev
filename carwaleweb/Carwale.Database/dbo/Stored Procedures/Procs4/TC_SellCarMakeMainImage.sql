IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_SellCarMakeMainImage]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_SellCarMakeMainImage]
GO

	-- =============================================  
-- Author:  Nilesh Utture
-- Create date: MAR 02, 2011  
-- Description: SP to remove selected image, if selected image was main image   
--              then mark next available image as a main image 
-- =============================================  
CREATE PROCEDURE [dbo].[TC_SellCarMakeMainImage]  
 -- Add the parameters for the stored procedure here     
 @SellerInquiriesId  BIGINT,    
 @PhotoId  BIGINT  
AS  
BEGIN   
 UPDATE TC_SellCarPhotos SET IsMain = 0 WHERE TC_SellerInquiriesId = @SellerInquiriesId
 UPDATE TC_SellCarPhotos SET IsMain = 1 WHERE Id = @PhotoId   
END  
  
 
  
  
