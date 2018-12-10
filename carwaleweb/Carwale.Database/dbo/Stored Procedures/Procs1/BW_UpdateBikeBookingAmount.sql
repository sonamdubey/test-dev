IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[BW_UpdateBikeBookingAmount]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[BW_UpdateBikeBookingAmount]
GO

	
-- =============================================
-- Author:		Ashwini Todkar
-- Create date: 17th Dec 2014
-- Description:	To update bike booking amount of a dealer.
-- =============================================
CREATE PROCEDURE [dbo].[BW_UpdateBikeBookingAmount]
	@BookingId INT
	,@BookingAmount INT
AS
BEGIN
	UPDATE BW_DealerBikeBookingAmounts
	SET Amount = @BookingAmount
	WHERE ID = @BookingId
END

