IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_ReassignInquiry_12Apr]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_ReassignInquiry_12Apr]
GO

	

-- Created By:	Binumon George
-- Create date: 15 Feb 2012
-- Description:	Reassignment of Inquiries
-- =============================================
CREATE PROCEDURE [dbo].[TC_ReassignInquiry_12Apr]
@BranchId NUMERIC,
@AssignedTo BIGINT,
@InqType BIGINT,
@UserId BIGINT,
@CustId BigINT
AS           

BEGIN
	DECLARE @TC_InquiriesLeadId BIGINT
	--Getting lead id based on customer and inquiry type
	SELECT @TC_InquiriesLeadId=TC_InquiriesLeadId  FROM TC_InquiriesLead 
	WHERE TC_CustomerId=@CustId AND TC_InquiryTypeId=@InqType
	
	-- Updating assignee for TC_InquiriesLeadId
	UPDATE TC_InquiriesLead SET TC_UserId=@AssignedTo,ModifiedBy=@UserId,ModifiedDate=GETDATE()
	WHERE BranchId=@BranchId AND TC_InquiriesLeadId=@TC_InquiriesLeadId	
END

