IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[IMG_CarPhotosUpdateReplicateRQ_v15]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[IMG_CarPhotosUpdateReplicateRQ_v15]
GO

	-- Author  : chetan dev
-- Create date : 09/10/2012 14:00 PM  
-- Description : Updates Hosturl and status id
-- Modified by : Manish on 09-07-2014 implement all the changes in step 1 i.e. "IMG_ImageUpdateRQ" and step 2 i.e."IMG_CarPhotosUpdateInBetweenProcRQ" in this sp and commented step 1 and step 2 sp.
-- Modified by : Vinay Kumar Praajapati on 31st july 2014 comment out updation of smallPic and largePic of Table CarVersions.
-- Modified By :Tejashree Patil on 17 Oct 2014, New Category ManageWebsite47.
-- Added By Ruchira Patil on 17th Dec 2014(for absure images)
-- Modified By : Khushabo Patil on 16/03/2015 added hosturl in carmodels xlarge
-- Modified By : Khushabo Patil on 23/03/2015 added hosturl in carmodels ImagePathLarge
-- Modified By : Khushaboo Patil on 31/03/2015 added @PhotoGalleryId and applicationId condition
-- Modified By : Khushaboo Patil on 2-6-2015, New Category dealerprofilephoto. 
-- Modified By : Ashwini Todkar on 9 July 2015,Added query to update model and version image paths 
-- Modified By : Khushaboo Patil on 21-7-2015, New Category dmsscreenshot. 
-- Modified BY: Afrose on 22nd July 2015, new category bugsscreenshot
-- Modified BY : Vaibhav K 10-Aug-2015, New categories added for ManageWebsite section(autobiz)
-- Modified By : Manish on 08-09-2015 added try and catch block for exception handling.
-- Modified By : Vaibhav K 16-Sept-2015 added a new category 'dealeroffersslug'
-- Modified By Vivek Gupta on 22-09-2015, changes condition of inserting or updating photo urls in carphotos
-- Modified By : Vinay Kumar Prajapati 29th Sep 2015 Add AspectRatio To save Image "absure"
-- Modified BY : Vaibhav K 10-oct-2015 Category added to repliacte offer photos for Deals On Auto
-- Modified by: Kritika Choudhary on 6th oct 2015, added category serviceoffers
-- Modified by : Vinay Kumar Prajapati 27th oct 2015 added category  absurereportproblem 
-- Modified by : Navead Kazi 13th july 2015 added category stockimageapi
-- Modified by  : Harshil Mehta 28 Sept.2015 for testimonials category
-- Modified by : Afrose on 6-Oct-2016, added condition for IsApproved=1 for category stockimageapi 
-- =============================================   
CREATE PROCEDURE [dbo].[IMG_CarPhotosUpdateReplicateRQ_v15.8.1]  --5670, 'imgd1.aeplcdn.com', '', 'ShowRoomPhotos', '', NULL  
 -- Add the parameters for the stored procedure here      
 @PhotoId   BIGINT,
 @HostUrl VarChar(100),
 @ServerList VarChar(100),
 @Category varchar(100),
 @Environment VARCHAR(50) = '',
 @MaxServers Int=NULL
 AS      
