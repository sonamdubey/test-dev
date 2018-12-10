IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[UpdateDealerPhotoInfo]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[UpdateDealerPhotoInfo]
GO

	-- =============================================
-- Author:		Anchal gupta
-- Create date: 14-12-2015
-- Description:	update the dealer inforamtion in microsite_images table from DealerPhotosUpload.aspx page
-- =============================================
CREATE  PROCEDURE [dbo].[UpdateDealerPhotoInfo]
	-- Add the parameters for the stored procedure here
	 @Id INT
	,@isMainBanner BIT
	,@remove INT
	,@isActive INT
AS
BEGIN
	UPDATE Microsite_Images
	SET isMainBanner = @isMainBanner
		,isDeleted = @remove
		,isActive = @isActive
	WHERE Id = @Id
END

