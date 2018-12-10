IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[BW_GetBikeAvailabilitiy]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[BW_GetBikeAvailabilitiy]
GO

	-- =============================================
-- Author:		Suresh Prajapati
-- Create date: 11th Nov, 2014
-- Description:	To get Added Bike Availabilities By Specified Dealer
-- =============================================

CREATE PROCEDURE [dbo].[BW_GetBikeAvailabilitiy] 
 @DealerId INT
AS
BEGIN
	SET NOCOUNT ON;
	
	SELECT BA.ID AS Id, BM.Name AS Make, BMO.Name AS Model, BV.Name AS Version, BA.NumOfDays AS AvailableLimit 
	FROM BW_BikeAvailability AS BA WITH(NOLOCK)
	INNER JOIN BikeVersions AS BV WITH(NOLOCK) ON BV.ID=BA.BikeVersionId
	INNER JOIN BikeModels AS BMO WITH(NOLOCK) ON BMO.ID=BV.BikeModelId
	INNER JOIN BikeMakes  AS BM WITH(NOLOCK) ON BM.ID=BMO.BikeMakeId
	WHERE BA.IsActive=1 AND BA.DealerId=@DealerId
	ORDER BY BA.NumOfDays
END


