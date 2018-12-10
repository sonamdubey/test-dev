IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_GetOwnerValue]') 
    AND xtype IN (N'FN', N'IF', N'TF')
)
    DROP FUNCTION [dbo].[TC_GetOwnerValue]
GO

	
-- =============================================
-- Author:		Binumon George
-- Create date: 30-12-2011
-- Description:	return owner type of stock
-- =============================================
CREATE FUNCTION [dbo].[TC_GetOwnerValue]
(	
	-- Add the parameters for the function here
	@Owner INT
)
RETURNS VARCHAR(10)
AS
BEGIN
	DECLARE @OwnerVal VARCHAR(10)	
	SET @OwnerVal = (CASE @Owner
		WHEN 1 THEN 'First'
		WHEN 2 THEN 'Second'
		WHEN 3 THEN 'Third'
		WHEN 4  THEN 'Fourth'
		WHEN 5  THEN 'Fifth'
	ELSE 'Fifth' END)
	RETURN @OwnerVal
END
