IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_GetRSAPackages]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_GetRSAPackages]
GO

	-- =============================================
-- Author: Tejashree Patil		
-- Create date: 24-Sep-2014
-- Description:	Get Available RSA Packages
-- Modified By: Ashwini Dhamankar on Nov 6,2014. Fetched TC_AvaliableRSAPackagesId instead of PackageId
-- =============================================
CREATE PROCEDURE [dbo].[TC_GetRSAPackages]
	@BranchId INT
AS
BEGIN
		
	SELECT	RSA.Id, P.Name + ' (' + CONVERT(VARCHAR,RSA.AvailableQuantity) + ')' AS Name, RSA.AvailableQuantity Quantity    --Modified By: Ashwini Dhamankar on Nov 6,2014. Fetched TC_AvaliableRSAPackagesId instead of PackageId
	FROM	TC_AvailableRSAPackages RSA WITH(NOLOCK) 
			INNER JOIN Packages P WITH(NOLOCK) ON P.Id=RSA.PackageId
	WHERE	RSA.BranchId = @BranchId 
			AND RSA.AvailableQuantity > 0
END


