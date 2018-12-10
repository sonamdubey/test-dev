IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[UpdateNewCarPurchaseInquiry]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[UpdateNewCarPurchaseInquiry]
GO

	
--THIS PROCEDURE IS FOR UPDATING RECORDS FOR new car purchase inquiry

CREATE PROCEDURE [dbo].[UpdateNewCarPurchaseInquiry]
	@ID			NUMERIC,
	@CarVersionId		NUMERIC,	-- Car Version Id
	@NoOfCars		INT,
	@Color			VARCHAR(50),
	@Comments 		VARCHAR(2000)

 AS
	BEGIN
		UPDATE NewCarPurchaseInquiries SET
			CarVersionId	= @CarVersionId, 
			NoOfCars	= @NoOfCars,
			Color		= @Color, 
			Comments	= @Comments
		WHERE
			ID = @ID
	END
