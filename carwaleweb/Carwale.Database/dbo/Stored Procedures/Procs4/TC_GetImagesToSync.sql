IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_GetImagesToSync]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_GetImagesToSync]
GO

	
-- =============================================
-- Author      : Chetan Navin
-- Create date : 25th Oct 2016
-- Description : To fetch images to sync to carphotos table
-- EXEC TC_GetImagesToSync
-- =============================================
CREATE PROCEDURE [dbo].[TC_GetImagesToSync] (@ImageId INT)
AS
BEGIN
	SET NOCOUNT ON;

	DECLARE @TC_CarPhotosId INT

	SELECT @TC_CarPhotosId = ProcessedId
	FROM IMG_Photos WITH (NOLOCK)
	WHERE Id = @ImageId
		AND IsProcessed = 1
		AND CategoryId = 1

	SELECT TC.Id
		,TC.IsMain
		,TC.HostUrl
		,TC.OriginalImgPath
		,TS.Id AS StockId
	FROM TC_CarPhotos TC WITH (NOLOCK)
	INNER JOIN TC_Stock TS WITH (NOLOCK) ON TS.Id = TC.StockId
		AND TS.IsActive = 1
		AND TS.IsSychronizedCW = 1
	WHERE TC.Id = @TC_CarPhotosId
		AND TC.IsActive = 1
END
