IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[CheckShowroomPricesHash]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[CheckShowroomPricesHash]
GO

	-- Created By : jitendra on 29/04/2016
-- This Store procedure will checkwhether price changes or not 
-- and return setid from pricelog_itemvalue
CREATE PROCEDURE [dbo].[CheckShowroomPricesHash]

	@CarVersionId			INT,	-- Car Version Id
	@CityId					INT,	
	@IsMetallic             BIT,
	@CurrentPriceHash 		VARCHAR(500),
	@SetID					INT OUTPUT
AS
DECLARE
	@Price varchar(500),
	@IsMatchHash BIT
BEGIN

	SELECT @Price = ISNULL(@Price + ',', '') + LTRIM(RTRIM(Str(cwn.PQ_CategoryItemValue, 10, 0)))
	FROM CW_NewCarShowroomPrices as cwn WITH(NOLOCK)
	WHERE CarVersionId = @CarVersionId 
	and CityId = @CityId
	and isMetallic = @IsMetallic
	ORDER BY PQ_CategoryItem

	SET @IsMatchHash = CASE
						  WHEN @Price = @CurrentPriceHash THEN 1
						  ELSE 0
						END
	SET @SetID = 0
	IF @IsMatchHash = 0
	BEGIN
		SET @SetID = (SELECT ISNULL((MAX(SetID) + 1),1) FROM PriceLog_ItemValues WITH(NOLOCK))
	END
END
