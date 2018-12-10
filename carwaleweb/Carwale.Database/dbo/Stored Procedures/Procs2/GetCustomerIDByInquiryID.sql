IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[GetCustomerIDByInquiryID]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[GetCustomerIDByInquiryID]
GO

	-- =============================================
-- Author:		Amit Verma
-- Create date: 07/05/2013
-- Description:	Get CustomerID By InquiryID
-- =============================================	
/*
	declare @InquiryID varchar(50) = 1789
	declare @CustomerID BIGINT
	exec GetCustomerIDByInquiryID @InquiryID,@CustomerID out
	SELECT @CustomerID
*/
CREATE PROCEDURE [dbo].[GetCustomerIDByInquiryID] 
	@InquiryID varchar(50),
	@CustomerID BIGINT OUT
AS
BEGIN
	SET NOCOUNT ON;
	SELECT @CustomerID = CustomerId 
	FROM CustomerSellInquiries WITH(NOLOCK)
	WHERE ID = @InquiryID
	--SELECT @CustomerID
END
