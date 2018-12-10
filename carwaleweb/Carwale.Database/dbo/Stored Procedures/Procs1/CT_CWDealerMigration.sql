IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[CT_CWDealerMigration]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[CT_CWDealerMigration]
GO

	-- =============================================
-- Author:		Kartik Rathod
-- Create date: 20 july 2016
-- Description:	this sp will give all 2 months buyer inquiries for dealer and also down all stocks of that dealer from live
--  Modified By : Sunil M. Yadav On 26th Aug 2016 , Get model Id Based on stock version id.
-- Modified By : Sunil M. Yadav On 29th Aug 2016 , select source from TC_BuyerInquiries. 
--				 For number masking inquiries i.e. TC_InquirySourceId = 6 , Send Source = 2 i.e Incoming call , Else 1 = Verified Click.
-- modified By :Mihir A Chheda[16-09-2016]
--              fetch dealer id from TC_InquiriesLead because for masking number 
--				inquiries stock is not mapped so we dont get dealer id from TC_Stock table 
-- Modified By : Vaibhav K 23 Sept 2016 remove stock suspension on migration of dealer to keep stock in autobiz
-- Modified By : Vaibhav K 13 Oct 2016 to update sell inquiries with status=2 so that stock is not uploaded to CW
-- Modified By : Navead K 7 November 2016 Changes related to mysql migration
-- =============================================
CREATE PROCEDURE [dbo].[CT_CWDealerMigration]
@DealerId BIGINT 
AS
BEGIN
	SET NOCOUNT ON;
	    DECLARE @ToFollowupdate DATETIME =Convert(date,DATEADD(mm,-2,getdate()));
		SET @ToFollowupdate = convert(DATETIME, convert(VARCHAR(10), @ToFollowupdate, 120) + ' 23:59:59');
	-------------------------process to remove the car stock-----------
	--maintaining log of the removed car--
	--Navead K 7 November 2016 commented this
	--INSERT INTO TC_StockUploadedLog(SellInquiriesId,DealerId,IsCarUploaded,CreatedOn)
	--SELECT		SI.ID,SI.DealerId,0,GETDATE()
	--FROM		LiveListings AS LL WITH (NOLOCK)
	--JOIN		SellInquiries AS SI WITH (NOLOCK)  on SI.ID = LL.InquiryId
	--WHERE		LL.SellerType = 1 AND SI.DealerId=@DealerId
	--removed car from live listing for perticular dealer--
	--Vaibhav K 13 Oct 2016 commented below code
	/*DELETE		LiveListings FROM LiveListings AS LL 
	JOIN		SellInquiries AS SI with(NOLOCK) on SI.ID = LL.InquiryId
	WHERE		LL.SellerType = 1 AND SI.DealerId=@DealerId*/
	--following query will update tc_stock table and make them suspend(status=4)--
	--Vaibhav K 23 Sept 2016 removed set status=4 from below update query
	UPDATE		TC_Stock 
	SET			IsSychronizedCW=0
	FROM		TC_Stock ST  with(NOLOCK)
	--Navead K 7 November 2016 commented this
	-- JOIN		SellInquiries SI  with(NOLOCK) ON ST.Id=SI.TC_StockId
	-- WHERE       SI.DealerId=@DealerId AND SI.StatusId=1
	WHERE       ST.BranchId=@DealerId
	
	--Vaibhav K 13 Oct 2016
	--Navead K 7 November 2016 commented this
	--UPDATE		SellInquiries
	--SET			StatusId = 2, PackageExpiryDate = NULL
	--WHERE       DealerId=@DealerId AND StatusId=1 AND SourceId = 2 --source:autobiz
	--decativate dealers
	UPDATE		Dealers SET Status = 1,LastUpdatedOn=GETDATE()
	WHERE		Id=@DealerId
		--mysql sync
				declare 
		@ID	decimal(18,0)=@DealerId, @LoginId	varchar(30)=null, @Passwd	varchar(50)=null, @FirstName	varchar(100)=null, @LastName	varchar(100)=null, @EmailId	varchar(250)=null, @Organization	varchar(100)=null,@Address1	varchar(500)=null, @Address2	varchar(500)=null, @AreaId	decimal(18,0)=null, @CityId	decimal(18,0)=null, @StateId	decimal(18,0)=null, @Pincode	varchar(6)=null, @PhoneNo	varchar(50)=null,@FaxNo	varchar(50)=null,  @MobileNo	varchar(50)=null, @ExpiryDate	datetime=null, @WebsiteUrl	varchar(100)=null, @ContactPerson	varchar(200)=null, @ContactHours	varchar(30)=null, @ContactEmail	varchar(250)=null,@Status	tinyint=1, @LastUpdatedOn	datetime=GETDATE(), @CertificationId	smallint=null, @BPMobileNo	varchar(15)=null, @BPContactPerson	varchar(60)=null, @IsTCDealer	tinyint=null, @IsWKitSent	tinyint=null,@IsTCTrainingGiven	tinyint=null, @HostURL	varchar(100)=null, @TC_DealerTypeId tinyint=null,  @Longitude	float=null,  @Lattitude	float=null, @DeletedReason	smallint=null, @HavingWebsite	tinyint=null,@ActiveMaskingNumber	varchar(20)=null, @DealerVerificationStatus	smallint=null, @IsPremium	tinyint=null, @WebsiteContactMobile	varchar(50)=null, @WebsiteContactPerson	varchar(100)=null, @ApplicationId	tinyint=null, @IsWarranty	tinyint=null, @LeadServingDistance	smallint=null, @OutletCnt	int=null, @ProfilePhotoHostUrl	varchar(100)=null, @ProfilePhotoUrl	varchar(250)=null, @AutoInspection	tinyint=null, @RCNotMandatory	tinyint=null,@OriginalImgPath	varchar(250)=null,  @OwnerMobile	varchar(20)=null, @ShowroomStartTime	varchar(30)=null, @ShowroomEndTime	varchar(30)=null, @DealerLastUpdatedBy	int=null,@LegalName	varchar(50)=null,@PanNumber	varchar(50)=null, @TanNumber	varchar(50)=null, @AutoClosed	tinyint=null, @LandlineCode	varchar(4)=null,  @Ids Varchar(MAX)=null, @UpdateType int =12 , @IsDealerDeleted bit = null, @DeleteComment varchar(500)  = null, @DeletedOn datetime =null , @PackageRenewalDate datetime = null, @DealerSource int = null,@LogoUrl varchar(100) = null
		set @UpdateType = 12
		begin try
