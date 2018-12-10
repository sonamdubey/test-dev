IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[WA_GetOtherVersions]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[WA_GetOtherVersions]
GO

	/*



Author:Rakesh Yadav



Date: 02/07/2013



Desc : Get Other Versions of model
Modified By : Supriya on 1/7/2014 to add column SpecsSummary & Price
exec [dbo].[WA_GetOtherVersions] 2198

*/
CREATE PROCEDURE [dbo].[WA_GetOtherVersions] 
@versionId INT,
@CityId INT 
AS
BEGIN
	SELECT CV.Id AS VersionId
		,CV.NAME AS VersionName
		,CV.SpecsSummary
		,SCP.Price AS Price
	FROM CarVersions CV WITH (NOLOCK)
	LEFT JOIN NewCarShowroomPrices AS SCP WITH (NOLOCK) ON  SCP.CarVersionId = CV.ID AND SCP.CityId = @CityId
	WHERE CV.CarModelId = (
			SELECT CV1.CarModelId
			FROM CarVersions CV1 WITH (NOLOCK)
			WHERE CV1.ID = @versionId
			)
		AND CV.ID <> @versionId
		AND CV.New = 1
		AND CV.IsDeleted = 0
	ORDER BY CV.New DESC, Price ASC, CV.Name
END
