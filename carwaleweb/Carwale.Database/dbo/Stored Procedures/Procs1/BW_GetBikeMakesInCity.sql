IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[BW_GetBikeMakesInCity]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[BW_GetBikeMakesInCity]
GO

	-- =============================================
-- Author:		Ashish G. Kamble
-- Create date: 10 May 2015
-- Description:	Proc to get the list of bike makes in the city where dealers are available
-- =============================================
CREATE PROCEDURE [dbo].[BW_GetBikeMakesInCity]
	-- Add the parameters for the stored procedure here
	@CityId INT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT DISTINCT M.ID AS MakeId, M.Name AS Make
	FROM BW_NewBikeDealerShowroomPrices SP WITH (NOLOCK)
	INNER JOIN BikeVersions BV WITH (NOLOCK) ON SP.BikeVersionId = BV.ID
	INNER JOIN BikeModels BMO WITH (NOLOCK) ON BMO.ID = BV.BikeModelId
	INNER JOIN BikeMakes M WITH (NOLOCK) ON M.ID = BMO.BikeMakeId	
	JOIN Dealers D ON D.ID = SP.DealerId
	WHERE SP.CityId = @CityId
		AND BV.New = 1
		AND BV.IsDeleted = 0
		AND D.IsDealerActive = 1
	ORDER BY M.Name ASC
END



