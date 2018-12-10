IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[CRM_CallRqstFromDealershipSave]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[CRM_CallRqstFromDealershipSave]
GO

	-- =============================================
-- Author:		AMIT KUMAR
-- Create date: 26th sept 2013
-- Description:	To save the data of call requested from dealer.
-- Modifier : Amit Kumar 5th mar 2014(added IsApproved = 1, ApprovedOn=GETDATE(),ApprovedBy=@EventRaisedBy in update query)
-- =============================================
CREATE PROCEDURE [dbo].[CRM_CallRqstFromDealershipSave] 
@CbdId			NUMERIC(18,0),
@DealerId		NUMERIC(18,0),
@CallDate		DATETIME,
@LeadId			NUMERIC(18,0),
@EventRaisedBy	NUMERIC(18,0)

AS
BEGIN

	UPDATE CRM_CustomerCallRqstLog SET EventCompletedOn = @CallDate , EventCompletedBy =@EventRaisedBy,IsApproved = 1,ApprovedOn=GETDATE(),ApprovedBy=@EventRaisedBy  WHERE DealerId = @DealerId AND CBDId = @CbdId AND LeadId = @LeadId

	IF(@@ROWCOUNT = 0 )
	BEGIN
		INSERT INTO CRM_CustomerCallRqstLog(EventRaisedOn,CallRequestDate,DealerId,CBDId,LeadId,EventRaisedBy) VALUES (GETDATE(),@CallDate,@DealerId,@CbdId,@LeadId,@EventRaisedBy)
	END
END

