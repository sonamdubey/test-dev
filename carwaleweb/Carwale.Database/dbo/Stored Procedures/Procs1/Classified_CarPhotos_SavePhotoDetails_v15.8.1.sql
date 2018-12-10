IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[Classified_CarPhotos_SavePhotoDetails_v15]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[Classified_CarPhotos_SavePhotoDetails_v15]
GO

	

-- =============================================
-- Author:		Vikas
-- Create date: 2012/10/10 
-- Description:	This SP used to maintain record of uploaded images with classified sell inquiries
				-- images will get live after verification
				-- seller can upload unlimited images
				-- image will get upload in three sizes i.e. 640x428|300x225|80x60
				-- seller can write description with every image that will be stored in description field.
				-- image name will be in the format [ModelYear_MakeName_ModelName_VersionName_CarPhotoId.jpg]
--Modified by chetan date 5/26/2014 for updation of image url medium
-- =============================================
CREATE PROCEDURE [dbo].[Classified_CarPhotos_SavePhotoDetails_v15.8.1]
	-- Add the parameters for the stored procedure here
	@InquiryId			NUMERIC,
	@FileName			VARCHAR(100),
	@ImageUrlFull		VARCHAR(100),
	@ImageUrlThumb		VARCHAR(100),
	@ImageUrlThumbSmall VARCHAR(100),
	@Description		VARCHAR(200),
	@IsDealer			BIT,
	@IsMain				BIT,
	@DirectoryPath		Varchar(200) = null,
	@HostUrl			VARCHAR(100) = null,
	@PhotoId			NUMERIC	 OUTPUT,
	@IsReplicated		BIT=0
AS
BEGIN
	DECLARE @Status SMALLINT ,
		@FileExtention Varchar(50)  --added by chetan 

	SET @Status = 0 
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	--set @HostUrl = 'img.carwale.com'  -- SD Commented on 12 Dec 2011 as it was temporary 
	if @DirectoryPath is null
	begin
		declare @profileid varchar(50)
		
		if @IsDealer = 1
			set @profileid = 'D' + Convert(varchar(50), @InquiryId)
		else
			set @profileid = 'S' + Convert(varchar(50), @InquiryId)
		
		set @DirectoryPath = '/cw/ucp/' + @profileid + '/'
	end
		
	Declare @isMainCheck bit
	set @isMainCheck=@IsMain
	
	IF NOT EXISTS( SELECT TOP 1 Id FROM CarPhotos WITH(NOLOCK) WHERE InquiryId = @InquiryId AND IsDealer = @IsDealer AND IsActive = 1 AND IsMain = 1 ) 
	BEGIN
		SET @IsMain = 1
	END
	
	-- If Photo is already made main then reset it
	if(@isMainCheck=1)
	BEGIN
		UPDATE CarPhotos SET IsMain=0 WHERE InquiryId = @InquiryId AND IsDealer = @IsDealer AND IsActive = 1 AND IsMain = 1
	END
	
	--INSERT INTO CarPhotos(InquiryId, ImageUrlFull, ImageUrlThumb, ImageUrlThumbSmall, [Description], IsDealer, IsMain, DirectoryPath, HostURL,IsReplicated)
	--VALUES(@InquiryId, @ImageUrlFull, @ImageUrlThumb, @ImageUrlThumbSmall, @Description, @IsDealer, @IsMain, @DirectoryPath, @HostUrl,@IsReplicated)
	
	INSERT INTO CarPhotos(InquiryId, [Description], IsDealer, IsMain, DirectoryPath, HostURL,IsReplicated)
	VALUES(@InquiryId, @Description, @IsDealer, @IsMain, @DirectoryPath, @HostUrl,@IsReplicated)
		
	SET @Status= 1
	SET @PhotoId = SCOPE_IDENTITY()
	SELECT  @FileExtention =  Data from [dbo].[StringSplit](@ImageUrlThumb,'.') WHERE id = 2

	
	UPDATE 
		CarPhotos 
	SET 
		ImageUrlFull = @FileName + '-' + CONVERT(VARCHAR, @PhotoId) + @ImageUrlFull,
		ImageUrlThumb = @FileName + '-' + CONVERT(VARCHAR, @PhotoId) + @ImageUrlThumb,
		ImageUrlThumbSmall = @FileName + '-' + CONVERT(VARCHAR, @PhotoId) + @ImageUrlThumbSmall,
		ImageUrlMedium = @FileName +'-' + CONVERT(VARCHAR, @PhotoId) + '-150x112.'+@FileExtention,
		OriginalImgPath= @DirectoryPath + @FileName +'-' + CONVERT(VARCHAR, @PhotoId) +'.'+@FileExtention,
		StatusId = 1
	WHERE 
		Id = @PhotoId
	
	-- The statements below are to track the progress of the Sell Car Process		
	UPDATE CustomerSellInquiries SET Progress = 90 WHERE ID = @InquiryId
		
END

