IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[App].[GetTopMakes]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [App].[GetTopMakes]
GO

	CREATE PROCEDURE [App].[GetTopMakes]
	
AS
BEGIN
	SET NOCOUNT ON;
	
	SELECT TOP 8 Id, Name, LogoUrl 
		   FROM	carwale_com.dbo.CarMakes 
		   WHERE New = 1 and IsDeleted = 0
		   AND ID NOT IN (46, 48, 53, 54, 55, 56)
		   ORDER BY Name
END