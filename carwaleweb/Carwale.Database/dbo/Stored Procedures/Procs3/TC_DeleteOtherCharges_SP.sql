IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_DeleteOtherCharges_SP]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_DeleteOtherCharges_SP]
GO
	  
-- =============================================  
-- Author:  Binumon George  
-- Create date: 14-09-2011  
-- Description: Delete other charges  
-- =============================================  
CREATE PROCEDURE [dbo].[TC_DeleteOtherCharges_SP]   
 -- Add the parameters for the stored procedure here  
 @TC_PaymentOtherCharges_Id INT ,  
 @Status INT OUTPUT  
AS  
BEGIN  
	DECLARE @TC_CarBooking_Id INT  
	DECLARE  @Amount INT  
	SET @Status=0  
	SELECT @TC_CarBooking_Id=TC_CarBooking_Id,@Amount=Amount FROM TC_PaymentOtherCharges WHERE TC_PaymentOtherCharges_Id=@TC_PaymentOtherCharges_Id 
	DELETE  TC_PaymentOtherCharges WHERE TC_PaymentOtherCharges_Id=@TC_PaymentOtherCharges_Id  
	-- reducing  deleted other charges from totalcost and netpayment  
	UPDATE TC_CarBooking SET TotalAmount=TotalAmount-@Amount,NetPayment=NetPayment-@Amount WHERE TC_CarBookingId=@TC_CarBooking_Id  
	SET @Status=1  
END