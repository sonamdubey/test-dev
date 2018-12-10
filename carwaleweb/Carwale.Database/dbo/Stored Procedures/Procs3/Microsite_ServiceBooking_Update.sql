IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[Microsite_ServiceBooking_Update]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[Microsite_ServiceBooking_Update]
GO

	-- =============================================      
-- Author:  Kritika Choudhary
-- Create date: 25th Nov 2015
-- Description: To update payment details

-- =============================================      
CREATE PROCEDURE [dbo].[Microsite_ServiceBooking_Update]  
(      
@ServiceCompleted bit,
@PaymentAmount float,
@PGTransactionId int,
@IsPaymentDone bit,
@AutobizInqId int
 )      
AS      
BEGIN 
  
   -- SET NOCOUNT ON added to prevent extra result sets from      
 SET NOCOUNT ON; 
 IF(@AutobizInqId IS NOT NULL)
 BEGIN     
   UPDATE Microsite_ServiceBooking
	SET ServiceCompleted  = ISNULL(@ServiceCompleted, ServiceCompleted), PaymentAmount = ISNULL(@PaymentAmount, PaymentAmount),
	 PGTransactionId = ISNULL(@PGTransactionId, PGTransactionId), IsPaymentDone = ISNULL(@IsPaymentDone, IsPaymentDone),PaymentDateTime= GETDATE()
	WHERE AutobizInqId=@AutobizInqId;
END

END 

