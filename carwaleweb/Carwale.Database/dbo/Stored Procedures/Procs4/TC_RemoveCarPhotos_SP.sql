IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_RemoveCarPhotos_SP]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_RemoveCarPhotos_SP]
GO

	
-- =========================================================================================   
-- Author:  Vikas C    
-- Create date: MAR 02, 2011
-- Description: SP to remove selected image, if selected image was main image     
-- then mark next available image as a main image    
-- Modified By: Nilesh Utture on 11th March, 2013 Updated LastUpdatedDate in TC_Stock
-- Modified By: Nilesh Utture on 03rd April, 2013 Added BranchId parameter and added check before Removing Images
-- Modified By: Tejashree Patil on 4 Jan 2015, Added condition Id = @@PhotoId while updatind IsActive = 0
-- Modified By: Tejashree Patil on 17 Oct 2016, Added conditions TC_CarPhotoId = @PhotoId  
-- Modified By: Tejashree Patil on 17 Oct 2016, To remove car photos from Autobiz and carwale
-- ==========================================================================================     
CREATE PROCEDURE [dbo].[TC_RemoveCarPhotos_SP] @StockId INT
	,@PhotoId INT
	,@BranchId INT = NULL
AS
BEGIN
	IF EXISTS (
			SELECT TOP (1) S.Id
			FROM TC_Stock S WITH (NOLOCK)
			WHERE S.Id = @StockId
				AND S.BranchId = @BranchId
			)
	BEGIN
		DECLARE @IsMainPhoto BIT
			,@ImageUrlFull VARCHAR(100)
			,@InquiryId INT

		SELECT @IsMainPhoto = IsMain
			,@ImageUrlFull = ImageUrlFull
		FROM TC_CarPhotos WITH (NOLOCK)
		WHERE Id = @PhotoId

		-- Remove the image    
		UPDATE TC_CarPhotos
		SET IsActive = 0
		WHERE Id = @PhotoId

		-- If it was main photo you just deleted, make the next photo as a main photo    
		IF @IsMainPhoto = 1
		BEGIN
			DECLARE @TCMainNewImgId INT

			SELECT TOP 1 @TCMainNewImgId = Id
			FROM TC_CarPhotos WITH (NOLOCK)
			WHERE IsActive = 1
				AND StockId = @StockId

			UPDATE TC_CarPhotos
			SET IsMain = 1
			WHERE StockId = @StockId
				AND Id = @TCMainNewImgId
		END

		------update stock last updated date change made on 13-03-13 by Nilesh
		UPDATE TC_Stock
		SET LastUpdatedDate = GETDATE()
		WHERE Id = @StockId
	END
END
