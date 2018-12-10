IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[PQCheckStateCityZone]') 
    AND xtype IN (N'FN', N'IF', N'TF')
)
    DROP FUNCTION [dbo].[PQCheckStateCityZone]
GO

	

/**********************************************************************************Function***********************************************************************************/
-- =============================================
-- Author:		Vikas J				
-- Create date: 15/04/2016
-- Description:	Checks that for a given city zone input if a match is found from state,city and zones column
-- =============================================
CREATE FUNCTION [dbo].[PQCheckStateCityZone] (
	@TableStateId INT
	,@TableCityId INT
	,@TableZoneId INT
	,@InputCityId INT
	,@InputZoneId INT
	)
RETURNS BIT
AS
-- Returns the stock level for the product.
BEGIN
	DECLARE @ret BIT = 0;

	IF (@InputCityId = 0)
		RETURN 0;

	IF (@TableStateId = - 1) --pan india
		SELECT @ret = 1;
	ELSE IF (
			@TableCityId = - 1
			AND @TableStateId = (
				SELECT StateId
				FROM Cities WITH (NOLOCK)
				WHERE Id = @InputCityId
				)
			) --pan state
		SELECT @ret = 1;
	ELSE IF (
			@TableCityId = @InputCityId
			AND @InputCityId NOT IN (1, 10, - 1)
			) --non zone cities
		SELECT @ret = 1;
	ELSE IF (
			ISNULL(@TableZoneId, 0) = ISNULL(@InputZoneId, 0)
			AND @InputCityId IN (1, 10)
			) --zones
		SELECT @ret = 1;

	RETURN @ret;
END;



