IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[GetOffers_v14]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[GetOffers_v14]
GO

	
-- =============================================
-- Author:		Supriya Khartode
-- Create date: 18/11/2014
-- Description:	Fetch all offers
-- Modified by Sanjay Soni On 4/12/2014 for fetch offers if (zoneId is available) elseIf (zoneId is not available but cityId is available)
-- Modeified by ashish verma added DealerName As Output parameter
-- =============================================
CREATE  PROCEDURE [dbo].[GetOffers_v14.11.3.2]
	-- Add the parameters for the stored procedure here
	@MakeId INT = - 1
	,@ModelId INT = - 1
	,@VersionId INT = - 1
	,@CityId INT = - 1
	,@ZoneId INT = null
	,@Title VARCHAR(100) OUTPUT
	,@Description VARCHAR(1000) OUTPUT
	,@AvailabiltyCount INT OUTPUT
	,@TermsAndCondition VARCHAR(max) OUTPUT
	,@Image VARCHAR(100) OUTPUT
	,@OfferId INT OUTPUT
	,@DealerId INT OUTPUT
	,@ExpiryDate VARCHAR(30) OUTPUT
	,@OfferType INT OUTPUT
	,@DealerName Varchar(50) OUTPUT -- added by ashish verma
	,@SourceCategory INT OUTPUT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	DECLARE @CMKId INT = @MakeId, @CMOId INT = @ModelId, @CV INT = @VersionId
	IF @VersionId <> -1
		SELECT @CMKId = MakeId, @CMOId = ModelId FROM vwMMV WHERE VersionId = @VersionId
	ELSE IF @ModelId <> -1
		SELECT @CMKId = MakeId FROM vwMMV WHERE ModelId = @ModelId
	SELECT TOP 1 @OfferId = DO.ID
		,@DealerId = DOD.DealerId
		,@Title = DO.OfferTitle
		,@Description = DO.OfferDescription
		,@AvailabiltyCount = (DO.OfferUnits - DO.ClaimedUnits)
		,@TermsAndCondition = DO.Conditions
		,@Image = 'http://' + DO.HostURL + DO.ImagePath + DO.ImageName
		,@ExpiryDate =  CONVERT(varchar, DO.EndDate, 106)
		,@OfferType = DO.OfferType
		,@DealerName = DNC.Name -- added by ashish verma
		,@SourceCategory = DO.SourceCategory
	FROM DealerOffers DO WITH (NOLOCK)
	INNER JOIN DealerOffersVersion OV WITH (NOLOCK) ON DO.ID = OV.OfferId
	INNER JOIN DealerOffersDealers DOD WITH (NOLOCK) ON DO.ID = DOD.OfferId
	LEFT JOIN Dealer_NewCar DNC  WITH (NOLOCK) ON DNC.Id = DOD.DealerId -- added by ashish verma
	WHERE DO.IsActive = 1
		AND DO.IsApproved = 1
		AND DO.OfferType IN (1,3)
		AND Convert(DATE, DO.StartDate) <= Convert(DATE, GETDATE())
		AND Convert(DATE, DO.EndDate) >= Convert(DATE, GETDATE())
		AND (@CityId=-1 OR DOD.CityId=-1  or (DOD.CityId=@CityId AND ISNULL(DOD.ZoneId,0) =ISNULL(@ZoneId,0) ))
		--AND   (     (DOD.CityId=@CityId AND @ZoneId IS NULL) OR  
		--             (ISNULL(DOD.ZoneId,0) =ISNULL(@ZoneId,0) ) OR 
		--			 DOD.CityId=-1 OR
		--			 @CityId=-1
		--      )
		AND (@CMKId=-1 OR OV.MakeId=@CMKId )
		AND (@CMOId=-1 OR OV.ModelId=@CMOId OR OV.ModelId=-1)
		AND (@CV=-1 OR OV.VersionId=@CV OR OV.VersionId=-1)
		AND ((DO.OfferUnits - DO.ClaimedUnits) > 0)
	ORDER BY DO.OfferType,NEWID()
END



/****** Object:  StoredProcedure [dbo].[GetAggregateOffers_V14.11.3.2]    Script Date: 12/23/2014 5:09:03 PM ******/
-- SET ANSI_NULLS ON
