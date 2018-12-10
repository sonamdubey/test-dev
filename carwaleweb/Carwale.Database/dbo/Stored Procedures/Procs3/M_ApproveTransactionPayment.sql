IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[M_ApproveTransactionPayment]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[M_ApproveTransactionPayment]
GO

	-- =============================================
-- Author	:	Sachin Bharti(5th May 2015)
-- Description	:	Approve payment for created transaction
-- Modifier	:	Vaibhav K 3-Dec-2015
-- Added a new parameter as PaymentRejectedReasonId and saved in table DCRM_PaymentDetails
-- Modifier	:Mihir Chheda 19-Feb-2016
-- To resume suspended packages if auto paused on PaymentReceived of more then 0 amount 
-- =============================================
CREATE PROCEDURE [dbo].[M_ApproveTransactionPayment] 
	
	@PaymentId	INT,
	@PaymentReceivedDate DATETIME = NULL,
	@Reason		VARCHAR(250) = NULL, 
	@IsApproved	BIT, 
	@ApprovedBy	INT,
	@IsUpdated	BIT OUTPUT,
	@PayRejectedReasonId INT = NULL
AS
BEGIN
	UPDATE DCRM_PaymentDetails
		SET IsApproved = @IsApproved,
			ApprovedBy = @ApprovedBy,
			ApprovedOn = GETDATE(),
			ReceivedDate = @PaymentReceivedDate,
			RejectedReason = @Reason,
			PaymentRejectedReasonId = @PayRejectedReasonId
	WHERE
		ID = @PaymentId
	IF @@ROWCOUNT <> 0 
		SET @IsUpdated = 1

   -- To resume suspended packages if auto paused on PaymentReceived of more then 0 amount 
   -- or payment is done within time then jsut null the auto paused date if  package status is not paused and amount paid is more then 0-- 
   DECLARE @PaymentReceived INT = NULL
   IF (@IsUpdated=1 AND @IsApproved=1)
   BEGIN
	   SELECT @PaymentReceived=ISNULL(DPD.AmountReceived,0)
	   FROM   DCRM_PaymentDetails(NOLOCK) DPD
	   WHERE  DPD.ID=@PaymentId AND DPD.IsApproved = 1 
	   IF @PaymentReceived > 0 
	   BEGIN
	      EXEC DCRM_ResumeAutoSuspendedPackage @PaymentId
	   END
   END
END
-----------------------------------------------------------------------------------------------------------------------------------------------------------
