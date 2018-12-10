IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[BerkshireCityInfo_SP]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[BerkshireCityInfo_SP]
GO

	-- =============================================  
-- Author:  Ashish G. Kamble  
-- Create date: May 2, 2012  
-- Description: This SP has specifically written for Berkshire Insurance to retrieve City information  
-- =============================================  
CREATE PROCEDURE [dbo].[BerkshireCityInfo_SP]  
 -- Add the parameters for the stored procedure here  
 @LocationCore VARCHAR(20), -- It can be 'MAKE', 'MODEL', 'VERSION'  
 @State_Code SMALLINT = NULL,  
 @City_Code SMALLINT = NULL  
AS  
BEGIN  
 -- SET NOCOUNT ON added to prevent extra result sets from  
 -- interfering with SELECT statements.  
 SET NOCOUNT ON;  
  
 -- Insert statements for procedure here  
 IF @LocationCore = 'STATE'  
 BEGIN  
  SELECT DISTINCT Bc.STATE_CODE,Bc.STATE  
  FROM BerkshireCityInfo AS Bc   
  WHERE Bc.Is_Deleted = 0  
  ORDER BY Bc.STATE  
 END  
 ELSE IF @LocationCore = 'CITY'  
 BEGIN  
  SELECT DISTINCT Bc.CITY_CODE AS Value, Bc.CITY AS Text  
  FROM BerkshireCityInfo AS Bc   
  WHERE Bc.STATE_CODE = @State_Code AND Bc.Is_Deleted = 0 AND Bc.CITY<>''  
  ORDER BY Bc.CITY  
 END  
 ELSE IF @LocationCore = 'AREA'  
 BEGIN  
  SELECT DISTINCT Bc.ID AS Value, Bc.AREA_NAME + ' ' +  CAST(Bc.PIN AS NVARCHAR) AS Text  
  FROM BerkshireCityInfo AS Bc   
  WHERE Bc.CITY_CODE = @City_Code AND Bc.Is_Deleted = 0 
  ORDER BY Text  
 END  
END 

