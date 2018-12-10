IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[AE_Bids_SP]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[AE_Bids_SP]
GO

	
CREATE PROCEDURE [dbo].[AE_Bids_SP]  
@AuctionCarId NUMERIC,  
@BidderId NUMERIC,  
@BidAmount NUMERIC,  
@BidDateTime DATETIME,
@IsBidded BIT,

@LatestBidUpdates VARCHAR(100) OUTPUT  
AS  
BEGIN  
 DECLARE @PreviousTopBidder NUMERIC = NULL
 DECLARE @CurrentTopBidder NUMERIC
 DECLARE @RemainingTokens INT
 DECLARE @BidStatus SMALLINT = 0
 DECLARE @MaxBidAmountProxy NUMERIC
 DECLARE @TopBidderProxy NUMERIC
 SET @RemainingTokens = (SELECT AvailableTokens - UsedTokens FROM AE_BidderDetails WHERE Id = @BidderId)
  
 --The following if condition is used so that user can bid
 --only when he has remaining tokens or he is bidding for the car which he has already bidded 
 IF @RemainingTokens > 0
 BEGIN
  
	 DECLARE @MaxBidAmount NUMERIC  
	   
	 DECLARE @FinalClosingTime DATETIME, @BasePrice NUMERIC = 0  
	 SELECT @FinalClosingTime = FinalClosingTime, @BasePrice = BasePrice, @PreviousTopBidder = TopBidder, @MaxBidAmount = HighestBid FROM AE_AuctionCars WHERE Id = @AuctionCarId
	  
	 /* 
		Here first part of if condition checks whether the BidAmount > MaxBidAmount when there is already a record for this car in ae_bids table  
		Here second part of of condion is true when there is no record for this car in table and hence the current BidAmount is max so insert
	*/ 
	 IF (@BidAmount > @MaxBidAmount OR @MaxBidAmount IS NULL) AND (@BidDateTime <= @FinalClosingTime) AND @BidAmount > @BasePrice
		 BEGIN 
			
			SELECT Top 1 @MaxBidAmountProxy = BidAmount, @TopBidderProxy = BidderId FROM AE_ProxyBids WHERE AuctionCarId = @AuctionCarId ORDER BY BidAmount DESC, BidDateTime ASC
			
			/* Insert proxy bid which is less then amount of current bidding */
			IF( @MaxBidAmountProxy > @MaxBidAmount AND @MaxBidAmountProxy < @BidAmount)
			BEGIN
				INSERT INTO AE_Bids (AuctionCarId, BidderId, BidAmount, BidDateTime, IsProxy) VALUES (@AuctionCarId, @TopBidderProxy, @MaxBidAmountProxy, GETDATE(), 1)
				UPDATE AE_AuctionCars SET TotalBids = TotalBids + 1 WHERE Id = @AuctionCarId				
			END
			
			/* Normal Bidding */
			INSERT INTO AE_Bids (AuctionCarId, BidderId, BidAmount, BidDateTime, IsProxy) VALUES (@AuctionCarId, @BidderId, @BidAmount, GETDATE(), 0)


			/******************************************************************************************* 
				If bidder is bidding for first time for this car then we will update IsBidded = 1 
				This will tell that bidder has already bidded for this car.
			*********************************************************************************************/
			IF @IsBidded = 0
			  BEGIN		
				/*
					- If user is eligible to bid on cars and bidding first time, Check car available in 'Watch List' If not enter it.
					- If already available in 'Watch List', Then update IsBidded flag to true.				
				*/
				IF NOT EXISTS( SELECT ID FROM AE_BiddingWishlist WHERE BidderId = @BidderId AND AuctionCarId = @AuctionCarId )
					BEGIN
						INSERT INTO AE_BiddingWishlist( AuctionCarId, BidderId, IsBidded, EntryDate ) VALUES(@AuctionCarId, @BidderId, 1, GETDATE())						
					END
				ELSE
					BEGIN
						UPDATE AE_BiddingWishlist SET IsBidded = 1 WHERE BidderId = @BidderId AND AuctionCarId = @AuctionCarId
					END

				SET @IsBidded = 1
			  END  
			
			/*******************************************************************************************************
				Manage availability of tokens.
			****************************************************************************************************/  
			SET @CurrentTopBidder = ( SELECT TopBidder FROM AE_AuctionCars WHERE Id = @AuctionCarId )
			
			IF( @PreviousTopBidder IS NOT NULL AND @PreviousTopBidder != @CurrentTopBidder )/* If previous bidder overtook by currrent bidder */
				BEGIN
					/* Release the token of the previous bidder because its now loser */
					UPDATE AE_BidderDetails SET UsedTokens = CASE WHEN UsedTokens > 0 THEN UsedTokens - 1 ELSE 0 END WHERE Id = @PreviousTopBidder
					
					/* Consume token of the winner*/
					UPDATE AE_BidderDetails SET UsedTokens = UsedTokens + 1 WHERE Id = @CurrentTopBidder
					
					SET @RemainingTokens = (SELECT AvailableTokens - UsedTokens FROM AE_BidderDetails WHERE Id = @BidderId)
				END
			ELSE IF @PreviousTopBidder IS NULL /* If its a first bid over this car */
				BEGIN
					/* Consume token of the first bidder */
					UPDATE AE_BidderDetails SET UsedTokens = UsedTokens + 1 WHERE Id = @BidderId
					
					SET @RemainingTokens = (SELECT AvailableTokens - UsedTokens FROM AE_BidderDetails WHERE Id = @BidderId)
				END			
			
			SET @BidStatus = 1
		 END
	 ELSE /* False bid request  */
		BEGIN
			SET @BidStatus = 2 /* Bid request is less then current bid */
		END
	
	-- TO RETUEN LATEST BID UPDATES(Concatenated with Pipe) AS A OUTPUT VARCHAR PARAMETER  
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
		 CONVERT(VARCHAR,CASE WHEN DATEDIFF(SECOND,GETDATE(),Ac.InitialClosingTime) < 0 THEN DATEDIFF(SECOND,GETDATE(),Ac.FinalClosingTime)  ELSE DATEDIFF(SECOND,GETDATE(), Ac.InitialClosingTime) END) +'|'+ 
		 CONVERT(VARCHAR,@RemainingTokens) +'|'+ 
		 CONVERT(VARCHAR,@BidStatus)
		FROM AE_AuctionCars Ac  
		WHERE Id = @AuctionCarId  
	)	
 END
END 
