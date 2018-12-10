IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[IMG_SavePhotos]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[IMG_SavePhotos]
GO

	-- =============================================
-- Author      : Chetan Navin
-- Create date : 09/10/2015
-- Description : To save image details in table Img_Photos
-- =============================================
CREATE PROCEDURE [dbo].[IMG_SavePhotos]
	@CategoryId INT,
	@ItemId INT,
	@HostUrl VARCHAR(250),
	@OriginalPath VARCHAR(250),
	@IsProcessed BIT,
	@AspectRatio DECIMAL(5,3),
	@IsWaterMark BIT,
	@IsMain BIT,
	@IsMaster BIT,
	@Id BIGINT OUTPUT 

AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	INSERT INTO IMG_Photos(CategoryId,ItemId,HostUrl,OriginalPath,IsProcessed,AspectRatio,IsWaterMark,IsMain,IsMaster) 
	VALUES (@CategoryId,@ItemId,@HostUrl,@OriginalPath,@IsProcessed,@AspectRatio,@IsWaterMark,@IsMain,@IsMaster)

	SET @Id = SCOPE_IDENTITY()
END


-----------------------------------------------------------------------------------------------------------------

