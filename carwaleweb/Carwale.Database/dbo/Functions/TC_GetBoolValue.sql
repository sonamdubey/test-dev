IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_GetBoolValue]') 
    AND xtype IN (N'FN', N'IF', N'TF')
)
    DROP FUNCTION [dbo].[TC_GetBoolValue]
GO

	
-- =============================================
-- Author:		Binumon George
-- Create date: 31-03-2012
-- Description:	return passed yes or no-- For isAccdental and is flood affected
-- =============================================
CREATE FUNCTION [dbo].[TC_GetBoolValue]
(	
	-- Add the parameters for the function here
	@Val INT
)
RETURNS VARCHAR(5)
AS
BEGIN
	DECLARE @RetVal VARCHAR(5)	
	SET @RetVal = (CASE @Val
		WHEN 0 THEN 'No'
		WHEN 1 THEN 'Yes'
	ELSE 'No' END)
	RETURN @RetVal
END
