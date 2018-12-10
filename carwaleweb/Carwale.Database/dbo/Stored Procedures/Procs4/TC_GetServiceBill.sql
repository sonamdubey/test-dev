IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_GetServiceBill]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_GetServiceBill]
GO

	-- =============================================
-- Author     : Vinay Kumar Prajapati  19th Oct 2015
-- Description: To Get ServiceCenter Bill Details  
-- EXEC  [TC_GetServiceBill] 1,'8149214429'
-- =============================================

CREATE PROCEDURE [dbo].[TC_GetServiceBill]
(
@ServiceBillNo INT    ,
@EmailOrMobileNo VARCHAR(100)  = NULL
)
AS 
 BEGIN
       --Avoid Extra message 
       SET NOCOUNT ON

	   SELECT  ISNULL(SB.TotalPrice,0) AS TotalPrice,ISNULL(SB.Discount,0) AS Discount ,ISNULL(SB.ServiceTax,'') AS ServiceTax ,ISNULL(SB.FilePath,'') AS FilePath
	   FROM TC_ServiceInquiriesBill AS SB WITH(NOLOCK) 
	   JOIN TC_ServiceInquiries SI WITH(NOLOCK) ON SI.TC_ServiceInquiriesId = SB.TC_ServiceInquiriesId
	   WHERE SB.TC_ServiceInquiriesId=@ServiceBillNo
	   AND (SI.CustomerMobile = @EmailOrMobileNo OR SI.CustomerEmail =@EmailOrMobileNo)

 END 







 
/****** Object:  StoredProcedure [dbo].[TC_GetGCMIdForUser]    Script Date: 12/2/2015 3:21:36 PM ******/
SET ANSI_NULLS ON
