IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[GetNearbyCities]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[GetNearbyCities]
GO

	-- =============================================
-- Author:        <Kirtan Shetty>
-- Create date: <25/8/2014>
-- Description:    <Gets the Nearby cities for a particular city>
-- =============================================
CREATE PROCEDURE [dbo].[GetNearbyCities] 
    @Distance    INT = 50
    ,@CityId    INT
    ,@Count        INT = 0
AS
BEGIN

    DECLARE @Value VARCHAR(200) = ''            -- The output of the SP
    DECLARE @Latitude FLOAT, @Longitude FLOAT    -- The Lat and Long of input City
    DECLARE @Radius FLOAT = 6371.0                -- Radius of Earth
    DECLARE @LatSecPerKm FLOAT = 32.57940665    -- Conversion factor from Seconds to Kilometers for Latitude
    DECLARE @LongSecPerKm FLOAT = 34.63696611    -- Conversion factor from Seconds to Kilometers for Longitude
    DECLARE @fac float = 3600.0                    -- Conversion factor for Seconds to Radians
    DECLARE @MaxLat FLOAT, @MinLat FLOAT, @MaxLong FLOAT, @MinLong FLOAT

    -- Getting the Latitude and Longitude of the input City
    SELECT
        @Latitude = Lattitude,
        @Longitude = Longitude
    FROM 
        Cities WITH(NOLOCK)
    WHERE
        Id = @CityId

    -- Setting the Minimum Latitude and Longitude
    SET @MaxLat = @Latitude + @Distance * @LatSecPerKm
    SET @MinLat = @Latitude - @Distance * @LatSecPerKm
    SET @MaxLong = @Longitude + @Distance * @LongSecPerKm 
    SET @MinLong = @Longitude - @Distance * @LongSecPerKm 
        
    SELECT Id AS NearByCityIds
    FROM 
        Cities WITH(NOLOCK)
    WHERE 
        (Lattitude BETWEEN @MinLat AND @MaxLat)
    AND
        (Longitude BETWEEN @MinLong AND @MaxLong)
    AND
        (Id <> @CityId)

END