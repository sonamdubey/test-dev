IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[GetNCSDealerMakeModels]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[GetNCSDealerMakeModels]
GO

	
-- =============================================
-- Author:		Chetan Thambad
-- Create date: 24-08-2016
-- EXEC GetNCSDealerMakeModels 7013
-- =============================================
CREATE PROCEDURE [dbo].[GetNCSDealerMakeModels] @DealerId INT
AS
BEGIN
	SELECT DMK.MakeId
		,DMO.ModelId
	FROM NCS_DealerMakes DMK WITH (NOLOCK)
	LEFT OUTER JOIN TC_NoDealerModels DMO WITH (NOLOCK) ON DMK.DealerId = DMO.DealerId AND DMO.[Source] = 2
	WHERE DMK.DealerId = @DealerId
END

