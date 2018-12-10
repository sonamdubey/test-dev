IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_UpdateCarPhotos]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_UpdateCarPhotos]
GO

	
-- Modified By : Chetan Navin - 16 Nov 2015 (To Update OriginalPath in TC_CarPhoots)
-- TC_UpdateCarPhotos 12683,null,null,2
-- =============================================
CREATE PROCEDURE [dbo].[TC_UpdateCarPhotos] 
	
	@PhotoIds VARCHAR(1000),
	@StockId BIGINT,
	@OriginalImgPath VARCHAR(250) = NULL,
	@Type TINYINT
AS
BEGIN	
	IF(@Type = 0)								       --Type 0 for Updating stockId against photoIds
		UPDATE TC_CarPhotos SET StockId = @StockId WHERE Id IN (SELECT ListMember FROM fnSplitCSV(@PhotoIds))

	ELSE IF(@Type = 1)								   --Type 1 for making photos inactive
		UPDATE TC_CarPhotos SET IsActive = 0 WHERE Id IN (SELECT ListMember FROM fnSplitCSV(@PhotoIds))

	ELSE IF(@Type = 2)								   --Type 2 to update original image path
	BEGIN	
		DECLARE @Version VARCHAR(20) = CONVERT(VARCHAR(20), GETDATE(), 20),@Expression VARCHAR(3) = '?v='
		UPDATE TC_CarPhotos 
		SET OriginalImgPath = CASE WHEN CHARINDEX(@Expression, OriginalImgPath ) > 0 THEN SUBSTRING(OriginalImgPath,1,(PATINDEX('%?%',OriginalImgPath)  - 1)) + @Expression + @Version ELSE OriginalImgPath + @Expression + @Version END,
			ImageUrlFull = CASE WHEN CHARINDEX(@Expression ,ImageUrlFull ) > 0  THEN SUBSTRING(ImageUrlFull,1,(PATINDEX('%?%',ImageUrlFull) - 1)) + @Expression + @Version ELSE ImageUrlFull + @Expression + @Version END,
			ImageUrlThumb = CASE WHEN CHARINDEX(@Expression,ImageUrlThumb) > 0  THEN SUBSTRING(ImageUrlThumb,1,(PATINDEX('%?%',ImageUrlThumb) - 1)) + @Expression + @Version ELSE ImageUrlThumb + @Expression + @Version END,
			ImageUrlThumbSmall = CASE WHEN CHARINDEX(@Expression,ImageUrlThumbSmall) > 0  THEN SUBSTRING(ImageUrlThumbSmall,1,(PATINDEX('%?%',ImageUrlThumbSmall) - 1)) + @Expression + @Version ELSE ImageUrlThumbSmall + @Expression + @Version END,
			ImageUrlMedium = CASE WHEN CHARINDEX(@Expression,ImageUrlMedium) > 0  THEN SUBSTRING(ImageUrlMedium,1,(PATINDEX('%?%',ImageUrlMedium) - 1)) + @Expression + @Version ELSE ImageUrlMedium + @Expression + @Version END,
			OrigFileName = CASE WHEN CHARINDEX(@Expression,OrigFileName) > 0  THEN SUBSTRING(OrigFileName,1,(PATINDEX('%?%',OrigFileName) - 1)) + @Expression + @Version ELSE OrigFileName + @Expression + @Version END
		WHERE Id IN (SELECT ListMember FROM fnSplitCSV(@PhotoIds))
	END
END