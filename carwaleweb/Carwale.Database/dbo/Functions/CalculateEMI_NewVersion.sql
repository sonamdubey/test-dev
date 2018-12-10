IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[CalculateEMI_NewVersion]') 
    AND xtype IN (N'FN', N'IF', N'TF')
)
    DROP FUNCTION [dbo].[CalculateEMI_NewVersion]
GO

	-- =============================================
-- Author:		<Rakesh Yadav>
-- Create date: <25 July 2016>
-- Description:	<Calculates EMI for new car versions, Emi will be calculated on 85% of actual price,Tenure is in months>
-- =============================================
CREATE FUNCTION [dbo].[CalculateEMI_NewVersion] 
(
	-- Add the parameters for the function here
	@PrincipleAmount INT,
	@Tenure INT = 60,
	@RateOfInterest FLOAT = 10.5
)
RETURNS FLOAT
AS
BEGIN
	-- Declare the return variable here
	DECLARE @EMI INT
    
	SET @EMI = ROUND(((@PrincipleAmount)*(@RateOfInterest/1200)*((POWER((1+(@RateOfInterest/1200)),@Tenure)/(POWER((1+(@RateOfInterest/1200)),@Tenure)-1)))),0)
	-- Return the result of the function
	RETURN @EMI
END

