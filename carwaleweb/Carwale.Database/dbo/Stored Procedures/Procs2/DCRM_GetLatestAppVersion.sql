IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[DCRM_GetLatestAppVersion]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[DCRM_GetLatestAppVersion]
GO

	
-- =============================================
-- Author:		Komal Manjare
-- Create date: 09-December-2015
-- Description:	get DCRM application latest version
-- =============================================
CREATE PROCEDURE [dbo].[DCRM_GetLatestAppVersion]
@ApplicationTypeId INT=NULL
AS
BEGIN
select AV.ID, AV.VersionId,AV.IsSupported,AV.IsLatest,AV.Description, LA.Organization AS ApplicationType
FROM WA_AndroidAppVersions AS AV WITH(NOLOCK) INNER JOIN LA_Agencies LA WITH(NOLOCK) ON AV.ApplicationType = LA.Id
where IsLatest=1 AND IsSupported=1 AND LA.Id=@ApplicationTypeId
END
