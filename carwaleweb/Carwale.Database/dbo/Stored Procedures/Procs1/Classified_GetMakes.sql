IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[Classified_GetMakes]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[Classified_GetMakes]
GO

	CREATE PROCEDURE [dbo].[Classified_GetMakes]
AS  
BEGIN  
 SET NOCOUNT ON;  
 
	SELECT Id AS Value, Name AS Text 
	FROM CarMakes 
	WHERE IsDeleted = 0 
	AND Used = 1 
	AND Futuristic = 0
	ORDER BY Name 
END 