IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_GetCityByStateId]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_GetCityByStateId]
GO
	
-- =============================================  
-- Author:  Vicky Gupta 
-- Create date: 19 October 2015  
-- Description: Get all city of a given stateId  
-- =============================================  
CREATE PROCEDURE [dbo].[TC_GetCityByStateId] 
@StateId INT
AS  	
BEGIN  
	SELECT Id,Name FROM Cities WITH (NOLOCK) WHERE IsDeleted=0 AND StateId =@StateId ORDER BY Name	
END 

