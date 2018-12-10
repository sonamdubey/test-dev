IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[EntryMyCarwaleCarsExpiries]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[EntryMyCarwaleCarsExpiries]
GO

	--THIS PROCEDURE IS FOR INSERTING AND UPDATING RECORDS FOR mycarwalecars table
--ID, CustomerId, VersionId, MakeYear, StartingKm, CurrentKm, PurchaseDate, RegistrationNo, ChasisNo, EngineNo, IsActive

CREATE PROCEDURE [dbo].[EntryMyCarwaleCarsExpiries]
	@MyCarwaleCarId		NUMERIC,
	@InsuranceExpiryDate 		DATETIME,
	@WarrantyExpiryDate 		DATETIME,
	@PUCExpiryDate 		DATETIME,
	@ServiceReminderDate 		DATETIME
 AS
	
BEGIN
	UPDATE	 MyCarwaleCars 
	SET		 InsuranceExpiryDate = @InsuranceExpiryDate, WarrantyExpiryDate = @WarrantyExpiryDate, 
			PUCExpiryDate = @PUCExpiryDate, ServiceReminderDate = @ServiceReminderDate
	WHERE	Id = @MyCarwaleCarId
END
