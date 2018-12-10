IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[GetOfferModelsByMake]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[GetOfferModelsByMake]
GO

	
-- =============================================
-- Author:		Supriya Khartode
-- Create date: 21/11/2014
-- Description:	Fetch all models for which offer is available based on cityid & makeid passed
-- =============================================
CREATE PROCEDURE [dbo].[GetOfferModelsByMake]
	@CityId INT = - 1
	,@MakeId INT = -1
AS
BEGIN

	SELECT Distinct CMO.Name AS ModelName, --,DO.ID As OfferId
	case when OV.ModelId <> -1 then  OV.ModelId else CMO.ID END AS ModelId
	FROM DealerOffersVersion OV  WITH (NOLOCK)
	INNER JOIN DealerOffers DO WITH (NOLOCK) ON OV.OfferId = DO.ID 
	INNER JOIN DealerOffersDealers DOD WITH (NOLOCK) ON DO.ID = DOD.OfferId
	LEFT JOIN CarModels CMO WITH (NOLOCK) ON OV.ModelId = CMO.ID OR OV.ModelId = -1 
	WHERE DO.IsActive = 1
		AND DO.IsApproved = 1
		AND DO.OfferType IN (1,3)
		AND Convert(DATE, DO.StartDate) <= Convert(DATE, GETDATE())
		AND Convert(DATE, DO.EndDate) >= Convert(DATE, GETDATE())
		AND ((DO.OfferUnits - DO.ClaimedUnits) > 0)
		AND (@CityId=-1 OR DOD.CityId=-1  OR (DOD.CityId=@CityId))
		AND (@MakeId=-1 OR OV.MakeId = @MakeId)
		AND CMO.CarMakeId = @MakeId 
		AND CMO.IsDeleted = 0 AND CMO.New = 1 AND CMO.Futuristic = 0
END

