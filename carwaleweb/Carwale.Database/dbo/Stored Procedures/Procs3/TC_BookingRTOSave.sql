IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_BookingRTOSave]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_BookingRTOSave]
GO

	-- =============================================
-- Author:		Binumon George	
-- Create date: 18 Oct 2011
-- Description:	save RTO details for booking
-- =============================================
CREATE PROCEDURE [dbo].[TC_BookingRTOSave]
(
@StockId INT ,
@DealerId INT,
@TC_BookingRTO_Id INT =NULL, 
@RTOBy CHAR(6)=NULL,
@IsRTONOCGiven BIT =NULL, 
@IsTransferSetGiven BIT =NULL, 
@IsBankNOCGiven BIT =NULL, 
@TC_RTO_Id INT =NULL,
@TC_RTOAgent_Id INT =NULL,
@AgentCommision INT =NULL,
@IsTransferLodged BIT= NULL,
@TransferLodgedDate DATE= NULL,
@IsDocumentDelivered BIT =NULL,
@DocumentDeliveredDate DATE =NULL,
@Comments VARCHAR(200) =NULL,
@transferCharges INT=NULL,
@Status INT OUTPUT
)	
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	SET @Status=0
	IF EXISTS(SELECT Id	FROM TC_Stock WHERE Id=@StockId AND BranchId=@DealerId)--checking valid stock id and dealerid
		BEGIN
			DECLARE @TC_CarBooking_Id INT
			SELECT @TC_CarBooking_Id=TC_CarBookingId FROM TC_Carbooking WHERE stockId=@StockId--taking bookingid as per stockid
			
			IF(@TC_BookingRTO_Id IS NULL AND @TC_CarBooking_Id IS NOT NULL)--checking @TC_BookingRTO_Id null or not.if null insert
				BEGIN
					INSERT INTO TC_BookingRTO(TC_CarBooking_Id, RTOBy, IsRTONOCGiven, IsTransferSetGiven, IsBankNOCGiven, TC_RTO_Id, TC_RTOAgent_Id, AgentCommisionPaid, IsTransferLodged, TransferLodgedDate, IsDocumentDelivered, DocumentDeliveredDate, Comments)
					VALUES(@TC_CarBooking_Id, @RTOBy, @IsRTONOCGiven, @IsTransferSetGiven, @IsBankNOCGiven, @TC_RTO_Id, @TC_RTOAgent_Id, @AgentCommision, @IsTransferLodged, @TransferLodgedDate, @IsDocumentDelivered, @DocumentDeliveredDate, @Comments)
					SET @Status=SCOPE_IDENTITY()
					IF NOT EXISTS(SELECT TC_CarBooking_Id FROM TC_PaymentOtherCharges WHERE TC_CarBooking_Id=@TC_CarBooking_Id AND TC_PaymentVariables_Id=2 )
						
							BEGIN
								--For insert new row in other charge payment
								INSERT TC_PaymentOtherCharges(TC_CarBooking_Id,Amount,TC_PaymentVariables_Id)VALUES(@TC_CarBooking_Id,0,2)
							END	
				END
				ELSE
				BEGIN --if @TC_BookingRTO not null update row
					IF(@TC_CarBooking_Id IS NOT NULL)
					BEGIN
						 UPDATE TC_BookingRTO SET TC_CarBooking_Id=TC_CarBooking_Id, RTOBy=@RTOBy, IsRTONOCGiven=@IsRTONOCGiven, IsTransferSetGiven=@IsTransferSetGiven,
						 IsBankNOCGiven=@IsBankNOCGiven, TC_RTO_Id=@TC_RTO_Id, TC_RTOAgent_Id=@TC_RTOAgent_Id, AgentCommisionPaid=@AgentCommision, IsTransferLodged=@IsTransferLodged,
						 TransferLodgedDate=@TransferLodgedDate, IsDocumentDelivered=@IsDocumentDelivered, DocumentDeliveredDate=@DocumentDeliveredDate, Comments=@Comments WHERE TC_BookingRTO_Id=@TC_BookingRTO_Id
						 SET @Status=@TC_BookingRTO_Id
						 IF NOT EXISTS(SELECT TC_CarBooking_Id FROM TC_PaymentOtherCharges WHERE TC_CarBooking_Id=@TC_CarBooking_Id AND TC_PaymentVariables_Id=2 )
						
							BEGIN
								--For insert new row in other charge payment
								INSERT TC_PaymentOtherCharges(TC_CarBooking_Id,Amount,TC_PaymentVariables_Id)VALUES(@TC_CarBooking_Id,0,2)
							END	
					END
					ELSE
					BEGIN
						SET @Status=-1--error.because stock not booked
					END
				END
		END
END


/****** Object:  StoredProcedure [dbo].[TC_BookingPremiumSave]    Script Date: 11/10/2011 17:51:10 ******/
SET ANSI_NULLS ON
