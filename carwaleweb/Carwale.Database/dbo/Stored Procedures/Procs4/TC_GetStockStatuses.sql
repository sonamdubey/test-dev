IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_GetStockStatuses]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_GetStockStatuses]
GO

	--========================================================
-- Author		: Suresh Prajapati
-- Created Date : 1st Oct, 2015
-- Description	: To Get All possible statuses of a stock
--========================================================
CREATE PROCEDURE [dbo].[TC_GetStockStatuses]
AS
BEGIN
	SET NOCOUNT ON;

	SELECT Id
		,[Status]
	FROM TC_StockStatus WITH (NOLOCK)
	WHERE IsActive = 1
END

