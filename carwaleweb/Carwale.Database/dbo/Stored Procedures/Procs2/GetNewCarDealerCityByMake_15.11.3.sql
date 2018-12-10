IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[GetNewCarDealerCityByMake_15]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[GetNewCarDealerCityByMake_15]
GO

	-- =============================================          
-- Author:  <Supriya K>          
-- Description: <Get the List of New Car Dealer City By Make>          
-- Tables Used : Dealer_NewCar,CarMakes          
-- Create By: Supriya on <4/9/2014>     
-- Modified BY Manish on 05 Nov 2015 Implementation of migration of Dealers_NewCar
-- =============================================          
CREATE PROCEDURE [dbo].[GetNewCarDealerCityByMake_15.11.3] (@MakeId SMALLINT)
AS
BEGIN
	SET NOCOUNT ON;

	/*	SELECT DISTINCT d.CityId AS CityId, c.Name AS CityName,S.ID StateId ,s.Name AS StateName 
	FROM  Dealer_NewCar d WITH (NOLOCK)
	 JOIN Cities c WITH (NOLOCK) ON d.CityId = c.ID
	 JOIN States s WITH (NOLOCK) ON s.ID = c.StateId
	WHERE d.makeid =@MakeId 
	  AND d.IsActive = 1 AND d.IsNewDealer = 1 AND c.IsDeleted = 0
	  ORDER BY S.ID */
	SELECT DISTINCT d.CityId AS CityId
		,c.NAME AS CityName
		,S.ID StateId
		,s.NAME AS StateName
	FROM Dealers d WITH (NOLOCK)
	JOIN DealerLocatorConfiguration AS DLC WITH (NOLOCK) ON DLC.DealerId = D.ID
	JOIN Cities c WITH (NOLOCK) ON d.CityId = c.ID
	JOIN States s WITH (NOLOCK) ON s.ID = c.StateId
	JOIN TC_DealerMakes AS TCM WITH (NOLOCK) ON TCM.DealerId = D.ID
	WHERE TCM.makeid = @MakeId
		AND D.[Status] = 0
		AND D.TC_DealerTypeId = 2
		AND DLC.IsLocatorActive = 1
	ORDER BY S.ID
END

