IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[Classified_CarPhotos_Remove]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[Classified_CarPhotos_Remove]
GO

	  
-- =============================================    
-- Author:  Satish Sharma    
-- Create date: DEC 10, 2008    
-- Description: SP to remove selected image, if selected image was main image     
--    then mark next available image as a main image    
-- =============================================    
CREATE PROCEDURE [dbo].[Classified_CarPhotos_Remove]    
 -- Add the parameters for the stored procedure here      
 @InquiryId  NUMERIC,    
 @PhotoId  NUMERIC  
AS    
BEGIN    
 DECLARE @IsMainPhoto Bit, @IsDealer Bit    
 SELECT @IsMainPhoto = IsMain, @IsDealer = IsDealer FROM CarPhotos WHERE Id = @PhotoId  
     
 -- Remove the image    
 UPDATE CarPhotos SET IsActive = 0 WHERE Id = @PhotoId  
    
 -- If it was main photo you just deleted, make the next photo as a main photo    
 IF @IsMainPhoto = 1  
  BEGIN  
   UPDATE CarPhotos SET IsMain = 1 WHERE InquiryId = @InquiryId AND     
   Id IN( SELECT TOP 1 Id FROM CarPhotos WHERE IsActive = 1 AND InquiryId = @InquiryId AND IsDealer = @IsDealer AND IsApproved = 1 )   
  END    
    
  -- Modified by Vikas to track the Progress in the sell car process.  
  DECLARE @PhotoCount INT  
  DECLARE @ProgressVal INT   
  SELECT @PhotoCount = (SELECT COUNT(*) FROM CarPhotos Where InquiryId = @InquiryId And IsActive = 1)    
  SELECT @ProgressVal = (SELECT Progress From CustomerSellInquiries Where Id = @InquiryId)     
   
  IF @PhotoCount = 0  
  UPDATE CustomerSellInquiries Set Progress =  CASE @ProgressVal WHEN 90 THEN 70 WHEN 100 THEN 80 ELSE @ProgressVal END  Where ID = @InquiryId  
END    
    