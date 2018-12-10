IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[GetAllImagesForProfileId_15]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[GetAllImagesForProfileId_15]
GO

	
-- =============================================
-- Author:		Akansha 
-- Create date: 17.06.2014
-- Description:	Get Full Image Path for used car search gallery
-- Modified by Naved on 12/06/2015 - Added a query for getting video URL
-- =============================================
CREATE PROCEDURE [dbo].[GetAllImagesForProfileId_15.7.1]
@InquiryId INT,
@IsDealer bit
AS
BEGIN
	SELECT HostUrl + DirectoryPath + ImageUrlFull AS ImagePath,
		HostUrl + DirectoryPath + ImageUrlThumb AS ImageThumb
	FROM CarPhotos WITH (NOLOCK)
	WHERE IsActive = 1
		AND IsApproved = 1
		AND InquiryId = @InquiryId
		AND IsDealer = @IsDealer
	ORDER BY IsMain DESC

	-- Query for accessing video URL
	IF(@IsDealer = 1)
	BEGIN
		SELECT  CD.YoutubeVideo
		FROM SellInquiriesDetails CD WITH (NOLOCK)
		WHERE CD.IsYouTubeVideoApproved = 1
		AND CD.SellInquiryId = @InquiryId
	END
	ELSE IF(@IsDealer = 0)
	BEGIN
		SELECT  CSD.YoutubeVideo
		FROM CustomerSellInquiryDetails CSD WITH (NOLOCK) 
		WHERE CSD.IsYouTubeVideoApproved = 1
			AND CSD.InquiryId = @InquiryId
	END
END



