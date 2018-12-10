IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[cw].[RetrievePageMetaTags]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [cw].[RetrievePageMetaTags]
GO

	
-- =============================================  
-- Author:  <Prashant Vishe >  
-- Create date: <On 24 May 2013>  
-- Description: <for retrieving Page Meta tags data>  
-- =============================================  
CREATE PROCEDURE [cw].[RetrievePageMetaTags]   
 -- Add the parameters for the stored procedure here  
 @Id numeric  
AS  
BEGIN  
 -- SET NOCOUNT ON added to prevent extra result sets from  
 -- interfering with SELECT statements.  
 SET NOCOUNT ON;  
  
    -- Insert statements for procedure here  
    declare @ModelId int  
    set @ModelId=(select ModelId From PageMetaTags where Id=@Id)  
      
    IF @ModelId is null  
  SELECT PM.*,PMM.PAGENAME,CM1.NAME AS MAKENAME,NULL AS MODELNAME FROM PAGEMETATAGS PM  
  LEFT JOIN  PAGEMETATAGSMASTER PMM ON PM.PAGEID=PMM.ID  
  LEFT JOIN CARMAKES CM1 ON PM.MAKEID=CM1.ID  
  WHERE  PM.Id=@Id  
    ELSE  
  SELECT PM.ID,PM.PAGEID,C.CARMAKEID AS MAKEID,PM.MODELID,PM.TITLE,PM.DESCRIPTION,PM.KEYWORDS,PM.HEADING,PM.ISACTIVE,PM.Summary,PMM.PAGENAME,CM.NAME AS MAKENAME,C.NAME AS MODELNAME  
  FROM PAGEMETATAGS PM   
  LEFT JOIN PAGEMETATAGSMASTER PMM ON PM.PAGEID=PMM.ID   
  LEFT JOIN CARMODELS C  ON  PM.MODELID=C.ID  
  LEFT JOIN CARMAKES CM ON C.CARMAKEID=CM.ID  
  WHERE  PM.Id=@Id   
   
END  
