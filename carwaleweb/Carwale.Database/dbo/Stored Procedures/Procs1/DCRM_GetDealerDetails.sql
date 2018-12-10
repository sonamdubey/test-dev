IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[DCRM_GetDealerDetails]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[DCRM_GetDealerDetails]
GO
	
-- =============================================
-- Author:		ASHWINI TODKAR
-- Create date: 14 July 2015
-- Description:	PROC TO GET DEALER DETAILS
-- =============================================
CREATE PROCEDURE [dbo].[DCRM_GetDealerDetails]
	-- Add the parameters for the stored procedure here
	@MobileNumber VARCHAR(20)
AS
BEGIN
	SELECT MM.MaskingNumber
		,MM.Mobile
		,D.Organization
	FROM MM_SellerMobileMasking MM WITH(NOLOCK)
	INNER JOIN Dealers D WITH(NOLOCK) ON D.ID = MM.ConsumerId
	WHERE MM.Mobile = @MobileNumber
END
