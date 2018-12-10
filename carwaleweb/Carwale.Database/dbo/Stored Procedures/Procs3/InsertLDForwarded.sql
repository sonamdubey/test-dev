IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[InsertLDForwarded]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[InsertLDForwarded]
GO

	--THIS PROCEDURE IS FOR INSERTING records to the LDForwarded Table

CREATE PROCEDURE [dbo].[InsertLDForwarded]
	@LeadType		AS SMALLINT, 
	@ForwardedTo		AS INT, 
	@CustomerName	AS VARCHAR(200), 
	@CustomerEmail	AS VARCHAR(100), 
	@LandLineNo		AS VARCHAR(50),  
	@MobileNo		AS VARCHAR(50),  
	@CityId			AS NUMERIC, 
	@RecordId		AS NUMERIC, 
	@ForwardCount		AS INT,
	@ForwardedDate	AS DATETIME,
	@SourceId		AS SMALLINT,
	@Id			NUMERIC OUTPUT
	
 AS
	
BEGIN
	
	SELECT ID FROM LDForwarded WHERE ForwardedTo = @ForwardedTo AND MobileNo = @MobileNo 
	IF @@ROWCOUNT = 0
		BEGIN
	
			INSERT INTO LDForwarded 
				(
					LeadType, 		ForwardedTo, 		CustomerName, 		CustomerEmail, 		LandLineNo, 		MobileNo, 		CityId, 			
					RecordId, 		ForwardCount,		InternalStatus, 		ADFStatus, 		ForwardedDate,		SourceId
				)	
			VALUES
				(
					@LeadType, 		@ForwardedTo, 	@CustomerName, 	@CustomerEmail, 	@LandLineNo, 		@MobileNo, 		@CityId, 
					@RecordId, 		@ForwardCount,	-1, 			-1, 			@ForwardedDate,	@SourceId	
				)
			
			SET  @Id = SCOPE_IDENTITY()
		END
END

