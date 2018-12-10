IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[GetNewCarDealerStateByMake]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[GetNewCarDealerStateByMake]
GO

	-- =============================================
-- Author:		AnchalGupta
-- Create date: 29/12/2015
-- Description:	Get states by make
-- =============================================
CREATE PROCEDURE [dbo].[GetNewCarDealerStateByMake] 
        @MakeId SMALLINT
	
AS
BEGIN
	SELECT DISTINCT S.ID StateId ,s.Name AS StateName	
		FROM DealerLocatorConfiguration AS DNC WITH (NOLOCK)
		INNER JOIN Dealers D WITH (NOLOCK) ON D.ID = DNC.DealerId
		INNER JOIN TC_DealerMakes TDM WITH (NOLOCK) ON TDM.DealerId = D.ID
		JOIN States AS S WITH (NOLOCK) ON D.StateId = S.Id
		JOIN CarMakes AS CMA WITH (NOLOCK) ON TDM.MakeId = CMA.ID
		WHERE 
			TDM.MakeId = @MakeId
			AND DNC.IsLocatorActive = 1
			AND S.IsDeleted = 0
			AND CMA.IsDeleted = 0
			AND D.TC_DealerTypeId = 2
			AND D.IsDealerActive = 1
			AND CMA.New = 1 
			GROUP BY S.ID,s.Name
	  ORDER BY S.Name 
END
