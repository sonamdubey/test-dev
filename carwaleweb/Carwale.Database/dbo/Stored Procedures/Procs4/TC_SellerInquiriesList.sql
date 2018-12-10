IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_SellerInquiriesList]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_SellerInquiriesList]
GO

	-- =============================================  
-- Author:  <Nilesh Utture>  
-- Create date: <2nd November, 2012>  
-- Description: <Will get all the seller leads for particular Customer>  
-- EXEC TC_SellerInquiriesList 5, 974  
 -- AND I.IsActive=1   Commented
-- =============================================  
CREATE PROCEDURE [dbo].[TC_SellerInquiriesList]  
 -- Add the parameters for the stored procedure here  
 @BranchId BIGINT,  
 @CustomerId BIGINT  
AS  
BEGIN  
 -- SET NOCOUNT ON added to prevent extra result sets from  
 -- interfering with SELECT statements.  
 SET NOCOUNT ON;  
  
  -- AND I.IsActive=1   Commented
    -- Insert statements for procedure here  
 SELECT I.CarName, SI.MakeYear, SI.Price, SI.Colour, SI.RegNo, SI.TC_SellerInquiriesId, SI.IsPurchased 
 FROM TC_SellerInquiries SI  WITH (NOLOCK)
 INNER JOIN TC_Inquiries I WITH (NOLOCK) ON SI.TC_InquiriesId = I.TC_InquiriesId -- AND I.IsActive=1   
 WHERE I.BranchId=@BranchId AND I.TC_CustomerId=@CustomerId AND SI.IsPurchased<>1
END  




