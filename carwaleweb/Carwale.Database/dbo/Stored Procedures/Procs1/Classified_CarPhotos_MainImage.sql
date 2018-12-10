IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[Classified_CarPhotos_MainImage]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[Classified_CarPhotos_MainImage]
GO

	-- =============================================  
-- Author:  Satish Sharma  
-- Create date: DEC 10, 2008  
-- Description: SP to remove selected image, if selected image was main image   
--    then mark next available image as a main image  
-- Changed by Vaibhav on 05/01/2011 for Updating Main Image in Livelisting table  
-- =============================================  
CREATE PROCEDURE [dbo].[Classified_CarPhotos_MainImage]  
 -- Add the parameters for the stored procedure here     
 @InquiryId  NUMERIC,  
 @PhotoId  NUMERIC,  
 @IsDealer  NUMERIC  
AS  
BEGIN   
 UPDATE CarPhotos SET IsMain = 0 WHERE InquiryId = @InquiryId AND IsDealer = @IsDealer  
 UPDATE CarPhotos SET IsMain = 1 WHERE Id = @PhotoId   
END  
  
  