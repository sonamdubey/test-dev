IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_InquiriesFollowupActionSelect]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_InquiriesFollowupActionSelect]
GO

	  
-- Author:  Surendra  
-- Create date: 12 Jan 2012  
-- Description: This procedure is used to get Inquiry Actions  
-- =============================================  
CREATE PROCEDURE [dbo].[TC_InquiriesFollowupActionSelect]   
AS  
BEGIN   
 SELECT TC_InquiriesFollowupActionId, ActionName FROM TC_InquiriesFollowupAction  WITH(NOLOCK)  WHERE IsActive=1 and TC_LeadTypeId=1
END  