BEGIN      
	-- SET NOCOUNT ON added to prevent extra result sets from      
	-- interfering with SELECT statements.      
	SET NOCOUNT ON;  
	-- updating final image name  
	DECLARE @ImgUrlMedium VARCHAR(100)
	DECLARE @PhotoGalleryId AS TINYINT
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
	@XLargePic varchar(150)=null,
	@CV_ID int = null,
	@SelId varchar(8000)=null,
	@CarModelId numeric = null
		  
	declare   @SegmentId numeric(18, 0)  = null, @BodyStyleId numeric(18, 0)  = null,  @Discontinuation_date datetime =null, @LaunchDate datetime = null, @ReplacedByVersionId smallint = null,  @VUpdatedBy numeric = null, @VUpdatedOn datetime = null, @CarFuelType tinyint = null, @CarTransmission tinyint = null, @OriginalImgPath VARCHAR(250) = null
		--mysql sync end
	BEGIN TRY 
	IF(@Category='editcms')
		BEGIN
			UPDATE Con_EditCms_Images SET IsActive=1 ,
										   HostUrl = @HostUrl,
										   StatusId = 3,
										   IsReplicated=1,
										   OriginalImgPath = @Environment + OriginalImgPath 
			WHERE Id=@PhotoId  
			UPDATE IMG_AllCarPhotos SET StatusId = 3 , 
										ItemStorage =@ServerList , 
										MaxServers =@MaxServers
			WHERE ItemId = @PhotoId
			-- Modified By : Khushaboo Patil on 23/03/2015 added hosturl in carmodels ImagePathLarge
			--UPDATE CarModels SET XLargePic = @HostUrl + EI.ImagePathLarge
			--	FROM Con_EditCms_Images EI
			--WHERE CarModels.ID= EI.ModelId AND EI.ID = @PhotoId AND EI.IsMainImage = 1
			-- Modified By : Khushaboo Patil on 31/03/2015 added @PhotoGalleryId and applicationId condition
			UPDATE CM 
			SET CM.OriginalImgPath = EI.OriginalImgPath --Added By Ashwini Todkar on 9 July 2015, updated image path to carmodels
			,CM.HostURL = EI.HostURL
			,CM.IsReplicated = 1
			FROM CarModels CM   WITH(NOLOCK)
			INNER JOIN Con_EditCms_Images EI WITH(NOLOCK) ON CM.ID = EI.ModelId
			INNER JOIN Con_EditCms_Basic EB WITH(NOLOCK) ON EB.Id = EI.BasicId
			WHERE EI.IsMainImage = 1  AND EB.CategoryId = 10 AND EB.ApplicationID = 1 
				AND EI.Id = @PhotoId -- For CarWale -- 10 for Photo Gallery
			set @Id=@PhotoId
			set @UpdateType=24
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
				@Environment ,
				@XLargePic ,
				@CV_ID,
				@SelId,
				@CarModelId
	end try
	BEGIN CATCH
		INSERT INTO CarWaleMyslSyncExceptions(ModuleName,SPName,ErrorMsg,TableName,FailedId,CreatedOn,InputParameter)
		VALUES('MysqlSync','IMG_CarPhotosUpdateReplicateRQ_v15.8.1',ERROR_MESSAGE(),'SyncCarModelsWithMysqlUpdate',@Id,GETDATE(),@UpdateType)
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
			WHERE EI.Id = @PhotoId AND EI.IsMainImage = 1  AND EB.CategoryId = 10 AND EB.ApplicationID = 1 
				AND CV.SpecialVersion = 0 --Special version having different image than model image
			--sync with mysql
			set @UpdateType = 5
			begin try
			exec [dbo].[SyncCarVersionsWithMysqlUpdate] 
				@PhotoId , @Name , @SegmentId , @BodyStyleId , @Used , @New , @IsDeleted , @Indian , @Imported , @Futuristic , @Classic , @Modified ,		 @Discontinuation_date ,	 @MaskingName , @SubSegmentId , @LaunchDate , @DiscontinuitionId , @ReplacedByVersionId , @Comment , @VUpdatedBy , @VUpdatedOn , @CarFuelType , @CarTransmission , @HostUrl , @UpdateType , @Environment , @OriginalImgPath , @SmallPic , @LargePic	
			end try
			BEGIN CATCH
				INSERT INTO CarWaleMyslSyncExceptions(ModuleName,SPName,ErrorMsg,TableName,FailedId,CreatedOn,InputParameter)
				VALUES('MysqlSync','IMG_CarPhotosUpdateReplicateRQ_v15.8.1',ERROR_MESSAGE(),'SyncCarVersionsWithMysqlUpdate',@PhotoId,GETDATE(),@UpdateType)
			END CATCH
			--mysql sync end
		
		END
	IF(@Category='tradingcars')
		BEGIN
			--update in trading cars table
			UPDATE TC_CarPhotos SET IsActive=1 , 
									HostUrl = @HostUrl,
									StatusId=3,
									IsReplicated=1,
									OriginalImgPath = @Environment + OriginalImgPath, --Added By Deepak on 5th Aug 2015
									DirectoryPath = '',
									OrigFileName = '/0X0/' + @Environment + OriginalImgPath,
									ImageUrlThumb = '/310X174/' + @Environment + OriginalImgPath,
									ImageUrlMedium = '/160X89/' + @Environment + OriginalImgPath,
									ImageUrlThumbSmall = '/110X61/' + @Environment + OriginalImgPath, 
									ImageUrlFull = '/664X374/' + @Environment + OriginalImgPath
			  WHERE Id=@PhotoId  
			--update all carphotos
			UPDATE IMG_AllCarPhotos SET StatusId = 3 , 
										ItemStorage =@ServerList , 
										MaxServers =@MaxServers
			WHERE ItemId = @PhotoId
			DECLARE 
				@InquiryId BIGINT,
				@StockId BIGINT,
				@ImageUrlFull  VARCHAR(100),
				@ImageUrlThumb  VARCHAR(100),
				@ImageUrlThumbSmall VARCHAR(100),
				@IsMain BIT,
				@HostUri VARCHAR(100),
				@DirPath VARCHAR(100)				
				
			SELECT 
				@StockId=StockId,
				@ImageUrlFull=ImageUrlFull,
				@ImageUrlThumb=ImageUrlThumb,	
				@ImageUrlThumbSmall=ImageUrlThumbSmall,
				@IsMain=IsMain ,
				@HostUri=HostUrl,
				@DirPath=ISNULL(DirectoryPath,''),
				@ImgUrlMedium=ImageUrlMedium,          ---added ImageUrlMedium on 20-05-2014 by Manish
				@OriginalImgPath = OriginalImgPath		--Added By Deepak on 5th Aug 2015
				
			FROM TC_CarPhotos WITH(NOLOCK)
			WHERE Id=@PhotoId 
	
			SELECT @InquiryId = Id FROM SellInquiries Si WITH(NOLOCK) WHERE Si.Tc_StockId = @StockId AND StatusId=1 AND SourceId = 2
			--Inserting the car photos details in the CarPhotos table
			IF (@InquiryId IS NOT NULL)
				BEGIN
					IF NOT EXISTS (SELECT Id FROM CarPhotos WITH(NOLOCK) WHERE TC_CarPhotoId = @PhotoId)
					BEGIN
						-- AM Modified 6-11-2012 to insert PhotoId from TC_CarPhotos to CarPhotos
						INSERT INTO CarPhotos(InquiryId,ImageUrlFull,ImageUrlThumb,ImageUrlThumbSmall,IsDealer,IsMain,IsActive,IsApproved,DirectoryPath, HostUrl,IsReplicated,TC_CarPhotoId,ImageUrlMedium, OriginalImgPath)
										VALUES(@InquiryId,@ImageUrlFull,@ImageUrlThumb,@ImageUrlThumbSmall,1,@IsMain,1,1,@DirPath, @HostUri,1,@PhotoId,@ImgUrlMedium, @OriginalImgPath)   ---added ImageUrlMedium on 20-05-2014 by Manish --@OriginalImgPath Added By Deepak on 5th Aug 2015
					END
					ELSE 
					    UPDATE CarPhotos SET
						ImageUrlFull = @ImageUrlFull
						,ImageUrlThumb = @ImageUrlThumb
						,ImageUrlThumbSmall = @ImageUrlThumbSmall					
						,IsMain = @IsMain						
						,DirectoryPath = @DirPath
						, HostUrl = @HostUri											
						,ImageUrlMedium = @ImgUrlMedium
						, OriginalImgPath = @OriginalImgPath
						WHERE InquiryId = @InquiryId
						AND TC_CarPhotoId = @PhotoId
				END
		END
		
	IF(@Category='carversion')
		BEGIN
			UPDATE CarVersions SET IsReplicated = 1 , 
									HostUrl = @HostUrl,
									OriginalImgPath = @Environment + OriginalImgPath
			WHERE Id=@PhotoId 
		set @UpdateType = 6
		begin try
		exec [dbo].[SyncCarVersionsWithMysqlUpdate] @PhotoId , @Name , @SegmentId , @BodyStyleId , @Used , @New , @IsDeleted , @Indian , @Imported , @Futuristic , @Classic , @Modified , @Discontinuation_date ,	 @MaskingName , @SubSegmentId , @LaunchDate , @DiscontinuitionId , @ReplacedByVersionId , @Comment , @VUpdatedBy , @VUpdatedOn , @CarFuelType , @CarTransmission , @HostUrl , @UpdateType , @Environment , @OriginalImgPath , @SmallPic , @LargePic 	
		end try
		BEGIN CATCH
			INSERT INTO CarWaleMyslSyncExceptions(ModuleName,SPName,ErrorMsg,TableName,FailedId,CreatedOn,InputParameter)
			VALUES('MysqlSync','IMG_CarPhotosUpdateReplicateRQ_v15.8.1',ERROR_MESSAGE(),'SyncCarVersionsWithMysqlUpdate',@PhotoId,GETDATE(),@UpdateType)
		END CATCH
		END
	IF(@Category='expectedlaunch')
		BEGIN
			UPDATE CarModels SET HostURL = @HostUrl , 
								 IsReplicated =1,
								 OriginalImgPath = @Environment + OriginalImgPath
			WHERE ID = @PhotoId
			set @Id=@PhotoId
			set @UpdateType=25
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
				@Environment ,
				@XLargePic ,
				@CV_ID,
				@SelId,
				@CarModelId
			end try
			BEGIN CATCH
				INSERT INTO CarWaleMyslSyncExceptions(ModuleName,SPName,ErrorMsg,TableName,FailedId,CreatedOn,InputParameter)
				VALUES('MysqlSync','IMG_CarPhotosUpdateReplicateRQ_v15.8.1',ERROR_MESSAGE(),'SyncCarModelsWithMysqlUpdate',@PhotoId,GETDATE(),@UpdateType)
			END CATCH
			--mysql sync end
		END
	IF(@Category='featuredlisting')
		BEGIN
			UPDATE Con_FeaturedListings SET HostUrl = @HostUrl, 
											IsReplicated =1
			WHERE CarId = @PhotoId
		END
	IF(@Category ='carcomparisionlist')
		BEGIN
			UPDATE Con_CarComparisonList SET HostURL = @HostUrl,
											OriginalImgPath = @Environment + OriginalImgPath, 
											 IsReplicated = 1
			WHERE ID = @PhotoId
		END
		
	IF(@Category='topsellingcars')
		BEGIN
			UPDATE Con_TopSellingCars SET HostURL=@HostUrl,
										  OriginalImgPath = @Environment + OriginalImgPath,
										  IsReplicated =1
			WHERE ModelId=@PhotoId
		END
	IF(@Category='addbrand')
		BEGIN
			UPDATE Acc_Brands SET HostURL=@HostUrl, 
								  IsReplicated =1
			WHERE Id=@PhotoId
		END
	IF(@Category='additems')
		BEGIN
			UPDATE Acc_Items SET HostURL=@HostUrl, 
								 IsReplicated =1
			WHERE Id=@PhotoId
		END
	IF(@Category='additionalitems')
		BEGIN
			UPDATE Acc_ItemsAdditionalImages SET HostURL=@HostUrl, 
												 IsReplicated =1
			WHERE Id=@PhotoId
		END
	IF(@Category='showroomphotos')
		BEGIN
			--OriginalImgPath Added By Deepak on 5th Aug 2015
			UPDATE ShowRoomPhotos SET HostURL=@HostUrl,
									 IsReplicated =1,
									 DirectoryPath =  '',
									 Thumbnail = '/144X81/' + @Environment + OriginalImgPath,
									 LargeImage = '/559X314/' + @Environment + OriginalImgPath,
									 OriginalImgPath = @Environment + OriginalImgPath
			WHERE Id=@PhotoId
		END
	IF(@Category='dealercertification')
		BEGIN
			--OriginalImgPath Added By Deepak on 5th Aug 2015
			UPDATE Classified_CertifiedOrg SET HostURL=@HostUrl, 
											   IsReplicated =1,
											   DirectoryPath = '',
											   LogoURL = '/0X0/' + @Environment + OriginalImgPath,
											   OriginalImgPath = @Environment + OriginalImgPath
			WHERE Id=@PhotoId
			--mysql sync
			begin try
			exec	SyncClassifiedCertifiedOrgWithMysqlUpdate 	@PhotoId ,
					null ,
					null,
					@HostURL ,
					null,
					null,
					null,
					null,
					null,
					null,
					@Environment,
					1
			end try
			BEGIN CATCH
				INSERT INTO CarWaleMyslSyncExceptions(ModuleName,SPName,ErrorMsg,TableName,FailedId,CreatedOn,InputParameter)
				VALUES('MysqlSync','DCRM_SaveDealerCertification',ERROR_MESSAGE(),'SyncClassifiedCertifiedOrgWithMysqlUpdate',@PhotoId,GETDATE(),1)
			END CATCH			
				
			--end sync
		END
	IF(@Category='sellcarinquiry')
		BEGIN
			--OriginalImgPath Added By Deepak on 5th Aug 2015
			UPDATE TC_SellCarPhotos SET IsActive=1,
										 HostUrl = @HostUrl, 
										 StatusId = 3,
										 IsReplicated=1,
										 OriginalImgPath = @Environment + OriginalImgPath, --Added By Deepak on 5th Aug 2015
										 DirectoryPath = '',
										 --OrigFileName = '/0X0/' + @Environment + OriginalImgPath,
										 ImageUrlThumb = '/310X174/' + @Environment + OriginalImgPath,
										 --ImageUrlMedium = '/160X89/' + @Environment + OriginalImgPath,
										 ImageUrlThumbSmall = '/110X61/' + @Environment + OriginalImgPath, 
										 ImageUrlFull = '/664X374/' + @Environment + OriginalImgPath
			WHERE Id=@PhotoId 
			
			--update all carphotos
			UPDATE IMG_AllCarPhotos SET StatusId = 3 , 
										ItemStorage =@ServerList , 
										MaxServers =@MaxServers
			WHERE ItemId = @PhotoId
		END
	IF(@category='managewebsite40' or @category='managewebsite41' or @category='managewebsite42' or @category='managewebsite43' or @category='managewebsite44'or @category='managewebsite45'or @category='managewebsite46' or @category='managewebsite47')
		BEGIN
			UPDATE Microsite_Images SET HostURL = @HostUrl, 
										StatusId = 3,
										IsReplicated=1,
										OriginalImgPath = @Environment + OriginalImgPath 
			WHERE Id = @PhotoId
			UPDATE IMG_AllCarPhotos SET StatusId = 3 , 
										ItemStorage =@ServerList , 
										MaxServers =@MaxServers
			WHERE ItemId = @PhotoId
		END
	IF(@Category ='managewebsite')
		BEGIN
			UPDATE Microsite_Images SET HostURL = @HostUrl, 
										StatusId = 3,
										IsReplicated = 1,
										OriginalImgPath = @Environment + OriginalImgPath
			WHERE Id = @PhotoId
		END
	IF(@Category='cwcommunity')
		BEGIN
			UPDATE UP_Photos SET HostURL = @HostUrl, 
								 StatusId = 3,  
								 Size500 = 1, 
								 Size1024 = 1,
								 Size800 = 1,
								 IsReplicated=1,
								 OriginalImgPath = @Environment + OriginalImgPath 
			WHERE Id = @PhotoId
			UPDATE IMG_AllCarPhotos SET StatusId = 3 , 
										ItemStorage =@ServerList , 
										MaxServers =@MaxServers
			WHERE ItemId = @PhotoId
		END
		
	IF(@Category='avtarimage')
		BEGIN
			UPDATE UserProfile SET HostURL = @HostUrl, 
								   StatusId=3,
								   IsReplicated = 1,
								   AvtOriginalImgPath = @Environment + AvtOriginalImgPath
			WHERE UserId = @PhotoId
		END
	IF(@Category='forumsrealimage')
		BEGIN
			UPDATE UserProfile SET HostURL = @HostUrl, 
								   StatusId=3,
								   IsReplicated = 1,
								   RealOriginalImgPath = @Environment + RealOriginalImgPath
			WHERE UserId = @PhotoId
			UPDATE IMG_AllCarPhotos SET StatusId = 3 , 
										ItemStorage =@ServerList , 
										MaxServers =@MaxServers
			WHERE ItemId = @PhotoId
		END
	IF(@Category='usedsellcars')
		BEGIN
			UPDATE CarPhotos SET HostURL = @HostUrl, 
								 StatusId=3,
								 IsReplicated = 1,
								 OriginalImgPath = @Environment + OriginalImgPath
			WHERE Id = @PhotoId
			UPDATE IMG_AllCarPhotos SET StatusId = 3 , 
										ItemStorage =@ServerList , 
										MaxServers =@MaxServers
			WHERE ItemId = @PhotoId
		END
	--Added By Ranjeet || For Broker App images
	--IF(@Category='brokerapp')
	--	BEGIN
	--		UPDATE BA_ImageSize SET HostURL = @HostUrl, 
	--		                        StatusId=3,
	--								IsReplicated = 1
	--		WHERE Id = @PhotoId
	--		UPDATE IMG_AllCarPhotos SET StatusId = 3 , 
	--		                            ItemStorage =@ServerList , 
	--									MaxServers =@MaxServers
	--		WHERE ItemId = @PhotoId
	--	END
