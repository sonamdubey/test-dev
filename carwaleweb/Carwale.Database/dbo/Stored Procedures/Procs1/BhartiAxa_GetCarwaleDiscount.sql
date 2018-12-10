IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[BhartiAxa_GetCarwaleDiscount]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[BhartiAxa_GetCarwaleDiscount]
GO

	-- =============================================
-- Author:		<Author,,Ashish Verma>
-- Create date: <Create Date,5/06/2014,>
-- Description:	<Description,For carwale discount,>
-- =============================================
create PROCEDURE [dbo].[BhartiAxa_GetCarwaleDiscount]
	-- Add the parameters for the stored procedure here
	@CityId int,
	@ModelId int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	Select distinct Discount
	From Con_InsuranceDiscount
	where CityId=@CityId and ModelId=@ModelId
END
