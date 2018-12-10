IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[GetNewCarDealerCountByMake_v15]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[GetNewCarDealerCountByMake_v15]
GO

	-- =============================================          
-- Author:  <Vinayak>          
-- Description: <Get the count of new car dealers>
-- Modified: Vicky, 16/11/2015, Removed Dealer_NewCar dependency
-- Modified: Shalini Nair on 27/11/2015 ordered by carname 
-- EXEC [GetNewCarDealerCountByMake_v15.11.3]
-- =============================================
CREATE PROCEDURE [dbo].[GetNewCarDealerCountByMake_v15.11.5.1]
AS
BEGIN
	SELECT CM.ID AS MakeId
		,CM.NAME AS MakeName
		,COUNT(D.ID) AS DealerCount
	FROM Dealers D WITH (NOLOCK)
	INNER JOIN DealerLocatorConfiguration DLC WITH (NOLOCK) ON DLC.DealerId = D.ID
		AND D.TC_DealerTypeId IN (2, 3)
		AND D.[Status] = 0
		AND DLC.IsLocatorActive = 1
	INNER JOIN TC_DealerMakes TDM WITH (NOLOCK) ON D.ID = TDM.DealerId
	INNER JOIN CarMakes AS CM WITH (NOLOCK) ON CM.ID = TDM.MakeId
		AND CM.IsDeleted = 0 AND CM.New =1 
	GROUP BY CM.NAME
		,CM.ID
	ORDER BY CM.Name
END
