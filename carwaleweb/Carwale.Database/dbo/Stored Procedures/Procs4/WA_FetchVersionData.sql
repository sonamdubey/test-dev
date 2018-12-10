IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[WA_FetchVersionData]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[WA_FetchVersionData]
GO

	/*

Author:Rakesh Yadav

Date: 02/07/2013

Desc : Get all data of car version like details, overview, specifications,features, other versios and available colors
Modified By : Supriya on 1/7/2014 to pass cityId parameter to WA_GetOtherVersions sp
*/



CREATE  PROCEDURE [dbo].[WA_FetchVersionData]



@versionId INT,

@CityId INT      

 

AS

BEGIN

EXECUTE WA_GetVersionDetails @versionId,@CityId

EXECUTE [CD].[GetOverviewByVersionID] @versionId

EXECUTE [CD].[GetCarSpecsByVersionID] @versionId

EXECUTE [CD].[GetCarFeaturesByVersionID] @versionId

EXECUTE WA_GetOtherVersions @versionId,@CityId

EXECUTE WA_GetVersionColors @versionId

END

