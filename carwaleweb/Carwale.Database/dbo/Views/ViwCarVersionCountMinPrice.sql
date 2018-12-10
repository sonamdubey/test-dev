IF EXISTS(
SELECT *
   FROM sys.views
     WHERE schema_id = SCHEMA_ID('dbo'))
     name = 'ViwCarVersionCountMinPrice' AND
     DROP VIEW dbo.ViwCarVersionCountMinPrice
GO

	
--drop view ViwCarVersionCountMinPrice
CREATE VIEW ViwCarVersionCountMinPrice AS
SELECT
MO.ID AS ModelId,
CV.ID AS VersionId,
Count(SI.ID) AS CarCount,
Min(SI.Price) AS MinPrice

FROM
CarVersions AS CV,
CarModels AS MO,
SellInquiries AS SI
WHERE
CV.IsDeleted = 0 AND
MO.ID = CV.CarModelId AND
SI.CarVersionId = CV.ID AND
SI.Price >= 20000
GROUP BY
MO.ID, CV.ID
/*
order by
versionid, modelid*/
--select * from ViwCarVersionCountMinPrice



