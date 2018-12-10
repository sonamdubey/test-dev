IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[AbSure_GetCities]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[AbSure_GetCities]
GO

	-- =============================================
-- Author:		Ashwini Dhamankar
-- Create date: 16-12-2014
-- Description:	Fetch Cities for AbSure Inspector
-- Modified By : Vinay Prajapati, Added condition of  'OR D.IsInspection=1' WHERE D.IsWarranty = 1 
-- =============================================
CREATE PROCEDURE [dbo].[AbSure_GetCities] 
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT			DISTINCT C.Name Text, C.ID Value
	FROM			TC_DealerCities DC WITH (NOLOCK)
	INNER JOIN		Dealers D WITH (NOLOCK) ON D.id=DC.DealerId
	INNER JOIN		Cities C WITH (NOLOCK) ON DC.CityId=C.Id
	WHERE			(D.IsWarranty = 1 OR D.IsInspection=1)
	AND				DC.IsActive = 1
	AND				C.IsDeleted = 0

END
