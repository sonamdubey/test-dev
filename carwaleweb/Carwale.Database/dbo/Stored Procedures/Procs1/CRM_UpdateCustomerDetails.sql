IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[CRM_UpdateCustomerDetails]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[CRM_UpdateCustomerDetails]
GO

	
-- =============================================
-- Author      : Chetan Navin		
-- Create date : 16 Dec 2013
-- Description : To Update Customer Details
-- =============================================
CREATE PROCEDURE [dbo].[CRM_UpdateCustomerDetails]

	@CustomerId			Numeric,
	@FirstName          VARCHAR(200),
	@LastName			VARCHAR(100),
	@Email				VARCHAR(200),
	@Mobile             VARCHAR(50),
	@AlterNateContact   VARCHAR(100)
				
 AS
	
BEGIN
	IF @CustomerId <> -1
		BEGIN
			BEGIN
				UPDATE CRM_Customers SET
					FirstName = CASE WHEN @FirstName<>'' THEN @FirstName ELSE FirstName END,
					LastName = CASE WHEN @LastName<>'' THEN @LastName ELSE LastName END,
					Email = CASE WHEN @Email<>'' THEN @Email ELSE Email END,
					Mobile = CASE WHEN @Mobile<>'' THEN @Mobile ELSE Mobile END,
					Landline = CASE WHEN @AlterNateContact<>'' THEN @AlterNateContact ELSE Landline END
				WHERE Id = @CustomerId
			END
		END
END









