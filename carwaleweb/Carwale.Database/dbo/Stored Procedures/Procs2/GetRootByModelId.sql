IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[GetRootByModelId]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[GetRootByModelId]
GO

	
-- =============================================
-- Author:		Akansha
-- Create date: 2.6.2014
-- Description:	Get RootId based on ModelId
-- =============================================
CREATE PROCEDURE [dbo].[GetRootByModelId] @ModelId INT
AS
BEGIN
	SELECT CMR.RootId
		,CMR.RootName
	FROM CarModels CM with(nolock)
	INNER JOIN CarModelRoots CMR with(nolock) ON CM.RootId = CMR.RootId
	WHERE id = @ModelId
END
