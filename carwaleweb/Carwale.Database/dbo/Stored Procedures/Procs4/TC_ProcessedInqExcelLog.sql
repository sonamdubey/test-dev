IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_ProcessedInqExcelLog]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_ProcessedInqExcelLog]
GO

	-- Created By:	Tejashree Patil
-- Create date: 18 May 2013
-- Description:	Update status of excel sheet in log table after proccessing done.
-- Modified By:	Tejashree Patil on 12 July 2013, Added parameter DivertedInquiries,AssignedInquiries.
-- Modified By:	Tejashree Patil on 7 Feb 2014, Added condition of @BranchId IS NULL OR DealerId=@BranchId instead of DealerId=@BranchId.
-- =============================================
CREATE  PROCEDURE       [dbo].[TC_ProcessedInqExcelLog]
( 
@BranchId BIGINT,
@UserId BIGINT,
@ExcelSheetId BIGINT,
@NewInquiries INT,
@OldInquiries INT,
@TotalInquiries INT,
@InvalidInquiries INT,
@UnassignedInquiries INT,
@DivertedInquiries INT,
@AssignedInquiries INT,
@DivertedLeadIds VARCHAR(1000)=NULL,
@IsProperExcelFormat BIT
)		
AS 	
BEGIN

	SET NOCOUNT ON;
	
	SET @UnassignedInquiries=ABS(@UnassignedInquiries-@InvalidInquiries)
	
	IF(@IsProperExcelFormat = 1)--Valid format so done with task
	BEGIN
		--inserting record with inactive status,later once image will save in appropriate folder need to activate
		UPDATE	TC_ExelSheetLog    
		SET		IsDeleted=1,StatusId=1,LastUpdatedDate=GETDATE(),NewInquiries=@NewInquiries,IsProperExcel=1,UnassignedInquiries=@UnassignedInquiries,
				OldInquiries=@OldInquiries,TotalInquiries=@TotalInquiries,InvalidInquiries=@InvalidInquiries, DivertedInquiries=@DivertedInquiries,
				AssignedInquiries=@AssignedInquiries, DivertedLeadIds=@DivertedLeadIds
		WHERE	(@BranchId IS NULL OR DealerId=@BranchId) AND UserId=@UserId AND IsDeleted=0 AND StatusId=0
				AND Id=@ExcelSheetId			
	END
	ELSE --If file format(columns) is not as per inquiry source selected
	BEGIN
		--inserting record with inactive status,later once image will save in appropriate folder need to activate
		UPDATE	TC_ExelSheetLog    
		SET		IsDeleted=1,StatusId=0,LastUpdatedDate=GETDATE(),NewInquiries=@NewInquiries,IsProperExcel=0,UnassignedInquiries=@UnassignedInquiries,
				OldInquiries=@OldInquiries,TotalInquiries=@TotalInquiries,InvalidInquiries=@InvalidInquiries, DivertedInquiries=@DivertedInquiries,
				AssignedInquiries=@AssignedInquiries, DivertedLeadIds=@DivertedLeadIds
		WHERE	(@BranchId IS NULL OR DealerId=@BranchId) AND UserId=@UserId AND IsDeleted=0 AND StatusId=0
				AND Id=@ExcelSheetId	
	END
	
			

END


SET ANSI_NULLS ON
