IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[SkodaRapidBooking_UpdatePaymentStatus]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[SkodaRapidBooking_UpdatePaymentStatus]
GO

	
-- =============================================  
-- Author:  Vikas
-- Create date: 15/11/2011 4:48 PM  
-- Description: To update payment status of Booking Laura RS. This SP will be called after successful transaction.  
-- =============================================  
CREATE PROCEDURE [dbo].[SkodaRapidBooking_UpdatePaymentStatus]  
 -- Add the parameters for the stored procedure here  
 @BookingId INT,  
 @IsPaymentSuccessful Bit,  
 @Status Bit = 0 Output  
AS  
BEGIN  
 -- SET NOCOUNT ON added to prevent extra result sets from  
 -- interfering with SELECT statements.  
 SET NOCOUNT ON;  
   
 IF(@BookingId IS NOT NULL AND @BookingId != -1 AND @IsPaymentSuccessful = 1)  
 BEGIN  
  UPDATE SkodaRapidBooking SET IsPaymentSuccessful = @IsPaymentSuccessful WHERE ID = @BookingId  
  SET @Status = 1  
 END  
END  
