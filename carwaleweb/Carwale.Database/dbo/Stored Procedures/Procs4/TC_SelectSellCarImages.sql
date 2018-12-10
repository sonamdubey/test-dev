IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_SelectSellCarImages]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_SelectSellCarImages]
GO

	-- =============================================    
-- Author:  Umesh Ojha    
-- Create date: 16/10/2012    
-- Description: This SP returns images of the Sell car stock
-- Modified by: Komal Manjare on 7th August 2015 OriginalImgPath fetched
-- =============================================    
CREATE PROCEDURE [dbo].[TC_SelectSellCarImages]     
 -- Add the parameters for the stored procedure here    
 @InquiryId BigInt    
AS    
BEGIN    
 -- SET NOCOUNT ON added to prevent extra result sets from    
 -- interfering with SELECT statements.    
 SET NOCOUNT ON;   
    
    -- Insert statements for procedure here    
	SELECT Id, ImageUrlFull, ImageUrlThumb, ImageUrlThumbSmall, IsMain,DirectoryPath,HostUrl,OriginalImgPath
	 FROM TC_SellCarPhotos WITH(NOLOCK)
	WHERE IsActive = 1 AND TC_SellerInquiriesId = @InquiryId ORDER BY IsMain DESC, Id  
END 