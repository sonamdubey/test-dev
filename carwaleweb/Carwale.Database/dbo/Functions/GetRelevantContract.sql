IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[GetRelevantContract]') 
    AND xtype IN (N'FN', N'IF', N'TF')
)
    DROP FUNCTION [dbo].[GetRelevantContract]
GO

	-- =============================================
-- Created by:		Vicky Lund
-- Creation date:	10-feb-2015
-- Description:		Return relevant contract against a campaign in following order:
--					1. Most recent currently active contract
--					2. Most recent currently paused contract
--					3. Most recent Queued active contract
--					4. Most recent Queued paused contract
--					5. Most recent Completed active contract
--					6. Most recent Completed paused contract
-- SELECT dbo.[GetRelevantContract] (4440)
-- Modified: Vicky Lund, 01/04/2016, Used applicationId column of TC_ContractCampaignMapping
-- =============================================
CREATE FUNCTION [dbo].[GetRelevantContract] (@CampaignId INT)
RETURNS INT
AS
BEGIN
	DECLARE @ContractId INT = NULL

	--Currently running contract
	SELECT TOP 1 @ContractId = TCCM.ContractId
	FROM TC_ContractCampaignMapping TCCM WITH (NOLOCK)
	WHERE TCCM.CampaignId = @CampaignId
		AND TCCM.ApplicationID = 1
		AND TCCM.ContractStatus IN (1, 2)
		AND (
			(
				--Lead based
				TCCM.ContractBehaviour = 1
				AND TCCM.StartDate <= CONVERT(DATE, GETDATE())
				AND ISNULL(TCCM.TotalDelivered, 0) < ISNULL(TCCM.TotalGoal, 999999999)
				)
			OR (
				--Date based
				TCCM.ContractBehaviour = 2
				AND CONVERT(DATE, GETDATE()) BETWEEN CONVERT(DATE, TCCM.StartDate)
					AND CONVERT(DATE, TCCM.EndDate)
				)
			)
	ORDER BY TCCM.ContractStatus ASC
		,TCCM.StartDate ASC

	IF (@ContractId IS NULL)
	BEGIN
		--Queued contract, active or paused
		SELECT TOP 1 @ContractId = TCCM.ContractId
		FROM TC_ContractCampaignMapping TCCM WITH (NOLOCK)
		WHERE TCCM.CampaignId = @CampaignId
			AND TCCM.ApplicationID = 1
			AND TCCM.ContractStatus IN (1, 2)
			AND TCCM.StartDate > CONVERT(DATE, GETDATE())
		ORDER BY TCCM.ContractStatus ASC
			,TCCM.StartDate ASC
	END

	IF (@ContractId IS NULL)
	BEGIN
		--Completed contract
		SELECT TOP 1 @ContractId = TCCM.ContractId
		FROM TC_ContractCampaignMapping TCCM WITH (NOLOCK)
		WHERE TCCM.CampaignId = @CampaignId
			AND TCCM.ApplicationID = 1
			AND TCCM.ContractStatus IN (3, 4)
		ORDER BY TCCM.ContractStatus ASC
			,TCCM.EndDate DESC
	END

	IF (@ContractId IS NULL)
	BEGIN
		RETURN - 1
	END

	RETURN @ContractId
END
