IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[Microsite_DealerDeliveryEdit]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[Microsite_DealerDeliveryEdit]
GO

	
-- =============================================
-- Author:		Vivek Gupta
-- Create date: 17-04-2015
-- Description:	Show Delivery Detail
-- =============================================
CREATE PROCEDURE [dbo].[Microsite_DealerDeliveryEdit]
@DealerId INT,
@Id INT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	DECLARE @ApplicationId SMALLINT
	SELECT @ApplicationId = ApplicationId FROM Dealers WITH(NOLOCK) WHERE Id = @DealerId

	SELECT MD.VersionId, BookingAmount, DeliveryTime, CityId, V.MakeId, V.ModelId
	FROM Microsite_DeliveryTime MD WITH(NOLOCK)
	JOIN vwAllMMV V WITH(NOLOCK) ON MD.VersionId = V.VersionId AND V.ApplicationId = @ApplicationId
	WHERE MD.Id= @Id AND MD.DealerId = @DealerId
END

