IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[EntryDealerReviews]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[EntryDealerReviews]
GO

	
--THIS PROCEDURE IS FOR entry of dealers reviews
--DealersReviews(Table Name)


CREATE PROCEDURE [dbo].[EntryDealerReviews]
	@CustomerId		NUMERIC, 
	@DealerId		NUMERIC, 
	@ProductRating		SMALLINT, 
	@FinancePlans		SMALLINT, 
	@ServiceAndSupport	SMALLINT, 
	@StaffCourtesy		SMALLINT, 
	@Timeliness		SMALLINT, 
	@Pros			VARCHAR(100), 
	@Cons			VARCHAR(100), 
	@Comments		VARCHAR(4000),
	@Title			VARCHAR(100), 
	@EntryDateTime	DATETIME,
	@ID			NUMERIC OUTPUT
	
 AS
	
BEGIN
	
	--IT IS FOR THE INSERT
	
	INSERT INTO DealerReviews
		(
			CustomerId, 		DealerId, 		ProductRating, 		FinancePlans, 		ServiceAndSupport, 
			StaffCourtesy, 		Timeliness, 		Pros, 			Cons, 			Comments, 		
			Title, 			EntryDateTime, 		Liked,			Disliked,		Viewed
		)
		VALUES
		(	
			@CustomerId, 		@DealerId, 		@ProductRating, 	@FinancePlans, 	@ServiceAndSupport, 
			@StaffCourtesy, 	@Timeliness	, 	@Pros	, 		@Cons	, 		@Comments, 
			@Title, 			@EntryDateTime,	0, 			0, 			0
					
		)
	
	SET @ID = SCOPE_IDENTITY()
		
END