exec [dbo].[SyncDealersWithMysqlUpdate] 
@ID	, @LoginId	, @Passwd	, @FirstName	, @LastName	, @EmailId	, @Organization	, @Address1 , @Address2	, @AreaId	, @CityId	, @StateId	, @Pincode	, @PhoneNo	, @FaxNo	, @MobileNo	, @ExpiryDate	, @WebsiteUrl	, @ContactPerson	, @ContactHours	, @ContactEmail	, @Status	, @LastUpdatedOn	, @CertificationId	, @BPMobileNo	, @BPContactPerson	, @IsTCDealer	, @IsWKitSent	, @IsTCTrainingGiven	, @HostURL	, @TC_DealerTypeId ,  @Longitude	, @Lattitude	, @DeletedReason	, @HavingWebsite	, @ActiveMaskingNumber	, @DealerVerificationStatus	, @IsPremium	, @WebsiteContactMobile	, @WebsiteContactPerson	, @ApplicationId	, @IsWarranty	, @LeadServingDistance	, @OutletCnt	, @ProfilePhotoHostUrl	, @ProfilePhotoUrl	, @AutoInspection	, @RCNotMandatory	, @OriginalImgPath	, @OwnerMobile	, @ShowroomStartTime	, @ShowroomEndTime	, @DealerLastUpdatedBy	, @LegalName	, @PanNumber	, @TanNumber	, @AutoClosed	, @LandlineCode	, @Ids , @UpdateType , @IsDealerDeleted , @DeleteComment , @DeletedOn  , @PackageRenewalDate, @DealerSource, @LogoUrl
 	end try
	BEGIN CATCH
		INSERT INTO CarWaleMyslSyncExceptions(ModuleName,SPName,ErrorMsg,TableName,FailedId,CreatedOn,InputParameter)
		VALUES('MysqlSync','CT_CWDealerMigration',ERROR_MESSAGE(),'SyncDealersWithMysqlUpdate',@Id,GETDATE(),@UpdateType)
	END CATCH			
