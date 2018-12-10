IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[Microsite_GetDealerBookingAmount]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[Microsite_GetDealerBookingAmount]
GO

	-- =============================================
-- Author:		<HARSH PATEL>
-- Create date: <14-4-2015>
-- Description:	<FETCH BOOKING AMOUNT>
-- =============================================
CREATE PROCEDURE [dbo].[Microsite_GetDealerBookingAmount]
	@onRoadPrice INT,
	@dealerId INT,
	@bookingAmount INT = 0 OUTPUT 
AS
BEGIN

	SELECT TOP 1 @bookingAmount = BookingAmount FROM Microsite_DealerBookingAmount
	WHERE DealerId = @dealerId AND  @onRoadPrice <= UpperLimit AND IsActive=1
	ORDER BY UpperLimit ASC

END
