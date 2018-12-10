IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[GetValuationData]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[GetValuationData]
GO

	CREATE PROCEDURE [GetValuationData] 

	@CustomerId		NUMERIC OUTPUT,
	@ValuationId		NUMERIC,
	@MumbaiValue		NUMERIC OUTPUT,
	@DelhiValue		NUMERIC OUTPUT,
	@ValueExcellent	NUMERIC OUTPUT,
	@ValueGood		NUMERIC OUTPUT,
	@ValueFair		NUMERIC OUTPUT,
	@ValuePoor		NUMERIC OUTPUT,
	@ValueExcellentDealer	NUMERIC OUTPUT,
	@ValueGoodDealer	NUMERIC OUTPUT,
	@ValueFairDealer	NUMERIC OUTPUT,
	@ValuePoorDealer	NUMERIC OUTPUT,
	@isActive		BIT OUTPUT
AS

	DECLARE @CarVersionId  AS	NUMERIC,
	@CarYear		AS		DATETIME,
	@Deviation		AS NUMERIC
BEGIN

	SELECT @CarVersionId=CarVersionId, @CarYear=CarYear FROM CarValuations WHERE ID=@ValuationId
	
 	SELECT @MumbaiValue=CarValue FROM CarValues WHERE CarVersionId=@CarVersionId  AND CarYear=YEAR(@CarYear)  AND GuideId=1
	-- Calculate Deviation from the standard guide
	SELECT @Deviation=Deviation FROM CarValuesCityDeviation WHERE CityId=1
	SET @MumbaiValue = @MumbaiValue + @MumbaiValue * @Deviation / 100
	SELECT @DelhiValue=CarValue FROM CarValues WHERE CarVersionId=@CarVersionId  AND CarYear=YEAR(@CarYear)  AND GuideId=10
	-- Calculate Deviation from the standard guide
	SELECT @Deviation=Deviation FROM CarValuesCityDeviation WHERE CityId=10
	SET @DelhiValue = @DelhiValue + @DelhiValue * @Deviation / 100

	SELECT 
		@CustomerId		= CustomerId,
		@ValueExcellent	=ISNULL(ValueExcellent,0),
		@ValueGood		=ISNULL(ValueGood,0),
		@ValueFair		=ISNULL(ValueFair,0),
		@ValuePoor		=ISNULL(ValuePoor,0),
		@ValueExcellentDealer	=ISNULL(ValueExcellentDealer,0),
		@ValueGoodDealer	=ISNULL(ValueGoodDealer,0),
		@ValueFairDealer	=ISNULL(ValueFairDealer,0),
		@ValuePoorDealer	=ISNULL(ValuePoorDealer,0),	
		@IsActive		=ISNULL(isActive,0)	
	FROM CarValuations WHERE Id=@ValuationId

END