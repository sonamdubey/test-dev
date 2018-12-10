IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[DCRM_ReleaseMaskedDealers]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[DCRM_ReleaseMaskedDealers]
GO

	-- =============================================
-- Author:		Ruchira Patil
-- Create date: 27th June 2014
-- Description:	<Description,,>
-- EXEC DCRM_ReleaseMaskedDealers '8108295365,9777777777,9896453298',5,0,1
-- Modified By : Sunil M. Yadav On 15th July 2016, Release masking Number for bikewale dealers and accept masking numbers in @Ids
-- Modified By : Mihir A. Chheda[18-08-2016] while masking number release also store state id in MM_AvailableNumbers table
-- Modified By : Sunil M. Yadav On 26th Aug 2016 , set cityId as -1 while releasing masking number for both carwale and bikewale dealers 
--				because masking numbers are mapped w.r.t. states.
-- =============================================
CREATE PROCEDURE [dbo].[DCRM_ReleaseMaskedDealers] 
	@Ids VARCHAR(MAX),
	@DeletedBy INT,
	@Status BIT OUTPUT,
	@IsDeleteMaskingNumber BIT = 0 -- if flag is true then release masking number based on number else based on id of MM_SellerMobileMasking table.
AS
BEGIN
--mysql sync
	declare 
		@ID	decimal(18,0)=null, @LoginId	varchar(30)=null, @Passwd	varchar(50)=null, @FirstName	varchar(100)=null, @LastName	varchar(100)=null, @EmailId	varchar(250)=null, @Organization	varchar(100)=null,@Address1	varchar(500)=null, @Address2	varchar(500)=null, @AreaId	decimal(18,0)=null, @CityId	decimal(18,0)=null, @StateId	decimal(18,0)=null, @Pincode	varchar(6)=null, @PhoneNo	varchar(50)=null,@FaxNo	varchar(50)=null,  @MobileNo	varchar(50)=null, @ExpiryDate	datetime=null, @WebsiteUrl	varchar(100)=null, @ContactPerson	varchar(200)=null, @ContactHours	varchar(30)=null, @ContactEmail	varchar(250)=null, @LastUpdatedOn	datetime=null, @CertificationId	smallint=null, @BPMobileNo	varchar(15)=null, @BPContactPerson	varchar(60)=null, @IsTCDealer	tinyint=null, @IsWKitSent	tinyint=null,@IsTCTrainingGiven	tinyint=null, @HostURL	varchar(100)=null, @TC_DealerTypeId tinyint=null,  @Longitude	float=null,  @Lattitude	float=null, @DeletedReason	smallint=null, @HavingWebsite	tinyint=null,@ActiveMaskingNumber	varchar(20)=null, @DealerVerificationStatus	smallint=null, @IsPremium	tinyint=null, @WebsiteContactMobile	varchar(50)=null, @WebsiteContactPerson	varchar(100)=null, @ApplicationId	tinyint=null, @IsWarranty	tinyint=null, @LeadServingDistance	smallint=null, @OutletCnt	int=null, @ProfilePhotoHostUrl	varchar(100)=null, @ProfilePhotoUrl	varchar(250)=null, @AutoInspection	tinyint=null, @RCNotMandatory	tinyint=null,@OriginalImgPath	varchar(250)=null,  @OwnerMobile	varchar(20)=null, @ShowroomStartTime	varchar(30)=null, @ShowroomEndTime	varchar(30)=null, @DealerLastUpdatedBy	int=null,@LegalName	varchar(50)=null,@PanNumber	varchar(50)=null, @TanNumber	varchar(50)=null, @AutoClosed	tinyint=null, @LandlineCode	varchar(4)=null,  @UpdateType int =null 
