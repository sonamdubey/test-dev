IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_GetPQComponents]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_GetPQComponents]
GO

	
-- ===================================================
-- Auhtor		: Suresh Prajapati
-- Created On	: 04th Feb, 2016
-- Description	: To Get Active PQ Components
--EXEC TC_GetPQComponents
--Modified By : Nilima More On 17th May 2016.
--Added IsManditory = 0 to fetch only PqComponents,IsManditory =1 (Price breakup components).
-- ===================================================
CREATE PROCEDURE [dbo].[TC_GetPQComponents]
AS
BEGIN
	SET NOCOUNT ON;

	SELECT TC_PQComponentsId
		,NAME,Action
	FROM TC_PQComponents WITH (NOLOCK)
	WHERE IsActive = 1 AND IsManditory = 0 --Added IsManditory = 0 to fetch only PqComponents,IsManditory =1 (Price breakup components).
	ORDER BY Name
END

--------------------------------------------------------------------------------------------------------------------
