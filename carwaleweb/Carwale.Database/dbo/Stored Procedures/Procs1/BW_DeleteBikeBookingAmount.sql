IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[BW_DeleteBikeBookingAmount]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[BW_DeleteBikeBookingAmount]
GO

	
-- =============================================
-- Author:		Suresh Prajapati
-- Create date: 30th Dec 2014
-- Description:	To Delete bike booking amount of a dealer.
-- =============================================
CREATE PROCEDURE [dbo].[BW_DeleteBikeBookingAmount]
	@BookingId INT
AS
BEGIN
	UPDATE BW_DealerBikeBookingAmounts
	SET IsActive = 0
	WHERE ID = @BookingId
END
