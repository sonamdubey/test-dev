IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[BerkshireOtherVehicleInfo_SP]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[BerkshireOtherVehicleInfo_SP]
GO

	
-- =============================================
-- Author:		Ashish G. Kamble
-- Create date: 11/6/2012
-- Description:	On the basis of make, model, version selected retrieve the other vehicle information for berkshire insurance.
-- =============================================
CREATE PROCEDURE [dbo].[BerkshireOtherVehicleInfo_SP]
	@vehicleCode float,
	@modelCode float,
	@versionCode float
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    SELECT bv.VEHICLE_CODE, bv.CUBIC_CAPACITY,bv.CARRYING_CAPACITY,bv.FUEL
	FROM dbo.BerkshireVehicleInfo AS bv
	WHERE bv.MAKE_CODE = @vehicleCode AND bv.MODEL_CODE = @modelCode AND bv.SUBTYPE_CODE = @versionCode;
	
END

