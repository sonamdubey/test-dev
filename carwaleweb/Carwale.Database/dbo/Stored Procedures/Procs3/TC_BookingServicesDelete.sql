IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_BookingServicesDelete]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_BookingServicesDelete]
GO

	-- =============================================
-- Author:		Binumon George
-- Create date: 30th Nov 2011
-- Description:	Added status parameter
-- =============================================
-- Author:		Binumon George
-- Create date: 11th October 2011
-- Description:	This procedure will be used to Delete Services
-- =============================================
CREATE PROCEDURE [dbo].[TC_BookingServicesDelete]
(
@DealerId NUMERIC,
@TC_BookingServices_Id INT,
@Status INT OUTPUT
)

AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	SET @Status=0
	IF NOT EXISTS(SELECT Top 1 * FROM TC_BookingDelivery WHERE TC_BookingServices_Id=@TC_BookingServices_Id AND IsActive=1)
		BEGIN
			UPDATE TC_BookingServices  SET IsActive=0
			WHERE DealerId=@DealerId AND TC_BookingServices_Id=@TC_BookingServices_Id
			SET @Status=1
		END
		ELSE
		BEGIN
			SET @Status=2--record refrence with TC_BookingDelivery table. so cant delete
		END
END

