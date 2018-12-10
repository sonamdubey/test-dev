IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[Microsite_GetFuel_TransData]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[Microsite_GetFuel_TransData]
GO

	-- =============================================
-- Author:		Chetan Kane
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[Microsite_GetFuel_TransData]
	-- Add the parameters for the stored procedure here
	@Id int
AS
BEGIN
	
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT distinct cv.CarFuelType,cv.CarTransmission FROM CarVersions cv 
	INNER JOIN CarModels cm ON cv.CarModelId = cm.ID WHERE cm.ID = @Id AND cv.CarFuelType <= 2
END