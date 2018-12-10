IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[CRM_ADM_AddInvoices]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[CRM_ADM_AddInvoices]
GO

	

--THIS PROCEDURE INSERTS THE VALUES FOR THE Cities

CREATE PROCEDURE [dbo].[CRM_ADM_AddInvoices]
	@ID				NUMERIC,
	@MakeId			NUMERIC,
	@InvoiceNo		VARCHAR(50),
	@Date			DATETIME,
	@Status			BIT OUTPUT
	
 AS
	
BEGIN
	IF @ID = -1
		BEGIN
			SELECT Id FROM CRM_ADM_Invoices WITH (NOLOCK) WHERE InvoiceNo = @InvoiceNo

			IF @@ROWCOUNT = 0
				BEGIN
					INSERT INTO CRM_ADM_Invoices(MakeId, InvoiceNo,InvoiceMonth)			
					Values(@MakeId, @InvoiceNo,@Date)	

					SET @Status = 1
				END
			ELSE
				SET @Status = 0
		END
	ELSE
		BEGIN
			SELECT Id FROM CRM_ADM_Invoices WITH (NOLOCK) WHERE InvoiceNo = @InvoiceNo AND Id <> @Id

			IF @@ROWCOUNT = 0
				BEGIN
					UPDATE CRM_ADM_Invoices SET MakeId = @MakeId, InvoiceNo=@InvoiceNo,InvoiceMonth=@Date			
					WHERE Id = @Id	
					
					SET @Status = 1
				END
			ELSE
				SET @Status = 0
		END
END

