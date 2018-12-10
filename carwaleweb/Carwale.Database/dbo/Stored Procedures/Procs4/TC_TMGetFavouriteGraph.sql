IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_TMGetFavouriteGraph]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_TMGetFavouriteGraph]
GO

	--	Author		:	Vivek Singh(26th December 2013)

--	Description :-To get the users fav graph information
--	============================================================

CREATE PROCEDURE [dbo].[TC_TMGetFavouriteGraph] 
@LoggedInUserId NUMERIC(20,0)

AS
SET NOCOUNT ON;
BEGIN
  SELECT TFG.FavoriteGraphURL AS GraphUrl,TFG.GraphName AS GraphName FROM TC_TMUsersFavouriteGraph TFG WHERE TC_SpecialUsersId=@LoggedInUserId AND IsActive=1;
END
