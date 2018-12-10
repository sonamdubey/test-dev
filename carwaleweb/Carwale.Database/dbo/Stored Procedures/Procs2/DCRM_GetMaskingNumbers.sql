IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[DCRM_GetMaskingNumbers]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[DCRM_GetMaskingNumbers]
GO

	-- =============================================
-- Author:		<Khushaboo Patil>
-- Create date: <17/03/2015>
-- Description:	<Get Masking number against cityid>
-- Modifier   :  Mihir Chheda[12-08-2016]
-- Description:  get list of masking number based on state id and service provider id 
-- =============================================
CREATE PROCEDURE [dbo].[DCRM_GetMaskingNumbers]
	@CityId		Int= NULL,
	@StateId    Int= NULL,
	@ServiceProviders INT = NULL

AS	
BEGIN
	SELECT ID,MaskingNumber
	FROM MM_AvailableNumbers(NOLOCK) 
	WHERE CityId = ISNULL(@CityId,CityId) AND StateId=ISNULL(@StateId,StateId) AND ServiceProvider = ISNULL(@ServiceProviders,ServiceProvider) -- Mihir Chheda[12-08-2016]
	ORDER BY ID DESC
END


