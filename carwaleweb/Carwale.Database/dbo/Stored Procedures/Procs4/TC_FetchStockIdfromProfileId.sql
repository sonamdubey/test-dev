IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_FetchStockIdfromProfileId]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_FetchStockIdfromProfileId]
GO

	-- =============================================
-- Author:		Ruchira Patil
-- Create date: 25th Aug 2014
-- Description:	To fetch the stock id of MFC dealer from profile id to push the leads through API
-- Modified By: Tejashree Patil on 25 Aug 2014, Fetched branchId.
-- =============================================
CREATE PROCEDURE [dbo].[TC_FetchStockIdfromProfileId]
@ProfileId INT
AS
BEGIN
	SELECT	SI.TC_StockId AS StockId,  SI.DealerId AS BranchId
	FROM	SellInquiries SI WITH(NOLOCK)
	WHERE	SI.ID=@ProfileId
END
