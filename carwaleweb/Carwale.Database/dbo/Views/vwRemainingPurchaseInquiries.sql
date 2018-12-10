IF EXISTS(
SELECT *
   FROM sys.views
     WHERE schema_id = SCHEMA_ID('dbo'))
     name = 'vwRemainingPurchaseInquiries' AND
     DROP VIEW dbo.vwRemainingPurchaseInquiries
GO

	


CREATE VIEW vwRemainingPurchaseInquiries AS

SELECT ds.DealerId,
	( ds.MaxPurchaseInquiries - ( SELECT COUNT(ID) FROM PurchaseInquiries WHERE StatusId = 1 AND dealerId=ds.DealerId ) ) AS Remaining
FROM DealerSettings ds



