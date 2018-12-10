IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_GetAreaByCityId]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_GetAreaByCityId]
GO
	-- =============================================  
-- Author:  Vicky Gupta 
-- Create date: 19 October 2015  
-- Description: Get all Area of a given cityId  
-- =============================================  
CREATE PROCEDURE [dbo].[TC_GetAreaByCityId] 
@CityId INT
AS  	
BEGIN  
	SELECT ID,Name FROM Areas WITH (NOLOCK) WHERE IsDeleted=0 AND CityId =@CityId ORDER BY Name	
END 



