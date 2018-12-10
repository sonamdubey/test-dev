IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[GetOwnerType]') 
    AND xtype IN (N'FN', N'IF', N'TF')
)
    DROP FUNCTION [dbo].[GetOwnerType]
GO

	
-- =============================================
-- Author:		Binumon George
-- Create date: 01-11-2011
-- Description:	return owner type of stock
-- =============================================
CREATE FUNCTION [dbo].[GetOwnerType]
(	
	-- Add the parameters for the function here
	@Owner VARCHAR(10)
)
RETURNS CHAR(1)
AS
BEGIN
	DECLARE @OwnerVal CHAR(1)	
	SET @OwnerVal = (CASE @Owner
		WHEN  'first' THEN 1
		WHEN  'second' THEN 2
		WHEN  'third' THEN 3
		WHEN  'fourth' THEN 4
		WHEN  'fifth' THEN 5
	ELSE 2 END)
	RETURN @OwnerVal
END

