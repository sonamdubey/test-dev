IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[Microsite_TestimonialsSave]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[Microsite_TestimonialsSave]
GO

	-- =============================================  
-- Author:  Chetan Kane
-- Create date: 
-- Description: To Save and update (Edited from managewebsite) Testimonials for the DealerWebSite 
-- Modified By: Chetan Kane on 25th July 2012
-- Description: Added a Approved parameter for the testimonial writed by the customer on the DealerWebSite
-- =============================================  
CREATE PROCEDURE [dbo].[Microsite_TestimonialsSave]   
(  
 @DealerId INT,  
 @Id INT = NULL,
 @Testimonials VARCHAR(500),  
 @CustomerName VARCHAR(50),
 @Approved BIT = 1,
 @MobileNo varchar(12)= NULL,
 @Email VARCHAR(50)= NULL
)  
AS  
BEGIN  
 	IF(@Id Is NULL)
		BEGIN  
		
			INSERT INTO Microsite_Testimonials(DealerId,Testimonials,CustomerName,IsApproved,CustomersNo,CustomersEmail) 
			VALUES(@DealerId,@Testimonials,@CustomerName,@Approved,@MobileNo,@Email)
		END  
	ELSE  
		BEGIN  
			UPDATE Microsite_Testimonials SET Testimonials=@Testimonials,CustomerName=@CustomerName,IsApproved=@Approved
			WHERE DealerId=@DealerId AND IsActive=1  AND Id=@Id 
		END  
END 

