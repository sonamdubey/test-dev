IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[BA_SaveStockImageInformation]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[BA_SaveStockImageInformation]
GO

	-- =============================================
-- Author:		Ranjeet Kumar
-- Create date: 21-May-14
-- Description:	Save the Upload Image information
-- =============================================
CREATE PROCEDURE [dbo].[BA_SaveStockImageInformation] 
	@StockId INT,
	@Dir VARCHAR(max),
	@ImageName VARCHAR(max),
	@Url VARCHAR(max) 
AS

BEGIN
	SET NOCOUNT ON;
	DECLARE  @StockImageId INT = -1
	--DECLARE @ID INT =-1 

   INSERT INTO [dbo].[BA_StockImage]
           ([StockId]
           ,[EntryDate]
           ,[ModifyDate]
           ,[Directory]
           ,[IsActive])
     VALUES
			(@StockId,
			GETDATE(),
			NULL,
			@Dir,
			1
			)
---Return the ID 
SELECT @StockImageId = SCOPE_IDENTITY() 

IF @StockImageId <> -1
BEGIN

---Save in IMG_AllCarPhotos Common info Table
INSERT INTO [dbo].[IMG_AllCarPhotos]
           ([CategoryId]
           ,[ItemId]
           ,[URL]
           ,[StatusId]
           ,[OriginalFilename]
           ,[EntryDate]
           ,[ItemStorage]
           ,[MaxServers])
     VALUES
			(8,
			 @StockImageId,
			 @Url,
			 1,
			 @ImageName,
			 GETDATE(),
			 NULL,
			 NULL)
--Insert
INSERT INTO [dbo].[BA_ImageSize]
           ([StockImageId]
           ,[Image]
           ,[Dir]
           ,[HostUrl]
           ,[StatusId]
           ,[IsReplicated]
           ,[Small]
           ,[Medium]
           ,[Large])
     VALUES
			(@StockImageId,
			@ImageName,
			@Dir,
			@Url,
			1,
			0,
		(CAST(@StockImageId as VARCHAR)+'_100x100.jpg'),
		(CAST(@StockImageId AS VARCHAR)+'_200x200.jpg'),
		(CAST(@StockImageId AS VARCHAR)+'_400x400.jpg')
			)

--ID
SELECT SCOPE_IDENTITY() AS ID 
END

ELSE

SELECT -1 AS ID --Fail


END
