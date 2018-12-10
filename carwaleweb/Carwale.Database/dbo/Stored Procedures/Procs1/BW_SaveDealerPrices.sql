IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[BW_SaveDealerPrices]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[BW_SaveDealerPrices]
GO

	-- =============================================
-- Author:		Ashwini Todkar
-- Create date: 5 Nov 2014
-- Description:	Proc to save version dealers prices and also to save prices
-- =============================================
CREATE PROCEDURE [dbo].[BW_SaveDealerPrices] @DealerId INT
	,@CityId INT
	,@BikeVersionId INT
	,@ItemId SMALLINT
	,@ItemValue INT
AS
BEGIN
	DECLARE @RecordExist INT
		,@LastUpdatedDate DATETIME
		,@CatItemId SMALLINT
		,@CatItemValue INT

	SELECT @CatItemValue = SP.ItemValue
		,@LastUpdatedDate = SP.EntryDate
	FROM BW_NewBikeDealerShowroomPrices SP WITH (NOLOCK)
	WHERE SP.BikeVersionId = @BikeVersionId
		AND SP.CityId = @CityId
		AND SP.DealerId = @DealerId
		AND SP.ItemId = @ItemId

	SET @RecordExist = @@ROWCOUNT

	IF @RecordExist <= 0
	BEGIN
		INSERT INTO BW_NewBikeDealerShowroomPrices (
			DealerId
			,BikeVersionId
			,CityId
			,ItemId
			,ItemValue
			,EntryDate
			)
		VALUES (
			@DealerId
			,@BikeVersionId
			,@CityId
			,@ItemId
			,@ItemValue
			,GETDATE()
			)
	END
	ELSE
	BEGIN
		INSERT INTO BW_DealerPricesLog (
			DealerId
			,BikeVersionId
			,CityId
			,ItemId
			,ItemValue
			,LastUpdatedOn
			)
		VALUES (
			@DealerId
			,@BikeVersionId
			,@CityId
			,@ItemId
			,@CatItemValue
			,@LastUpdatedDate
			)

		UPDATE BW_NewBikeDealerShowroomPrices
		SET ItemValue = @ItemValue
			,EntryDate = GETDATE()
		WHERE BikeVersionId = @BikeVersionId
			AND DealerId = @DealerId
			AND CityId = @CityId
			AND ItemId = @ItemId
	END
END

