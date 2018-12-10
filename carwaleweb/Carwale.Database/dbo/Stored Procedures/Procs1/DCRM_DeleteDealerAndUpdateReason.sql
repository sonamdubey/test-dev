IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[DCRM_DeleteDealerAndUpdateReason]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[DCRM_DeleteDealerAndUpdateReason]
GO

	-- =============================================
-- Author:		Vinay Kumar
-- Create date: 18 SEPT 2013
-- Description:	This Proc. is used to insert or delete dealer id in DeleteDealers  And Update dealers 
--            :@Flag =0 means User do not want to delete dealer and delete record from DeleteDealers
--            :@Flag =1 means User  want to delete dealer and insert record into DeleteDealers
--            :@Flag =2 means User  want to change Reason only
--            : This Proc. Used in EditDealerContactDetails.cs
-- =============================================
CREATE PROCEDURE [dbo].[DCRM_DeleteDealerAndUpdateReason]  
(
@DealerId		NUMERIC, 
@Flag  INT,
@DeletedBy  NUMERIC     =  NULL,
@DeletedOn  DATETIME    =  NULL,
@DeletedReason  SMALLINT =  NULL,
@DealerVerificationStatus SMALLINT =  NULL,---1- verified 2-notverified 3-deleted
@IsDealerDeleted  BIT   = NULL 
)  
AS
BEGIN 
	--mysql sync
				declare 
		@ID	decimal(18,0)=null, @LoginId	varchar(30)=null, @Passwd	varchar(50)=null, @FirstName	varchar(100)=null, @LastName	varchar(100)=null, @EmailId	varchar(250)=null, @Organization	varchar(100)=null,@Address1	varchar(500)=null, @Address2	varchar(500)=null, @AreaId	decimal(18,0)=null, @CityId	decimal(18,0)=null, @StateId	decimal(18,0)=null, @Pincode	varchar(6)=null, @PhoneNo	varchar(50)=null,@FaxNo	varchar(50)=null,  @MobileNo	varchar(50)=null, @ExpiryDate	datetime=null, @WebsiteUrl	varchar(100)=null, @ContactPerson	varchar(200)=null, @ContactHours	varchar(30)=null, @ContactEmail	varchar(250)=null,@Status	tinyint=null, @LastUpdatedOn	datetime=null, @CertificationId	smallint=null, @BPMobileNo	varchar(15)=null, @BPContactPerson	varchar(60)=null, @IsTCDealer	tinyint=null, @IsWKitSent	tinyint=null,@IsTCTrainingGiven	tinyint=null, @HostURL	varchar(100)=null, @TC_DealerTypeId tinyint=null,  @Longitude	float=null,  @Lattitude	float=null, @HavingWebsite	tinyint=null,@ActiveMaskingNumber	varchar(20)=null, @IsPremium	tinyint=null, @WebsiteContactMobile	varchar(50)=null, @WebsiteContactPerson	varchar(100)=null, @ApplicationId	tinyint=null, @IsWarranty	tinyint=null, @LeadServingDistance	smallint=null, @OutletCnt	int=null, @ProfilePhotoHostUrl	varchar(100)=null, @ProfilePhotoUrl	varchar(250)=null, @AutoInspection	tinyint=null, @RCNotMandatory	tinyint=null,@OriginalImgPath	varchar(250)=null,  @OwnerMobile	varchar(20)=null, @ShowroomStartTime	varchar(30)=null, @ShowroomEndTime	varchar(30)=null, @DealerLastUpdatedBy	int=null,@LegalName	varchar(50)=null,@PanNumber	varchar(50)=null, @TanNumber	varchar(50)=null, @AutoClosed	tinyint=null, @LandlineCode	varchar(4)=null,  @Ids Varchar(MAX)=null, @UpdateType int =null 
		begin try
	  IF @Flag=0
		  BEGIN
			DELETE FROM DeletedDealers WHERE DealerId =@DealerId
			
			UPDATE Dealers 
				 SET  
				 DeletedBy	    =	@DeletedBy,       
				 DeletedOn	    =	@DeletedOn,      
				 IsDealerDeleted =	0,
				 DeletedReason	=	@DeletedReason,
			     DealerVerificationStatus =@DealerVerificationStatus  
			 WHERE Id=@DealerId  
			
			set @DealerLastUpdatedBy=@DeletedBy
			set @LastUpdatedOn=@DeletedOn
			set @ID=@DealerId
			set @UpdateType=19
			exec SyncDealersWithMysqlUpdate @ID	, @LoginId	, @Passwd	, @FirstName	, @LastName	, @EmailId	, @Organization	,@Address1	, @Address2	, @AreaId	, @CityId	, @StateId	, @Pincode	, @PhoneNo	,@FaxNo	,  @MobileNo	, @ExpiryDate, @WebsiteUrl	, @ContactPerson	, @ContactHours	, @ContactEmail	,@Status	, @LastUpdatedOn, @CertificationId	, @BPMobileNo	, @BPContactPerson	, @IsTCDealer	, @IsWKitSent	,@IsTCTrainingGiven	, @HostURL	, @TC_DealerTypeId ,  @Longitude,  @Lattitude, @DeletedReason	, @HavingWebsite	,@ActiveMaskingNumber	, @DealerVerificationStatus	, @IsPremium	, @WebsiteContactMobile	, @WebsiteContactPerson	, @ApplicationId	, @IsWarranty	, @LeadServingDistance	, @OutletCnt	, @ProfilePhotoHostUrl	, @ProfilePhotoUrl	, @AutoInspection	, @RCNotMandatory	,@OriginalImgPath	,  @OwnerMobile	, @ShowroomStartTime	, @ShowroomEndTime	, @DealerLastUpdatedBy	,@LegalName	,@PanNumber	, @TanNumber	, @AutoClosed	, @LandlineCode	,  @Ids , @UpdateType,0
		--mysql sync end
		  END
		  
	   IF @Flag = 1
		BEGIN 
		    INSERT INTO DeletedDealers VALUES(@DealerId)
		    UPDATE Dealers 
				 SET  
				 DeletedBy	    =	@DeletedBy,       
				 DeletedOn	    =	@DeletedOn,      
				 DeletedReason	=	@DeletedReason,
				 IsDealerDeleted =	@IsDealerDeleted,
			     Status          =  1 , -- Dealer suspended
			     DealerVerificationStatus =@DealerVerificationStatus  -- status Deleted(3)
			 WHERE Id=@DealerId  
			set @DealerLastUpdatedBy=@DeletedBy
			set @LastUpdatedOn=@DeletedOn
			set @ID=@DealerId
			set @Status=1
			set @UpdateType=18
			exec SyncDealersWithMysqlUpdate @ID	, @LoginId	, @Passwd	, @FirstName	, @LastName	, @EmailId	, @Organization	,@Address1	, @Address2	, @AreaId	, @CityId	, @StateId	, @Pincode	, @PhoneNo	,@FaxNo	,  @MobileNo	, @ExpiryDate, @WebsiteUrl	, @ContactPerson	, @ContactHours	, @ContactEmail	,@Status	, @LastUpdatedOn, @CertificationId	, @BPMobileNo	, @BPContactPerson	, @IsTCDealer	, @IsWKitSent	,@IsTCTrainingGiven	, @HostURL	, @TC_DealerTypeId ,  @Longitude,  @Lattitude, @DeletedReason	, @HavingWebsite	,@ActiveMaskingNumber	, @DealerVerificationStatus	, @IsPremium	, @WebsiteContactMobile	, @WebsiteContactPerson	, @ApplicationId	, @IsWarranty	, @LeadServingDistance	, @OutletCnt	, @ProfilePhotoHostUrl	, @ProfilePhotoUrl	, @AutoInspection	, @RCNotMandatory	,@OriginalImgPath	,  @OwnerMobile	, @ShowroomStartTime	, @ShowroomEndTime	, @DealerLastUpdatedBy	,@LegalName	,@PanNumber	, @TanNumber	, @AutoClosed	, @LandlineCode	,  @Ids , @UpdateType,@IsDealerDeleted
		--mysql sync end
		END
		  
      IF @Flag=2 
		 BEGIN
             UPDATE Dealers 
				 SET  				         
				 DeletedReason	=	@DeletedReason,
				 DealerVerificationStatus =@DealerVerificationStatus
			 WHERE Id=@DealerId  
			 
			set @ID=@DealerId
			set @UpdateType=14
			exec SyncDealersWithMysqlUpdate @ID	, @LoginId	, @Passwd	, @FirstName	, @LastName	, @EmailId	, @Organization	,@Address1	, @Address2	, @AreaId	, @CityId	, @StateId	, @Pincode	, @PhoneNo	,@FaxNo	,  @MobileNo	, @ExpiryDate, @WebsiteUrl	, @ContactPerson	, @ContactHours	, @ContactEmail	,@Status	, @LastUpdatedOn, @CertificationId	, @BPMobileNo	, @BPContactPerson	, @IsTCDealer	, @IsWKitSent	,@IsTCTrainingGiven	, @HostURL	, @TC_DealerTypeId ,  @Longitude,  @Lattitude, @DeletedReason	, @HavingWebsite	,@ActiveMaskingNumber	, @DealerVerificationStatus	, @IsPremium	, @WebsiteContactMobile	, @WebsiteContactPerson	, @ApplicationId	, @IsWarranty	, @LeadServingDistance	, @OutletCnt	, @ProfilePhotoHostUrl	, @ProfilePhotoUrl	, @AutoInspection	, @RCNotMandatory	,@OriginalImgPath	,  @OwnerMobile	, @ShowroomStartTime	, @ShowroomEndTime	, @DealerLastUpdatedBy	,@LegalName	,@PanNumber	, @TanNumber	, @AutoClosed	, @LandlineCode	,  @Ids , @UpdateType
		 END  
		 	 
	 	end try
	BEGIN CATCH
		INSERT INTO CarWaleMyslSyncExceptions(ModuleName,SPName,ErrorMsg,TableName,FailedId,CreatedOn,InputParameter)
		VALUES('MysqlSync','DCRM_DeleteDealerAndUpdateReason',ERROR_MESSAGE(),'SyncDealersWithMysqlUpdate',@DealerId,GETDATE(),@UpdateType)
	END CATCH			
 
END 

