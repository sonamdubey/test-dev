IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[DCRM_FetchSellInqueriesForAlbumRating]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[DCRM_FetchSellInqueriesForAlbumRating]
GO

	
-- =============================================
-- Author:		Chetan Kane
-- Create date: 9th August 2012
-- Description:	to fetch sell inqueries of a perticular dealer to rate the Photo Album of the car 
-- Modified By	:	Sachin Bharti(1st Aug 2014)
-- Purpose		:	Uploaded SP to live because IsDealer field is not there added with (nolock)
-- =============================================
CREATE PROCEDURE [dbo].[DCRM_FetchSellInqueriesForAlbumRating]
	
	@DealerId NUMERIC
AS	
BEGIN
	SELECT DISTINCT
		SI.ID as Id, LL.MakeName + LL.ModelName + LL.VersionName AS CarName, SI.CarRegNo AS RegNo, LL.Color AS Color, SI.AlbumRating AS Rating,CP.IsDealer    
	FROM
		LiveListings LL WITH (NOLOCK)
		LEFT JOIN SellInquiries AS SI WITH (NOLOCK)  ON LL.Inquiryid = SI.ID AND LL.SellerType = 1
		LEFT JOIN CarPhotos CP WITH (NOLOCK) ON SI.Id = CP.InquiryId AND CP.IsDealer = 1 AND CP.IsApproved = 1 AND CP.IsActive = 1
	WHERE 
		 CP.ImageUrlThumb IS NOT NULL AND SI.DealerId = @DealerId
	ORDER BY Id
END


