IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[UpdateNewCarPriceQuoteInquiry]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[UpdateNewCarPriceQuoteInquiry]
GO

	
--THIS PROCEDURE IS FOR UPDATING RECORDS FOR new car purchase inquiry

CREATE PROCEDURE [dbo].[UpdateNewCarPriceQuoteInquiry]
	@ID			NUMERIC,
	@CarVersionId		NUMERIC

 AS
	BEGIN
		UPDATE NewCarPriceQuoteRequests SET
			CarVersionId	= @CarVersionId 
		WHERE
			ID = @ID
	END
