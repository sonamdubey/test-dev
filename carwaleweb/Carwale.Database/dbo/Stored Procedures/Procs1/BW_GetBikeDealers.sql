IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[BW_GetBikeDealers]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[BW_GetBikeDealers]
GO

	-- =============================================
-- Author:		Suresh Prajapati
-- Create date: 28th Oct 2014
-- Description:	To Get Bike Dealers.

-- Modified By : Suresh Prajapati on 20th jan, 2015
-- Description : Retrieved MakeId field from dealer's Id to get dealer's brand name 

-- Modified By : Suresh Prajapati on 12th Feb, 2015
-- Description : added DISTINCT in select clause to avoid selection of duplicate  data 
-- EXEC BW_GetBikeDealers 1
-- Modified By : Sadhana Upadhyay on 5 Jan 2015 To sort by Organization
-- =============================================
CREATE PROCEDURE [dbo].[BW_GetBikeDealers] @cityId INT
AS
BEGIN
	SET NOCOUNT ON;

	SELECT DISTINCT D.Organization AS Text
		,D.Id AS Value
		,DMA.MakeId AS MakeId
	FROM Dealers AS D WITH (NOLOCK)
	INNER JOIN TC_DealerMakes AS DMA WITH (NOLOCK) ON DMA.DealerId = D.ID
	INNER JOIN BikeMakes AS BMA WITH (NOLOCK) ON BMA.ID = DMA.MakeId
	WHERE D.ApplicationId = 2
		AND D.CityId = @cityId
		AND D.IsDealerActive = 1
		AND D.IsDealerDeleted = 0
		AND BMA.IsDeleted = 0
		AND BMA.New = 1
		AND BMA.Futuristic = 0
	ORDER BY D.Organization
END


