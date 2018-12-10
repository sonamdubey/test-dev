IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_FeaturedStatus]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_FeaturedStatus]
GO

	
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[TC_FeaturedStatus]
(
@stock_id BIGINT,
@branch_id BIGINT
)

AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	
	-- getting sellinquiry id from Sellinq table for given stock
	Declare @sell_inqid BIGINT
	SELECT @sell_inqid=Id FROM SellInquiries WHERE TC_StockId=@stock_id
	
	IF EXISTS(SELECT * FROM DealerFeaturedCars WHERE DealerId=@branch_id AND CarId=@sell_inqid)
	BEGIN
		DELETE FROM DealerFeaturedCars WHERE DealerId=@branch_id AND CarId=@sell_inqid
		UPDATE TC_Stock SET IsFeatured=0 WHERE Id=@stock_id 
	END
	ELSE
	BEGIN
		INSERT INTO DealerFeaturedCars(DealerId,CarId,UpdateOn)
		VALUES(@branch_id,@sell_inqid,GETDATE())
		UPDATE TC_Stock SET IsFeatured=1 WHERE Id=@stock_id
	END
	   
END

