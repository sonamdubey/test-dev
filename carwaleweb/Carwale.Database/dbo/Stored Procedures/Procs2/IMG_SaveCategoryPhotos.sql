IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[IMG_SaveCategoryPhotos]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[IMG_SaveCategoryPhotos]
GO

	-- =============================================
-- Author      : Chetan Navin
-- Create date : 16th Oct 2015
-- Description : To save image details in database on the basis of category id
-- Modified By : Chetan Navin on 29th Dec 2015 (Removed '/' prepended on orgImgPath of CarPhotos table)
-- Modified : Vaibhav K 29-Dec-2015
-- Added conditions for new categories requied for DCRM - ChequeDDPDC, LPA, DepsoitSlip
-- Modified By : Chetan Navin 12-Dec-2015 (Changed query in CategoryId 3)
-- Modified By : Vaibhav K 25-Jan-2016 (For Cat-76 cheque photos) change update logic to get unique id for payment of that transaction
-- Modified By : Vaibhav K 14-Mar-2016 for category-76 change logic to update the specific PaymentDetailsId as ItemId and image paths for the same
-- To get PaymentDetails only in payment Mode (2,3,8) i.e. (cheque,DD,PDC) and payments that are not rejected
-- Modified By : Chetan Navin 15-Mar-2018 (In Category Id 1,added query to set main photo)
-- Modified By : Vaibhav K 18 Apr 2016 For Category = 78 (LPA) updated the IsActive = 1, UpdatedOn with getdate
-- Modified By : Suresh Prajapati on 16th June, 2016
-- Description : To Update DMSScreenShotUrl instead of OriginalImgPath for @CategoryId=9
-- Modified By : Komal Manjare on 07-09-2016 
-- Desc : update ticketimages and related tables/colum
-- Modified By : Supreksha Songh on 29-09-2016
-- Description : To add images for sponsored campaigns
-- Modified by manish on 05-10-2016 commented if block for Microsite_ServiceOffers and DOA_Offers as tables not exist on production
-- =============================================
CREATE PROCEDURE [dbo].[IMG_SaveCategoryPhotos] 			  
	 @ReqPhotoId      INT,							--Id from IMG_Photos Table
	 @CategoryId      TINYINT,
	 @ItemId          INT,
	 @IsMain          BIT, 
	 @HostUrl         VARCHAR(200),
	 @OriginalPath	  VARCHAR(200),
	 @ImageId         INT = NULL OUTPUT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	DECLARE @ImgUrlMedium VARCHAR(100),@PhotoGalleryId AS TINYINT
	DECLARE @PhotoId INT
	DECLARE @CWPhotoId INT
	BEGIN TRY 
		--mysql sync
	declare
	@Name varchar(30) =null,
	@CarMakeId numeric(18, 0) =null,
	@IsDeleted bit =null,
	@MoCreatedOn datetime =null,
	@MoUpdatedOn datetime =null,
	@MoUpdatedBy numeric(18, 0) =null,
	@Used bit =null,
	@New bit =null,
	@Indian bit =null,
	@Imported bit =null,
	@Classic bit =null,
	@Futuristic bit =null,
	@Modified bit =null,
	@Id int =null,
	@SmallPic varchar(200) =null,
	@LargePic varchar(200) =null,
	@UpdateType INT =null,
	@DiscontinuitionId int =null,
	@ReplacedByModelId smallint =null,
	@comment Varchar(max) = null,
	@Discontinuition_date datetime = null,
	@Maskingname varchar(50) = null,
	@RootId smallint =null,
	@Platform varchar(50)=null, 
	@Generation tinyint=null, 
	@Upgrade tinyint =null, 
	@ModelLaunchDate datetime =null,
	@SubSegmentId int = null,
	@CarVersionID_Top int =null,
	@IsSolidColor bit =null,
	@IsMetallicColor bit =null,
	@MinPrice int =null, 
	@MaxPrice int =null,
	@ReviewRate decimal = null,   
	@Looks decimal = null,   
	@Comfort decimal = null,   
	@Performance decimal = null,   
	@ValueForMoney decimal = null,   
	@FuelEconomy decimal = null,   
	@ReviewCount decimal = null,
	@Summary varchar(max) = null,
	@OriginalImgPath varchar(150)= null,
	@XLargePic varchar(150)=null,
	@CV_ID int = null,
	@SelId varchar(8000)=null,
	@CarModelId numeric = null
			declare   @SegmentId numeric(18, 0)  = null, @BodyStyleId numeric(18, 0)  = null,  @Discontinuation_date datetime =null, @LaunchDate datetime = null, @ReplacedByVersionId smallint = null,  @VUpdatedBy numeric = null, @VUpdatedOn datetime = null, @CarFuelType tinyint = null, @CarTransmission tinyint = null,@Environment varchar(150) =null 
		--mysql sync end
	--Trading Cars
	IF(@CategoryId = 1)
		BEGIN
		    DECLARE @ImageUrlThumb  VARCHAR(200) = '310X174' + @OriginalPath,
			@ImageUrlThumbSmall VARCHAR(200) = '110X61' + @OriginalPath,
			@ImageUrlMedium VARCHAR(200) = '160X89' + @OriginalPath,
			@OrigFileName VARCHAR(200) = '0X0' + @OriginalPath, 
			@ImageUrlFull VARCHAR(200) = '664X374' + @OriginalPath
			
			--if there is already main image don't set it again
			--Added By Deepak on 26th Aug 2016
			SELECT Id FROM TC_CarPhotos WITH (NOLOCK) WHERE ISNULL(IsMain,0) = 1 AND StockId = @ItemId AND IsActive = 1
			IF @@ROWCOUNT > 0
				SET @IsMain = 0
							
			--Step 1 : Insert in table based on category
			INSERT INTO TC_CarPhotos (StockId,IsMain,IsActive,HostUrl,IsReplicated,StatusId,OriginalImgPath,DirectoryPath,OrigFileName,
			                          ImageUrlThumb,ImageUrlMedium,ImageUrlThumbSmall,ImageUrlFull)
			VALUES(@ItemId,@IsMain,1,@HostUrl,1,3,@OriginalPath,'',@OrigFileName,
					@ImageUrlThumb,@ImageUrlMedium,@ImageUrlThumbSmall,
					@ImageUrlFull)
            
			SET @PhotoId = SCOPE_IDENTITY()
            SET @ImageId = @PhotoId
            --Commented By Deepak on 26th Aug 2016
			--Update main photo 
			--Step 2
			--IF(@IsMain = 1)
			--BEGIN
			--	UPDATE TC_CarPhotos SET IsMain = 0 WHERE StockId = @ItemId AND Id <> @PhotoId AND IsActive = 1
			--END
			
			--Step 3 : Update main table 
			UPDATE IMG_Photos SET IsProcessed = 1,HostUrl = @HostUrl,OriginalPath = @OriginalPath,ProcessedId = @PhotoId,ProcessedOn = GETDATE()
			WHERE Id = @ReqPhotoId
			--DECLARE @InquiryId INT			
	
			--SELECT @InquiryId = Id FROM SellInquiries Si WITH(NOLOCK) WHERE Si.Tc_StockId = @ItemId AND StatusId = 1 AND SourceId = 2
			----Inserting the car photos details in the CarPhotos table
			--IF (@InquiryId IS NOT NULL)
			--	BEGIN
			--		IF NOT EXISTS (SELECT Id FROM CarPhotos WITH(NOLOCK) WHERE TC_CarPhotoId = @PhotoId)
			--		BEGIN
			--			-- AM Modified 6-11-2012 to insert PhotoId from TC_CarPhotos to CarPhotos
			--			INSERT INTO CarPhotos(InquiryId,ImageUrlFull,ImageUrlThumb,ImageUrlThumbSmall,IsDealer,IsMain,IsActive,IsApproved,DirectoryPath, HostUrl,IsReplicated,TC_CarPhotoId,ImageUrlMedium, OriginalImgPath)
			--			VALUES(@InquiryId,@ImageUrlFull,@ImageUrlThumb,@ImageUrlThumbSmall,1,@IsMain,1,1,NULL, @HostUrl,1,@PhotoId,@ImgUrlMedium, @OriginalPath)  
			--		END
			--		--ELSE 
			--		--    UPDATE CarPhotos SET
			--		--	ImageUrlFull = @ImageUrlFull
			--		--	,ImageUrlThumb = @ImageUrlThumb
			--		--	,ImageUrlThumbSmall = @ImageUrlThumbSmall					
			--		--	,IsMain = @IsMain						
			--		--	,DirectoryPath = NULL
			--		--	, HostUrl = @HostUrl											
			--		--	,ImageUrlMedium = @ImgUrlMedium
			--		--	, OriginalImgPath = @OriginalPath
			--		--	WHERE InquiryId = @InquiryId AND TC_CarPhotoId = @ReqPhotoId
			--	END
		END
    -- Edit CMS
	IF(@CategoryId= 2)
		BEGIN
			--INSERT INTO Con_EditCms_Images(IsActive,HostUrl,StatusId,IsReplicated,OriginalImgPath) VALUES(1,@HostUrl,3,1,@OriginalPath) 
			--SET @PhotoId = SCOPE_IDENTITY()
			UPDATE Con_EditCms_Images SET IsActive= 1,HostUrl= @HostUrl,StatusId=3,IsReplicated=1,OriginalImgPath=@OriginalPath
			WHERE Id = @ItemId
			UPDATE IMG_Photos SET IsProcessed = 1,HostUrl = @HostUrl,OriginalPath = @OriginalPath,ProcessedId = @ItemId,ProcessedOn = GETDATE()
			WHERE Id = @ReqPhotoId
			UPDATE CM 
			SET CM.OriginalImgPath = EI.OriginalImgPath --Added By Ashwini Todkar on 9 July 2015, updated image path to carmodels
			,CM.HostURL = EI.HostURL
			,CM.IsReplicated = 1
			FROM CarModels CM   WITH(NOLOCK)
			INNER JOIN Con_EditCms_Images EI WITH(NOLOCK) ON CM.ID = EI.ModelId
			INNER JOIN Con_EditCms_Basic EB WITH(NOLOCK) ON EB.Id = EI.BasicId
			WHERE EI.IsMainImage = 1  AND EB.CategoryId = 10 AND EB.ApplicationID = 1 
				AND EI.Id = @ItemId -- For CarWale -- 10 for Photo Gallery
		
		set @Id=@ItemId
		set @UpdateType=19
				
		--mysql sync
	begin try
	exec [dbo].[SyncCarModelsWithMysqlUpdate]	
		@Name ,
		@CarMakeId,
		@IsDeleted,
		@MoCreatedOn,
		@MoUpdatedOn,
		@MoUpdatedBy,
		@Used ,
		@New ,
		@Indian ,
		@Imported ,
		@Classic ,
		@Futuristic ,
		@Modified ,
		@Id,
		@SmallPic ,
		@LargePic ,
		@HostUrl ,
		@UpdateType ,
		@DiscontinuitionId  ,
		@ReplacedByModelId  ,
		@comment,
		@Discontinuition_date ,
		@Maskingname ,
		@RootId  ,
		@Platform , 
		@Generation , 
		@Upgrade  , 
		@ModelLaunchDate  ,
		@SubSegmentId ,
		@CarVersionID_Top  ,
		@IsSolidColor  ,
		@IsMetallicColor  ,
		@MinPrice  , 
		@MaxPrice  ,
		@ReviewRate ,   
		@Looks ,   
		@Comfort ,   
		@Performance ,   
		@ValueForMoney ,   
		@FuelEconomy ,   
		@ReviewCount ,
		@Summary,
		@OriginalImgPath ,
		@XLargePic ,
		@CV_ID,
		@SelId,
		@CarModelId
			end try
			BEGIN CATCH
				INSERT INTO CarWaleMyslSyncExceptions(ModuleName,SPName,ErrorMsg,TableName,FailedId,CreatedOn,InputParameter)
				VALUES('MysqlSync','IMG_SaveCategoryPhotos',ERROR_MESSAGE(),'SyncCarModelsWithMysqlUpdate',@Id,GETDATE(),@UpdateType)
			END CATCH
	--mysql sync end
			-- Added by Ashwini Todkar  on 9 July 2015 to Update version image paths same as model image paths
			UPDATE CV
			SET CV.OriginalImgPath = EI.OriginalImgPath  --Added By Ashwini Todkar on 9 July 2015, saved small image path to carversions
				,CV.HostURL = EI.HostURL
				,CV.IsReplicated = 1
			FROM CarVersions CV WITH (NOLOCK)
			JOIN Con_EditCms_Images EI WITH (NOLOCK) ON CV.CarModelId = EI.ModelId
			INNER JOIN Con_EditCms_Basic EB WITH(NOLOCK) ON EB.Id = EI.BasicId
			WHERE EI.Id = @ItemId AND EI.IsMainImage = 1  AND EB.CategoryId = 10 AND EB.ApplicationID = 1 
				AND CV.SpecialVersion = 0 --Special version having different image than model image
  			--sync with mysql
			set @UpdateType = 5
			begin try
			exec [dbo].[SyncCarVersionsWithMysqlUpdate] 
				@ItemId , @Name , @SegmentId , @BodyStyleId , @Used , @New , @IsDeleted , @Indian , @Imported , @Futuristic , @Classic , @Modified ,		 @Discontinuation_date ,	 @MaskingName , @SubSegmentId , @LaunchDate , @DiscontinuitionId , @ReplacedByVersionId , @Comment , @VUpdatedBy , @VUpdatedOn , @CarFuelType , @CarTransmission , @HostUrl , @UpdateType , @Environment , @OriginalImgPath , @SmallPic , @LargePic	
			
			
			end try
			BEGIN CATCH
				INSERT INTO CarWaleMyslSyncExceptions(ModuleName,SPName,ErrorMsg,TableName,FailedId,CreatedOn,InputParameter)
				VALUES('MysqlSync','IMG_SaveCategoryPhotos',ERROR_MESSAGE(),'SyncCarVersionsWithMysqlUpdate',@ItemId,GETDATE(),@UpdateType)
			END CATCH
			
			--mysql sync end
		END	
    
	--carversion 
	IF(@CategoryId = 60)
		BEGIN
			UPDATE CarVersions SET IsReplicated = 1 , 
								   HostUrl = @HostUrl,
								   OriginalImgPath = OriginalImgPath
			WHERE Id=@ItemId 
			set @UpdateType = 8
			begin try
			exec [dbo].[SyncCarVersionsWithMysqlUpdate] 
				@ItemId , @Name , @SegmentId , @BodyStyleId , @Used , @New , @IsDeleted , @Indian , @Imported , @Futuristic , @Classic , @Modified ,		 @Discontinuation_date ,	 @MaskingName , @SubSegmentId , @LaunchDate , @DiscontinuitionId , @ReplacedByVersionId , @Comment , @VUpdatedBy , @VUpdatedOn , @CarFuelType , @CarTransmission , @HostUrl , @UpdateType , @Environment , @OriginalImgPath , @SmallPic , @LargePic	
			
			end try
			BEGIN CATCH
				INSERT INTO CarWaleMyslSyncExceptions(ModuleName,SPName,ErrorMsg,TableName,FailedId,CreatedOn,InputParameter)
				VALUES('MysqlSync','IMG_SaveCategoryPhotos',ERROR_MESSAGE(),'SyncCarVersionsWithMysqlUpdate',@ItemId,GETDATE(),@UpdateType)
			END CATCH
			--mysql sync end
			--Step 2 : Update main table 
			UPDATE IMG_Photos SET IsProcessed = 1,HostUrl = @HostUrl,OriginalPath = @OriginalPath,ProcessedId = @ItemId,ProcessedOn = GETDATE()
			WHERE Id = @ReqPhotoId
		END
    -- expectedlaunch
	IF(@CategoryId = 61)
		BEGIN
			UPDATE CarModels SET HostURL = @HostUrl , 
								 IsReplicated =1,
								 OriginalImgPath = OriginalImgPath
			WHERE ID = @ItemId
			
		set @Id=@ItemId
		set @UpdateType=22
				
		--mysql sync
		begin try
	exec [dbo].[SyncCarModelsWithMysqlUpdate]	
		@Name ,
		@CarMakeId,
		@IsDeleted,
		@MoCreatedOn,
		@MoUpdatedOn,
		@MoUpdatedBy,
		@Used ,
		@New ,
		@Indian ,
		@Imported ,
		@Classic ,
		@Futuristic ,
		@Modified ,
		@Id,
		@SmallPic ,
		@LargePic ,
		@HostUrl ,
		@UpdateType ,
		@DiscontinuitionId  ,
		@ReplacedByModelId  ,
		@comment,
		@Discontinuition_date ,
		@Maskingname ,
		@RootId  ,
		@Platform , 
		@Generation , 
		@Upgrade  , 
		@ModelLaunchDate  ,
		@SubSegmentId ,
		@CarVersionID_Top  ,
		@IsSolidColor  ,
		@IsMetallicColor  ,
		@MinPrice  , 
		@MaxPrice  ,
		@ReviewRate ,   
		@Looks ,   
		@Comfort ,   
		@Performance ,   
		@ValueForMoney ,   
		@FuelEconomy ,   
		@ReviewCount ,
		@Summary,
		@OriginalImgPath ,
		@XLargePic ,
		@CV_ID,
		@SelId,
		@CarModelId
	--mysql sync end
	
			end try
			BEGIN CATCH
				INSERT INTO CarWaleMyslSyncExceptions(ModuleName,SPName,ErrorMsg,TableName,FailedId,CreatedOn,InputParameter)
				VALUES('MysqlSync','IMG_SaveCategoryPhotos',ERROR_MESSAGE(),'SyncCarModelsWithMysqlUpdate',@Id,GETDATE(),@UpdateType)
			END CATCH
			--Step 2 : Update main table 
			UPDATE IMG_Photos SET IsProcessed = 1,HostUrl = @HostUrl,OriginalPath = @OriginalPath,ProcessedId = @ItemId,ProcessedOn = GETDATE()
			WHERE Id = @ReqPhotoId
		END
	--featuredlisting
	IF(@CategoryId = 62)
		BEGIN
			UPDATE Con_FeaturedListings SET HostUrl = @HostUrl, 
											IsReplicated =1
			WHERE CarId = @ItemId
			--Step 2 : Update main table 
			UPDATE IMG_Photos SET IsProcessed = 1,HostUrl = @HostUrl,OriginalPath = @OriginalPath,ProcessedId = @ItemId,ProcessedOn = GETDATE()
			WHERE Id = @ReqPhotoId
		END
    --carcomparisionlist
	IF(@CategoryId = 63)
		BEGIN
			UPDATE Con_CarComparisonList SET HostURL = @HostUrl,
											OriginalImgPath =  OriginalImgPath, 
											 IsReplicated = 1
			WHERE ID = @ItemId
			--Step 2 : Update main table 
			UPDATE IMG_Photos SET IsProcessed = 1,HostUrl = @HostUrl,OriginalPath = @OriginalPath,ProcessedId = @ItemId,ProcessedOn = GETDATE()
			WHERE Id = @ReqPhotoId
		END
	--topsellingcars	
	IF(@CategoryId = 64)
		BEGIN
			UPDATE Con_TopSellingCars SET HostURL=@HostUrl,
										  OriginalImgPath = OriginalImgPath,
										  IsReplicated =1
			WHERE ModelId = @ItemId
			--Step 2 : Update main table 
			UPDATE IMG_Photos SET IsProcessed = 1,HostUrl = @HostUrl,OriginalPath = @OriginalPath,ProcessedId = @ItemId,ProcessedOn = GETDATE()
			WHERE Id = @ReqPhotoId
		END
	--addbrand
	IF(@CategoryId = 65)
		BEGIN
			UPDATE Acc_Brands SET HostURL=@HostUrl,IsReplicated =1
			WHERE Id = @ItemId
			--Step 2 : Update main table 
			UPDATE IMG_Photos SET IsProcessed = 1,HostUrl = @HostUrl,OriginalPath = @OriginalPath,ProcessedId = @ItemId,ProcessedOn = GETDATE()
			WHERE Id = @ReqPhotoId
		END
	--additems
	IF(@CategoryId = 66)
		BEGIN
			UPDATE Acc_Items SET HostURL=@HostUrl, 
								 IsReplicated =1
			WHERE Id = @ItemId
			--Step 2 : Update main table 
			UPDATE IMG_Photos SET IsProcessed = 1,HostUrl = @HostUrl,OriginalPath = @OriginalPath,ProcessedId = @ItemId,ProcessedOn = GETDATE()
			WHERE Id = @ReqPhotoId
		END
	--additionalitems
	IF(@CategoryId = 67)
		BEGIN
			INSERT INTO Acc_ItemsAdditionalImages(ItemId,HostURL,IsReplicated) VALUES(@ItemId,@HostUrl,1) 
			SET @PhotoId = SCOPE_IDENTITY()
			--Step 2 : Update main table 
			UPDATE IMG_Photos SET IsProcessed = 1,HostUrl = @HostUrl,OriginalPath = @OriginalPath,ProcessedId = @PhotoId,ProcessedOn = GETDATE()
			WHERE Id = @ReqPhotoId
		END
	--showroomphotos
	IF(@CategoryId = 68)
		BEGIN
			INSERT INTO ShowRoomPhotos(HostURL,IsReplicated,DirectoryPath,Thumbnail,LargeImage,OriginalImgPath) 
			VALUES(@HostUrl,1,'','/144X81/' + @OriginalPath,'/559X314/' + @OriginalPath,  @OriginalPath)
			SET @PhotoId = SCOPE_IDENTITY()
            
			--Step 2 : Update main table 
			UPDATE IMG_Photos SET IsProcessed = 1,HostUrl = @HostUrl,OriginalPath = @OriginalPath,ProcessedId = @PhotoId,ProcessedOn = GETDATE()
			WHERE Id = @ReqPhotoId
		END
	--dealercertification
	IF(@CategoryId = 69)
		BEGIN
			--OriginalImgPath Added By Deepak on 5th Aug 2015
			UPDATE Microsite_Images SET HostURL = @HostUrl, 
										StatusId = 3,
										IsReplicated = 1,
										OriginalImgPath =  OriginalImgPath
			WHERE Id = @ItemId
            
			--Step 2 : Update main table 
			UPDATE IMG_Photos SET IsProcessed = 1,HostUrl = @HostUrl,OriginalPath = @OriginalPath,ProcessedId = @ItemId,ProcessedOn = GETDATE()
			WHERE Id = @ReqPhotoId		
		END
	--Sell Car Photos
	IF(@CategoryId = 3)
		BEGIN
			SET @ImageUrlThumb   = '310X174' + @OriginalPath
			SET @ImageUrlThumbSmall  = '110X61' + @OriginalPath
			SET	@ImageUrlMedium  = '160X89' + @OriginalPath
			SET	@OrigFileName  = '0X0' + @OriginalPath
			SET	@ImageUrlFull  = '664X374' + @OriginalPath
		    INSERT INTO TC_SellCarPhotos (TC_SellerInquiriesId,IsActive,IsMain,HostUrl,StatusId,IsReplicated,OriginalImgPath,DirectoryPath,ImageUrlThumb,ImageUrlThumbSmall,ImageUrlFull) 
			VALUES(@ItemId,1,@IsMain,@HostUrl,3,1,@OriginalPath,'',@ImageUrlThumb,@ImageUrlThumbSmall,@ImageUrlFull)
			 
			SET @PhotoId = SCOPE_IDENTITY()
            
			--Step 2 : Update main photo
			IF(@IsMain = 1)
			BEGIN
				UPDATE TC_CarPhotos SET IsMain = 0 WHERE StockId = @ItemId AND Id <> @PhotoId AND IsActive = 1
			END
			--Step 3 : Update main table 
			UPDATE IMG_Photos SET IsProcessed = 1,HostUrl = @HostUrl,OriginalPath = @OriginalPath,ProcessedId = @PhotoId,ProcessedOn = GETDATE()
			WHERE Id = @ReqPhotoId
		END
	--ManageWebSite
	IF(@CategoryId= 40 OR @CategoryId = 41 OR @CategoryId = 42 OR @CategoryId= 43  OR @CategoryId = 44 OR @CategoryId = 45 OR @CategoryId = 46 OR @CategoryId = 47)
		BEGIN
			UPDATE Microsite_Images SET HostURL = @HostUrl, 
										StatusId = 3,
										IsReplicated = 1,
										OriginalImgPath =  OriginalImgPath
			WHERE Id = @ItemId
            
			--Step 2 : Update main table 
			UPDATE IMG_Photos SET IsProcessed = 1,HostUrl = @HostUrl,OriginalPath = @OriginalPath,ProcessedId = @ItemId,ProcessedOn = GETDATE()
			WHERE Id = @ReqPhotoId
		END
	--ManageWebSite
	IF(@CategoryId = 70)
		BEGIN
			UPDATE Microsite_Images SET HostURL = @HostUrl, 
										StatusId = 3,
										IsReplicated = 1,
										OriginalImgPath =  OriginalImgPath
			WHERE Id = @ItemId
            
			--Step 2 : Update main table 
			UPDATE IMG_Photos SET IsProcessed = 1,HostUrl = @HostUrl,OriginalPath = @OriginalPath,ProcessedId = @ItemId,ProcessedOn = GETDATE()
			WHERE Id = @ReqPhotoId
		END
	--CWCommunity
	IF(@CategoryId = 5)
		BEGIN
			INSERT INTO UP_Photos(HostURL,StatusId,Size500,Size1024,Size800,IsReplicated,OriginalImgPath) VALUES(@HostUrl,3,1,1,1,1,@OriginalPath)
			 
			SET @PhotoId = SCOPE_IDENTITY()
            
			--Step 2 : Update main table 
			UPDATE IMG_Photos SET IsProcessed = 1,HostUrl = @HostUrl,OriginalPath = @OriginalPath,ProcessedId = @PhotoId,ProcessedOn = GETDATE()
			WHERE Id = @ReqPhotoId
			
		END
	--avtarimage
	IF(@CategoryId = 71)
		BEGIN
			UPDATE UserProfile SET HostURL = @HostUrl, StatusId=3,IsReplicated = 1,AvtOriginalImgPath = AvtOriginalImgPath
			WHERE UserId = @ItemId		    
			--Step 2 : Update main table 
			UPDATE IMG_Photos SET IsProcessed = 1,HostUrl = @HostUrl,OriginalPath = @OriginalPath,ProcessedId = @ItemId,ProcessedOn = GETDATE()
			WHERE Id = @ReqPhotoId
		END
	--ForumsRealImage
	IF(@CategoryId = 6)
		BEGIN
		    UPDATE UserProfile SET HostURL = @HostUrl, 
								   StatusId=3,
								   IsReplicated = 1,
								   AvtOriginalImgPath = @OriginalPath
			WHERE UserId = @ItemId
			--Step 2 : Update main table 
			UPDATE IMG_Photos SET IsProcessed = 1,HostUrl = @HostUrl,OriginalPath = @OriginalPath,ProcessedId = @ItemId,ProcessedOn = GETDATE()
			WHERE Id = @ReqPhotoId
		END
	--UsedSellCars
	IF(@CategoryId = 7)
		BEGIN
			INSERT INTO CarPhotos(HostURL,StatusId,IsReplicated,OriginalImgPath) VALUES(@HostUrl,3,1,@OriginalPath)
			SET @PhotoId = SCOPE_IDENTITY()
			--Step 2 : Update main table 
			UPDATE IMG_Photos SET IsProcessed = 1,HostUrl = @HostUrl,OriginalPath = @OriginalPath,ProcessedId = @PhotoId,ProcessedOn = GETDATE()
			WHERE Id = @ReqPhotoId
		END
	--Absure
	IF(@CategoryId = 49)
		BEGIN
		   DECLARE @AspectRatio Float
		  -- Find Aspect Ratio then give resizing image url ... 
		   SELECT @AspectRatio= ISNULL(ACP.AspectRatio,1.33)  FROM  AbSure_CarPhotos AS ACP WITH(NOLOCK) WHERE AbSure_CarPhotosId = @PhotoId
		   IF @AspectRatio = 1.77
			   BEGIN
					UPDATE AbSure_CarPhotos SET HostURL = @HostUrl, 
											StatusId=3,
											IsReplicated = 1,
											OriginalImgPath =  OriginalImgPath, --Added By Deepak on 5th Aug 2015
											DirectoryPath = '',
											ImageUrlExtraLarge = '/1024X576/'  + OriginalImgPath,
											ImageUrlLarge      = '/640X360/'   + OriginalImgPath,
											ImageUrlThumb      = '/320X180/'   + OriginalImgPath, 
											ImageUrlSmall      = '/80X45/'     + OriginalImgPath,
											ImageUrlOriginal   = '/0X0/'       + OriginalImgPath
					WHERE AbSure_CarPhotosId = @PhotoId
				END
			ELSE      -- For aspect ratio 1.33 is default 
				BEGIN
				
				    UPDATE AbSure_CarPhotos SET HostURL = @HostUrl, 
											StatusId=3,
											IsReplicated = 1,
											OriginalImgPath =  OriginalImgPath, --Added By Deepak on 5th Aug 2015
											DirectoryPath       = '',										
											ImageUrlExtraLarge  = '/1024x786/' + OriginalImgPath,
											ImageUrlLarge       = '/640x480/'  + OriginalImgPath,
											ImageUrlThumb       = '/300x225/'  + OriginalImgPath, 
											ImageUrlSmall       = '/80x60/'    + OriginalImgPath,
											ImageUrlOriginal    = '/0X0/'      + OriginalImgPath	
					WHERE AbSure_CarPhotosId = @PhotoId			
				END
		END
	--DealerProfilePhoto
	IF(@CategoryId = 4)
	BEGIN
		--OriginalImgPath Added By Deepak on 5th Aug 2015		
		UPDATE Dealers SET ProfilePhotoHostUrl = @HostUrl,
						   ProfilePhotoStatusId = 3, 
						   ProfilePhotoUrl = '/0X0/' + OriginalImgPath,
						   OriginalImgPath = OriginalImgPath 
						   WHERE ID = @ItemId
		--mysql sync start
		declare
		 @NewID	decimal(18,0) = null,@LoginId	varchar(30) = null, @Passwd	varchar(50) = null, @FirstName	varchar(100) = null, @LastName	varchar(100) = null, @EmailId	varchar(250) = null, @Organization	varchar(100) = null, @Address1 varchar(500) = null, @Address2	varchar(500) = null, @AreaId	decimal(18,0) = null, @CityId	decimal(18,0) = null, @StateId	decimal(18,0) = null, @Pincode	varchar(6) = null, @PhoneNo	varchar(50) = null, @FaxNo	varchar(50) = null, @MobileNo	varchar(50) = null, @ExpiryDate	datetime = null, @WebsiteUrl	varchar(100) = null, @ContactPerson	varchar(200) = null, @ContactHours	varchar(30) = null, @ContactEmail	varchar(250) = null, @Status	tinyint = null, @LastUpdatedOn	datetime = null, @CertificationId	smallint = null, @BPMobileNo	varchar(15) = null, @BPContactPerson	varchar(60) = null, @IsTCDealer	tinyint = null, @IsWKitSent	tinyint = null, @IsTCTrainingGiven	tinyint = null,  @TC_DealerTypeId tinyint,  @Longitude	float = null, @Lattitude	float = null, @DeletedReason	smallint = null, @HavingWebsite	tinyint = null, @ActiveMaskingNumber	varchar(20) = null, @DealerVerificationStatus	smallint = null, @IsPremium	tinyint = null, @WebsiteContactMobile	varchar(50) = null, @WebsiteContactPerson	varchar(100) = null, @ApplicationId	tinyint, @IsWarranty	tinyint = null, @LeadServingDistance	smallint = null, @OutletCnt	int = null, @ProfilePhotoHostUrl	varchar(100) = null, @ProfilePhotoUrl	varchar(250) = null, @AutoInspection	tinyint = null, @RCNotMandatory	tinyint = null,  @OwnerMobile	varchar(20) = null, @ShowroomStartTime	varchar(30) = null, @ShowroomEndTime	varchar(30) = null, @DealerLastUpdatedBy	int = null, @LegalName	varchar(50) = null, @PanNumber	varchar(50) = null, @TanNumber	varchar(50) = null, @AutoClosed	tinyint = null, @LandlineCode	varchar(4), @Ids Varchar(MAX),  @IsDealerDeleted bit = null, @DeleteComment varchar(500)  = null, @DeletedOn datetime =null , @PackageRenewalDate datetime = null, @DealerSource int = null,@LogoUrl varchar(100) = null
		set @UpdateType = 3
		set @NewID=@ItemId
		begin try
		exec [dbo].[SyncDealersWithMysqlUpdate] 
		@NewID	, @LoginId	, @Passwd	, @FirstName	, @LastName	, @EmailId	, @Organization	, @Address1 , @Address2	, @AreaId	, @CityId	, @StateId	, @Pincode	, @PhoneNo	, @FaxNo	, @MobileNo	, @ExpiryDate	, @WebsiteUrl	, @ContactPerson	, @ContactHours	, @ContactEmail	, @Status	, @LastUpdatedOn	, @CertificationId	, @BPMobileNo	, @BPContactPerson	, @IsTCDealer	, @IsWKitSent	, @IsTCTrainingGiven	, @HostURL	, @TC_DealerTypeId ,  @Longitude	, @Lattitude	, @DeletedReason	, @HavingWebsite	, @ActiveMaskingNumber	, @DealerVerificationStatus	, @IsPremium	, @WebsiteContactMobile	, @WebsiteContactPerson	, @ApplicationId	, @IsWarranty	, @LeadServingDistance	, @OutletCnt	, @ProfilePhotoHostUrl	, @ProfilePhotoUrl	, @AutoInspection	, @RCNotMandatory	, @OriginalImgPath	, @OwnerMobile	, @ShowroomStartTime	, @ShowroomEndTime	, @DealerLastUpdatedBy	, @LegalName	, @PanNumber	, @TanNumber	, @AutoClosed	, @LandlineCode	, @Ids , @UpdateType , @IsDealerDeleted , @DeleteComment , @DeletedOn  , @PackageRenewalDate, @DealerSource, @LogoUrl
		
			end try
			BEGIN CATCH
				INSERT INTO CarWaleMyslSyncExceptions(ModuleName,SPName,ErrorMsg,TableName,FailedId,CreatedOn,InputParameter)
				VALUES('MysqlSync','IMG_SaveCategoryPhotos',ERROR_MESSAGE(),'SyncDealersWithMysqlUpdate',@NewId,GETDATE(),@UpdateType)
			END CATCH
		-- mysql sync end
		--Step 2 : Update main table 
		UPDATE IMG_Photos SET IsProcessed = 1,HostUrl = @HostUrl,OriginalPath = @OriginalPath,ProcessedId = @ItemId,ProcessedOn = GETDATE()
		WHERE Id = @ReqPhotoId
	END
	
    --DMSScreenShot
	IF(@CategoryId = 9)
	BEGIN
		UPDATE TC_NewCarInquiries SET DMSScreenShotHostUrl = @HostUrl,DMSScreenShotStatusId = 3,DMSScreenShotUrl=@OriginalPath-- OriginalImgPath =  @OriginalPath 
		WHERE TC_NewCarInquiriesId = @ItemId
		--Step 2 : Update main table 
		UPDATE IMG_Photos SET IsProcessed = 1,HostUrl = @HostUrl,OriginalPath = @OriginalPath,ProcessedId = @ItemId,ProcessedOn = GETDATE()
		WHERE Id = @ReqPhotoId
	END
	--BugsScreenShot
	IF(@CategoryId = 10)
	BEGIN
		--OriginalImgPath Added By Deepak on 5th Aug 2015
		UPDATE TC_BugFeedback SET HostUrl = @HostUrl,
				BugScreenShotStatusId = 3,
				FilePath = '/0X0/' + OriginalImgPath,  
				OriginalImgPath =  OriginalImgPath 
		WHERE TC_Bug_Id = @ItemId
		--Step 2 : Update main table 
		UPDATE IMG_Photos SET IsProcessed = 1,HostUrl = @HostUrl,OriginalPath = @OriginalPath,ProcessedId = @ItemId,ProcessedOn = GETDATE()
		WHERE Id = @ReqPhotoId
	END
	--splashscreen
	IF(@CategoryId = 57)
	BEGIN
		UPDATE AppSplashScreenSetting SET HostUrl = @HostUrl, OriginalImgPath = OriginalImgPath, IsReplicated = 1 
		WHERE Id = @ItemId
		--Step 2 : Update main table 
		UPDATE IMG_Photos SET IsProcessed = 1,HostUrl = @HostUrl,OriginalPath = @OriginalPath,ProcessedId = @ItemId,ProcessedOn = GETDATE()
		WHERE Id = @ReqPhotoId
	END
	IF(@CategoryId = 72)
	BEGIN
		UPDATE Con_ModelVideos SET ThumbnailHostURL = @HostUrl, OriginalImgPath = OriginalImgPath, IsReplicated = 1 
		WHERE Id = @ItemId
		--Step 2 : Update main table 
		UPDATE IMG_Photos SET IsProcessed = 1,HostUrl = @HostUrl,OriginalPath = @OriginalPath,ProcessedId = @ItemId,ProcessedOn = GETDATE()
		WHERE Id = @ReqPhotoId
	END
	IF(@CategoryId = 73)
	BEGIN
		UPDATE CarGalleryPhotos SET HostUrl = @HostUrl, OriginalImgPath =  OriginalImgPath, IsReplicated = 1 
		WHERE Id = @ItemId
		--Step 2 : Update main table 
		UPDATE IMG_Photos SET IsProcessed = 1,HostUrl = @HostUrl,OriginalPath = @OriginalPath,ProcessedId = @ItemId,ProcessedOn = GETDATE()
		WHERE Id = @ReqPhotoId
	END
	--carwaleoffers
	IF(@CategoryId = 56)
	BEGIN
		UPDATE DealerOffers SET HostUrl = @HostUrl,
								OriginalImgPath = OriginalImgPath
		WHERE Id = @ItemId 
		--Step 2 : Update main table 
		UPDATE IMG_Photos SET IsProcessed = 1,HostUrl = @HostUrl,OriginalPath = @OriginalPath,ProcessedId = @ItemId,ProcessedOn = GETDATE()
		WHERE Id = @ReqPhotoId
	END
	--DealerModelColors
	IF(@CategoryId = 54)
	BEGIN
		UPDATE Microsite_DealerModelColors
		SET HostUrl = @HostUrl,
			OriginalImgPath = OriginalImgPath,
			IsActive = 1
		WHERE Id = @ItemId
		--Step 2 : Update main table 
		UPDATE IMG_Photos SET IsProcessed = 1,HostUrl = @HostUrl,OriginalPath = @OriginalPath,ProcessedId = @ItemId,ProcessedOn = GETDATE()
		WHERE Id = @ReqPhotoId
	END
	--DealerModelFeatures
	IF(@CategoryId = 53)
	BEGIN
		UPDATE Microsite_DWModelFeatures
		SET HostUrl = @HostUrl,
			OriginalImgPath = OriginalImgPath,
			IsActive = 1
		WHERE Id = @ItemId
		--Step 2 : Update main table 
		UPDATE IMG_Photos SET IsProcessed = 1,HostUrl = @HostUrl,OriginalPath = @OriginalPath,ProcessedId = @ItemId,ProcessedOn = GETDATE()
		WHERE Id = @ReqPhotoId
	END
	--DealerModels
	IF(@CategoryId = 52)
	BEGIN
		UPDATE TC_DealerModels
		SET HostUrl = @HostUrl,
			OriginalImgPath = OriginalImgPath,
			IsDeleted = 0
		WHERE Id = @ItemId
		--Step 2 : Update main table 
		UPDATE IMG_Photos SET IsProcessed = 1,HostUrl = @HostUrl,OriginalPath = @OriginalPath,ProcessedId = @ItemId,ProcessedOn = GETDATE()
		WHERE Id = @ReqPhotoId
	END
	--DealerOffers
	IF(@CategoryId = 51)
	BEGIN
		UPDATE Microsite_DealerOffers
		SET HostUrl = @HostUrl,
			OriginalImgPath = OriginalImgPath,
			IsActive = 1
		WHERE Id = @ItemId
		--Step 2 : Update main table 
		UPDATE IMG_Photos SET IsProcessed = 1,HostUrl = @HostUrl,OriginalPath = @OriginalPath,ProcessedId = @ItemId,ProcessedOn = GETDATE()
		WHERE Id = @ReqPhotoId
	END
	
	--DealerOffersSlug
	IF(@CategoryId = 58)
	BEGIN
		UPDATE Microsite_DealerOffers
		SET HostUrl = @HostUrl,
			OriginalImgPath = OriginalImgPath,
			IsActive = 1
		WHERE Id = @ItemId
		--Step 2 : Update main table 
		UPDATE IMG_Photos SET IsProcessed = 1,HostUrl = @HostUrl,OriginalPath = @OriginalPath,ProcessedId = @ItemId,ProcessedOn = GETDATE()
		WHERE Id = @ReqPhotoId
	END
	--DealerModelPhotos
	IF(@CategoryId = 50)
	BEGIN
		UPDATE Microsite_DealerOffers
		SET HostUrl = @HostUrl,
			OriginalImgPath = OriginalImgPath,
			IsActive = 1
		WHERE Id = @ItemId
		--Step 2 : Update main table 
		UPDATE IMG_Photos SET IsProcessed = 1,HostUrl = @HostUrl,OriginalPath = @OriginalPath,ProcessedId = @ItemId,ProcessedOn = GETDATE()
		WHERE Id = @ReqPhotoId
	END
	/*IF(@CategoryId = 74)
	BEGIN
		UPDATE Microsite_ServiceOffers
		SET HostUrl = @HostUrl,
			OriginalImgPath = OriginalImgPath,
			IsActive = 1
		WHERE Id = @ItemId
		--Step 2 : Update main table 
		UPDATE IMG_Photos SET IsProcessed = 1,HostUrl = @HostUrl,OriginalPath = @OriginalPath,ProcessedId = @ItemId,ProcessedOn = GETDATE()
		WHERE Id = @ReqPhotoId
	END*/
	------------------------------------------------------
	IF(@CategoryId = 75)
	BEGIN
	    INSERT INTO CarPhotos(HostUrl,OriginalImgPath,DirectoryPath,ImageUrlThumb,ImageUrlMedium,ImageUrlThumbSmall,ImageUrlFull) 
		VALUES (@HostUrl,@OriginalPath,'','/310X174/' + @OriginalPath,'/160X89/' + @OriginalPath,'/110X61/' + @OriginalPath,'/664X374/' + @OriginalPath)
		
		SET @PhotoId = SCOPE_IDENTITY()
		--Step 2 : Update main table 
		UPDATE IMG_Photos SET IsProcessed = 1,HostUrl = @HostUrl,OriginalPath = @OriginalPath,ProcessedId = @PhotoId,ProcessedOn = GETDATE()
		WHERE Id = @ReqPhotoId
	END 
	/*IF(@CategoryId = 59)
	BEGIN
		UPDATE DOA_Offers
		SET HostUrl = @HostUrl,OriginalImgPath =  OriginalImgPath
		WHERE Id = @ItemId
		--Step 2 : Update main table 
		UPDATE IMG_Photos SET IsProcessed = 1,HostUrl = @HostUrl,OriginalPath = @OriginalPath,ProcessedId = @ItemId,ProcessedOn = GETDATE()
		WHERE Id = @ReqPhotoId
	END*/
	--DCRMChequeDDPDC
	IF(@CategoryId = 76)
	BEGIN
		--CODE for updating cheque/dd/pdc image related tables/columns
		DECLARE @PaymentDetailsId INT
		/*SELECT TOP 1 @PaymentDetailsId = ID 
		FROM DCRM_PaymentDetails WITH (NOLOCK) WHERE TransactionId = @ItemId 
		ORDER BY ID DESC*/
		SELECT TOP 1 @PaymentDetailsId = ID 
		FROM DCRM_PaymentDetails WITH (NOLOCK) 
		WHERE TransactionId = @ItemId AND HostUrl IS NULL AND Mode IN (2,3,8) AND ISNULL(IsApproved,1) = 1--Vaibhav K 14-Mar-2016 neglect rejected payments and mode in (2,3,8)
		ORDER BY ID
		UPDATE DCRM_PaymentDetails
		SET HostUrl = @HostUrl, OriginalImgPath = @OriginalPath
		WHERE ID = @PaymentDetailsId
		UPDATE IMG_Photos 
		SET IsProcessed = 1,HostUrl = @HostUrl,OriginalPath = @OriginalPath,ProcessedId = @PhotoId,ProcessedOn = GETDATE(),
			ItemId = @PaymentDetailsId
		WHERE Id = @ReqPhotoId
	END
	--DCRMDepositSlip
	IF(@CategoryId = 77)
	BEGIN
		--CODE for updating attafched Lpa related tables/columns
		
		UPDATE DCRM_PaymentDetails
		SET DepSlipHostUrl = @HostUrl, DepSlipOriginalImgPath = @OriginalPath
		WHERE Id = @ItemId
		UPDATE IMG_Photos 
		SET IsProcessed = 1,HostUrl = @HostUrl,OriginalPath = @OriginalPath,ProcessedId = @PhotoId,ProcessedOn = GETDATE()			
		WHERE Id = @ReqPhotoId
	END
	--DCRMLPA
	IF(@CategoryId = 78)
	BEGIN
		--CODE for updating attafched Lpa related tables/columns
		DECLARE @AttachedLpaId INT
		SELECT TOP 1 @AttachedLpaId = Id
		FROM M_AttachedLpaDetails WITH (NOLOCK)
		WHERE SalesDealerId = @ItemId AND HostURL IS NULL --Vaibhav K 15-jan-2016
		ORDER BY Id	DESC
		--Vaibhav K 18 Apr 2016 updated the IsActive = 1, UpdatedOn with getdate
		UPDATE M_AttachedLpaDetails
		SET HostURL = @HostUrl, OriginalImgPath = @OriginalPath, IsActive = 1, UploadedOn = GETDATE()
		WHERE Id = @AttachedLpaId
		UPDATE IMG_Photos 
		SET IsProcessed = 1,HostUrl = @HostUrl,OriginalPath = @OriginalPath,ProcessedId = @PhotoId,ProcessedOn = GETDATE(),
			ItemId = @AttachedLpaId
		WHERE Id = @ReqPhotoId AND HostURL IS NULL --Vaibhav K 15-jan-2016
	END
	--OprUserProfile
	IF(@CategoryId = 79)
	BEGIN
		UPDATE OprUsers
		SET HostURL = @HostUrl, OriginalImgPath = @OriginalPath
		WHERE Id = @ItemId
		UPDATE IMG_Photos 
		SET IsProcessed = 1,HostUrl = @HostUrl,OriginalPath = @OriginalPath,ProcessedId = @ItemId,ProcessedOn = GETDATE()
		WHERE Id = @ReqPhotoId
	END
	-- Modified By : Komal Manjare on 07-09-2016 
	-- update ticketimages and related tables/column
	IF(@CategoryId=82)
	BEGIN
		DECLARE @TicketLogId INT, @AttachedFileId INT
		
		SELECT TOP 1 @TicketLogId = Id
		FROM Support_TicketLog WITH (NOLOCK)
		WHERE TicketId = @ItemId 
		ORDER BY Id	DESC
		SELECT TOP 1 @AttachedFileId = ID
		FROM Support_AttachedFileDetails WITH (NOLOCK)
		WHERE TicketLogId = @TicketLogId AND HostUrl IS NULL
		ORDER BY Id
		UPDATE Support_AttachedFileDetails
		SET HostUrl = @HostUrl, OriginalImgPath = @OriginalPath,IsActive=1,UploadedOn=GETDATE()
		WHERE Id = @AttachedFileId--TicketLogId = @TicketLogId
		UPDATE IMG_Photos 
		SET IsProcessed = 1,HostUrl = @HostUrl,OriginalPath = @OriginalPath,ProcessedId = @PhotoId,ProcessedOn = GETDATE(),
			ItemId = @AttachedFileId--@TicketLogId
		WHERE Id = @ReqPhotoId
	END
	-- Sponsored Campaigns
	IF(@CategoryId=83)
	BEGIN
		UPDATE IMG_Photos 
		SET IsProcessed = 1,HostUrl = @HostUrl,OriginalPath = @OriginalPath,ProcessedId = @ItemId,ProcessedOn = GETDATE()
		WHERE Id = @ReqPhotoId
	END
	END TRY
	BEGIN CATCH 
	INSERT INTO CarWaleWebSiteExceptions (
			                                ModuleName,
											SPName,
											ErrorMsg,
											TableName,
											FailedId,
											CreatedOn)
						            VALUES ('Photo Replication RQ',
									        '[dbo].[IMG_SaveCategoryPhotos]',
											 ERROR_MESSAGE(),
											 @CategoryId,
											 @PhotoId,
											 GETDATE()
                                            )
	END CATCH 
	
END

