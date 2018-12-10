IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[InitCap]') 
    AND xtype IN (N'FN', N'IF', N'TF')
)
    DROP FUNCTION [dbo].[InitCap]
GO

	CREATE FUNCTION [dbo].[InitCap] (
 @string varchar(255)
)  
RETURNS varchar(255) AS


BEGIN 

 RETURN upper(left(@string, 1)) + right(@string, len(@string) - 1) 

END