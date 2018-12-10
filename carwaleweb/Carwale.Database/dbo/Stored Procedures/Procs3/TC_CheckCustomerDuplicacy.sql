IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_CheckCustomerDuplicacy]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_CheckCustomerDuplicacy]
GO

	-- Created By:	Tejashree Patil
-- Create date: 2 Aug 2013
-- Description:	Check duplicacy of customer using parameters like mobile,email for active lead.
-- EXEC TC_CheckCustomerDuplicacy 5,NULL,'3242342343'
-- =============================================
CREATE PROCEDURE [dbo].[TC_CheckCustomerDuplicacy]
@BranchId BIGINT, 
@CustomerEmail VARCHAR(100),
@CustomerMobile VARCHAR(15),
@InqType SMALLINT = 3
AS 

BEGIN
SET NOCOUNT ON;
	
	SELECT	DISTINCT Id, Email, Mobile, CustomerName, ActiveLeadId, IL.TC_UserId AS LeadOwnerId
	FROM	TC_CustomerDetails CD WITH(NOLOCK)
			INNER JOIN TC_Lead L WITH(NOLOCK) ON L.TC_CustomerId=CD.Id
			INNER JOIN TC_InquiriesLead IL WITH(NOLOCK) ON IL.TC_LeadId=L.TC_LeadId AND IL.TC_LeadInquiryTypeId=@InqType
	WHERE	(@CustomerEmail IS NULL OR Email= @CustomerEmail)
			AND (@CustomerMobile IS NULL OR Mobile= @CustomerMobile)
			AND CD.BranchId=@BranchId 
			AND CD.IsActive=1 
			AND	IsleadActive = 1
			--Add condition for closed lead also
	 
	 
END

 

