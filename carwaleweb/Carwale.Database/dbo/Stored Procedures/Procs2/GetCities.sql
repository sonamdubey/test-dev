IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[GetCities]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[GetCities]
GO

	-- =============================================        
-- Author:  Umesh Ojha        
-- Create date: 22 feb 2013      
-- Description: Fetching all cities of the Sates      
-- =============================================        
Create PROCEDURE [dbo].[GetCities]   
@StateId TINYINT
AS        
BEGIN        
 -- SET NOCOUNT ON added to prevent extra result sets from        
 -- interfering with SELECT statements.        
 SET NOCOUNT ON;       
	SELECT ID AS Value, Name AS Text 
	FROM Cities WHERE IsDeleted = 0 AND
	StateId = @stateId ORDER BY Text
END
