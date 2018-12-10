IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_BookingDeliveryView]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_BookingDeliveryView]
GO

	-- =============================================
-- Author:		Binumon George	
-- Create date: 19 Oct 2011
-- Description:	Getting delivery details for booking
-- =============================================
CREATE PROCEDURE [dbo].[TC_BookingDeliveryView]
(
@StockId INT,
@DealerId INT
)	
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
		
	SELECT TC_BookingDelivery_Id, Delivery.TC_CarBooking_Id, IsDocDelivered, IsCarChecked, IsAccessoriesGiven,Delivery.DeliveryDate ,
	TC_BookingWarranties_Id, TC_BookingServices_Id,IsCarDelivered,Delivery.Comments
	FROM TC_BookingDelivery Delivery
	INNER JOIN TC_CarBooking B ON Delivery.TC_CarBooking_Id=B.TC_CarBookingId
	WHERE b.StockId=@StockId AND Delivery.IsActive=1 AND B.IsCanceled=0 
	
	SELECT TC_BookingWarranties_Id, WarrantyName FROM TC_BookingWarranties WHERE DealerId=@DealerId AND IsActive=1
	SELECT TC_BookingServices_Id, ServiceName FROM TC_BookingServices WHERE DealerId=@DealerId AND IsActive=1
   
END


/****** Object:  StoredProcedure [dbo].[TC_BookingDeliverySave]    Script Date: 11/10/2011 17:50:48 ******/
SET ANSI_NULLS ON
