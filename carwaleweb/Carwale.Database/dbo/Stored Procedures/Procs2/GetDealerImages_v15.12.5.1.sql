IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[GetDealerImages_v15]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[GetDealerImages_v15]
GO

	-- =============================================
-- Author:  Raghu  
-- Create date: 16/7/2013  
-- Modeified by: Anchal Gupta 
-- Description: Insering Data to the table when image uploaded
-- Modified By Sanjay Soni, Added StatusId field 
-- =============================================
CREATE PROCEDURE [dbo].[GetDealerImages_v15.12.5.1]
	-- Add the parameters for the stored procedure here
	 @DealerId Int
	
AS
BEGIN
	select Id, HostURL, OriginalImgPath as Img, IsBanner, isMainBanner, IsActive , StatusId
	from Microsite_Images with (NOLOCK)
	where DealerId =@DealerId and isDeleted =0
END
