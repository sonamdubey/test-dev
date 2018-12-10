IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[BW_UpdateDealerOffers_07012016]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[BW_UpdateDealerOffers_07012016]
GO

	
-- =============================================
-- Author:		Suresh Prajapati
-- Create date: 07th Jan, 2015
-- Description:	To Update Dealer offers.

-- Modified by: Sangram Nandkhile on 07th Jan 2016 
-- Description:	Added new parameter isPriceImpact
-- =============================================
CREATE PROCEDURE [dbo].[BW_UpdateDealerOffers_07012016] 
	@OfferId INT
	,@UserId BIGINT
	,@OfferCategoryId INT
	,@OfferText VARCHAR (MAX)
	,@OfferValue INT
	,@OfferValidTill DATETIME
	,@isPriceImpact bit
	
AS
BEGIN
	SET NOCOUNT OFF;

	UPDATE BW_PQ_Offers 
		SET OfferCategoryId=@OfferCategoryId
			,OfferText=@OfferText
			,OfferValue=ISNULL(@OfferValue,0)
			,OfferValidTill=@OfferValidTill
			,LastUpdated=GETDATE()
			,UpdatedBy=@UserId
			,isPriceImpact = @isPriceImpact
		WHERE Id=@OfferId
END
