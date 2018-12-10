IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[GetAllImagesForProfileId]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[GetAllImagesForProfileId]
GO

	
-- =============================================
-- Author:		Akansha 
-- Create date: 17.06.2014
-- Description:	Get Full Image Path for used car search gallery
-- =============================================
CREATE PROCEDURE [dbo].[GetAllImagesForProfileId] 
@InquiryId NUMERIC(18, 0),
@IsDealer bit
AS
BEGIN
	SELECT HostUrl + DirectoryPath + ImageUrlFull AS ImagePath
	FROM CarPhotos
	WHERE IsActive = 1
		AND IsApproved = 1
		AND InquiryId = @InquiryId
		AND IsDealer = @IsDealer
	ORDER BY IsMain DESC
END

