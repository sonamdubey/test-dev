IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_FetchCustomerDetails]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_FetchCustomerDetails]
GO

	-- =============================================
-- Author:		<Khushaboo Patil>
-- Create date: <4/08/2016>
-- Description:	<Fetch customer details>
-- =============================================
create PROCEDURE [dbo].[TC_FetchCustomerDetails] 
	@CustomerId INT
AS
BEGIN
	SELECT C.Salutation,C.CustomerName,C.LastName,C.Email,C.Mobile,C.Address
	FROM TC_CustomerDetails C WITH (NOLOCK)
	WHERE Id = @CustomerId;
END

