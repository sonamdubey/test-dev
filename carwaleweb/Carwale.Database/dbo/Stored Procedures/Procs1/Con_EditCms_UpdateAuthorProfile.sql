IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[Con_EditCms_UpdateAuthorProfile]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[Con_EditCms_UpdateAuthorProfile]
GO

	
CREATE Proc  [dbo].[Con_EditCms_UpdateAuthorProfile]
(
@AuthorId Bigint,
@Designation varchar(25),
@PicPath varchar(500),
@BriefProfile varchar(100),
@FullProfile varchar(500)
)
as 
 Update Con_EditCms_Author set Designation=@Designation,
 PicName=@PicPath,BriefProfile=@BriefProfile,
 FullProfile=@FullProfile where Authorid=@AuthorId
