IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[MapUnregisteredCustomers]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[MapUnregisteredCustomers]
GO

	
--THIS PROCEDURE IS FOR completing the unregistered customers

CREATE    PROCEDURE [dbo].[MapUnregisteredCustomers]
	@URID			NUMERIC,	-- id of the table unregistered
	@CustomerId		NUMERIC	-- id of the customer which is to be verified

 AS
	DECLARE
		@RegistrationType	AS SMALLINT, 
		@RecordId		AS NUMERIC
BEGIN
	SELECT @RegistrationType = RegistrationType, @RecordId = RecordId FROM UnregisteredCustomers WHERE ID = @URID

	IF @RegistrationType = 1	--buyer for individual car
	BEGIN
		UPDATE ClassifiedRequests SET CustomerId = @CustomerId WHERE ID = @RecordId
	END

	IF @RegistrationType = 2	--buyer for dealer car
	BEGIN
		UPDATE UsedCarPurchaseInquiries SET CustomerId = @CustomerId WHERE ID = @RecordId
	END

	IF @RegistrationType = 3	--customer for reviews
	BEGIN
		UPDATE CustomerReviews SET CustomerId = @CustomerId WHERE ID = @RecordId
	END

	IF @RegistrationType = 4	--customer for forums
	BEGIN
		UPDATE Forums SET CustomerId = @CustomerId WHERE ID = @RecordId
		UPDATE ForumThreads SET CustomerId = @CustomerId WHERE ID IN ( SELECT TOP 1 ID FROM ForumThreads WHERE ForumID = @RecordId ORDER BY ID)     
	END

	IF @RegistrationType = 5	--NewDealerShowroom
	BEGIN
		UPDATE NewCarShowroomInquiries SET CustomerId = @CustomerId WHERE ID = @RecordId
	END	

	IF @RegistrationType = 6	--NewDealerShowroom
	BEGIN
		UPDATE DealerReviews SET CustomerId = @CustomerId WHERE ID = @RecordId
	END	

	IF @RegistrationType = 7	--UsedCarAlerts
	BEGIN
		UPDATE UsedCarAlerts SET CustomerId = @CustomerId WHERE ID = @RecordId
	END	
	
	IF @RegistrationType = 9	--customer for forums (reply)
	BEGIN
		UPDATE ForumThreads SET CustomerId = @CustomerId WHERE ID = @RecordId
	END

	IF @RegistrationType = 10	-- customer for Answers (Question)
	BEGIN
		UPDATE CAQuestions SET CustomerId = @CustomerId WHERE ID = @RecordId
	END

	IF @RegistrationType = 11	-- customer for review comment
	BEGIN
		UPDATE CustomerReviewComments SET CustomerId = @CustomerId WHERE ID = @RecordId
	END

	--now deletw this record
	DELETE FROM UnregisteredCustomers WHERE ID = @URID
END