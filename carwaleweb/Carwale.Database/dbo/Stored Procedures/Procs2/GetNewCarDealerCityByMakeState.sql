IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[GetNewCarDealerCityByMakeState]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[GetNewCarDealerCityByMakeState]
GO

	-- =============================================
-- Author:		AnchalGupta
-- Create date: 29/12/2015
-- Description:	Get cities by make and state
-- =============================================
CREATE PROCEDURE  [dbo].[GetNewCarDealerCityByMakeState] 
    @MakeId SMALLINT
	,@StateId int
	
AS
BEGIN
	Select DISTINCT d.CityId AS CityId, c.Name AS CityName	
		FROM DealerLocatorConfiguration AS DNC WITH (NOLOCK)
		INNER JOIN Dealers D WITH (NOLOCK) ON D.ID = DNC.DealerId
		INNER JOIN TC_DealerMakes TDM WITH (NOLOCK) ON TDM.DealerId = D.ID
		JOIN Cities AS C WITH (NOLOCK) ON D.CityId = C.ID
		JOIN States AS S WITH (NOLOCK) ON C.StateId = S.Id
		JOIN CarMakes AS CMA WITH (NOLOCK) ON TDM.MakeId = CMA.ID
		WHERE 
			TDM.MakeId = @MakeId
			AND S.Id = @StateId
			AND DNC.IsLocatorActive = 1
			AND C.IsDeleted = 0
			AND S.IsDeleted = 0
			AND CMA.IsDeleted = 0
			AND D.TC_DealerTypeId = 2
			AND D.IsDealerActive = 1
			AND CMA.New = 1 -- Added By Chetan <25/11/2015>
			GROUP BY d.CityId,c.Name
END
