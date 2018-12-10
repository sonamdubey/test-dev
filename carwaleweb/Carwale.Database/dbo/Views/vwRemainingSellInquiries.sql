IF EXISTS(
SELECT *
   FROM sys.views
     WHERE schema_id = SCHEMA_ID('dbo'))
     name = 'vwRemainingSellInquiries' AND
     DROP VIEW dbo.vwRemainingSellInquiries
GO

	
CREATE VIEW vwRemainingSellInquiries AS 

SELECT ds.DealerId,
	( ds.MaxSellInquiries - ( SELECT COUNT(ID) FROM SellInquiries WHERE StatusId = 1 AND dealerId=ds.DealerId ) ) AS Remaining
FROM DealerSettings ds




