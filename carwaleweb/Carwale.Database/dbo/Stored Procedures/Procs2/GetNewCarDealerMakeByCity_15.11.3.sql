IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[GetNewCarDealerMakeByCity_15]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[GetNewCarDealerMakeByCity_15]
GO

	-- Modified By : Vicky Lund- Implementation of data migration of dealer_NewCar
CREATE PROCEDURE [dbo].[GetNewCarDealerMakeByCity_15.11.3] (@CityId SMALLINT)
AS
BEGIN
	SET NOCOUNT ON;

	SELECT Ma.ID AS Value
		,Ma.NAME AS TEXT
	FROM CarMakes Ma WITH (NOLOCK)
	WHERE Ma.IsDeleted = 0
		AND Ma.Id IN (
			SELECT TDM.MakeId
			FROM TC_DealerMakes TDM WITH (NOLOCK)
			INNER JOIN Dealers D WITH (NOLOCK) ON TDM.DealerId = D.ID
				AND D.CityId = @CityId
				AND D.TC_DealerTypeId = 2
				AND D.IsDealerActive = 1
				AND D.IsDealerDeleted = 0
			INNER JOIN DealerLocatorConfiguration DLC WITH (NOLOCK) ON TDM.DealerId = DLC.DealerId
				AND DLC.IsLocatorActive = 1
			)
	ORDER BY TEXT
END
