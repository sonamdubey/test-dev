IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[CRM_SaveCarInvoiceData]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[CRM_SaveCarInvoiceData]
GO

	

CREATE PROCEDURE [dbo].[CRM_SaveCarInvoiceData]

	@CBDId				Numeric,
	@InvoiceId			Numeric,
	@UpdatedBy			Numeric	
				
 AS
	
BEGIN
		UPDATE CRM_CarInvoices SET InvoiceId = @InvoiceId, UpdatedBy = @UpdatedBy, UpdatedOn = GETDATE()
		WHERE CBDId = @CBDId
		
		IF @@ROWCOUNT = 0
			BEGIN
				INSERT INTO CRM_CarInvoices(CBDId, InvoiceId, UpdatedBy, UpdatedOn)
				VALUES(@CBDId, @InvoiceId, @UpdatedBy, GETDATE()) 
			END
END

