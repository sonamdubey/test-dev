IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[AE_ConfirmBidEnd]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[AE_ConfirmBidEnd]
GO

	
-- =============================================
-- Author:		Satish Sharma
-- Create date: 12/12/2009
-- Description:	Bid end confirmation
-- =============================================
CREATE PROCEDURE [dbo].[AE_ConfirmBidEnd] 
	-- Add the parameters for the stored procedure here
	@AuctionCarId	NUMERIC,
	@BidderId		NUMERIC,
	@LatestBidUpdates VARCHAR(200) OUTPUT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	
	DECLARE @AuctionStatus VARCHAR(10) = ''
	DECLARE @TopBidder	NUMERIC, @FinalClosingTime DATETIME, @TotalBids NUMERIC
	DECLARE @HighestBid NUMERIC, @TimeLeft NUMERIC, @IncrementAmount NUMERIC, @ReservePrice NUMERIC
	
	SELECT	@TopBidder = TopBidder, @FinalClosingTime = FinalClosingTime, @TotalBids = TotalBids, @HighestBid = HighestBid,
			@TimeLeft = ( CASE WHEN DATEDIFF(SECOND,GETDATE(),Ac.InitialClosingTime) < 0 THEN DATEDIFF(SECOND,GETDATE(),Ac.FinalClosingTime)  ELSE DATEDIFF(SECOND,GETDATE(), Ac.InitialClosingTime) END ),
			@IncrementAmount = dbo.AE_GetBidIncrementAmount( Ac.HighestBid ), @ReservePrice = IsNull(ReservePrice, 0)
	FROM AE_AuctionCars Ac
	WHERE Id = @AuctionCarId
	
	If @FinalClosingTime < GETDATE() AND @TotalBids > 0
		BEGIN
			-- Change the status of the car to 'Blocked'
			UPDATE AE_AuctionCars Set StatusId = 2 WHERE Id = @AuctionCarId
			
			-- Release bidder's token if not a winner
			IF @TopBidder <> @BidderId
				BEGIN					
					SET @AuctionStatus = 'L'
				END	
			ELSE
				BEGIN		
					SET @AuctionStatus = 'W'
				END
		END
	ELSE IF @FinalClosingTime < GETDATE() AND @TotalBids = 0
		BEGIN
			UPDATE AE_AuctionCars Set StatusId = 6 WHERE Id = @AuctionCarId
			SET @AuctionStatus = 'O' -- Ended with no bids
		END
	
	/* GET bid details */
	-- TO RETUEN LATEST BID UPDATES AS A OUTPUT VARCHAR PARAMETER  
	SET @LatestBidUpdates = ( CONVERT(VARCHAR,@HighestBid) +'|'+  CONVERT(VARCHAR,@TotalBids) +'|'+ CONVERT(VARCHAR,@IncrementAmount) +'|'+ CONVERT(VARCHAR,@AuctionStatus) +'|'+ CONVERT(VARCHAR,@TimeLeft) +'|'+ CONVERT(VARCHAR,@ReservePrice))
END

