IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[CalculateEMI]') 
    AND xtype IN (N'FN', N'IF', N'TF')
)
    DROP FUNCTION [dbo].[CalculateEMI]
GO

	-- =============================================
-- Author:		<Reshma Shetty>
-- Create date: <28/08/2012>
-- Description:	<Calculates EMI>
-- select dbo.CalculateEMI(100000,0,16.5)
-- =============================================
CREATE FUNCTION [dbo].[CalculateEMI] 
(
	-- Add the parameters for the function here
	@PrincipleAmount INT,
	@Tenure INT,
	@RateOfInterest FLOAT
)
RETURNS FLOAT
AS
BEGIN
	-- Declare the return variable here
	DECLARE @EMI INT, @N SMALLINT
    IF (@Tenure<6)
    SET @N = 5
    ELSE
    SET @N = 3
	-- Add the T-SQL statements to compute the return value here
	DECLARE @MonthlyROI FLOAT ,
			@Temp FLOAT 
    SET @MonthlyROI = (@RateOfInterest / 100.0) / 12 
	SET @Temp = POWER((1 + @MonthlyROI ),@N*12)

	
	SET @EMI = 0.9*@PrincipleAmount * @MonthlyROI * @Temp / (@Temp - 1)
	-- Return the result of the function
	RETURN @EMI
END
