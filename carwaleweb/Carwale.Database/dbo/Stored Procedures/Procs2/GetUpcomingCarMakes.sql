IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[GetUpcomingCarMakes]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[GetUpcomingCarMakes]
GO

	-- =============================================  
-- Author:  <Prashant Vishe>  
-- Create date: <05 April 2013>  
-- Description: <Retrieves Upcoming Car Make Details>  
-- =============================================  
CREATE PROCEDURE [dbo].[GetUpcomingCarMakes]   
 -- Add the parameters for the stored procedure here  
   
AS  
BEGIN  
 -- SET NOCOUNT ON added to prevent extra result sets from  
 -- interfering with SELECT statements.  
 SET NOCOUNT ON;  
  
    -- Insert statements for procedure here  
 SELECT DISTINCT MA.ID AS ID, NAME AS NAME FROM EXPECTEDCARLAUNCHES ECL  
       INNER JOIN CARMAKES MA ON MA.ID = ECL.CARMAKEID  
       WHERE ISLAUNCHED = 0 ORDER BY NAME  
END
