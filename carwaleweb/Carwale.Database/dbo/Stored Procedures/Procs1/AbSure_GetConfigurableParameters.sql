IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[AbSure_GetConfigurableParameters]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[AbSure_GetConfigurableParameters]
GO

	-- ===========================================================
-- Author:		Ruchira Patil
-- Create date: 2nd June, 2015
-- Description:	To Save Configurable parameters for absure

-- Modified By : Suresh Prajapati on 12th June, 2015
-- Description : Added Sequence parameter in select
-- ==========================================================
CREATE PROCEDURE [dbo].[AbSure_GetConfigurableParameters] @Id INT
AS
BEGIN
	SELECT Category
		,Parameter
		,MinValue
		,MaxValue
		,ConstantValue
		,Sequence
		,IsActive
	FROM AbSure_ConfigurableParameters
	WHERE ID = @Id
END
