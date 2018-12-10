IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[IsCampaignActiveBasic]') 
    AND xtype IN (N'FN', N'IF', N'TF')
)
    DROP FUNCTION [dbo].[IsCampaignActiveBasic]
GO

	-- =============================================
-- Created by:		Vicky Lund
-- Creation date:	18-November-2015
-- Description:		
-- SELECT dbo.IsCampaignActive (3907)
-- Modified: Vicky Lund, 01/04/2016, Used applicationId column of TC_ContractCampaignMapping
-- =============================================
CREATE FUNCTION [dbo].[IsCampaignActiveBasic] (@CampaignId INT)
RETURNS BIT
AS
BEGIN
	DECLARE @IsActive BIT = 0

	SELECT @IsActive = ISNULL(CASE 
				WHEN (
						(
							(
								PDS.EndDate IS NOT NULL
								AND CONVERT(DATE, GETDATE()) BETWEEN CONVERT(DATE, PDS.StartDate)
									AND CONVERT(DATE, PDS.EndDate)
								AND (ISNULL(PDS.DailyCount, 0) < ISNULL(PDS.DailyGoal, 999999999))
								)
							OR (
								PDS.EndDate IS NULL
								AND PDS.StartDate <= CONVERT(DATE, GETDATE())
								AND ISNULL(PDS.TotalCount, 0) < PDS.TotalGoal
								AND ISNULL(PDS.DailyCount, 0) < ISNULL(PDS.DailyGoal, PDS.TotalGoal)
								)
							)
						)
					THEN 1
				ELSE 0
				END, 0)
	FROM PQ_DealerSponsored PDS WITH (NOLOCK)
	INNER JOIN TC_ContractCampaignMapping CCM WITH (NOLOCK) ON PDS.Id = CCM.CampaignId
		AND PDS.Id = @CampaignId
		AND CCM.ApplicationId = 1
		AND PDS.IsActive = 1
	INNER JOIN Dealers D WITH (NOLOCK) ON D.ID = PDS.DealerId
		AND D.IsTCDealer = 1

	RETURN @IsActive
END
