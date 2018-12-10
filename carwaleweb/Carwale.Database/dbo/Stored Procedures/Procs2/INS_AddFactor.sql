IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[INS_AddFactor]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[INS_AddFactor]
GO

	CREATE PROCEDURE [dbo].[INS_AddFactor]
	@MakeId           NUMERIC, 
	@A1		DECIMAL(9,3),
	@A2		DECIMAL(9,3),
	@A3		DECIMAL(9,3),
	@B1		DECIMAL(9,3),
	@B2		DECIMAL(9,3),
	@B3		DECIMAL(9,3),
	@Status	INT OUTPUT		
 AS
	
BEGIN
	SET @Status = 0

	BEGIN

		SELECT MakeId FROM InsuranceFactors WHERE MakeId = @MakeId
		
		IF @@RowCount = 0
			BEGIN
				INSERT INTO InsuranceFactors( MakeId, A1, A2,. A3, B1, B2, B3 ) 
				VALUES( @MakeId, @A1, @A2, @A3, @B1, @B2, @B3 )
	
				SET @Status = 1 
			END
		ELSE
			BEGIN
			
				UPDATE  InsuranceFactors SET  A1 = @A1, A2  = @A2, A3 = @A3, B1 = @B1, B2 = @B2, B3 = @B3
				 WHERE MakeId = @MakeId	
				
			END
	END
END