IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[GetTopVersionByMake]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[GetTopVersionByMake]
GO

	-- =============================================
-- Author:		Amit Verma
-- Create date: 28 Aug 2013
-- Description:	Get top versions by makeid
-- EXEC GetTopVersionByMake 10
-- =============================================
CREATE PROCEDURE [dbo].[GetTopVersionByMake]
	-- Add the parameters for the stored procedure here
	@MakeId Int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
    SELECT CM.ID, CM.Name + (CASE CM.New WHEN 1 THEN '' ELSE '*' END) AS ModelName,CM.CarVersionID_Top VersionId,CV.Name VersionName,CSS.Name SubSegment
    FROM CarModels CM WITH (NOLOCK)
    LEFT JOIN CarVersions CV WITH (NOLOCK) ON CM.CarVersionID_Top = CV.ID
    LEFT JOIN CarSubSegments CSS WITH (NOLOCK) ON CM.SubSegmentID = CSS.Id
    WHERE CM.CarMakeId = @MakeId
    AND CM.IsDeleted = 0 ORDER BY CM.New DESC,CM.Name ASC
END
