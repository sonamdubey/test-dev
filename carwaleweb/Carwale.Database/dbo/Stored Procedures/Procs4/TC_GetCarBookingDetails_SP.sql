IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_GetCarBookingDetails_SP]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_GetCarBookingDetails_SP]
GO

	---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
-- =============================================
-- Modified By:		Surendra
-- Modified date: 06-09-2011
-- Description:	
-- =============================================

-- =============================================
-- Author:		Binumon George
-- Create date: 30-08-2011
-- Description:	Get car booking details
-- =============================================
CREATE PROCEDURE [dbo].[TC_GetCarBookingDetails_SP] 
	-- Add the parameters for the stored procedure here
@StockID INT     
AS
BEGIN
	DECLARE @TC_CarBooking_Id  INT
	SELECT @TC_CarBooking_Id=TC_CarBookingId FROM TC_CarBooking WHERE StockId=@StockId AND IsCanceled=0
	
	DECLARE @TotalOtherCharges DECIMAL(18,0)	
	SELECT @TotalOtherCharges=SUM(Amount) From TC_PaymentOtherCharges WHERE TC_CarBooking_Id=@TC_CarBooking_Id
	
	IF(@TotalOtherCharges IS NULL)
	BEGIN
		SET @TotalOtherCharges=0
	END
	
	SELECT cb.TC_CarBookingId, (@TotalOtherCharges + st.Price) AS TotalAmount, cb.Discount,
	((@TotalOtherCharges + st.Price)- cb.Discount)AS NetPayment,cb.StockId, cb.CustomerId, cb.UserId, cb.DeliveryDate,
	st.Price AS CarPrice,@TotalOtherCharges AS Othercharges
	FROM TC_CarBooking cb INNER JOIN TC_Stock st ON cb.StockId=st.Id
	WHERE  cb.TC_CarBookingId=@TC_CarBooking_Id AND cb.IsCanceled=0
	
	SELECT TC_PaymentReceived_Id,TC_CarBooking_Id, PR.TC_PaymentOptions_Id,OptionName, UserId, AmountReceived, PaymentType, 
		PayDate, ChequeNo,ChequeDate, BankName, Remarks
		FROM TC_PaymentReceived PR INNER JOIN TC_PaymentOptions PO ON PR.TC_PaymentOptions_Id=PO.TC_PaymentOptions_Id
		WHERE TC_CarBooking_Id=@TC_CarBooking_Id AND PR.IsActive=1
	
	SELECT  TC_PaymentOptions_Id, OptionName FROM TC_PaymentOptions WHERE IsActive=1
END


