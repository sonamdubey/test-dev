IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[BW_GetAvailabilityDays]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[BW_GetAvailabilityDays]
GO

	
-- Author: Suresh Prajapati
-- Create date: 13th Nov, 2014
-- Description: To Availability Days By Specified Dealer and Version id.
-- =============================================
CREATE PROCEDURE [dbo].[BW_GetAvailabilityDays] @DealerId INT
	,@VersionId INT
AS
BEGIN
	SELECT ISNULL(NumOfDays,0) NumOfDays
	FROM BW_BikeAvailability WITH (NOLOCK)
	WHERE DealerId = @DealerId
		AND BikeVersionId = @VersionId
		AND IsActive = 1
END
