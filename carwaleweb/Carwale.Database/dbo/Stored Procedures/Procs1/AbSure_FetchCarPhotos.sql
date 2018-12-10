IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[AbSure_FetchCarPhotos]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[AbSure_FetchCarPhotos]
GO

	-- ==========================================================================================================
-- Author:		Ruchira Patil
-- Create date: 16th Dec 2014
-- Description:	To Fetch AbSure Car Photos
-- Modofied By 1 : Ruchira Patil on 8th Jun 2015 (to fetch Car photo count)
-- Modified By : Suresh Prajapati on 15th June, 2015
-- Description : To fetch ImageUrlExtraLarge as BigImageUrl if exists else select ImageUrlLarge as BigImageUrl
-- Modified By : Vinay Kumar Prajapati 24th June 2015 ... To hide Rc Image as per Requirment .... 
-- Modified By : Ashwini Dhamankar on June 26,2015 (Added @IsButterflyImage,@ImageTagType,@ImageTagId)
-- Modified By : Ashwini Dhamankar on Aug 26,2015 (Avoided 2 images of same imagecaption or imagetagid)
-- Modified By : Ashwini Dhamankar on Aug 28,2015 (Handle the condition if imagecaption is empty )
-- Modified By : Nilima More on 16th sept,2015 (Show multiple images which are uploaded by dealer and one image which are uploaded by surveyor)
-- ===========================================================================================================
CREATE PROCEDURE [dbo].[AbSure_FetchCarPhotos]
 (@AbSure_CarDetailsId INT,
 @AbSure_HideRCImage BIT = 0, -- 1 for to hide (Image Caption) 'RC' Image only and  0 for all images
 @IsButterflyImage BIT = 0 ,
 @ImageTagType INT = NULL,
 @ImageTagId INT = NULL
 )
AS

DECLARE @DealerId INT = NULL,@PhotoCount INT = NULL,@PhotoSynced INT = NULL

BEGIN
-- SET NOCOUNT ON added to prevent extra result sets from
-- interfering with SELECT statements.
	SET NOCOUNT ON;
	WITH CTE1 AS
	(
		SELECT T.* FROM
		(
			SELECT AbSure_CarPhotosId,ImageUrlOriginal
			,ImageUrlLarge
			,ImageUrlThumb
			,ImageUrlSmall
			,DirectoryPath
			,ImageCaption
			,IsMain
			,IsChassisImage
			,HostUrl
			,ISNULL(ImageUrlExtraLarge, ImageUrlLarge) AS BigImageUrl
			,TC_UserId
			,StatusId
			--,CASE WHEN (ImageTagId IS NOT NULL OR (ImageCaption IS NOT NULL AND LEN(ImageCaption)>0)) THEN ROW_NUMBER() OVER (PARTITION BY ImageTagId,ImageCaption ORDER BY EntryDate DESC) ELSE 1 END RowNum 
			,CASE UR.RoleId WHEN 13 THEN ROW_NUMBER() OVER (PARTITION BY ImageCaption,UR.UserId ORDER BY P.EntryDate DESC) ELSE ROW_NUMBER() OVER (PARTITION BY AbSure_CarPhotosId ORDER BY P.EntryDate DESC) END RowNum --TC_USERID=13 (SURVEYOR) 
			,ImageTagType
			FROM AbSure_CarPhotos P WITH (NOLOCK)
			LEFT JOIN TC_Users TC WITH (NOLOCK) ON TC.Id = P.TC_UserId
			LEFT JOIN TC_UsersRole UR WITH(NOLOCK) ON UR.UserId = TC.Id
			WHERE P.AbSure_CarDetailsId = @AbSure_CarDetailsId
			AND (P.ImageTagType IN (1, 2, 3) OR P.ImageTagType IS NULL)
			AND  ISNULL(P.ImageTagId,0) <> CASE @AbSure_HideRCImage WHEN 1 THEN  1 ELSE -1 END			-- if 1 then filter RC Image , if 0 then take all images 
			AND P.IsActive = 1
		)  T
		WHERE T.RowNum = 1
	)

	SELECT * INTO #TempTable
	FROM CTE1

	IF(ISNULL(@IsButterflyImage,0) = 0)
	BEGIN
		SELECT AbSure_CarPhotosId,ImageUrlOriginal
				,ImageUrlLarge
				,ImageUrlThumb
				,ImageUrlSmall
				,DirectoryPath
				,ImageCaption
				,IsMain
				,IsChassisImage
				,HostUrl
				,BigImageUrl
		FROM #TempTable
		WHERE ImageTagType IN(2,3) OR ImageTagType IS NULL
	END

	ELSE IF(@IsButterflyImage = 1)
	BEGIN
		SELECT AbSure_CarPhotosId
		,ImageUrlOriginal
		,ImageUrlLarge                                                         
		,ImageUrlThumb
		,ImageUrlSmall
		,DirectoryPath
		,ImageCaption
		,IsMain
		,IsChassisImage
		,HostUrl
		,ISNULL(ImageUrlExtraLarge, ImageUrlLarge) AS BigImageUrl
		FROM AbSure_CarPhotos P WITH(NOLOCK)
		WHERE 
		P.AbSure_CarDetailsId = @AbSure_CarDetailsId  AND
		P.ImageTagType = @ImageTagType AND
		P.ImageTagId = @ImageTagId AND
		P.IsActive = 1
		ORDER BY EntryDate DESC
	END
	
	SELECT @DealerId = DealerId,@PhotoCount = PhotoCount  FROM AbSure_CarDetails WITH (NOLOCK)   WHERE Id = @AbSure_CarDetailsId

	SELECT @PhotoSynced = COUNT(AbSure_CarPhotosId)  
	FROM #TempTable 
	WHERE StatusId = 3
		AND TC_UserId = (
			SELECT TC_UserId
			FROM AbSure_CarSurveyorMapping
			WHERE AbSure_CarDetailsId = @AbSure_CarDetailsId
			)

	SELECT @DealerId DealerId,@PhotoCount PhotoCount,@PhotoSynced PhotoSynced

	DROP TABLE #TempTable
END