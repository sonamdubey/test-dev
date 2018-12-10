IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[cw].[GetPageMetaTags]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [cw].[GetPageMetaTags]
GO

	-- =============================================  
-- Author:  <Prashant Vishe>  
-- Create date: <10 JULY 2013>  
-- Description: <to retrieve page meta tags information>  
-- =============================================  
Create PROCEDURE [cw].[GetPageMetaTags]  
 -- Add the parameters for the stored procedure here 
 @PageId numeric 
   
AS  
BEGIN  
 -- SET NOCOUNT ON added to prevent extra result sets from  
 -- interfering with SELECT statements.  
 SET NOCOUNT ON;  
  
    -- Insert statements for procedure here  
    SELECT * FROM PAGEMETATAGS WHERE ISACTIVE=1  and PageId=@PageId
   
END  