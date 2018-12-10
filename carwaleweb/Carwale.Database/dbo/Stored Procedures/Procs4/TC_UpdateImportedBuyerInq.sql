IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_UpdateImportedBuyerInq]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_UpdateImportedBuyerInq]
GO

	-- =============================================  
-- Author:  Tejashree Patil  
-- Create date: 5 March 2013  
-- Description: Update content of inquiries imported from excel sheet. 
-- Modified By : Tejashree Patil on 1 April , Removed make , model update statement 
-- =============================================  
CREATE PROCEDURE  [dbo].[TC_UpdateImportedBuyerInq]  
 -- Add the parameters for the stored procedure here  
 @Name VARCHAR(100),  
 @Mobile VARCHAR(15),  
 @Email VARCHAR(100),  
 @Location VARCHAR(50), 
 @Price VARCHAR(50),
 @CarYear VARCHAR(50), 
 @CarDetails VARCHAR(50),  
 @Comments VARCHAR(50), 
 @BranchId BIGINT,  
 @Id BIGINT,
 @ValidInqIds VARCHAR(MAX) = NULL,
 @Status TINYINT OUTPUT 
AS  
BEGIN  
	SET @Status = 0  
  
	IF(@ValidInqIds IS NULL)
	BEGIN		
		-- Insert statements for procedure here  
		UPDATE	TC_ImportBuyerInquiries  
		SET		Name=@Name,Mobile=@Mobile,Email=@Email,IsValid=NULL,Location=@Location,CarYear=@CarYear,Price=@Price,
				CarDetails=@CarDetails,Comments=@Comments
		WHERE	BranchId=@BranchId 
				AND TC_ImportBuyerInquiriesId=@Id 
		
		SET @Status = 1 
	END
    ELSE
    BEGIN
		UPDATE	TC_ImportBuyerInquiries  
		SET		IsValid=1
		WHERE	BranchId=@BranchId 
				AND TC_ImportBuyerInquiriesId IN (SELECT ListMember FROM fnSplitCSV(@ValidInqIds))
		
		SET @Status = 2 
    END
END