-- mysql sync end
	-------------------------------------------------------------------------
    
	
	------------ getbuyer inquiries of last 2 months for dealer--------------
	SELECT ST.Id AS stockId  , case when CUS.CustomerName is null or cus.customername='' then  'unknown' else cus.CustomerName end AS custName, 
	CUS.Mobile AS custMobile,
	CUS.Email AS custEmail, IL.BranchId AS cw_dealer_id , BI.TC_BuyerInquiriesId AS cw_lead_id, INQ.Source AS cw_original_source -- Miihr A Chheda 16-09-2016
	,CASE BI.TC_InquirySourceId WHEN 6 THEN 2 ELSE 1 END AS Source,		-- Sunil M. Yadav On 29th Aug 2016			
	BI.Comments AS CustomerComments, BI.BuyDate AS BuyingTime , CV.CarModelId AS Model_id							
	,TIS.Status AS StatusCategory,BI.CreatedOn AS Lead_date,IL.ModifiedDate AS Lead_lastUpdated,DATEPART(yyyy,ST.MakeYear) AS Mfgyear
	, ISNULL(IL.ModifiedDate,IL.CreatedDate) AS StatusDate 
	, CASE ISNULL(TLS.TC_LeadStageId,0) WHEN 1 THEN 'Fresh' WHEN 2 THEN 'Followup' 
	WHEN 3 THEN (CASE IL.TC_LeadDispositionID WHEN 4 THEN 'Converted' ELSE 'Closed' END) 
	ELSE 'Fresh' END AS Status,
	CASE ISNULL(TLS.TC_LeadStageId,0) WHEN 2 THEN ISNULL(TNC.NextAction,'Call Later') WHEN 1 THEN '' ELSE TLD.Name END AS SubStatus
	FROM	TC_BuyerInquiries AS BI WITH(NOLOCK)
	JOIN	TC_InquiriesLead AS IL  WITH(NOLOCK) ON BI.TC_InquiriesLeadId = IL.TC_InquiriesLeadId AND IL.TC_LeadInquiryTypeID = 1
	JOIN	TC_CustomerDetails AS CUS WITH(NOLOCK) ON IL.TC_CustomerId = CUS.Id
	LEFT JOIN	TC_Stock AS ST WITH(NOLOCK) ON BI.StockId = ST.Id
	LEFT JOIN CarVersions CV WITH(NOLOCK) ON CV.ID = ST.VersionId			-- Sunil M. Yadav On 26th Aug 2016
	LEFT JOIN	TC_InquirySource AS INQ WITH(NOLOCK) ON BI.TC_InquirySourceId = INQ.Id
	LEFT JOIN TC_InquiryStatus TIS WITH(NOLOCK) ON TIS.TC_InquiryStatusId = IL.TC_InquiryStatusId
	LEFT JOIN TC_LeadStage TLS WITH(NOLOCK) ON TLS.TC_LeadStageId = IL.TC_LeadStageId
	LEFT JOIN TC_LeadDisposition TLD WITH(NOLOCK) ON TLD.TC_LeadDispositionId = IL.TC_LeadDispositionID
	LEFT JOIN TC_ActiveCalls TAC WITH(NOLOCK) ON TAC.TC_LeadId = IL.TC_LeadId
	LEFT JOIN TC_NextAction TNC WITH(NOLOCK) ON TNC.TC_NextActionId = TAC.TC_NextActionId
	WHERE	IL.BranchId = @DealerId AND BI.CreatedOn > @ToFollowupdate
	-------------------------------------------------------------------------
END

