IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_InquiryTypeSelect]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_InquiryTypeSelect]
GO

	-- Modified By: Surendra  
-- Create date: 2nd Apr,2012  
-- Description: added param @TC_DealerTypeId  
-- =============================================  
-- Author:  Surendra  
-- Create date: 12 Jan 2012  
-- Description: This procedure is used to get Inquiry Type like Buyer ,seller  
-- =============================================  
CREATE PROCEDURE [dbo].[TC_InquiryTypeSelect]  
@TC_DealerTypeId TINYINT  
AS  
BEGIN   

IF(@TC_DealerTypeId =0)
BEGIN
    SET @TC_DealerTypeId=1
END

IF(@TC_DealerTypeId=3)  
    BEGIN  
        SELECT TC_InquiryTypeId,InquiryType , Abbreviation 
        FROM TC_InquiryType WHERE IsActive=1  
    END  
ELSE  
    BEGIN 
         
        SELECT TC_InquiryTypeId,InquiryType, Abbreviation FROM TC_InquiryType 
        WHERE IsActive=1 AND (TC_LeadTypeId=@TC_DealerTypeId  OR TC_LeadTypeId IS NULL)
    END  
END


SET ANSI_NULLS ON
