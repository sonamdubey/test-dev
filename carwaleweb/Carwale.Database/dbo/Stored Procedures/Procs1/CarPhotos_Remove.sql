IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[CarPhotos_Remove]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[CarPhotos_Remove]
GO

	-- =============================================
-- Author:		Satish Sharma
-- Create date: DEC 10, 2008
-- Description:	SP to remove selected image, if selected image was main image 
--				then mark next available image as a main image
-- =============================================
CREATE PROCEDURE [dbo].[CarPhotos_Remove]
	-- Add the parameters for the stored procedure here
	@ImageUrlThumb	VARCHAR(50),
	@InquiryId		NUMERIC
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	
	DECLARE @IsMainPhoto Bit

	SELECT @IsMainPhoto = IsMain FROM CarPhotos 
	WHERE IsDealer = 0 AND IsActive = 1 AND InquiryId = @InquiryId AND ImageUrlThumb = @ImageUrlThumb

	-- Remove the image
	UPDATE CarPhotos SET IsActive = 0 WHERE IsDealer = 0 AND
	ImageUrlThumb = @ImageUrlThumb AND InquiryId = @InquiryId

	-- If it was main photo you just deleted, make the next photo as a main photo
	IF @IsMainPhoto = 1
		BEGIN
			UPDATE CarPhotos SET IsMain = 1 WHERE InquiryId = @InquiryId AND
			IsDealer = 0 AND ImageUrlThumb IN( SELECT TOP 1 ImageUrlThumb FROM CarPhotos 
			WHERE IsDealer = 0 AND IsActive = 1 AND InquiryId = @InquiryId)
		END
END