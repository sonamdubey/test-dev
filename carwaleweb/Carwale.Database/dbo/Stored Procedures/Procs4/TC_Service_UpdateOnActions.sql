IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_Service_UpdateOnActions]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_Service_UpdateOnActions]
GO

	-- ================================================
-- Author:		<Khushaboo Patil>
-- Create date: <15/7/2016>
-- Description:	update service booking status on booking cancel,pick up request and mark as delivered
-- =============================================
CREATE PROCEDURE [dbo].[TC_Service_UpdateOnActions]
	@InquiryLeadId INT ,
	@BookingStatus INT =NULL,
	@PickUpCompletedDate DATETIME = NULL,
	@ServiceDeliveredDate DATETIME = NULL,
	@ModifiedBy	INT
AS
BEGIN
	UPDATE SI SET BookingStatus = CASE WHEN @BookingStatus IS NOT NULL THEN @BookingStatus END,
	PickUpCompletedDate = CASE WHEN @PickUpCompletedDate IS NOT NULL THEN GETDATE() END
	,ModifiedBy = @ModifiedBy , ModifiedDate = GETDATE(),
	ServiceDeliveredDate = CASE WHEN @ServiceDeliveredDate IS NOT NULL THEN @ServiceDeliveredDate END
	FROM TC_Service_Inquiries  SI WITH(NOLOCK)
	WHERE SI.TC_InquiriesLeadId = @InquiryLeadId
END
