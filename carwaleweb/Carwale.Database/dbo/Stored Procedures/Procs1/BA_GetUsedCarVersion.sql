IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[BA_GetUsedCarVersion]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[BA_GetUsedCarVersion]
GO

	-- =============================================
-- Author:		Ranjeet
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[BA_GetUsedCarVersion]
	@ModelId Int
AS
BEGIN
	SET NOCOUNT ON;

	select CV.ID, CV.Name, CV.HostURL, CV.CarTransmission AS CarTransmission, CV.CarFuelType AS FuelType 
	from CarVersions AS CV WITH (NOLOCK) where CV.Used = 1 AND  CV.IsDeleted =0 AND CV.CarModelId = @ModelId
END
