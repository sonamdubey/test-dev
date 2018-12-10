IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_MakeMainImage_SP]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_MakeMainImage_SP]
GO

	-- =============================================  
-- Author:  Vikas C 
-- Create date: MAR 02, 2011  
-- Description: SP to remove selected image, if selected image was main image   
--    then mark next available image as a main image 
-- Modified By: Nilesh Utture on 11th March, 2013 Updated LastUpdatedDate in TC_Stock
-- Modified By: Nilesh Utture on 03rd April, 2013 Added BranchId parameter and added Making image as Main Image
-- Modified By: Manish on 05-08-2015 for optimization purpose added condition IsMain=1
-- =============================================  
CREATE PROCEDURE [dbo].[TC_MakeMainImage_SP]  
 -- Add the parameters for the stored procedure here     
 @StockId  INT,  
 @PhotoId  INT,
 @BranchId INT=NULL
AS  
BEGIN   
IF EXISTS (SELECT Top(1) S.Id FROM TC_Stock S WITH (NOLOCK) WHERE S.Id = @StockId AND S.BranchId = @BranchId)-- Modified By: Nilesh Utture on 03rd April, 2013
	BEGIN
		 UPDATE TC_CarPhotos SET IsMain = 0 WHERE StockId = @StockId AND IsMain=1  ---IsMain condition added by Manish on 05-08-2015
		 UPDATE TC_CarPhotos SET IsMain = 1 WHERE Id = @PhotoId 
		 UPDATE TC_Stock SET LastUpdatedDate = GETDATE() WHERE Id = @StockId 
	END 
END
