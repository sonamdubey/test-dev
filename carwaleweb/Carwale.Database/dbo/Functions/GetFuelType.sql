IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[GetFuelType]') 
    AND xtype IN (N'FN', N'IF', N'TF')
)
    DROP FUNCTION [dbo].[GetFuelType]
GO

	---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
-- =============================================
-- Author:		Umesh Ojha
-- Create date: 10-April-2012
-- Description:	Return fuel type of model
-- =============================================
CREATE FUNCTION [dbo].[GetFuelType]
(	
	-- Add the parameters for the function here
	@FuelValue INT
)
RETURNS Varchar(10)
AS
BEGIN
	DECLARE @FuelText Varchar(10)	
	SET @FuelText = (CASE @FuelValue
		WHEN  1 THEN 'Petrol'
		WHEN  2 THEN 'Diesel'		
		WHEN  3 THEN 'CNG'
		WHEN  4 THEN 'LPG'
		WHEN  5 THEN 'Electric'
	ELSE '' END)
	RETURN @FuelText
END

