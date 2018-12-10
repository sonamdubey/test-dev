IF EXISTS(
SELECT *
   FROM sys.views
     WHERE schema_id = SCHEMA_ID('dbo'))
     name = 'AbsureScoreView' AND
     DROP VIEW dbo.AbsureScoreView
GO

	
CREATE VIEW [dbo].[AbsureScoreView]

AS

SELECT
	ProfileId, 
	PackageType,
	AbsureScore,
	Score AS OScore,
	(0.95 * Score + 0.05 *
		(CASE 
			WHEN ISNULL(AbsureScore, 94) BETWEEN 99 AND 100 THEN 1.0
			WHEN ISNULL(AbsureScore, 94) BETWEEN 97 AND 98 THEN 0.8
			WHEN ISNULL(AbsureScore, 94) BETWEEN 93 AND 96 THEN 0.5
			ELSE 0.2
		END) * SQUARE(ISNULL(AbsureScore, 94) / 100.0)) AS Score
FROM 
	LiveListings WITH(NOLOCK)
WHERE CityId NOT IN (176,12,40,2,105,198)

