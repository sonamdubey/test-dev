IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[Microsite_CategoryWiseNavigation_Save]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[Microsite_CategoryWiseNavigation_Save]
GO

	-- =============================================        
-- Author:  Vikas J    
-- Create date:12/06/2013     
-- Description: To save all data of navigation list for a category that is updated by the dealer for his website    
-- =============================================        
CREATE PROCEDURE [dbo].[Microsite_CategoryWiseNavigation_Save]         
(        
 @DealerId INT,        
 @ContentCatagoryId INT,  
 @SubCatagoryId INT,    
 @SubCatagoryName VARCHAR(MAX),    
 @UrlValue VARCHAR(MAX),  
 @SubCategoryOrder INT,    
 @IsActive  BIT  ,  
 @NavigationID INT    
)        
AS        
BEGIN        
 -- SET NOCOUNT ON added to prevent extra result sets from        
 SET NOCOUNT ON;    
 DECLARE @COUNT INT=0;    
 SELECT @COUNT=COUNT(*) FROM  Microsite_DealerContentSubCategories WHERE DealerId=@DealerId and CategoryId=@ContentCatagoryId and SubCatagoryId=@SubCatagoryId ;       
 IF(@COUNT=0)      
  BEGIN        
   INSERT INTO Microsite_DealerContentSubCategories(CategoryId,SubCatagoryId,DealerId,isActive,SubCatagoryName,SubCategoryOrder,UrlValue,NavigationId)       
   VALUES(@ContentCatagoryId,@SubCatagoryId,@DealerId,@IsActive,@SubCatagoryName,@SubCategoryOrder,@UrlValue,@NavigationID)      
  END        
 ELSE        
  BEGIN        
   UPDATE Microsite_DealerContentSubCategories     
   SET SubCatagoryName=@SubCatagoryName,NavigationId=@NavigationID,isActive=@IsActive,SubCategoryOrder=@SubCategoryOrder ,UrlValue=@UrlValue
   WHERE DealerId=@DealerId AND CategoryId=@ContentCatagoryId AND SubCatagoryId=@SubCatagoryId      
 END         
END
