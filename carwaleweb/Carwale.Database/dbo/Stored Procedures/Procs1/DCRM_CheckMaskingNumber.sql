IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[DCRM_CheckMaskingNumber]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[DCRM_CheckMaskingNumber]
GO

	-- =============================================
-- Author:		Komal Manjare
-- Create date: 15-July-2016
-- Description:	Check whether masking number is present
-- =============================================
CREATE PROCEDURE  [dbo].[DCRM_CheckMaskingNumber]
@MaskingNumber VARCHAR(20)

AS
BEGIN
SELECT MM_SellerMobileMaskingId 
FROM MM_SellerMobileMasking WITH(NOLOCK)
WHERE MaskingNumber=@MaskingNumber
END
