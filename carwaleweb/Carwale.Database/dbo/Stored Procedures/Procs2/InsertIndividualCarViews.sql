IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[InsertIndividualCarViews]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[InsertIndividualCarViews]
GO
	--THIS PROCEDURE IS FOR INSERTING AND UPDATING RECORDS FOR IndividualCarViews





CREATE PROCEDURE [dbo].[InsertIndividualCarViews]
	@CityId			NUMERIC,	
	@SubmissionDate	DATETIME,	
	@TotalCars		NUMERIC,	
	@TotalViews		NUMERIC,
	@STATUS		INTEGER OUTPUT	--return value, -1 for unsuccessfull attempt, and 0 for success
	
 AS
	
BEGIN
	
	SET @STATUS = 1

	BEGIN

		INSERT INTO IndividualCarViews ( CityId, TotalCars, TotalViews, EntryDate  )
		 VALUES ( @CityId, @TotalCars, @TotalViews, @SubmissionDate  )
		SET @STATUS = 0
	END

					
	
END