IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[InsertSummaryDetails]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[InsertSummaryDetails]
GO

	



--THIS PROCEDURE IS FOR INSERTING AND UPDATING RECORDS FOR SummaryDetails TABLE

CREATE PROCEDURE [InsertSummaryDetails]
	@SummaryHeadId		NUMERIC,	-- Sell Inquiry Id
	@SummaryValue		NUMERIC,	-- customer ID
	@MonthYear			DATETIME,
	@SummaryOfMonth		DATETIME,	-- Entry Date
	@IsOriginalValue		VARCHAR,
	@Status			NUMERIC OUTPUT

 AS
	BEGIN
		INSERT INTO SummaryDetails(SummaryHeadId,SummaryValue, MonthYear, SummaryOfMonth, isOriginal)
		VALUES (@SummaryHeadId,@SummaryValue, @MonthYear, @SummaryOfMonth, @IsOriginalValue) 
	END

	SET @Status = SCOPE_IDENTITY()
