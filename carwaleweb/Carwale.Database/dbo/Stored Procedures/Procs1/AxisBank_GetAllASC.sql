IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[AxisBank_GetAllASC]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[AxisBank_GetAllASC]
GO

	

-- =============================================
-- Author:		Akansha
-- Create date: 21.12.2013
-- Description:	Get all the ASC
-- exec AxisBank_GetAllASC 11,20,0
-- =============================================
CREATE PROCEDURE [dbo].[AxisBank_GetAllASC] @StartIndex INT = 1
	,@EndIndex INT = 10
AS
BEGIN
	WITH CTE
	AS (
		SELECT *
			,ROW_NUMBER() OVER (
				ORDER BY id DESC
				) AS RowNum
		FROM AxisBank_ASC
		)
	SELECT *
	FROM CTE
	WHERE RowNum >= @StartIndex
		AND RowNum <= @EndIndex

	SELECT Count(*)
	FROM AxisBank_ASC

END


