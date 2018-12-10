IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[GetCustStockLogData]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[GetCustStockLogData]
GO

	
-- =============================================
-- Author:		Sahil Sharma
-- Create date: 14th oct, 2016
-- Description:	To get individual stock log (create, update, delete).
-- =============================================
CREATE PROCEDURE [dbo].[GetCustStockLogData]
	-- No parameters
AS
BEGIN
	SELECT Id, CustSellInquiryId AS InquiryId , EntryTime, ActionType FROM CustStockLog WITH(NOLOCK);
END

