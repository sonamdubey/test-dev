IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[Microsite_ConversionDetails_View]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[Microsite_ConversionDetails_View]
GO

	-- =============================================      
-- Author:  Kritika Choudhary
-- Create date:  17th July 2015 
-- Description: To view the conversion details

-- =============================================      
CREATE PROCEDURE [dbo].[Microsite_ConversionDetails_View]  
     
AS      
BEGIN   

  -- SET NOCOUNT ON added to prevent extra result sets from      
 SET NOCOUNT ON; 
  SELECT ConversionId,ConversionLabel,PageType,PageId,CD.ID AS ID,CD.IsActive AS IsActive
  FROM Microsite_DealerConversionDetails CD
  JOIN Microsite_DealerPages DP ON PageId=DP.ID

END
