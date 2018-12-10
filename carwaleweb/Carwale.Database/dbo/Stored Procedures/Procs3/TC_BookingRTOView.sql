IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_BookingRTOView]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_BookingRTOView]
GO

	-- =============================================
-- Author:		Binumon George	
-- Create date: 18 Oct 2011
-- Description:	Getting RTO details for booking
-- =============================================
CREATE PROCEDURE [dbo].[TC_BookingRTOView]
(
@StockId INT,
@DealerId INT
)	
AS
BEGIN

	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
		 
	SELECT TC_BookingRTO_Id, RTOBy, IsRTONOCGiven, IsTransferSetGiven, IsBankNOCGiven, TC_RTO_Id, TC_RTOAgent_Id, 
	AgentCommisionPaid, IsTransferLodged, TransferLodgedDate, IsDocumentDelivered, DocumentDeliveredDate, RTO.Comments,OT.Amount AS RTOCharge
	FROM TC_BookingRTO RTO
	INNER JOIN TC_CarBooking B ON RTO.TC_CarBooking_Id=B.TC_CarBookingId
	LEFT JOIN TC_PaymentOtherCharges OT ON B.TC_CarBookingId=OT.TC_CarBooking_Id
	WHERE b.StockId=@StockId AND RTO.IsActive=1 AND B.IsCanceled=0 AND OT.TC_PaymentVariables_Id=2
	
	SELECT TC_RTO_Id, RTOName FROM TC_RTO WHERE DealerId=@DealerId AND IsActive=1
	
	SELECT TC_RTOAgent_Id, AgentName FROM TC_RTOAgent WHERE IsActive=1 AND TC_RTO_Id IN(SELECT TC_RTO_Id FROM TC_BookingRTO RTO INNER JOIN TC_CarBooking B ON RTO.TC_CarBooking_Id=B.TC_CarBookingId
	WHERE b.StockId=@StockId AND RTO.IsActive=1 AND B.IsCanceled=0)
   
   --SELECT TC_RTOAgent_Id, AgentName FROM TC_RTOAgent WHERE IsActive=1  
END


/****** Object:  StoredProcedure [dbo].[TC_BookingRTOSave]    Script Date: 11/10/2011 17:51:13 ******/
SET ANSI_NULLS ON
