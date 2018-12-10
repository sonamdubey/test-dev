IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[Microsites_InsertImageUploadData_V15]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[Microsites_InsertImageUploadData_V15]
GO

	-- =============================================
-- Author:		Umesh Ojha
-- Create date : 27/2/2011
-- Description :	Insering Data to the table when image uploaded
-- Modified By : Nilesh Utture on 25th October, 2013 Added parameter and Inserted @ImageCategoryId.
-- Modified By : Tejashree Patil on 11 Sept 2014, Added parameters @ImgPathCustom600, @ImgPathCustom300
-- Modified By : Vivek Gupta on 11-11-2014, made inactive old profile image of dealer if he uploads new profile image
-- Modified By : Komal Manjare on 7th August 2015, @OriginalImgPath parameter added
-- Modified by : Sanjay soni Added IsActive with default value false
-- =============================================				  
CREATE PROCEDURE [dbo].[Microsites_InsertImageUploadData_V15.12.1] 
	-- Add the parameters for the stored procedure here
	(
		@DealerId Int,
		@ThumbImage Varchar(50)= null,
		@LargeImage varchar(50)= null,
		@EntryDate Datetime,
		@DirectoryPath varchar(50),
		@HostURL Varchar(50)=null,
		@IsBanner bit = null,		
		@LandingBanner varchar(200) = null,
		@ImageCategoryId INT = NULL,
		@ImgPathCustom600 Varchar(50)= null,
		@ImgPathCustom300 Varchar(50)= null,
		@OriginalImgPath VARCHAR(300)=NULL
	)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.	
	declare @BannerSorting int
	if(@IsBanner=1)
		begin
			set @BannerSorting = (select COUNT(id) from Microsite_Images WITH (NOLOCK) where DealerId=@DealerId and IsBanner=1 and IsActive=1)
		end
	else
		begin
			set @BannerSorting = null
		end

    if(@ImageCategoryId = 5)
	BEGIN  
	   UPDATE Microsite_Images SET IsActive = 0 WHERE DealerId = @DealerId AND ImageCategoryId = 5
	END 
		
	SET NOCOUNT ON;
    -- Insert statements for procedure here
		Insert Into Microsite_Images (DealerId,ThumbImage,LargeImage,EntryDate,
		DirectoryPath,HostURL,IsBanner,BannerImgSortingOrder,LandingURL, ImageCategoryId, ImgPathCustom600, ImgPathCustom300,OriginalImgPath,IsActive) values 
		(@DealerId,@ThumbImage,@LargeImage,@EntryDate,@DirectoryPath,@HostURL,
		@IsBanner,@BannerSorting,@LandingBanner, @ImageCategoryId, @ImgPathCustom600, @ImgPathCustom300,@OriginalImgPath,0 )
		
		SELECT SCOPE_IDENTITY()  

		
END