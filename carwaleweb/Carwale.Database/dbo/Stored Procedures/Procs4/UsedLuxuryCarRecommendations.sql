IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[UsedLuxuryCarRecommendations]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[UsedLuxuryCarRecommendations]
GO

	

-- Created By: Aditi Dhaybar on 8-4-2015 for BBT dealer recommendation cars
CREATE PROCEDURE [dbo].[UsedLuxuryCarRecommendations] @ModelId NUMERIC(18, 0)
	,@DealerId INT = NULL
AS
BEGIN
	SELECT 'http://' + CP.HostUrl as HostUrl
		,LL.Price
		,LL.Kilometers
		,ISNULL(CP.DirectoryPath, '') + ISNULL(CP.ImageUrlThumb, '') AS FrontImagePath
		,YEAR(LL.MakeYear) AS MakeYear
		,LL.MakeName
		,LL.ModelName
		,LL.VersionName
		,DL.ActiveMaskingNumber
		,LL.CityName
		,LL.ProfileId
		,CMO.MaskingName
	FROM livelistings LL WITH (NOLOCK)
	INNER JOIN CarModels CMO WITH (NOLOCK) ON LL.RootId = CMO.RootId
	LEFT JOIN Dealers DL WITH (NOLOCK) ON LL.DealerId = DL.ID
	INNER JOIN CarModelRoots CMR WITH (NOLOCK) ON LL.RootId = CMR.RootId
	INNER JOIN CarPhotos CP WITH(NOLOCK) ON CP.InquiryId = LL.Inquiryid AND Sellertype=1 AND  CP.IsApproved = 1 AND CP.IsMain = 1 AND IsDealer=1
	WHERE DealerId = 3849
		AND CMO.ID = @ModelId
		AND PhotoCount <> 0
		AND CMR.IsSuperLuxury = 1
	ORDER BY Price
END

