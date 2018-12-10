IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[InsertDealerCarViews]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[InsertDealerCarViews]
GO

	


--THIS PROCEDURE IS FOR INSERTING AND UPDATING RECORDS FOR DelaerCarVIews





CREATE PROCEDURE [dbo].[InsertDealerCarViews]
	@DealerId		NUMERIC,	
	@SubmissionDate	DATETIME,	
	@TotalCars		NUMERIC,	
	@TotalViews		NUMERIC,
	@STATUS		INTEGER OUTPUT	--return value, -1 for unsuccessfull attempt, and 0 for success
	
 AS
	
BEGIN
	
	SET @STATUS = 1

	BEGIN

		INSERT INTO DealerCarViews ( DealerId, TotalCars, TotalViews, EntryDate  )
		 VALUES ( @DealerId, @TotalCars, @TotalViews, @SubmissionDate  )
		SET @STATUS = 0
	END

					
	
END
