IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[AE_ProxyBid_SP]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[AE_ProxyBid_SP]
GO

	
-- =============================================
-- Author:		Satish Sharma
-- Create date: Nov 10, 2009
-- Description:	Function to set bidder proxy, System will bid on the behalf of the bidder untill 
				-- reach max bid amount. Bidder can't quit proxy bidding untill reach the max amount.				
-- =============================================
CREATE PROCEDURE [dbo].[AE_ProxyBid_SP]
	-- Add the parameters for the stored procedure here
	@BidderId			NUMERIC,
	@AuctionCarId		NUMERIC,
	@MaxProxyAmount		NUMERIC,
	@IsBidded			BIT,	
	@LatestBidUpdates VARCHAR(100) OUTPUT
AS

DECLARE @ProxyBidStatus SMALLINT = 0
DECLARE @OldProxyAmountBidder	NUMERIC = 0
DECLARE @PreviousMaxProxy  NUMERIC = NULL
DECLARE @RemainingTokens INT
DECLARE @MaxBidAmount NUMERIC  
DECLARE @FinalClosingTime DATETIME
DECLARE @NextBidAmount NUMERIC = 0
DECLARE @BasePrice NUMERIC = 0
DECLARE @Increment NUMERIC
DECLARE @TopBidder NUMERIC -- bidder of top proxy
DECLARE @PreviousTopBidder	NUMERIC = NULL
DECLARE @CurrentTopBidder	NUMERIC

BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	SET @RemainingTokens = (SELECT AvailableTokens - UsedTokens FROM AE_BidderDetails WHERE Id = @BidderId)
	SET @OldProxyAmountBidder = ( SELECT  ISNULL( Max(BidAmount), 0) FROM AE_ProxyBids WHERE AuctionCarId = @AuctionCarId AND BidderId = @BidderId )
	
	IF  @RemainingTokens > 0 AND @MaxProxyAmount > @OldProxyAmountBidder
		BEGIN
			 -- Get the last bid Details
			 SELECT @FinalClosingTime = FinalClosingTime, @BasePrice = BasePrice, @MaxBidAmount = HighestBid, @PreviousTopBidder = TopBidder
			 FROM AE_AuctionCars WHERE Id = @AuctionCarId
			 
			 -- Get the increment amount over current bidding amount 
			 SET @Increment = dbo.AE_GetBidIncrementAmount( @MaxBidAmount )
			 
			 -- Next bid amount will be current bidding + upcoming increment
			 SET @NextBidAmount = @MaxBidAmount + @Increment
			
			 -- 1. Proxy amount user want to set will be always greter then current amount plus incremant amount
			 -- 2. If its a first bid on the car then bid amount will be base price + increment amount
			 -- 3. User can bid till the final closing time. As final closing time exceeds, Bidding will close		
			 IF (@MaxProxyAmount >=	@NextBidAmount OR @MaxBidAmount IS NULL) AND ( GETDATE() <= @FinalClosingTime )  AND @NextBidAmount > @BasePrice
			 BEGIN						
				SELECT Top 1 @PreviousMaxProxy = BidAmount, @TopBidder = BidderId  FROM AE_ProxyBids WHERE AuctionCarId = @AuctionCarId ORDER BY BidAmount DESC, BidDateTime ASC
				
				IF @PreviousMaxProxy < @MaxBidAmount
					BEGIN
						SET @PreviousMaxProxy = @MaxBidAmount
					END
				IF @TopBidder = @BidderId
					BEGIN
						SET @TopBidder = @BidderId
					END
				
				IF( @PreviousMaxProxy IS NOT NUll ) -- IF Its not a first bid '@PreviousMaxProxy' will be null
					BEGIN
						IF( @PreviousMaxProxy > @MaxProxyAmount ) -- Current proxy is less then previously set proxy
							BEGIN
								-- In case current bidder trying to set proxy which is less then priviously set proxy by other bidder.
								-- in this case current bidder going to lose this bid for sure and two bids will be considered.
								IF( @TopBidder <> @BidderId AND @PreviousMaxProxy > @MaxBidAmount)
								BEGIN
									INSERT INTO AE_Bids (AuctionCarId, BidderId, BidAmount, BidDateTime, IsProxy) VALUES (@AuctionCarId, @BidderId, @MaxProxyAmount, GETDATE(), 1)		
									UPDATE AE_AuctionCars SET TopBidder = @BidderId, HighestBid = @MaxProxyAmount, TotalBids = TotalBids + 1, LastBidDateTime = GETDATE() WHERE Id = @AuctionCarId
								END
								SET @NextBidAmount = CASE WHEN (@MaxProxyAmount + @Increment) <= @PreviousMaxProxy THEN (@MaxProxyAmount + @Increment) ELSE @PreviousMaxProxy END
								SET @CurrentTopBidder = @TopBidder
							END
						ELSE IF( @PreviousMaxProxy < @MaxProxyAmount ) -- Current proxy is more then previously set proxy
							BEGIN
								IF( @TopBidder <> @BidderId AND @PreviousMaxProxy > @MaxBidAmount )
								BEGIN
									INSERT INTO AE_Bids (AuctionCarId, BidderId, BidAmount, BidDateTime, IsProxy) VALUES (@AuctionCarId, @TopBidder, @PreviousMaxProxy, GETDATE(), 1)		
									UPDATE AE_AuctionCars SET TopBidder = @TopBidder, HighestBid = @PreviousMaxProxy, TotalBids = TotalBids + 1, LastBidDateTime = GETDATE() WHERE Id = @AuctionCarId
								END
								
								SET @NextBidAmount = CASE WHEN(@PreviousMaxProxy + @Increment) <= @MaxProxyAmount THEN @PreviousMaxProxy + @Increment ELSE @MaxProxyAmount END
								SET @CurrentTopBidder = @BidderId
							END
						ELSE IF( @PreviousMaxProxy = @MaxProxyAmount ) -- if both proxy amount is same then bidder who set the proxy first will be winner
							BEGIN
								IF( @TopBidder <> @BidderId AND @PreviousMaxProxy > @MaxBidAmount )
								BEGIN
								INSERT INTO AE_Bids (AuctionCarId, BidderId, BidAmount, BidDateTime, IsProxy) VALUES (@AuctionCarId, @BidderId, @MaxProxyAmount, GETDATE(), 1)		
								UPDATE AE_AuctionCars SET TopBidder = @BidderId, HighestBid = @MaxProxyAmount, TotalBids = TotalBids + 1, LastBidDateTime = GETDATE() WHERE Id = @AuctionCarId
								END
								SET @NextBidAmount = @PreviousMaxProxy
								SET @CurrentTopBidder = @TopBidder
							END
					END
				ELSE -- Its a first bid
					BEGIN
						SET @CurrentTopBidder = @BidderId
					END
				
				INSERT INTO AE_ProxyBids(AuctionCarId,BidderId,BidAmount,BidDateTime) Values(@AuctionCarId,@BidderId,@MaxProxyAmount, GETDATE())
				INSERT INTO AE_Bids (AuctionCarId, BidderId, BidAmount, BidDateTime, IsProxy) VALUES (@AuctionCarId, @CurrentTopBidder, @NextBidAmount, GETDATE(), 1)
				
				UPDATE AE_AuctionCars 
				SET TopBidder = @CurrentTopBidder, HighestBid = @NextBidAmount, 
					TotalBids = TotalBids + 1, LastBidDateTime = GETDATE(),
					FinalClosingTime = (CASE WHEN DATEDIFF(Second, GETDATE(), FinalClosingTime) <= 120 THEN DateAdd(SECOND,120,FinalClosingTime) ELSE FinalClosingTime END)
				WHERE Id = @AuctionCarId
				
				-- MANAGE TOKENS
				-- If previous bidder overtook by currrent bidder
				IF( @PreviousTopBidder IS NOT NULL AND @PreviousTopBidder != @CurrentTopBidder ) -- if its not first bid verr the car
					BEGIN
						-- Release the token of the previous bidder because its now loser
						UPDATE AE_BidderDetails SET UsedTokens = CASE WHEN UsedTokens > 0 THEN UsedTokens - 1 ELSE 0 END WHERE Id = @PreviousTopBidder
						
						-- Consume token of the winner
						UPDATE AE_BidderDetails SET UsedTokens = UsedTokens + 1 WHERE Id = @CurrentTopBidder
						
						SET @RemainingTokens = (SELECT AvailableTokens - UsedTokens FROM AE_BidderDetails WHERE Id = @BidderId)
					END
				ELSE IF(@PreviousTopBidder IS NULL)-- If its a first bid
					BEGIN
						-- Consume token of the winner
						UPDATE AE_BidderDetails SET UsedTokens = UsedTokens + 1 WHERE Id = @BidderId
						
						SET @RemainingTokens = (SELECT AvailableTokens - UsedTokens FROM AE_BidderDetails WHERE Id = @BidderId)
					END
				
				-- If bidder is bidding for first time for this car then we will update IsBidded = 1 
				-- This will tell that bidder has already bidded for this car
				-- Since he is bidding for first time to this car we are incrementing UsedTokens value for bidder by 1
				IF @IsBidded = 0
				  BEGIN				
					-- If user bidding first time, Check car available in 'Watch List' If not enter it
					-- If already available in 'Watch List', Then update BidderId flag to true so that we can manage 
					-- token count accordingly
					IF NOT EXISTS( SELECT ID FROM AE_BiddingWishlist WHERE BidderId = @BidderId AND AuctionCarId = @AuctionCarId )
						BEGIN
							INSERT INTO AE_BiddingWishlist( AuctionCarId, BidderId, IsBidded, EntryDate )
							VALUES(@AuctionCarId, @BidderId, 1, GETDATE())
						END
					ELSE
						BEGIN
							UPDATE AE_BiddingWishlist
							SET IsBidded = 1
							WHERE BidderId = @BidderId AND AuctionCarId = @AuctionCarId
						END
				
					SET @IsBidded = 1												  
				  END	
				
				SET @ProxyBidStatus = 4					
			END	
			ELSE
				BEGIN
					SET @ProxyBidStatus = 5
				END			
		END
	ELSE
		BEGIN
			SET @ProxyBidStatus = 3
		END
	
	-- TO RETUEN LATEST BID UPDATES AS A OUTPUT VARCHAR PARAMETER  
	SET @LatestBidUpdates =  
	(   
		SELECT   
		 Convert(VARCHAR,Ac.TopBidder) +'|'+  
		 Convert(VARCHAR,Ac.HighestBid) +'|'+   
		 Convert(VARCHAR,Ac.TotalBids) +'|'+   
		 Convert(VARCHAR,Ac.LastBidDateTime, 107) +'|'+ 
		 Convert(VARCHAR,Ac.LastBidDateTime, 108) +'|'+ 
		 Convert(VARCHAR,dbo.AE_GetBidIncrementAmount( Ac.HighestBid ) ) +'|'+   
		 CONVERT(VARCHAR, @IsBidded) +'|'+
		 CONVERT(VARCHAR, CASE WHEN DATEDIFF(SECOND,GETDATE(),Ac.InitialClosingTime) < 0 THEN DATEDIFF(SECOND,GETDATE(),Ac.FinalClosingTime) ELSE DATEDIFF(SECOND,GETDATE(), Ac.InitialClosingTime) END) +'|'+  
		 CONVERT(VARCHAR,@RemainingTokens) +'|'+
		 CONVERT(VARCHAR,@ProxyBidStatus)
		FROM AE_AuctionCars Ac  
		WHERE Id = @AuctionCarId  
	)	
END
