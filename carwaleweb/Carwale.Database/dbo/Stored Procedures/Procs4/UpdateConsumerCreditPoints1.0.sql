IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[UpdateConsumerCreditPoints1]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[UpdateConsumerCreditPoints1]
GO

	-- Modified by Manish on 12 Jun 2015 enable and disable trigger on livelistings table
-- Modified by Manish on 12 Jun 2015 enable and disable trigger on livelistings table commented
CREATE PROCEDURE [dbo].[UpdateConsumerCreditPoints1.0]  

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
   @HasShowroom BIT
  
  SET @CurrPoints = 0  
  SET @IsPreviousPackage = 0  
  SET @IsRenewal = @IsPkgRenewal  
  SET @IsSamePreviousPackage = 0  
   
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
	FROM InquiryPointCategory
	WHERE Id=@PackageType
    -----------------------------------------------------------------------------------------------------
	print @ConsumerType  
	
	--Insert into the log file  
	INSERT INTO ConsumerCreditPointsLogs( ConsumerId, PackageId, ConsumerPkgReqId, SuggestedInquiry, ActualInquiry,  
		 SuggestedPrice,  ActualPrice, EntryDate, EnteredBy,ConsumerType, EnteredById)  
	VALUES(@ConsumerId,@PackageId, @ConsumerPkgReqId, @SuggestedInqPts, @ActualInquiryPoints, @SuggestedAmount,   
		@ActualAmount, GetDate(), @EnteredBy, @ConsumerType, @EnteredById)  
  
	--Check for the consumer type  
	IF  @ConsumerType = 1 --DEALER
		BEGIN
		
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
					FROM  ConsumerCreditPoints   
					WHERE   
						ConsumerId =  @ConsumerId  AND   
						ConsumerType = @ConsumerType   
---------------------------Modified by Reshma Shetty 22/11/2013 Update the IsPremium flag if the Current Package's IsPremium does not match
					  UPDATE Dealers
					  SET IsPremium=@IsPremium
					  WHERE Id= @ConsumerId and IsPremium<>@IsPremium
---------------------------------------------------------------------------------------------------------------------------------------
					--Check whether some earlier package existed  
					IF @IsPreviousPackage = 0  
						BEGIN --There is no existing package  
							--Insert the new package here  
							INSERT   
							INTO  ConsumerCreditPoints(ConsumerType, ConsumerId, Points, ExpiryDate, PackageType)  
							VALUES(@ConsumerType, @ConsumerId, @ActualInquiryPoints, DATEADD(dd, @ActualValidity, @NewExpiryDate), @PackageType)  
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
									WHERE ConsumerId =  @ConsumerId  AND ConsumerType = @ConsumerType   
			           
									--All the suspended cars which were expired last 3 days would be renewed upto the  
									--maximum number of avilable points. Also update their package type  
/*update Is premium and dealers*/	Update SellInquiries 
									Set PackageType = @PackageType, PackageExpiryDate = DATEADD(dd,@ActualValidity, @NewExpiryDate) 
									,IsPremium=@IsPremium -----------Modified by Reshma Shetty 22/11/2013 Set IsPremium flag
									Where ID IN (Select Top (CONVERT(INT, @ActualInquiryPoints)) ID From SellInquiries Where DealerId = @ConsumerId AND StatusId = 1   
									AND DATEDIFF(dd,GETDATE(),PackageExpiryDate) >= -5 Order By LastUpdated DESC)  
			          
									--Remove showroom if exists  
									---Modified by Reshma Shetty 22/11/2013  @HasShowroom instead of hardcoding PackageIds to determine whether the showroom has to be set or not
											--IF(@PackageId != 32 AND  @PackageId != 33)
											--BEGIN  
											--   UPDATE ActiveDealers SET HasShowroom = 0 WHERE DealerId = @ConsumerId  
											--END 
									UPDATE ActiveDealers   
									SET HasShowroom = @HasShowroom 
									WHERE DealerId = @ConsumerId    


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
													WHERE ConsumerId = @ConsumerId AND ConsumerType = @ConsumerType AND PackageType = @PackageType 
													
													-- Update Stcok expiry dates as well
													Update SellInquiries Set PackageExpiryDate = DATEADD(dd,@ActualValidity, PackageExpiryDate) 
													,IsPremium=@IsPremium -----------Modified by Reshma Shetty 22/11/2013 Set IsPremium flag
													Where ID In (Select LL.InquiryId From LiveListings AS LL, SellInquiries AS S Where   
													S.DealerId = @ConsumerId AND LL.Inquiryid = S.ID AND LL.SellerType = 1) 
													
												END  

											ELSE  
												BEGIN 
													--The Package has expired  
													--Total expiry points would be the new one and expiry date would be the new one  
													--No stock is to be updated  
													UPDATE ConsumerCreditPoints   
													SET Points = @ActualInquiryPoints, ExpiryDate = DATEADD(dd,@ActualValidity, @NewExpiryDate)  
													WHERE ConsumerId = @ConsumerId AND ConsumerType = @ConsumerType AND PackageType = @PackageType 
													
													-- Update Stcok expiry dates as well
