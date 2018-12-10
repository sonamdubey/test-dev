IF EXISTS(
SELECT *
   FROM sys.views
     WHERE schema_id = SCHEMA_ID('dbo'))
     name = 'vwForAlerNewCarPurchaseInquiries' AND
     DROP VIEW dbo.vwForAlerNewCarPurchaseInquiries
GO

	CREATE view vwForAlerNewCarPurchaseInquiries
as
select *
from dbo.NewCarPurchaseInquiries_archive_0616
union
select *
from dbo.NewCarPurchaseInquiries with(nolock)