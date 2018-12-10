IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_GetCities]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_GetCities]
GO

	
-- =============================================        
-- Author:  Kritika Choudhary        
-- Create date: 5th april 2016      
-- Description: Fetches all cities for a particular state      
-- =============================================        
CREATE PROCEDURE [dbo].[TC_GetCities]   
@StateId INT
AS        
BEGIN        
 -- SET NOCOUNT ON added to prevent extra result sets from        
 -- interfering with SELECT statements.        
 SET NOCOUNT ON;       
	SELECT ID AS Value, Name AS Text 
	FROM Cities WITH(NOLOCK) 
	WHERE IsDeleted = 0 AND	StateId = @stateId ORDER BY Text
END 
