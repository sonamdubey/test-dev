IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_GetUnassignedExcelInqDetails]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_GetUnassignedExcelInqDetails]
GO

	
-- =============================================
-- Author:		Tejashree Patil
-- Create date: 15 May,2013
-- Description:	This Proc Returns unassigned inquiries details depend on type imported from excel.
-- TC_GetUnassignedExcelInqDetails 3,1028,385
-- Modified By: Tejashree Patil on 24 July 2013, Fetched TestDriveDate.
-- =============================================
CREATE PROCEDURE [dbo].[TC_GetUnassignedExcelInqDetails]
	@InquiryType TINYINT,
	@BranchId  BIGINT,
	@ExcelInquiryId BIGINT
AS
BEGIN
	IF(@InquiryType=3)--New Car
	BEGIN
		
		SELECT	E.Id, 
				E.InquiryDate,
				E.NextFollowUpDate,
				E.RecentCommentDate,
				E.RecentComment,
				E.ActivityFeed,
				E.ExcelSheetId,
				E.TestDriveDate -- Modified By: Tejashree Patil on 24 July 2013, Fetched TestDriveDate.
		 FROM	TC_ExcelInquiries E WITH(NOLOCK)
		 WHERE  E.BranchId=@BranchId
				AND E.Id=@ExcelInquiryId 
				          
	END	
	
END

