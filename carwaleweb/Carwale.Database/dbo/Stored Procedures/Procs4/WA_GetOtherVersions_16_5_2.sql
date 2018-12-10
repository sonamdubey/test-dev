IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[WA_GetOtherVersions_16_5_2]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[WA_GetOtherVersions_16_5_2]
GO

	
/*
Author:Rakesh Yadav
Date: 02/07/2013
Desc : Get Other Versions of model
Modified By : Supriya on 1/7/2014 to add column SpecsSummary & Price
exec [dbo].[WA_GetOtherVersions] 2198
Modified By : Shalini on 30/12/14 to retrieve column New
*/
CREATE PROCEDURE [dbo].[WA_GetOtherVersions_16_5_2] --3585
@versionId INT
AS
BEGIN
	DECLARE @MODELID INT

	SELECT @MODELID = CV.CarModelId
	FROM CarVersions CV WITH (NOLOCK)
	WHERE CV.ID = @versionId;

	WITH VERSIONS
	AS (
		SELECT SP.CarVersionId AS VersionId
			,CV.NAME AS VersionName
			,isnull(CV.SpecsSummary, ' ') AS SpecsSummary
			,SP.Price
			,CV.New AS New
			,SP.CityId
			,ROW_NUMBER() OVER (
				PARTITION BY SP.CarVersionId ORDER BY SP.CityId DESC
				) AS CITYPREFERENCE
		FROM CarVersions CV WITH (NOLOCK)
		INNER JOIN NewCarShowroomPrices SP WITH (NOLOCK) ON SP.CarModelId = @MODELID
			AND SP.CarVersionId <> @versionId
			AND SP.CityId IN (
				1
				,10
				)
			AND CV.ID = SP.CarVersionId
		WHERE SP.IsActive = 1
		)
	SELECT V.VersionId
		,V.VersionName
		,V.SpecsSummary
		,V.Price
		,V.New
		,V.CityId
	FROM VERSIONS V
	WHERE V.CITYPREFERENCE = 1
	ORDER BY V.New DESC
		,V.Price ASC
		,V.VersionName
END

