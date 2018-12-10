IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[GetOffersVersionCity]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[GetOffersVersionCity]
GO

	 

-- =============================================
-- Author:		Piyush Sahu
-- Create date: 09/12/2015
-- Description:	Fetch all offers

-- =============================================
CREATE   PROCEDURE [dbo].[GetOffersVersionCity] 
	-- Add the parameters for the stored procedure here
	 @ModelId INT
	,@CityId INT 
	,@PlatformId INT
	
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	DECLARE @makeId INT;
	SELECT @makeId = CarMakeId FROM CarModels WITH (NOLOCK) WHERE Id = @ModelId
	
	
	SELECT  DO.ID as OfferId
		,DOD.DealerId
		,DO.OfferTitle as Title
		,DO.OfferDescription as Description
		,(DO.OfferUnits - DO.ClaimedUnits) as AvailabilityCount
		,DO.HostURL
		,DO.OriginalImgPath as OriginalImg
		,CONVERT(varchar, DO.EndDate, 106) as ExpiryDate
		,DO.OfferType
		,DNC.Organization
		,DO.SourceCategory
		,DO.DispOnDesk
		,DO.DispOnMobile
		,DO.DispSnippetOnDesk
		,DO.DispSnippetOnMob
		,DO.ShortDescription
		,OV.VersionId
		,DO.StartDate
	FROM DealerOffers DO WITH (NOLOCK)
	INNER JOIN DealerOffersVersion OV WITH (NOLOCK) ON DO.ID = OV.OfferId
	INNER JOIN DealerOffersDealers DOD WITH (NOLOCK) ON DO.ID = DOD.OfferId
	LEFT JOIN Dealers DNC  WITH (NOLOCK) ON DNC.Id = DOD.DealerId
	WHERE DO.IsActive = 1
		AND DO.IsApproved = 1
		AND DO.OfferType = 3
		AND Convert(DATE, DO.StartDate) <= Convert(DATE, GETDATE())
		AND Convert(DATE, DO.EndDate) >= Convert(DATE, GETDATE())
		AND (DOD.CityId=@CityId OR DOD.CityId = -1)
		AND (OV.MakeId = @makeId AND (OV.ModelId=@ModelId OR OV.ModelId = -1))
		
		AND ((DO.OfferUnits - DO.ClaimedUnits) > 0)
		--AND ( (@PlatformId=1 AND DispOnDesk=1) OR (@PlatformId in (43,74,83) AND DispOnMobile=1) OR OfferType!=3 )--Added By Vikas J
		AND ( (@PlatformId=1 AND DispSnippetOnDesk =1) OR (@PlatformId in (43,74,83) AND DispSnippetOnMob=1) OR OfferType!=3 ) --Added Mukul Bansal
	
END



