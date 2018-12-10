IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[Microsite_GetAllConversionPageText]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[Microsite_GetAllConversionPageText]
GO

	-- =============================================      
-- Author:  Kritika Choudhary
-- Create date:  16th July 2015 
-- Description: To get all dealer conversion page text

-- =============================================      
CREATE PROCEDURE [dbo].[Microsite_GetAllConversionPageText]  
     
AS      
BEGIN   

  -- SET NOCOUNT ON added to prevent extra result sets from      
 SET NOCOUNT ON; 
  
  SELECT ID AS Value,PageType as Text
  FROM Microsite_DealerPages
  WHERE IsActive=1


END
