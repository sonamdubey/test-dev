IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[RoundUp]') 
    AND xtype IN (N'FN', N'IF', N'TF')
)
    DROP FUNCTION [dbo].[RoundUp]
GO

	CREATE FUNCTION [dbo].[RoundUp] (@value float, @places int) RETURNS float
AS
BEGIN
    RETURN CEILING(@value * POWER(10, @places)) / POWER(10, @places)
END