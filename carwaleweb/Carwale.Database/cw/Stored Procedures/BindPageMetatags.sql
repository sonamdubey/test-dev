IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[cw].[BindPageMetatags]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [cw].[BindPageMetatags]
GO

	
-- =============================================  
-- Author:  <Prashant Vishe>  
-- Create date: <On 24 May 2013>  
-- Description: <For binding Page Meta tags data>  
-- =============================================  
CREATE PROCEDURE [cw].[BindPageMetatags]  
 -- Add the parameters for the stored procedure here  
   
AS  
BEGIN  
 -- SET NOCOUNT ON added to prevent extra result sets from  
 -- interfering with SELECT statements.  
 SET NOCOUNT ON;  
  
    -- Insert statements for procedure here  
 SELECT PM.*,PMM.PAGENAME,CM1.NAME AS MAKENAME,NULL AS MODELNAME FROM PAGEMETATAGS PM  
 LEFT JOIN  PAGEMETATAGSMASTER PMM ON PM.PAGEID=PMM.ID  
 LEFT JOIN CARMAKES CM1 ON PM.MAKEID=CM1.ID  
 WHERE PM.MODELID IS NULL  
  
 UNION  
  
 SELECT PM.ID,PM.PAGEID,C.CARMAKEID AS MAKEID,PM.MODELID,PM.TITLE,PM.DESCRIPTION,PM.KEYWORDS,PM.HEADING,PM.ISACTIVE,PM.Summary,PMM.PAGENAME,CM.NAME AS MAKENAME,C.NAME AS MODELNAME  
 FROM PAGEMETATAGS PM   
 LEFT JOIN PAGEMETATAGSMASTER PMM ON PM.PAGEID=PMM.ID   
 LEFT JOIN CARMODELS C  ON  PM.MODELID=C.ID  
 LEFT JOIN CARMAKES CM ON C.CARMAKEID=CM.ID  
 WHERE PM.MAKEID IS NULL  
END  
