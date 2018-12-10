IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[EntryUnregisteredCustomers]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[EntryUnregisteredCustomers]
GO

	--THIS PROCEDURE IS FOR INSERTING in the table UnregisteredCustomers
--RegistrationType, Email, ContactNo, Comment, RecordId, EntryDateTime
CREATE PROCEDURE [dbo].[EntryUnregisteredCustomers]
	@RegistrationType		AS SMALLINT, 
	@Email				AS VARCHAR(100),
	@ContactNo			AS VARCHAR(100),
	@Comment			AS VARCHAR(500),
	@RecordId 			AS NUMERIC,
	@EntryDateTime		AS DATETIME,
	@ID				AS NUMERIC OUTPUT
 AS
	
BEGIN
	
	
		INSERT INTO UnregisteredCustomers
			(
				RegistrationType, 	Email, 		ContactNo, 		Comment, 
				RecordId, 		EntryDateTime		
			) 
		VALUES
			(
				@RegistrationType, 	@Email, 	@ContactNo, 		@Comment, 
				@RecordId, 		@EntryDateTime		
			)

		SET @ID = SCOPE_IDENTITY()  		
		
END