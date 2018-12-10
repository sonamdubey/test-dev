IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[Microsite_ModelWiseNavigation_Save]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[Microsite_ModelWiseNavigation_Save]
GO

	-- =============================================      
-- Author:  Vikas J  
-- Create date:04/06/2013   
-- Description: To save all data of navigation list for a model that is updated by the dealer for his website  
-- =============================================      
CREATE PROCEDURE [dbo].[Microsite_ModelWiseNavigation_Save]       
(      
 @DealerId INT,      
 @ModelId INT,
 @Id INT,  
 @NavigationText VARCHAR(MAX),  
 @NavigationLink VARCHAR(MAX),
 @NavigationOrder INT,  
 @IsActive  BIT  ,
 @NavigationID INT  
)      
AS      
BEGIN      
 -- SET NOCOUNT ON added to prevent extra result sets from      
 SET NOCOUNT ON;  
 DECLARE @COUNT INT=0;  
 SELECT @COUNT=COUNT(*) FROM  DealerWebsite_NavigationLinks WHERE ID=@Id ;     
 IF(@COUNT=0)    
  BEGIN      
   INSERT INTO DealerWebsite_NavigationLinks(ModelId,NavigationText,NavigationLink,isActive,entryDateTime,DealerId,NavigationOrder,NavigationId)     
   VALUES(@ModelId,@NavigationText,@NavigationLink,@IsActive,GETDATE(),@DealerId,@NavigationOrder,@NavigationID)    
  END      
 ELSE      
  BEGIN      
   UPDATE DealerWebsite_NavigationLinks   
   SET NavigationText=@NavigationText,NavigationLink=@NavigationLink,isActive=@IsActive,  
    NavigationOrder=@NavigationOrder,NavigationId=@NavigationID
   WHERE DealerId=@DealerId AND Id=@Id AND ModelId=@ModelId     
 END       
END
