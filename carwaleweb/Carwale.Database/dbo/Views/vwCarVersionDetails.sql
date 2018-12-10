IF EXISTS(
SELECT *
   FROM sys.views
     WHERE schema_id = SCHEMA_ID('dbo'))
     name = 'vwCarVersionDetails' AND
     DROP VIEW dbo.vwCarVersionDetails
GO

	CREATE VIEW vwCarVersionDetails
AS
SELECT  CV.ID as CarVersionId,
		CS.ID as CarSubSegmentsId,
		CB.ID as CarBodyStylesId
FROM   CarVersions  AS CV WITH (NOLOCK) 
JOIN   CarBodyStyles AS CB WITH (NOLOCK) ON CV.BodyStyleId=CB.Id
JOIN   CarSubSegments AS CS WITH (NOLOCK) ON CS.ID=CV.SubSegmentId