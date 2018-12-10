IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[AbSure_BindImageTags]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[AbSure_BindImageTags]
GO

	-- =============================================
-- Author:		Ruchira Patil
-- Create date: 28th July 2015
-- Description:	to fetch image tags for carid for which there are no photos. If @IsDealer = 1 then fetch only RC tag 
-- =============================================
CREATE PROCEDURE [dbo].[AbSure_BindImageTags]
	@AbSure_CarDetailsId INT,
	@IsDealer			 BIT
AS
BEGIN

	IF @IsDealer = 0
	BEGIN
		SELECT	TagId AS Id,ImageDescription AS Name
		FROM	AbSure_ImageTags 
		WHERE	TagId NOT IN (SELECT ImageTagId FROM AbSure_CarPhotos WHERE AbSure_CarDetailsId=@AbSure_CarDetailsId AND ImageTagType=2) 
				AND ImageTypeId  = 2 --ImageTypeId = 2(OTHER PHOTOS)
	END
	ELSE -- To fetch only for dealer to upload RC image
	BEGIN
		SELECT	TagId AS Id,ImageDescription AS Name
		FROM	AbSure_ImageTags 
		WHERE	TagId = 1 -- RC Image tag id 
				AND ImageTypeId  = 2 --ImageTypeId = 2(OTHER PHOTOS) 
	END
END

