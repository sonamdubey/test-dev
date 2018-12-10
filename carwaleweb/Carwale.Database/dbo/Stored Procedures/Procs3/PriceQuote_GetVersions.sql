IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[PriceQuote_GetVersions]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[PriceQuote_GetVersions]
GO

	-- =============================================
-- Author:		<Chetan Kane>
-- Create date: <2/9/2012>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[PriceQuote_GetVersions] 
	-- Add the parameters for the stored procedure here
	(
	@ModelId int 
	)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	 SELECT Vs.ID AS Value, Vs.Name AS Text
     FROM CarVersions Vs, Con_NewCarNationalPrices Ncp
     WHERE Vs.IsDeleted = 0 AND Vs.New = 1 AND Vs.ID = Ncp.VersionId AND Ncp.IsActive = 1 AND CarModelId = @ModelId 
     ORDER BY Text
END
