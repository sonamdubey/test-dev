IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[BW_UpdateDealerOffers]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[BW_UpdateDealerOffers]
GO

	-- =============================================
-- Author:		Suresh Prajapati
-- Create date: 07th Jan, 2015
-- Description:	To Update Dealer offers.
-- =============================================
CREATE PROCEDURE [dbo].[BW_UpdateDealerOffers] 
	@OfferId INT
	,@UserId BIGINT
	,@OfferCategoryId INT
	,@OfferText VARCHAR (MAX)
	,@OfferValue INT
	,@OfferValidTill DATETIME
	
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
		WHERE Id=@OfferId
END
