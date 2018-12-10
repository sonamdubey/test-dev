IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[AE_BestOffer_SP]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[AE_BestOffer_SP]
GO

	

-- =============================================
-- Author:		Satish Sharma
-- Create date: 26-02-2010
-- Description:	To Save Best Offer Price of Bidder
-- =============================================
CREATE PROCEDURE [dbo].[AE_BestOffer_SP]
	-- Add the parameters for the stored procedure here
	@AuctionCarId		NUMERIC,
	@BidderId			NUMERIC,
	@OfferPrice			NUMERIC,
	@IpAddress			VARCHAR(50),
	@Status				VARCHAR(50) OUTPUT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
   
	DECLARE @MaxBidAmount NUMERIC, @ReservePrice NUMERIC, @TopBidder NUMERIC
	SELECT @MaxBidAmount = HighestBid, @TopBidder = TopBidder, @ReservePrice = ReservePrice FROM AE_AuctionCars WHERE Id = @AuctionCarId
   
	IF( @OfferPrice > @ReservePrice ) SET @OfferPrice = @ReservePrice
	
	IF( @OfferPrice > @MaxBidAmount AND @TopBidder = @BidderId )
	BEGIN
		IF EXISTS( SELECT AuctionCarId FROM AE_OfferPrice WHERE AuctionCarId = @AuctionCarId )
			BEGIN
				
					BEGIN
						INSERT INTO AE_Bids (AuctionCarId, BidderId, BidAmount, BidDateTime, IsProxy) VALUES (@AuctionCarId, @BidderId, @OfferPrice, GETDATE(), 0)
						UPDATE AE_OfferPrice SET OfferPrice = @OfferPrice WHERE AuctionCarId = @AuctionCarId					
					END
			END
		ELSE
			BEGIN	
					BEGIN
						INSERT INTO AE_Bids (AuctionCarId, BidderId, BidAmount, BidDateTime, IsProxy) VALUES (@AuctionCarId, @BidderId, @OfferPrice, GETDATE(), 0)
						INSERT INTO AE_OfferPrice(AuctionCarId, OfferPrice, IpAddress, EntryDate) VALUES(@AuctionCarId, @OfferPrice, @IpAddress, GETDATE())										
					END
			END				
	END
				
	SET @Status = ( SELECT Convert(VARCHAR,Ac.HighestBid) +'|'+ Convert(VARCHAR,Ac.TotalBids) +'|'+ Convert(VARCHAR, ISNULL(Ac.ReservePrice,0)) FROM AE_AuctionCars Ac WHERE Id = @AuctionCarId )		
END


