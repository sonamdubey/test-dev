IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_AP_AutoSyncStockPhotos]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_AP_AutoSyncStockPhotos]
GO

	-- =============================================
-- Author:		<Vivek,,Gupta>
-- Create date: <Create Date,26-06-2015,>
-- Description:	Uploading Photos of stock which are not synced on live(carphotos)
-- =============================================
CREATE PROCEDURE [dbo].[TC_AP_AutoSyncStockPhotos]
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	/*
	DECLARE @TempPhotoId TABLE (Id INT)
	DECLARE @TempMainPhotoId TABLE (Id INT)

	INSERT INTO @TempPhotoId
	SELECT TC.Id 
	FROM TC_CarPhotos TC WITH (NOLOCK) 
	INNER JOIN TC_Stock TCS WITH (NOLOCK) ON TC.StockId = TCS.Id
	INNER JOIN SellInquiries SI WITH (NOLOCK) ON SI.TC_StockId = TCS.Id AND TCS.BranchId = SI.DealerId AND SI.SourceId = 2 --2 for Autobiz
	INNER JOIN LiveListings LL WITH (NOLOCK) ON SI.ID = LL.Inquiryid AND LL.SellerType = 1
	LEFT JOIN CarPhotos CP WITH(NOLOCK) ON   TC.ID=CP.TC_CarPhotoId 
										AND  CP.IsDealer = 1 
										AND CP.InquiryId = SI.ID
	WHERE TC.IsActive = 1 AND TC.EntryDate >= '2015-01-01 00:00:00.560' 
	AND CP.Id IS NULL ORDER BY TC.Id  DESC

	WHILE EXISTS (SELECT TOP 1 Id FROM @TempPhotoId)
	BEGIN
	    DECLARE @PhotoId INT
		SET @PhotoId = (SELECT TOP 1 Id FROM @TempPhotoId)
		EXEC TC_CarPhotosUpdate @PhotoId
		DELETE FROM @TempPhotoId WHERE Id=@PhotoId
	END*/
		
	--Release Duplicate Masking numbers also
	--Not related to above task
	--Putting as it runs 
	DELETE FROM MM_AvailableNumbers WHERE MaskingNumber IN(SELECT DISTINCT MaskingNumber FROM MM_SellerMobileMasking WITH (NOLOCK))
	
	--Set Is Mail for photos
	/*INSERT INTO @TempMainPhotoId
	SELECT LL.Inquiryid FROM LiveListings LL WITH (NOLOCK) 
	INNER JOIN CarPhotos CP WITH(NOLOCK) ON LL.Inquiryid = CP.InquiryId 
								AND LL.SellerType = 1 AND  CP.IsDealer = 1 AND CP.IsActive = 1					
	GROUP BY LL.Inquiryid 
	HAVING 	SUM(CASE ISNULL(CP.IsMain,0) WHEN 1 THEN 1 ELSE 0 END) = 0	
	
	WHILE EXISTS (SELECT TOP 1 Id FROM @TempMainPhotoId)
	BEGIN
	    DECLARE @MainPhotoId INT
		SET @MainPhotoId = (SELECT TOP 1 Id FROM @TempMainPhotoId)
		UPDATE CarPhotos SET IsMain =  1 WHERE Id = (SELECT TOP 1 Id FROM CarPhotos WITH (NOLOCK) WHERE InquiryId = @MainPhotoId AND IsActive = 1 AND IsDealer = 1 ORDER BY Id) 
		DELETE FROM @TempMainPhotoId WHERE Id=@MainPhotoId
	END*/

END
