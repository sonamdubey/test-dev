IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[Microsite_TestimonialsApprove]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[Microsite_TestimonialsApprove]
GO

	-- =============================================  
-- Author:  Chetan Kane
-- Create date: 18th July 2012
-- Description: To approve testimonial writen by customers for the dealer 
-- =============================================  
CREATE PROCEDURE [dbo].[Microsite_TestimonialsApprove]   
(  
 @DealerId INT,  
 @Id INT
)  
AS  
DECLARE @Activation BIT

BEGIN
	SET   @Activation = (SELECT IsApproved FROM Microsite_Testimonials WHERE Id = @Id)
	 
	IF(@Activation = 0) 
	BEGIN  
		UPDATE Microsite_Testimonials SET IsApproved=1
		WHERE Id=@Id 
	END 
	 
	ELSE
	BEGIN
		UPDATE Microsite_Testimonials SET IsApproved=0
		WHERE Id=@Id 
	END
END 

