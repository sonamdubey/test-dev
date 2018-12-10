IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[Microsite_GetModelWiseNavigation]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[Microsite_GetModelWiseNavigation]
GO

	-- =============================================        
-- Author:  Vikas J        
-- Create date: 4/6/2013,         
-- Description: SP to Get the list of model based navigation list.This navigation list is dynamic for a website based on delaerid and modelid.  
-- =============================================        
        
CREATE PROCEDURE [dbo].[Microsite_GetModelWiseNavigation]          
(          
 @ModelId INT,  
 @DealerId INT         
)     
AS            
 --  fetching all the navigations avialable for that model and for that dealer          
 BEGIN           
   SELECT ID,NavigationText,NavigationLink,entryDateTime,NavigationOrder,isActive,NavigationId   
   FROM DealerWebsite_NavigationLinks 
   WHERE ModelId=@ModelId AND DealerId=@DealerId
 END
