IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[Microsite_ModelBannersPriority_Update]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[Microsite_ModelBannersPriority_Update]
GO

	
-- =============================================      
-- Author:  Kritika Choudhary
-- Create date:  10th June 2015 
-- Description: To Update Priority of Dealer Model Banners   
-- Input Parameter @Priority format = "Id:value,Id:value,"
-- =============================================      
CREATE PROCEDURE [dbo].[Microsite_ModelBannersPriority_Update]  
(  
    @Priority   VARCHAR(2000)
)      
AS      
BEGIN   
 DECLARE @Pointer int, @SortOrder INT, @ID INT, @Pair VARCHAR(50),@ColonPointer int
  -- SET NOCOUNT ON added to prevent extra result sets from      
 SET NOCOUNT ON; 
 SET @Priority = LTRIM(RTRIM(@Priority))  
 IF (RIGHT(@Priority, 1) != ',')  
         SET @Priority=@Priority+ ','
 SET @Pointer = CHARINDEX(',', @Priority, 0)  
 IF REPLACE(@Priority, ',', '') <> '' 
   BEGIN
   
	 WHILE(@Pointer > 0)
	 BEGIN
		  SET @Pair = LTRIM(RTRIM(LEFT(@Priority, @Pointer - 1)))
		  SET @ColonPointer = CHARINDEX(':', @Pair, 0)
		  SET @ID = LTRIM(RTRIM(LEFT(@Pair, @ColonPointer - 1)))
		  SET @SortOrder = LTRIM(RTRIM(RIGHT(@Pair, (@Pointer - @ColonPointer) - 1 )))
		  
		  UPDATE Microsite_DealerModelBanners
          SET SortOrder= @SortOrder
          WHERE ID= @ID  

		  SET @Priority = RIGHT(@Priority, LEN(@Priority) - @Pointer)  
          SET @Pointer = CHARINDEX(',', @Priority, 1)  
	 END
   END
	  
END
