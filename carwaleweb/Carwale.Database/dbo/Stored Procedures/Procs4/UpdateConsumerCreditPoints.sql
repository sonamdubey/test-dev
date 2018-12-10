IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[UpdateConsumerCreditPoints]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[UpdateConsumerCreditPoints]
GO

	--Created By Deepak on 19th Oct 2016
-- Modified by Manish on 12 Jun 2015 enable and disable trigger on livelistings table
-- Modified by Manish on 12 Jun 2015 enable and disable trigger on livelistings table commented
-- Modified by Manish on 19 Oct 2015 capturing package id in ConsumerCreditPoints table.
-- 24-12-2015 Naved Added for CPS Updates
--Modified By Deepak on 9th Sept 2016 for Showroom as Add-on Package
-- Modified By Vaibhav K 21 sept 2016 to check for dealer migrated to cartrade
-- if dealer is migraetd to car trade no package activation should be proceeded
--Modified By Deepak on 12th Oct 2016: Commented MySQL Code to take SP Live. Everywhere I put a comment saying --Commented By Deepak on 12th Oct 2016 for a release
CREATE PROCEDURE [dbo].[UpdateConsumerCreditPoints]  
	-- Modified by Avishkar (15-11-2013)  to set IsPremium column in SellInquiries
	/*Modified by Reshma Shetty 22/11/2013  
	Set IsPremium in SellInquiries and Dealers 
	Use HasShowroom flag in Packages table to remove hardcoding of Package Ids
	*/
	@ConsumerPkgReqId NUMERIC,  -- Id of the consumer it may be dealer or individual  
	@NewExpiryDate DATETIME,    
	@EnteredBy  SMALLINT,  -- 1 for admin and 2 for consumer  
	@EnteredById  NUMERIC,    
	@Status  NUMERIC OUTPUT,  
	@IsPkgRenewal  Bit = 0  
AS  
   
