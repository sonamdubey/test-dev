IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[BW_GetDealersLatLong]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[BW_GetDealersLatLong]
GO

	-- Author:		Ashish Kamble
-- Create date: 12 Jan 2015
-- Description:	To get Number of Dealers with their lattitude and longitude for the given version whose prices for the given version are entered.
-- Modified By : Ashwini Todkar on 21 Jan 2015 
-- Retrieved LeadServingDistance from dealers table
-- EXEC BW_GetDealersLatLong 760
-- =============================================
CREATE PROCEDURE [dbo].[BW_GetDealersLatLong] @VersionId INT
	,@AreaId INT
AS
BEGIN
	SET NOCOUNT ON;

	DECLARE @CityId INT

	SELECT @CityId = CityId
	FROM Areas
	WHERE ID = @AreaId

	SELECT DISTINCT D.ID AS DealerId
		,D.Lattitude
		,D.Longitude
		,ISNULL(D.LeadServingDistance,0) LeadServingDistance
	FROM BW_NewBikeDealerShowroomPrices AS BDP WITH (NOLOCK)
	INNER JOIN Dealers D WITH (NOLOCK) ON D.ID = BDP.DealerId
	WHERE D.IsDealerActive = 1
		AND D.ApplicationId = 2
		AND D.IsDealerDeleted = 0
		AND BDP.ItemValue IS NOT NULL
		AND BDP.ItemValue > 0
		AND BDP.BikeVersionId = @VersionId
		AND BDP.CityId = @CityId
	ORDER BY D.ID
END
