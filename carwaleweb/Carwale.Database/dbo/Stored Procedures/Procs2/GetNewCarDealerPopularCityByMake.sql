IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[GetNewCarDealerPopularCityByMake]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[GetNewCarDealerPopularCityByMake]
GO

	-- =============================================          
-- Author:  <Supriya K>          
-- Description: <Get the List of New Car Dealer Popular Cities By Make>          
-- Tables Used : Dealer_NewCar,CarMakes          
-- Create By: Supriya on <8/9/2014>     
-- =============================================          
CREATE PROCEDURE [dbo].[GetNewCarDealerPopularCityByMake] (@MakeId SMALLINT)
AS
BEGIN
	SET NOCOUNT ON;

	SELECT DISTINCT d.CityId AS CityId, c.Name AS CityName,c.cityImageUrl AS CityImgUrl 
	FROM  Dealer_NewCar d WITH (NOLOCK)
	LEFT JOIN Cities c WITH (NOLOCK) ON d.CityId = c.ID
	WHERE d.makeid =@MakeId AND d.IsActive = 1 AND c.IsDeleted = 0 AND c.IsPopular = 1 AND d.IsNewDealer=1
	
END

