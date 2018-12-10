IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[CMS_UpdatePaymentDetails]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[CMS_UpdatePaymentDetails]
GO

	
CREATE PROCEDURE [dbo].[CMS_UpdatePaymentDetails]
	@Id			BIGINT,
	@ReceivedAmount	DECIMAL,
	@ReceivedOn		DATETIME,
	@ChequeDDNo		VARCHAR(100),
	@ChequeDDDate	DATETIME,
	@Comments                    VARCHAR(2000),
	@Status		INT OUTPUT		
 AS
	
BEGIN
	SET @Status = 0

	IF @Id <> -1
			
		BEGIN
			UPDATE  CMS_Campaigns SET ReceivedAmount = @ReceivedAmount,  ReceivedOn = @ReceivedOn, 
			ChequeDDNo =  @ChequeDDNo, ChequeDDDate = @ChequeDDDate, Comments = @Comments WHERE ID = @Id
		
			SET @Status = 1 
		END
END
