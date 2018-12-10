IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[GetNewCarDealerPopularCityByMake_v15]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[GetNewCarDealerPopularCityByMake_v15]
GO

	-- =============================================
-- Author:  <Supriya K>          
-- Description: <Get the List of New Car Dealer Popular Cities By Make>  
-- Modified by:		Anchal gupta
-- Create date: 05/11/2014
-- Description:	<IMPLEMENTATION OF MIGRATION OF DEALER_NEWCAR>
-- =============================================
CREATE PROCEDURE [dbo].[GetNewCarDealerPopularCityByMake_v15.11.3] @MakeId SMALLINT
AS
BEGIN
	SET NOCOUNT ON;

	SELECT DISTINCT D.CityId AS CityId
		,C.NAME AS CityName
		,C.cityImageUrl AS CityImgUrl
	FROM Dealers D WITH (NOLOCK)
	INNER JOIN DealerLocatorConfiguration DLC WITH (NOLOCK) ON D.ID = DLC.DealerId
	JOIN TC_DealerMakes TCM WITH (NOLOCK) ON D.ID = TCM.DealerId
	LEFT JOIN Cities C WITH (NOLOCK) ON D.CityId = C.ID
	WHERE TCM.makeid = @MakeId
		AND D.[Status] = 0
		AND C.IsDeleted = 0
		AND C.IsPopular = 1
		AND D.TC_DealerTypeId = 2
		AND DLC.IsLocatorActive = 1
END

