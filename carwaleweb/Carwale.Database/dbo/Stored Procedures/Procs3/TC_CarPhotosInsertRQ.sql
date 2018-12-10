IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_CarPhotosInsertRQ]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_CarPhotosInsertRQ]
GO

	
-- Author		:	Chetan Dev
-- Create date	:	01/03/2013 15:58:26 
-- Description	:	This SP used to maintain record of uploaded images in the Trading Cars Software       
--					image will get upload in three sizes i.e. 640x428|300x225|80x60 ,this record initially will inactive       
--modified by chetan added mediumurlimage
-- Modified By Vivek Gupta on 12-08-2015, added OriginalImgPath
-- Modified By Vaibhav K 25 Oct 2016 To solve multiple main image issue
-- =============================================    
CREATE PROCEDURE [dbo].[TC_CarPhotosInsertRQ]
	-- Add the parameters for the stored procedure here    
	@StockId BIGINT
	,@FileExtension VARCHAR(100)
	,@IsMain BIT
	,@DirPath VARCHAR(200)
	,@PhotoId BIGINT OUTPUT
	,@ImageName VARCHAR(100) OUTPUT
	,@UrlThumbSmall VARCHAR(100) OUTPUT
	,@HostUrl VARCHAR(100)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from    
	-- interfering with SELECT statements.    
	SET NOCOUNT ON;

	DECLARE @ImageUrlFull VARCHAR(100)
		,@ImageUrlThumb VARCHAR(100)
		,@ImageUrlThumbSmall VARCHAR(100)
		,@ImageUrlMedium VARCHAR(100)
		,--added by chetan 
		@CarName VARCHAR(100)
		,@InquiryId NUMERIC
		,@CarMake VARCHAR(100)
		,@CarModel VARCHAR(100)
		,@CarVersion VARCHAR(100)
		,@MakeYear VARCHAR(20)
		,@OrgImgPath VARCHAR(250)

	SELECT @MakeYear = CONVERT(CHAR(4), S.MakeYear, 120)
		,@CarMake = REPLACE(V.Make, ' ', '-')
		,@CarModel = REPLACE(V.Model, ' ', '-')
		,@CarVersion = REPLACE(V.Version, ' ', '-')
	FROM TC_Stock S WITH (NOLOCK)
	INNER JOIN vwMMV V ON S.VersionId = V.VersionId
	WHERE S.Id = @StockId

	SELECT @CarMake = dbo.ReplaceCharacters(@CarMake, 'a-z0-9-')

	SELECT @CarModel = dbo.ReplaceCharacters(@CarModel, 'a-z0-9-')

	SELECT @CarVersion = dbo.ReplaceCharacters(@CarVersion, 'a-z0-9-')

	/*
SET @CarModel=REPLACE(@CarModel,'+','')
SET @CarModel=REPLACE(@CarModel,'/','')
SET @CarModel=REPLACE(@CarModel,'\','')
SET @CarModel=REPLACE(@CarModel,'*','')
SET @CarModel=REPLACE(@CarModel,'(','')
SET @CarModel=REPLACE(@CarModel,')','')
SET @CarModel=REPLACE(@CarModel,'.','')
SET @CarModel=REPLACE(@CarModel,'--','-')

SET @CarVersion=REPLACE(@CarVersion,'+','')
SET @CarVersion=REPLACE(@CarVersion,'/','')
SET @CarVersion=REPLACE(@CarVersion,'\','')
SET @CarVersion=REPLACE(@CarVersion,'*','')
SET @CarVersion=REPLACE(@CarVersion,'(','')
SET @CarVersion=REPLACE(@CarVersion,')','')
SET @CarVersion=REPLACE(@CarVersion,'.','')
SET @CarVersion=REPLACE(@CarVersion,'--','-')
*/
	SET @CarName = ISNULL(@MakeYear, '') + '-' + ISNULL(@CarMake, '') + '-' + ISNULL(@CarModel, '') + '-' + ISNULL(@CarVersion, '') --IsNull check : Added By Chetan Navin on 10 Aug 2015
	SET @CarName = REPLACE(@CarName, '--', '')
	SET @OrgImgPath = @DirPath

	--SELECT @CarName=CONVERT(CHAR(4), S.MakeYear, 120) + '-' +REPLACE(V.Make,' ','-') + '-' + REPLACE(V.Model,' ','-') + '-' + REPLACE(V.Version,' ','-')
	----CONVERT(CHAR(4), S.MakeYear, 120) + '-' + V.Make + '-' + V.Model + '-' + V.Version
	--FROM TC_Stock S INNER JOIN vwMMV V ON S.VersionId=V.VersionId
	--WHERE S.Id=@StockId	
	IF NOT EXISTS (
			SELECT TOP 1 Id
			FROM TC_CarPhotos WITH (NOLOCK)
			WHERE StockId = @StockId
				AND IsActive = 1
				AND IsMain = 1
			)
	BEGIN -- if main image is not set
		SET @IsMain = 1
	END

	--inserting record with inactive status,later once image will save in appropriate folder need to activate
	INSERT INTO TC_CarPhotos (
		StockId
		,ImageUrlFull
		,ImageUrlThumb
		,ImageUrlThumbSmall
		,IsMain
		,DirectoryPath
		,HostUrl
		,IsActive
		,StatusId
		,OriginalImgPath
		)
	VALUES (
		@StockId
		,@CarName
		,@CarName
		,@CarName
		,@IsMain
		,@DirPath
		,@HostUrl
		,1
		,1
		,@OrgImgPath
		)

	SET @PhotoId = SCOPE_IDENTITY()
	SET @ImageName = @CarName + '-' + CAST(@PhotoId AS VARCHAR)
	SET @ImageName = REPLACE(@ImageName, '--', '') --Added By Chetan Navin on 10 Aug 2015
	SET @ImageUrlFull = @ImageName + '-640x428' + @FileExtension
	SET @ImageUrlThumb = @ImageName + '-300x225' + @FileExtension
	SET @ImageUrlThumbSmall = @ImageName + '-80x60' + @FileExtension
	SET @ImageUrlMedium = @ImageName + '-150x112' + @FileExtension --added by chetan dev 
	SET @UrlThumbSmall = 'http://' + @HostUrl + '/' + @DirPath + @ImageUrlThumbSmall
	SET @OrgImgPath = @DirPath + @ImageName + @FileExtension;

	---- updating final image name
	UPDATE TC_CarPhotos
	SET ImageUrlFull = @ImageUrlFull
		,ImageUrlThumb = @ImageUrlThumb
		,ImageUrlThumbSmall = @ImageUrlThumbSmall
		,OrigFileName = @ImageName
		,ImageUrlMedium = @ImageUrlMedium
		,OriginalImgPath = @OrgImgPath
	WHERE Id = @PhotoId --modified by chetan dev 

	--Vaibhav K 25 Oct 2016 To solve multiple main image issue
	IF @IsMain = 1
		UPDATE TC_CarPhotos
		SET IsMain = 0
		WHERE StockId = @StockId
			AND Id <> @PhotoId
END
