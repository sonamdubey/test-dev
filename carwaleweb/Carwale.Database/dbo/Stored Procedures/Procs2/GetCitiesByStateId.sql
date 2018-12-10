IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[GetCitiesByStateId]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[GetCitiesByStateId]
GO

	-- =============================================
-- Modified By   : Suresh Prajapati
-- Modified date : 15th May, 2015
-- Description   : To Get Cities independent of state id
-- EXEC GetCitiesByStateId null
-- =============================================
CREATE PROCEDURE [dbo].[GetCitiesByStateId] @StateId INT = NULL
AS
BEGIN
	SET NOCOUNT ON;

	SELECT ID AS Value
		,NAME AS TEXT
	FROM Cities WITH (NOLOCK)
	WHERE IsDeleted = 0
		AND (
			(@StateId IS NULL)
			OR StateId = @StateId
			)
	ORDER BY NAME
END

