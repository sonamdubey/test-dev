IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_MMCustomerDetail]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_MMCustomerDetail]
GO

	
-- =============================================
-- Author:		Ranjeet kumar
-- Create date: 31/10/2013
-- Description:	Get the customer mobile, name and email for Mix N Match in AJAX
--Table : Customers
-- =============================================
CREATE PROCEDURE [dbo].[TC_MMCustomerDetail] 
	@CustomerId int
	AS
BEGIN
	SET NOCOUNT ON;
	
	SELECT  CC.Name AS Name, CC.Mobile AS Moblie, CC.email AS Email FROM Customers	AS CC WHERE CC.Id = @CustomerId ; 
END

