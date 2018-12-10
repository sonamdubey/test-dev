IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[GetIsActive]') 
    AND xtype IN (N'FN', N'IF', N'TF')
)
    DROP FUNCTION [dbo].[GetIsActive]
GO

	-- =============================================
-- Author:		Avishkar
-- Create date: 12/7/2012
-- Description:	Reflect IsActive Column from status column
-- =============================================
CREATE FUNCTION GetIsActive
(
	@Status bit
)
RETURNS bit
AS
BEGIN
	-- Declare the return variable here
	DECLARE @IsActive bit

	-- Add the T-SQL statements to compute the return value here
	SELECT @IsActive= case @Status when 0 then 1 when 1 then 0 end 

	-- Return the result of the function
	RETURN @IsActive

END
