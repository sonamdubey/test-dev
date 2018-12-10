IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[App].[GetMakes]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [App].[GetMakes]
GO

	
-- =========================================
-- Author:		Supriya
-- Create date: 10/01/2012
-- Description:	SP for fetching make list 
-- ========================================

CREATE PROCEDURE [App].[GetMakes]
	
AS
BEGIN
	SET NOCOUNT ON;
	
	SELECT Id, Name, LogoUrl 
		   FROM	CarMakes 
		   WHERE New = 1 and IsDeleted = 0
		   AND ID NOT IN (46, 48, 53, 54, 55, 56)
		   ORDER BY Name
END
