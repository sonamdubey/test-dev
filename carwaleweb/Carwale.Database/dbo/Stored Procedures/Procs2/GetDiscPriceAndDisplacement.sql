IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[GetDiscPriceAndDisplacement]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[GetDiscPriceAndDisplacement]
GO

	-- =============================================
-- Author:		Rohan
-- Create date: 26/07/2016
-- Modified by: 
-- =============================================
CREATE PROCEDURE [dbo].[GetDiscPriceAndDisplacement]
	 @CarVersionId INT
	,@CityId INT
	,@Year INT = NULL
	,@Discount INT OUTPUT
	,@Displacement INT OUTPUT
	,@Price INT OUTPUT
AS
BEGIN
	SELECT @Displacement=Displacement
	FROM NewCarSpecifications WITH(NOLOCK) WHERE carVersionId=@CarVersionId

	SELECT @Discount=IsNull(Discount, 0) 
	FROM Con_InsuranceDiscount CD WITH(NOLOCK) , CarVersions CV WITH(NOLOCK)
	WHERE CV.Id = @CarVersionId AND CD.ModelID = CV.CarModelId AND CD.CityId = @CityId

	SELECT top 1 @Price=Price from NewCarShowroomPrices WITH(NOLOCK) where CarVersionId=@CarVersionId and CityId=@CityId
	
	IF @Price is null
	BEGIN
	select top 1 @Price=CarValue from CarValues WITH(NOLOCK) where CarVersionId=@CarVersionId and (CarYear<= @Year OR @Year IS NULL) order by CarYear desc
	END

	IF @Price is null
	BEGIN
	select top 1 @Price=CarValue from CarValues WITH(NOLOCK) where CarVersionId=@CarVersionId order by CarYear desc
	END
	
END

