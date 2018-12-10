IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[AE_BiddingWatchList_SP]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[AE_BiddingWatchList_SP]
GO

	

-- =============================================
-- Author:		Satish Sharma
-- Create date: Nov 12, 2009
-- Description:	SP to add auction cars to watch list
-- =============================================
CREATE PROCEDURE [dbo].[AE_BiddingWatchList_SP]
	-- Add the parameters for the stored procedure here
	@AuctionCarId		NUMERIC,
	@BidderId			NUMERIC,
	@OperationType		VARCHAR(50), -- Add/Remove
	@Status				TINYINT = 0 OUTPUT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	IF @OperationType = 'Add'
		BEGIN
			IF NOT EXISTS(SELECT id FROM AE_BiddingWishlist WHERE AuctionCarId = @AuctionCarId AND BidderId = @BidderId)
				BEGIN 
					INSERT INTO AE_BiddingWishlist(AuctionCarId, BidderId, EntryDate)
					VALUES(@AuctionCarId, @BidderId, GETDATE())
					
					Set @Status = 1
				END
		END
	ELSE IF @OperationType = 'Remove'
		BEGIN
			DELETE FROM AE_BiddingWishlist WHERE AuctionCarId = @AuctionCarId AND BidderId = @BidderId
			Set @Status = 2
		END
		
   
END


