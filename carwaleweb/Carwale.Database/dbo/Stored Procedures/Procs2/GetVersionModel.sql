IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[GetVersionModel]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[GetVersionModel]
GO
-- =============================================
-- Author:		<Anuj Dhar>
-- Create date: <05/08/2016>
-- Description: To fetch the modelid for given versionid
-- =============================================
CREATE PROCEDURE [dbo].[GetVersionModel]
	@VersionId INT
	,@ModelId INT OUT
	,@MakeId INT OUT
AS
BEGIN
	SET NOCOUNT ON

	SET @ModelId = -1
	SET @MakeId = -1
	SELECT TOP 1 @MakeId = MakeId, @ModelId = ModelId
	FROM CD.vwMMV_16_12_1 WITH (NOLOCK)
	WHERE VersionId = @VersionId
END
