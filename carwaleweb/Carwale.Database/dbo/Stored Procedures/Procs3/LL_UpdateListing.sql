IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[LL_UpdateListing]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[LL_UpdateListing]
GO

	
---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
--Modified date 16-11-2012 by manish(AE1665) for additional parameter used car scoring
--Modified by Reshma Shetty(16-1-2013)removed parameters fuel type and owners since no longer used in calculation
--Modified date 15-04-2013 by manish(AE1665) for updating CertifiedLogoUrl in LiveListings table.
--Modified by Reshma Shetty(03-05-2013)Added parameter owners to update the newly introduced Owners field in LiveListing
--(has been removed handled in sorting)Modified by Reshma Shetty(05-08-2013)Added PackageType priority to the calculated Score
-- Added by Avishkar 14-10-2013 to populate DealerId in Livelistings table
-- Added by Avishkar 14-11-2013 for @IsPremium 
--Modified by Manish(AE1665) on 12 Feb 2014 for update OfferStartDate and EndDate in Livelistings table
--Modified By: Avishkar on 10-4-2014  To set lead score for  live listings
--Modified by Manish on 30-04-2014 for automated comment implementation
--Modified by Manish on 21-05-2014 for sync of RootId,RootName and OwnerTypeId field in Livelistings table.
-- Modified by: Manish on 28-05-2014 correction for the bug fixing when dealer car and individual car having same inquiry id.
-- Modified BY : Manish on 09-08-2014 for Updation of ImageUrlMedium on Livelistings table.
-- Modified by : Manish on 24-07-2014 added with (nolock) keyword
-- Modified By Vivek Gupta on 10-11-2014, added @UsedCarEMI to copy stock EMI to livelisting
-- Added column SellerName,SellerContact By Sadhana Upadhyay on 20 Apr 2015 
-- Modified by Manish on 30 June 2015 for optimization purpose (No change in business logic) discussed on 29 June meeting. Refer email from Shikhar dated 30 June 2015 and also added Response Parameter.
-- Added column OriginalImgPath for new image processing by Navead on 08-Aug-2015
-- Added new variable @HostUrl for passing hosturl during insert and update for new image processing by Navead on 08-Aug-2015
-- Modified By Supriya Bhide(22-1-2016) changed @SortScore calculation logic
-- Modified By Supriya Bhide(02-05-2016), changed @PhotoCount calculation sequence, before using it in CarScore calculation
 --Modified By : Tejashree Patil on 8 Feb 2016, Added VideoCount in LiveListings.
 --Modified by Prachi Phalak on 17th Feb,2016, added bucket 9 for Dealer Premium Pan-India Package
 --Modified by Supriya Bhide(28-04-2016), Added 3 new packages for bucketing - Optimizer20, Maxi60 and Premier100
 --Modified by Supriya Bhide(19-05-2016), Removed 2 new packages from bucketing - Optimizer20 and Premier100
 --Modified by Supriya Bhide(31-05-2016), Added bucket for Maximizer Plus
 --Modified by Supriya Bhide(28-06-2016), Start consuming new sortscore functions
CREATE PROCEDURE [dbo].[LL_UpdateListing] @ProfileId AS VARCHAR(50)
	,@SellerType AS SMALLINT
	,@Seller AS VARCHAR(50)
	,@Inquiryid AS INT
	,@MakeId AS INT
	,@MakeName AS VARCHAR(100)
	,@ModelId AS INT
	,@ModelName AS VARCHAR(100)
	,@VersionId AS INT
	,@VersionName AS VARCHAR(100)
	,@StateId AS INT
	,@StateName AS VARCHAR(100)
	,@CityId AS INT
	,@CityName AS VARCHAR(100)
	,@AreaId AS INT
	,@AreaName AS VARCHAR(100)
	,@Lattitude AS DECIMAL(18, 4)
	,@Longitude AS DECIMAL(18, 4)
	,@MakeYear AS DATETIME
	,@Price AS   FLOAT
	,@Kilometers AS FLOAT
	,@Color AS VARCHAR(100)
	,@Comments AS VARCHAR(MAX)
	,@EntryDate AS DATETIME
	,@LastUpdated AS DATETIME
	,@PackageId AS SMALLINT
	,@PackageType AS SMALLINT
	,@ShowDetails AS BIT
	,@Priority AS SMALLINT
	,@Owners AS VARCHAR(20)
	,@CertificationId AS SMALLINT = NULL
	,@AdditionalFuel AS VARCHAR(50) = NULL
	,@EMI AS INT = NULL
	,@DealerId AS INT = NULL   -- Added by Avishkar 14-10-2013 to populate DealerId in Livelistings table,
	,@IsPremium AS BIT = 0    -- Added By Avishkar(13-11-2013) 
	,@UsedCarEMI AS INT = NULL   -- Modified By Vivek Gupta on 10-11-2014
	,@SellerName AS VARCHAR(100)  --Added By Sadhana Upadhyay on 20 Apr 2015
	,@SellerContact AS VARCHAR(20) --Added By Sadhana Upadhyay on 20 Apr 2015
	,@Responses INT=NULL           --Added by Manish on 30 June 2015
