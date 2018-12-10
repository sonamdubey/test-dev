IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[AbSure_ImageReplication]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[AbSure_ImageReplication]
GO

	
CREATE PROCEDURE [dbo].[AbSure_ImageReplication]
	@CategoryId INT = NULL,
	@CarId VARCHAR(MAX) = NULL
AS
BEGIN
	
	IF @CategoryId = 49 -- AbSure
		BEGIN
		
			--Update The data First
			--UPDATE	Absure_CarPhotos 
			--SET		IsReplicated = 0, StatusId = NULL 
			--WHERE	Absure_cardetailsid  IN(SELECT ListMember FROM fnSplitCSV(@CarId))
			
			--Get The image details
			SELECT		REPLACE(HostUrl + DirectoryPath + ISNULL(ImageUrlOriginal,ImageUrlLarge),'-640x428','') ImageUrl, CD.DealerId, AbSure_CarDetailsId Id, 
						AbSure_CarPhotosId ImgId, P.EntryDate,StatusId, P.DirectoryPath,* 
			FROM		AbSure_CarPhotos P WITH (NOLOCK)
						INNER JOIN AbSure_CarDetails CD WITH (NOLOCK) ON P.AbSure_CarDetailsId = CD.Id
			WHERE		(StatusId <> 3 OR StatusId IS NULL) 
						AND P.IsActive = 1 
						AND IsReplicated=0 
						AND Absure_cardetailsid IN(SELECT ListMember FROM fnSplitCSV(@CarId))
			ORDER BY	P.AbSure_CarDetailsId DESC
			
			--Update Image Versioning
			UPDATE	Absure_CarPhotos 
			SET		ImageUrlOriginal =  ImageUrlOriginal + '?v=2.0', ImageUrlExtraLarge = ImageUrlExtraLarge + '?v=2.0',
					ImageUrlLarge = ImageUrlLarge + '?v=2.0', ImageUrlSmall = ImageUrlSmall + '?v=2.0', ImageUrlThumb = ImageUrlThumb + '?v=2.0'
			WHERE	Absure_cardetailsid  IN(SELECT ListMember FROM fnSplitCSV(@CarId))
 
		END
		
	ELSE IF @CategoryId = 1 -- AutoBiz
	
		BEGIN
			
			
			--Get The image details
			SELECT	 HostURL  + OriginalImgPath ImageUrl
				, S.BranchId DealerId, S.Id Id, P.Id ImgId
						, P.EntryDate,P.StatusId,  P.* 
			FROM		TC_CarPhotos P WITH (NOLOCK)
						INNER JOIN TC_Stock S WITH (NOLOCK) ON P.StockId = S.Id
			WHERE		(P.StatusId <> 3 OR P.StatusId IS NULL) 
						AND P.IsReplicated=0  AND 
						P.IsActive = 1  AND YEAR(P.entryDate) = 2015 AND MONTH(P.entryDate) IN(8,9)
						--AND HostUrl like '%192.168.1.20:9001%' 
						--AND CONVERT(DATE,P.EntryDate) 
						AND S.Id IN(SELECT ListMember FROM fnSplitCSV(@CarId))
			ORDER BY	P.Id DESC
			
			--Update Image Versioning
			UPDATE	TC_CarPhotos 
			SET		OrigFileName =  OrigFileName + '?v=2.0', ImageUrlMedium = ImageUrlMedium + '?v=2.0',
					ImageUrlThumbSmall = ImageUrlThumbSmall + '?v=2.0', ImageUrlThumb = ImageUrlThumb + '?v=2.0', ImageUrlFull = ImageUrlFull + '?v=2.0'
			WHERE	StockId  IN(SELECT ListMember FROM fnSplitCSV(@CarId))
		END
END