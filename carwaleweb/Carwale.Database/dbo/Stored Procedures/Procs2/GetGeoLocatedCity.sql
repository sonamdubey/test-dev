IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[GetGeoLocatedCity]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[GetGeoLocatedCity]
GO

	

-- =============================================
-- Author:		<Shalini Nair>
-- Create date: <12/05/2015>
-- Description:	<To get the nearest City based on Latitude and Longitude passed>
-- =============================================
CREATE PROCEDURE [dbo].[GetGeoLocatedCity]
	-- Add the parameters for the stored procedure here
	@Latitude NUMERIC
	,@Longitude NUMERIC
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	DECLARE @Latt DECIMAL = ABS(@Latitude)
	DECLARE @Long DECIMAL = ABS(@Longitude)
	DECLARE @lattSecPerKm DECIMAL = 32.57940665
	DECLARE @longSecPerKm DECIMAL = 34.63696611
	DECLARE @ProxDist INT = 300
	DECLARE @TempLatt DECIMAL = NULL
	DECLARE @TempLong DECIMAL = NULL
	DECLARE @ConstLt FLOAT = 0.030694236
	DECLARE @ConstLg FLOAT = 0.028870889 -- // constants 

	SET @TempLatt = @ProxDist * @lattSecPerKm
	SET @TempLong = @ProxDist * @longSecPerKm

	SELECT TOP 1 CT.NAME AS CityName
		,CT.ID AS CityId
		,Sqrt(Power(((ISNULL(@Latt, 0) - CT.Lattitude) * @ConstLt), 2) + Power(((ISNULL(@Long, 0) - CT.Longitude) * @ConstLg), 2)) AS Distance
	FROM Cities CT WITH(NOLOCK)	
	WHERE CT.IsDeleted = 0
		--AND Lattitude BETWEEN ABS((@TempLatt - @Latt)) -- TO LIMIT SEARCH TO A RADIUS OF @ProxDist 
		--	AND (@TempLatt + @Latt)
		--AND Longitude BETWEEN ABS((@TempLong - @Long))
		--	AND (@TempLong + @Long)
	ORDER BY Distance
END

