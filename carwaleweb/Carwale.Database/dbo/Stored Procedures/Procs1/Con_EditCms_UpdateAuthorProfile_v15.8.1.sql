IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[Con_EditCms_UpdateAuthorProfile_v15]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[Con_EditCms_UpdateAuthorProfile_v15]
GO

	-- Modified BY: Sanjay on 29 July 2015, Host Url And Time Stamp Added
-- =============================================   
CREATE Proc  [dbo].[Con_EditCms_UpdateAuthorProfile_v15.8.1]
(
@AuthorId Bigint,
@Designation varchar(25),
@PicPath varchar(500),
@BriefProfile varchar(100),
@FullProfile varchar(500),
@TimeStamp varchar(25),
@HostUrl varchar(250)
)
as 
 Update Con_EditCms_Author set Designation=@Designation, HostURL = @HostUrl,
 PicName=@PicPath + '?v=' + @TimeStamp,BriefProfile=@BriefProfile,
 FullProfile=@FullProfile where Authorid=@AuthorId