--mysql sync end
begin try
	IF(@IsDeleteMaskingNumber = 0)
	BEGIN
		INSERT INTO MM_AvailableNumbers(CityId,MaskingNumber,ServiceProvider,StateId)
		SELECT -1,MaskingNumber,SM.ServiceProvider,c.StateId -- Mihir A. Chheda[18-08-2016]
		FROM MM_SellerMobileMasking SM WITH(NOLOCK)
		INNER JOIN Dealers D WITH(NOLOCK) ON D.ID = SM.ConsumerId
		INNER JOIN Cities C WITH(NOLOCK) ON C.ID = D.CityId
		WHERE SM.MM_SellerMobileMaskingId IN (SELECT ListMember FROM fnSplitCSV(@Ids)) AND SM.MaskingNumber NOT IN(SELECT MaskingNumber FROM MM_AvailableNumbers WITH(NOLOCK))
		INSERT INTO MM_SellerMobileMaskingLog(MM_SellerMobileMaskingId,ConsumerId,ConsumerType,MaskingNumber,Mobile,ProductTypeId,NCDBrandId,ActionTakenOn,ActionTakenBy,Remarks,ServiceProvider)
		SELECT MM_SellerMobileMaskingId,ConsumerId,ConsumerType,MaskingNumber,Mobile,ProductTypeId,NCDBrandId,GETDATE(),@DeletedBy,'Record Deleted',ServiceProvider FROM MM_SellerMobileMasking WITH(NOLOCK)
		WHERE MM_SellerMobileMaskingId IN (SELECT ListMember FROM fnSplitCSV(@Ids)) 
	
		UPDATE Dealers SET ActiveMaskingNumber = NULL 
		WHERE ActiveMaskingNumber IN(SELECT MaskingNumber FROM MM_SellerMobileMasking WITH(NOLOCK) WHERE MM_SellerMobileMaskingId IN (SELECT ListMember FROM fnSplitCSV(@Ids)))
	
		set @UpdateType=20
		--mysql sync start
		exec SyncDealersWithMysqlUpdate @ID	, @LoginId	, @Passwd	, @FirstName	, @LastName	, @EmailId	, @Organization	,@Address1	, @Address2	, @AreaId	, @CityId	, @StateId	, @Pincode	, @PhoneNo	,@FaxNo	,  @MobileNo	, @ExpiryDate, @WebsiteUrl	, @ContactPerson	, @ContactHours	, @ContactEmail	,@Status	, @LastUpdatedOn, @CertificationId	, @BPMobileNo	, @BPContactPerson	, @IsTCDealer	, @IsWKitSent	,@IsTCTrainingGiven	, @HostURL	, @TC_DealerTypeId ,  @Longitude,  @Lattitude, @DeletedReason	, @HavingWebsite	,@ActiveMaskingNumber	, @DealerVerificationStatus	, @IsPremium	, @WebsiteContactMobile	, @WebsiteContactPerson	, @ApplicationId	, @IsWarranty	, @LeadServingDistance	, @OutletCnt	, @ProfilePhotoHostUrl	, @ProfilePhotoUrl	, @AutoInspection	, @RCNotMandatory	,@OriginalImgPath	,  @OwnerMobile	, @ShowroomStartTime	, @ShowroomEndTime	, @DealerLastUpdatedBy	,@LegalName	,@PanNumber	, @TanNumber	, @AutoClosed	, @LandlineCode	,  @Ids , @UpdateType 
		--mysql sync end
		DELETE FROM MM_SellerMobileMasking WHERE MM_SellerMobileMaskingId IN (SELECT ListMember FROM fnSplitCSV(@Ids))
	
		SET @Status = 1
	END
	ELSE
	BEGIN
	-- Release masking Number for bikewale dealers and accept masking numbers in @Ids
		INSERT INTO MM_AvailableNumbers(CityId,MaskingNumber,ServiceProvider,StateId)
		SELECT -1,MaskingNumber,SM.ServiceProvider,C.StateId
		FROM MM_SellerMobileMasking SM WITH(NOLOCK)
		INNER JOIN Dealers D WITH(NOLOCK) ON D.ID = SM.ConsumerId
		INNER JOIN Cities C WITH(NOLOCK) ON C.ID = D.CityId
		INNER JOIN fnSplitCSVValuesWithIdentity(@Ids) AS F ON F.ListMember = SM.MaskingNumber
		WHERE SM.MaskingNumber NOT IN(SELECT MaskingNumber FROM MM_AvailableNumbers WITH(NOLOCK))
		INSERT INTO MM_SellerMobileMaskingLog(MM_SellerMobileMaskingId,ConsumerId,ConsumerType,MaskingNumber,Mobile,ProductTypeId,NCDBrandId,ActionTakenOn,ActionTakenBy,Remarks,ServiceProvider)
		SELECT MM_SellerMobileMaskingId,ConsumerId,ConsumerType,MaskingNumber,Mobile,ProductTypeId,NCDBrandId,GETDATE(),@DeletedBy,'Record Deleted',ServiceProvider 
		FROM MM_SellerMobileMasking SM WITH(NOLOCK)
		INNER JOIN fnSplitCSVValuesWithIdentity(@Ids) AS F ON F.ListMember = SM.MaskingNumber
		
		UPDATE Dealers SET ActiveMaskingNumber = NULL 
		WHERE ActiveMaskingNumber IN(SELECT MaskingNumber 
									FROM MM_SellerMobileMasking SM WITH(NOLOCK) 
									INNER JOIN fnSplitCSVValuesWithIdentity(@Ids) AS F ON F.ListMember = SM.MaskingNumber
									)
	
			set @UpdateType=15
		--mysql sync start
		exec SyncDealersWithMysqlUpdate @ID	, @LoginId	, @Passwd	, @FirstName	, @LastName	, @EmailId	, @Organization	,@Address1	, @Address2	, @AreaId	, @CityId	, @StateId	, @Pincode	, @PhoneNo	,@FaxNo	,  @MobileNo	, @ExpiryDate, @WebsiteUrl	, @ContactPerson	, @ContactHours	, @ContactEmail	,@Status	, @LastUpdatedOn, @CertificationId	, @BPMobileNo	, @BPContactPerson	, @IsTCDealer	, @IsWKitSent	,@IsTCTrainingGiven	, @HostURL	, @TC_DealerTypeId ,  @Longitude,  @Lattitude, @DeletedReason	, @HavingWebsite	,@ActiveMaskingNumber	, @DealerVerificationStatus	, @IsPremium	, @WebsiteContactMobile	, @WebsiteContactPerson	, @ApplicationId	, @IsWarranty	, @LeadServingDistance	, @OutletCnt	, @ProfilePhotoHostUrl	, @ProfilePhotoUrl	, @AutoInspection	, @RCNotMandatory	,@OriginalImgPath	,  @OwnerMobile	, @ShowroomStartTime	, @ShowroomEndTime	, @DealerLastUpdatedBy	,@LegalName	,@PanNumber	, @TanNumber	, @AutoClosed	, @LandlineCode	,  @Ids , @UpdateType 
		--mysql sync end
		DELETE FROM MM_SellerMobileMasking 
		WHERE MaskingNumber IN (SELECT ListMember FROM fnSplitCSVValuesWithIdentity(@Ids))
	
		SET @Status = 1
	END
	 	end try
	BEGIN CATCH
		INSERT INTO CarWaleMyslSyncExceptions(ModuleName,SPName,ErrorMsg,TableName,FailedId,CreatedOn,InputParameter)
		VALUES('MysqlSync','DCRM_ReleaseMaskedDealers',ERROR_MESSAGE(),'SyncDealersWithMysqlUpdate',@Ids,GETDATE(),@UpdateType)
	END CATCH			
END

