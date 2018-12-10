IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_CarTrade_UpdateCertificationStatus]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_CarTrade_UpdateCertificationStatus]
GO

	-- =============================================
-- Author      : Chetan Navin
-- Create date : 16th Dec 2015
-- Description : To Update certification status called from update/delete api exposed to cartrade
-- Modified By : Khushaboo Patil - 19/2/2016 added data in TC_notifications to notify on when inspection report is available
--				 Chetan Navin - 24/02/1026 (Added parameter status description)

-- Modified By  : Khushaboo Patil - 31/3/2016 added only dealer principal userid in TC_notifications when inspection report is available
-- Modified By  : Chetan Navin - 04/20/2016 (Handled updation of certification status)
-- =============================================
CREATE PROCEDURE [dbo].[TC_CarTrade_UpdateCertificationStatus]
	@RequestURL					VARCHAR(2000),
	@RequestBody				VARCHAR(2000),
	@Response					VARCHAR(2000),
	@Status						SMALLINT,
	@ProductId					TINYINT,
	@ProductItemId				INT,
	@ListingId                  INT,
	@ActionType                 TINYINT,
	@StatusDescription          VARCHAR(150)    
AS
BEGIN
	DECLARE @RecordType INT,@RecordId INT,@DealerId INT

	--Step 1. Save api call log 
	EXEC TC_CarTrade_SaveAPICallLog @RequestURL,@RequestBody,@Response,@Status,@ProductId,@ProductItemId

	--Step 2. Save carTrade action Log
	INSERT INTO TC_CarTradeCertificationAction (ListingId,ActionType,EntryDate)
	VALUES(@ListingId,@ActionType,GETDATE())

	--Step 3. Save Status Change log
	INSERT INTO TC_CarTradeCertificationStatusChangeLog (TC_CarTradeCertificationRequestId,TC_CarTradeCertificationStatusId,EntryDate,StatusDate)
	SELECT TC_CarTradeCertificationRequestId,CertificationStatus,GETDATE(),ISNULL(StatusDate,EntryDate)
	FROM TC_CarTradeCertificationRequests WITH(NOLOCK)
	WHERE TC_CarTradeCertificationRequestId = @ProductItemId

	--Step 4. Update latest status in certificate request table
	UPDATE TC_CarTradeCertificationRequests SET CertificationStatus = 
	CASE WHEN CertificationStatus <= 4 THEN @Status ELSE CertificationStatus END
	,CertificationStatusDesc = @StatusDescription,StatusDate = GETDATE()	
	WHERE TC_CarTradeCertificationRequestId = @ProductItemId

	--Step 5. If status is approved and action is update,save in LiveListing
	IF(@Status = 1 AND @ActionType = 1)
	BEGIN
	IF NOT EXISTS(SELECT 1 FROM TC_CarTradeCertificationLiveListing WITH(NOLOCK) WHERE ListingId = @ListingId)
		BEGIN
			INSERT INTO TC_CarTradeCertificationLiveListing (ListingId,TC_CarTradeCertificationRequestId)
			VALUES(@ListingId,@ProductItemId)

			SET @RecordId = SCOPE_IDENTITY()
			
			SELECT @RecordType = TC_NotificationRecordTypeId FROM TC_NotificationRecordType WITH(NOLOCK) WHERE TypeName = 'Inspection report available for stock'
			SELECT @DealerId = DealerId FROM TC_CarTradeCertificationRequests WITH(NOLOCK) WHERE TC_CarTradeCertificationRequestId = @ProductItemId

			INSERT INTO TC_Notifications (RecordType,RecordId,TC_UserId,NotificationDateTime)	
			SELECT DISTINCT @RecordType,@RecordId,TU.Id,GETDATE()
			FROM TC_Users TU WITH(NOLOCK) 
			INNER JOIN Dealers D WITH(NOLOCK) ON D.ID = TU.BranchId
			INNER JOIN TC_UsersRole TUR WITH(NOLOCK) ON TUR.UserId = TU.Id
			WHERE BranchId = @DealerId AND IsActive = 1 AND D.TC_DealerTypeId IN (1,3) AND TUR.RoleId = 1

		END
	END
	--If action is delete : Step 5. Delete the record from LiveListing
	ELSE IF(@ActionType = 2)
	BEGIN
		DELETE FROM TC_CarTradeCertificationLiveListing WHERE TC_CarTradeCertificationRequestId = @ProductItemId
	END
END

