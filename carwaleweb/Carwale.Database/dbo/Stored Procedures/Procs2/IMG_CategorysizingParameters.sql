IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[IMG_CategorysizingParameters]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[IMG_CategorysizingParameters]
GO

	
-- Author		:	Chetan Dev
-- Create date	:	01/03/2013 15:58:26 
-- Description	:	This sp fetch width, height, Iscrop,IsWatermark according to CategoryId
-- =============================================    
CREATE PROCEDURE [dbo].[IMG_CategorysizingParameters]    
 -- Add the parameters for the stored procedure here    
 @Category VARCHAR(100)
 

As
BEGIN    
 -- SET NOCOUNT ON added to prevent extra result sets from    
 -- interfering with SELECT statements.    
SET NOCOUNT ON;

DECLARE

@CategoryId int


SELECT @CategoryId = S.CategoryId 
FROM img_categories S WITH(NOLOCK)
WHERE S.CategoryName = @Category


SELECT * FROM IMG_CategoryResizing WITH(NOLOCK)
WHERE CategoryId = @CategoryId
END    


