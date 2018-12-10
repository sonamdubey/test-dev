IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[AbSure_MakeMainImage_SP]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[AbSure_MakeMainImage_SP]
GO

	-- =============================================
-- Author:		Ashwini Dhamankar
-- Create date: 19-01-2015
-- Description:	To make selected image as main image
-- =============================================
CREATE PROCEDURE [dbo].[AbSure_MakeMainImage_SP]
	@AbSure_CarDetailsId BIGINT,
	@AbSure_CarPhotosId BIGINT,
	@ModifiedBy INT
AS
BEGIN
	SET NOCOUNT ON;
	UPDATE AbSure_CarPhotos SET IsMain = 0 WHERE AbSure_CarDetailsId = @AbSure_CarDetailsId
	UPDATE AbSure_CarPhotos SET IsMain = 1,ModifiedBy = @ModifiedBy, ModifiedDate = GETDATE() WHERE AbSure_CarPhotosId = @AbSure_CarPhotosId 
END
