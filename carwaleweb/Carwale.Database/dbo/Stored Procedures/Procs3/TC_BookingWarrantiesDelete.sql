IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_BookingWarrantiesDelete]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_BookingWarrantiesDelete]
GO

	-- =============================================
-- Author:		Binumon George
-- Create date: 30th Nov 2011
-- Description:	Added status parameter
-- =============================================
-- Author:		Binumon George
-- Create date: 11th October 2011
-- Description:	This procedure will be used to Delete warranties
-- =============================================
CREATE PROCEDURE [dbo].[TC_BookingWarrantiesDelete]
(
@DealerId NUMERIC,
@TC_BookingWarranties_Id INT,
@Status INT OUTPUT
)

AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	SET @Status=0
	IF NOT EXISTS(SELECT Top 1 * FROM TC_BookingDelivery WHERE TC_BookingWarranties_Id=@TC_BookingWarranties_Id AND IsActive=1)
		BEGIN
			UPDATE TC_BookingWarranties  SET IsActive=0
			WHERE DealerId=@DealerId AND TC_BookingWarranties_Id=@TC_BookingWarranties_Id
			SET @Status=1
		END
		ELSE
		BEGIN
			SET @Status=2--record refrence with TC_BookingDelivery table. so cant delete
		END
END

