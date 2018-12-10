IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[Absure_GetRegistrationNumber]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[Absure_GetRegistrationNumber]
GO

	-- =============================================
-- Author		: Yuga Hatolkar
-- Created On	: 1st June, 2015
-- Description	: Get Registration Number based on CarDetails Id.
-- EXEC Absure_GetRegistrationNumber 4
-- =============================================
CREATE PROCEDURE [dbo].[Absure_GetRegistrationNumber]

	@AbSure_CarDetailsId INT = NULL
	
AS
BEGIN
	SET NOCOUNT OFF;
    
		SELECT RegNumber FROM AbSure_CarDetails WITH(NOLOCK) WHERE Id = @AbSure_CarDetailsId

END



