IF EXISTS(
SELECT *
   FROM sys.views
     WHERE schema_id = SCHEMA_ID('dbo'))
     name = 'vwAllVersionColors' AND
     DROP VIEW dbo.vwAllVersionColors
GO

	CREATE VIEW [dbo].[vwAllVersionColors]
AS
SELECT  VC.CarVersionID VersionId,
		VC.Code			VersionCode,
		VC.Color		VersionColor,
		VC.HexCode		VersionHexCode,
		VC.ID			VersionColorsId,
		1 as ApplicationId
FROM	VersionColors AS VC WITH (NOLOCK)
WHERE	VC.IsActive=1

UNION ALL

SELECT  BVC.BikeVersionID  VersionId,
		BVC.Code		   VersionCode,
		BVC.Color		   VersionColor,
		BVC.HexCode		   VersionHexCode,
		BVC.ID			   VersionColorsId,
		2 as ApplicationId
FROM	BikeVersionColors AS BVC WITH(NOLOCK)
WHERE	BVC.IsActive=1

