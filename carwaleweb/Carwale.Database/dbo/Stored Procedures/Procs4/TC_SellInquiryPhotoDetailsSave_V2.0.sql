IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_SellInquiryPhotoDetailsSave_V2]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_SellInquiryPhotoDetailsSave_V2]
GO

	

-- Created By:	Deepak Tripathi
-- Create date: 11th July 2016
-- Description:	Adding New Lead

-- =============================================
create  PROCEDURE [dbo].[TC_SellInquiryPhotoDetailsSave_V2.0]
	@CWSellInquiryId		INT,
	@CW_CustomerId			INT,
	@TC_SellerInquiryId		INT
AS
	BEGIN
		SET NOCOUNT ON;
		IF (@CWSellInquiryId IS NOT NULL AND @CW_CustomerId IS NOT NULL)
		BEGIN	
			IF EXISTS(	SELECT TOP 1 C.Id FROM CarPhotos C WITH(NOLOCK) 
						INNER JOIN CustomerSellInquiries SI WITH(NOLOCK) ON SI.Id=C.InquiryId
						WHERE C.InquiryId=@CWSellInquiryId AND C.IsDealer=0)
			BEGIN									
				INSERT INTO TC_SellCarPhotos(TC_SellerInquiriesId,ImageUrlFull,ImageUrlThumb,ImageUrlThumbSmall,
							DirectoryPath,IsMain,IsActive,HostUrl,IsReplicated,EntryDate,OriginalImgPath)
				SELECT	@TC_SellerInquiryId,ImageUrlFull,ImageUrlThumb,ImageUrlThumbSmall,DirectoryPath,IsMain,IsActive,HostUrl,IsReplicated,GETDATE(),OriginalImgPath
				FROM	CarPhotos WITH(NOLOCK)
				WHERE	InquiryId=@CWSellInquiryId 
						AND IsDealer=0
			END
		END
END



