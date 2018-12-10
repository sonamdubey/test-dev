IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[Microsite_DealerDeliveryView]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[Microsite_DealerDeliveryView]
GO

	
-- =============================================
-- Author:		Vivek Gupta
-- Create date: 17-04-2015
-- Description: View all delivery time details of the dealer
-- =============================================
CREATE PROCEDURE [dbo].[Microsite_DealerDeliveryView]
@DealerId INT
AS
BEGIN	
	SET NOCOUNT ON;
    
	DECLARE @ApplicationId SMALLINT
	SELECT @ApplicationId = ApplicationId FROM Dealers WITH(NOLOCK) WHERE Id = @DealerId

	SELECT MD.Id, MD.BookingAmount, MD.DeliveryTime, C.Name AS City, V.Version AS Version , V.Car AS CarDetails
	FROM Microsite_DeliveryTime MD WITH(NOLOCK)
	JOIN Cities C WITH(NOLOCK) ON MD.CityId = C.ID
	JOIN vwAllMMV V WITH(NOLOCK) ON MD.VersionId = V.VersionId AND V.ApplicationId = @ApplicationId
	WHERE MD.DealerId = @DealerId
	ORDER BY CarDetails

END

