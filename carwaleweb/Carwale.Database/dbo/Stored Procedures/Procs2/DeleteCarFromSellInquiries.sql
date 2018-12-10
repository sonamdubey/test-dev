IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[DeleteCarFromSellInquiries]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[DeleteCarFromSellInquiries]
GO

	-- =============================================
-- Author:		Afrose
-- Create date: 25th July,2015
-- Description:	This will delete entry from livelistings
-- =============================================
CREATE PROCEDURE [dbo].[DeleteCarFromSellInquiries]
    @TC_StockId INT,
	@SourceId TINYINT,
	@DealerId INT

	AS
	BEGIN

		 UPDATE SellInquiries
		 SET StatusId=2
		 WHERE TC_StockId=@TC_StockId AND SourceId=@SourceId AND DealerId =@DealerId
				
	END


