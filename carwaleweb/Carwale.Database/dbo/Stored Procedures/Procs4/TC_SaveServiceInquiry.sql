IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_SaveServiceInquiry]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_SaveServiceInquiry]
GO

	
-- =============================================
-- Author:		Ruchira Patil
-- Create date : 13th Oct 2015
-- Description : To save service inquiry 
-- Modified By : Nilima More On 6th november 2015.
-- Description : To fetch Email AND Comments for NCD.
-- =============================================
CREATE PROCEDURE [dbo].[TC_SaveServiceInquiry]
	@BranchId INT,
	@TC_UserId INT,
	@CustomerName VARCHAR(25),
	@CustomerMobile VARCHAR(15),
	@CustomerEmail VARCHAR(50),
	@RegNo VARCHAR(15),
	@CarModelId INT,
	@VersionId INT,
	@ServiceDate DATETIME,
	@Kms INT,
	@ManufactureYear DATETIME,
	@TC_ServiceStatusId TINYINT,
	@TC_ServiceCenterId INT,
	@TC_ServiceInqSourceId TINYINT,
	@CustomerAddress VARCHAR(250) = NULL,
	@AreaId INT = NULL,
	@CityId INT = NULL,
	@StateId INT = NULL,
	@Pincode VARCHAR (10) = NULL,
	@pickupRequest BIT = NULL,
	@ServiceEmailId VARCHAR(60) = NULL OUTPUT,
	@ServicingId INT = NULL OUTPUT,
	@servicingComments VARCHAR(60) = Null OUTPUT


AS
BEGIN
	  SELECT @ServiceEmailId = SC.EmailId  FROM TC_ServiceCenter SC  WITH(NOLOCK) WHERE TC_ServiceCenterId = @TC_ServiceCenterId
	

	IF @CityId IS NULL
	BEGIN
		SET @CityId = (SELECT Cityid FROM TC_ServiceCenter WITH(NOLOCK) WHERE TC_ServiceCenterId  = @TC_ServiceCenterId)
		SET @StateId = (SELECT Top 1 StateId FROM Cities WITH(NOLOCK) WHERE Id = @CityId)
	END
	

	IF @ServicingId IS NULL
	BEGIN
		INSERT INTO TC_ServiceInquiries(BranchId,TC_UserId,CustomerName,CustomerMobile,CustomerEmail,RegNo,VersionId,CarModelId,ServiceDate,RequestDate,Kms,
					ManufactureYear,TC_ServiceStatusId,TC_ServiceCenterId,TC_ServiceInqSourceId,CustomerAddress,AreaId,CityId,StateId,Pincode,ServicingComments,PickupRequest)
		VALUES (@BranchId,@TC_UserId,@CustomerName,@CustomerMobile,@CustomerEmail,@RegNo,@VersionId,@CarModelId,@ServiceDate,GETDATE(),@Kms,
		@ManufactureYear,@TC_ServiceStatusId ,@TC_ServiceCenterId,@TC_ServiceInqSourceId,@CustomerAddress,@AreaId,@CityId,@StateId,@Pincode,@servicingComments,@pickupRequest)
	
	SET @ServicingId = SCOPE_IDENTITY()
	END
	ELSE
	BEGIN
		UPDATE TC_ServiceInquiries
		SET BranchId = @BranchId
			,CustomerName = @CustomerName
			,CustomerMobile = @CustomerMobile
			,CustomerEmail = @CustomerEmail
			,RegNo = @RegNo
			,VersionId = @VersionId
			,CarModelId = @CarModelId
			,ServiceDate = @ServiceDate
			,Kms = @Kms
			,ManufactureYear = @ManufactureYear
			,TC_ServiceStatusId = @TC_ServiceStatusId
			,TC_ServiceCenterId = @TC_ServiceCenterId
			,TC_ServiceInqSourceId = @TC_ServiceInqSourceId
			,CustomerAddress = @CustomerAddress
			,AreaId = @AreaId
			,CityId = @CityId
			,StateId = @StateId
			,Pincode = @Pincode
			,ModifiedBy = @TC_UserId
			,ModifiedDate = GETDATE()
			,ServicingComments = @servicingComments
			,PickupRequest = @pickupRequest
		WHERE TC_ServiceInquiriesId = @ServicingId
		
	  SELECT @servicingComments = ServicingComments FROM TC_ServiceInquiries WITH(NOLOCK) WHERE TC_ServiceInquiriesId = @ServicingId
		
	END
END