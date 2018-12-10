IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[SyncDealersWithMysqlUpdate]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[SyncDealersWithMysqlUpdate]
GO

	-- =============================================
-- Author:		<Prasad Gawde>
-- Create date: <Create Date,20/10/2016>
-- Description:	<Description, syncing dealers with mysql>
-- =============================================
CREATE PROCEDURE  [dbo].[SyncDealersWithMysqlUpdate]
@ID	decimal(18,0),
@LoginId	varchar(30),
@Passwd	varchar(50),
@FirstName	varchar(100),
@LastName	varchar(100),
@EmailId	varchar(250),
@Organization	varchar(100),
@Address1	varchar(500),
@Address2	varchar(500),
@AreaId	decimal(18,0),
@CityId	decimal(18,0),
@StateId	decimal(18,0),
@Pincode	varchar(6),
@PhoneNo	varchar(50),
@FaxNo	varchar(50),
@MobileNo	varchar(50),
@ExpiryDate	datetime,
@WebsiteUrl	varchar(100),
@ContactPerson	varchar(200),
@ContactHours	varchar(30),
@ContactEmail	varchar(250),
@Status	tinyint,
@LastUpdatedOn	datetime,
@CertificationId	smallint,
@BPMobileNo	varchar(15),
@BPContactPerson	varchar(60),
@IsTCDealer	tinyint,
@IsWKitSent	tinyint,
@IsTCTrainingGiven	tinyint,
@HostURL	varchar(100),
@TC_DealerTypeId tinyint, 
@Longitude	float,
@Lattitude	float,
@DeletedReason	smallint,
@HavingWebsite	tinyint,
@ActiveMaskingNumber	varchar(20),
@DealerVerificationStatus	smallint,
@IsPremium	tinyint,
@WebsiteContactMobile	varchar(50),
@WebsiteContactPerson	varchar(100),
@ApplicationId	tinyint, 
@IsWarranty	tinyint,
@LeadServingDistance	smallint,
@OutletCnt	int,
@ProfilePhotoHostUrl	varchar(100),
@ProfilePhotoUrl	varchar(250),
@AutoInspection	tinyint,
@RCNotMandatory	tinyint,
@OriginalImgPath	varchar(250),
@OwnerMobile	varchar(20),
@ShowroomStartTime	varchar(30),
@ShowroomEndTime	varchar(30),
@DealerLastUpdatedBy	int,
@LegalName	varchar(50),
@PanNumber	varchar(50),
@TanNumber	varchar(50),
@AutoClosed	tinyint,
@LandlineCode	varchar(4), 
@Ids Varchar(MAX),	
@UpdateType int,
@IsDealerDeleted bit = null,
@DeleteComment varchar(500) = null,
@DeletedOn datetime =null,
@PackageRenewalDate datetime = null,
@DealerSource int = null,
@LogoUrl varchar(100) = null
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
begin try	
		if @UpdateType=1 
			UPDATE mysql_test...dealers SET ProfilePhotoHostUrl = @HostUrl,
								   ProfilePhotoStatusId = 3, 
								   ProfilePhotoUrl = '/0X0/' + @ProfilePhotoUrl + OriginalImgPath,
								   OriginalImgPath = @ProfilePhotoUrl + OriginalImgPath 
								   WHERE ID = @ID
		else if @UpdateType=2						   
		UPDATE mysql_test...dealers SET ProfilePhotoHostUrl = @HostUrl,ProfilePhotoStatusId = 3 WHERE ID = @ID
		else if @UpdateType=3
		UPDATE mysql_test...dealers SET ProfilePhotoHostUrl = @HostUrl,
								   ProfilePhotoStatusId = 3, 
								   ProfilePhotoUrl = '/0X0/' + OriginalImgPath,
								   OriginalImgPath = OriginalImgPath 
								   WHERE ID = @ID;
		else if @UpdateType=4
		UPDATE mysql_test...dealers SET IsWarranty = @IsWarranty WHERE ID = @ID;
		else if @UpdateType=5
		UPDATE mysql_test...dealers SET HavingWebsite=@HavingWebsite WHERE ID=@ID;
		else if @UpdateType=6
		UPDATE mysql_test...dealers SET IsGroup = 1 WHERE Id = @ID
		else if @UpdateType=7
		UPDATE mysql_test...dealers SET IsMultiOutlet = 1 WHERE Id = @ID   
		else if @UpdateType=8
		UPDATE mysql_test...dealers SET ProfilePhotoHostUrl = @ProfilePhotoHostUrl , 
							   ProfilePhotoUrl = @ProfilePhotoUrl,
							   ProfilePhotoStatusId = 1, 
							   OriginalImgPath = @OriginalImgPath 
			WHERE Id = @ID
		else if @UpdateType=9	
		UPDATE mysql_test...dealers SET Status = 1 , isDealerActive = 0
				WHERE ID IN(
				SELECT ConsumerId FROM ConsumerCreditPoints CCP with(NOLOCK) 
				LEFT JOIN CWCTDealerMapping CMP with(NOLOCK) ON CCP.ConsumerId = CMP.CWDealerID
				WHERE ConsumerType = 1 AND DATEDIFF(DD, ExpiryDate, GETDATE()) >= 1
				AND ISNULL(CMP.IsMigrated,0) = 0
				)
				AND  Status = 0 AND TC_DealerTypeId = 1	
		else if @UpdateType=10		
		UPDATE mysql_test...dealers
				SET FirstName = @ContactPerson
					,EmailId = @EmailId
					,Organization = @Organization
					,Address1 = @Address1
					,Address2 = NULL
					,AreaId = @AreaId
					,CityId = @CityId
					,StateId = @StateId
					,Pincode = @Pincode
					,MobileNo = @MobileNo
					,FaxNo = @FaxNo
					,PhoneNo = @PhoneNo
					,LandlineCode = @LandLineCode
					,WebsiteUrl = @WebsiteUrl
					,ContactPerson = @ContactPerson
					,ShowroomStartTime = @ShowroomStartTime
					,ShowroomEndTime = @ShowroomEndTime
					,ContactEmail = @EmailId
					,LogoUrl = NULL
					,[Status] = case when @STATUS = 1 then 0 else 1 end
					,isDealerActive = @STATUS
					,Lattitude = @Lattitude
					,Longitude = @Longitude
					,LastUpdatedOn = GETDATE()
					,DealerLastUpdatedBy = @DealerLastUpdatedBy
					,ApplicationId=@ApplicationId --Mihir A Chheda [21-09-2016]
				WHERE ID = @ID	
		else if @UpdateType=11		
		UPDATE mysql_test...dealers
			SET LOGINID = @LOGINID
				,PASSWD = @PASSWD
				,FIRSTNAME = @FIRSTNAME
				,LASTNAME = @LASTNAME
				,EMAILID = @EMAILID
				,ORGANIZATION = @ORGANIZATION
				,ADDRESS1 = @ADDRESS1
				,ADDRESS2 = @ADDRESS2
				,AREAID = @AREAID
				,CITYID = @CITYID
				,STATEID = @STATEID
				,PINCODE = @PINCODE
				,PHONENO = @PHONENO
				,FAXNO = @FAXNO
				,MOBILENO = @MOBILENO
				,BPContactPerson = @BPCONTACTPERSON
				,EXPIRYDATE = @EXPIRYDATE
				,WEBSITEURL = @WEBSITEURL
				,CONTACTPERSON = @CONTACTPERSON
				,CONTACTHOURS = @CONTACTHOURS
				,CONTACTEMAIL = @CONTACTEMAIL
				,[STATUS] = @STATUS
				, isDealerActive = case when @STATUS = 1 then 0 else 1 end
				,LastUpdatedOn = @LastUpdatedOn
				,WebsiteContactPerson = @WebsiteContactPerson
				,WebsiteContactMobile = @WebsiteContactMobile
				,IsWarranty = @IsWarranty
				,IsInspection = @IsWarranty
				,AutoInspection = @AutoInspection
				,-- This column is used for Auto inspection request when New Stock Is Added in Autobiz
				BPMOBILENO = @BPMOBILENO
				,IsTCDealer = @IsTCDealer
				,IsWKitSent = @IsWKitSent
				,IsTCTrainingGiven = @IsTCTrainingGiven
				,Longitude = @Longitude
				,Lattitude = @Lattitude
				,ActiveMaskingNumber = @ActiveMaskingNumber
				,TC_DealerTypeId = @TC_DealerTypeId
				,OutletCnt = @OutletCnt
				,LeadServingDistance = @LeadServingDistance
				,RCNotMandatory = @RCNotMandatory
				,OwnerMobile = @OwnerMobile
				,LegalName = @LegalName
				,PanNumber = @PanNumber
				,TanNumber = @TanNumber
				,AutoClosed = @AutoClosed
				,DealerLastUpdatedBy = @DealerLastUpdatedBy
			WHERE Id = @ID
		else if @UpdateType=12
		UPDATE		mysql_test...dealers SET Status = @Status,isDealerActive = case when @STATUS = 1 then 0 else 1 end,LastUpdatedOn=@LastUpdatedOn
			WHERE		Id=@ID
		else if @UpdateType=13	
		UPDATE mysql_test...dealers
								SET IsPremium=@IsPremium
								WHERE Id= @ID and IsPremium<>@IsPremium
		else if @UpdateType=14
		UPDATE mysql_test...dealers 
						 SET  				         
						 DeletedReason	=	@DeletedReason,
						 DealerVerificationStatus =@DealerVerificationStatus
					 WHERE Id=@ID  
		else if @UpdateType=15 
		UPDATE mysql_test...dealers SET ActiveMaskingNumber = NULL 
				WHERE ActiveMaskingNumber IN(SELECT MaskingNumber 
											FROM MM_SellerMobileMasking SM WITH(NOLOCK) 
											INNER JOIN fnSplitCSVValuesWithIdentity(@Ids) AS F ON F.ListMember = SM.MaskingNumber
											)
		else if @UpdateType=16
		UPDATE mysql_test...dealers
				SET ActiveMaskingNumber = @ActiveMaskingNumber
				WHERE ID = @ID
		else if @UpdateType=17		
				 UPDATE mysql_test...dealers SET CertificationId = @CertificationId WHERE ID = @ID
		else if @UpdateType=18
		 UPDATE mysql_test...dealers 
						 SET  
						 DeletedBy	    =	@DealerLastUpdatedBy,       
						 DeletedOn	    =	@LastUpdatedOn,      
						 DeletedReason	=	@DeletedReason,
						 IsDealerDeleted =	@IsDealerDeleted,
						 Status          =  @Status , -- Dealer suspended
						 isDealerActive	=	case when @Status = 1 then 0 else 1 end,
						 DealerVerificationStatus =@DealerVerificationStatus  -- status Deleted(3)
					 WHERE Id=@Id  
		else if @UpdateType=19
			UPDATE mysql_test...dealers 
						 SET  
						 DeletedBy	    =	@DealerLastUpdatedBy,       
						 DeletedOn	    =	@LastUpdatedOn,      
						 IsDealerDeleted =	@IsDealerDeleted,
						 DeletedReason	=	@DeletedReason,
						 DealerVerificationStatus =@DealerVerificationStatus  
					 WHERE Id=@Id  
		else if @UpdateType=20
			UPDATE mysql_test...dealers SET ActiveMaskingNumber = NULL 
				WHERE ActiveMaskingNumber IN(SELECT MaskingNumber FROM MM_SellerMobileMasking WITH(NOLOCK) WHERE MM_SellerMobileMaskingId IN (SELECT ListMember FROM fnSplitCSV(@Ids))) 
		else if @UpdateType=21
			UPDATE mysql_test...dealers SET BPContactPerson=@BPContactPerson, BPMobileNo=@BPMobileNo 
						 WHERE Id=@ID
		else if @UpdateType=22
			UPDATE mysql_test...dealers SET LoginId = @LoginId, LastUpdatedOn = getdate() WHERE ID = @ID;
		else if @UpdateType=23
			UPDATE mysql_test...dealers SET LastServiceVisit=getdate() WHERE ID = @ID
		else if @UpdateType=24
			UPDATE mysql_test...dealers SET DeletedBy =@DealerLastUpdatedBy ,DeletedOn =@DeletedOn ,DeletedReason= @DeletedReason,DeletedComment = @DeleteComment 
			WHERE EmailId = @EmailId AND IsDealerDeleted = 1
		--else if @UpdateType = 25
		--UPDATE mysql_test...dealers
		--	SET PackageRenewalDate= @PackageRenewalDate
		--	WHERE ID= @ID
			--TRG_AI_ConsumerPackageRequests Added an Update in this trigger..HAS TO BE REMOVED WHEN SYNCING STOPS
		else if @UpdateType = 26
		UPDATE mysql_test...dealers SET  FirstName=@FIRSTNAME, LastName=@LASTNAME, 
									EmailId=@EMAILID, Organization=@ORGANIZATION, Address1=@ADDRESS1, AreaId=@AREAID,
									CityId=@CITYID, StateId=@STATEID, Pincode=@PINCODE, PhoneNo=@PHONENO, FaxNo=@FAXNO, 
									MobileNo=@MOBILENO, ContactPerson= @CONTACTPERSON, ContactHours=@CONTACTHOURS, ContactEmail=@CONTACTEMAIL, 
									DealerSource = @DealerSource, IsDealerDeleted = 0,TC_DealerTypeID=@TC_DealerTypeID WHERE  Id =  @ID
		else if @UpdateType=27
		UPDATE mysql_test...dealers 
						SET 
							Organization = ISNULL(@ORGANIZATION,Organization),ContactPerson = ISNULL(@CONTACTPERSON,ContactPerson),
							PhoneNo = ISNULL(@PHONENO,PhoneNo),MobileNo = ISNULL(@MOBILENO,MobileNo),ContactEmail = ISNULL(@CONTACTEMAIL,ContactEmail),
							EmailId = ISNULL(@EMAILID,EmailId),Address1 = ISNULL(@ADDRESS1,Address1),AreaId = ISNULL(@AREAID,AreaId),Pincode = ISNULL(@PINCODE,Pincode),
							CityId = ISNULL(@CITYID,CityId),StateId = ISNULL(@STATEID,StateId),Lattitude = ISNULL(@Lattitude,Lattitude), Longitude = ISNULL(@Longitude,Longitude),
							CertificationId = ISNULL(@CertificationId,CertificationId),TC_DealerTypeID = ISNULL(@TC_DealerTypeID,TC_DealerTypeId),
							LogoUrl = ISNULL(@LogoUrl,LogoUrl) , Status  = @STATUS , WebsiteUrl = ISNULL(@WEBSITEURL,WebsiteUrl), isDealerActive = case when @STATUS = 1 then 0 else 1 end
					
						WHERE ID = @ID
		else if @UpdateType =28
			UPDATE mysql_test...dealers SET LastUpdatedOn = @LastUpdatedOn WHERE ID = @Id
	end try
	BEGIN CATCH
		INSERT INTO CarWaleMyslSyncExceptions(ModuleName,SPName,ErrorMsg,TableName,FailedId,CreatedOn,InputParameter)
		VALUES('MysqlSync','SyncdealersWithMysqlUpdate',ERROR_MESSAGE(),'dealers',@Id,GETDATE(),@UpdateType)
	END CATCH
END

