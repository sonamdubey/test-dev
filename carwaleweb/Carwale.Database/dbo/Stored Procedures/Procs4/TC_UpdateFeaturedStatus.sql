IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_UpdateFeaturedStatus]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_UpdateFeaturedStatus]
GO

	
-- =============================================
-- Author:		<Author,Nilesh Utture>
-- Create date: <Create Date, 03rd April, 2013>
-- Description:	<Description, Used to change featured status of stock from API>
-- =============================================
CREATE PROCEDURE [dbo].[TC_UpdateFeaturedStatus]
(
@StockId BIGINT,
@BranchId BIGINT,
@UpdateFlag BIT
)

AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	-- getting sellinquiry id from Sellinq table for given stock
	Declare @sell_inqid BIGINT
	SELECT @sell_inqid=Id FROM SellInquiries WITH (NOLOCK) WHERE TC_StockId=@StockId
	
	IF @UpdateFlag = 0 -- Remove stock from featured listing
		BEGIN		
		IF EXISTS(SELECT * FROM DealerFeaturedCars WITH (NOLOCK) WHERE DealerId=@BranchId AND CarId=@sell_inqid)
		BEGIN
			DELETE FROM DealerFeaturedCars WHERE DealerId=@BranchId AND CarId=@sell_inqid
			UPDATE TC_Stock SET IsFeatured=0 WHERE Id=@StockId 
		END
	END
	ELSE IF @UpdateFlag = 1 -- Add stock to featured listing
	BEGIN
	-- Check if the stock is already present in DealerFeaturedCars Table
	IF NOT EXISTS(SELECT * FROM DealerFeaturedCars WITH (NOLOCK) WHERE DealerId=@BranchId AND CarId=@sell_inqid)
	BEGIN
		INSERT INTO DealerFeaturedCars(DealerId,CarId,UpdateOn)
		VALUES(@BranchId,@sell_inqid,GETDATE())
		UPDATE TC_Stock SET IsFeatured=1 WHERE Id=@StockId
	END
	END
	   
END

