IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_DealersWebsiteFlagUpdate]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_DealersWebsiteFlagUpdate]
GO

	-- =============================================  
-- Author:  Manish  
-- Create date: 06-March-2013 
-- Details: SP will insert the logs of Access of Dealer Website through CRM and update the having website field on Dealers table
-- Modified By Manish on 23-04-2013 inserting roles for the dealer's users 
-- =============================================  
CREATE PROCEDURE  [dbo].[TC_DealersWebsiteFlagUpdate]  
  @BranchId  AS INT,
  @HavingWebsite AS BIT,
  @OprUserId AS INT
AS
  BEGIN
		--mysql sync
				declare 
		@ID	decimal(18,0)=@BranchId, @LoginId	varchar(30)=null, @Passwd	varchar(50)=null, @FirstName	varchar(100)=null, @LastName	varchar(100)=null, @EmailId	varchar(250)=null, @Organization	varchar(100)=null,@Address1	varchar(500)=null, @Address2	varchar(500)=null, @AreaId	decimal(18,0)=null, @CityId	decimal(18,0)=null, @StateId	decimal(18,0)=null, @Pincode	varchar(6)=null, @PhoneNo	varchar(50)=null,@FaxNo	varchar(50)=null,  @MobileNo	varchar(50)=null, @ExpiryDate	datetime=null, @WebsiteUrl	varchar(100)=null, @ContactPerson	varchar(200)=null, @ContactHours	varchar(30)=null, @ContactEmail	varchar(250)=null,@Status	tinyint=null, @LastUpdatedOn	datetime=null, @CertificationId	smallint=null, @BPMobileNo	varchar(15)=null, @BPContactPerson	varchar(60)=null, @IsTCDealer	tinyint=null, @IsWKitSent	tinyint=null,@IsTCTrainingGiven	tinyint=null, @HostURL	varchar(100)=null, @TC_DealerTypeId tinyint=null,  @Longitude	float=null,  @Lattitude	float=null, @DeletedReason	smallint=null, @ActiveMaskingNumber	varchar(20)=null, @DealerVerificationStatus	smallint=null, @IsPremium	tinyint=null, @WebsiteContactMobile	varchar(50)=null, @WebsiteContactPerson	varchar(100)=null, @ApplicationId	tinyint=null, @IsWarranty	tinyint=null, @LeadServingDistance	smallint=null, @OutletCnt	int=null, @ProfilePhotoHostUrl	varchar(100)=null, @ProfilePhotoUrl	varchar(250)=null, @AutoInspection	tinyint=null, @RCNotMandatory	tinyint=null,@OriginalImgPath	varchar(250)=null,  @OwnerMobile	varchar(20)=null, @ShowroomStartTime	varchar(30)=null, @ShowroomEndTime	varchar(30)=null, @DealerLastUpdatedBy	int=null,@LegalName	varchar(50)=null,@PanNumber	varchar(50)=null, @TanNumber	varchar(50)=null, @AutoClosed	tinyint=null, @LandlineCode	varchar(4)=null,  @Ids Varchar(MAX)=null, @UpdateType int =null 
		--mysql sync end
		IF @HavingWebsite=1 AND NOT EXISTS (SELECT 1 FROM TC_DealerWebsiteLog 
		                                    WHERE BranchID=@BranchId AND IsActive=1)
		BEGIN
		 
			 UPDATE Dealers SET HavingWebsite=1
			     		   WHERE ID=@BranchId;
		--mysql sync
		SET @HavingWebsite=1
		set @UpdateType=5
		begin try
		exec SyncDealersWithMysqlUpdate @ID	, @LoginId	, @Passwd	, @FirstName	, @LastName	, @EmailId	, @Organization	,@Address1	, @Address2	, @AreaId	, @CityId	, @StateId	, @Pincode	, @PhoneNo	,@FaxNo	,  @MobileNo	, @ExpiryDate, @WebsiteUrl	, @ContactPerson	, @ContactHours	, @ContactEmail	,@Status	, @LastUpdatedOn, @CertificationId	, @BPMobileNo	, @BPContactPerson	, @IsTCDealer	, @IsWKitSent	,@IsTCTrainingGiven	, @HostURL	, @TC_DealerTypeId ,  @Longitude,  @Lattitude, @DeletedReason	, @HavingWebsite	,@ActiveMaskingNumber	, @DealerVerificationStatus	, @IsPremium	, @WebsiteContactMobile	, @WebsiteContactPerson	, @ApplicationId	, @IsWarranty	, @LeadServingDistance	, @OutletCnt	, @ProfilePhotoHostUrl	, @ProfilePhotoUrl	, @AutoInspection	, @RCNotMandatory	,@OriginalImgPath	,  @OwnerMobile	, @ShowroomStartTime	, @ShowroomEndTime	, @DealerLastUpdatedBy	,@LegalName	,@PanNumber	, @TanNumber	, @AutoClosed	, @LandlineCode	,  @Ids , @UpdateType 
			end try
			BEGIN CATCH
				INSERT INTO CarWaleMyslSyncExceptions(ModuleName,SPName,ErrorMsg,TableName,FailedId,CreatedOn,InputParameter)
				VALUES('MysqlSync','TC_DealersWebsiteFlagUpdate',ERROR_MESSAGE(),'SyncDealersWithMysqlUpdate',@Id,GETDATE(),@UpdateType)
			END CATCH	
		--mysql sync end
			 
			 INSERT INTO TC_DealerWebsiteLog VALUES(@BranchId,GETDATE(),NULL,@OprUserId,1);  
			 
			 
			--------------------Modified by Manish on 23-04-2013 inserting roles for the user having dealer principle role-- 
			 INSERT INTO TC_UsersRole (UserId,RoleId) 
			                     Select DISTINCT Userid,3 
			                     FROM TC_UsersRole TCUR 
			                     JOIN TC_Users AS TCU ON TCU.Id=TCUR.UserId
			                     WHERE TCU.BranchId=@BranchId 
			                     AND TCUR.RoleId=1
           -------------------------------------------------------------------------------------------------------------
		 END 
		
		ELSE IF @HavingWebsite=0
		  BEGIN
				 UPDATE Dealers SET HavingWebsite=0
						   WHERE ID=@BranchId;
	--mysql sync
		SET @HavingWebsite=0
		set @UpdateType=5
		begin try
		exec SyncDealersWithMysqlUpdate @BranchId	, @LoginId	, @Passwd	, @FirstName	, @LastName	, @EmailId	, @Organization	,@Address1	, @Address2	, @AreaId	, @CityId	, @StateId	, @Pincode	, @PhoneNo	,@FaxNo	,  @MobileNo	, @ExpiryDate, @WebsiteUrl	, @ContactPerson	, @ContactHours	, @ContactEmail	,@Status	, @LastUpdatedOn, @CertificationId	, @BPMobileNo	, @BPContactPerson	, @IsTCDealer	, @IsWKitSent	,@IsTCTrainingGiven	, @HostURL	, @TC_DealerTypeId ,  @Longitude,  @Lattitude, @DeletedReason	, @HavingWebsite	,@ActiveMaskingNumber	, @DealerVerificationStatus	, @IsPremium	, @WebsiteContactMobile	, @WebsiteContactPerson	, @ApplicationId	, @IsWarranty	, @LeadServingDistance	, @OutletCnt	, @ProfilePhotoHostUrl	, @ProfilePhotoUrl	, @AutoInspection	, @RCNotMandatory	,@OriginalImgPath	,  @OwnerMobile	, @ShowroomStartTime	, @ShowroomEndTime	, @DealerLastUpdatedBy	,@LegalName	,@PanNumber	, @TanNumber	, @AutoClosed	, @LandlineCode	,  @Ids , @UpdateType 
			end try
			BEGIN CATCH
				INSERT INTO CarWaleMyslSyncExceptions(ModuleName,SPName,ErrorMsg,TableName,FailedId,CreatedOn,InputParameter)
				VALUES('MysqlSync','TC_DealersWebsiteFlagUpdate',ERROR_MESSAGE(),'SyncDealersWithMysqlUpdate',@BranchId,GETDATE(),@UpdateType)
			END CATCH	
		--mysql sync end
				 UPDATE TC_DealerWebsiteLog SET EndDate=GETDATE(),
												OprUserId=@OprUserId,
												IsActive=0
							               WHERE BranchID=@BranchId
							               
            --------------------Modified by Manish on 23-04-2013 deleting  roles for the user having website manager role--
                    DELETE    TCUR 
			          FROM    TC_UsersRole AS TCUR
			          JOIN    TC_Users AS TCU ON TCU.Id=TCUR.UserId
			                     WHERE TCU.BranchId=@BranchId 
			                     AND TCUR.RoleId=3
	       -------------------------------------------------------------------------------------------------------------
           END 
        END

