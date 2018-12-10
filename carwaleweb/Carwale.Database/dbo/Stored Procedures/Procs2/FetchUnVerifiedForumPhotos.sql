IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[FetchUnVerifiedForumPhotos]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[FetchUnVerifiedForumPhotos]
GO

	CREATE PROCEDURE [dbo].[FetchUnVerifiedForumPhotos]
AS 
BEGIN
--Date Created: 21 March 2016
--Author: Rakesh Yadav
--Desc: Fetch Un verified forum photos
SELECT 
	UserId,
	CASE 
	WHEN AvtOriginalImgPath IS NOT NULL AND AvtOriginalImgPath != ''
	THEN HostURL+'110x61'+AvtOriginalImgPath 
	END AvtarPhoto,
	CASE WHEN RealOriginalImgPath IS NOT NULL AND RealOriginalImgPath != ''
	THEN HostURL+'110x61'+RealOriginalImgPath 
	END RealPhoto 

	FROM UserProfile WITH(NOLOCK)
	WHERE (IsNull(RealOriginalImgPath,'')<>'' AND IsRealApproved=0) 
	OR (IsNull(AvtOriginalImgPath,'')<>'' AND IsAvtarApproved=0)
END

