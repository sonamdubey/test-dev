IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[InsertCarValues]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[InsertCarValues]
GO

	--THIS PROCEDURE IS FOR INSERTING AND UPDATING RECORDS FOR Class TABLE





CREATE PROCEDURE [dbo].[InsertCarValues]
	@Id			NUMERIC,	-- Id. Will be -1 if Its Insertion
	@GuideId		NUMERIC,
	@CarVersionId		NUMERIC,	
	@CarYear		NUMERIC,	
	@CarValue		NUMERIC,
	@STATUS		INTEGER OUTPUT	--return value, -1 for unsuccessfull attempt, and 0 for success
	
 AS
	
BEGIN
	IF @Id = -1 -- Insertion

		BEGIN
	
			----------FIRST CHECK WHETHER THERE IS ANY DISTRICT ALREADY 
			----------THERE WITH THE NAME AS PASSED IN THE PARAMETER
	
			SELECT CarValue FROM CarValues WHERE CarVersionId = @CarVersionId AND CarYear = @CarYear AND GuideId=@GuideId
	
			--IF THERE IS NONE THEN INSERT THE ENTRY AND RETURN 0 ELSE RETURN -1
			IF @@ROWCOUNT = 0 	
				BEGIN
					INSERT INTO CarValues ( GuideId, CarVersionId, CarYear, CarValue )
					 VALUES( @GuideId, @CarVersionId, @CarYear, @CarValue )
					SET @STATUS = 0
                       			END
			ELSE 
					SET @STATUS = -1
		END
	ELSE

		BEGIN
			DELETE FROM CarValues WHERE CarVersionId = @CarVersionId AND CarYear = @CarYear AND GuideId=@GuideId

			INSERT INTO CarValues ( GuideId, CarVersionId, CarYear, CarValue )
					 VALUES( @GuideId, @CarVersionId, @CarYear, @CarValue )
			SET @STATUS = 0
		END
	
					
	
END