AS
BEGIN

	DECLARE @CertifiedLogoUrl VARCHAR(200)
	       ,@OverallCondition VARCHAR(100)
	       ,@RootId SMALLINT
		   ,@RootName VARCHAR(50)
		   ,@OwnerTypeId TINYINT
           ,@PhotoCount INT
	       ,@FrontImagePath VARCHAR(300)
		   ,@ImageUrlMedium VARCHAR(250)
		   ,@SCORE FLOAT 
		   ,@OfferStartDate DATE
		   ,@OfferEndDate   DATE
		   ,@SortScore  FLOAT
		   ,@PackagePriority TINYINT
		   ,@OriginalImgPath varchar(300) ---Added by Navead on 06/08/2015
		   ,@HostUrl varchar(100) ---Added by Navead on 18/08/2015
		   ,@VideoCount SMALLINT = NULL --Added by Tejashree Patil on 8 Feb 2016.

     BEGIN TRY
				SELECT @PhotoCount = COUNT(Id)	-- Modified By Supriya Bhide(02-05-2016)
				FROM CarPhotos WITH (NOLOCK)
				WHERE InquiryId = @Inquiryid
					AND IsDealer = (
									CASE 
									WHEN @SellerType = 1  THEN 1
														  ELSE 0
									END
								   )
					AND IsActive = 1
					AND IsApproved = 1

				SELECT @RootId = CM.RootId
					  ,@RootName = CMR.RootName
				FROM CarModels     AS CM WITH (NOLOCK)
				JOIN CarModelRoots AS CMR WITH (NOLOCK) ON CM.RootId = CMR.RootId
				WHERE CM.ID = @ModelId

	
			   SET @OwnerTypeId = CASE 
					WHEN @Owners = 'First Owner '
						THEN 1
					WHEN @Owners = 'Second Owner '
						THEN 2
					WHEN @Owners = 'Third Owner '
						THEN 3
					WHEN @Owners = 'Fourth Owner'
						THEN 4
					WHEN @Owners = 'UnRegistered Car'
						THEN 6
					WHEN @Owners = 'N/A'
						THEN 7
					ELSE 5
					END

	
				IF (@CertificationId IS NOT NULL)
				BEGIN
					SELECT @CertifiedLogoUrl = HostURL + DirectoryPath + LogoURL
					FROM Classified_CertifiedOrg WITH (NOLOCK)
					WHERE Id = @CertificationId
				END



				SELECT  @Score = round(dbo.UsedCarScoreCalcWithparm(@Price, @Kilometers, ISNULL(@PhotoCount, 0)), 6);

				SELECT @PackagePriority=[Priority] 
				FROM  PackageTypePriority PT WITH (NOLOCK) 
				WHERE PackageType=@PackageType;

				IF (@Responses IS NULL)
				BEGIN 
					SELECT @Responses=Responses  
					FROM livelistings  WITH (NOLOCK) 
					WHERE  Inquiryid=@Inquiryid
					   AND SellerType=@SellerType
				END
				
				--Modified by Supriya Bhide(28-06-2016)
				-- fetching the newscore from paidsellerscore is still pending
				SELECT @SortScore = 
						CASE(@SellerType)
							WHEN 1 THEN dbo.CalculateSortScoreForDealer(@PackageId, 0, @Score, null, @PhotoCount, 0)
							WHEN 2 THEN dbo.CalculateSortScoreForIndividual(@IsPremium, @Score, 0, @PhotoCount)
						END

				--Added By : Tejashree Patil on 8 Feb 2016
				IF @SellerType = 1
				BEGIN
					SELECT  @VideoCount = IsYouTubeVideoApproved
					FROM	SellInquiriesDetails WITH (NOLOCK)
					WHERE	SellInquiryId = @Inquiryid
				END

					IF (@SellerType=1)
					BEGIN
								SELECT 
										 @OfferStartDate=MIN(TCS.StartDate) 
										,@OfferEndDate=MAX(TCS.EndDate)
									FROM TC_MappingOfferWithStock TCS WITH (NOLOCK)
									JOIN SellInquiries AS SI WITH (NOLOCK) ON SI.TC_StockId = TCS.StockId
									WHERE (
											TCS.IsActive = 1
											OR TCS.EndDate > CONVERT(DATE, GETDATE())
										  )
									 AND SI.ID = @Inquiryid
									 GROUP BY SI.ID
			
					   END 								

				IF @PhotoCount > 0
				BEGIN
					SELECT 
						 @FrontImagePath = C.DirectoryPath + C.ImageUrlThumbSmall
						,@ImageUrlMedium = C.DirectoryPath + C.ImageUrlMedium 
						,@OriginalImgPath = C.OriginalImgPath --Added by Navead on 06/08/2015
						,@HostUrl = C.HostURL --Added by Navead on 18/08/2015
					FROM  CarPhotos C WITH (NOLOCK) 
					WHERE  C.IsDealer = (
										CASE 
											WHEN @SellerType = 1
												THEN 1
											ELSE 0
											END
										)
					AND C.IsMain = 1
					AND C.InquiryId = @InquiryId
					AND C.IsActive = 1
					AND C.IsApproved = 1
          
				END

			------------Below if block added by Manish on 30-04-2014 for automated comment implementation---------------
			IF     @Comments = ''
				OR @Comments IS NULL
				OR EXISTS (
							SELECT UsedCarWordsConsiderAutoCommentId
							FROM UsedCarWordsConsiderAutoComment WITH (NOLOCK)
							WHERE WORD = @Comments
						  )
			BEGIN
				IF @SellerType = 1
				BEGIN
					SELECT @OverallCondition = OverallCondition
					FROM SellInquiriesDetails WITH (NOLOCK)
					WHERE SellInquiryId = @Inquiryid
				END
				ELSE
				BEGIN
					SELECT @OverallCondition = OverallCondition
					FROM CustomerSellInquiryDetails WITH (NOLOCK)
					WHERE InquiryId = @Inquiryid
				END

		EXEC [dbo].[GetAutoGeneratedCommentSellInquiries] @CarMakeYear = @MakeYear
			,@Kms = @Kilometers
			,@OverAllCondition = @OverallCondition
			,@NoOfOwner = @Owners
			,@CarVersionId = @VersionId
			,@CarColour = @Color
			,@CityId = @CityId
			,@CertificationId = @CertificationId
			,@AutomatedComment = @Comments OUTPUT
	END

	---------------------------------------------------------------------------------------------------------
	--first update the listing. if it is not there then insert the data
	-- Modified By: Reshma Shetty Date: 30/08/2012 -- Added parameter @EMI to calculate EMI for HDFC empaneled dealers
	-- Modified By: Navead Kazi Date: 18/08/2015 -- Added parameter @HostUrl 
						
	
			IF EXISTS ( SELECT InquiryId  
						FROM livelistings WITH(NOLOCK) 
						WHERE Inquiryid=@Inquiryid
						AND   SellerType=@SellerType
					   )
			BEGIN 
						UPDATE LiveListings
						SET  MakeId = @MakeId
							,MakeName = @MakeName
							,ModelId = @ModelId
							,ModelName = @ModelName
							,VersionId = @VersionId
							,VersionName = @VersionName
							,StateId = @StateId
							,StateName = @StateName
							,CityId = @CityId
							,CityName = @CityName
							,AreaId = @AreaId
							,AreaName = @AreaName
							,Lattitude = @Lattitude
							,Longitude = @Longitude
							,MakeYear = @MakeYear
							,Price = @Price
							,Kilometers = @Kilometers
							,Color = @Color
							,Comments = @Comments
							,EntryDate = @EntryDate
							,LastUpdated = @LastUpdated
							,PackageType = @PackageType
							,ShowDetails = @ShowDetails
							,Priority = @Priority
							,CertificationId = @CertificationId
							,AdditionalFuel = @AdditionalFuel
							,CalculatedEMI = @EMI
							,Owners = @Owners
							,DealerId = @DealerId -- Added by Avishkar 14-10-2013 to populate DealerId in Livelistings table
							,CertifiedLogoUrl = @CertifiedLogoUrl  ----Line Added by Manish on 15-04-2013 for updating CertifiedLogoUrl on livelisting table
							,IsPremium = @IsPremium     -- Added By Avishkar(13-11-2013) 
							,RootId = @RootId      -- Added By Manish (21-05-2014) 
							,RootName = @RootName  -- Added By Manish (21-05-2014) 
							,OwnerTypeId = @OwnerTypeId 
							,EMI = @UsedCarEMI  -- Modified By Vivek Gupta on 10-11-2014
							,SellerName = @SellerName    -- Added By : Sadhana Upadhyay on 20 Apr 2015 To add sellerName and seller Contact no in livelisting Table
							,SellerContact = @SellerContact
							,PhotoCount  =@PhotoCount
							,FrontImagePath=@FrontImagePath
							,ImageUrlMedium=@ImageUrlMedium
							,score=@SCORE
							,OfferStartDate=@OfferStartDate
							,OfferEndDate=@OfferEndDate
							,SortScore=@SortScore
							,OriginalImgPath=@OriginalImgPath  --Added by Navead on 06/08/2015
							,HostURL=@HostUrl  --Added by Navead on 18/08/2015
						WHERE InquiryId=@Inquiryid                  --ProfileId = @ProfileId
						  AND SellerType=@SellerType
			 END
			 ELSE
				 BEGIN
				
				---------------------------------------------------------------------------------------------------------
				--since the record is not there. hence add the data
				-- Modified By: Reshma Shetty Date: 30/08/2012 -- Added parameter @EMI to calculate EMI for HDFC empaneled dealers
				-- Modified By: Navead Kazi Date: 18/08/2015 -- Added parameter @HostUrl
				        
						INSERT INTO LiveListings (
													ProfileId
													,SellerType
													,Seller
													,Inquiryid
													,MakeId
													,MakeName
													,ModelId
													,ModelName
													,VersionId
													,VersionName
													,StateId
													,StateName
													,CityId
													,CityName
													,AreaId
													,AreaName
													,Lattitude
													,Longitude
													,MakeYear
													,Price
													,Kilometers
													,Color
													,Comments
													,EntryDate
													,LastUpdated
													,PackageType
													,ShowDetails
													,Priority
													,CertificationId
													,AdditionalFuel
													,CalculatedEMI
													,CertifiedLogoUrl ----Line Added by Manish on 15-04-2013 for updating CertifiedLogoUrl on livelisting table,
													,Owners
													,DealerId 	 -- Added by Avishkar 14-10-2013 to populate DealerId in Livelistings table
													,IsPremium   -- Added By Avishkar(13-11-2013) 
													,RootId      -- Added By Manish (21-05-2014) 
													,RootName    -- Added By Manish (21-05-2014) 
													,OwnerTypeId 
													,EMI
													,SellerName
													,SellerContact
													,PhotoCount
													,FrontImagePath
													,ImageUrlMedium
													,Score
													,OfferStartDate
													,OfferEndDate
													,SortScore
													,Responses
													,OriginalImgPath  --Added by Navead on 06/08/2015
													,HostURL  --Added by Navead on 18/08/2015
													,VideoCount --Added By : Tejashree Patil on 8 Feb 2016
												)
										VALUES (
												 @ProfileId
												,@SellerType
												,@Seller
												,@Inquiryid
												,@MakeId
												,@MakeName
												,@ModelId
												,@ModelName
												,@VersionId
												,@VersionName
												,@StateId
												,@StateName
												,@CityId
												,@CityName
												,@AreaId
												,@AreaName
												,@Lattitude
												,@Longitude
												,@MakeYear
												,@Price
												,@Kilometers
												,@Color
												,@Comments
												,@EntryDate
												,@LastUpdated
												,@PackageType
												,@ShowDetails
												,@Priority
												,@CertificationId
												,@AdditionalFuel
												,@EMI
												,@CertifiedLogoUrl ----Line Added by Manish on 15-04-2013 for updating CertifiedLogoUrl on livelisting table
												,@Owners   
												,@DealerId    -- Added by Avishkar 14-10-2013 to populate DealerId in Livelistings table
												,@IsPremium   -- Added By Avishkar(13-11-2013) 
												,@RootId
												,@RootName
												,@OwnerTypeId
												,@UsedCarEMI -- Modified By Vivek Gupta on 10-11-2014
												,@SellerName  -- Added By : Sadhana Upadhyay on 20 Apr 2015 To add sellerName and seller Contact no in livelisting Table
												,@SellerContact
												,@PhotoCount
												,@FrontImagePath
												,@ImageUrlMedium
												,@SCORE
												,@OfferStartDate
												,@OfferEndDate
												,@SortScore
												,ISNULL(@Responses,0)
												,@OriginalImgPath  --Added by Navead on 06/08/2015
												,@HostUrl  --Added by Navead on 18/08/2015
												,@VideoCount  --Added By : Tejashree Patil on 8 Feb 2016
											   )
							 END
              END TRY
			  BEGIN CATCH
			         INSERT INTO CarWaleWebSiteExceptions (
															ModuleName,
															SPName,
															ErrorMsg,
															TableName,
															FailedId,
															CreatedOn
														   )
												VALUES ('Used Car Update and Insert',
														'dbo.LL_UpdateListing',
															ERROR_MESSAGE(),
															'livelistings',
															@ProfileId,
															GETDATE()
														)

			   
			  END CATCH;
	
      END

