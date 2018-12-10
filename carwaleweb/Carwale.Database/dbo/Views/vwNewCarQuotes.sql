IF EXISTS(
SELECT *
   FROM sys.views
     WHERE schema_id = SCHEMA_ID('dbo'))
     name = 'vwNewCarQuotes' AND
     DROP VIEW dbo.vwNewCarQuotes
GO

	Create view vwNewCarQuotes
as
select * from NewCarQuotes
union
select * from NewCarQuotes_History 