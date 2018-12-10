IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[CH_GetToApprovePhotoVideo]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[CH_GetToApprovePhotoVideo]
GO

	
-- =============================================
-- Author      : Vinay Kumar Prajapati
-- Create date : 22nd Jan 2014
-- Description : Get video and photo data on basis of inquiry Id for Indivisual 
-- Modifier     : Vinay kumar prajapati 25 july 2014 adding WITH(NOLOCK) and get CustomerId  From Table CustomerSellInquiries For IC
-- =============================================

CREATE PROCEDURE [dbo].[CH_GetToApprovePhotoVideo]
    (
	@InquiryId   INT,
	@IsDealer    BIT
	)
 AS
   BEGIN
		IF @IsDealer = 1
			BEGIN
					SELECT CP.Id, CP.ImageUrlThumb, CP.ImageUrlThumbSmall,CP.ImageUrlFull,CP.ImageUrlMedium,DirectoryPath, CP.IsMain, 
						( MA.Name + ' ' + MO.Name + ' ' + CV.Name ) As  CarName, CP.HostUrl , CP.OriginalImgPath
					FROM CarPhotos AS  CP WITH(NOLOCK) 				
						INNER JOIN SellInquiries AS SI WITH(NOLOCK) On SI.ID = CP.InquiryId
						INNER JOIN CarVersions AS  CV WITH(NOLOCK) On CV.Id = SI.CarVersionId 			   
						INNER JOIN CarModels AS  MO  WITH(NOLOCK) On MO.Id = CV.CarModelId 
						INNER JOIN CarMakes AS MA WITH(NOLOCK) On MA.Id = MO.CarMakeId 
					WHERE IsActive = 1 AND CP.IsApproved = 0 AND CP.InquiryId = @InquiryId 

				END
			ELSE
				BEGIN 
					SELECT CP.Id, CP.ImageUrlThumb, CP.ImageUrlThumbSmall,CP.ImageUrlFull,CP.ImageUrlMedium,CP.DirectoryPath, CP.IsMain, 
					( MA.Name + ' ' + MO.Name + ' ' + CV.Name ) As  CarName, CP.HostUrl,CSI.CustomerId , CP.OriginalImgPath
					FROM CarPhotos AS CP WITH(NOLOCK)				
						INNER JOIN CustomerSellInquiries  AS CSI WITH(NOLOCK) On CSI.ID = CP.InquiryId 
						INNER JOIN CarVersions AS CV WITH(NOLOCK) ON CV.Id = CSI.CarVersionId 		   
						INNER JOIN CarModels AS MO WITH(NOLOCK)  ON MO.Id = CV.CarModelId 
						INNER JOIN CarMakes AS  MA WITH(NOLOCK) ON MA.Id = MO.CarMakeId 
					WHERE IsActive = 1 AND CP.IsApproved = 0 AND CP.InquiryId = @InquiryId 
				END


         SELECT CSD.InquiryId AS Id, CSD.IsYouTubeVideoApproved,ISNULL(CSD.YoutubeVideo,'') AS YoutubeVideo
         FROM CustomerSellInquiryDetails AS CSD WITH(NOLOCK) 
         WHERE CSD.InquiryId=@InquiryId AND CSD.IsYouTubeVideoApproved=0
            
    END

