IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[ManageAndroidVersios]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[ManageAndroidVersios]
GO

	CREATE PROCEDURE ManageAndroidVersios







@VersionId INT,







@Desc VARCHAR(50),







@IsSupported bit=1,







@IsLatest bit=1







AS 



--Author: Rakesh Yadav



--Date Created: 22 jan 2014



--Desc: Add new android app version and make it latest



BEGIN







	UPDATE AndroidAppVersions







	SET IsLatest=0







	WHERE VersionId<>@VersionId AND IsSupported=1 AND IsLatest=1















	INSERT INTO AndroidAppVersions (VersionId,IsSupported,IsLatest,Description) VALUES (@VersionId,@IsSupported,@IsLatest,@Desc);







END