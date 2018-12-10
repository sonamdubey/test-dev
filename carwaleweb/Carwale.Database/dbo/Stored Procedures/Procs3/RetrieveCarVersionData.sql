IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[RetrieveCarVersionData]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[RetrieveCarVersionData]
GO

	-- =============================================  
-- Author:  <Prashant Vishe>  
-- Create date: <30 Aug 2013>  
-- Description: <For retrieving car versions related data.>  
-- =============================================  
CREATE PROCEDURE [dbo].[RetrieveCarVersionData]   
 -- Add the parameters for the stored procedure here  
  
AS  
BEGIN  
 -- SET NOCOUNT ON added to prevent extra result sets from  
 -- interfering with SELECT statements.  
 SET NOCOUNT ON;  
  
    -- Insert statements for procedure here  
 SELECT Id, Name FROM CarSegments where Name IS NOT NULL  ORDER bY NAME  
  
  
 SELECT Id, Name FROM CarSubSegments where Name IS NOT NULL  ORDER bY NAME  
  
  
 SELECT Id, Name FROM CarBodyStyles where Name IS NOT NULL  ORDER bY NAME  
  
  
 SELECT FUELTYPEID AS Id,FUELTYPE AS Name FROM CARFUELTYPE where FUELTYPE IS NOT NULL  ORDER bY NAME  
  
   
 SELECT Id,DESCR AS Name FROM CARTRANSMISSION where DESCR IS NOT NULL  ORDER bY NAME  
                   
END
