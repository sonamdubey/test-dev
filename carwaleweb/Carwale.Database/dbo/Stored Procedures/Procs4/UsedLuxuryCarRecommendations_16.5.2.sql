IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[UsedLuxuryCarRecommendations_16]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[UsedLuxuryCarRecommendations_16]
GO

	
-- Created By: Aditi Dhaybar on 8-4-2015 for BBT dealer recommendation cars
-- Modified By : Aditi Dhaybar on 14-7-2015 for Make Page BBT dealer recommendation cars
CREATE PROCEDURE [dbo].[UsedLuxuryCarRecommendations_16.5.2] @CarId INT
	,@DealerId INT = NULL
	,@PageId INT = NULL

AS
BEGIN
DECLARE @MakeId INT = NULL
DECLARE	@ModelId INT = NULL


IF @PageId = 1
		SET @MakeId = @CarId
ELSE
		SET @ModelId = @CarId

	
			SELECT 'http://' + LL.HostUrl AS HostUrl
				,LL.Price
				,LL.Kilometers
				,(
					SELECT TOP 1 ISNULL(CP.DirectoryPath, '') + ISNULL(CP.ImageUrlThumb, '')  --Specific size required for the widget
					FROM CarPhotos AS CP WITH (NOLOCK)
					WHERE CP.InquiryId = LL.Inquiryid
						AND LL.Sellertype = 1
						AND CP.IsApproved = 1
						AND CP.IsMain = 1
						AND CP.IsDealer = 1
					) 'FrontImagePath'
				,YEAR(LL.MakeYear) AS MakeYear
				,LL.MakeName
				,LL.ModelName
				,LL.VersionName
				,LL.SellerContact
				,LL.CityName
				,LL.ProfileId
				,CMO.MaskingName
			FROM livelistings LL WITH (NOLOCK)
			INNER JOIN CarModels CMO WITH (NOLOCK) ON LL.ModelId = CMO.Id
			INNER JOIN CarModelRoots CMR WITH (NOLOCK) ON LL.RootId = CMR.RootId
			WHERE DealerId = @DealerId
				AND (
					LL.MakeId = @MakeId
					OR CMO.ID = @ModelId
					)
				AND PhotoCount <> 0
				AND CMR.IsSuperLuxury = 1
			ORDER BY LL.Price
END