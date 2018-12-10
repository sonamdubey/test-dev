IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[GetAggregateOffers]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[GetAggregateOffers]
GO

	
-- =============================================
-- Author:		Supriya Khartode
-- Create date: 20/11/2014
-- Description:	Fetch all offers required on Offer aggregation page
-- exec [GetAggregateOffers]
-- =============================================
CREATE PROCEDURE [dbo].[GetAggregateOffers]
	-- Add the parameters for the stored procedure here
	@MakeId INT = - 1
	,@ModelId INT = - 1
	,@VersionId INT = - 1
	,@CityId INT = - 1
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	DECLARE @CMKId INT = @MakeId, @CMOId INT = @ModelId, @CV INT = @VersionId
	IF @VersionId <> -1
		SELECT @CMKId = MakeId, @CMOId = ModelId FROM vwMMV WHERE VersionId = @VersionId
	ELSE IF @ModelId <> -1
		SELECT @CMKId = MakeId FROM vwMMV WHERE ModelId = @ModelId;
	
	WITH  CTE AS (
	SELECT DISTINCT
	DO.ID As OfferId,DO.MaxOfferValue, DOD.DealerId,DO.OfferTitle,DO.OfferDescription,(DO.OfferUnits - DO.ClaimedUnits) AS AvailabiltyCount,DO.Conditions,('http://' + DO.HostURL + DO.ImagePath + DO.ImageName)AS ImageUrl,DO.OfferType,
			 CONVERT(varchar, DO.EndDate, 106) AS ExpiryDate,OV.MakeId AS MakeId,CMA.Name AS MakeName, OV.ModelId AS ModelId,OV.VersionId AS VersionId,DOD.CityId AS CityId,DOD.ZoneId AS ZoneId,C.Name AS CityName
			 ,CMO.Name As ModelName,CV.Name AS VersionName,
			 ROW_NUMBER() OVER (PARTITION BY DO.Id Order by DOD.CityId) Row_Num
	FROM DealerOffers DO WITH (NOLOCK)
	INNER JOIN DealerOffersVersion OV WITH (NOLOCK) ON DO.ID = OV.OfferId
	INNER JOIN DealerOffersDealers DOD WITH (NOLOCK) ON DO.ID = DOD.OfferId
	LEFT JOIN CarMakes CMA WITH (NOLOCK) ON OV.MakeId = CMA.ID
	LEFT JOIN Cities C WITH (NOLOCK) ON DOD.CityId = C.ID
	LEFT JOIN CarModels CMO WITH (NOLOCK) ON OV.ModelId = CMO.Id
	LEFT JOIN CarVersions CV WITH (NOLOCK) ON OV.VersionId = CV.Id
	WHERE DO.IsActive = 1 
		AND DO.IsApproved = 1
		AND DO.OfferType IN (1,3)
		AND Convert(DATE, DO.StartDate) <= Convert(DATE, GETDATE())
		AND Convert(DATE, DO.EndDate) >= Convert(DATE, GETDATE())
	
		AND (@CityId=-1 OR DOD.CityId=-1  OR (DOD.CityId=@CityId))
		AND (@CMKId=-1 OR OV.MakeId=@CMKId )
		AND (@CMOId=-1 OR OV.ModelId=@CMOId OR OV.ModelId=-1)
		AND (@CV=-1 OR OV.VersionId=@CV OR OV.VersionId=-1)
		AND ((DO.OfferUnits - DO.ClaimedUnits) > 0)
     ) SELECT   OfferId,
                MaxOfferValue, 
                DealerId,
                OfferTitle,
                OfferDescription,
                AvailabiltyCount,
                Conditions,
                 ImageUrl,
                 OfferType,
			     ExpiryDate,
			     MakeId,
			     MakeName, 
			     ModelId,
			     VersionId,
			     CityId,
			     ZoneId,
			     CityName,
			     ModelName,
			     VersionName
       FROM CTE WHERE Row_Num=1
		ORDER BY OfferType,MaxOfferValue Desc

END
