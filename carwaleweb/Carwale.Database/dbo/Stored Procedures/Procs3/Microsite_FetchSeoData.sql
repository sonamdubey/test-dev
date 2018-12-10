IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[Microsite_FetchSeoData]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[Microsite_FetchSeoData]
GO

	-- =============================================
-- Author:		Komal Manjare
-- Create date: 04 August 2016
-- Description: Fetch dealer website data for SEO 
-- Modified By : Sunil M. Yadav On 17th Aug 2016, get city and version details.
-- EXEC Microsite_FetchSeoData 5,12,1,NULL,NULL,10
-- =============================================
CREATE PROCEDURE [dbo].[Microsite_FetchSeoData] @DealerId INT
	,@Microsite_WebsitePageId INT
	,@CityId INT = NULL
	,@VersionId INT = NULL
	,@ModelId INT = NULL
	,@MakeId INT = NULL
	,@StockId INT = NULL
	,@IsDealer BIT = NULL
AS
BEGIN
	IF (
			@StockId IS NOT NULL
			AND @StockId > 0
			)
	BEGIN
		SELECT @VersionId = VersionId
		FROM TC_Stock WITH (NOLOCK)
		WHERE Id = @StockId
	END
	ELSE
		IF (
				@VersionId IS NULL
				OR @VersionId < 0
				)
		BEGIN
			SELECT TOP 1 @VersionId = VersionId
			FROM vwMMV WITH (NOLOCK)
			WHERE MakeId = @MakeId
				AND (
					@ModelId IS NULL
					OR ModelId = @ModelId
					)
			ORDER BY VersionId DESC
		END

	SELECT TOP 1 D.Organization AS DealerName
		,MDSD.TitleTag
		,MDSD.DescriptionTag
		,MDSD.KeywordsTag
		,MDSD.DealerId
		,MWP.TitleTag AS DefaultTitleTag
		,MWP.KeywordsTag AS DefaultKeywordsTag
		,MWP.DescriptionTag AS DefaultDescriptionTag
		,MDSD.Microsite_WebsitePagesId
		,C.NAME AS City
		,VW.Make AS Brand
		,VW.Version AS Variant
		,VW.Model AS Model
		,CFT.FuelType
		,CTM.Descr Transmission
	FROM Microsite_WebsitePages MWP WITH (NOLOCK)
	JOIN Dealers D WITH (NOLOCK) ON D.Id = @DealerId
	JOIN Cities C WITH (NOLOCK) ON C.ID = D.CityId
	LEFT JOIN vwMMV VW WITH (NOLOCK) ON VW.VersionId = @VersionId
	LEFT JOIN CarFuelType CFT WITH (NOLOCK) ON CFT.FuelTypeId = VW.CarFuelType
	LEFT JOIN CarTransmission CTM WITH (NOLOCK) ON CTM.Id = VW.CarTransmission
	LEFT JOIN Microsite_DealerSeoDetails MDSD WITH (NOLOCK) ON MDSD.Microsite_WebsitePagesId = MWP.Id
		AND MDSD.DealerId = D.Id
	WHERE MWP.Id = @Microsite_WebsitePageId
		AND MWP.IsActive = 1
END
