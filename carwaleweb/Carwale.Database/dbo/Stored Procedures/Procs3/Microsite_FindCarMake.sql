IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[Microsite_FindCarMake]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[Microsite_FindCarMake]
GO

	
      
      
-- =============================================        
-- Author:  Chetan Kane        
-- Create date: 2nd July 2012      
-- Description: To slect car Make for the select Make dropdown in FindCar widget, only the Makes available in the dealers stock will be fetch       
-- Modifier : Rakesh Y 29 June 2015 Added TS.IsActive flag
-- =============================================        
CREATE PROCEDURE [dbo].[Microsite_FindCarMake]      
(        
 @DealerId INT      
)        
AS        
BEGIN        
 -- SET NOCOUNT ON added to prevent extra result sets from        
 -- interfering with SELECT statements.        
 SET NOCOUNT ON;        
       
 SELECT DISTINCT Make AS Text,MakeId AS Value   
 FROM vwMMV AS V INNER JOIN TC_Stock AS TS ON V.VersionId = TS.VersionId   
 WHERE TS.BranchId = @DealerId  AND TS.StatusId = 1 AND TS.IsActive = 1 ORDER BY V.make  
END 
