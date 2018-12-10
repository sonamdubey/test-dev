IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_RemoveCWstockCN_V16]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_RemoveCWstockCN_V16]
GO

	
-- Modified By: Tejashree Patil On 19 Nov 2012:Added parameter @BranchId and added 'AND CarId=@StockId' condition in DELETE query, 
-- commented @DealerId parameter
-- DECLARE @RetVal TINYINT  execute [TC_RemoveCWstock] 3535,5,@RetVal OUTPUT SELECT @REtVal
-- Modified By: Nilesh Utture on 11th March, 2013 Updated LastUpdatedDate in TC_Stock
-- Modified By:  Manish on 26-07-2013 for maintaining log of the uploaded car
-- Modified By Vivek Gupta on 6th jan 2014 , Added queries to remove youtube link from carvideos and sellinquiriesdetails
-- Created As New By: Vivek Gupta, to remove stock of Car nations while they are removed from carwale
-- Modified  By VIvek GUpta on 26-08-2015, checked if the dealer is mfc dealer or not then only delete stock from while removing from carwale
-- =============================================
CREATE PROCEDURE [dbo].[TC_RemoveCWstockCN_V16.10.1] @StockId BIGINT
	,@BranchId BIGINT
	,@RetVal TINYINT OUTPUT
AS
BEGIN
	-- Modified By: Tejashree Patil On 19 Nov 2012
	IF EXISTS (
			SELECT ID
			FROM TC_Stock WITH (NOLOCK)
			WHERE IsSychronizedCW = 1
				AND BranchId = @BranchId
				AND Id = @StockId
				AND IsActive = 1
				AND StatusId = 1
			)
	BEGIN
		IF EXISTS (
				SELECT DealerId
				FROM TC_MFCDealers WITH (NOLOCK)
				WHERE DealerId = @BranchId
				)
		BEGIN
			UPDATE dbo.TC_Stock
			SET IsSychronizedCW = 0
				,IsFeatured = 0
				,LastUpdatedDate = GETDATE()
				,IsActive = 0
			WHERE Id = @StockId
		END
		ELSE
		BEGIN
			UPDATE dbo.TC_Stock
			SET IsSychronizedCW = 0
				,IsFeatured = 0
				,LastUpdatedDate = GETDATE()
			WHERE Id = @StockId
		END

		DELETE
		FROM DealerFeaturedCars
		WHERE DealerId = @BranchId
			AND CarId = @StockId

		-------------------------Below insert statement add by Manish on 26-07-2013 for maintaining log of the uploaded car-----------
		--INSERT INTO TC_StockUploadedLog (
		--	SellInquiriesId
		--	,DealerId
		--	,IsCarUploaded
		--	,CreatedOn
		--	)
		--SELECT ID
		--	,DealerId
		--	,0
		--	,GETDATE()
		--FROM SellInquiries WITH (NOLOCK)
		--WHERE TC_StockId = @StockId
		--	AND SourceId = 2
		---------------------------------------------------------------------------------------------------------------------------------------------
		-- Modified By Vivek Gupta on 6th jan 2014
		UPDATE TC_CarVideos
		SET StatusId = 2
			,IsSellerInq = 0
			,ModifiedDate = GETDATE()
		WHERE StockId = @StockId
			AND IsActive = 1

		---------------------------------------------------------------------------------------------------------------------------------------------
		SET @RetVal = 1
	END
	ELSE
	BEGIN
		SET @RetVal = 2
	END
END
