IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_ReassignInquiry]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_ReassignInquiry]
GO

	-- Created By:    Binumon George
-- Create date: 15 Feb 2012
-- Description:    Reassignment of Inquiries
-- =============================================
CREATE PROCEDURE [dbo].[TC_ReassignInquiry]
@BranchId NUMERIC,
@AssignedTo BIGINT,
@LeadId BIGINT,
@UserId BIGINT
AS           

BEGIN
    -- Updating assignee for TC_InquiriesLeadId
    UPDATE TC_InquiriesLead SET TC_UserId=@AssignedTo,ModifiedBy=@UserId,ModifiedDate=GETDATE()
    WHERE BranchId=@BranchId AND TC_InquiriesLeadId=@LeadId    
END


SET ANSI_NULLS ON
