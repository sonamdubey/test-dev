IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_ExcelOperationStatusInsert]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_ExcelOperationStatusInsert]
GO

	-- =============================================
-- Author:		Tejashree Patil
-- Create date: 30th May,2013
-- Description:	Insert log for excel inquiries operation
-- =============================================
CREATE PROCEDURE [dbo].[TC_ExcelOperationStatusInsert]
@BranchId BIGINT,
@UserId BIGINT,
@ReqCategory VARCHAR(20)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	--TC_ReqCategoryId =2 for Unassigned Page, 3 for Invalid page request.
	INSERT INTO TC_ExcelOperationLog(TC_UserId,BranchId,TC_ReqCategory,CreatedOn,Status)
	VALUES		(@UserId,@BranchId,@ReqCategory,GETDATE(), 0)
	
END
