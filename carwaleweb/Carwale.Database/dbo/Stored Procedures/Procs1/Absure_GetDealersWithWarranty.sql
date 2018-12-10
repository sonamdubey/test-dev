IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[Absure_GetDealersWithWarranty]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[Absure_GetDealersWithWarranty]
GO

	-- =============================================
-- Author:		Chetan Navin
-- Create date: 3rd Feb 2015
-- Description:	To fetch dealers with warranty
-- Modified By : Vinay Prajapati, Added condition of  'OR D.IsInspection=1' WHERE D.IsWarranty = 1
-- =============================================
CREATE PROCEDURE [dbo].[Absure_GetDealersWithWarranty] 
	
AS
BEGIN
    -- Insert statements for procedure here
	SELECT DISTINCT C.Id,C.Name FROM Cities C WITH(NOLOCK)
	INNER JOIN Dealers D WITH(NOLOCK) ON D.CityId = C.Id AND (D.IsWarranty = 1 OR D.IsInspection=1)
	WHERE C.IsDeleted = 0 
	ORDER BY C.Name

END
