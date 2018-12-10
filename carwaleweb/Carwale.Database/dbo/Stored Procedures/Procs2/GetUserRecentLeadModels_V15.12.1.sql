IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[GetUserRecentLeadModels_V15]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[GetUserRecentLeadModels_V15]
GO

	

-- ==========================================================================================
-- Author: Vicky Lund
-- Create date: 01/12/2015
-- Description:	Get Recently submitted leads by a user based on user's mobile number
-- exec [dbo].[GetUserRecentLeadModels_V15.12.1] '9975002486'
-- ==========================================================================================
CREATE PROCEDURE [dbo].[GetUserRecentLeadModels_V15.12.1] @MobileNo VARCHAR(15)
AS
BEGIN
	SELECT CV.CarModelId AS ModelId
		,DAL.CityId
		,DAL.ZoneId AS ZoneId
		,DAL.PlatformId
	FROM PQDealerAdLeads DAL WITH (NOLOCK)
	INNER JOIN CarVersions CV WITH (NOLOCK) ON DAL.VersionId = CV.Id
		AND Mobile = @MobileNo
		AND DAL.RequestDateTime > DATEADD(MONTH, - 2, GETDATE())
	ORDER BY DAL.RequestDateTime DESC
END
