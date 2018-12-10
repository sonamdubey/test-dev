IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[Microsite_TestimonialsView]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[Microsite_TestimonialsView]
GO

	-- =============================================  
-- Author:  Chetan
-- Create date:   
-- Description: To view testimonials on DealerWebSite and in ManageWebSite testimonial section 
-- Modified by : Chetan Kane on 25th July 2012
-- Description: Added a if else condition if its runing on the DealerWebSite only approved testimoials will be fetch 
--              and in ManageWebSite both approved and un approved will be fetched  
-- =============================================  
CREATE PROCEDURE [dbo].[Microsite_TestimonialsView]
(  
 @DealerId INT,
 @IsApproved BIT = 0
)  
AS  

BEGIN  
 -- SET NOCOUNT ON added to prevent extra result sets from  
 -- interfering with SELECT statements.  
 SET NOCOUNT ON;  
	IF(@IsApproved = 1) -- For DealerWebSite
		BEGIN
			SELECT Id, Testimonials, CustomerName,IsApproved 
			FROM Microsite_Testimonials   
			WHERE DealerId=@DealerId AND IsActive=1 AND  IsApproved = @IsApproved
		END
 
	ELSE				-- For ManageWebSite
		BEGIN
			SELECT Id, Testimonials, CustomerName,CustomersNo,CustomersEmail,IsApproved 
			FROM Microsite_Testimonials   
			WHERE DealerId=@DealerId AND IsActive=1 
		END
END 

