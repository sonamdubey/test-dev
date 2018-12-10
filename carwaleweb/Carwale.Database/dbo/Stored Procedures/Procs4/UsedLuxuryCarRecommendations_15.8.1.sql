IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[UsedLuxuryCarRecommendations_15]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[UsedLuxuryCarRecommendations_15]
GO

	
-- Created By: Aditi Dhaybar on 8-4-2015 for BBT dealer recommendation cars
-- Modified By : Aditi Dhaybar on 14-7-2015 for Make Page BBT dealer recommendation cars
-- Modified By : Prachi Phalak on 04/08/2015 for resizing Make Page BBT dealer recommendation cars
CREATE PROCEDURE [dbo].[UsedLuxuryCarRecommendations_15.8.1] @CarId INT
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

	
			SELECT  LL.HostUrl AS HostUrl
				,LL.Price
				,LL.Kilometers
				,(
					SELECT TOP 1 ISNULL(OriginalImgPath, '')  --Specific size required for the widget
					FROM CarPhotos WITH (NOLOCK)
					WHERE InquiryId = LL.Inquiryid
						AND Sellertype = 1
						AND IsApproved = 1
						AND IsMain = 1
						AND IsDealer = 1
					) 'OriginalImgPath'
				,(
					SELECT TOP 1 ISNULL(DirectoryPath, '') + ISNULL(ImageUrlThumb, '')  --Specific size required for the widget
					FROM CarPhotos WITH (NOLOCK)
					WHERE InquiryId = LL.Inquiryid
						AND Sellertype = 1
						AND IsApproved = 1
						AND IsMain = 1
						AND IsDealer = 1
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
			WHERE DealerId = 3849
				AND (
					LL.MakeId = @MakeId
					OR CMO.ID = @ModelId
					)
				AND PhotoCount <> 0
				AND CMR.IsSuperLuxury = 1
			ORDER BY LL.Price
END

