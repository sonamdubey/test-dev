IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[Microsite_SelectStockImages]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[Microsite_SelectStockImages]
GO

	-- =============================================  
-- Author:  Umesh Ojha  
-- Create date: 30/4/2012  
-- Description: This SP returns images of the particular stock  
--Modified By Vivek Gupta on 12-08-2015 added originalimgpath
-- =============================================  
CREATE PROCEDURE [dbo].[Microsite_SelectStockImages]   
 -- Add the parameters for the stored procedure here  
 @StockId BigInt  
AS  
BEGIN  
 -- SET NOCOUNT ON added to prevent extra result sets from  
 -- interfering with SELECT statements.  
 SET NOCOUNT ON;  
  
    -- Insert statements for procedure here  
 select Id,ImageUrlFull,ImageUrlThumb,ImageUrlThumbSmall,IsMain,DirectoryPath,HostUrl , OriginalImgPath
 from TC_CarPhotos where IsActive=1 and StockId=@StockId  ORDER BY IsMain DESC, Id
END  












/****** Object:  StoredProcedure [dbo].[CarDetailsInitialize]    Script Date: 8/14/2015 11:49:38 AM ******/
SET ANSI_NULLS ON
