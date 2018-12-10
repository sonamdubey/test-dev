IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[UpdateTC_ContractStatus]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[UpdateTC_ContractStatus]
GO

	-- =============================================
-- Author:		Sunil M. Yadav
-- Create date: 08 feb 2016
-- Description:	change contractStaus for lead based contract and multi-outlet dealers.
-- =============================================
CREATE PROCEDURE UpdateTC_ContractStatus
@ContractId INT,
@CampaignId INT ,
@TotalGoal INT
AS
BEGIN

	DECLARE @TempTC_ContractCampaignMapping TABLE(ContractId INT, CampaignId INT, DealerId INT, TotalGoal INT, TotalDelivered INT)

	INSERT INTO @TempTC_ContractCampaignMapping(ContractId,CampaignId,DealerId,TotalGoal,TotalDelivered)
	SELECT ContractId,CampaignId,DealerId,TotalGoal,TotalDelivered FROM TC_ContractCampaignMapping WITH(NOLOCK)
	WHERE ContractId = @ContractId AND ContractStatus = 1 

	IF((SELECT SUM(TotalDelivered) FROM @TempTC_ContractCampaignMapping  WHERE ContractId = @ContractId ) >= @TotalGoal)
	BEGIN
		UPDATE TCC 
		SET TCC.ContractStatus = 3, -- 1-Active, 2-Paused, 3-Closed, 4- Aborted
			TCC.EndDate = GETDATE(),
			TCC.TotalGoal = TCC.TotalDelivered
		FROM TC_ContractCampaignMapping TCC WITH(NOLOCK)
			JOIN @TempTC_ContractCampaignMapping TTCC ON TTCC.CampaignId = TCC.CampaignId AND TTCC.ContractId = TCC.ContractId
	END

END
