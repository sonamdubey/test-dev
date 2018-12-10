IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[Microsite_DWVersionInfo]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[Microsite_DWVersionInfo]
GO

	-- =============================================  
-- Author: Rakesh Yadav on 26 JUN 2015
-- Fetch basic data of dealer versions
-- Modified By : Sunil M. Yadav On 27th Oct 2016, Get CWModelId.
-- ============================================= 
CREATE PROCEDURE [dbo].[Microsite_DWVersionInfo]
@VersionId INT,
@DealerId INT
AS

BEGIN
	SELECT CM.Name AS MakeName,DM.DWModelName, DV.DWVersionName,DV.CWVersionId,DV.ID,DM.CWModelId 
	FROM TC_DealerVersions DV WITH (NOLOCK)
	JOIN TC_DealerModels DM WITH (NOLOCK) ON DV.DWModelId=DM.ID
	JOIN CarModels CMO WITH (NOLOCK) ON DM.CWModelId=CMO.ID
	JOIN CarMakes CM WITH (NOLOCK) ON CM.ID=CMO.CarMakeId
	WHERE DV.ID=@VersionId AND DV.DealerId=@DealerId
END



