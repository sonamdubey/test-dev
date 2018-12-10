IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[CheckStockIdValidation]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[CheckStockIdValidation]
GO

	

-- =============================================
-- Author:		Manish
-- Create date: 19-06-2013
-- Description:	Alert if any record insert into sellinquiries with no stock id
-- =============================================
CREATE PROCEDURE [dbo].[CheckStockIdValidation]	
AS
BEGIN
    SELECT ID AS [SellInquiriesId] FROM  SellInquiries WITH (NOLOCK)
	WHERE TC_StockId IS NULL
	AND EntryDate > '2012-12-31 23:59:00.413'
END
