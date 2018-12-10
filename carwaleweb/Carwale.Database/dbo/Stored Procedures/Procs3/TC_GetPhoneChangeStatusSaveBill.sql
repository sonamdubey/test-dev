IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_GetPhoneChangeStatusSaveBill]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_GetPhoneChangeStatusSaveBill]
GO
	
-- =============================================
-- Author:		Vicky Gupta
-- Create date: 23th Oct 2015
-- Description:	To get Phone number of dealer and change the status and save bill amount
-- =============================================
CREATE PROCEDURE [dbo].[TC_GetPhoneChangeStatusSaveBill] 
@ServiceInqId INT,
@StatusId SMALLINT = NULL,
@BillAmount INT = NULL,
@DealerPhone VARCHAR(10) OUTPUT
AS
BEGIN
	
	SELECT @DealerPhone = SC.PhoneNo 
	FROM
	TC_ServiceInquiries  SI WITH (NOLOCK) JOIN TC_ServiceCenter AS SC WITH (NOLOCK) ON SI.TC_ServiceInquiriesId=  @ServiceInqId AND SI.TC_ServiceCenterId = SC.TC_ServiceCenterId 

	IF(@StatusId IS NOT NULL)
	BEGIN
		UPDATE TC_ServiceInquiries SET TC_ServiceStatusId = @StatusId
		WHERE TC_ServiceInquiriesId = @ServiceInqId
	END

	IF(@BillAmount IS NOT NULL)
	BEGIN
		SELECT TC_ServiceInquiriesId FROM TC_ServiceInquiriesBill WITH (NOLOCK)
		WHERE TC_ServiceInquiriesId = @ServiceInqId
		IF(@@ROWCOUNT>0)
		BEGIN
			UPDATE TC_ServiceInquiriesBill SET TotalPrice = @BillAmount
			,TC_ServiceStatusId = @StatusId
			WHERE TC_ServiceInquiriesId = @ServiceInqId
		END
		ELSE
		BEGIN
			INSERT INTO TC_ServiceInquiriesBill ( TC_ServiceInquiriesId,TotalPrice, TC_ServiceStatusId) VALUES(@ServiceInqId,@BillAmount, @StatusId)
		END
	END
END