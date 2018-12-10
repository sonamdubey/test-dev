IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[WA_GetOtherVersions_15_1_1]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[WA_GetOtherVersions_15_1_1]
GO

	
/*
Author:Rakesh Yadav
Date: 02/07/2013
Desc : Get Other Versions of model
Modified By : Supriya on 1/7/2014 to add column SpecsSummary & Price
exec [dbo].[WA_GetOtherVersions] 2198
Modified By : Shalini on 30/12/14 to retrieve column New
*/
CREATE PROCEDURE [dbo].[WA_GetOtherVersions_15_1_1] 
@versionId INT,
@CityId INT 
AS
BEGIN
	SELECT CV.Id AS VersionId
		,CV.NAME AS VersionName
		,isnull(CV.SpecsSummary,' ') as SpecsSummary
		,SCP.Price AS Price
		,CV.New AS New
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



/****** Object:  StoredProcedure [dbo].[PQ_GetDealerSponsorship_API_V15.3.3]    Script Date: 3/30/2015 4:40:28 PM ******/
SET ANSI_NULLS ON
