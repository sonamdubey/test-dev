IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_InquiriesFormOnLoad]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_InquiriesFormOnLoad]
GO

	

-- Modified by:		Binumon George
-- Create date: 09 Jul 2012
-- Description:	removed TC_GetCarMake. because now using class in front end
-- ============================================================================================
-- Author:		Binumon George
-- Create date: 2 Apr 2012
-- Description:	Added one more parameter in for TC_InquiryTypeSelect proc
-- ============================================================================================
-- Author:		Surendra
-- Create date: 12 Jan 2012
-- Description:	This procedure will be used to bind Controls in Search fields in Inquiries Form
-- =============================================
CREATE PROCEDURE [dbo].[TC_InquiriesFormOnLoad]
(
@BranchId BIGINT,
@DealerTypeId BIGINT
)
AS
BEGIN	
	-- Give all Make as Table
	--EXECUTE TC_GetCarMake	-- Commented by Binumon George  09 Jul 2012
		
	-- Give Inquiry type table
	EXECUTE TC_InquiryTypeSelect @DealerTypeId
	
	-- Give Inquiry followup actions table
	EXECUTE TC_InquiriesFollowupActionSelect	
	
	-- Give all Status table
	EXECUTE TC_InquiryStatusSelect
	
	-- Give all Source as Table
	EXECUTE TC_InquirySourceSelect	
	
	-- Get User list
	--EXECUTE TC_GetAssignee @BranchId
	EXECUTE TC_UsersForInuiryAssignment @BranchId
	
END


