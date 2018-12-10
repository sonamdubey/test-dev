IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[cw].[RecentlyLaunchedCars29012015]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [cw].[RecentlyLaunchedCars29012015]
GO

	-- =============================================            
-- Author:  <Prashant Vishe>            
-- Create date: <01 Nov 2012>            
-- Description:<Returns Recently launched Cars by launched date in descending order>EXEC cw.RecentlyLaunchedCars 1,10, 10            
-- Avishkar 15-1-2013 Modified to show correct count  
-- Vikas 16-1-2013 Modifed Table ( Join Con_NewCarNationalPrices instead of NewCarShowroomPrices -- also changed period to 1 year from 6 months  
-- Modified By : Ashish G. Kamble.  
-- Description : Prices are refered from carmodels table. Prices are shown from new delhi  
--modified on 1/10/14 initial value of input parameter are set to null and if no parameter is passed from codebehind on sp call default value is set to null and where clause stay true always
CREATE PROCEDURE [cw].[RecentlyLaunchedCars29012015]         
 -- Add the parameters for the stored procedure here            
  @StartIndex INT=NULL,        
  @EndIndex INT=NULL ,        
  @cityId smallint =NULL         
AS            
BEGIN            
 -- SET NOCOUNT ON added to prevent extra result sets from            
 -- interfering with SELECT statements.            
 SET NOCOUNT ON;            
             
 SELECT distinct * FROM(        
  SELECT *,ROW_NUMBER()OVER(ORDER BY LaunchDate DESC)Row FROM(       
   SELECT * FROM (    
   (SELECT         
   Distinct SP.id as launchid, (Ma.Name + ' ' + Mo.Name) AS CarName,        
   Ma.Name as Make,         
   Mo.Name as Model,        
   Mo.Id as ModelId,        
   E.BasicId,    
   SP.Description,    
   SP.LaunchDate AS LaunchDate,        
   Mo.ReviewRate AS ReviewRate,        
   Mo.ReviewCount,        
   Mo.LargePic AS ModelImage,        
   MO.HostURL,        
   --MIN(NP.MinPrice) OVER( PARTITION BY MO.ID ) AS MinPrice,        
   --MAX(NP.MaxPrice) OVER( PARTITION BY MO.ID ) AS MaxPrice,    
   MO.MinPrice,  
   MO.MaxPrice,      
   ROW_NUMBER()OVER(PARTITION BY SP.CarModelId ORDER BY SP.CarModelId DESC) ModelRow,
   MO.MaskingName    
           
   FROM ExpectedCarLaunches SP        
   --INNER JOIN vwMMV AS vw on vw.ModelId= SP.CarModelId        
   INNER JOIN  CarModels Mo WITH (NOLOCK) ON MO.ID = SP.CarModelId -- and MO.IsDeleted=0        
   INNER JOIN CarVersions VS WITH (NOLOCK) ON VS.CarModelId = MO.ID        
   INNER JOIN CarMakes Ma WITH (NOLOCK) ON Ma.ID = Mo.CarMakeId        
   --LEFT JOIN NewCarShowroomPrices NP WITH (NOLOCK) ON NP.CarVersionId = VS.ID AND NP.CityId = @cityId AND NP.IsActive = 1         
   --LEFT JOIN Con_NewCarNationalPrices NP WITH (NOLOCK) ON NP.VersionId = VS.ID AND NP.IsActive = 1  
   LEFT JOIN (    
    SELECT EC.BasicId, EC.ModelId     
    FROM Con_EditCms_Cars EC INNER JOIN Con_EditCms_Basic EB ON EB.Id = EC.BasicId AND EB.IsPublished = 1 AND EB.CategoryId = 8 -- roadtests    
   ) AS E     
   ON E.ModelId = MO.ID         
   WHERE SP.IsLaunched = 1 AND SP.IsDeleted = 0 AND Mo.New = 1 AND Mo.Futuristic = 0 AND Mo.IsDeleted = 0     
  AND VS.New=1 AND VS.IsDeleted = 0 AND SP.LaunchDate >= DATEADD(YEAR, -1, GETDATE()))) AS Tbl_Inner WHERE ModelRow = 1 ) AS Tab) AS Tab1        
  WHERE (  (Tab1.Row BETWEEN @StartIndex AND @EndIndex)  OR  @StartIndex IS NULL)
  ORDER BY LaunchDate DESC ;   
          
  --SELECT COUNT(DISTINCT SP.Id) AS RecordCount        
  --FROM ExpectedCarLaunches SP        
  -- INNER JOIN  CarModels Mo WITH (NOLOCK) ON MO.ID = SP.CarModelId        
  -- INNER JOIN CarVersions VS WITH (NOLOCK) ON VS.CarModelId = MO.ID        
  --WHERE IsLaunched = 1 AND Mo.Futuristic = 0 AND Mo.IsDeleted = 0  AND VS.New=1 AND VS.IsDeleted = 0     
  --AND LaunchDate >= DATEADD(YEAR, -1, GETDATE() ) -- Avishkar 15-1-2013 Modified to show correct count        
 with CTE  
 as  
 (  
  SELECT DISTINCT SP.Id,ROW_NUMBER() OVER(PARTITION BY MO.Id order by Mo.MinPrice asc) as row --NP.MinPrice asc) as row   
  FROM ExpectedCarLaunches SP        
  INNER JOIN  CarModels Mo WITH (NOLOCK) ON MO.ID = SP.CarModelId        
  INNER JOIN CarVersions VS WITH (NOLOCK) ON VS.CarModelId = MO.ID    
  INNER JOIN CarMakes Ma  WITH (NOLOCK) ON Ma.ID = Mo.CarMakeId     and Ma.IsDeleted=0    
  --LEFT JOIN Con_NewCarNationalPrices NP WITH (NOLOCK) ON NP.VersionId = VS.ID  AND NP.IsActive = 1      
  WHERE IsLaunched = 1 AND Mo.Futuristic = 0 AND Mo.IsDeleted = 0  AND VS.New=1 AND VS.IsDeleted = 0     
  AND SP.LaunchDate >= DATEADD(YEAR, -1, GETDATE() ) -- Avishkar 15-1-2013 Modified to show correct count  
  AND SP.IsDeleted = 0  
  AND Mo.New = 1    
 )  
 SELECT COUNT(Id) AS RecordCount  
 FROM CTE  
 WHERE row=1        
END         
        
        
        
        
        
