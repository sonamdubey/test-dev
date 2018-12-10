IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_DeleteExcelRecord]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_DeleteExcelRecord]
GO

	-- ================================================
-- Author:		Vivek Gupta
-- Create date: 28th May,2013
-- Description: Set IsDeleted as 1 for the requested ImportBuyerInquiesId or ImportSellerInquiriesId
-- TC_DeleteExcelRecord 1,'2,3,4,'
-- Modified By: Tejashree Patil on 19 Sept 2013, Added @UserId ,@BranchId ,@ExcelType to identify request from stock or inquiry.
-- =============================================
CREATE PROCEDURE [dbo].[TC_DeleteExcelRecord]
	@InqType INT,
	@RecordsId VARCHAR(1000),
	@UserId INT = NULL,
	@BranchId INT = NULL,
	@ExcelType SMALLINT = 1
AS
BEGIN
	SET NOCOUNT ON;
	
	DECLARE @Separator_position INT -- This is used to locate each separator character  
    DECLARE @array_value VARCHAR(1000) -- this holds each array value as it is returned  	
	
	IF(@ExcelType=1)
	BEGIN
		WHILE PATINDEX( '%,%' , @RecordsId) <> 0  
		BEGIN
		-- patindex matches the a pattern against a string  
			   SELECT  @Separator_position = PATINDEX('%,%',@RecordsId)  
			   SELECT  @array_value = LEFT(@RecordsId, @Separator_position - 1)
	           
			   IF(@InqType=1)
			   BEGIN
				 UPDATE TC_ImportBuyerInquiries SET IsDeleted = 1 WHERE TC_ImportBuyerInquiriesId=@array_value
			   END
			   ELSE IF(@InqType=2)
			   BEGIN
				 UPDATE TC_ImportSellerInquiries SET IsDeleted = 1 WHERE TC_ImportSellerInquiriesId=@array_value
			   END   
			   ELSE IF(@InqType=3)
			   BEGIN
				 UPDATE TC_ExcelInquiries SET IsDeleted = 1 WHERE Id=@array_value
			   END              
			   -- This replaces what we just processed with and empty string  
			   SELECT  @RecordsId = STUFF(@RecordsId, 1, @Separator_position, '')  	
		END -- while end
	END
	ELSE IF(@ExcelType=2)
	BEGIN
		UPDATE	TC_ExcelStockInventory 
		SET		IsDeleted = 1 , ModifiedBy = @UserId, ModifiedDate=GETDATE()
		WHERE	TC_ExcelStockInventoryId IN (SELECT ListMember AS UserId FROM fnSplitCSV(@RecordsId)) OR @RecordsId IS NULL
				AND BranchId=@BranchId 
				AND CreatedBy=@UserId
	END

END
