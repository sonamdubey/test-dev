IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_SyncStock]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_SyncStock]
GO

	
-- Created By:	Binu
-- Modified Date : 30 Aug 2011
-- Surendra Modified 3-12-2011 	Added BranchId I/p parameter for implementing Security
-- Surendra Modified 3-3-2012 Solved the issue of not uploading stock and alo put Transaction and handled exception
-- Reshma Shetty  Modified 30/08/2012  Code has been added to calculate EMI for HDFC empaneled dealers
-- AM Modified 6-11-2012 to insert PhotoId from TC_CarPhotos to CarPhotos
-- Modified By:	Tejashree Patil on 8 Nov 2012 at 4pm Description: @Color VARCHAR(50) from VARCHAR(30)
-- Modified By:	Tejashree Patil on 5 Dec 2012 at 4pm Description: Added @CarDriven in InsertSellInquiries previous it was ''(null)
-- Modified By: Nilesh Utture on 11th March, 2013 Updated LastUpdatedDate in TC_Stock
-- Modified By : Tejashree Patil on 7 Aug 2013, Fetched PackageType from ConsumerCreditPoint instead of hardcoded value 2.
-- Modified By  Vivek Gupta on 25/11/13, Declared @IsPremium to insert or update premium details.
-- Modified By Vivek Gupta on 6th jan 2014, added @Videourl variable to save youtube link while uploading car in carwale
-- Modified By Vivek Gupta on 21-01- 2014, added INSERT STATEMENT IN EXCEPTION TABLE.
-- Modified by Vivek gupta on 04-03-2014 for resolving bug related to VideoUrl 
-- Modified by Manish on 22-04-2014 added  WITH (NOLOCK) keyword wherever not found.
-- Modified By Vivek Gupta, on 20/05/2014, Added @ImageUrlMedium while updating from TC_CarPhotos to CarPhotos
-- Modified By Vivek Gupta on 10-11-2014 added @EMI parameter
-- Modified By Tejashree Patil on 16 Feb 2015, 1. If score is >= 60% Change the certification logo url with absure logo url at both live listing and sell inquiries table
--				 2. if score < 60% check if the stock id already have absure logo url, replace it with dealers certification url else no changes.
-- Modified By Tejashree Patil on 3 March 2015, 1.Updated CarScore and Warranty in livelisting.
-- Modified by Manish on 02 July 2015 commented explicit transactions.
-- Modified By Vivek Gupta on 11-08-2015 added @OrgImgPath to insert into carphotos while stock sync
-- Modified By Vaibhav K 30 Aug 2016 to stop upload stock to carwale in case of migrated dealer
-- =============================================

CREATE PROCEDURE [dbo].[TC_SyncStock]

(
@StockIdChain VARCHAR(1000),
@BranchId BIGINT,
@Separator CHAR(1),
@PackageExpDate DATETIME,
@Status bit output
)  
AS
  
BEGIN  
SET NOCOUNT ON
  
