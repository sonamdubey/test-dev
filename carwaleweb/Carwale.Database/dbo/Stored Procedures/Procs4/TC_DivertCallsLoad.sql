IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_DivertCallsLoad]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_DivertCallsLoad]
GO

	-- Modified by:		Binumon George
-- Modification date: 16 Jan 2012
-- Description:	Added @DealerTypeId param for  TC_InquiryTypeSelect proc
-- =============================================
-- Author:		Binumon George
-- Create date: 16 Jan 2012
-- Description:	This procedure will be used to bind Controls in Divert calls form during load
-- =============================================
CREATE PROCEDURE [dbo].[TC_DivertCallsLoad]
(
	@BranchId NUMERIC,
	@DealerTypeId BIGINT
)
AS
BEGIN
	
	-- Give Inquiry Status
	EXECUTE TC_InquiryStatusSelect
	
	-- Give all active lead type as Table
	EXECUTE TC_InquiryTypeSelect @DealerTypeId
		
	-- Give all Source as Table
	EXECUTE TC_UsersForInuiryAssignment @BranchId
	
	-- Give all lead status as Table	
	EXECUTE TC_InquiriesFollowupActionSelect
	
END