/*update for is premium or dealers not necessary as the package type remains same*/
													Update SellInquiries Set PackageExpiryDate = DATEADD(dd,@ActualValidity, @NewExpiryDate)  
													Where ID In (Select LL.InquiryId From LiveListings AS LL, SellInquiries AS S Where   
													S.DealerId = @ConsumerId AND LL.Inquiryid = S.ID AND LL.SellerType = 1)   
												END    
										END  
									ELSE --There is change in package  
										BEGIN  
											
											SELECT @PackageId AS pkd
											--Update the package type and the package expiry date.  
											UPDATE ConsumerCreditPoints   
											SET Points = @ActualInquiryPoints, ExpiryDate = DATEADD(dd,@ActualValidity, @NewExpiryDate), PackageType = @PackageType  
											WHERE ConsumerId = @ConsumerId AND ConsumerType = @ConsumerType  
			            
											--Check for the number of live listings for the dealer. If it is less than or equal to the number of   
											--avilable points, then update the live listings with the new package type and package expiry date  
											--If live listings is more than the available points then update only the cars maximum upto   
											--number of available points    
											Select @LiveStockCount = COUNT(LL.ProfileId)   
											From LiveListings AS LL, SellInquiries AS S   
											Where S.DealerId = @ConsumerId AND LL.SellerType = 1 AND LL.Inquiryid = S.ID 

											---- Modified by Avishkar (15-11-2013)  to set IsPremium column in SellInquiries
											--UPDATE SI
											--SET IsPremium= (CASE D.IsPremium --Modified by Avishkar (15-11-2013) Added @IsPremium to set IsPremium column in LiveListing
											--			  WHEN 1 THEN 1
											--			  ELSE 0
											--			END
											--			)
											--FROM SellInquiries as SI
											--  JOIN Dealers as D on D.ID=SI.DealerId
											
											SELECT @PackageId AS pkg
											
											--Remove showroom if exists  
											---Modified by Reshma Shetty 22/11/2013  @HasShowroom instead of hardcoding PackageIds to determine whether the showroom has to be set or not
											     --IF(@PackageId != 32 AND  @PackageId != 33)
												 --BEGIN  
													--   UPDATE ActiveDealers SET HasShowroom = 0 WHERE DealerId = @ConsumerId  
												 --END 
											UPDATE ActiveDealers   
											SET HasShowroom = @HasShowroom 
											WHERE DealerId = @ConsumerId    

											IF @LiveStockCount <= @ActualInquiryPoints  
												BEGIN --Total live listing is less than or equal to the points available  
													--In this case we can update all the live listings with the new package type and the expirydate  
	/*update Is premium and dealers*/				Update SellInquiries Set PackageType = @PackageType, PackageExpiryDate = DATEADD(dd,@ActualValidity, @NewExpiryDate)
													,IsPremium=@IsPremium -----------Modified by Reshma Shetty 22/11/2013 Set IsPremium flag  
													Where ID In (Select LL.InquiryId From LiveListings AS LL, SellInquiries AS S Where   
													S.DealerId = @ConsumerId AND LL.Inquiryid = S.ID AND LL.SellerType = 1)   
												END  
											ELSE  
												BEGIN --Total live listings is greater than the available points.  
													--In this case remove the earliest listing(based on lastupdated) which is equal to  
													--TotalLiveStock - AvailablePoints.               
	/*do not update Is premium and dealers*/		Update SellInquiries Set PackageExpiryDate = DATEADD(dd,-1, GETDATE())  
													Where ID In (Select TOP (Convert(INT, @LiveStockCount - @ActualInquiryPoints)) LL.InquiryId From LiveListings AS LL, SellInquiries AS S Where   
													S.DealerId = @ConsumerId AND LL.Inquiryid = S.ID AND LL.SellerType = 1 ORDER By LL.LastUpdated ASC)   
			                  
													--Update the remaining live listing which is equal to   
													--the availablepoints with the new packagetype and the expirydates  
	/*update Is premium and dealers*/				Update SellInquiries Set PackageType = @PackageType, PackageExpiryDate = DATEADD(dd,@ActualValidity, @NewExpiryDate)
													,IsPremium=@IsPremium -----------Modified by Reshma Shetty 22/11/2013 Set IsPremium flag  
													Where ID In (Select LL.InquiryId From LiveListings AS LL, SellInquiries AS S Where   
													S.DealerId = @ConsumerId AND LL.Inquiryid = S.ID AND LL.SellerType = 1)   
												END
										END
								END
						END
				END
			END-- For Dealer  
		ELSE -- FOR Individual  
			BEGIN  
				--Check whether some earlier entry exists for the individual for the same package type  
				Select @IsSamePreviousPackage = 1   
				From ConsumerCreditPoints   
				WHERE ConsumerId =  @ConsumerId  AND ConsumerType = @ConsumerType AND PackageType = @PackageType  
	       
				IF @IsSamePreviousPackage = 1  
					BEGIN --An earlier entry existed for the same package type  
						--Update the existing entry  
						UPDATE ConsumerCreditPoints   
						SET Points = Points + @ActualInquiryPoints, ExpiryDate = DATEADD(dd,@ActualValidity, ExpiryDate)  
						WHERE ConsumerId =  @ConsumerId  AND ConsumerType = @ConsumerType AND PackageType = @PackageType   
					END  
				ELSE  
					BEGIN -- No entry existed for the same package type  
						--Insert a new record for the package  
						INSERT   
						INTO  ConsumerCreditPoints(ConsumerType, ConsumerId, Points, ExpiryDate, PackageType)  
						VALUES(@ConsumerType, @ConsumerId, @ActualInquiryPoints, DATEADD(dd, @ActualValidity, @NewExpiryDate), @PackageType)  
					END  
			END --For Individual  
			
		--Now approve the package request  
		Update ConsumerPackageRequests Set IsApproved = 1 Where ID = @ConsumerPkgReqId  
   
	END -- Main End block  


-- added by Manish on 12 Jun 2015 enable and disable trigger on livelistings table-------
	--ALTER TABLE dbo.LiveListings ENABLE TRIGGER TrigUpdateLiveListingCities
     
END  