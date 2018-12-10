IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[PriceQuote_GetMake]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[PriceQuote_GetMake]
GO

	-- =============================================
-- Author:		<Chetan Kane>
-- Create date: <2/9/2012>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[PriceQuote_GetMake] 
	-- Add the parameters for the stored procedure here
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT Distinct Ma.Name AS Make, Ma.ID 
    FROM CarMakes AS Ma, CarModels MO, CarVersions Cv, Con_NewCarNationalPrices NAV 
    WHERE Ma.IsDeleted = 0 AND Ma.ID = MO.CarMakeId AND MO.ID = Cv.CarModelId AND Cv.ID = NAV.VersionId AND NAV.IsActive = 1 
    ORDER BY Make 
END
