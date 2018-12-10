IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[CMS_UpdateInvoiceDetails]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[CMS_UpdateInvoiceDetails]
GO

	

CREATE PROCEDURE [dbo].[CMS_UpdateInvoiceDetails]
	@Id			BIGINT,
	@InvoiceNo		VARCHAR(25), 
	@InvoiceAmount	DECIMAL,
	@InvoiceDate		DATETIME,
	@InvoiceFilePath	VARCHAR(100),
	@Comments		VARCHAR(2000),
	@Status		INT OUTPUT		
 AS
	
BEGIN
	SET @Status = 0

	IF @Id <> -1
			
		BEGIN
			UPDATE  CMS_Campaigns SET InvoiceNo = @InvoiceNo,  InvoiceAmount = @InvoiceAmount, InvoiceDate =  @InvoiceDate,
			 InvoiceFilePath = @InvoiceFilePath, Comments = @Comments  WHERE ID = @Id
		
			SET @Status = 1 
		END
END
