IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[WA_ManageAndroidVersios]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[WA_ManageAndroidVersios]
GO

	CREATE PROCEDURE [dbo].[WA_ManageAndroidVersios]

@VersionId INT,
@Desc VARCHAR(50),
@ApplicationType TinyInt,
@IsSupported bit=1,
@IsLatest bit=1

AS 

--Author: Rakesh Yadav
--Date Created: 22 jan 2014
--Desc: Add new android app version and make it latest
--Modified By: Vinay Kumar Prajapati 5th Nov 2014 , Adding ApplicationType Colomn  for Android Or iOS

BEGIN

	--mark prev latest version false

	UPDATE WA_AndroidAppVersions

	SET IsLatest=0  WHERE VersionId<>@VersionId AND IsSupported=1 AND IsLatest=1 AND ApplicationType=@ApplicationType



	--insert new version

	INSERT INTO WA_AndroidAppVersions (VersionId,IsSupported,IsLatest,Description,ApplicationType) VALUES (@VersionId,@IsSupported,@IsLatest,@Desc,@ApplicationType);

END