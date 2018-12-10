IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[GetTataVersionDetails]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[GetTataVersionDetails]
GO

	
-- =============================================
-- Author:		Shalini Nair
-- Create date: 17/08/2016
-- Description:	To get versionName as per Tata Version mapping
-- Modified by Anuj Dhar on <02/09/2016> to return version code of a random version of the model of the input version in case 
-- the version code of the input version is not present in CRM_TATAVersionMapping table.
-- =============================================
CREATE PROCEDURE [dbo].[GetTataVersionDetails] @VersionId INT
AS
DECLARE @VersionName VARCHAR(50)
DECLARE @ModelId INT
BEGIN
	SET @VersionName = ''

	SELECT @VersionName = CT.VersionCode
	FROM CRM_TATAVersionMapping CT WITH (NOLOCK)
	WHERE ct.VersionId = @VersionId

	IF (@VersionName = '')
	BEGIN
		SET @ModelId = (SELECT CarModelId
						FROM CarVersions WITH (NOLOCK)
						WHERE ID = @VersionId)
		SELECT TOP 1 VersionCode AS Name
		FROM CRM_TATAVersionMapping WITH (NOLOCK)
		WHERE ModelId = @ModelId
	END
	ELSE
	BEGIN
		SELECT @VersionName AS Name
	END
END
