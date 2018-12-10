IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[GetCarFuelTypes]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[GetCarFuelTypes]
GO

	

-- =============================================
-- Author:		Shalini Nair	
-- Create date: 17/06/2016
-- Description:	To fetch all car fuel types
-- =============================================
CREATE PROCEDURE [dbo].[GetCarFuelTypes]
	-- Add the parameters for the stored procedure here
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	-- Insert statements for procedure here
	SELECT CFT.FuelTypeId AS Value
		,CFT.FuelType AS Text
	FROM CarFuelType AS CFT WITH (NOLOCK)
	ORDER BY Value
END

