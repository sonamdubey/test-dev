IF EXISTS(
SELECT *
   FROM sys.views
     WHERE schema_id = SCHEMA_ID('dbo'))
     name = 'vwNewCarQuoteOptions' AND
     DROP VIEW dbo.vwNewCarQuoteOptions
GO

	Create view vwNewCarQuoteOptions
as
select * from NewCarQuoteOptions
union
select * from NewCarQuoteOptions_History 