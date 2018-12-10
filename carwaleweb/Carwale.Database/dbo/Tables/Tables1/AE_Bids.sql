CREATE TABLE [dbo].[AE_Bids] (
    [Id]           NUMERIC (18) IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [AuctionCarId] NUMERIC (18) NOT NULL,
    [BidderId]     NUMERIC (18) NOT NULL,
    [BidAmount]    NUMERIC (18) NOT NULL,
    [BidDateTime]  DATETIME     NOT NULL,
    [IsProxy]      BIT          NULL,
    CONSTRAINT [PK_AE_Bids] PRIMARY KEY CLUSTERED ([Id] ASC) WITH (FILLFACTOR = 90)
);


GO

CREATE TRIGGER [dbo].[AE_BIDS_INSERT]
ON [dbo].[AE_Bids]
AFTER INSERT
AS
BEGIN

	DECLARE @IsProxy bit
	DECLARE @BidAmountInserted NUMERIC
	DECLARE @AuctionCarId NUMERIC(18, 0)
	DECLARE @BidderId NUMERIC(18, 0)
	DECLARE @TopBid NUMERIC
	DECLARE @TopBidder NUMERIC
	DECLARE @LastBidDateTime DATETIME
	DECLARE @NextTopBid NUMERIC
		
	SELECT @IsProxy = IsProxy, @BidAmountInserted = BidAmount, @AuctionCarId = AuctionCarId, @BidderId = BidderId, @LastBidDateTime = BidDateTime FROM Inserted	
	
	SET @TopBid = @BidAmountInserted
	SET @TopBidder = @BidderId
	SET @NextTopBid = @TopBid + dbo.AE_GetBidIncrementAmount( @TopBid )
	
	--If bid inserted in AE_Bids is not proxy bid then only we want to comapre it with proxy bids for the same car
	--otherwise this trigger will execute in infinite loop as we are inserting in the same AE_Bids table from here also
	IF @IsProxy = '0'
	BEGIN		
		DECLARE @BidsAdded INT
		SET @BidsAdded = 1
	
		--Here we are getting the max proxy bid amount for the car for which bid is inserted in ae_bids table
		DECLARE @MaxBidAmountProxy  NUMERIC
		
		SELECT Top 1 @MaxBidAmountProxy = BidAmount, @BidderId = BidderId FROM AE_ProxyBids WHERE AuctionCarId = @AuctionCarId ORDER BY BidAmount DESC, BidDateTime ASC
		
		IF @MaxBidAmountProxy IS NOT NULL AND @MaxBidAmountProxy >= @TopBid
		BEGIN
			SET @NextTopBid = CASE WHEN @NextTopBid > @MaxBidAmountProxy THEN @MaxBidAmountProxy ELSE @NextTopBid END					
			SET @LastBidDateTime = GETDATE()
			SET @TopBidder = @BidderId
		
			INSERT INTO AE_Bids(AuctionCarId, BidderId, BidAmount, BidDateTime, IsProxy) 
			VALUES(@AuctionCarId, @BidderId, @NextTopBid, @LastBidDateTime, '1')
			
			SET @BidsAdded = 2
			SET @TopBid = @NextTopBid		
		END
		
		-- NOTE : If bid occurs within 2 minutes before final closing time then update the final closing time by 2 minutes
		-- So that other bidder will get chance to bid on this car
		UPDATE AE_AuctionCars
			SET
			TopBidder = @TopBidder,
			HighestBid = @TopBid,
			TotalBids = TotalBids + @BidsAdded,
			LastBidDateTime = @LastBidDateTime,
			FinalClosingTime = (CASE WHEN DATEDIFF(Second, @LastBidDateTime, FinalClosingTime) <= 120 THEN DateAdd(SECOND,120,FinalClosingTime) ELSE FinalClosingTime END)
		WHERE
			Id = @AuctionCarId
	END	
END

