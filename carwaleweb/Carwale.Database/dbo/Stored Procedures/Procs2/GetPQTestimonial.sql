IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[GetPQTestimonial]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[GetPQTestimonial]
GO

	-- =============================================
-- Author:		Vikas
-- Create date: 16/8/2012
-- Description:	Get a random Testimonial from the last 10 entries in Testimonials
-- =============================================
CREATE PROCEDURE [dbo].[GetPQTestimonial] 
	
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	Select Top 1 CustomerName, Comments From ( Select Top 10 NEWID() as RandomVal, CustomerName, Comments From Con_Testimonial Where IsActive = 1 And CatId = 1 Order by Id Desc ) A Order by RandomVal 
END
