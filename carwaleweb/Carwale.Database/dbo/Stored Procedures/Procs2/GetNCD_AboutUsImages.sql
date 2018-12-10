IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[GetNCD_AboutUsImages]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[GetNCD_AboutUsImages]
GO

	
-- =============================================
-- Author:		Supriya Khartode
-- Create date: 10/9/2014
-- Description:	Get the images for about us for new car dealers based on dealerid passed
-- modified by sanjay on 13/3/2015 isBanner flag added.
-- =============================================
CREATE PROCEDURE [dbo].[GetNCD_AboutUsImages]
@DealerId Int
AS
BEGIN

	SET NOCOUNT ON;

    SELECT Id,HostURL+DirectoryPath+ThumbImage as ImgThumbUrl,LandingURL,HostURL+DirectoryPath+LargeImage as ImgLargeUrl 
    FROM Microsite_Images WITH (NOLOCK)
    WHERE DealerId=@DealerId AND IsActive = 1  and ThumbImage !='' AND IsBanner = 1 
END

