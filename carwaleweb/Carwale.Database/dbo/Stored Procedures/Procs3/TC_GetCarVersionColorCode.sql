IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_GetCarVersionColorCode]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_GetCarVersionColorCode]
GO
	-- =============================================
-- Author:		Tejashree Patil
-- Create date: 27 Sept,2013
-- Description:	Get all color codes for version code.
-- [TC_GetCarVersionColorCode] 1028,'WVWE1000000000005'
-- Modified By : Tejashree Patil on 11 Nov 2013: Added condition SI.ColourCode=V.ColorCode in LEFT JOIN with TC_StockInventory.
-- Modified By : Tejashree Patil on 18 Dec 2013: Commented condition of branchId.
-- =============================================
CREATE PROCEDURE [dbo].[TC_GetCarVersionColorCode]
@BranchId BIGINT,
@ChassisNumber VARCHAR(17)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
				
	--Get Version / model code and Get Color code
	SELECT	V.VersionColorsId,V.ColorCode,V.CarVersionID,
			V.Color,V.HexCode,V.ID,V.CarVersionCode,
			SI.ModelYear
	FROM	TC_vwVersionColorCode V
			LEFT JOIN TC_StockInventory SI WITH(NOLOCK)
						ON SI.ColourCode=V.ColorCode AND SI.ModelCode = V.CarVersionCode
	WHERE	V.IsActive=1
			--AND (@BranchId IS NULL OR SI.BranchId = @BranchId)-- Modified By : Tejashree Patil on 18 Dec 2013
			AND (@ChassisNumber IS NULL OR SI.ChassisNumber = @ChassisNumber)
	ORDER BY V.CarVersionID DESC

END


