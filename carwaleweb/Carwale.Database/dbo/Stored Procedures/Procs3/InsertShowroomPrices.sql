IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[InsertShowroomPrices]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[InsertShowroomPrices]
GO

	-- =============================================
--Modified by Raghupathy 10/23/2013  changed entire logic to insert data into both tables while uploading Bulk PriceQuote
--Modified by Jitendra Solanki  16/10/2015 Mettalic/Non-Metallic paramters
--Modified by Jitendra Solanki  28/04/2015 Checking whether price changed or not
--Modified by Jitendra solanki 18/07/2016 added new tcs prices for bulkprice upload 
--Modified by Shalini Nair on 18/07/2016 to pass setID
--Modified: Shalini Nair,26/07/2016 to pass SetID as varchar
--Modified: Chetan Thambad, 27/07/2016 to update national avg price insert prices
--Modified: Anuj Dhar, 14/10/2016 to use SP CW_InsertShowroomPrices_v16.8.7 instead of CW_InsertShowroomPrices
-- =============================================
CREATE PROCEDURE [dbo].[InsertShowroomPrices] @CarVersionId NUMERIC
	,-- Car Version Id
	@MumbaiPrice NUMERIC
	,-- Mumbai Price
	@MumbaiInsurance NUMERIC
	,-- Mumbai Insurance
	@MumbaiRTO NUMERIC
	,-- Mumbai RTO
	@CityId NUMERIC
	,-- 
	@SetID VARCHAR(50)
	,@isMetallic BIT
	,@LastUpdatedBy INT
	,@LastUpdated DATETIME -- Entry Date
AS
BEGIN
	DECLARE @ExShow_ItemId INT = 2
		,@RTO_ItemId INT = 3
		,@Insurance_ItemId INT = 5
		,@Exshow_CategoryId INT = 3
		,@RTO_CategoryId INT = 4
		,@Insurance_CategoryId INT = 5
	DECLARE @IsPriceChange BIT = 0

	SET @IsPriceChange = ISNULL((
				SELECT TOP 1 1
				FROM CW_NewCarShowroomPrices WITH (NOLOCK)
				WHERE CarVersionId = @CarVersionId
					AND CityId = @CityId
					AND isMetallic = @isMetallic
					AND (
						(
							PQ_CategoryItem = @ExShow_ItemId
							AND PQ_CategoryItemValue != @MumbaiPrice
							)
						OR (
							PQ_CategoryItem = @Insurance_ItemId
							AND PQ_CategoryItemValue != @MumbaiInsurance
							)
						OR (
							PQ_CategoryItem = @RTO_ItemId
							AND PQ_CategoryItemValue != @MumbaiRTO
							)
						)
				), 0)

	IF @MumbaiPrice > 0
	BEGIN
		EXEC [dbo].[CW_InsertShowroomPrices_v16.8.7] @CarVersionId
			,@CityId
			,@LastUpdated
			,@ExShow_ItemId
			,@MumbaiPrice
			,@Exshow_CategoryId
			,@isMetallic
			,@LastUpdatedBy
			,@SetID

		EXEC [dbo].[CW_InsertShowroomPrices_v16.8.7] @CarVersionId
			,@CityId
			,@LastUpdated
			,@RTO_ItemId
			,@MumbaiRTO
			,@RTO_CategoryId
			,@isMetallic
			,@LastUpdatedBy
			,@SetID

		EXEC [dbo].[CW_InsertShowroomPrices_v16.8.7] @CarVersionId
			,@CityId
			,@LastUpdated
			,@Insurance_ItemId
			,@MumbaiInsurance
			,@Insurance_CategoryId
			,@isMetallic
			,@LastUpdatedBy
			,@SetID

		IF (@MumbaiPrice >= 1000000) -- check for 1000000 above price car 
		BEGIN
			DECLARE @TCS_price NUMERIC = ROUND(MIN(@MumbaiPrice * 0.01), 0) -- calculate 1% tcs charges and insert into table 

			EXEC [dbo].[CW_InsertShowroomPrices_v16.8.7] @CarVersionId
				,@CityId
				,@LastUpdated
				,77
				,@TCS_price
				,6
				,@isMetallic
				,@LastUpdatedBy
				,@SetID
		END
	END

	EXEC [dbo].[Con_SaveNewCarNationalPrices] @CarVersionId
		,@LastUpdatedBy
		,@LastUpdated
END

