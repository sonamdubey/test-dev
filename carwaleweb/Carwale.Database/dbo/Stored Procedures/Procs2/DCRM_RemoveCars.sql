IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[DCRM_RemoveCars]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[DCRM_RemoveCars]
GO

	
CREATE PROCEDURE [dbo].[DCRM_RemoveCars]
	@InquiryId			NUMERIC,
	@Reason				NUMERIC,
	@SoldMedium		    NUMERIC = NULL,
	@SoldPrice		    NUMERIC = NULL,
	@Soldto				NUMERIC = NULL,
	@RemovedBy 			NUMERIC,
	@Comments			VARCHAR(500),
	@Status				INT OUTPUT	
	
	
AS
BEGIN
	
	INSERT INTO DCRM_RemovedCars( InquiryId, Reason, SoldMedium, SoldPrice, Soldto, RemovalDate, RemovedBy, Comments ) 
	VALUES( @InquiryId, @Reason, @SoldMedium, @SoldPrice, @Soldto, GETDATE(), @RemovedBy, @Comments )
	
	UPDATE SellInquiries SET StatusId= 2, LastUpdated = GETDATE()
	WHERE ID = @InquiryId
	
	SET @Status =1		
END



