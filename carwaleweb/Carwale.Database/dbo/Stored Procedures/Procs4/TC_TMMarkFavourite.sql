IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_TMMarkFavourite]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_TMMarkFavourite]
GO

	--	Author		:	Vivek Singh(26th December 2013)

--	Description :-To Store the users fav graph information
--	============================================================

CREATE PROCEDURE [dbo].[TC_TMMarkFavourite] 
@LoggedInUserId NUMERIC(20,0),
@GraphUrl       VARCHAR(150),
@IsMarked      INT = NULL,
@GraphName     VARCHAR(150),
@PageLoad      INT = NULL
AS
SET NOCOUNT ON;
BEGIN
DECLARE @Count INT;


IF(@IsMarked=1)
BEGIN
 SELECT @COUNT=COUNT(*) FROM TC_TMUsersFavouriteGraph WHERE TC_SpecialUsersId=@LoggedInUserId

 IF(@Count=1)
   BEGIN
    UPDATE TC_TMUsersFavouriteGraph SET FavoriteGraphURL=@GraphUrl, LastUpdatedOn=GETDATE(),IsActive=1,GraphName=@GraphName WHERE TC_SpecialUsersId=@LoggedInUserId;
    SELECT COUNT(*) AS COUNT FROM TC_TMUsersFavouriteGraph WHERE TC_SpecialUsersId=@LoggedInUserId AND IsActive=1;
   END
 ELSE
   BEGIN
    INSERT INTO TC_TMUsersFavouriteGraph VALUES(@LoggedInUserId,@GraphUrl,1,GETDATE(),GETDATE(),@GraphName)
    SELECT COUNT(*) AS COUNT FROM TC_TMUsersFavouriteGraph WHERE TC_SpecialUsersId=@LoggedInUserId AND IsActive=1;;
   END
 END

ELSE IF(@IsMarked=0)
  BEGIN
     UPDATE TC_TMUsersFavouriteGraph SET IsActive=0, LastUpdatedOn=GETDATE() WHERE TC_SpecialUsersId=@LoggedInUserId;
	 SELECT COUNT(*) AS COUNT FROM TC_TMUsersFavouriteGraph WHERE TC_SpecialUsersId=@LoggedInUserId AND IsActive=1;
  END
ELSE IF(@IsMarked IS NULL)
 BEGIN 
  IF(@PageLoad=1)
   BEGIN
     SELECT COUNT(*) AS COUNT FROM TC_TMUsersFavouriteGraph WHERE TC_SpecialUsersId=@LoggedInUserId AND IsActive=1 AND @GraphUrl=FavoriteGraphURL;
   END 
 END
END
