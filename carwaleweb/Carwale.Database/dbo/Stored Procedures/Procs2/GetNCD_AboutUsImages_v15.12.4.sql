IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[GetNCD_AboutUsImages_v15]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[GetNCD_AboutUsImages_v15]
GO

	
-- =============================================
-- Author:		Supriya Khartode
-- Create date: 10/9/2014
-- Description:	Get the images for about us for new car dealers based on dealerid passed
-- modified by sanjay on 21/12/2015 isMainBanner added.
-- =============================================
CREATE PROCEDURE [dbo].[GetNCD_AboutUsImages_v15.12.4]
@DealerId Int
AS
BEGIN

	SET NOCOUNT ON;

    SELECT Id,HostURL,OriginalImgPath
    FROM Microsite_Images WITH (NOLOCK)
    WHERE DealerId=@DealerId AND IsActive = 1 AND IsDeleted = 0 order by isMainBanner desc
END


