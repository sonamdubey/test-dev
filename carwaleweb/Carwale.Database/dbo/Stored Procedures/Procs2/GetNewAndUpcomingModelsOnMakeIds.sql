IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[GetNewAndUpcomingModelsOnMakeIds]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[GetNewAndUpcomingModelsOnMakeIds]
GO

	-- =============================================
-- Author:		Vicky Lund
-- Create date: 12/08/2016
-- Description:	Returns new and upcoming models for multiple makeIds(comma-separated makeIds)
-- =============================================
CREATE PROCEDURE [dbo].[GetNewAndUpcomingModelsOnMakeIds]
	@MakeIds VARCHAR(150)
AS
BEGIN
	SELECT ID AS Value
		,NAME AS [Text]
	FROM carmodels WITH (NOLOCK)
	WHERE (
			CarMakeId IN (
				SELECT listmember
				FROM fnSplitCSV(@MakeIds)
				)
			OR @MakeIds = '-1'
			)
		AND (New = 1 OR Futuristic = 1)
		AND IsDeleted = 0
	ORDER BY NAME
END

