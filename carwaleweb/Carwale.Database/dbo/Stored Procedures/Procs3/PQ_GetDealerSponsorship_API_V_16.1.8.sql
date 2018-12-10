IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[PQ_GetDealerSponsorship_API_V_16]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[PQ_GetDealerSponsorship_API_V_16]
GO

	

-- =============================================
-- Author:		Anchal Gupta
-- Create date: 25/09/2015
-- Description:	adding the toggle value for showing email
-- Modified By Chetan Thambad 14/10/2015 To Show All Cities For Perticular State If Available
-- Modified by Vicky Lund, 30-10-2015, Inserted Daily goal condition for Date based campaigns
-- Modified by Vinayak, 12-10-2015, added join with tc_contractmapping to show only active contracts.
-- exec [dbo].[PQ_GetDealerSponsorship_API_V_15.11.1] 511,1,1,1
-- Modified By: Shalini Nair on 22/12/2015 to retrieve masking number from MM_SellerMobileMasking
-- Modified By: Shalini Nair on 29/01/2016 to retrieve CarwaleTollFree Number based on Isdefaultnumber column
-- =============================================
CREATE PROCEDURE [dbo].[PQ_GetDealerSponsorship_API_V_16.1.8]
	-- Add the parameters for the stored procedure here
	 @ModelId INT
	,@CityId INT
	,@ZoneId INT
	,@PlatformId TINYINT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from   
	-- interfering with SELECT statements.   
	SET NOCOUNT ON;

	WITH dealerCampaigns_cte
	AS (
		SELECT ds.Id
			,ds.DealerName
			,Phone = CASE 
				WHEN DS.IsDefaultNumber != 0 THEN 
					CTOLL.TollFreeNumber
				ELSE 
					MM.MaskingNumber 
				END
			,ds.DealerEmailId
			,dl.MobileNo
			,dl.DealerLeadBusinessType
			,SDT.Template
			,SDT.TemplateName
			,ds.LeadPanel
			,ds.DealerId
			,ds.LinkText
			,ds.CampaignPriority
			,ds.ShowEmail
		FROM pq_dealersponsored ds WITH (NOLOCK)
		INNER JOIN pq_dealercitiesmodels PCM WITH (NOLOCK) ON PCM.CampaignId = ds.Id
		INNER JOIN dealers dl WITH (NOLOCK) ON dl.ID = ds.DealerId
		INNER JOIN TC_ContractCampaignMapping TCC WITH (NOLOCK) ON TCC.CampaignId=ds.Id--join for active contracts
		INNER JOIN pq_dealerad_template_platform_maping TPM WITH (NOLOCK) ON TPM.CampaignId = ds.Id
		INNER JOIN pq_sponsoreddealead_templates SDT WITH (NOLOCK) ON SDT.TemplateId = TPM.AssignedTemplateId
		LEFT JOIN MM_SellerMobileMasking MM WITH (NOLOCK) ON MM.LeadCampaignId = DS.Id
		LEFT JOIN CarwaleTollFreeNumber CTOLL WITH(NOLOCK) ON DS.IsDefaultNumber = CTOLL.Id
		WHERE PCM.MakeId = (
				SELECT CarMakeId
				FROM CarModels WITH (NOLOCK)
				WHERE Id = @ModelId
				)
			AND (
				PCM.ModelId = @ModelId
				OR PCM.ModelId = - 1
				)
			AND (
				(PCM.StateId = - 1) --Pan India
				OR (
					PCM.StateId = (
						SELECT StateId
						FROM Cities WITH (NOLOCK)
						WHERE Id = @CityId
						)
					AND (
						(
							PCM.CityId = @CityId
							AND (
								ISNULL(PCM.ZoneId, 0) = ISNULL(@ZoneId, 0)
								OR @CityId NOT IN (1, 10)
								) -- zone check only for mumbai and newDelhi
							)
						OR PCM.CityId = - 1
						)
					) --City wise or Pan State
				)
			AND ds.IsActive = 1
			AND TPM.PlatformId = @PlatformId --modified by ashish   
			AND (
				(
					ds.EndDate IS NULL
					AND ds.TotalCount < ds.TotalGoal
					AND ISNULL(ds.DailyCount, 0) < ISNULL(ds.DailyGoal, ds.TotalGoal)
					AND CONVERT(DATE,ds.StartDate) <= CONVERT(DATE, GETDATE())
					) --Lead Based
				OR (
					ds.EndDate IS NOT NULL
					AND CONVERT(DATE, GETDATE()) BETWEEN CONVERT(DATE, ds.StartDate)
						AND CONVERT(DATE, ds.EndDate)
					AND (
						--ISNULL(ds.DailyGoal, 0) = 0
						--OR (
						--ISNULL(ds.DailyGoal, 0) != 0
						--AND 
						ISNULL(ds.DailyCount, 0) < ISNULL(ds.DailyGoal, 999999999)
						--)
						)
					) --Time Based
				)
			AND  (MM.MaskingNumber IS NOT NULL OR DS.IsDefaultNumber != 0 )
		)
	SELECT cc.Id
		,cc.DealerName
		,cc.Phone
		,cc.DealerEmailId
		,cc.MobileNo
		,cc.DealerLeadBusinessType
		,cc.Template
		,cc.TemplateName
		,cc.LeadPanel
		,cc.DealerId
		,cc.LinkText
		,cc.CampaignPriority
		,cc.ShowEmail
	FROM dealerCampaigns_cte cc WITH (NOLOCK)
	WHERE campaignpriority = (
			SELECT Min(campaignpriority)
			FROM dealerCampaigns_cte WITH (NOLOCK)
			)
END


