IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[GetNearestCity]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[GetNearestCity]
GO

	
-- =============================================
-- Created date : 14th Dev, 2015
-- Description   : To Get Nearest City From City id
-- Used By	:	Rohan Sapkal
-- EXEC [dbo].[GetNearestCity] 40,20
-- =============================================
CREATE PROCEDURE [dbo].[GetNearestCity] @CityID INT
	,@Count INT
AS
BEGIN
	DECLARE @City1Latitude NUMERIC
		,@City1Longitude NUMERIC
		,@NearestCity INT = NULL
		,@ConstLt FLOAT = 0.030694236 -- // constant (km per 1 unit)
		,@ConstLg FLOAT = 0.028870889 -- // constant (km per 1 unit)

	SELECT @City1Latitude = c.Lattitude
		,@City1Longitude = c.Longitude
	FROM Cities c WITH (NOLOCK)
	WHERE id = @CityId

	DECLARE @Latt DECIMAL = ABS(@City1Latitude)
		,@Long DECIMAL = ABS(@City1Longitude);

	WITH CTE
	AS (
		SELECT CT.ID AS CityId
			,CT.NAME AS CityName
			,ROW_NUMBER() OVER (
				ORDER BY Sqrt(Power(((ISNULL(@Latt, 0) - CT.Lattitude) * @ConstLt), 2) + Power(((ISNULL(@Long, 0) - CT.Longitude) * @ConstLg), 2))
				) AS RowNum
		FROM Cities CT WITH (NOLOCK)
		WHERE CT.IsDeleted = 0
			AND CT.ID <> @CityId
		)
	SELECT CityId
		,CityName
		,RowNum
	FROM CTE
	WHERE RowNum <= @Count
END
