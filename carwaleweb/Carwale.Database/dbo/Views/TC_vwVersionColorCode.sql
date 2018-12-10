IF EXISTS(
SELECT *
   FROM sys.views
     WHERE schema_id = SCHEMA_ID('dbo'))
     name = 'TC_vwVersionColorCode' AND
     DROP VIEW dbo.TC_vwVersionColorCode
GO

	


CREATE VIEW [dbo].[TC_vwVersionColorCode]
AS
	SELECT	VCC.VersionColorsId,
			VCC.ColorCode,
			VC.CarVersionID,
			VC.Color,
			VC.HexCode,
			VC.ID,
			VCD.CarVersionCode,
			VCD.IsActive
	FROM	TC_VersionColourCode VCC
			INNER JOIN	VersionColors VC WITH(NOLOCK)
						ON VC.ID=VCC.VersionColorsId
			INNER JOIN	TC_VersionsCode VCD WITH(NOLOCK)
						ON VCD.CarVersionId=VC.CarVersionID
			INNER JOIN	CarVersions CV WITH(NOLOCK)
						ON CV.ID=VC.CarVersionID
	WHERE	VCC.IsActive=1




