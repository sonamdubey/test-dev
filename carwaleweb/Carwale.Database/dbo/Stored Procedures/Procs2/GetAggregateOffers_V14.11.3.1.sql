IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[GetAggregateOffers_V14]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[GetAggregateOffers_V14]
GO

	
-- =============================================
-- Author:		Supriya Khartode
-- Create date: 20/11/2014
-- Description:	Fetch all offers required on Offer aggregation page
-- exec [GetAggregateOffers]
--added DealerName by Ashish verma on 12/03/2014
-- 22-12-2014 Avishkar Commented to use below in CTE
-- 22-12-2014 Avishkar Modified to use benefit of index
-- =============================================
CREATE PROCEDURE [dbo].[GetAggregateOffers_V14.11.3.1]
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
	SELECT 
	DO.ID As OfferId,DO.MaxOfferValue, DOD.DealerId,DO.OfferTitle,DO.OfferDescription,(DO.OfferUnits - DO.ClaimedUnits) AS AvailabiltyCount,DO.Conditions,('http://' + DO.HostURL + DO.ImagePath + DO.ImageName)AS ImageUrl,DO.OfferType,
			 CONVERT(varchar, DO.EndDate, 106) AS ExpiryDate,OV.MakeId AS MakeId,CMA.Name AS MakeName, OV.ModelId AS ModelId,OV.VersionId AS VersionId,DOD.CityId AS CityId,DOD.ZoneId AS ZoneId,C.Name AS CityName
			 ,CMO.Name As ModelName,CV.Name AS VersionName,
			 --DNC.Name AS DealerName, --added DealerName by Ashish verma on 12/03/2014
			 ROW_NUMBER() OVER (PARTITION BY DO.Id Order by DOD.CityId) Row_Num
	FROM DealerOffers DO WITH (NOLOCK)
	INNER JOIN DealerOffersVersion OV WITH (NOLOCK) ON DO.ID = OV.OfferId
	INNER JOIN DealerOffersDealers DOD WITH (NOLOCK) ON DO.ID = DOD.OfferId	
	JOIN CarMakes CMA WITH (NOLOCK) ON OV.MakeId = CMA.ID
	LEFT JOIN Cities C WITH (NOLOCK) ON DOD.CityId = C.ID
	LEFT JOIN CarModels CMO WITH (NOLOCK) ON OV.ModelId = CMO.Id
	LEFT JOIN CarVersions CV WITH (NOLOCK) ON OV.VersionId = CV.Id
	--JOIN Dealer_NewCar DNC WITH (NOLOCK) ON DNC.Id = DOD.DealerId -- 22-12-2014 Avishkar Commented to use below in CTE
	WHERE DO.IsActive = 1 
		AND DO.IsApproved = 1
		AND DO.OfferType IN (1,3)
		AND DO.StartDate <=GETDATE() -- 22-12-2014 Avishkar Modified to use benefit of index
		AND DO.EndDate >=GETDATE()		
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
			     CTE.MakeId,
			     MakeName, 
			     ModelId,
			     VersionId,
			     CTE.CityId,
			     ZoneId,
			     CityName,
			     ModelName,
			     VersionName,
				 DNC.Name AS DealerName
       FROM CTE 
       LEFT JOIN Dealer_NewCar DNC WITH (NOLOCK) ON DNC.Id = CTE.DealerId
       WHERE Row_Num=1
		ORDER BY OfferType,MaxOfferValue Desc

END
