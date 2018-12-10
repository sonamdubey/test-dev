IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_CarTrade_SaveCertificationRequest]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_CarTrade_SaveCertificationRequest]
GO

	-- =============================================
-- Author       : Chetan Navin
-- Create date  : 15th Dec 2015
-- Description  : To Save details of certification request to cartrade
-- Modified By  : Chetan Navin - 15th Feb 2016 (Added @CertificationStatusDesc paremeter to save certification status description)
-- Modified By  : Khushaboo Patil - 19/2/2016 added data in TC_notifications to notify when inspection report is available
-- Modified By  : Khushaboo Patil - 31/3/2016 added only dealer principal userid in TC_notifications when inspection report is available
-- =============================================
CREATE PROCEDURE [dbo].[TC_CarTrade_SaveCertificationRequest] 
	@RequestURL VARCHAR(2000)
	,@RequestBody VARCHAR(2000)
	,@Response VARCHAR(2000)=NULL
	,@Status SMALLINT = NULL
	,@RegistrationNo VARCHAR(20)
	,@DealerId INT
	,@ListingId INT
	,@DealerName VARCHAR(100)
	,@DealerMobile VARCHAR(15)
	,@DealerAddress VARCHAR(500)
	,@Make VARCHAR(20)
	,@Model VARCHAR(20)
	,@Variant VARCHAR(20)
	,@Color VARCHAR(20)
	,@ManufacturingYear INT
	,@CarTradeCertificationId INT
	,@CertificationStatus TINYINT
	,@CertificationStatusDesc VARCHAR(150)
	,@DealerEmail VARCHAR(150)
	,@DealerContactPerson VARCHAR(10)
	,@DealerPinCode VARCHAR(10)
	,@DealerCity VARCHAR(20)
AS
BEGIN
	DECLARE @ProductItemId INT
	DECLARE @RecordType INT,@RecordId INT

	--Step 1. Save certification request  	
	INSERT INTO TC_CarTradeCertificationRequests (
		RegistrationNo
		,DealerId
		,ListingId
		,DealerName
		,DealerMobile
		,DealerAddress
		,Make
		,Model
		,Variant
		,Color
		,ManufacturingYear
		,CarTradeCertificationId
		,CertificationStatus
		,EntryDate
		,StatusDate
		,CertificationStatusDesc
		,DealerEmail
		,DealerContactPerson
		,DealerPinCode
		,DealerCity
		)
	VALUES (
		@RegistrationNo
		,@DealerId
		,@ListingId
		,@DealerName
		,@DealerMobile
		,@DealerAddress
		,@Make
		,@Model
		,@Variant
		,@Color
		,@ManufacturingYear
		,@CarTradeCertificationId
		,@CertificationStatus
		,GETDATE()
		,GETDATE()
		,@CertificationStatusDesc
		,@DealerEmail
		,@DealerContactPerson
		,@DealerPinCode
		,@DealerCity
		)

	SET @ProductItemId = SCOPE_IDENTITY()

	--Step 2 . Save in TC_CarTradeCertificationLiveListing if status is done
	IF(@CertificationStatus = 1)                --Certification done
	BEGIN
		IF NOT EXISTS(SELECT 1 FROM TC_CarTradeCertificationLiveListing WITH(NOLOCK) WHERE ListingId = @ListingId)
		BEGIN
			INSERT INTO TC_CarTradeCertificationLiveListing (ListingId,TC_CarTradeCertificationRequestId)
			VALUES(@ListingId,@ProductItemId)		
			
			SET @RecordId = SCOPE_IDENTITY()
			
			SELECT @RecordType = TC_NotificationRecordTypeId FROM TC_NotificationRecordType WITH(NOLOCK) WHERE TypeName = 'Inspection report available for stock'

			INSERT INTO TC_Notifications (RecordType,RecordId,TC_UserId,NotificationDateTime)	
			SELECT DISTINCT @RecordType,@RecordId,TU.Id,GETDATE()
			FROM TC_Users TU WITH(NOLOCK) 
			INNER JOIN Dealers D WITH(NOLOCK) ON D.ID = TU.BranchId
			INNER JOIN TC_UsersRole TUR WITH(NOLOCK) ON TUR.UserId = TU.Id
			WHERE BranchId = @DealerId AND IsActive = 1 AND D.TC_DealerTypeId IN (1,3) AND TUR.RoleId = 1

		END
	END
	--Step 2. Log api call
	EXEC TC_CarTrade_SaveAPICallLog @RequestURL
		,@RequestBody
		,@Response
		,@Status
		,1
		,@ProductItemId
END

---------------------------------------------------------

