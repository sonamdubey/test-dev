IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[Con_InsertUpdateTestimonial_v16_9_1]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[Con_InsertUpdateTestimonial_v16_9_1]
GO

	-- Modified by Harshil on 28 Sept, for advantage Testimonials
CREATE  procedure [dbo].[Con_InsertUpdateTestimonial_v16_9_1]
@TID INT,
@CatId INT,
@CustomerName VARCHAR(100),
@CityId INT,
@IsActive Bit,
@Email VARCHAR(100),
@ContactNo VARCHAR(100),
@Comments VARCHAR(500),
@TestimonialDate DATETIME,
@EntryDate DATETIME,
@MakeId INT,
@ModelId INT,
@HostUrl VARCHAR(100),
@OriginalImgPath VARCHAR(250),
@IsImageUpdated bit = 0 ,
@ID NUMERIC OUTPUT 

AS
	
BEGIN
	IF @TID = -1
		BEGIN
		INSERT INTO Con_Testimonial 
		(CatId, CustomerName, CityId, Email, ContactNo, Comments, TestimonialDate, EntryDate ,MakeId,ModelId ,IsActive ,HostURL ,OriginalImgPath)
		VALUES 
		(@CatId, @CustomerName, @CityId, @Email, @ContactNo, @Comments, @TestimonialDate, @EntryDate,@MakeId,@ModelId ,@IsActive ,@HostUrl ,@OriginalImgPath)
		SET @ID = SCOPE_IDENTITY() 
		END
	ELSE
		if @IsImageUpdated = 1 
		BEGIN
		UPDATE Con_Testimonial
		SET CatId = @CatId, CustomerName=@CustomerName,
		CityId = @CityId, Email = @Email, ContactNo = @ContactNo,MakeId= @MakeId,ModelId = @ModelId,
		Comments = @Comments, TestimonialDate = @TestimonialDate,IsActive = @IsActive
		Where ID = @TID
		SET @ID = @TID 
		END
		ELSE
		BEGIN
		UPDATE Con_Testimonial
		SET CatId = @CatId, CustomerName=@CustomerName,
		CityId = @CityId, Email = @Email, ContactNo = @ContactNo,MakeId= @MakeId,ModelId = @ModelId,
		Comments = @Comments, TestimonialDate = @TestimonialDate,IsActive = @IsActive, HostURL =@HostUrl , OriginalImgPath = @OriginalImgPath
		Where ID = @TID
		SET @ID = @TID 
		END
END

