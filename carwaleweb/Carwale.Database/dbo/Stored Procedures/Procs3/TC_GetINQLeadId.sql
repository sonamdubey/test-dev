IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_GetINQLeadId]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_GetINQLeadId]
GO
	-- =============================================  
-- Author:  Yuga Hatolkar
-- Created On: 17th Desc, 2015
-- Description: Get Inquiry LeadId
-- Modified By: Ashwini Dhamankar on Feb 2,2016(Added ISNULL condition on TC_LeadStageId)
-- =============================================  
CREATE PROCEDURE [dbo].[TC_GetINQLeadId] 
	 @LeadId BIGINT
AS  
BEGIN 
	
	SELECT TC_InquiriesLeadId FROM TC_InquiriesLead WITH(NOLOCK)
	WHERE TC_LeadId = @LeadId AND ISNULL(TC_LeadStageId,0) <> 3
	
END
