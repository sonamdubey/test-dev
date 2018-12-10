IF EXISTS(
SELECT *
   FROM sys.views
     WHERE schema_id = SCHEMA_ID('dbo'))
     name = 'vwNewCarMake' AND
     DROP VIEW dbo.vwNewCarMake
GO

	CREATE VIEW vwNewCarMake AS
SELECT id AS Makeid,name AS Make FROM CarMakes 
WHERE id in (18,1,2,56,4,5,7,8,9,10,11,21,45,15,16,17,20)