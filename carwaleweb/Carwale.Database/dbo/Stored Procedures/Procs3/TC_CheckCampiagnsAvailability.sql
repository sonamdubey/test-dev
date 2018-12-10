IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_CheckCampiagnsAvailability]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_CheckCampiagnsAvailability]
GO

	-- =============================================
-- Author:		Tejashree Patil
-- Create date: 12 Oct,2013
-- Description:	This Proc Returns all camaign list depend on input parameter.
-- Modified By Vivek Gupta on 24th oct,2013...Added Inner Joins with CityMap and ModelMap Tables and modified where conditions
-- =============================================
CREATE PROCEDURE [dbo].[TC_CheckCampiagnsAvailability]
	@CityId	   INT,
	@InquiryDate DATETIME,
	@ModelId   INT,
	--@AreaIds   INT
	@BranchId INT = NULL

AS
BEGIN
	
	SELECT	CS.TC_CampaignSchedulingId Value,CS.CampaignName Text, CS.LeadTarget, CS.Amount, IsSpecialUser
	FROM	TC_CampaignScheduling CS
	INNER JOIN TC_CampaignCityMapping CCM ON CS.TC_CampaignSchedulingId = CCM.TC_CampaignSchedulingId 
	INNER JOIN TC_CampaignModelMapping CMM ON CS.TC_CampaignSchedulingId = CMM.TC_CampaignSchedulingId
	WHERE	(CCM.CityId = -1 OR CCM.CityId=@CityId)-- Modified By Vivek Gupta on 24th oct,2013
			AND CONVERT(DATE,@InquiryDate) BETWEEN CONVERT(DATE,CS.CampaignFromDate) AND CONVERT(DATE,CS.CampaignToDate)
			AND	(CMM.ModelId=@ModelId)
			AND CS.IsActive=1
			AND CS.IsSpecialUser = 0-- Added By Vivek Gupta on 24th oct,2013
			AND (@BranchId IS NULL OR CS.BranchId = @BranchId)
	ORDER BY CampaignName
	--ORDER BY IsSpecialUser DESC
			--AND (@AreaIds IS NULL OR )

END