BEGIN  
-- added by Manish on 12 Jun 2015 enable and disable trigger on livelistings table-------
--ALTER TABLE dbo.LiveListings DISABLE TRIGGER TrigUpdateLiveListingCities;
	DECLARE  
	   @ConsumerId   NUMERIC,  
	   @ConsumerType  NUMERIC,  
	   @PackageId   NUMERIC,  
	   @PkgValidity    NUMERIC,  
	   @ActualInquiryPoints   NUMERIC,  
	   @ActualValidity    NUMERIC,  
	   @ActualAmount   NUMERIC,  
	   @SuggestedInqPts  NUMERIC,  
	   @SuggestedAmount  NUMERIC,  
	   @PackageType   SMALLINT,    
	   @CurrPoints    NUMERIC,  
	   @PreviousPackageType  SMALLINT,    
	   @CreditPoints   NUMERIC,  
	   @ExpiryDate   DATETIME,   
	   @LiveStockCount  NUMERIC,  
	   @IsPreviousPackage BIT,  
	   @PreviousExpiryDate DATETIME,  
	   @IsRenewal   BIT,       
	   @IsSamePreviousPackage BIT , 
	   @IsPremium BIT,
	   @HasShowroom BIT,
	   @AddonPackageTblId	INT
  
	  SET @CurrPoints = 0  
	  SET @IsPreviousPackage = 0  
	  SET @IsRenewal = @IsPkgRenewal  
	  SET @IsSamePreviousPackage = 0  
   
    declare	@InsertId int =1
	BEGIN  
	
		-- SELECT CURRENT EXP DATE  
		SELECT   
			@ConsumerId  = CPR.ConsumerId,  
			@ConsumerType = CPR.ConsumerType,  
			@PackageId  = CPR.PackageId,  
			@ActualInquiryPoints  = CPR.ActualInquiryPoints,   
			@ActualValidity = CPR.ActualValidity,  
			@ActualAmount = CPR.ActualAmount,   
			@SuggestedInqPts= P.InquiryPoints,   
			@SuggestedAmount= P.Amount,  
			@PackageType = P.InqPtCategoryId,
			@HasShowroom = P.HasShowroom -----Modified by Reshma Shetty 22/11/2013 Set @HasShowroom to be used for updation in ActiveDealers
			  
		FROM  ConsumerPackageRequests CPR with(nolock),  Packages AS P  with(nolock)
		WHERE CPR.PackageId = P.Id AND CPR.Id = @ConsumerPkgReqId  
		-----------Modified by Reshma Shetty 22/11/2013 Set @IsPremium to be used for updation in SellInquiries and Dealers
		SELECT @IsPremium=IsPremium 
		FROM InquiryPointCategory  WITH (NOLOCK) 
		WHERE Id=@PackageType
		-----------------------------------------------------------------------------------------------------
		--print @ConsumerType  
		begin try
		--Vaibhav K 21 sept 2016 to stop package activation in case of cartrade migrated dealer
	   DECLARE @RowCount INT 
	   SELECT Id FROM CWCTDealerMapping cmp	WITH(NOLOCK) 
	   WHERE cmp.CWDealerID = @ConsumerId AND ISNULL(IsMigrated,0) = 1 
	   AND @ConsumerType = 1
  
	   SET @RowCount = @@ROWCOUNT
	   IF @RowCount = 0
	   BEGIN --begin for if dealer is not migrated to cartrade
	
		--Insert into the log file  
		INSERT INTO ConsumerCreditPointsLogs( ConsumerId, PackageId, ConsumerPkgReqId, SuggestedInquiry, ActualInquiry,  
			 SuggestedPrice,  ActualPrice, EntryDate, EnteredBy,ConsumerType, EnteredById)  
		VALUES(@ConsumerId,@PackageId, @ConsumerPkgReqId, @SuggestedInqPts, @ActualInquiryPoints, @SuggestedAmount,   
			@ActualAmount, GetDate(), @EnteredBy, @ConsumerType, @EnteredById)  
  
		--Check for the consumer type  
		IF  @ConsumerType = 1 --DEALER
			BEGIN
				--Added By Deepak on 9th Sept 2016 for showroom Add-on Package
				SELECT @AddonPackageTblId = Id FROM CT_AddOnPackages WITH (NOLOCK) WHERE CWDealerId = @ConsumerId AND AddOnPackageId = 100 --DealerShowroom
	
				--added by :Khushaboo Patil on 27/04/2015
				EXEC DCRM_SaveActivatedPkg @ConsumerId , @EnteredById 
			
				-- Dealer website related task
				IF @PackageId = 36 OR  @PackageId = 37
					BEGIN
						--Save Site Data
						--If already exist then update or else insert new
						UPDATE DealerSiteCreditPoints
							SET Points = @ActualInquiryPoints,
								ExpiryDate = @NewExpiryDate
						WHERE DealerId = @ConsumerId AND PackageType = 21
						
						IF @@ROWCOUNT = 0
							BEGIN
								INSERT INTO DealerSiteCreditPoints (DealerId,Points,ExpiryDate,PackageType)
								VALUES (@ConsumerId,@ActualInquiryPoints,@NewExpiryDate,21)
							END	
					END
				ELSE
					--Dealer Package related Task
					BEGIN  
						--PREVIOUS PACKAGE DETAILS for the dealer  
						SELECT    
							@CurrPoints =  Points , @PreviousPackageType = PackageType,  
							@IsPreviousPackage = 1, @PreviousExpiryDate = ExpiryDate  
						FROM  ConsumerCreditPoints  WITH (NOLOCK) 
						WHERE   
							ConsumerId =  @ConsumerId  AND   
							ConsumerType = @ConsumerType   
							--Modified by Reshma Shetty 22/11/2013 Update the IsPremium flag if the Current Package's IsPremium does not match
						UPDATE Dealers
						SET IsPremium=@IsPremium
						WHERE Id= @ConsumerId and IsPremium<>@IsPremium
						
						-- mysql sync
						declare
						@ID	decimal(18,0) = null, @LoginId	varchar(30) = null, @Passwd	varchar(50) = null, @FirstName	varchar(100) = null, @LastName	varchar(100) = null, @EmailId	varchar(250) = null, @Organization	varchar(100) = null, @Address1 varchar(500) = null, @Address2	varchar(500) = null, @AreaId	decimal(18,0) = null, @CityId	decimal(18,0) = null, @StateId	decimal(18,0) = null, @Pincode	varchar(6) = null, @PhoneNo	varchar(50) = null, @FaxNo	varchar(50) = null, @MobileNo	varchar(50) = null,  @WebsiteUrl	varchar(100) = null, @ContactPerson	varchar(200) = null, @ContactHours	varchar(30) = null, @ContactEmail	varchar(250) = null,  @LastUpdatedOn	datetime = null, @CertificationId	smallint = null, @BPMobileNo	varchar(15) = null, @BPContactPerson	varchar(60) = null, @IsTCDealer	tinyint = null, @IsWKitSent	tinyint = null, @IsTCTrainingGiven	tinyint = null, @HostURL	varchar(100) = null, @TC_DealerTypeId tinyint,  @Longitude	float = null, @Lattitude	float = null, @DeletedReason	smallint = null, @HavingWebsite	tinyint = null, @ActiveMaskingNumber	varchar(20) = null, @DealerVerificationStatus	smallint = null,  @WebsiteContactMobile	varchar(50) = null, @WebsiteContactPerson	varchar(100) = null, @ApplicationId	tinyint, @IsWarranty	tinyint = null, @LeadServingDistance	smallint = null, @OutletCnt	int = null, @ProfilePhotoHostUrl	varchar(100) = null, @ProfilePhotoUrl	varchar(250) = null, @AutoInspection	tinyint = null, @RCNotMandatory	tinyint = null, @OriginalImgPath	varchar(250) = null, @OwnerMobile	varchar(20) = null, @ShowroomStartTime	varchar(30) = null, @ShowroomEndTime	varchar(30) = null, @DealerLastUpdatedBy	int = null, @LegalName	varchar(50) = null, @PanNumber	varchar(50) = null, @TanNumber	varchar(50) = null, @AutoClosed	tinyint = null, @LandlineCode	varchar(4), @Ids Varchar(MAX), @UpdateType int = null, @IsDealerDeleted bit = null , @DeleteComment varchar(500) = null , @DeletedOn datetime =null , @PackageRenewalDate datetime = null
						set @UpdateType = 13
						set @Id = @ConsumerId
						exec [dbo].[SyncDealersWithMysqlUpdate] 
						@ID	, @LoginId	, @Passwd	, @FirstName	, @LastName	, @EmailId	, @Organization	, @Address1 , @Address2	, @AreaId	, @CityId	, @StateId	, @Pincode	, @PhoneNo	, @FaxNo	, @MobileNo	, @ExpiryDate	, @WebsiteUrl	, @ContactPerson	, @ContactHours	, @ContactEmail	, @Status	, @LastUpdatedOn	, @CertificationId	, @BPMobileNo	, @BPContactPerson	, @IsTCDealer	, @IsWKitSent	, @IsTCTrainingGiven	, @HostURL	, @TC_DealerTypeId ,  @Longitude	, @Lattitude	, @DeletedReason	, @HavingWebsite	, @ActiveMaskingNumber	, @DealerVerificationStatus	, @IsPremium	, @WebsiteContactMobile	, @WebsiteContactPerson	, @ApplicationId	, @IsWarranty	, @LeadServingDistance	, @OutletCnt	, @ProfilePhotoHostUrl	, @ProfilePhotoUrl	, @AutoInspection	, @RCNotMandatory	, @OriginalImgPath	, @OwnerMobile	, @ShowroomStartTime	, @ShowroomEndTime	, @DealerLastUpdatedBy	, @LegalName	, @PanNumber	, @TanNumber	, @AutoClosed	, @LandlineCode	, @Ids , @UpdateType , @IsDealerDeleted , @DeleteComment , @DeletedOn  , @PackageRenewalDate 
						-- mysql sync end
						--Check whether some earlier package existed  
						IF @IsPreviousPackage = 0  
							BEGIN --There is no existing package  
								--Insert the new package here  
								
								declare @NewDateTime datetime ;
								set @NewDateTime = DATEADD(dd, @ActualValidity, @NewExpiryDate);
								
								INSERT INTO  ConsumerCreditPoints(ConsumerType, ConsumerId, Points, ExpiryDate, PackageType, CustomerPackageId)   ---CustomerPackageId added by Manish on 19 oct 2015
								VALUES(@ConsumerType, @ConsumerId, @ActualInquiryPoints, @NewDateTime, @PackageType, @PackageId);						
								declare @CcpId int;
								set @CcpId=SCOPE_IDENTITY();
								set @InsertId = 1
								-- syncing with mysql
								--Commented By Deepak on 12th Oct 2016 for a release								
									 exec [dbo].[SyncConsumerCreditPointsWithMysql] @CcpId,@ConsumerId,@ConsumerType,@ActualInquiryPoints,@PackageType,@NewDateTime ,@PackageId,@InsertId 
							END  
						ELSE --There was an earlier package  
							BEGIN  
								--Check for the renewal  
								IF @IsRenewal = 1  
									BEGIN 
										--This is the case for the renewal.   
										--In this case the new package could be same as the earlier package  
										--or upgrade/downgrade. Upgrade the packagetype, their points and the expirydate  
										--Total expiry points would be the new one and expiry date would be the new one  
				          
										UPDATE ConsumerCreditPoints   
										SET Points = @ActualInquiryPoints, ExpiryDate = DATEADD(dd,@ActualValidity, @NewExpiryDate), PackageType = @PackageType   
										,CustomerPackageId=@PackageId -----CustomerPackageId added by Manish on 19 oct 2015
										WHERE ConsumerId =  @ConsumerId  AND ConsumerType = @ConsumerType   
										
										--Commented By Deepak on 12th Oct 2016 for a release
										exec SyncConsumerCreditPointsWithMysqlUpdate @ConsumerId,@ConsumerType,@ActualInquiryPoints,@ActualValidity,@NewExpiryDate,null,@PackageType,@PackageId,1,null;		
									END  
								ELSE  
									BEGIN 
										-- This is not the case for renewal  
										--SELECT @PackageType AS Pkg
										--SELECT @PreviousPackageType AS PrevPkg
										 
										--Check the previous package type to determine whether the package is being changed  
										IF @PackageType = @PreviousPackageType  
											BEGIN --There is no change in the package 
												--Now check whether expiry date of the existing package has reached?  
												IF DATEDIFF(dd,GETDATE(),@PreviousExpiryDate) >= 0  
													BEGIN 
														--The package has not expired yet. This will be the case for the topups  
														--Total Inquiry points should be current + new  
														--Total Expiry date would be existing + new  
														--There won't be any stock updation in this case  
														UPDATE ConsumerCreditPoints   
														SET Points = Points + @ActualInquiryPoints, ExpiryDate = DATEADD(dd,@ActualValidity, ExpiryDate)  
														,CustomerPackageId=@PackageId ---CustomerPackageId added by Manish on 19 oct 2015
														,@NewExpiryDate = ExpiryDate
														WHERE ConsumerId = @ConsumerId AND ConsumerType = @ConsumerType AND PackageType = @PackageType 
														
														--Commented By Deepak on 12th Oct 2016 for a release
														exec SyncConsumerCreditPointsWithMysqlUpdate @ConsumerId,@ConsumerType,@ActualInquiryPoints,@ActualValidity,null,null,@PackageType,@PackageId,2,null;
														
													END  
												ELSE  
													BEGIN 
														--The Package has expired  
														--Total expiry points would be the new one and expiry date would be the new one  
														--No stock is to be updated  
														UPDATE ConsumerCreditPoints   
														SET Points = @ActualInquiryPoints, ExpiryDate = DATEADD(dd,@ActualValidity, @NewExpiryDate)  
														,CustomerPackageId=@PackageId ---CustomerPackageId added by Manish on 19 oct 2015
														WHERE ConsumerId = @ConsumerId AND ConsumerType = @ConsumerType AND PackageType = @PackageType;
														
														--Commented By Deepak on 12th Oct 2016 for a release
														exec SyncConsumerCreditPointsWithMysqlUpdate @ConsumerId,@ConsumerType,@ActualInquiryPoints,@ActualValidity,@NewExpiryDate,null,@PackageType,@PackageId,3,null;
													END    
											END  
										ELSE --There is change in package  
											BEGIN  
												SELECT @PackageId AS pkd
												--Update the package type and the package expiry date.  
												UPDATE ConsumerCreditPoints   
												SET Points = @ActualInquiryPoints, ExpiryDate = DATEADD(dd,@ActualValidity, @NewExpiryDate), PackageType = @PackageType  
												,CustomerPackageId=@PackageId ---CustomerPackageId added by Manish on 19 oct 2015
												WHERE ConsumerId = @ConsumerId AND ConsumerType = @ConsumerType;
												
												--Commented By Deepak on 12th Oct 2016 for a release
												exec SyncConsumerCreditPointsWithMysqlUpdate @ConsumerId,@ConsumerType,@ActualInquiryPoints,@ActualValidity,@NewExpiryDate,null,@PackageType,@PackageId,1,null;
			            
											END
									END
							END --There was an earlier package 
						
												
						-- Added By Deepak on 9th Sept 2016
						-- Handle Unverified lead Add-on package
						UPDATE CT_AddOnPackages SET IsActive = 1 WHERE CWDealerId = @ConsumerId AND AddOnPackageId = 101
						IF @@ROWCOUNT = 0
							BEGIN
								DECLARE @curDateVal DATETIME = GETDATE();
								INSERT INTO CT_AddOnPackages(AddOnPackageId, CWDealerId, StartDate, EndDate, IsActive, CreatedOn)
								VALUES(101, @ConsumerId, @curDateVal, @curDateVal, 1, @curDateVal) --Added By Deepak on 9th Sept 2016
								DECLARE @lastRowId INT =SCOPE_IDENTITY();
								
								--Commented By Deepak on 12th Oct 2016 for a release	
								EXEC SyncCTAddOnPackagesWithMysql @lastRowId,@ConsumerId,101,@curDateVal,@curDateVal,@curDateVal,1;
							END
						ELSE
							--Commented By Deepak on 12th Oct 2016 for a release	
							EXEC SyncCTAddOnPackagesWithMysqlUpdate @ConsumerId,101,null,null,null,1,null,2
						
							
						-- Handle Showroom Add-on Package
						DECLARE @curDate DATETIME;
						DECLARE @endDate DATETIME;
						SET @endDate = DATEADD(dd, @ActualValidity, @NewExpiryDate);
						SET @curDate = GETDATE();
						IF @HasShowroom = 1
							BEGIN
								IF ISNULL(@AddonPackageTblId,0) > 0
									BEGIN									
										UPDATE CT_AddOnPackages 
										SET StartDate = @curDate, EndDate = @endDate,UpdatedOn = @curDate, IsActive = 1 
										WHERE Id = @AddonPackageTblId;
										
										--Commented By Deepak on 12th Oct 2016 for a release	
										EXEC SyncCTAddOnPackagesWithMysqlUpdate null,null,@curDate,@endDate,@curDate,1,@AddonPackageTblId,3;
									END
								ELSE
									BEGIN
										INSERT INTO CT_AddOnPackages(AddOnPackageId, CWDealerId, StartDate, EndDate, IsActive, CreatedOn)
										VALUES(100, @ConsumerId, @curDate, @endDate, 1, @curDate)
										SET @lastRowId = SCOPE_IDENTITY();
										
										--Commented By Deepak on 12th Oct 2016 for a release	
										EXEC SyncCTAddOnPackagesWithMysql @lastRowId,@ConsumerId,100,@curDate,@endDate,@curDate,1;
									END
							END -- Has Showroom
						ELSE
							BEGIN
								DECLARE @yesterday DATETIME
								SET @yesterday = @curDate-1;
								UPDATE CT_AddOnPackages SET EndDate = @yesterday, UpdatedOn = @curDate, IsActive = 0 
								WHERE Id = @AddonPackageTblId
								
								--Commented By Deepak on 12th Oct 2016 for a release	
								EXEC SyncCTAddOnPackagesWithMysqlUpdate null,null,null,@yesterday,@curDate,0,@AddonPackageTblId,4;
							END
					END --Dealer Package related Task
			END-- For Dealer  
		ELSE -- FOR Individual  
			BEGIN  
				--Check whether some earlier entry exists for the individual for the same package type  
				Select @IsSamePreviousPackage = 1   
				From ConsumerCreditPoints  WITH (NOLOCK) 
				WHERE ConsumerId =  @ConsumerId  AND ConsumerType = @ConsumerType AND PackageType = @PackageType  
	       
				IF @IsSamePreviousPackage = 1  
					BEGIN --An earlier entry existed for the same package type  
						--Update the existing entry  
						UPDATE ConsumerCreditPoints   
						SET Points = Points + @ActualInquiryPoints, ExpiryDate = DATEADD(dd,@ActualValidity, ExpiryDate) 
						,CustomerPackageId=@PackageId ---CustomerPackageId added by Manish on 19 oct 2015 
						WHERE ConsumerId =  @ConsumerId  AND ConsumerType = @ConsumerType AND PackageType = @PackageType;
						
						--Commented By Deepak on 12th Oct 2016 for a release
						exec SyncConsumerCreditPointsWithMysqlUpdate @ConsumerId,@ConsumerType,@ActualInquiryPoints,@ActualValidity,null,null,@PackageType,@PackageId,2,null;
					END  
				ELSE  
					BEGIN -- No entry existed for the same package type  
						--Insert a new record for the package  
						set @NewDateTime = DATEADD(dd, @ActualValidity, @NewExpiryDate);
						INSERT INTO  ConsumerCreditPoints(ConsumerType, ConsumerId, Points, ExpiryDate, PackageType,CustomerPackageId)  -----CustomerPackageId added by Manish on 19 oct 2015
						VALUES(@ConsumerType, @ConsumerId, @ActualInquiryPoints, @NewDateTime, @PackageType,@PackageId)  
										
						set @CcpId=SCOPE_IDENTITY();
						
						-- syncing with mysql
						--Commented By Deepak on 12th Oct 2016 for a release
						exec [dbo].[SyncConsumerCreditPointsWithMysql] @CcpId, @ConsumerId, @ConsumerType, @ActualInquiryPoints, @PackageType,@NewDateTime ,@PackageId,1;	
	
					END  
			END --For Individual  
			
		--Now approve the package request  
		Update ConsumerPackageRequests Set IsApproved = 1,isActive = 1, ApprovedBy = @EnteredById, ApprovalDate = @NewExpiryDate  Where ID = @ConsumerPkgReqId
		
		--Commented By Deepak on 12th Oct 2016 for a release
		exec SyncConsumerPackageRequestsWithMysqlUpdate @EnteredById,@NewExpiryDate,@ConsumerPkgReqId,null,null,null,null,null,null,null,null,null,null,1
		END --end of begin for if dealer is not migrated to cartrade
		end try
	 BEGIN CATCH
		INSERT INTO CarWaleMyslSyncExceptions(ModuleName,SPName,ErrorMsg,TableName,FailedId,CreatedOn,InputParameter)
		VALUES('MysqlSync','UpdateConsumerCreditPoints_OPR',ERROR_MESSAGE(),'ConsumerCreditPoints',null,GETDATE(),@updateType)
	END CATCH	
	END -- Main End block 
	
END  

