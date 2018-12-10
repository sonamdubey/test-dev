IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[OnlineInsuranceLeads_Insert]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[OnlineInsuranceLeads_Insert]
GO

	
CREATE PROCEDURE [dbo].[OnlineInsuranceLeads_Insert]
	@MyCarwaleCarId		NUMERIC,
	@IsNew			Bit,
	@ClientIPAddress		VARCHAR(50),
	@Status			NUMERIC OUTPUT	--id of the inquiry just submitted
 AS
	BEGIN
		BEGIN
			INSERT INTO OnlineInsuranceLeads(MyCarwaleCarId, IsNew, ClientIPAddress)
			VALUES( @MyCarwaleCarId, @IsNew, @ClientIPAddress)
		END
		SET @Status =  SCOPE_IDENTITY()  
	END