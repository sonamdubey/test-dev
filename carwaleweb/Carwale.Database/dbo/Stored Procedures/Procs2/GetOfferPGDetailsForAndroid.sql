IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[GetOfferPGDetailsForAndroid]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[GetOfferPGDetailsForAndroid]
GO

	-- =============================================
-- Author:		Supriya Khartode
-- Create date: 1/12/2014
-- Description:	Fetch all the details needed for payment gateway transaction according to responseid & cityid passed.
--			  : This will be used in case when user comes from android to mobile page for online payment. 
-- =============================================
CREATE PROCEDURE [dbo].[GetOfferPGDetailsForAndroid]
	@ResponseId INT
	,@CityId INT 
	,@OfferId INT
	,@VersionId INT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	SELECT DL.Name,DL.Email,DL.Mobile,DL.PQId	
	FROM  PQDealerAdLeads DL WITH (NOLOCK) WHERE Id= @ResponseId
	
	SELECT Name AS CityName FROM Cities WITH(NOLOCK) WHERE id=@CityId

	SELECT OfferDescription FROM DealerOffers WITH (NOLOCK) WHERE ID = @OfferId 

	SELECT CMA.Name + ' ' + CMO.Name + ' ' + CV.Name AS CarName,'http://' + CV.HostURL + CV.DirPath + CV.largePic AS CarImage
	FROM CarVersions CV WITH (NOLOCK) 
	JOIN CarModels CMO WITH (NOLOCK)
	ON CV.CarModelId = CMO.ID
	JOIN CarMakes CMA WITH (NOLOCK)
	ON CMO.CarMakeId = CMA.ID
	WHERE CV.ID = @VersionId
	
END
