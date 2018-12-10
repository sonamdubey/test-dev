IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[Classified_SellersOffering_v16]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[Classified_SellersOffering_v16]
GO

	-- =============================================
-- Author:		<kush kumar>
-- Create date: <02/11/2012>
-- Description:	<Procedure to get Seller's Offerings >
-- Modified by Prachi Phalak on 04/08/2016 --Added check of sourceId for Autobiz cars.    
-- =============================================

CREATE PROCEDURE [dbo].[Classified_SellersOffering_v16.8.1]

	-- Input Parameter
	@ID INT
	AS
	BEGIN 
		SELECT cv.TC_CarValueAdditionsId, cv.ValueAddName, cv.Logo 
		FROM TC_StockCarValueAdditions cva WITH (NOLOCK)
		INNER JOIN TC_CarValueAdditions cv WITH (NOLOCK) ON cv.TC_CarValueAdditionsId = cva.TC_CarValueAdditionsId 
		INNER JOIN SellInquiries si WITH (NOLOCK) ON cva.TC_StockId = si.TC_StockId and SI.TC_StockId IS NOT NULL AND si.SourceId = 2
		INNER JOIN LiveListings as ll WITH (NOLOCK) ON ll.Inquiryid = si.ID and ll.SellerType = 1
		WHERE cva.IsActive = 1 AND si.ID = @ID
	END
	


