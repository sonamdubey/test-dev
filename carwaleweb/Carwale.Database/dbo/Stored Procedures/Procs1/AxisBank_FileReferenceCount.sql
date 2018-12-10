IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[AxisBank_FileReferenceCount]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[AxisBank_FileReferenceCount]
GO

	
-- =============================================
-- Author:		Kumar Vikram
-- Create date: 11.12.2013
-- Description:	Gets the count of the file reference no. a user has used
-- exec AxisBank_FileReferenceCount 12556567, 1
-- =============================================
CREATE PROCEDURE [dbo].[AxisBank_FileReferenceCount] @FileReferenceNumber VARCHAR(20)
	,@CustomerId NUMERIC
	,@Count INT = 0 OUTPUT
AS
BEGIN
	SET NOCOUNT ON;

	SELECT @Count = COUNT(id)
	FROM AxisBank_CarValuations
	WHERE FileReferenceNumber = @FileReferenceNumber
		AND CustomerId = @CustomerId

	SELECT @Count
END
