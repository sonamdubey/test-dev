IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[BerkshireVehicleInfo_sp]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[BerkshireVehicleInfo_sp]
GO

	-- =============================================  
-- Author:  Satish Sharma  
-- Create date: May 2, 2012  
-- Description: This SP has specifically written for Berkshire Insurance to retrieve vehicle information  
-- =============================================  
CREATE PROCEDURE [dbo].[BerkshireVehicleInfo_sp]  
 -- Add the parameters for the stored procedure here  
 @VehicleCore VARCHAR(20), -- It can be 'MAKE', 'MODEL', 'VERSION'  
 @VechileMakeId SMALLINT = NULL,  
 @VechileModelId SMALLINT = NULL  
AS  
BEGIN  
 -- SET NOCOUNT ON added to prevent extra result sets from  
 -- interfering with SELECT statements.  
 SET NOCOUNT ON;  
  
    -- Insert statements for procedure here  
 IF @VehicleCore = 'MAKE'  
 BEGIN  
  -- PRODUCT_ID 1801 for Cars and 1802 for bikes  
  SELECT DISTINCT Bv.MAKE_NAME, Bv.MAKE_CODE   
  FROM BerkshireVehicleInfo AS Bv   
  WHERE Bv.PRODUCT_ID = 1801  AND Bv.MAKE_NAME<>'' 
  ORDER BY Bv.MAKE_NAME  
 END  
 ELSE IF @VehicleCore = 'MODEL'  
 BEGIN  
  SELECT DISTINCT Bv.MODEL_NAME AS Text, Bv.MODEL_CODE AS Value  
  FROM BerkshireVehicleInfo AS Bv   
  WHERE Bv.MAKE_CODE = @VechileMakeId AND Bv.PRODUCT_ID = 1801  AND  Bv.MODEL_NAME<>''
  ORDER BY Bv.MODEL_NAME  
 END  
 ELSE IF @VehicleCore = 'VERSION'  
 BEGIN  
  SELECT DISTINCT Bv.SUBTYPE_NAME AS Text, Bv.SUBTYPE_CODE As Value  
  FROM BerkshireVehicleInfo AS Bv   
  WHERE Bv.MAKE_CODE = @VechileMakeId AND Bv.MODEL_CODE = @VechileModelId AND Bv.PRODUCT_ID = 1801 AND  Bv.SUBTYPE_NAME<>''  
  ORDER BY Bv.SUBTYPE_NAME  
 END  
   
END  