IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[Con_InsertUpdateTestimonial]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[Con_InsertUpdateTestimonial]
GO

	CREATE procedure [dbo].[Con_InsertUpdateTestimonial]
@TID NUMERIC,
@CatId NUMERIC,
@CustomerName VARCHAR(100),
@CityId NUMERIC,
@Email VARCHAR(100),
@ContactNo VARCHAR(100),
@Comments VARCHAR(500),
@TestimonialDate DATETIME,
@EntryDate DATETIME,
@ID NUMERIC OUTPUT 

AS
	
BEGIN
	IF @TID = -1
		BEGIN
		INSERT INTO Con_Testimonial 
		(CatId, CustomerName, CityId, Email, ContactNo, Comments, TestimonialDate, EntryDate)
		VALUES 
		(@CatId, @CustomerName, @CityId, @Email, @ContactNo, @Comments, @TestimonialDate, @EntryDate)
		SET @ID = SCOPE_IDENTITY() 
		END
	ELSE
		BEGIN
		UPDATE Con_Testimonial
		SET CatId = @CatId, CustomerName=@CustomerName,
		CityId = @CityId, Email = @Email, ContactNo = @ContactNo,
		Comments = @Comments, TestimonialDate = @TestimonialDate
		Where ID = @TID
		SET @ID = @TID 
		END
	
END