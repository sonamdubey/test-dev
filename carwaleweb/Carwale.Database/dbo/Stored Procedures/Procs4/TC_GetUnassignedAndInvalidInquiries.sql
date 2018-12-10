IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_GetUnassignedAndInvalidInquiries]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_GetUnassignedAndInvalidInquiries]
GO

	-- =============================================
-- Author:		Afrose
-- Create date: 10th November 2015
-- Description:	Get invalid and unassigned inquiries to show on top navigation in one go instead of calling it three times
-- EXEC TC_GetUnassignedAndInvalidInquiries 243,5,NULL,NULL,NULL,NULL,NULL,NULL,NULL
-- =============================================
CREATE PROCEDURE [dbo].[TC_GetUnassignedAndInvalidInquiries]
	@UserId    INT,
	@BranchId  INT,
	@InquiryType TINYINT=NULL,
	@ReqFromUnassignedPage BIT=NULL,
	@IsSpecialUser BIT=NULL,
	@FromIndex  INT=NULL, 
	@ToIndex  INT=NULL,
	@FromDate DATETIME=NULL,
	@ToDate   DATETIME=NULL
AS
BEGIN
	
	SET NOCOUNT ON;
	
	DECLARE @DealerType AS TINYINT
	
	SELECT @DealerType = D.TC_DealerTypeId FROM Dealers D WITH (NOLOCK) WHERE ID = @BranchId
    
    IF @DealerType = 1
		BEGIN
			EXEC TC_ExcelUnassignedInquiriesLoad @UserId,@BranchId,1,@ReqFromUnassignedPage,@IsSpecialUser,@FromIndex,@ToIndex
			EXEC TC_ExcelUnassignedInquiriesLoad @UserId,@BranchId,2,@ReqFromUnassignedPage,@IsSpecialUser,@FromIndex,@ToIndex
			SELECT 0 AS RecordCount
			SELECT 0 AS RecordCount
			EXEC TC_UnassigendLeadAndEnquiryReport @BranchId,1,@UserId,@ReqFromUnassignedPage,@FromDate,@ToDate,@FromIndex,@ToIndex
			EXEC TC_UnassigendLeadAndEnquiryReport @BranchId,2,@UserId,@ReqFromUnassignedPage,@FromDate,@ToDate,@FromIndex,@ToIndex
			SELECT 0 AS RecordCount
			SELECT 0 AS RecordCount
		END
		
	ELSE IF @DealerType = 2
		BEGIN
			SELECT 0 AS RecordCount
			SELECT 0 AS RecordCount
			SELECT 0 AS RecordCount
			SELECT 0 AS RecordCount
			EXEC TC_ExcelUnassignedInquiriesLoad @UserId,@BranchId,3,@ReqFromUnassignedPage,@IsSpecialUser,@FromIndex,@ToIndex
			SELECT 0 AS RecordCount
			SELECT 0 AS RecordCount
			SELECT 0 AS RecordCount
			SELECT 0 AS RecordCount
			EXEC TC_UnassigendLeadAndEnquiryReport @BranchId,3,@UserId,@ReqFromUnassignedPage,@FromDate,@ToDate,@FromIndex,@ToIndex
			
		END
	ELSE IF @DealerType = 3
		BEGIN
	
			EXEC TC_ExcelUnassignedInquiriesLoad @UserId,@BranchId,1,@ReqFromUnassignedPage,@IsSpecialUser,@FromIndex,@ToIndex
			EXEC TC_ExcelUnassignedInquiriesLoad @UserId,@BranchId,2,@ReqFromUnassignedPage,@IsSpecialUser,@FromIndex,@ToIndex
			EXEC TC_ExcelUnassignedInquiriesLoad @UserId,@BranchId,3,@ReqFromUnassignedPage,@IsSpecialUser,@FromIndex,@ToIndex
			EXEC TC_UnassigendLeadAndEnquiryReport @BranchId,1,@UserId,@ReqFromUnassignedPage,@FromDate,@ToDate,@FromIndex,@ToIndex
			EXEC TC_UnassigendLeadAndEnquiryReport @BranchId,2,@UserId,@ReqFromUnassignedPage,@FromDate,@ToDate,@FromIndex,@ToIndex
			EXEC TC_UnassigendLeadAndEnquiryReport @BranchId,3,@UserId,@ReqFromUnassignedPage,@FromDate,@ToDate,@FromIndex,@ToIndex
		END
END
