IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[GetVersionDetailsById]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[GetVersionDetailsById]
GO

	
-- =============================================
-- Author:		Chetan Thambad
-- Create date: 29-06-2016
-- Description:	Get Version Details by passing id
-- Modified By: Shalini Nair on 14/07/2016 to fetch airbagId and segmentId
-- =============================================
CREATE PROCEDURE [dbo].[GetVersionDetailsById]
	@VersionId INT
AS
BEGIN
	SELECT NCS.Displacement
		   ,NCS.GroundClearance
		   ,NCS.[Length]
		   ,CFT.FuelTypeId
		   ,IV.UserDefinedId as AirBagId
		   ,CV.SegmentId
	 from NewCarSpecifications NCS WITH(NOLOCK) 
	 JOIN CarFuelType CFT WITH(NOLOCK) ON NCS.FuelType = CFT.FuelType
	 JOIN Carversions CV with(NOLOCK) on NCS.CarVersionId = CV.ID
	 JOIN cd.ItemValues IV with(nolock) on IV.CarVersionId = CV.ID and ItemMasterId = 155
	where NCS.CarVersionId = @VersionId;
END

