IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[GetNewCarDealerCityByMake_V15]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[GetNewCarDealerCityByMake_V15]
GO

	 
-- =============================================          
-- Author:  <Supriya K>          
-- Description: <Get the List of New Car Dealer City By Make>          
-- Tables Used : Dealer_NewCar,CarMakes          
-- Create By: Supriya on <4/9/2014>   
-- Modified By: Chetan on <19/11/2015> added parameter Total count And getting data from dealers table 
-- Modified by: Shalini Nair on 30/11/2015 removed order by statename
-- EXEC [GetNewCarDealerCityByMake_V15.11.4.1] 8
-- =============================================    
CREATE PROCEDURE [dbo].[GetNewCarDealerCityByMake_V15.11.6] (@MakeId SMALLINT)
AS
BEGIN
	SET NOCOUNT ON;
      
SELECT DISTINCT d.CityId AS CityId, c.Name AS CityName,S.ID StateId ,s.Name AS StateName,COUNT(d.Id) as TotalCount 	
		FROM DealerLocatorConfiguration AS DNC WITH (NOLOCK)
		INNER JOIN Dealers D WITH (NOLOCK) ON D.ID = DNC.DealerId
		INNER JOIN TC_DealerMakes TDM WITH (NOLOCK) ON TDM.DealerId = D.ID
		JOIN Cities AS C WITH (NOLOCK) ON D.CityId = C.ID
		JOIN States AS S WITH (NOLOCK) ON C.StateId = S.Id
		JOIN CarMakes AS CMA WITH (NOLOCK) ON TDM.MakeId = CMA.ID
		WHERE 
			TDM.MakeId = @MakeId
			AND DNC.IsLocatorActive = 1
			AND C.IsDeleted = 0
			AND S.IsDeleted = 0
			AND CMA.IsDeleted = 0
			AND D.TC_DealerTypeId = 2
			AND D.IsDealerActive = 1
			AND CMA.New = 1 -- Added By Chetan <25/11/2015>
			GROUP BY d.CityId,c.Name,S.ID,s.Name
	  ORDER BY S.Name 

END
