IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[DeleteLiveListingOnPackageExpiryPart3]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[DeleteLiveListingOnPackageExpiryPart3]
GO

	-- =============================================
-- Author:		<Shikhar Maheshwari>
-- Create date: <5 Nov 2016>
-- Description:	<Update Dealers table>
-- =============================================
CREATE PROCEDURE [dbo].[DeleteLiveListingOnPackageExpiryPart3]
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	UPDATE Dealers SET Status = 1 
		WHERE ID IN (SELECT ConsumerId 
		FROM ConsumerCreditPoints CCP with(NOLOCK) 
		LEFT JOIN CWCTDealerMapping CMP with(NOLOCK)
			ON CCP.ConsumerId = CMP.CWDealerID 
			WHERE ConsumerType = 1 AND DATEDIFF(DD, ExpiryDate, GETDATE()) >= 1
		            AND ISNULL(CMP.IsMigrated,0) = 0) AND  Status = 0 AND TC_DealerTypeId = 1;
declare @ID	decimal(18,0)=null, @LoginId	varchar(30)=null, @Passwd	varchar(50)=null, @FirstName	varchar(100)=null, 
	@LastName	varchar(100)=null, @EmailId	varchar(250)=null, @Organization	varchar(100)=null,@Address1	varchar(500)=null, 
	@Address2	varchar(500)=null, @AreaId	decimal(18,0)=null, @CityId	decimal(18,0)=null, @StateId	decimal(18,0)=null, 
	@Pincode	varchar(6)=null, @PhoneNo	varchar(50)=null,@FaxNo	varchar(50)=null,  @MobileNo	varchar(50)=null, 
	@ExpiryDate	datetime=null, @WebsiteUrl	varchar(100)=null, @ContactPerson	varchar(200)=null, @ContactHours	varchar(30)=null, 
	@ContactEmail	varchar(250)=null,@Status	tinyint=null, @LastUpdatedOn	datetime=null, @CertificationId	smallint=null, 
	@BPMobileNo	varchar(15)=null, @BPContactPerson	varchar(60)=null, @IsTCDealer	tinyint=null, @IsWKitSent	tinyint=null,
	@IsTCTrainingGiven	tinyint=null, @HostURL	varchar(100)=null, @TC_DealerTypeId tinyint=null,  @Longitude	float=null,  
	@Lattitude	float=null, @DeletedReason	smallint=null, @HavingWebsite	tinyint=null,@ActiveMaskingNumber	varchar(20)=null, 
	@DealerVerificationStatus	smallint=null, @IsPremium	tinyint=null, @WebsiteContactMobile	varchar(50)=null, 
	@WebsiteContactPerson	varchar(100)=null, @ApplicationId	tinyint=null, @IsWarranty	tinyint=null, @LeadServingDistance	smallint=null, 
	@OutletCnt	int=null, @ProfilePhotoHostUrl	varchar(100)=null, @ProfilePhotoUrl	varchar(250)=null, @AutoInspection	tinyint=null, 
	@RCNotMandatory	tinyint=null,@OriginalImgPath	varchar(250)=null,  @OwnerMobile	varchar(20)=null, @ShowroomStartTime	varchar(30)=null, 
	@ShowroomEndTime	varchar(30)=null, @DealerLastUpdatedBy	int=null,@LegalName	varchar(50)=null,@PanNumber	varchar(50)=null, 
	@TanNumber	varchar(50)=null, @AutoClosed	tinyint=null, @LandlineCode	varchar(4)=null,  @Ids Varchar(MAX)=null, @UpdateType int =9 
	begin try
		exec SyncDealersWithMysqlUpdate @ID	, @LoginId	, @Passwd	, @FirstName	, @LastName	, @EmailId	, @Organization	,@Address1	, 
		@Address2	, @AreaId	, @CityId	, @StateId	, @Pincode	, @PhoneNo	,@FaxNo	,  @MobileNo	, @ExpiryDate, @WebsiteUrl	, 
		@ContactPerson	, @ContactHours	, @ContactEmail	,@Status	, @LastUpdatedOn, @CertificationId	, @BPMobileNo	, @BPContactPerson	, 
		@IsTCDealer	, @IsWKitSent	,@IsTCTrainingGiven	, @HostURL	, @TC_DealerTypeId ,  @Longitude,  @Lattitude, @DeletedReason	, 
		@HavingWebsite	,@ActiveMaskingNumber	, @DealerVerificationStatus	, @IsPremium	, @WebsiteContactMobile	, @WebsiteContactPerson	, 
		@ApplicationId	, @IsWarranty	, @LeadServingDistance	, @OutletCnt	, @ProfilePhotoHostUrl	, @ProfilePhotoUrl	, @AutoInspection	, 
		@RCNotMandatory	,@OriginalImgPath	,  @OwnerMobile	, @ShowroomStartTime	, @ShowroomEndTime	, @DealerLastUpdatedBy	,@LegalName	,
		@PanNumber	, @TanNumber	, @AutoClosed	, @LandlineCode	,  @Ids , @UpdateType 
 	end try
	BEGIN CATCH
		INSERT INTO CarWaleMyslSyncExceptions(ModuleName,SPName,ErrorMsg,TableName,FailedId,CreatedOn,InputParameter)
		VALUES('MysqlSync','DeleteLiveListingOnPackageExpiryPart3',ERROR_MESSAGE(),'SyncDealersWithMysqlUpdate',null,GETDATE(),@UpdateType)
	END CATCH		
	
END

