IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_ProcessedExcelReport]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_ProcessedExcelReport]
GO

	-- Created By:	Tejashree Patil
-- Create date: 18 May 2013
-- Description:	Get report from excel sheet from log table after proccessing done.
-- Modified By:	Tejashree Patil on 12 July 2013, Fetched DivertedInquiries,AssignedInquiries.
-- Modified By:	Tejashree Patil on 7 Feb 2014, Added condition of BranchId IS NULL.
-- =============================================
CREATE  PROCEDURE       [dbo].[TC_ProcessedExcelReport]
( 
@BranchId BIGINT = NULL,
@UserId BIGINT,
@ExcelSheetId BIGINT
)		
AS 	
BEGIN

	SET NOCOUNT ON;
	
	--inserting record with inactive status,later once image will save in appropriate folder need to activate
	SELECT	NewInquiries,OldInquiries,TotalInquiries,InvalidInquiries,UnassignedInquiries,IsProperExcel,StatusId, DirPath+FileName AS 'FilePath',
			DivertedInquiries, AssignedInquiries , DivertedLeadIds
	FROM	TC_ExelSheetLog
	WHERE	(@BranchId IS NULL OR DealerId=@BranchId) 
			AND UserId=@UserId --IsDeleted=1 AND StatusId=1 AND IsProperExcel=1
			AND Id=@ExcelSheetId
		
END

SET ANSI_NULLS ON
