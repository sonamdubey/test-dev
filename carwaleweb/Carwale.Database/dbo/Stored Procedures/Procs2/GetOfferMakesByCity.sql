IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[GetOfferMakesByCity]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[GetOfferMakesByCity]
GO

	
-- =============================================
-- Author:		Supriya Khartode
-- Create date: 21/11/2014
-- Description:	Fetch all makes for which offer is available based on cityid passed
-- =============================================
CREATE PROCEDURE [dbo].[GetOfferMakesByCity]
	@CityId INT = - 1
AS
BEGIN

	SELECT DISTINCT OV.MakeId,CMA.Name AS MakeName
	FROM DealerOffersVersion OV  WITH (NOLOCK)
	INNER JOIN  DealerOffers DO WITH (NOLOCK) ON OV.OfferId = DO.ID 
	INNER JOIN DealerOffersDealers DOD WITH (NOLOCK) ON DO.ID = DOD.OfferId
	INNER JOIN CarMakes CMA WITH (NOLOCK) ON OV.MakeId = CMA.ID
	WHERE DO.IsActive = 1
		AND DO.IsApproved = 1
	
		AND DO.OfferType IN (1,3)
		AND Convert(DATE, DO.StartDate) <= Convert(DATE, GETDATE())
		AND Convert(DATE, DO.EndDate) >= Convert(DATE, GETDATE())
		AND ((DO.OfferUnits - DO.ClaimedUnits) > 0)
		AND (@CityId=-1 OR DOD.CityId=-1  OR (DOD.CityId=@CityId))
	ORDER BY CMA.Name
END