-- @StockIdChain is the array we wish to parse  
-- @Separator is the separator charactor such as a comma  
  set @Status = 0  
   DECLARE @RowCount INT 
   --Vaibhav K 30 Aug 2016 to stop upload stock to carwale in case of migrated dealer
   SELECT Id FROM CWCTDealerMapping cmp	with(nolock) WHERE cmp.CWDealerID = @BranchId AND isnull(IsMigrated,0) = 1
  
   SET @RowCount = @@ROWCOUNT 
   IF @ROWCOUNT = 0
   BEGIN
	   DECLARE @IsPremium BIT -- Modified By  Vivek Gupta on 25/11/13, Declared @IsPremium to insert or update premium details.
	   DECLARE @Separator_position INT -- This is used to locate each separator character  
	   DECLARE @array_value VARCHAR(1000) -- this holds each array value as it is returned  
	  -- For my loop to work I need an extra separator at the end. I always look to the  
	  -- left of the separator character for each array value  
	  -- Modified By Vivek Gupta on 6th jan 2014
	   DECLARE @VideoUrl VARCHAR(20) = NULL

	  SELECT @IsPremium = IsPremium FROM Dealers WITH(NOLOCK) WHERE ID=@BranchId
	  -- Modified By  Vivek Gupta on 25/11/13, Declared @IsPremium to insert or update premium details.
		SET @StockIdChain = @StockIdChain + @Separator  
  
	  -- Loop through the string searching for separtor characters    
		WHILE PATINDEX('%' + @Separator + '%', @StockIdChain) <> 0   
			BEGIN  			
				-- patindex matches the a pattern against a string  
				SELECT  @Separator_position = PATINDEX('%' + @Separator + '%',@StockIdChain)  
				SELECT  @array_value = LEFT(@StockIdChain, @Separator_position - 1)  
				-- This is where you process the values passed.

				-- Replace this select statement with your processing  
				-- @array_value holds the value of this element of the array  
			   --SELECT  Array_Value = @array_value 
				IF EXISTS(SELECT Id FROM TC_Stock WITH(NOLOCK) WHERE Id= @array_value AND BranchId=@BranchId AND StatusId=1 AND IsActive=1)
				BEGIN --Added on- 2nd Dec 2011, checking this stock is realy blongs to logged in Deealer
				
					BEGIN TRY
						--BEGIN TRANSACTION  
							-- ModifiedBy:	Tejashree Patil on 8 Nov 2012 at 4pm 
							DECLARE @StockId numeric, @DealerId numeric,@VersionId numeric, @Price numeric,  
							@Kms numeric, @MakeYear datetime,@Color varchar(50), @ColorCode varchar, @RegNo varchar(50), @RegPlace varchar(50),  
							@Owners varchar(50), @OneTimeTax varchar(50), @Insurance varchar(50), @InsuranceExpiry datetime,  
							@LastUpdatedDate datetime, @InteriorColor varchar(50),@InteriorColorCode varchar(6),  
							@CityMileage varchar(50), @AdditionalFuel varchar(50),@CarDriven varchar(50),  
							@Accidental bit, @FloodAffected bit, @Warranties varchar(500), @Modifications varchar(500),  
							@Comments varchar(500), @ACCondition varchar(50), @BatteryCondition varchar(50),  
							@BrakesCondition varchar(50), @ElectricalsCondition varchar(50), @EngineCondition varchar(50),  
							@ExteriorCondition varchar(50), @InteriorCondition varchar(50), @SeatsCondition varchar(50),  
							@SuspensionsCondition varchar(50), @TyresCondition varchar(50), @OverallCondition varchar(50),  
							@Features_SafetySecurity varchar(200), @Features_Comfort varchar(200), @Features_Others varchar(200), 
							@EntryDate datetime= getdate(), @ImportChecksum numeric = -1, @RecordId numeric,   
							@PackageType int , @PackageExpiryDate datetime = @PackageExpDate, @CertificationId smallint, @EMI INT  -- Modified By Vivek Gupta on 10-11-2014
							-----------------------------  Added By Tejashree Patil on 7 Aug 2013  -------------------------------
							SELECT		Top 1 @PackageType=PackageType 
							FROM		ConsumerCreditPoints WITH(NOLOCK)
							WHERE		ConsumerType=1 AND ConsumerId=@BranchId 
							ORDER BY	ExpiryDate DESC
							------------------------------------------------------------------------------------------------------
							SELECT @StockId = St.Id, @DealerId = BranchId, @VersionId = VersionId, @Price = Price,  
							@Kms = Kms, @MakeYear = MakeYear, @Color = Colour, @RegNo = RegNo, @Comments = Comments, @Owners = Owners, @RegPlace = RegistrationPlace,  
							@OneTimeTax = OneTimeTax, @Insurance = Insurance, @InsuranceExpiry = InsuranceExpiry,  
							@InteriorColor = InteriorColor, @InteriorColorCode = InteriorColorCode, @CityMileage = CityMileage,  
							@AdditionalFuel = AdditionalFuel, @CarDriven = CarDriven, @Accidental = Accidental,  
							@FloodAffected = FloodAffected, @Warranties = Warranties, @Modifications = Modifications,  
							@Comments = Comments, @ACCondition = ACCondition, @BatteryCondition = BatteryCondition,  
							@BrakesCondition = BrakesCondition, @ElectricalsCondition = ElectricalsCondition, @EngineCondition = EngineCondition,  
							@ExteriorCondition = ExteriorCondition, @InteriorCondition = InteriorCondition, @SeatsCondition = SeatsCondition,  
							@SuspensionsCondition = SuspensionsCondition, @TyresCondition = TyresCondition, @OverallCondition = OverallCondition,  
							@Features_SafetySecurity = Features_SafetySecurity, @Features_Comfort = Features_Comfort, @Features_Others = Features_Others ,@CertificationId = CertificationId ,
							@EMI = EMI  -- Modified By Vivek Gupta on 10-11-2014
						 
							FROM TC_Stock St WITH(NOLOCK) , TC_CarCondition Con WITH(NOLOCK)
							WHERE St.Id = Con.StockId AND St.Id = @array_value

							-- get inquiry if car is already synced if not set inquiry id to -1  
							declare @cw_inquiryid BIGINT 
							SET @cw_inquiryid=NULL 
							SELECT @cw_inquiryid = Id FROM SellInquiries Si WITH(NOLOCK) WHERE Si.TC_StockId = @StockId AND DealerId = @BranchId AND SI.SourceId = 2 --2 for Autobiz
						
							if @cw_inquiryid is null
							Begin
								set @cw_inquiryid = -1
							END
                        
							--Execute SP to upload stock details
							-- Note: if @cw_inquiryid is -1 than perform insert operation else update operation

							---- Added By Vivek Gupta on 6th jan 2014
							--Commneted By Deepak on 26th Aug 2016
							IF EXISTS (SELECT Id FROM TC_CarVideos WITh(NOLOCK) WHERE StockId = @StockId AND VideoUrl IS NOT NULL AND IsActive=1)
							BEGIN
							 SET @VideoUrl = (SELECT VideoUrl FROM TC_CarVideos WITH(NOLOCK) WHERE StockId = @StockId AND IsActive=1)

							-- UPDATE livelistings 
							-- SET	VideoCount = 1 
							-- FROM	TC_CarVideos WITH(NOLOCK) 
							-- WHERE	StockId = @StockId AND IsActive=1

							END

							--SELECT  @cw_inquiryid cw_inquiryid,@DealerId DealerId,@VersionId VersionId,@RegNo RegNo,1,@EntryDate EntryDate,@Price Price,  
							--@MakeYear MakeYear,@Kms Kms,@Color Color,@ColorCode ColorCode,@Comments Comments,@ImportChecksum ImportChecksum,@RecordId output,'',0,  
							--@PackageType PackageType,@PackageExpiryDate PackageExpiryDate,@Owners Owners,@RegPlace RegPlace,@OneTimeTax oneTimeTax,  
							--@Insurance Insurance,@InsuranceExpiry InsuranceExpiry,@EntryDate EntryDate/*last updated*/,@InteriorColor InteriorColor,  
							--@InteriorColorCode InteriorColorCode,@CityMileage CityMileage,@AdditionalFuel AdditionalFuel,
							--@CarDriven CarDriven,  -- Modified By:	Tejashree Patil on 5 Dec 2012
							--@Accidental Accidental,@FloodAffected FloodAffected,@Warranties Warranties,
							--@Modifications Modifications,@ACCondition ACCondition,@BatteryCondition BatteryCondition,@BrakesCondition BrakesCondition,@ElectricalsCondition ElectricalsCondition,  
							--@EngineCondition EngineCondition,@ExteriorCondition ExteriorCondition,@SeatsCondition SeatsCondition,@SuspensionsCondition SuspensionsCondition,  
							--@TyresCondition TyresCondition,@OverallCondition OverallCondition,@Features_SafetySecurity Features_SafetySecurity,
							--@Features_Comfort Features_Comfort,@Features_Others Features_Others, @CertificationId CertificationId,@InteriorCondition InteriorCondition,
							--@IsPremium IsPremium,@VideoUrl VideoUrl, -- Modified By  Vivek Gupta on 25/11/13 -- Modified By Vivek Gupta on 6th jan 2014                    
							--@EMI EMI

							EXEC InsertSellInquiry @cw_inquiryid,@DealerId,@VersionId,@RegNo,1,@EntryDate,@Price,  
							@MakeYear,@Kms,@Color,@ColorCode,@Comments,@ImportChecksum,@RecordId output,'',0,  
							@PackageType,@PackageExpiryDate,@Owners,@RegPlace,@OneTimeTax,  
							@Insurance,@InsuranceExpiry,@EntryDate/*last updated*/,@InteriorColor,  
							@InteriorColorCode,@CityMileage,@AdditionalFuel,
							@CarDriven,  -- Modified By:	Tejashree Patil on 5 Dec 2012
							@Accidental,@FloodAffected,@Warranties,
							@Modifications,@ACCondition,@BatteryCondition,@BrakesCondition,@ElectricalsCondition,  
							@EngineCondition,@ExteriorCondition,@SeatsCondition,@SuspensionsCondition,  
							@TyresCondition,@OverallCondition,@Features_SafetySecurity,
							@Features_Comfort,@Features_Others, @CertificationId,@InteriorCondition,@IsPremium,@VideoUrl, -- Modified By  Vivek Gupta on 25/11/13 -- Modified By Vivek Gupta on 6th jan 2014                    
							@EMI  -- Modified By Vivek Gupta on 10-11-2014

							if @RecordId is not null  -- toggle flag if and only if stock uploaded to carwale successfully
							begin
								-- If car uploaded to the CarWale successfully toggle the flag 'IsSychronizedCW' to true
								update TC_Stock set IsSychronizedCW = 1, LastUpdatedDate = GETDATE() where Id = @StockId
							
								-- Modified By: Reshma Shetty Date: 30/08/2012 -- To calculate EMI for HDFC empaneled dealers
								-- Since the car is successfully uploaded Calculate EMI for it
								-- Modified 26/09/2012 Passing @DiffYear instead of 5-@DiffYear due to change in requirements
								  DECLARE @DiffYear TINYINT
								  SET @DiffYear = DATEDIFF(YEAR,@MakeYear,GETDATE())
							  
								  IF(dbo.HDFCVehicleEligiblity(@DealerId,@Price,@Owners,@DiffYear)=1)
								  BEGIN
									UPDATE Sellinquiries
									SET CalculatedEMI = dbo.CalculateEMI(@Price,@DiffYear,16.5)
									WHERE Sellinquiries.ID = @RecordId 
								  END
								  ELSE
								  BEGIN
									UPDATE Sellinquiries
									SET CalculatedEMI = NULL
									WHERE Sellinquiries.ID = @RecordId 
								  END
        
								-- Check if any photos available to upload
								declare @photoId numeric, @img_large varchar(150), @img_medium varchar(150), @img_small varchar(150), @HostUrl varchar(100), 
								@is_main bit, @directory_path varchar(50), @ret_id numeric ,@IsReplicaed BIT, @ImageUrlMedium VARCHAR(100), -- Modified By Vivek Gupta, on 20/05/2014
								@OrgImgPath VARCHAR(100)

								select @photoId = min(id) from TC_CarPhotos WITH(NOLOCK) where StockId = @StockId and IsActive = 1

								while @photoId is not null
									begin
										select @img_large = ImageUrlFull, @img_medium = ImageUrlThumb, @img_small = ImageUrlThumbSmall, 
										@is_main = IsMain,@HostUrl=HostUrl,@directory_path=DirectoryPath,@IsReplicaed=IsReplicated , @ImageUrlMedium = ImageUrlMedium,
										@OrgImgPath = OriginalImgPath -- Modified By Vivek Gupta, on 11-08-2015
										from TC_CarPhotos WITH(NOLOCK) where Id = @photoId
									
										-- Commented By Surendra on 25 Jan,2012
										--set @directory_path = '/tc/' + CONVERT(varchar, @DealerId) + '/'
									
										if not exists(select id from CarPhotos  WITH(NOLOCK) where ImageUrlFull = @img_large)
										begin
											EXEC Classified_CarPhotos_Insert @RecordId,@img_large,@img_medium,@img_small,'',1,@is_main,@directory_path, @HostUrl, @ret_id output,@IsReplicaed,@OrgImgPath
										
											-- AM Modified 6-11-2012 to insert PhotoId from TC_CarPhotos to CarPhotos
											UPDATE CarPhotos
											SET TC_CarPhotoId=@photoId , ImageUrlMedium = @ImageUrlMedium -- Modified By Vivek Gupta, on 20/05/2014
											WHERE ID=@ret_id
													
										end
										select @photoId = min(id) from TC_CarPhotos WITH(NOLOCK) where StockId = @StockId and id > @photoId and IsActive = 1
									end
								-- Also upload all available photos of the stock		
							end	 

							-- @cw_inquiryid is -1 means this car uploaded first time to CarWale
							-- link stock id with carwale inquiry id 
							if @cw_inquiryid = -1  
							update SellInquiries set TC_StockId = @StockId where ID = @RecordId  

						
							IF(@VideoUrl IS NOT NULL)
							BEGIN
								DELETE FROM CarVideos WHERE InquiryId=@RecordId   AND IsActive = 1        					    
								INSERT INTO CarVideos
								(InquiryId,IsDealer,IsMain,IsApproved,TC_CarVideoId,VideoUrl)
								VALUES
								((SELECT ID FROM SellInquiries WITH(NOLOCK) WHERE TC_StockId = @StockId AND DealerId = @BranchId AND SourceId = 2),1,1,1,(SELECT Id FROM TC_CarVideos WITH(NOLOCK) WHERE  IsActive = 1 AND StockId = @StockId),@VideoUrl)

								UPDATE TC_CarVideos SET StatusId = 1, ModifiedDate = GETDATE(), IsSellerInq = 1 WHERE StockId = @StockId
							
								--Added By Deepak on 26th Aug 2016
								UPDATE livelistings SET	VideoCount = 1 WHERE SellerType = 1 
									AND Inquiryid IN(SELECT ID FROM SellInquiries WITH(NOLOCK) WHERE TC_StockId = @StockId AND DealerId = @BranchId AND SourceId = 2)
							
							END

							set @Status = 1

							/*************************** Modified By Tejashree Patil on 16 Feb 2015 to update Absure certification based on criteria *****************************/
							/*DECLARE @CarScore INT = NULL
							SELECT	@CarScore = CarScore  
							FROM	AbSure_CarDetails WITH(NOLOCK) 
							WHERE	StockId = @StockId

							IF(@CarScore IS NOT NULL)
							BEGIN*/
							EXECUTE AbSure_ChangeCertification @StockId, NULL,NULL
							--END
							/********************************************************/

						--COMMIT TRANSACTION  
					END TRY
							
					BEGIN CATCH
								--ROLLBACK TRAN 
								 INSERT INTO TC_Exceptions
											  (Programme_Name,
											   TC_Exception,
											   TC_Exception_Date,
											   InputParameters)
								 VALUES('TC_SyncStock',
								 (ERROR_MESSAGE()+'ERROR_NUMBER(): '+CONVERT(VARCHAR,ERROR_NUMBER())),
								 GETDATE(),
								 '@StockIdChain:' + ISNULL(@StockIdChain,'NULL') + 
								 ' @BranchId :'+ ISNULL(CONVERT(VARCHAR,@BranchId),'NULL') + 
								 ' @Separator : '+	ISNULL(@Separator,'NULL')  +
								 ' @PackageExpDate : '+ISNULL(CAST( @PackageExpDate AS VARCHAR(50)),'NULL')+
								 ' @Status: ' + ISNULL(CAST( @Status AS VARCHAR(50)),'NULL')
								 )
								SELECT ERROR_MESSAGE();
					END CATCH;
				END 	
			 
			                 
				-- This replaces what we just processed with and empty string 
				SET @VideoUrl= NULL  --- Modified by Vivek gupta on 04-03-2014 for resolving bug related to VideoUrl 
				SELECT  @StockIdChain = STUFF(@StockIdChain, 1, @Separator_position, '')  
			END -- while end
		END
SET NOCOUNT OFF  
END

