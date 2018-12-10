IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[BW_UpdateDealerBikeAvailability]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[BW_UpdateDealerBikeAvailability]
GO

	
-- =============================================
-- Author:		Suresh Prajapati
-- Create date: 12th Nov, 2014
-- Description:	To Edit Availability Days.
-- =============================================
CREATE PROCEDURE [dbo].[BW_UpdateDealerBikeAvailability] 
	@AvailabilityId INT
	,@Days INT
AS
BEGIN
	SET NOCOUNT OFF;

	UPDATE BW_BikeAvailability
	SET NumOfDays = @Days
	WHERE ID = @AvailabilityId
END

