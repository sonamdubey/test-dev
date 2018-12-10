IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[cw].[TopFeaturedCars]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [cw].[TopFeaturedCars]
GO

	CREATE PROCEDURE [cw].[TopFeaturedCars]        
 -- Add the parameters for the stored procedure here        
  @TopCount INT    
        
AS        
BEGIN        
 -- SET NOCOUNT ON added to prevent extra result sets from        
 -- interfering with SELECT statements.        
 SET NOCOUNT ON;        
         
	SELECT TOP(@TOPCOUNT) EntryDateTime,Co.ID ,
	M.Name+ ' '+ Mo.Name AS CarName,M.Name AS MakeName,
	Mo.Name AS ModelName, Co.HostUrl+Co.DirectoryPath AS ImagePath
	FROM Con_FeaturedListings Co,CarModels Mo,CarMakes M
	WHERE Co.CarId=Mo.ID AND Mo.CarMakeId=M.Id AND ISACTIVE = 1 ORDER BY EntryDateTime DESC      
END     
    
    
    
    
    





