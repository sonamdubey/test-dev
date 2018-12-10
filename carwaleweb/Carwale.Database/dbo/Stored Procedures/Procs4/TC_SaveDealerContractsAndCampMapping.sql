IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_SaveDealerContractsAndCampMapping]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_SaveDealerContractsAndCampMapping]
GO

	



-- =============================================
-- Author:		<Khushaboo Patil>
-- Create date: <6th Oct 15>
-- Description:	<Save Dealer Contracts And Campaign Mapping data>
-- Updated By : Vinay Kumar Prajapati 26 Nov  2015  Assign @CostPerLead = null 
-- Updated by : Kritika Choudhary on 8th Dec 2015, in else case insert data to TC_ContractCampaignMappingLog and update end date and contractstatus 
-- Updated by : Kritika Choudhary on 9th Dec 2015, added campaignId parameter
-- Modified By : Sunil Yadav On 18 feb 2016 
-- Description : set auto pause date for contract.
-- Modified By : Sunil Yadav On 8th april 2016 
-- Description : save applicationId of dealer for bikewale contracts.
-- =============================================
CREATE PROCEDURE [dbo].[TC_SaveDealerContractsAndCampMapping]
	@ContractId	INT,
	@DealerId	INT,
	@StartDate	DATETIME,
	@EndDate	DATETIME,
	@ContractBehaviour	SMALLINT,
	@TotalLeadCnt	INT = NULL,
	@CostPerLead	INT= NULL,
	@ContractType	SMALLINT,
	@ReplacementContractId	INT,
	@UserId	INT,
	@ContractCampaignMappingId INT = NULL,
	@RetId		INT	OUTPUT,
	@CampaignId INT	= NULL OUTPUT
AS
BEGIN
	-- sunil yadav on 8th feb 2016, to save applicationId 
	DECLARE @ApplicationId INT 
	SELECT @ApplicationId = ApplicationId 
	FROM Dealers WITH(NOLOCK)
	WHERE ID = @DealerId

	IF @ContractCampaignMappingId IS NULL
		BEGIN
			INSERT INTO TC_ContractCampaignMapping(ContractId,DealerId,StartDate,EndDate,ContractBehaviour,TotalGoal,CostPerLead,ContractType,ReplacementContractId,ContractStatus,ActionTakenBy,ApplicationID)
			VALUES(@ContractId,@DealerId,@StartDate,@EndDate,@ContractBehaviour,@TotalLeadCnt,@CostPerLead,@ContractType,@ReplacementContractId,1,@UserId,@ApplicationId)
			SET @RetId = SCOPE_IDENTITY()
			
			IF(@ContractType <>2)
			BEGIN
			EXEC DCRM_SetAutoPauseDate @StartDate, @ContractId
			END

		END
	ELSE IF @ContractCampaignMappingId IS NOT NULL
		BEGIN
			--Log Data
			INSERT INTO TC_ContractCampaignMappingLog(ContractId,CampaignId,DealerId,StartDate,EndDate,TotalGoal,TotalDelivered,ContractStatus,ContractBehaviour,CostPerLead,ContractType,ReplacementContractId,ActionTakenBy)
			SELECT ContractId,CampaignId,DealerId,StartDate,EndDate,TotalGoal,TotalDelivered,ContractStatus,ContractBehaviour,CostPerLead,ContractType,ReplacementContractId,ActionTakenBy
			FROM TC_ContractCampaignMapping WITH(NOLOCK)
			WHERE Id = @ContractCampaignMappingId

				
			SET @CampaignId = (SELECT CampaignId
			FROM TC_ContractCampaignMapping WITH(NOLOCK)
			WHERE Id = @ContractCampaignMappingId)
	
			UPDATE TC_ContractCampaignMapping 
			SET StartDate     = ISNULL(@StartDate,StartDate),
			EndDate           = CASE WHEN ContractStatus = 3 AND @TotalLeadCnt > TotalGoal THEN NULL ELSE ISNULL(@EndDate,EndDate) END,
			ContractBehaviour = ISNULL(@ContractBehaviour,ContractBehaviour),
			TotalGoal         = ISNULL(@TotalLeadCnt,TotalGoal),
			CostPerLead       = ISNULL(@CostPerLead,CostPerLead),
			ContractType      = ISNULL(@ContractType,ContractType),
			ReplacementContractId = ISNULL(@ReplacementContractId,ReplacementContractId),
			ActionTakenBy     = ISNULL(@UserId,ActionTakenBy),
			ContractStatus    = CASE WHEN ContractStatus = 3 AND @TotalLeadCnt > TotalGoal THEN 1 ELSE ContractStatus END
			WHERE Id = @ContractCampaignMappingId
			SET @RetId = @ContractCampaignMappingId

			IF(@ContractType <>2 AND DATEDIFF(DAY,@StartDate,GETDATE()) >= 0)
			BEGIN 
			EXEC DCRM_SetAutoPauseDate @StartDate, @ContractId
			END
		END
END

