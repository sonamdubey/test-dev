IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[UP_InsertFavouritePhoto]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[UP_InsertFavouritePhoto]
GO

	
--THIS PROCEDURE is for setting the review comment as abuse

CREATE PROCEDURE [dbo].[UP_InsertFavouritePhoto]
	@PhotoId		BIGINT,
	@CustomerId		BIGINT,
	@DateAdded		DateTime
 AS

BEGIN
	
	INSERT INTO UP_FavPhotos( CustomerId, PhotoId, DateAdded )
	VALUES( @CustomerId, @PhotoId, @DateAdded )

END
