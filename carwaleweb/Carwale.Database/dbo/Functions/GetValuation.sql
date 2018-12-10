IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[GetValuation]') 
    AND xtype IN (N'FN', N'IF', N'TF')
)
    DROP FUNCTION [dbo].[GetValuation]
GO

	-- =============================================
-- Author:		Satish Sharma
-- Create date: 18-may-09
-- Description:	get the value of a car based on some parameters
-- =============================================
CREATE FUNCTION [dbo].[GetValuation]
(
	-- Add the parameters for the function here
	@VersionId  INT, @CarYear INT, @CityId	INT		
)
RETURNS NUMERIC
AS
BEGIN
	-- Declare the return variable here
	DECLARE @RetVal NUMERIC

	-- Add the T-SQL statements to compute the return value here
	SELECT @RetVal = CarValue FROM CarValues 
	WHERE GuideId = @CityId AND CarVersionId = @VersionId AND CarYear = @CarYear

	IF @@RowCount = 0 SET @RetVal = 0

	-- Return the result of the function
	RETURN @RetVal
END
