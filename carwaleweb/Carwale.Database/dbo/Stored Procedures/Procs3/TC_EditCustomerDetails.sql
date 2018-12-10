IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_EditCustomerDetails]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_EditCustomerDetails]
GO
	-- modified by-Binu, Date-26-jun-2012, Desc- condition checking changed from email to mobile.
-- Created By:	Surendra
-- Create date: 3rd April 2012
-- Description:	Edist Customer details from inq followup page

-- =============================================
CREATE PROCEDURE [dbo].[TC_EditCustomerDetails]
@BranchId NUMERIC,
@CustId BIGINT,
@Email VarChar(100), 
@Name VarChar(100),          
@Mobile VARCHAR(15),
@ModifiedBy BIGINT
AS                
	BEGIN
		IF NOT EXISTS(SELECT TOP 1 Id FROM TC_CustomerDetails WHERE Mobile=@Mobile AND BranchId=@BranchId AND Id<>@CustId )
		BEGIN 
			UPDATE TC_CustomerDetails SET CustomerName=@Name, Mobile=@Mobile, Email=@Email,
			ModifiedBy=@ModifiedBy,ModifiedDate=GETDATE()
			WHERE Id=@CustId AND BranchId=@BranchId

			UPDATE TC_TaskLists SET CustomerName=@Name, CustomerEmail=@Email, CustomerMobile=@Mobile
			WHERE CustomerId=@CustId AND BranchId=@BranchId
		END
	END
