IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[GoogleKeywordsByModel]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[GoogleKeywordsByModel]
GO

	-- =============================================
-- Author:		Shalini Nair
-- Create date: 08/10/14
-- Description:	Returns the Google Keywords based on ModelId passed
-- =============================================
CREATE PROCEDURE [dbo].[GoogleKeywordsByModel]
	-- Add the parameters for the stored procedure here
	@ModelId int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	 SELECT DISTINCT CM.Name AS Make, Se.Name AS SubSegment, Bo.Name CarBodyStyle 
				 FROM CarModels AS CMO WITH (NOLOCK), CarMakes AS CM WITH (NOLOCK), CarBodyStyles Bo WITH (NOLOCK), 
				(CarVersions Ve WITH (NOLOCK) LEFT JOIN CarSubSegments Se WITH (NOLOCK) ON Se.Id = Ve.SubSegmentId ) 
				 WHERE CM.ID=CMO.CarMakeId AND CMO.ID=Ve.CarModelId AND Bo.Id=Ve.BodyStyleId 
				AND Ve.CarModelId = @ModelId AND Ve.IsDeleted = 0 
END

