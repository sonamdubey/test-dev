IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[DCRM_GetLpaImages]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[DCRM_GetLpaImages]
GO

	-- =============================================
-- Author:		Amit Yadav
-- Create date: 11-04-2016
-- Description:	To get the LPA images for a SalesDealerId
-- Modified by:komal Manjare(24-05-2016) handle null condition for isactive
-- =============================================
CREATE PROCEDURE [dbo].[DCRM_GetLpaImages]
	-- Add the parameters for the stored procedure here
	@SalesDealerId INT = NULL,
	@LPAImageId INT = NULL
AS
BEGIN
    
	SELECT LPA.Id AS AttachedLpaDetailsId,
		   LPA.SalesDealerId,
		   LPA.HostURL,
		   LPA.OriginalImgPath,
		   LPA.IsActive,
		   LPA.AttachedFileName AS AttachedLpaName
	FROM M_AttachedLpaDetails AS LPA WITH(NOLOCK)
	WHERE LPA.SalesDealerId = @SalesDealerId AND LPA.Id=ISNULL(@LPAImageId,LPA.Id) AND ISNULL(LPA.IsActive,1)=1 -- komal Manjare(24-05-2016) handle null condition for isactive

END