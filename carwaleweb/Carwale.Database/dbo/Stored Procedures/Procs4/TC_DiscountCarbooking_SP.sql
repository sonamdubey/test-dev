IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_DiscountCarbooking_SP]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_DiscountCarbooking_SP]
GO

	
-- =============================================
-- Author:		Binumon George
-- Create date: 31-08-2011
-- Description: update TC_CarBooking  discount table
-- =============================================
CREATE PROCEDURE [dbo].[TC_DiscountCarbooking_SP] 
	-- Add the parameters for the stored procedure here
@Id INT,
@Discount DECIMAL,
@Status INT OUTPUT       

AS
BEGIN
	DECLARE @TotalAmount DECIMAL
	SELECT @TotalAmount=TotalAmount FROM TC_CarBooking  WHERE TC_CarBookingId=@Id

	UPDATE TC_CarBooking SET Discount=@Discount,NetPayment=@TotalAmount-@Discount WHERE TC_CarBookingId=@Id
	SET @Status=1
END