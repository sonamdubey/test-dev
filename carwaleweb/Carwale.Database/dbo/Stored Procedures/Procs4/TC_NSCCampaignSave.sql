IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_NSCCampaignSave]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_NSCCampaignSave]
GO

	-- =============================================
-- Author:		Vivek Gupta
-- Create date: 25-10-2013
-- Description:	Save NSC Campaign Automatically in while adding new car inquiries
-- =============================================
CREATE PROCEDURE [dbo].[TC_NSCCampaignSave]
	@CityId	   INT,
	@InquiryDate DATETIME,
	@VersionId   INT,
	@InqId     INT
AS
BEGIN

    DECLARE @TC_CampaignSchedulingId INT = NULL
	DECLARE @ModelId INT

	SELECT @ModelId = CarModelId 
	FROM CarVersions
	WHERE ID=@VersionId

	SET @TC_CampaignSchedulingId = (SELECT Top 1 CS.TC_CampaignSchedulingId
									FROM	TC_CampaignScheduling CS
									INNER JOIN TC_CampaignCityMapping CCM ON CS.TC_CampaignSchedulingId = CCM.TC_CampaignSchedulingId
									INNER JOIN TC_CampaignModelMapping CMM ON CS.TC_CampaignSchedulingId = CMM.TC_CampaignSchedulingId
									WHERE	(CCM.CityId = -1 OR CCM.CityId=@CityId)
											AND CONVERT(DATE,@InquiryDate) BETWEEN CONVERT(DATE,CS.CampaignFromDate) AND CONVERT(DATE,CS.CampaignToDate)
											AND	(CMM.ModelId=@ModelId)
											AND CS.IsActive=1
											AND CS.IsSpecialUser = 1
									ORDER BY CS.TC_CampaignSchedulingId DESC
									)

   IF @TC_CampaignSchedulingId IS NOT NULL
   BEGIN
      UPDATE TC_NewCarInquiries
	  SET NSCCampaignSchedulingId = @TC_CampaignSchedulingId 
	  WHERE TC_NewCarInquiriesId = @InqId
   END

END

