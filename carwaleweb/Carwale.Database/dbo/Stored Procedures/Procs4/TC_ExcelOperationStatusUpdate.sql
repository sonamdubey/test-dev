IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_ExcelOperationStatusUpdate]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_ExcelOperationStatusUpdate]
GO

	-- =============================================
-- Author:		Vivek Gupta
-- Create date: 30th May,2013
-- Description:	Updates The Status as 1 after showing the excel inquiry count
-- =============================================
CREATE PROCEDURE [dbo].[TC_ExcelOperationStatusUpdate]
@BranchId BIGINT,
@UserId BIGINT,
@CountU INT OUTPUT,
@CountI INT OUTPUT,
@ReqCategory VARCHAR(10),
@isClicked TINYINT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	IF(@isClicked = 0)
		BEGIN
			IF EXISTS(
						SELECT Top 1 *
						FROM TC_ExcelOperationLog WITH(NOLOCK)
						WHERE BranchId = @BranchId
						AND TC_UserId = @UserId
						AND Status = 0
						AND TC_ReqCategory = 'Unassigned')
				BEGIN
				   SET @CountU = 1
				END
			ELSE
				BEGIN
				   SET @CountU = 0
				END
				
			IF EXISTS(
						SELECT Top 1 *
						FROM TC_ExcelOperationLog WITH(NOLOCK)
						WHERE BranchId = @BranchId
						AND TC_UserId = @UserId
						AND Status = 0
						AND TC_ReqCategory = 'Invalid')
	
				BEGIN
					SET @CountI = 1
				END
			ELSE
				BEGIN
				    SET @CountI = 0
				END
		END
		
	ELSE IF (@isClicked = 1)
	BEGIN
	    DELETE
		FROM TC_ExcelOperationLog
		WHERE BranchId = @BranchId
		AND TC_UserId = @UserId
		AND Status = 0
	    AND TC_ReqCategory = @ReqCategory
	    
	    SET @CountU = 0
	    SET @CountI = 0
	END
		
END
