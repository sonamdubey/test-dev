IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_SaveMFCPhotoUrls]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_SaveMFCPhotoUrls]
GO

	-- =============================================
-- Author:		Vivek Gupta
-- Create date: 27-02-2015
-- Description:	saving MFC photo urls to prevent duplicacy of images
-- =============================================
CREATE PROCEDURE [dbo].[TC_SaveMFCPhotoUrls]
	@StockId INT,
	@Url VARCHAR(500),
	@StatusId SMALLINT = 1 OUTPUT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
    
	IF EXISTS (SELECT Id FROM TC_MFCPhotoUrl WITH(NOLOCK) WHERE StockId = @StockId AND PhotoUrl = @Url)
	BEGIN
	  SET @StatusId = 0
	END

	ELSE
	BEGIN
	   INSERT INTO TC_MFCPhotoUrl (StockId, PhotoUrl)
	   VALUES (@StockId, @Url)
	END
END
