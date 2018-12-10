IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[M_GetInvoiceStatus]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[M_GetInvoiceStatus]
GO

	
-- =============================================
-- Author	:	Sachin Bharti(14th April 2015)
-- Description	:	Get all invoice status
-- =============================================
CREATE PROCEDURE [dbo].[M_GetInvoiceStatus]
	
AS
BEGIN
	
	-- Insert statements for procedure here
	 SELECT ID, NAME  FROM M_InvoiceStatus WITH (NOLOCK) ORDER BY Id
END

