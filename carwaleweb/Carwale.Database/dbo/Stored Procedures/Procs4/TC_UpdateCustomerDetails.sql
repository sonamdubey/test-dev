IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_UpdateCustomerDetails]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_UpdateCustomerDetails]
GO

	-- =============================================
-- Author:	Nilima More	
-- Create date: 11th July 2016
-- Description:	update customer address.
-- =============================================
create PROCEDURE [dbo].[TC_UpdateCustomerDetails] 
	@CustomerId INT,
	@Address VARCHAR(200) = NULL
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	--SET NOCOUNT ON;
	UPDATE TC_CustomerDetails 
	SET Address = @Address
	WHERE Id = @CustomerId
END
