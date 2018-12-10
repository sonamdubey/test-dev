IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[Classified_SellersOffering_15]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[Classified_SellersOffering_15]
GO

	-- =============================================
-- Author:		<kush kumar>
-- Create date: <02/11/2012>
-- Description:	<Procedure to get Seller's Offerings >
-- =============================================

CREATE PROCEDURE [dbo].[Classified_SellersOffering_15.4.1]

	-- Input Parameter
	@ID NUMERIC(18,0)
	AS
	BEGIN 
		SELECT cv.TC_CarValueAdditionsId, cv.ValueAddName, cv.Logo 
		FROM TC_StockCarValueAdditions cva WITH (NOLOCK)
		INNER JOIN TC_CarValueAdditions cv WITH (NOLOCK) ON cv.TC_CarValueAdditionsId = cva.TC_CarValueAdditionsId 
		INNER JOIN SellInquiries si WITH (NOLOCK) ON cva.TC_StockId = si.TC_StockId and SI.TC_StockId IS NOT NULL
		INNER JOIN LiveListings as ll WITH (NOLOCK) ON ll.Inquiryid = si.ID and ll.SellerType = 1
		WHERE cva.IsActive = 1 AND si.ID = @ID
	END