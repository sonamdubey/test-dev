IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[GetNewCarDealerCityByMake]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[GetNewCarDealerCityByMake]
GO

	-- =============================================          
-- Author:  <Supriya K>          
-- Description: <Get the List of New Car Dealer City By Make>          
-- Tables Used : Dealer_NewCar,CarMakes          
-- Create By: Supriya on <4/9/2014>     
-- =============================================          
CREATE PROCEDURE [dbo].[GetNewCarDealerCityByMake] (@MakeId SMALLINT)
AS
BEGIN
	SET NOCOUNT ON;


	SELECT DISTINCT d.CityId AS CityId, c.Name AS CityName,S.ID StateId ,s.Name AS StateName 
	FROM  Dealer_NewCar d WITH (NOLOCK)
	 JOIN Cities c WITH (NOLOCK) ON d.CityId = c.ID
	 JOIN States s WITH (NOLOCK) ON s.ID = c.StateId
	WHERE d.makeid =@MakeId 
	  AND d.IsActive = 1 AND d.IsNewDealer = 1 AND c.IsDeleted = 0
	  ORDER BY S.ID 
END

