IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[EntryMyCarwaleServiceDetails]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[EntryMyCarwaleServiceDetails]
GO

	
--THIS PROCEDURE IS FOR INSERTING AND UPDATING RECORDS FOR mycarwaleservicedetails table
-- MyCarwaleServiceDetails : ID, CustomerId, MyCarwaleCarId, ServiceTypeId, ServiceDate, ServiceKm, ServiceAmount, Comments, IsActive

CREATE PROCEDURE [dbo].[EntryMyCarwaleServiceDetails]
	@CustomerId		NUMERIC, 
	@MyCarwaleCarId	NUMERIC, 
	@ServiceTypeId	NUMERIC, 
	@ServiceDate		DATETIME, 
	@ServiceKm		NUMERIC, 
	@ServiceAmount	DECIMAL(18,2), 
	@Comments		VARCHAR(300),
	@Workshop		VARCHAR(50),
	@BillNo			VARCHAR(50)
	
 AS
	
BEGIN
	
		--IT IS FOR THE INSERT
		INSERT INTO MyCarwaleServiceDetails
			(
				CustomerId, 	MyCarwaleCarId, 	ServiceTypeId, 		ServiceDate, 	
				ServiceKm, 	ServiceAmount, 		Comments, 		IsActive,
				Workshop,	BillNo
			)
		VALUES
			(	
				@CustomerId, 	@MyCarwaleCarId, 	@ServiceTypeId, 	@ServiceDate, 	
				@ServiceKm, 	@ServiceAmount, 	@Comments, 		1,
				@Workshop,	@BillNo
			)

		
END
