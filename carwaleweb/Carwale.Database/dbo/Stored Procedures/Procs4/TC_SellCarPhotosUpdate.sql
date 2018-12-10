IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_SellCarPhotosUpdate]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_SellCarPhotosUpdate]
GO

	-- Author  : Surendra  
-- Create date : 09/10/2012 14:00 PM  
-- Description : will set IsActive 1
-- =============================================      
CREATE PROCEDURE [dbo].[TC_SellCarPhotosUpdate]      
 -- Add the parameters for the stored procedure here      
 @PhotoId   BIGINT 
AS      
BEGIN      
 -- SET NOCOUNT ON added to prevent extra result sets from      
 -- interfering with SELECT statements.      
SET NOCOUNT ON;  
-- updating final image name  
UPDATE TC_SellCarPhotos SET IsActive=1
WHERE Id=@PhotoId  
    
END    




