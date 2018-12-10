IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[BindCarModels]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[BindCarModels]
GO

	



-- =============================================  
-- Author: Prashant Vishe      
-- Create date: <04 Aug 2013>  
-- Description: used for retrieving car models related data...   exec BindCarModels 12
--Modified By:Prashant Vishe On 29 Jan 2014 
--modification:added query for retrieving car model roots. 
--Modified by Ashwini Todkar on 28 Sep 2015 retrieved RootId 
-- =============================================  
CREATE PROCEDURE [dbo].[BindCarModels] 
  -- Add the parameters for the stored procedure here  
  @MakeId INT 
AS 
  BEGIN 
      -- SET NOCOUNT ON added to prevent extra result sets from  
      -- interfering with SELECT statements.  
      SET nocount ON; 

      -- Insert statements for procedure here  
      SELECT DISTINCT( Mo.id ), 
                     Mo.platform, 
                     Mo.generation, 
                     Mo.upgrade, 
                     Mo.ModelLaunchDate, 
                     CR.rootname, 
					 CR.RootId, --added by Ashwini Todkar
                     Mo.name, 
                     MO.used, 
                     MO.new, 
                     MO.indian, 
                     MO.hosturl, 
                     MO.imported, 
                     MO.classic, 
                     MO.modified, 
                     MO.futuristic, 
                     MO.carmakeid, 
                     CONVERT(VARCHAR(24), Mo.mocreatedon, 113) AS CreatedOn, 
                     CONVERT(VARCHAR(24), Mo.moupdatedon, 113) AS UpdatedOn, 
                     OU.username                               AS UpdatedBy, 
                     TSC.entrydate, 
                     MO.smallpic, 
                     MO.maskingname, 
                     ( CASE 
                         WHEN TSC.status IS NULL THEN 0 
                         ELSE TSC.status 
                       END )                                   AS IsTopSelling 
      FROM   (carmodels Mo WITH(NOLOCK)
              LEFT JOIN con_topsellingcars TSC  WITH(NOLOCK)
                     ON TSC.modelid = Mo.id) 
             LEFT JOIN oprusers OU  WITH(NOLOCK)
                    ON Mo.moupdatedby = OU.id 
             LEFT JOIN carmodelroots CR  WITH(NOLOCK)
                    ON CR.rootid = Mo.rootid 
      WHERE  MO.isdeleted = 0 
             AND Mo.carmakeid = @MakeId 
      ORDER  BY MO.futuristic DESC ,Mo.new DESC, 
                CR.rootname ASC 
  END 

