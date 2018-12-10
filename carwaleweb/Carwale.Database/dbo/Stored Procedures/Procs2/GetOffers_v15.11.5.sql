IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[GetOffers_v15]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[GetOffers_v15]
GO

	-- =============================================
-- Author:		Supriya Khartode
-- Create date: 18/11/2014
-- Description:	Fetch all offers
-- Modified by Sanjay Soni On 4/12/2014 for fetch offers if (zoneId is available) elseIf (zoneId is not available but cityId is available)
-- Modeified by ashish verma added DealerName As Output parameter
-- Modeified by Rohan S set @TermsAndCondition = ''
-- Modified By Vikas J: Added New parametere PlatformId and used it for filter 
-- Modified By Chetan T: Retrieved 3 More columns
-- =============================================
CREATE PROCEDURE [dbo].[GetOffers_v15.11.5]
	-- Add the parameters for the stored procedure here
	@MakeId INT = - 1
	,@ModelId INT = - 1
	,@VersionId INT = - 1
	,@CityId INT = - 1
	,@ZoneId INT = null
	,@PlatformId INT = 1
	,@Title VARCHAR(100) OUTPUT
	,@Description VARCHAR(1000) OUTPUT
	,@AvailabiltyCount INT OUTPUT
	,@TermsAndCondition VARCHAR(10) OUTPUT
	,@Image VARCHAR(100) OUTPUT
	,@HostUrl VARCHAR(100) OUTPUT
	,@OriginalImg VARCHAR(150) OUTPUT
	,@OfferId INT OUTPUT
	,@DealerId INT OUTPUT
	,@DispOnDesk BIT OUTPUT  -- added by Sanjay On 25/2/2015
	,@DispOnMobile BIT OUTPUT -- added by Sanjay On 25/2/2015
	,@ExpiryDate VARCHAR(30) OUTPUT
	,@OfferType INT OUTPUT
	,@DealerName Varchar(50) OUTPUT -- added by ashish verma
	,@SourceCategory INT OUTPUT
	,@DispSnippetOnDesk BIT OUTPUT
	,@DispSnippetOnMob BIT OUTPUT
	,@ShortDescription VARCHAR(MAX) OUTPUT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	DECLARE @CMKId INT = @MakeId, @CMOId INT = @ModelId, @CV INT = @VersionId
	IF @VersionId <> -1
		SELECT @CMKId = MakeId, @CMOId = ModelId FROM vwMMV WITH (NOLOCK) WHERE VersionId = @VersionId
	ELSE IF @ModelId <> -1
		SELECT @CMKId = MakeId FROM vwMMV  WITH (NOLOCK) WHERE ModelId = @ModelId
	SELECT TOP 1 @OfferId = DO.ID
		,@DealerId = DOD.DealerId
		,@Title = DO.OfferTitle
		,@Description = DO.OfferDescription
		,@AvailabiltyCount = (DO.OfferUnits - DO.ClaimedUnits)
		,@TermsAndCondition = ''--modified by Rohan S
		,@Image = 'http://' + DO.HostURL + DO.ImagePath + DO.ImageName
		,@HostUrl = DO.HostURL
		,@OriginalImg = DO.OriginalImg
		,@ExpiryDate =  CONVERT(varchar, DO.EndDate, 106)
		,@OfferType = DO.OfferType
		,@DealerName = DNC.Organization -- added by vinayak
		,@SourceCategory = DO.SourceCategory
		,@DispOnDesk = DO.DispOnDesk
		,@DispOnMobile = DO.DispOnMobile
		,@DispSnippetOnDesk= DO.DispSnippetOnDesk
		,@DispSnippetOnMob = DO.DispSnippetOnMob
		,@ShortDescription = DO.ShortDescription
	FROM DealerOffers DO WITH (NOLOCK)
	INNER JOIN DealerOffersVersion OV WITH (NOLOCK) ON DO.ID = OV.OfferId
	INNER JOIN DealerOffersDealers DOD WITH (NOLOCK) ON DO.ID = DOD.OfferId
	LEFT JOIN Dealers DNC  WITH (NOLOCK) ON DNC.Id = DOD.DealerId -- added by ashish verma
	WHERE DO.IsActive = 1
		AND DO.IsApproved = 1
		AND DO.OfferType IN (1,3)
		AND Convert(DATE, DO.StartDate) <= Convert(DATE, GETDATE())
		AND Convert(DATE, DO.EndDate) >= Convert(DATE, GETDATE())
		AND (@CityId=-1 OR DOD.CityId=-1  or (DOD.CityId=@CityId AND ISNULL(DOD.ZoneId,0) =ISNULL(@ZoneId,0) ))
		AND (@CMKId=-1 OR OV.MakeId=@CMKId )
		AND (@CMOId=-1 OR OV.ModelId=@CMOId OR OV.ModelId=-1)
		AND (@CV=-1 OR OV.VersionId=@CV OR OV.VersionId=-1)
		AND ((DO.OfferUnits - DO.ClaimedUnits) > 0)
		--AND ( (@PlatformId=1 AND DispOnDesk=1) OR (@PlatformId in (43,74,83) AND DispOnMobile=1) OR OfferType!=3 )--Added By Vikas J
		AND ( (@PlatformId in (1,43)) OR(@PlatformId in (74,83) AND DispOnMobile=1) OR OfferType!=3 ) --Added Chetan
	ORDER BY DO.OfferType,NEWID()
END
