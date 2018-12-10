IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[Microsite_TestimonialsSelect]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[Microsite_TestimonialsSelect]
GO

	-- =============================================
-- Author:		Surendra
-- Create date: 10th Aug,2011
-- Description:	view Content for aboutus
-- =============================================
CREATE PROCEDURE [dbo].[Microsite_TestimonialsSelect]
(
	@DealerId INT,
	@Id INT
)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	
	SELECT Testimonials, CustomerName FROM Microsite_Testimonials 
	WHERE DealerId=@DealerId AND IsActive=1 AND Id=@Id
END
