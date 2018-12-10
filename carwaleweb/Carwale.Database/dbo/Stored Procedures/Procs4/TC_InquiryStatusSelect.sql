IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_InquiryStatusSelect]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_InquiryStatusSelect]
GO

	-- Author:  Surendra  
-- Create date: 12 Jan 2012  
-- Description: This procedure is used to get Inquiry Status like Hot ,warm  
-- =============================================  
CREATE PROCEDURE [dbo].[TC_InquiryStatusSelect]   
AS  
BEGIN   
 SELECT TC_InquiryStatusId ,Status FROM TC_InquiryStatus WHERE IsActive=1  
END  
  