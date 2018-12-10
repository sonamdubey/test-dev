IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[InsertMessageToDealers]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[InsertMessageToDealers]
GO
	--THIS PROCEDURE IS FOR INSERTING AND UPDATING RECORDS FOR SellInquiries TABLE

CREATE PROCEDURE [dbo].[InsertMessageToDealers]

	@DealerId		NUMERIC,		-- Car RegistrationNo 
	@Message		VARCHAR(1000),	-- Entry Date
	@EntryDate		DATETIME

 AS
	BEGIN
			INSERT INTO MessageToDealers( DealerId, Message, EntryDate) 
			VALUES(@DealerId, @Message, @EntryDate)

	END