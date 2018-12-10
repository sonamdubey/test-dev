IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[Dealer_SaveTestimonial]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[Dealer_SaveTestimonial]
GO

	

CREATE PROCEDURE [dbo].[Dealer_SaveTestimonial]
	@Id				NUMERIC,
	@DealerID		NUMERIC,
	@UserName		VARCHAR(100),
	@Testimonial    VARCHAR(500),
	@UpdatedOn		DATETIME,
	@UpdatedBy		NUMERIC 
AS

BEGIN

	UPDATE Dealer_Testimonial 
	SET UpdatedOn = @UpdatedOn, UpdatedBy = @UpdatedBy, 
		DealerID = @DealerID, UserName = @UserName, 
		Testimonial = @Testimonial
	WHERE Id  = @Id 
	
	IF @@ROWCOUNT = 0
		BEGIN
			
			INSERT INTO Dealer_Testimonial
			(
				DealerID, UserName, 
				Testimonial, UpdatedOn, UpdatedBy
			) 
			VALUES
			( 
				@DealerID, @UserName, 
				@Testimonial, @UpdatedOn, @UpdatedBy
			)
		END
	
END
