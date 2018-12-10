IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[Classified_CarPhotos_Insert]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[Classified_CarPhotos_Insert]
GO

	
-- Modified By: Surendra
-- Modified date: 25 Jan 2012
-- Description: -- IsReplicated field need to update
-- =============================================
-- Author:		Satish Sharma
-- Create date: 2010/08/16 1:48 PM Monday
-- Description:	This SP used to maintain record of uploaded images with classified sell inquiries
				-- images will get live after verification
				-- seller can upload unlimited images
				-- image will get upload in three sizes i.e. 640x428|300x225|80x60
				-- seller can write description with every image that will be stored in description field.
--Modified
-- SD Commented on 12 Dec 2011 as it was temporary 
-- Modified by : Manish on 22-04-2014 added WITH(NOLOCK) keyword wherever not found.

-- Modified By Vivek Gupta on 11-08-2015 , added @OrgImgPath
-- =============================================
CREATE PROCEDURE [dbo].[Classified_CarPhotos_Insert]
	-- Add the parameters for the stored procedure here
	@InquiryId			NUMERIC,
	@ImageUrlFull		VARCHAR(100),
	@ImageUrlThumb		VARCHAR(100),
	@ImageUrlThumbSmall VARCHAR(100),
	@Description		VARCHAR(200),
	@IsDealer			BIT,
	@IsMain				BIT,
	@DirectoryPath		Varchar(200) = null,
	@HostUrl			VARCHAR(100) = null,
	@PhotoId			NUMERIC	OUTPUT,
	@IsReplicated		BIT=NULL,
	@OrgImgPath         VARCHAR(100)=NULL
AS
BEGIN
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
		
		set @DirectoryPath = '/ucp/' + @profileid + '/'
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
	
	INSERT INTO CarPhotos(InquiryId, ImageUrlFull, ImageUrlThumb, ImageUrlThumbSmall, [Description], IsDealer, IsMain, DirectoryPath, HostURL,IsReplicated,IsApproved, OriginalImgPath)
	VALUES(@InquiryId, @ImageUrlFull, @ImageUrlThumb, @ImageUrlThumbSmall, @Description, @IsDealer, @IsMain, @DirectoryPath, @HostUrl,@IsReplicated,1, @OrgImgPath)
		
	SET @PhotoId = SCOPE_IDENTITY()
END











/****** Object:  StoredProcedure [dbo].[TC_SyncStock]    Script Date: 8/14/2015 11:44:52 AM ******/
SET ANSI_NULLS ON