--Added By Ruchira Patil on 17th Dec 2014(for absure images)
-- Updated By Vinay Kumar Prajapati  Add @AspectRatio to save image 
	IF(@Category='absure')
		BEGIN
		   DECLARE @AspectRatio Float
		  -- Find Aspect Ratio then give resizing image url ... 
		   SELECT @AspectRatio= ISNULL(ACP.AspectRatio,1.33)  FROM  AbSure_CarPhotos AS ACP WITH(NOLOCK) WHERE AbSure_CarPhotosId = @PhotoId
		   IF @AspectRatio = 1.77
			   BEGIN
					UPDATE AbSure_CarPhotos SET HostURL = REPLACE(@HostUrl,'http://',''), 
											StatusId=3,
											IsReplicated = 1,
											OriginalImgPath = @Environment + OriginalImgPath, --Added By Deepak on 5th Aug 2015
											DirectoryPath = '',
											ImageUrlExtraLarge = '/1024X576/'  + @Environment + OriginalImgPath,
											ImageUrlLarge      = '/640X360/'   + @Environment + OriginalImgPath,
											ImageUrlThumb      = '/320X180/'   + @Environment + OriginalImgPath, 
											ImageUrlSmall      = '/80X45/'     + @Environment + OriginalImgPath,
											ImageUrlOriginal   = '/0X0/'       + @Environment + OriginalImgPath
					WHERE AbSure_CarPhotosId = @PhotoId
				END
			ELSE      -- For aspect ratio 1.33 is default 
				BEGIN
				
				    UPDATE AbSure_CarPhotos SET HostURL = REPLACE(@HostUrl,'http://',''), 
											StatusId=3,
											IsReplicated = 1,
											OriginalImgPath = @Environment + OriginalImgPath, --Added By Deepak on 5th Aug 2015
											DirectoryPath       = '',										
											ImageUrlExtraLarge  = '/1024x786/' + @Environment + OriginalImgPath,
											ImageUrlLarge       = '/640x480/'  + @Environment + OriginalImgPath,
											ImageUrlThumb       = '/300x225/'  + @Environment + OriginalImgPath, 
											ImageUrlSmall       = '/80x60/'    + @Environment + OriginalImgPath,
											ImageUrlOriginal    = '/0X0/'      + @Environment + OriginalImgPath	
					WHERE AbSure_CarPhotosId = @PhotoId			
				END
			UPDATE IMG_AllCarPhotos SET StatusId = 3 , 
										ItemStorage =@ServerList , 
										MaxServers =@MaxServers
			WHERE ItemId = @PhotoId
		END
	IF(@Category = 'dealerprofilephoto')
	BEGIN
		--OriginalImgPath Added By Deepak on 5th Aug 2015
		UPDATE Dealers SET ProfilePhotoHostUrl = @HostUrl,
						   ProfilePhotoStatusId = 3, 
						   ProfilePhotoUrl = '/0X0/' + @Environment + OriginalImgPath,
						   OriginalImgPath = @Environment + OriginalImgPath 
						   WHERE ID = @PhotoId	
				--mysql sync start
			declare
			@NewID	decimal(18,0) = null, @LoginId	varchar(30) = null, @Passwd	varchar(50) = null, @FirstName	varchar(100) = null, @LastName	varchar(100) = null, @EmailId	varchar(250) = null, @Organization	varchar(100) = null, @Address1 varchar(500) = null, @Address2	varchar(500) = null, @AreaId	decimal(18,0) = null, @CityId	decimal(18,0) = null, @StateId	decimal(18,0) = null, @Pincode	varchar(6) = null, @PhoneNo	varchar(50) = null, @FaxNo	varchar(50) = null, @MobileNo	varchar(50) = null, @ExpiryDate	datetime = null, @WebsiteUrl	varchar(100) = null, @ContactPerson	varchar(200) = null, @ContactHours	varchar(30) = null, @ContactEmail	varchar(250) = null, @Status	tinyint = null, @LastUpdatedOn	datetime = null, @CertificationId	smallint = null, @BPMobileNo	varchar(15) = null, @BPContactPerson	varchar(60) = null, @IsTCDealer	tinyint = null, @IsWKitSent	tinyint = null, @IsTCTrainingGiven	tinyint = null,  @TC_DealerTypeId tinyint,  @Longitude	float = null, @Lattitude	float = null, @DeletedReason	smallint = null, @HavingWebsite	tinyint = null, @ActiveMaskingNumber	varchar(20) = null, @DealerVerificationStatus	smallint = null, @IsPremium	tinyint = null, @WebsiteContactMobile	varchar(50) = null, @WebsiteContactPerson	varchar(100) = null, @ApplicationId	tinyint, @IsWarranty	tinyint = null, @LeadServingDistance	smallint = null, @OutletCnt	int = null, @ProfilePhotoHostUrl	varchar(100) = null, @ProfilePhotoUrl	varchar(250) = null, @AutoInspection	tinyint = null, @RCNotMandatory	tinyint = null,  @OwnerMobile	varchar(20) = null, @ShowroomStartTime	varchar(30) = null, @ShowroomEndTime	varchar(30) = null, @DealerLastUpdatedBy	int = null, @LegalName	varchar(50) = null, @PanNumber	varchar(50) = null, @TanNumber	varchar(50) = null, @AutoClosed	tinyint = null, @LandlineCode	varchar(4), @Ids Varchar(MAX), @IsDealerDeleted bit = null, @DeleteComment varchar(500)  = null, @DeletedOn datetime =null , @PackageRenewalDate datetime = null, @DealerSource int = null,@LogoUrl varchar(100) = null
			set @UpdateType = 1
			set @NewID=@PhotoId
			set @ProfilePhotoUrl=@Environment
			begin try
			exec [dbo].[SyncDealersWithMysqlUpdate] 
			@NewID	, @LoginId	, @Passwd	, @FirstName	, @LastName	, @EmailId	, @Organization	, @Address1 , @Address2	, @AreaId	, @CityId	, @StateId	, @Pincode	, @PhoneNo	, @FaxNo	, @MobileNo	, @ExpiryDate	, @WebsiteUrl	, @ContactPerson	, @ContactHours	, @ContactEmail	, @Status	, @LastUpdatedOn	, @CertificationId	, @BPMobileNo	, @BPContactPerson	, @IsTCDealer	, @IsWKitSent	, @IsTCTrainingGiven	, @HostURL	, @TC_DealerTypeId ,  @Longitude	, @Lattitude	, @DeletedReason	, @HavingWebsite	, @ActiveMaskingNumber	, @DealerVerificationStatus	, @IsPremium	, @WebsiteContactMobile	, @WebsiteContactPerson	, @ApplicationId	, @IsWarranty	, @LeadServingDistance	, @OutletCnt	, @ProfilePhotoHostUrl	, @ProfilePhotoUrl	, @AutoInspection	, @RCNotMandatory	, @OriginalImgPath	, @OwnerMobile	, @ShowroomStartTime	, @ShowroomEndTime	, @DealerLastUpdatedBy	, @LegalName	, @PanNumber	, @TanNumber	, @AutoClosed	, @LandlineCode	, @Ids , @UpdateType , @IsDealerDeleted , @DeleteComment , @DeletedOn  , @PackageRenewalDate, @DealerSource, @LogoUrl
			end try
			BEGIN CATCH
				INSERT INTO CarWaleMyslSyncExceptions(ModuleName,SPName,ErrorMsg,TableName,FailedId,CreatedOn,InputParameter)
				VALUES('MysqlSync','IMG_CarPhotosUpdateReplicateRQ_v15.8.1',ERROR_MESSAGE(),'SyncDealersWithMysqlUpdate',@PhotoId,GETDATE(),@UpdateType)
			END CATCH
			-- mysql sync end
	END
	
	--OriginalImgPath Added By Deepak on 5th Aug 2015
	IF(@Category = 'dmsscreenshot')
	BEGIN
		UPDATE TC_NewCarInquiries SET DMSScreenShotHostUrl = @HostUrl,DMSScreenShotStatusId = 3, OriginalImgPath = @Environment + OriginalImgPath WHERE TC_NewCarInquiriesId = @PhotoId
	END
	IF(@Category = 'bugsscreenshot')
	BEGIN
		--OriginalImgPath Added By Deepak on 5th Aug 2015
		UPDATE TC_BugFeedback SET HostUrl = @HostUrl,
				BugScreenShotStatusId = 3,
				FilePath = '/0X0/' + @Environment + OriginalImgPath,  
				OriginalImgPath = @Environment + OriginalImgPath 
		WHERE TC_Bug_Id = @PhotoId
	END
	IF(@Category = 'splashscreen')
	BEGIN
		UPDATE AppSplashScreenSetting SET HostUrl = @HostUrl, OriginalImgPath = @Environment + OriginalImgPath, IsReplicated = 1 WHERE Id = @PhotoId
	END
	IF(@Category = 'videothumbnail')
	BEGIN
		UPDATE Con_ModelVideos SET ThumbnailHostURL = @HostUrl, OriginalImgPath = @Environment + OriginalImgPath, IsReplicated = 1 WHERE Id = @PhotoId
	END
	IF(@Category = 'cargalleryphoto')
	BEGIN
		UPDATE CarGalleryPhotos SET HostUrl = @HostUrl, OriginalImgPath = @Environment + OriginalImgPath, IsReplicated = 1 WHERE Id = @PhotoId
	END
	IF(@Category = 'carwaleoffers')
	BEGIN
		UPDATE DealerOffers SET HostUrl = @HostUrl,
								OriginalImgPath = @Environment + OriginalImgPath
		WHERE Id=@PhotoId 
	END
	-----------------------------------------------------
	--Modifier : Vaibhav K 10-Aug-2015
	IF(@Category = 'dealermodelcolors')
	BEGIN
		UPDATE Microsite_DealerModelColors
		SET HostUrl = @HostUrl,
			OriginalImgPath = @Environment + OriginalImgPath,
			IsActive = 1
		WHERE Id = @PhotoId
	END
	IF(@Category = 'dealermodelfeatures')
	BEGIN
		UPDATE Microsite_DWModelFeatures
		SET HostUrl = @HostUrl,
			OriginalImgPath = @Environment + OriginalImgPath,
			IsActive = 1
		WHERE Id = @PhotoId
	END
	IF(@Category = 'dealermodels')
	BEGIN
		UPDATE TC_DealerModels
		SET HostUrl = @HostUrl,
			OriginalImgPath = @Environment + OriginalImgPath,
			IsDeleted = 0
		WHERE Id = @PhotoId
	END
	IF(@Category = 'dealeroffers')
	BEGIN
		UPDATE Microsite_DealerOffers
		SET HostUrl = @HostUrl,
			OriginalImgPath = @Environment + OriginalImgPath,
			IsActive = 1
		WHERE Id = @PhotoId
	END
	--Vaibhav K 16-sept-2015
	--category added for dealer offer slug images in Manage Website - Autobiz
	IF(@Category = 'dealeroffersslug')
	BEGIN
		UPDATE Microsite_OfferImages
		SET HostUrl = @HostUrl,
			OriginalImgPath = @Environment + OriginalImgPath,
			IsActive = 1
		WHERE Id = @PhotoId		
	END
	IF(@Category = 'dealermodelphotos')
	BEGIN
		UPDATE Microsite_DealerModelBanners
		SET HostUrl = @HostUrl,
			OriginalImgPath = @Environment + OriginalImgPath,
			IsActive = 1
		WHERE Id = @PhotoId
	END
	IF(@Category = 'serviceoffers')
	BEGIN
		UPDATE Microsite_ServiceOffers
		SET HostUrl = @HostUrl,
			OriginalImgPath = @Environment + OriginalImgPath,
			IsActive = 1
		WHERE Id = @PhotoId
	END
	------------------------------------------------------
	IF(@Category = 'sellcarrotatephoto')
	BEGIN
		UPDATE CarPhotos
		SET HostUrl = @HostUrl,
			OriginalImgPath = @Environment + OriginalImgPath,
			DirectoryPath = '',
			ImageUrlThumb = '/310X174/' + @Environment + OriginalImgPath,
			ImageUrlMedium = '/160X89/' + @Environment + OriginalImgPath,
			ImageUrlThumbSmall = '/110X61/' + @Environment + OriginalImgPath, 
			ImageUrlFull = '/664X374/' + @Environment + OriginalImgPath
		WHERE Id = @PhotoId
	END 
	--Vaibhav K 10-oct-2015
	--Category added to repliacte offer photos for Deals On Auto
	IF(@Category = 'dealsonauto')
	BEGIN
		UPDATE DOA_Offers
		SET HostUrl = @HostUrl,
			OriginalImgPath = @Environment + OriginalImgPath
		WHERE Id = @PhotoId
	END
	--Added by Vinay Kumar prajapati  27th Oct 2015
	IF(@Category = 'absurereportproblem')
		BEGIN
				UPDATE Absure_ReportProblemPhotos 
				SET HostURL             = @HostUrl, 
					StatusId            = 3,
					IsReplicated        = 1,
					DirectoryPath       = '',										
					ImageUrlOriginal    = '/0X0/' + @Environment + ImageUrlOriginal	
			   WHERE Absure_ReportProblemPhotosId = @PhotoId		
	    END
	IF(@Category = 'stockimageapi')
	BEGIN
		UPDATE CarPhotos SET HostURL = @HostUrl, 
								 StatusId=3,
								 IsReplicated = 1,
								 IsApproved = 1, --added by Afrose
								 OriginalImgPath = @Environment + OriginalImgPath
			WHERE Id = @PhotoId
	END
	--added by Harshil 28 Sept.
	IF(@Category = 'testimonial')
	BEGIN
			UPDATE Con_testimonial SET StatusId = 3 , OriginalImgPath  = @Environment + OriginalImgPath
			Where Id = @PhotoId
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
									        'dbo.IMG_CarPhotosUpdateReplicateRQ_v15.8.1',
											 ERROR_MESSAGE(),
											 @Category,
											 @PhotoId,
											 GETDATE()
                                            )
	END CATCH 
END     

