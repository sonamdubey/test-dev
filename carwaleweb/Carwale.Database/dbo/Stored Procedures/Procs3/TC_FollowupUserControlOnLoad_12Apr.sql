IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_FollowupUserControlOnLoad_12Apr]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_FollowupUserControlOnLoad_12Apr]
GO
	
-- Created By:	Surendra
-- Create date: 30 Jan 2012
-- Description:	This Procedure will be used on the form load of Followup for Buyer
-- =============================================
CREATE PROCEDURE [dbo].[TC_FollowupUserControlOnLoad_12Apr]
(
@CustomerId INT,
@BranchId NUMERIC,
@InquiryType INT
)
as
-- Calling Procedure that will return Lead Priority
EXECUTE TC_InquiryStatusSelect

-- Calling Procedure that will return Lead Status
EXECUTE TC_InquiriesFollowupActionSelect

-- Table for User List those are having task access for Buyer
 SELECT U.Id,U.UserName from TC_Users U WITH(NOLOCK) 
 INNER JOIN TC_Roles R WITH(NOLOCK) ON U.RoleId=R.Id  
	WHERE U.IsActive=1 AND U.BranchId=@BranchId
 
 -- Table that will return all followup record for the given Customer   
SELECT FL.Comment,ST.Status,FA.InquiryType, FL.FollowUpDate,FL.NextFollowupDate,FA.ActionName
	FROM TC_InquiriesLead IL WITH(NOLOCK) 
	INNER JOIN TC_InquiriesFollowup FL WITH(NOLOCK) ON IL.TC_InquiriesLeadId=FL.TC_InquiriesLeadId
	LEFT JOIN TC_InquiryStatus ST WITH(NOLOCK) ON FL.TC_InquiryStatusId=ST.TC_InquiryStatusId
	LEFT JOIN TC_InquiriesFollowupAction FA WITH(NOLOCK) ON FL.TC_InquiriesFollowupActionId=FA.TC_InquiriesFollowupActionId
	WHERE IL.TC_CustomerId=@CustomerId AND IL.TC_InquiryTypeId=@InquiryType ORDER BY FL.TC_FollowupId DESC

SELECT IL.TC_UserId FROM TC_InquiriesLead IL WITH(NOLOCK) WHERE IL.TC_CustomerId=@CustomerId AND IL.TC_InquiryTypeId=@InquiryType
	

